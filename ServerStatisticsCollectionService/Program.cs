using Microsoft.Extensions.Configuration;
using ServerStatisticsCollectionService.Factories;


var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json")
.Build();

var messageQueuePublisherFactory = new MessageQueuePublisherFactory();
var messageQueuePublisher = messageQueuePublisherFactory.Create(configuration);


var serverStatisticsCollectorFactory = new ServerStatisticsCollectorFactory();
var statisticsCollectionService = serverStatisticsCollectorFactory.Create(configuration, messageQueuePublisher);


statisticsCollectionService.StartAsync();

Console.ReadKey();
