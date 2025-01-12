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
        List<PersonnelData> personnel = new();

        return personnel;
    }

}