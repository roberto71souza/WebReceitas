using ApiReceitas.Models;
using AutoMapper;
using Dominio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiReceitas.Profiles
{
    public class ProfileMapper : Profile
    {
        public ProfileMapper()
        {
            CreateMap<Receita,ReceitaModel>().ReverseMap();
            CreateMap<Usuario, UsuarioModel>().ReverseMap();
        }
    }
}
