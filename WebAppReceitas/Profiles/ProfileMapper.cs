using AutoMapper;
using Dominio;
using WebAppReceitas.Models;

namespace WebAppReceitas.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Receita,ReceitaModel>().ReverseMap();
        }
    }
}