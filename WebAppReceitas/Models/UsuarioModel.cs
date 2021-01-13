﻿using Dominio;
using System;
using System.Collections.Generic;


namespace WebAppReceitas.Models
{
    public class UsuarioModel
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Cidade { get; set; }
        public string Estado { get; set; }
        public string Data_Nascimento { get; set; }
        public List<ReceitaModel> Receitas { get; set; }
    }
}
