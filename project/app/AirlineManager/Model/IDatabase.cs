namespace AirlineManager.Model;

using System;

public interface IDatabase
{
    public bool IsOpen { get; }
    public event EventHandler Refresh; 
}