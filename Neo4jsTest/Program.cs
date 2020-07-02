using Neo4j.Driver;
using Neo4jClient;
using System;
using System.Linq;

namespace Neo4jsTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var hwe = new HelloWorldExample("bolt://localhost:7687", "neo4j", "neo4j");
            hwe.PrintGreening("Hello world"); 

            //var client = new GraphClient(new Uri("http://localhost/db/data"));
            //client.Connect();
            //var tathamQuery = client.Cypher
            //    .Start("root", client.RootNode)
            //    .Match("root-[:HAS_USER]->user")
            //    .Where((User user) => user.Name == "Tatham")
            //    .Return<Node<User>>("user");

            //var tahtam = tathamQuery
            //    .Results
            //    .Single();

            //Console.ReadLine();


        }
    }

    public class User
    {
        public string Name { get; set; }
    }

    public class Activity
    {
        public string Name { get; set; }
    }

    public class HelloWorldExample : IDisposable
    {
        private readonly IDriver _driver;

        public HelloWorldExample(string uri, string userName, string password)
        {
            _driver = GraphDatabase.Driver(uri, AuthTokens.Basic(userName, password));
        }
        public void PrintGreening(string message)
        {
            using var session = _driver.Session();
            var greeting = session.WriteTransaction(tx =>
            {
                var result = tx.Run("CREATE (a:Greeting) " +
                    "SET a.message = $message " +
                    "RETURN a.message + ',from node ' + id(a)",
                    new { message });
                return result.Single()[0].As<string>();
            });
            Console.WriteLine(greeting);
        }
        public void Dispose()
        {
            _driver.Dispose();
        }
    }
}
