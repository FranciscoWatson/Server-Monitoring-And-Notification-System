using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Driver;
using MessageProcessingService.Models;
using Microsoft.Extensions.Options;

namespace MessageProcessingService.Repository
{
    public class MongoDbServerStatisticsRepository : IServerStatisticsRepository
    {

        private readonly IMongoCollection<ServerStatistics> _serverStatisticsCollection;

        public MongoDbServerStatisticsRepository(string connectionString)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase("MessageProcessingService");
            _serverStatisticsCollection = database.GetCollection<ServerStatistics>("ServerStatistics");
        }

        public async Task InsertAsync(ServerStatistics serverStatistics)
        {
            _serverStatisticsCollection.InsertOneAsync(serverStatistics);
        }

    }

}