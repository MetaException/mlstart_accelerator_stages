using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;

namespace server.Models;

public partial class LoginModel : IdentityUser<int>
{

}
