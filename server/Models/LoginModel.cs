using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;

namespace server.Models;

public partial class LoginModel
{
    public int Id { get; set; }
    public string UserName { get; set; }

    public string Password { get; set; }
}
