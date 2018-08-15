using System.Collections.Generic;
using MongoDB.Bson;

namespace Agenda.Model
{
    public class Pessoa
    {
        public ObjectId Id { get; set; }
        public string Nome { get; set; }
        public string Telefone { get; set; }
        public string Email { get; set; }
        public bool Ativo { get; set; }

        public List<Endereco> Enderecos { get; set; }
    }
}