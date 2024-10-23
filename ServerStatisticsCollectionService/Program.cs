using Microsoft.Extensions.Configuration;
using ServerStatisticsCollectionService.Factories;



var configuration = new ConfigurationBuilder()
.SetBasePath(Directory.GetCurrentDirectory())
.AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
.AddEnvironmentVariables()
.Build();

var messageQueuePublisherFactory = new MessageQueuePublisherFactory();
var messageQueuePublisher = messageQueuePublisherFactory.Create(configuration);


var serverStatisticsCollectorFactory = new ServerStatisticsCollectorFactory();
var statisticsCollectionService = serverStatisticsCollectorFactory.Create(configuration, messageQueuePublisher);


statisticsCollectionService.StartAsync();

await Task.Run(() => Thread.Sleep(Timeout.Infinite));