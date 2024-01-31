using stage3.Model;

namespace stage3.Utils
{
    public class DbUtils
    {
        private readonly MallenomContext _context;

        public DbUtils(MallenomContext context)
        {
            _context = context;
        }

        public async Task<bool> AuthorizeUser(string login, string password)
        {
            var user = await FindUserByLogin(login);
            if (user is not null)
            {
                return BCrypt.Net.BCrypt.Verify(password, user.PassHash);
            }
            return false;
        }

        public async Task<User> FindUserByLogin(string login)
        {
            foreach (var user in _context.Users)
            {
                if (BCrypt.Net.BCrypt.Verify(login, user.LoginHash))
                    return user;
            }
            return null;
        }

        public async Task<bool> CheckIfUserExists(string login)
        {
            foreach (var user in _context.Users)
            {
                if (BCrypt.Net.BCrypt.Verify(login, user.LoginHash))
                    return true;
            }
            return false;
        }

        public async void RegisterNewUser(string login, string password)
        {
            if (!await CheckIfUserExists(login)) //убрать?
            {
                User toReg = new User
                {
                    PassHash = HashPassword(password),
                    LoginHash = HashPassword(login)
                };

                await _context.Users.AddAsync(toReg);
                await _context.SaveChangesAsync();
            }
        }

        public static string HashPassword(string text)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(text);

            return hashedPassword;
        }
    }
}
