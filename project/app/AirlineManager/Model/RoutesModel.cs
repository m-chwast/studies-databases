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

    public async Task<IEnumerable<RouteData>> GetRoutes()
    {
        const string query = @"
            SELECT * FROM airline.get_routes();";

        var dataTable = await _database.GetData(query);

        List<RouteData> routes = new();
        foreach (var row in dataTable.Data)
        {
            RouteData route = new(row[0], row[1], row[2], row[3]);
            routes.Add(route);
        }

        return routes;
    }

    public async Task AddRoute(string departure, string destination, float time)
    {
        string query = $@"
            CALL airline.insert_route('{departure}', '{destination}', {time});";
        await _database.ExecuteQuery(query);
    }

    public async Task DeleteRoute(int id)
    {
        string query = $@"
            CALL airline.delete_route({id});";
        await _database.ExecuteQuery(query);
    }
}