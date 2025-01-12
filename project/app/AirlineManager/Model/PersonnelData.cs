namespace AirlineManager.Model;

public class PersonnelData
{
    public int Id { get; }
    public string FirstName { get; }
    public string LastName { get; }
    public string Role { get; }

    public PersonnelData(string id, string firstName, string lastName, string role)
    {
        Id = int.Parse(id);
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }
}