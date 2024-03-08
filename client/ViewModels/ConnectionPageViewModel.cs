using client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace client.ViewModels;

public partial class ConnectionPageViewModel : ObservableObject
{
    private readonly NetUtils _netUtils;

    public ConnectionPageViewModel()
    {
        _netUtils = Application.Current.Handler.MauiContext.Services.GetService<NetUtils>();
        ConnectCommand = new RelayCommand(async () => await ConnectAsync());
    }

    [ObservableProperty]
    public string _ip;

    [ObservableProperty]
    public string _port;

    [ObservableProperty]
    public string _errorLabel;

    [ObservableProperty]
    public string _welcomeLabelText = "Введите Ip-адрес и порт";

    [ObservableProperty]
    public bool _isConnectButtonEnabled = true;

    [ObservableProperty]
    public bool _isErrorLabelEnabled = false;

    public RelayCommand ConnectCommand { get; }

    private async Task ConnectAsync()
    {
        IsErrorLabelEnabled = false;

        try
        {
            await _netUtils.SetIpAndPort(Ip, Port);
        }
        catch //TODO: расписать какие могут быть исключения
        {
            IsErrorLabelEnabled = true;
            ErrorLabel = "Некорректный ip-адрес или порт";
            return;
        }

        bool result = await _netUtils.CheckServerConnection(); ;
        if (result) // Подключено успешно
        {
            await Shell.Current.GoToAsync("AuthPage");
        }
        else
        {
            IsErrorLabelEnabled = true;
            ErrorLabel = "Ошибка подключения к серверу";
        }
    }
}