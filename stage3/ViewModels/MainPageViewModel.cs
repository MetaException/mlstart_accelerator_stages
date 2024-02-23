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
        while (true)
        {
            await Task.Run(() => Simulator.SimulateDay());
            await Task.Delay(TimeSpan.FromSeconds(1)); // По заданию цикл должен идти бесконечно
        }
    }
}
