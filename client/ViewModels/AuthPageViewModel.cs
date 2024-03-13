using client.Utils;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using static client.Model.ResponseEnum;

namespace client.ViewModels;

public partial class AuthPageViewModel : ObservableObject
{
    private readonly NetUtils _netUtils;

    public AuthPageViewModel()
    {
        _netUtils = Application.Current.Handler.MauiContext.Services.GetService<NetUtils>();
        LoginCommand = new RelayCommand(async () => await LoginAsync());
        RegisterCommand = new RelayCommand(async () => await RegisterAsync());

        _ = CheckServerConnection();
    }

    [ObservableProperty]
    public string _login;

    [ObservableProperty]
    public string _password;

    [ObservableProperty]
    public string _errorLabel;

    [ObservableProperty]
    public string _welcomeLabelText = "Введите логин и пароль";

    [ObservableProperty]
    public bool _isLoginButtonEnabled = true;

    [ObservableProperty]
    public bool _isRegisterButtonEnabled = true;

    [ObservableProperty]
    public bool _isErrorLabelEnabled = false;

    public RelayCommand LoginCommand { get; }
    public RelayCommand RegisterCommand { get; }

    private async Task<bool> CheckServerConnection()
    {
        if (!await _netUtils.CheckServerConnection())
        {
            IsErrorLabelEnabled = true;
            ErrorLabel = "Ошибка подключения к Серверу";
            return false;
        }
        return true;
    }

    private async Task LoginAsync()
    {
        if (!await CheckServerConnection())
        {
            return;
        }

        IsErrorLabelEnabled = false;

        try
        {
            IsLoginButtonEnabled = false;
            var result = await _netUtils.Login(Login, Password);

            if (result == NetUtilsResponseCodes.OK)
            {
                await Shell.Current.GoToAsync("MainPage");
                return;
            }
            else if (result == NetUtilsResponseCodes.UNATHROIZED)
            {
                IsErrorLabelEnabled = true;
                ErrorLabel = "Неверный логин или пароль";
            }
            else
            {
                IsErrorLabelEnabled = true;
                ErrorLabel = "Произошла ошибка при выполнении входа";
            }
        }
        catch (Exception ex)
        {
            IsErrorLabelEnabled = true;
            ErrorLabel = ex.Message;
        }
        finally
        {
            IsLoginButtonEnabled = true;
        }
    }

    // TODO: Отедльная страница регистрации
    private async Task RegisterAsync()
    {
        if (!await CheckServerConnection())
        {
            return;
        }

        try
        {
            IsRegisterButtonEnabled = false;
            var result = await _netUtils.Register(Login, Password);

            if (result == NetUtilsResponseCodes.OK)
            {
                await Shell.Current.GoToAsync("MainPage");
                return;
            }
            else if (result == NetUtilsResponseCodes.USERISALREDYEXISTS)
            {
                IsErrorLabelEnabled = true;
                ErrorLabel = "Пользователь уже зарегистрирован";
            }
            else if (result == NetUtilsResponseCodes.BADREQUEST)
            {
                IsErrorLabelEnabled = true;
                ErrorLabel = "Некорректно введён логин или пароль";
            }
            else
            {
                IsErrorLabelEnabled = true;
                ErrorLabel = "Произошла ошибка при регистрации пользователя";
            }
        }
        catch (Exception ex)
        {
            IsErrorLabelEnabled = true;
            ErrorLabel = ex.Message;
        }
        finally
        {
            IsRegisterButtonEnabled = true;
        }
    }
}