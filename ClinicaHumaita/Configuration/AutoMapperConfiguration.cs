using AutoMapper;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;


namespace ClinicaHumaita.Configuration
{
    public class AutoMapperConfiguration
    {
        public MapperConfiguration createAutoMapperConfig()
        {
            return new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonAddViewModel, Person>();
                cfg.CreateMap<PersonUpdateViewModel, Person>();
                cfg.CreateMap<PersonDeleteViewModel, Person>();
            });
        }
    }
}
