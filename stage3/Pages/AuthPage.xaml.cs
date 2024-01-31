using stage3.Utils;

namespace stage3.Pages
{
    public partial class AuthPage : ContentPage
    {
        private readonly DbUtils _dbUtils = DependencyService.Get<DbUtils>();

        public AuthPage()
        {
            InitializeComponent();
        }

        private async void LoginBtn_Clicked(object sender, EventArgs e)
        {
            LoginBtn.IsEnabled = false;

            try
            {
                bool isAuthorized = await _dbUtils.AuthorizeUser(LoginEntry.Text, PasswordEntry.Text);
                if (isAuthorized)
                {
                    ErrorLabel.TextColor = Colors.Black;
                    ErrorLabel.Text = "Вы успешно авторизовались";
                }
                else
                {
                    ErrorLabel.TextColor = Colors.Red;
                    ErrorLabel.Text = "Неверный логин или пароль";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.TextColor = Colors.Red;
                ErrorLabel.Text = ex.Message;
            }
            LoginBtn.IsEnabled = true;
        }

        private async void RegBtn_Clicked(object sender, EventArgs e)
        {
            RegBtn.IsEnabled = false;

            try
            {
                bool isRegistered = await _dbUtils.RegisterNewUser(LoginEntry.Text, PasswordEntry.Text);
                if (isRegistered)
                {
                    ErrorLabel.TextColor = Colors.Black;
                    ErrorLabel.Text = "Вы успешно зарегистрировались";
                }
            }
            catch (Exception ex)
            {
                ErrorLabel.TextColor = Colors.Red;
                ErrorLabel.Text = ex.Message;
            }
            RegBtn.IsEnabled = true;
        }
    }

}
