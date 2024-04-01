using MessageProcessingService;
using MessageProcessingService.Messaging;
using MessageProcessingService.Models;
using MessageProcessingService.Repository;
using MessageProcessingService.Services;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.Build();

var hostName = configuration.GetSection("RabbitMQConfig")["HostName"];
var port = Convert.ToInt32(configuration.GetSection("RabbitMQConfig")["Port"]);
var userName = configuration.GetSection("RabbitMQConfig")["UserName"];
var password = configuration.GetSection("RabbitMQConfig")["Password"];

var connectionString = configuration.GetSection("ConnectionStrings")["MongoDBConnection"];



var mongoDbServerStatisticsRepository = new MongoDbServerStatisticsRepository(connectionString);

var signalRUrl = configuration.GetSection("SignalRConfig")["SignalRUrl"];
var anomalyDetection = new AnomalyDetection(signalRUrl, configuration);
await anomalyDetection.StartAsync();
var rabbitMqConsumer = new RabbitMqConsumer(hostName, port, userName, password, mongoDbServerStatisticsRepository, anomalyDetection);

rabbitMqConsumer.Consume("ServerStatistics.*");


Console.ReadKey();