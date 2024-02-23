using CommunityToolkit.Mvvm.ComponentModel;
using second_stage;
using System.Collections.ObjectModel;

namespace stage3.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public MainPageViewModel()
    {
        _ = StartLogger();
        _ = RunSimulator();
    }

    [ObservableProperty]
    public ObservableCollection<Logger.LogRecord> _logsList;

    private async Task StartLogger()
    {
        Logger.CreateLogger();
        LogsList = Logger.logEntries;
    }

    private async Task RunSimulator()
    {
        await Task.Run(() => Simulator.SimulateDay());
    }
}
