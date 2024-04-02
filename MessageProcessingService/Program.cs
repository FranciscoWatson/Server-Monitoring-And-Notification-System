using MessageProcessingService.Models;
using MessageProcessingService.Repository;
using MessageProcessingService.Services;
using Microsoft.Extensions.Configuration;
using MessagingQueueLibrary.Models;
using MessagingQueueLibrary.Consumer;


var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.Build();

var connectionString = configuration.GetSection("ConnectionStrings")["MongoDBConnection"];
var rabbitMQConfig = configuration.GetSection("RabbitMQConfig").Get<RabbitMqConfig>();
var signalRConfig = configuration.GetSection("SignalRConfig").Get<SignalRConfig>();
var anomalyDetectionConfig = configuration.GetSection("AnomalyDetectionConfig").Get<AnomalyDetectionConfig>();


var mongoDbServerStatisticsRepository = new MongoDbServerStatisticsRepository(connectionString);

var anomalyDetection = new AnomalyDetection(signalRConfig, anomalyDetectionConfig);

await anomalyDetection.StartAsync();

var rabbitMqConsumer = new RabbitMqConsumer(rabbitMQConfig);

var processingService = new ProcessingService(mongoDbServerStatisticsRepository, anomalyDetection, rabbitMqConsumer);

await processingService.ProcessMessageAsync();


Console.ReadKey();