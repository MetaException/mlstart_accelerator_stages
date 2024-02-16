namespace stage3.Model;

public partial class User
{
    public int Id { get; set; }

    public string LoginHash { get; set; } = null!;

    public string PassHash { get; set; } = null!;
}
