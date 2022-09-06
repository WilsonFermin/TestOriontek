using AutoMapper;
using EventrixAPI.DTOs;
using EventrixAPI.Entidades;

namespace EventrixAPI.Utilidades
{
    public class AutoMapperProfile: Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Cliente, ClienteDTO>().ReverseMap();
            CreateMap<ClienteCreaccionDTO, Cliente>();
            CreateMap<ClienteEditarDTO, Cliente>();
            CreateMap<Cliente, ClienteDTOConDirecciones>();
            CreateMap<ClientePatchDTO, Cliente>().ReverseMap();

            CreateMap<Direccion, DireccionDTO>().ReverseMap();
            CreateMap<DireccionCreaccionDTO, Direccion>();
        }
    }
}
