using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebServer.Interfaces
{
    public interface IUserAuthenticator
    {
        bool AuthenticateUser(IUser user);
        
    }
}
