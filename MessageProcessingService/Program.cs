﻿using MessageProcessingService;
using MessageProcessingService.Messaging;
using MessageProcessingService.Models;
using MessageProcessingService.Repository;
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
var rabbitMqConsumer = new RabbitMqConsumer(hostName, port, userName, password, mongoDbServerStatisticsRepository);

rabbitMqConsumer.Consume("ServerStatistics.*");


Console.ReadKey();