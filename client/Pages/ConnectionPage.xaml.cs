using client.ViewModels;

namespace client.Pages
{
    public partial class ConnectionPage : ContentPage
    {
        public ConnectionPage(ConnectionPageViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }
    }

}
