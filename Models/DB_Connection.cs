using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos;

namespace SQUARE_API.Models
{
    public class DB_Connection
    {
        readonly string _endpoint ="https://ciem.documents.azure.com:443/";
        readonly string _key ="B5SkUYL5BLm9RnT5kPgU12itHZFjxZ3Q1VtvqbPn3LPOIlWy5r6fiuXdDQCvlSFPlo8ME7OtdE99ACDbrF8wMg==";
        public Database Database { get; set; }
        public CosmosClient Client { get; set; }
        public DB_Connection(){
        this.Client = new(_endpoint, _key); //initialiserer Cosmos Client
        this.Database = this.Client.GetDatabase("ciem"); //kobler til riktig database
        }

    }
}