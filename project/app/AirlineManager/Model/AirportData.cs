namespace AirlineManager.Model
{
    public class AirportData
    {
        public string LongName { get; }
        public string Designator { get; }

        public AirportData(string designator, string longName)
        {            
            Designator = designator;
            LongName = longName;
        }
    }
}