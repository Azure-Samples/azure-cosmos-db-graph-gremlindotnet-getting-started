using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gremlin.Net;
using Gremlin.Net.Driver;
using Newtonsoft.Json;

namespace GremlinNetSample
{
    /// <summary>
    /// Sample program that shows how to get started with the Graph (Gremlin) APIs for Azure Cosmos DB using the open-source connector Gremlin.Net
    /// </summary>
    class Program
    {
        // Azure Cosmos DB Configuration variables
        // Replace the values in these variables to your own.
        private static string hostname = "your-endpoint.gremlin.cosmosdb.azure.com";
        private static int port = 443;
        private static string authKey = "your-authentication-key";
        private static string database = "your-database";
        private static string collection = "your-collection-or-graph";

        // Gremlin queries that will be executed.
        private static Dictionary<string, string> gremlinQueries = new Dictionary<string, string>
            {
                { "Cleanup",        "g.V().drop()" },
                { "AddVertex 1",    "g.addV('person').property('id', 'thomas').property('firstName', 'Thomas').property('age', 44)" },
                { "AddVertex 2",    "g.addV('person').property('id', 'mary').property('firstName', 'Mary').property('lastName', 'Andersen').property('age', 39)" },
                { "AddVertex 3",    "g.addV('person').property('id', 'ben').property('firstName', 'Ben').property('lastName', 'Miller')" },
                { "AddVertex 4",    "g.addV('person').property('id', 'robin').property('firstName', 'Robin').property('lastName', 'Wakefield')" },
                { "AddEdge 1",      "g.V('thomas').addE('knows').to(g.V('mary'))" },
                { "AddEdge 2",      "g.V('thomas').addE('knows').to(g.V('ben'))" },
                { "AddEdge 3",      "g.V('ben').addE('knows').to(g.V('robin'))" },
                { "UpdateVertex",   "g.V('thomas').property('age', 44)" },
                { "CountVertices",  "g.V().count()" },
                { "Filter Range",   "g.V().hasLabel('person').has('age', gt(40))" },
                { "Project",        "g.V().hasLabel('person').values('firstName')" },
                { "Sort",           "g.V().hasLabel('person').order().by('firstName', decr)" },
                { "Traverse",       "g.V('thomas').out('knows').hasLabel('person')" },
                { "Traverse 2x",    "g.V('thomas').out('knows').hasLabel('person').out('knows').hasLabel('person')" },
                { "Loop",           "g.V('thomas').repeat(out()).until(has('id', 'robin')).path()" },
                { "DropEdge",       "g.V('thomas').outE('knows').where(inV().has('id', 'mary')).drop()" },
                { "CountEdges",     "g.E().count()" },
                { "DropVertex",     "g.V('thomas').drop()" },
            };

        // Starts a console application that executes every Gremlin query in the gremlinQueries dictionary. 
        static void Main(string[] args)
        {
            var gremlinServer = new GremlinServer(hostname, port, enableSsl: true, 
                                                    username: "/dbs/" + database + "/colls/" + collection, 
                                                    password: authKey);

            using (var gremlinClient = new GremlinClient(gremlinServer))
            {
                foreach (var query in gremlinQueries)
                {
                    Console.WriteLine(String.Format("Running this query: {0}: {1}", query.Key, query.Value));

                    // Create async task to execute the Gremlin query.
                    var task = gremlinClient.SubmitAsync<dynamic>(query.Value);
                    task.Wait();

                    foreach (var result in task.Result)
                    {
                        // The vertex results are formed as Dictionaries with a nested dictionary for their properties
                        string output = JsonConvert.SerializeObject(result);
                        Console.WriteLine(String.Format("\tResult:\n\t{0}", output));
                    }
                    Console.WriteLine();
                }
            }

            // Exit program
            Console.WriteLine("Done. Press any key to exit...");
            Console.ReadLine();
        }
    }
}
