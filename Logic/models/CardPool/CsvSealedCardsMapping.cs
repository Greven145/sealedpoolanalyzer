using TinyCsvParser.Mapping;

namespace Logic.models.CardPool
{
    public class CsvSealedCardsMapping : CsvMapping<SealedCards>
    {
        public CsvSealedCardsMapping()
        {
            MapProperty(0, x => x.TotalQty);
            MapProperty(1, x => x.RegQty);
            MapProperty(2, x => x.FoilQty);
            MapProperty(3, x => x.Card);
            MapProperty(4, x => x.Set);
            MapProperty(5, x => x.ManaCost);
            MapProperty(6, x => x.CardType);
            MapProperty(7, x => x.Color);
            MapProperty(8, x => x.Rarity);
            MapProperty(9, x => x.Mvid);
            MapProperty(10, x => x.SinglePrice);
            MapProperty(11, x => x.SingleFoilPrice);
            MapProperty(12, x => x.TotalPrice);
            MapProperty(13, x => x.PriceSource);
            MapProperty(14, x => x.Notes);
        }
    }
    
    //public class CsvSealedCardsMapping : ClassMap<SealedCards>
    //{
    //    public CsvSealedCardsMapping()
    //    {
    //        Map(x => x.TotalQty).NameIndex(0);
    //        Map(x => x.RegQty).NameIndex(1);
    //        Map(x => x.FoilQty).NameIndex(2);
    //        Map(x => x.Card).NameIndex(3);
    //        Map(x => x.Set).NameIndex(4);
    //        Map(x => x.ManaCost).NameIndex(5);
    //        Map(x => x.CardType).NameIndex(6);
    //        Map(x => x.Color).NameIndex(7);
    //        Map(x => x.Rarity).NameIndex(8);
    //        Map(x => x.Mvid).NameIndex(9);
    //        Map(x => x.SinglePrice).NameIndex(10);
    //        Map(x => x.SingleFoilPrice).NameIndex(11);
    //        Map(x => x.TotalPrice).NameIndex(12);
    //        Map(x => x.PriceSource).NameIndex(13);
    //        Map(x => x.Notes).NameIndex(14);
    //    }
    //}
}