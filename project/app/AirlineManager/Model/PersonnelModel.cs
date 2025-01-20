using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AirlineManager.Model;

public class PersonnelModel : ModelBase
{
    private IDatabase _database;

    public PersonnelModel(IDatabase database)
    {
        _database = database;
    }

    public async Task<IEnumerable<PersonnelData>> GetNewData(bool flightAttendants, bool captains, bool firstOfficers)
    {
        string query = GetPersonnelQuery(flightAttendants, captains, firstOfficers);
        var dataTable = await _database.GetData(query);
        var personnel = ExtractPersonnelFromDataTable(dataTable);
        return personnel;
    }

    public async Task<IEnumerable<string>> GetRoles()
    {
        const string query = @"
            SELECT r.rola_nazwa
            FROM linia.rola r
            ORDER BY r.rola_id;";

        var dataTable = await _database.GetData(query);

        List<string> roles = new();
        foreach(var row in dataTable.Data)
            roles.Add(row[0]);

        return roles;
    }

    public async Task<IEnumerable<PersonnelData>> GetFilteredPersonnel(int? id, string? surname)
    {
        if(id is null && string.IsNullOrEmpty(surname))
            return new List<PersonnelData>();

        string query = GetFilteredPersonnelQuery(id, surname);
        var dataTable = await _database.GetData(query);

        var personnel = ExtractPersonnelFromDataTable(dataTable);
        return personnel;
    }       

    private List<PersonnelData> ExtractPersonnelFromDataTable(DataTable dataTable)
    {
        List<PersonnelData> personnel = new();

        foreach(var row in dataTable.Data)
        {
            PersonnelData person = new(row[0], row[1], row[2], row[3]);
            personnel.Add(person);
        }

        return personnel;
    }

    public async Task<bool> AddPerson(string name, string surname, string role)
    {
        string query = $@"
            INSERT INTO linia.osoba (osoba_imie, osoba_nazwisko, osoba_rola_id)
            VALUES ('{name}', '{surname}', (SELECT rola_id FROM linia.rola WHERE rola_nazwa = '{role}'));";

        return await _database.ExecuteQuery(query);
    }

    public Task<bool> DeletePersons(IEnumerable<int> ids)
    {
        if(ids.Count() == 0)
            return Task.FromResult(true);

        string query = $@"
            DELETE FROM linia.osoba
            WHERE osoba_id IN ({string.Join(',', ids)});";

        return _database.ExecuteQuery(query);
    }

    private string GetFilteredPersonnelQuery(int? id, string? surname)
    {
        string query = @"
            SELECT o.osoba_id, o.osoba_imie, o.osoba_nazwisko, r.rola_nazwa
            FROM linia.osoba o
            JOIN linia.rola r
            ON o.osoba_rola_id = r.rola_id
            WHERE ";

        bool isSurnameFilter = !string.IsNullOrEmpty(surname);
        if(id is not null)
            query += $"o.osoba_id = {id}" + (isSurnameFilter ? " AND " : "");
        if(isSurnameFilter)
            query += $"o.osoba_nazwisko LIKE '{surname}%'";

        query += ";";

        return query;
    }

    private string GetPersonnelQuery(bool flightAttendants, bool captains, bool firstOfficers)
    {
        string query = @"
            SELECT o.osoba_id, o.osoba_imie, o.osoba_nazwisko, r.rola_nazwa
            FROM linia.osoba o
            JOIN linia.rola r
            ON o.osoba_rola_id = r.rola_id
            WHERE ";

        bool getAnyRoles = flightAttendants || captains || firstOfficers;

        // initialize to no roles, it will make our life easier later
        string whereCondition = "False";
        if(getAnyRoles)
        {
            if(flightAttendants)
                whereCondition += " OR r.rola_nazwa = 'Steward'";
            if(captains)
                whereCondition += " OR r.rola_nazwa = 'Kapitan'";
            if(firstOfficers)
                whereCondition += " OR r.rola_nazwa = 'Pierwszy Oficer'";
        }

        query += whereCondition;
        query += " ORDER BY o.osoba_nazwisko;";

        return query;           
    }

}