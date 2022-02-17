using AutoMapper;
using ClinicaHumaita.Data.Models;
using ClinicaHumaita.Shared.ViewModels;


namespace ClinicaHumaita.Tests.Configuration
{
    public class AutoMapperConfiguration
    {
        public MapperConfiguration createAutoMapperConfig()
        {
            return new AutoMapper.MapperConfiguration(cfg =>
            {
                cfg.CreateMap<PersonAddViewModel, Person>();
            });
        }
    }
}
