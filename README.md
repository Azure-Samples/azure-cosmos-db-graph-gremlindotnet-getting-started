# Getting started with Azure Cosmos DB: Graph API
Azure Cosmos DB is a globally distributed, multi-model database for mission critical applications. Azure Cosmos DB provides the Graph API for applications that need to model, query, and traverse large graphs efficiently using the Gremlin standard.

## About this sample: Using the Gremlin.Net open-source connector with Cosmos DB and its advantages.

This sample uses the open-source [Gremlin.Net driver](https://github.com/apache/tinkerpop/tree/master/gremlin-dotnet) to connect to an Azure Cosmos DB Graph API account and run some basic Create, Read, Update, Delete Gremlin queries. 

## Getting Started

### Prerequisites

The only dependency is the [Gremlin.Net driver](https://www.nuget.org/packages/Gremlin.Net/3.4.0-rc2) version `3.4.0-rc2`, which you can install with the following instructions:

- Using .NET CLI:

    ```
    dotnet add package Gremlin.Net
    ```

- Using Powershell Package Manager:

    ```
    Install-Package Gremlin.Net -Version 3.4.0-rc2
    ```

- For *.NET CORE* use the `nuget` [command-line utility](https://docs.microsoft.com/en-us/nuget/install-nuget-client-tools):

    ```
    nuget install Gremlin.Net
    ```

## Code
The following dictionary, under `Program.cs`, includes all the Gremlin queries that will be executed serially:
```cs
Dictionary<string, string> gremlinQueries = new Dictionary<string, string>
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
    { "Traverse",       "g.V('thomas').outE('knows').inV().hasLabel('person')" },
    { "Traverse 2x",    "g.V('thomas').outE('knows').inV().hasLabel('person').outE('knows').inV().hasLabel('person')" },
    { "Loop",           "g.V('thomas').repeat(out()).until(has('id', 'robin')).path()" },
    { "DropEdge",       "g.V('thomas').outE('knows').where(inV().has('id', 'mary')).drop()" },
    { "CountEdges",     "g.E().count()" },
    { "DropVertex",     "g.V('thomas').drop()" },
};
```

## Graph database support overview
[Azure Cosmos DB](http://cosmosdb.com) provides you with a fully-managed graph database service with global distribution, elastic scaling of storage and throughput, automatic indexing and query, tunable consistency levels, and supports the Gremlin standard. It also provides the ability to use multiple models like document and graph over the same data. For example, you can use a document collection to store graph data side by side with documents, and use both SQL queries over JSON and Gremlin queries to query the collection. For Graph API, this documentation page lists all the supported Gremlin steps: [Azure Cosmos DB Gremlin Graph Support](https://docs.microsoft.com/en-us/azure/cosmos-db/gremlin-support).
