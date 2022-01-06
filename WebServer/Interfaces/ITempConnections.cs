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
        public Dictionary<string, IUser> TempConnections { get; }

        /// <summary>
        /// Adds connection to TempConnections dictionary
        /// </summary>
        /// <param name="user"></param>
        /// <param name="connectionId"></param>
        public void AddConnection(string connectionId, IUser user);

        /// <summary>
        /// Removes connection from TempConnections dictionary
        /// </summary>
        /// <param name="user"></param>
        public void RemoveConnection(string connectionId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns> current number of connections </returns>
        public int NumOfConnections();
    }
}
