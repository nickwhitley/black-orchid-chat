namespace WebServer.User
{
    public interface IUser
    {
        string connectionId { get; set; }
        string password { get; set; }
        string username { get; set; }
    }
}