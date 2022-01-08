using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace WebServer.Interfaces
{
    public interface IFileProcessor
    {
        string FilePath { get; }

        void SaveListToCSV(List<IUser> users);

        List<IUser> LoadListFromCSV();
    }
}
