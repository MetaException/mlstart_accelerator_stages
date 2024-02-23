using stage3.ViewModels;

namespace stage3.Pages
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
