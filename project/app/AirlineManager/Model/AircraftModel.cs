using System.Collections.Generic;
using System.Threading.Tasks;

namespace AirlineManager.Model;

public class AircraftModel : ModelBase
{
    private IDatabase _database;

    public AircraftModel(IDatabase database)
    {
        _database = database;
    }

    public async Task<List<AircraftData>> GetNewData(bool isSummary)
    {
        const string aircraftLongQuery = @"
            SELECT * FROM linia.samolot_dlugi_widok";
        const string aircraftShortQuery = @"
            SELECT * FROM linia.samolot_krotki_widok";

        // another idea was to do this in a single query, something like this
        /*
        SELECT a.aircraft_id, at.aircraft_type_name, type_counts.type_count
        FROM airline.aircraft a JOIN airline.aircraft_type at 
        ON a.aircraft_type_id = at.aircraft_type_id
        JOIN 
            (SELECT a.aircraft_type_id aircraft_type_id, COUNT(*) type_count
            FROM airline.aircraft a
            GROUP BY a.aircraft_type_id) type_counts 
        ON a.aircraft_type_id = type_counts.aircraft_type_id;
        */

        string query = isSummary ? aircraftShortQuery : aircraftLongQuery;
        var dataTable = await _database.GetData(query);
        var updatedCollection = UpdateAirlineCollection(dataTable, isSummary);
        return updatedCollection;
    }

    private List<AircraftData> UpdateAirlineCollection(DataTable dataTable, bool isShortList)
    {
        List<AircraftData> newAircraft = new();
        foreach(var row in dataTable.Data)
        {
            AircraftData aircraftData = isShortList 
                ? new(row[0], null, row[1])
                : new(row[1], row[0]);
            newAircraft.Add(aircraftData);
        }
        return newAircraft;
    }
}