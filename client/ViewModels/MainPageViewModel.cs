using client.Model;
using client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;

namespace client.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly NetUtils _netUtils;

    [ObservableProperty]
    private ObservableCollection<LogRecord> _logsList;

    public MainPageViewModel(NetUtils netUtils)
    {
        _netUtils = netUtils;

        _ = GetDataAsync();
    }

    private async Task GetDataAsync()
    {
        while (true)
        {
            LogsList = await _netUtils.GetDataAsync();
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}