namespace AirlineManager.Model;

using System;

public interface IDatabase
{
    public event EventHandler Refresh; 
}