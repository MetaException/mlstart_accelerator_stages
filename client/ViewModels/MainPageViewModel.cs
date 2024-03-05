using CommunityToolkit.Mvvm.ComponentModel;
using cmd;
using System.Collections.ObjectModel;
using System.Net.Http.Json;
using client.Model;

namespace client.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    public MainPageViewModel()
    {
        _ = StartLogger();
        //_ = RunSimulator();
    }

    [ObservableProperty]
    public ObservableCollection<Logger.LogRecord> _logsList;

    private async Task test()
    {
        using (var client = new HttpClient())
        {
            var registerModel = new User
            {
                Login = "newuser",
                Password = "password123"
            };

            var response = await client.PostAsJsonAsync("https://localhost:7197/api/auth/register", registerModel);

            if (response.IsSuccessStatusCode)
            {
                //var result = await response.Content.ReadAsAsync<YourTokenResponseModel>();
            }
            else
            {
            }
        }
    }

    private async Task StartLogger()
    {
        Logger.CreateLogger();
        LogsList = Logger.logEntries;
    }

    private async Task RunSimulator()
    {
        while (true)
        {
            //wait Task.Run(() => Simulator.SimulateDay());
            await Task.Delay(TimeSpan.FromSeconds(1)); // По заданию цикл должен идти бесконечно
        }
    }
}
