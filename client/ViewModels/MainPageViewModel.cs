using client.Model;
using client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace client.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly NetUtils _netUtils;
    private CancellationTokenSource _cancellationTokenSource;

    [ObservableProperty]
    private ObservableCollection<LogRecord> _logsList;

    [ObservableProperty]
    private bool _isInternetErrorVisible;

    public RelayCommand NavigatedFromPageCommand { get; }

    public MainPageViewModel(NetUtils netUtils)
    {
        _netUtils = netUtils;

        _cancellationTokenSource = new CancellationTokenSource();

        NavigatedFromPageCommand = new RelayCommand(NavigatedFromPage);

        _ = GetDataAsync(_cancellationTokenSource.Token);
    }

    private async Task GetDataAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
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

    private void NavigatedFromPage()
    {
        _cancellationTokenSource.Cancel();
    }

}