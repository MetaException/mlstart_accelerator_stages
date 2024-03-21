using client.ViewModels;

namespace client.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel viewModel)
        {
            InitializeComponent();
            this.BindingContext = viewModel;
        }

        private void ContentPage_NavigatedFrom(object sender, NavigatedFromEventArgs e)
        {
            ((MainPageViewModel)BindingContext).NavigatedFromPageCommand.Execute(null);
        }
    }
}