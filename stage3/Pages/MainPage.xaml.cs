using second_stage;

namespace stage3.Pages
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();

            StartLogger();
            RunSimulator();
        }

        private async Task StartLogger()
        {
            Logger.CreateLogger();
            logsTable.ItemsSource = Logger.logEntries;
        }

        private async Task RunSimulator()
        {
            await Task.Run(() => Simulator.SimulateDay());
        }
    }

}
