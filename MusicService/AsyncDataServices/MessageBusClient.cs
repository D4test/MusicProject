using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Configuration;
using MusicService.Dtos;
using RabbitMQ.Client;

namespace MusicService.AsyncDataServices
{
    public class MessageBusClient : IMessageBusClient
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public MessageBusClient(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "trigger1", type: ExchangeType.Topic);
                DeclareExchange();

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Підключено до MessageBus");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Не вдалося підключитися до Message Bus: {ex.Message}");
            }
        }

        public void PublishNewMusic(MusicPublishedDto musicPublishedDto)
        {
            var message = JsonSerializer.Serialize(musicPublishedDto);

            if (_connection.IsOpen)
            {
                Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                SendMessage(message);
            }
            else
            {
                Console.WriteLine("--> RabbitMQ connectionis closed, not sending");
            }   
        }

        private void DeclareExchange()
        {
            try
            {
                var exchangeName = "my_exchange";
                var exchangeType = "topic"; 
    
                
                _channel.ExchangeDeclare(exchange: exchangeName, type: exchangeType);
    
                Console.WriteLine($"Exchange '{exchangeName}' declared successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error declaring exchange: {ex.Message}");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger1",
                            routingKey: "music.*",
                            basicProperties: null,
                            body: body);
            Console.WriteLine($"--> We have sent {message}");
        }

        public void Dispose()
        {
            Console.WriteLine("MessageBus Disposed");
            if (_channel.IsOpen)
            {
                _channel.Close();
                _connection.Close();
            }
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}