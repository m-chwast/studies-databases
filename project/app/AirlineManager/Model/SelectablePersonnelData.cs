namespace AirlineManager.Model;

public class SelectablePersonnelData : PersonnelData
{
    public bool IsSelected { get; set; }

    public SelectablePersonnelData(string id, string firstName, string lastName, string role) : base(id, firstName, lastName, role)
    {
        IsSelected = false;
    }
}