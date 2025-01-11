using System.Collections.Generic;

namespace AirlineManager.Model;

public class DataTable
{
    private List<List<string>> _data = new();

    public List<List<string>> Data { get => _data; }
    public int RowCount { get => _data.Count; }
    public int ColumnCount
    {
        get
        { 
            if(_data.Count == 0)
                return 0;
            return _data[0].Count; 
        }
    }

    public void AddRow(List<string> row)
    {
        _data.Add(row);
    }

    public override string ToString()
    {
        string res = string.Empty;
        foreach(var v in _data)
        {
            foreach(var s in v)
                res += s + ", ";
            res += "\r\n";
        }
        return res;
    }
}