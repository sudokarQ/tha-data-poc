namespace Application.CSVHelper
{
    using CsvHelper.Configuration;

    using Domain.Models;

    public sealed class CrashMap : ClassMap<CrashCSV>
    {
        public CrashMap()
        {
            Map(m => m.Date).Name("Collision Date", "Date", "DATE").Optional();

            Map(m => m.Year).Name("Year").Optional();
            
            Map(m => m.Month).Name("Month").Optional();
            
            Map(m => m.Day).Name("Day").Optional();
            
            Map(m => m.Time).Name("Hour", "Time", "Collision Time", "TIME");
            
            Map(m => m.InjuryType).Name("Injury Type").Optional();

            Map(m => m.NumberInjured).Name("Number Injured").Optional();

            Map(m => m.Inj).Name("Inj", "INJ").Optional();

            Map(m => m.NumberDead).Name("Number Dead").Optional();

            Map(m => m.Dead).Name("Dead", "DEAD").Optional();
            
            Map(m => m.PrimaryFactor).Name("Primary Factor");
            
            Map(m => m.Latitude).Name("Latitude");
            
            Map(m => m.Longitude).Name("Longitude");
        }
    }
}
