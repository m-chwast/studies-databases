using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        string personnelQuery = $@"
        SELECT p.person_id, p.person_name, p.person_surname, r.role_name
        FROM airline.flight f
        JOIN airline.flight_crew fc ON f.flight_id = fc.flight_id
        JOIN airline.person p ON p.person_id = fc.person_id
        JOIN airline.role r ON r.role_id = p.person_role_id
        WHERE f.flight_id = {flight.Id};";

        var personnelData = await _database.GetData(personnelQuery);
        List<PersonnelData> personnel = new();
        foreach (var personnelRow in personnelData.Data)
        {
            PersonnelData person = new(personnelRow[0], personnelRow[1], personnelRow[2], personnelRow[3]);
            personnel.Add(person);
        }
        flight.Crew = new ObservableCollection<PersonnelData>(personnel);
    }

    public async Task AddFlight(string route, string date, string aircraft)
    {
        string query = $@"CALL airline.insert_flight({route}, '{date}', {aircraft});";
        await _database.ExecuteQuery(query);
    }

    public async Task AddPersonToCrew(int flightId, int personId)
    {
        string query = $@"CALL airline.add_person_to_flight({flightId}, {personId});";
        await _database.ExecuteQuery(query);
    }
}