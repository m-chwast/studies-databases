using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineManager.Model;

public class FlightModel : ModelBase
{
    private IDatabase _database;

    public FlightModel(IDatabase database)
    {
        _database = database;
    }

    public async Task<IEnumerable<FlightData>> GetFlights()
    {
        const string query = "SELECT * FROM airline.flights_view";
        var data = await _database.GetData(query);
        List<FlightData> flights = new();
        foreach (var row in data.Data)
        {
            FlightData flight = new(row[0], row[1], row[2]);
            flights.Add(flight);
        }
        return flights;
    }
}