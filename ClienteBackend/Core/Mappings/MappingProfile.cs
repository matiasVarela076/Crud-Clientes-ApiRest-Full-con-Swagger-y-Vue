using AutoMapper;
using ClientesAPI.Core.DTOs;
using ClientesAPI.Core.Entities;

namespace ClientesAPI.Core.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Cliente, ClienteDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ID))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.NOMBRE))
                .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.APELLIDO))
                .ForMember(dest => dest.FechaNacimiento, opt => opt.MapFrom(src => src.FECHA_NACIMIENTO))
                .ForMember(dest => dest.Cuit, opt => opt.MapFrom(src => src.CUIT))
                .ForMember(dest => dest.Domicilio, opt => opt.MapFrom(src => src.DOMICILIO))
                .ForMember(dest => dest.Telefono, opt => opt.MapFrom(src => src.TELEFONO))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.EMAIL))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.ACTIVO))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.FECHA_CREACION));

            CreateMap<ClienteCreacionDto, Cliente>()
                .ForMember(dest => dest.NOMBRE, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.APELLIDO, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.FECHA_NACIMIENTO, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForMember(dest => dest.CUIT, opt => opt.MapFrom(src => src.Cuit))
                .ForMember(dest => dest.DOMICILIO, opt => opt.MapFrom(src => src.Domicilio))
                .ForMember(dest => dest.TELEFONO, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.EMAIL, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ACTIVO, opt => opt.MapFrom(src => true));

            CreateMap<ClienteActualizacionDto, Cliente>()
                .ForMember(dest => dest.NOMBRE, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.APELLIDO, opt => opt.MapFrom(src => src.Apellido))
                .ForMember(dest => dest.FECHA_NACIMIENTO, opt => opt.MapFrom(src => src.FechaNacimiento))
                .ForMember(dest => dest.DOMICILIO, opt => opt.MapFrom(src => src.Domicilio))
                .ForMember(dest => dest.TELEFONO, opt => opt.MapFrom(src => src.Telefono))
                .ForMember(dest => dest.EMAIL, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.ACTIVO, opt => opt.MapFrom(src => src.Activo ?? true));
        }
    }
}
