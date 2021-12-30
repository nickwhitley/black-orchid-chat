using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Interfaces
{
    public interface ITempConnections
    {
        /// <summary>
        /// Holds a user key and connection id value.
        /// </summary>
        public Dictionary<IUser, string> TempConnections { get; }

        /// <summary>
        /// Adds connection to TempConnections dictionary
        /// </summary>
        /// <param name="user"></param>
        /// <param name="connectionId"></param>
        public void AddConnection(IUser user, string connectionId);

        /// <summary>
        /// Removes connection from TempConnections dictionary
        /// </summary>
        /// <param name="user"></param>
        public void RemoveConnection(IUser user);

        /// <summary>
        /// 
        /// </summary>
        /// <returns> current number of connections </returns>
        public int NumOfConnections();

        public string GetConnectionId(IUser user);
    }
}
