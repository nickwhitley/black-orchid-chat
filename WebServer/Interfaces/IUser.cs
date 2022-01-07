namespace WebServer.Interfaces
{
    public interface IUser
    {
        string Username { get; set; }

        string ConnectionId { get; set; }
    }
}