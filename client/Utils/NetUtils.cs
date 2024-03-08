using client.Model;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net.NetworkInformation;
using System.Text.Json;

namespace client.Utils;

public class NetUtils
{
    private class TokenModel
    {
        public string token { get; set; }
    }

    private readonly HttpClientHandler _handler;
    private readonly HttpClient _client;

    public NetUtils()
    {
        _handler = new HttpClientHandler();
        _client = new HttpClient(_handler) { BaseAddress = new Uri("https://localhost:5000")  };
    }

    public async Task<bool> Register(string username, string password)
    {
        var newUser = new User
        {
            Login = username,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("api/auth/register", newUser);

        //_client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", yourToken);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenModel>(responseContent);
            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.token);
                return true;
            }
            return false;
        }
        else
        {
            return false;
        }
    }

    public async Task<bool> Login(string username, string password)
    {
        var user = new User
        {
            Login = username,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("api/auth/login", user);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenModel>(responseContent);
            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.token);
                return true;
            }
            return false;
        }
        else // Другие ошибки
        {
            return false;
        }
    }

    public async Task<bool> CheckServerConnection()
    {
        try
        {
            var response = await _client.GetAsync("api/health");

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            else
                return false;
        }
        catch
        {
            return false;
        }
    }

    public async Task<ObservableCollection<LogRecord>> GetDataAsync()
    {
        try
        {
            var response = await _client.GetAsync("api/data");

            if (response.IsSuccessStatusCode)
            {
                var a = await response.Content.ReadFromJsonAsync<ObservableCollection<LogRecord>>();
                return a;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            return null;
        }
    }
}