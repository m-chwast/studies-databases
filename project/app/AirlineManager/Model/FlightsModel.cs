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
        const string query = "SELECT * FROM linia.loty_widok;";
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
        string query = $@"SELECT * FROM linia.czytaj_detale_lotu({flight.Id});";
        var data = await _database.GetData(query);
        if(data.RowCount == 0)
            return;
        var row = data.Data[0];
        flight.Aircraft = row[0];   
        flight.AircraftDetails = row[1];
        flight.RouteDetails = row[2];

        string personnelQuery = $@"
        SELECT o.osoba_id, o.osoba_imie, o.osoba_nazwisko, r.rola_nazwa
        FROM linia.lot l
        JOIN linia.lot_zaloga lz ON l.lot_id = lz.lot_id
        JOIN linia.osoba o ON o.osoba_id = lz.osoba_id
        JOIN linia.rola r ON r.rola_id = o.osoba_rola_id
        WHERE l.lot_id = {flight.Id};";

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
        string query = $@"CALL linia.dodaj_lot({route}, '{date}', {aircraft});";
        await _database.ExecuteQuery(query);
    }

    public async Task DeleteFlight(int flightId)
    {
        string query = $@"DELETE FROM linia.lot l WHERE l.lot_id = {flightId};";
        await _database.ExecuteQuery(query);
    }

    public async Task AddPersonToCrew(int flightId, int personId)
    {
        string query = $@"CALL linia.dodaj_osobe_do_lotu({flightId}, {personId});";
        await _database.ExecuteQuery(query);
    }

    public async Task RemovePersonFromCrew(int flightId, int personId)
    {
        string query = $@"CALL linia.usun_osobe_z_lotu({flightId}, {personId});";
        await _database.ExecuteQuery(query);
    }
}