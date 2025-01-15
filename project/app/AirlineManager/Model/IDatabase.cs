namespace AirlineManager.Model;

using System;
using System.Threading.Tasks;

public interface IDatabase
{
    public event EventHandler Refresh; 

    public Task<bool> ExecuteQuery(string query);
    public Task<DataTable> GetData(string query, bool logResult = true);
}