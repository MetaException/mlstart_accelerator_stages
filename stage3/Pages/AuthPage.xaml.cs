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

        private void LoginBtn_Clicked(object sender, EventArgs e)
        {
            LoginBtn.IsEnabled = false;
            if (LoginEntry.Text != "" && PasswordEntry.Text != "")
                _dbUtils.AuthorizeUser(LoginEntry.Text, PasswordEntry.Text);
            LoginBtn.IsEnabled = true;
        }

        private void RegBtn_Clicked(object sender, EventArgs e)
        {
            RegBtn.IsEnabled = false;
            if (LoginEntry.Text != "" && PasswordEntry.Text != "")
                _dbUtils.RegisterNewUser(LoginEntry.Text, PasswordEntry.Text);
            RegBtn.IsEnabled = true;
        }
    }

}
