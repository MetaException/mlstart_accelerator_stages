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

    [ObservableProperty]
    private bool _isInternetErrorVisible;

    public MainPageViewModel(NetUtils netUtils)
    {
        _netUtils = netUtils;

        _ = GetDataAsync();
    }

    private async Task GetDataAsync()
    {
        while (true)
        {
            var recievedData = await _netUtils.GetDataAsync();

            if (recievedData is not null)
            {
                IsInternetErrorVisible = false;
                LogsList = recievedData;
            }
            else
            {
                IsInternetErrorVisible = true;
            }
            await Task.Delay(TimeSpan.FromSeconds(1));
        }
    }
}