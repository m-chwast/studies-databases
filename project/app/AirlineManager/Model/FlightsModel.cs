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

    public async Task GetFlightDetails(FlightData flight)
    {
        string query = $@"SELECT * FROM airline.get_flight_details({flight.Id})";
        var data = await _database.GetData(query);
        if(data.RowCount == 0)
            return;
        var row = data.Data[0];
        flight.Aircraft = row[0];   
        flight.AircraftDetails = row[1];
        flight.RouteDetails = row[2];
    }

    public async Task AddFlight(string route, string date, string aircraft)
    {
        string query = $@"CALL airline.insert_flight({route}, '{date}', {aircraft});";
        await _database.ExecuteQuery(query);
    }
}