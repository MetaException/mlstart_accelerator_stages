using client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Serilog;

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
    private string _ip;

    [ObservableProperty]
    private string _port;

    [ObservableProperty]
    private string _errorLabel;

    [ObservableProperty]
    private string _welcomeLabelText = "Введите Ip-адрес и порт";

    [ObservableProperty]
    private bool _isConnectButtonEnabled = true;

    [ObservableProperty]
    private bool _isErrorLabelEnabled = false;

    public RelayCommand ConnectCommand { get; }

    private async Task ConnectAsync()
    {
        IsErrorLabelEnabled = false;

        try
        {
            _netUtils.SetIpAndPort(Ip, Port);
        }
        catch (Exception ex) //TODO: расписать какие могут быть исключения
        {
            IsErrorLabelEnabled = true;
            Log.Error($"{ex.Message} ip = {Ip}, port = {Port}");
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