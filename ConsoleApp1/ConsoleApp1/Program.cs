using System;
using redis = StackExchange.Redis;

namespace ConsoleApp1
{
    static class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            //PublishSubscriver();
            Trabalho();

            Console.ReadKey();

        }

        private static void Trabalho()
        {

            var connection = redis.ConnectionMultiplexer.Connect("40.77.24.62");

            var pub = connection.GetSubscriber();
            var db = connection.GetDatabase();
            //pub.Subscribe("Perguntas", (canal, mensagem) =>
            //{

            //    Console.WriteLine(mensagem.ToString());
            //});

            var perguntas = pub.Subscribe("Perguntas");

            //perguntas.OnMessage(x => {

            //    Console.WriteLine(x.Message.ToString());
            //});

            pub.Subscribe("Perguntas", (ch, msg ) =>
            {
                var numeroPergunta = msg.ToString().Substring(0, msg.ToString().IndexOf(":"));
                db.HashSet(numeroPergunta, "FundoDireito", "Uma mensagem doidona");
                Console.WriteLine(msg.ToString());
            });

        }

        private static void PublishSubscriver()
        {
            var connection = redis.ConnectionMultiplexer.Connect("localhost");

            var pub = connection.GetSubscriber();

            Console.WriteLine("Informar a mensagem");
            pub.Publish("net15", Console.ReadLine());

            Console.WriteLine("Deseja enviar uma nova mensagem? S / N");
            var retorno = Console.ReadLine();

            if ("S".Equals(retorno, StringComparison.OrdinalIgnoreCase))
            {
                PublishSubscriver();
            }
        }

        private static void AdicionandoEmCache()
        {

            var connection = redis.ConnectionMultiplexer.Connect("localhost");
            var db = connection.GetDatabase();

            var adicionando = db.StringSet("Numero1", "Uma lista numero 1");

            var recebendo = db.StringGet("Numero1");

            Console.WriteLine(recebendo);
        }
    }
}
