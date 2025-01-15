using System.Collections.Generic;
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
            SELECT r.role_name
            FROM airline.role r
            ORDER BY r.role_id;";

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
            INSERT INTO airline.person (person_name, person_surname, person_role_id)
            VALUES ('{name}', '{surname}', (SELECT role_id FROM airline.role WHERE role_name = '{role}'));";

        return await _database.ExecuteQuery(query);
    }

    private string GetFilteredPersonnelQuery(int? id, string? surname)
    {
        string query = @"
            SELECT p.person_id, p.person_name, p.person_surname, r.role_name
            FROM airline.person p
            JOIN airline.role r
            ON p.person_role_id = r.role_id
            WHERE ";

        bool isSurnameFilter = !string.IsNullOrEmpty(surname);
        if(id is not null)
            query += $"p.person_id = {id}" + (isSurnameFilter ? " AND " : "");
        if(isSurnameFilter)
            query += $"p.person_surname LIKE '{surname}%'";

        query += ";";

        return query;
    }

    private string GetPersonnelQuery(bool flightAttendants, bool captains, bool firstOfficers)
    {
        string query = @"
            SELECT p.person_id, p.person_name, p.person_surname, r.role_name
            FROM airline.person p
            JOIN airline.role r
            ON p.person_role_id = r.role_id
            WHERE ";

        bool getAnyRoles = flightAttendants || captains || firstOfficers;

        // initialize to no roles, it will make our life easier later
        string whereCondition = "False";
        if(getAnyRoles)
        {
            if(flightAttendants)
                whereCondition += " OR r.role_name = 'Flight Attendant'";
            if(captains)
                whereCondition += " OR r.role_name = 'Captain'";
            if(firstOfficers)
                whereCondition += " OR r.role_name = 'First Officer'";
        }

        query += whereCondition;
        query += " ORDER BY p.person_surname;";

        return query;           
    }

}