using stage3.Model;

namespace stage3.Utils
{
    public class DbUtils
    {
        //Разобраться с async
        private readonly MallenomContext _context;

        public DbUtils(MallenomContext context)
        {
            _context = context;
        }

        public async Task<bool> AuthorizeUser(string login, string password)
        {
            if (login is null || password is null)
                return false;

            var user = await FindUserByLogin(login);
            if (user is not null)
            {
                return await Task.Run(async () => BCrypt.Net.BCrypt.Verify(password, user.PassHash)); //Не поддерживает await ???
            }
            return false;
        }

        public async Task<User> FindUserByLogin(string login)
        {
            foreach (var user in _context.Users)
            {
                if (await Task.Run(async () => BCrypt.Net.BCrypt.Verify(login, user.LoginHash)))
                    return user;
            }
            return null;
        }

        public async Task<bool> CheckIfUserExists(string login)
        {
            foreach (var user in _context.Users)
            {
                bool isExists = await Task.Run(async () => BCrypt.Net.BCrypt.Verify(login, user.LoginHash));
                if (isExists)
                    return true;
            }
            return false;
        }

        public async Task<bool> RegisterNewUser(string login, string password) //потом вернуть на void
        {
            if (login is null || password is null)
                return false;

            if (!await CheckIfUserExists(login)) //убрать?
            {
                User toReg = new User
                {
                    PassHash = await HashPassword(password),
                    LoginHash = await HashPassword(login)
                };

                await _context.Users.AddAsync(toReg);
                await _context.SaveChangesAsync();

                return true;
            }
            return false;
        }

        public static async Task<string> HashPassword(string text)
        {
            string hashedPassword = await Task.Run(async () => BCrypt.Net.BCrypt.HashPassword(text));

            return hashedPassword;
        }
    }
}
