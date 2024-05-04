using AutoMapper;

namespace RestaurantAPI;
public class RestaurantMappingProfile : Profile
{
    public RestaurantMappingProfile()
    {
        //createMap określa jak ma przebiegać mapowanie z jednego typu na drugi
        //jako parametry podajemy typ źródłowy a drugi to typ na jaki mapujemy
        CreateMap<Restaurant, RestaurantDto>()
            .ForMember(m => m.City, c => c.MapFrom(s => s.Address.City))
            .ForMember(m => m.Street, c => c.MapFrom(s => s.Address.Street))
            .ForMember(m => m.PostalCode, c => c.MapFrom(s => s.Address.PostalCode));
            //dla pozostałych pól nie musimy określać mapowania bo sie pokrywają więc 
            //mapper automatycznie to przepisze

        CreateMap<Dish, DishDto>();

        CreateMap<CreateRestaurantDto, Restaurant>()
            .ForMember(r => r.Address, 
                c => c.MapFrom(dto => new Address()
                    { City = dto.City, Street = dto.Street, PostalCode = dto.PostalCode }));
    }
}
