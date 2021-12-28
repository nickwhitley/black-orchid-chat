namespace WebServer.Interfaces
{
    public interface IUser
    {
        string ConnectionId { get; set; }
        string Password { get; set; }
        string Username { get; set; }
        bool IsOnline { get; }
    }
}