using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineManager.Model;

public class RoutesModel : ModelBase
{
    private IDatabase _database;

    public RoutesModel(IDatabase database)
    {
        _database = database;
    }

    public async Task<IEnumerable<AirportData>> GetAirports()
    {
        const string query = @"
            SELECT a.airport_designator, a.airport_name
            FROM airline.airport a
            ORDER BY a.airport_designator;";

        var dataTable = await _database.GetData(query);

        List<AirportData> airports = new();
        foreach (var row in dataTable.Data)
        {
            AirportData airport = new(row[0], row[1]);
            airports.Add(airport);
        }
        
        return airports;
    }
}