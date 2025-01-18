namespace AirlineManager.Model
{
    public class AirportData
    {
        public string LongName { get; }
        public string Designator { get; }

        public AirportData(string longName, string designator)
        {
            LongName = longName;
            Designator = designator;
        }
    }
}