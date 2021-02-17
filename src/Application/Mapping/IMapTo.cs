using AutoMapper;

namespace Application.Mapping {
    public interface IMapTo<T> {
        void Mapping(Profile profile) {
            profile.CreateMap(GetType(), typeof(T));
        }
    }
}
