using System;
using System.Collections.Generic;
using Agenda.Model;
using MongoDB.Driver;

namespace Agenda
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("conectando com servidor");
            //conectar ao servidor
            var client = new MongoClient("mongodb://localhost:27017");

            Console.WriteLine("conectando com banco de dados");
            //conectar no banco de dados
            var database = client.GetDatabase("Agenda");

            Console.WriteLine("obtendo conecao de contatos");
            //colecao aonde vai armazenar nosso objeto de pessoa
            IMongoCollection<Pessoa> colecao = database.GetCollection<Pessoa>("Pessoa");

            // Console.WriteLine("inserindo pessoa");
            // InserirPessoa(colecao);

            // Console.WriteLine("editar pessoa");
            // AtualizarPessoa(colecao);

            // Console.WriteLine("remover email pessoa");
            // RemoverColuna(colecao);

            // Console.WriteLine("remover pessoa");
            // ExcluirPessoa(colecao);

            ListarPessoas(colecao);

            Console.ReadKey();
        }

        private static void ListarPessoas(IMongoCollection<Pessoa> colecao)
        {
            var filtro = Builders<Pessoa>.Filter.Empty;
            var pessoas = colecao.Find<Pessoa>(filtro).ToList();//esse metodo pode ser tipado ou nao!

            pessoas.ForEach(x =>
            {
                Console.WriteLine($"nome: {x.Nome}, Telefone: {x.Telefone}, Ativo:{x.Ativo}, Email:{x.Email}");
                x.Enderecos.ForEach(e=> Console.WriteLine($"Cidade:{e.Cidade}, pais:{e.Pais}"));
            });
        }

        public static void InserirPessoa(IMongoCollection<Pessoa> colecao)
        {

            Pessoa p = new Pessoa()
            {
                Nome = "ricardo4",
                Telefone = "654613864",
                Email = "ricardo@email.com",
                Ativo = true,
                Enderecos = new List<Endereco>{
                    new Endereco{
                        Cidade = "rio de janeiro",
                        Pais = "brasil"
                    },
                    new Endereco{
                        Cidade = "sao paulo",
                        Pais = "brasil"
                    },
                    new Endereco{
                        Cidade = "minhas gerais",
                        Pais = "brasil"
                    },
                }
            };
            colecao.InsertOne(p);
        }

        public static void AtualizarPessoa(IMongoCollection<Pessoa> colecao)
        {
            //filtro
            //var filtro = Builders<Pessoa>.Filter.Where(x=> x.Nome.StartsWith("ricardo"));
            var filtro = Builders<Pessoa>.Filter.Empty;//esse filtro esta vazio para poder buscar todo os registros

            //auteracao
            var alteracao = Builders<Pessoa>.Update.Set(p => p.Ativo, true);//estou mudando para todos as pessoas ficarem com o campo ativo true

            colecao.UpdateMany(filtro, alteracao);//aquie que o update e execultado
        }

        private static void ExcluirPessoa(IMongoCollection<Pessoa> colecao)
        {
            var filtro = Builders<Pessoa>.Filter.Where(x => x.Nome.Contains("3"));
            colecao.DeleteMany(filtro);
        }

        public static void RemoverColuna(IMongoCollection<Pessoa> colecao)
        {
            //filtro
            var filtro = Builders<Pessoa>.Filter.Where(x => x.Nome.StartsWith("ricardo"));
            //var filtro = Builders<Pessoa>.Filter.Empty;//esse filtro esta vazio para poder buscar todo os registros

            //auteracao
            var alteracao = Builders<Pessoa>.Update.Unset(p => p.Email);//estou removendo a coluna email dos objetos

            colecao.UpdateMany(filtro, alteracao);//aquie que o update e execultado

        }
    }
}
