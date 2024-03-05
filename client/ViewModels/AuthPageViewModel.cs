using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace client.ViewModels;

public partial class AuthPageViewModel : ObservableObject
{
    //private readonly DbUtils _dbUtils;

    public AuthPageViewModel()
    {
        //_dbUtils = dbUtils;
        LoginCommand = new RelayCommand(async () => await LoginAsync());
        RegisterCommand = new RelayCommand(async () => await RegisterAsync());

        //_ = CheckDbConnection();
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

    private async Task<bool> CheckDbConnection()
    {
        //if (!await _dbUtils.CheckDbConnection())
        {
            IsErrorLabelEnabled = true;
            ErrorLabel = "Ошибка подключения к БД";
            return false;
        }
        return true;
    }

    private async Task LoginAsync()
    {
        if (!await CheckDbConnection())
        {
            return;
        }

        try
        {
            IsLoginButtonEnabled = false;
            bool isAuthorized = false; //await _dbUtils.AuthorizeUser(Login, Password);
            if (isAuthorized)
            {
                await Shell.Current.GoToAsync("MainPage");
                return;
            }
            else
            {
                WelcomeLabelText = "Неверный логин или пароль";
            }
        }
        catch (Exception ex)
        {
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
        if (!await CheckDbConnection())
        {
            return;
        }

        try
        {
            IsRegisterButtonEnabled = false;
            bool isRegistered = false;//await _dbUtils.RegisterNewUser(Login, Password);
            if (isRegistered)
            {
                await Shell.Current.GoToAsync("MainPage");
                return;
            }
        }
        catch (Exception ex)
        {
            ErrorLabel = ex.Message;
        }
        finally
        {
            IsRegisterButtonEnabled = true;
        }
    }
}
