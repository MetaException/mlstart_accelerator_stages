using client.Model;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using static client.Model.ResponseEnum;

namespace client.Utils;

public class NetUtils
{
    private class TokenModel
    {
        public string token { get; set; }
    }

    private readonly HttpClientHandler _handler;
    private HttpClient _client;

    public NetUtils()
    {
        _handler = new HttpClientHandler();
    }

    public async Task<NetUtilsResponseCodes> Register(string username, string password)
    {
        var newUser = new User
        {
            Login = username,
            Password = password
        };

        var response = await _client.PostAsJsonAsync("api/auth/register", newUser);

        if (response.IsSuccessStatusCode)
        {
            var responseContent = await response.Content.ReadAsStringAsync();
            var tokenResponse = JsonSerializer.Deserialize<TokenModel>(responseContent);
            if (tokenResponse != null && !string.IsNullOrEmpty(tokenResponse.token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenResponse.token);
                return NetUtilsResponseCodes.OK;
            }
        }
        else if (response.StatusCode == HttpStatusCode.Conflict)
        {
            return NetUtilsResponseCodes.USERISALREDYEXISTS;
        }
        else if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            return NetUtilsResponseCodes.BADREQUEST;
        }
        return NetUtilsResponseCodes.ERROR;
    }

    public async Task<NetUtilsResponseCodes> Login(string username, string password)
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
                return NetUtilsResponseCodes.OK;
            }
        }
        else if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            return NetUtilsResponseCodes.UNATHROIZED;
        }
        return NetUtilsResponseCodes.ERROR;
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

    public async Task SetIpAndPort(string ip, string port)
    {
        // Задавать параметры можно только до отправки первого запроса
        _client = new HttpClient(_handler) { BaseAddress = new Uri($"https://{ip}:{port}") };
    }
}