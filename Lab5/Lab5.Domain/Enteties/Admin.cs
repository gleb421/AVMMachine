namespace Lab5.Domain.Enteties;

public class Admin
{
    public Guid AdminId { get; private set; }

    public string SystemPassword { get; private set; }

    public Admin(Guid adminId, string systemPassword)
    {
        AdminId = adminId;
        SystemPassword = systemPassword;
    }

    public bool ValidatePassword(string inputPassword)
    {
        return SystemPassword == inputPassword;
    }
}