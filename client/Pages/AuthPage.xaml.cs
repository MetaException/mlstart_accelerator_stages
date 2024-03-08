using client.ViewModels;

namespace client.Pages
{
    public partial class AuthPage : ContentPage
    {
        public AuthPage(AuthPageViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }
}