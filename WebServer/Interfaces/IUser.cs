namespace WebServer.Interfaces
{
    public interface IUser
    {
        string Password { get; set; }
        string Username { get; set; }
        bool IsOnline { get; }
    }
}