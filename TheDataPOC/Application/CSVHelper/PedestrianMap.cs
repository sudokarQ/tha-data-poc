namespace Application.CSVHelper
{
    using CsvHelper.Configuration;

    using Domain.Models;

    public sealed class PedestrianMap : ClassMap<PedestrianCSV>
    {
        public PedestrianMap()
        {
            Map(m => m.Date).Name("Date", "date", "DATE", "Time", "time", "TIME");

            Map(m => m.SeventhAndParkCampus).Name("7th and Park Campus", "7th_and_Park_Campus", "SeventhAndParkCampus");
            
            Map(m => m.BlineConventionCentre).Name("Bline Convention Cntr", "BlineConventionCentre", "Bline_Convention_Cntr");
            
            Map(m => m.JordanAndSeventh).Name("Jordan and 7th", "Jordan_and_7th", "JordanAndSeventh");
            
            Map(m => m.NCollegeAndRailRoad).Name("N College and RR", "n_college_and_rr", "N_College_and_RR", "NCollegeAndRailRoad");
            
            Map(m => m.SWalnutAndWylie).Name("S Walnut and Wylie", "S_Walnut_and_Wylie", "SWalnutAndWylie");
        }
    }
}
