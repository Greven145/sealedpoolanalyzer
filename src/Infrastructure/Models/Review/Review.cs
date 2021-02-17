using Application.Mapping;
using AutoMapper;
using Domain.Entities;

namespace Infrastructure.Models.Review {
    public class Review : IMapTo<MagicCardReview> {
        public int Kal { get; set; }
        public string Name { get; set; }
        public string Cost { get; set; }
        public string Type { get; set; }
        public string Rarity { get; set; }
        public string Nizzahon { get; set; }
        public string Draftsim { get; set; }
        public string MtgazDrift { get; set; }
        public string MtgazRas { get; set; }
        public string LrMarshal { get; set; }
        public string Lrlsv { get; set; }
        public string Da { get; set; }
        public int NizzaScore { get; set; }
        public int DraftsimScore { get; set; }
        public int DriftScore { get; set; }
        public int RasScore { get; set; }
        public int MarshScore { get; set; }
        public int LsvScore { get; set; }
        public int DaScore { get; set; }
        public float AvgScore { get; set; }
        public float StDev { get; set; }

        public void Mapping(Profile profile) {
            profile.CreateMap<Review, MagicCardReview>()
                .ForMember(r => r.Score, opt => opt.MapFrom(s => s.AvgScore));
        }
    }
}
