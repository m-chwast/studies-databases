namespace AirlineManager.Model;

public class PersonnelData
{
    public string FirstName { get; }
    public string LastName { get; }
    public string Role { get; }

    public PersonnelData(string firstName, string lastName, string role)
    {
        FirstName = firstName;
        LastName = lastName;
        Role = role;
    }
}