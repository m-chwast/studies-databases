namespace AirlineManager.Model;

using System;
using System.Threading.Tasks;

public interface IDatabase
{
    public bool IsOpen { get; }
    public event EventHandler Refresh; 

    public Task<DataTable> GetData(string query, bool logResult = true);
}