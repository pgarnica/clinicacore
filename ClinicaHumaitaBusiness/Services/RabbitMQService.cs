using ClinicaHumaita.Business.Interfaces;
using ClinicaHumaita.Data.Interfaces;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace ClinicaHumaita.Services
{

    public class RabbitMQService : IRabbitMQService
    {
        private readonly IConfiguration _configuration;

        private readonly string _hostname;
        private readonly string _password;
        private readonly string _queueName;
        private readonly string _username;
        private IConnection _connection;

        public RabbitMQService(IConfiguration configuration)
        {
            _configuration = configuration;
            _hostname = _configuration["RabbitMQSettings:hostName"];
            _password = _configuration["RabbitMQSettings:password"];
            _queueName = "log";
            _username = _configuration["RabbitMQSettings:userName"];

        }

 
        public async Task Send(string message)
        {
            try
            {
                if (ConnectionExists())
                {
                    using (var channel = _connection.CreateModel())
                    {
                        channel.QueueDeclare(queue: _queueName, durable: false, exclusive: false, autoDelete: false, arguments: null);

                        var json = JsonConvert.SerializeObject(message);
                        var body = Encoding.UTF8.GetBytes(json);

                        channel.BasicPublish(exchange: "", routingKey: _queueName, basicProperties: null, body: body);
                    }
                }

            }
            catch(Exception ex) 
            {
                throw ex;
            }
        }

        private void CreateConnection()
        {
            try
            {
                var factory = new ConnectionFactory
                {
                    HostName = _hostname,
                    UserName = _username,
                    Password = _password
                };
                _connection = factory.CreateConnection();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Could not create connection: {ex.Message}");
            }
        }

        private bool ConnectionExists()
        {
            if (_connection != null)
            {
                return true;
            }

            CreateConnection();

            return _connection != null;
        }

    }
}
