using CsvHelper.Configuration;

namespace Logic.models.Review
{
    public class ReviewValuesClassMap : ClassMap<ReviewValues>
    {
        public ReviewValuesClassMap()
        {
            Map(m => m.Kal).Name("Kal");
            Map(m => m.Name).Name("NAME");
            Map(m => m.Cost).Name("COST");
            Map(m => m.Type).Name("TYPE");
            Map(m => m.Rarity).Name("Rarity");
            Map(m => m.Nizzahon).Name("Nizzahon");
            Map(m => m.Draftsim).Name("draftsim");
            Map(m => m.MtgazDrift).Name("mtgazDrift");
            Map(m => m.MtgazRas).Name("mtgazRas");
            Map(m => m.LrMarshal).Name("LRMarshal");
            Map(m => m.Lrlsv).Name("LRLSV");
            Map(m => m.Da).Name("DA");
            Map(m => m.NizzaScore).Name("NizzaScore");
            Map(m => m.DraftsimScore).Name("draftsimScore");
            Map(m => m.DriftScore).Name("DriftScore");
            Map(m => m.RasScore).Name("RasScore");
            Map(m => m.MarshScore).Name("MarshScore");
            Map(m => m.LsvScore).Name("LSVScore");
            Map(m => m.DaScore).Name("DAScore");
            Map(m => m.AvgScore).Name("AvgScore");
            Map(m => m.StDev).Name("STDev");
        }
    }
}