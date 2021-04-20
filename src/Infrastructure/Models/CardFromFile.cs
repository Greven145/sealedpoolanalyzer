namespace Infrastructure.Models {
    public class CardFromFile {
        public string Name { get; set; }
        public int Quantity { get; set; }
        public int MultiverseId { get; set; }
        
        public string Set { get; set; }
        
        public CardFromFile ShallowCopy() {
            return (CardFromFile) MemberwiseClone();
        }
    }
}
