using System.Net;
using System.Text.Json;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace HelloSQS_Fulfill
{
    internal static class Program
    {
        private const string OrderQueue = "orders";
        private static AmazonSQSClient _client = null!;
        private static string _queueUrl = null!;

        private static async Task Main(string[] args)
        {
            Console.WriteLine("Connecting to SQS");

            var config = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };
            _client = new AmazonSQSClient(config);

            _queueUrl = await GetOrCreateQueue();
            await ProcessOrders();
        }

        // Create orders queue if it doesn't exist, and return queue URL.

        private static async Task<string> GetOrCreateQueue()
        {
            string url;
            try
            {
                var getQueueUrlResponse = await _client.GetQueueUrlAsync(OrderQueue);
                url = getQueueUrlResponse.QueueUrl;
                Console.WriteLine("Orders queue exists");
            }
            catch (QueueDoesNotExistException)
            {
                Console.WriteLine("Creating orders queue");
                var createQueueRequest = new CreateQueueRequest
                {
                    QueueName = OrderQueue
                };
                var createQueueResponse = await _client.CreateQueueAsync(createQueueRequest);
                url = createQueueResponse.QueueUrl;
            }
            return url;
        }

        private static async Task ProcessOrders()
        {
            Console.WriteLine("Listening for messages");

            var request = new ReceiveMessageRequest
            {
                QueueUrl = _queueUrl,
                MaxNumberOfMessages = 10,
                WaitTimeSeconds = 10
            };

            while (true)
            {
                var response = await _client.ReceiveMessageAsync(request);

                if (response.HttpStatusCode == HttpStatusCode.OK)
                {
                    var messages = response.Messages;
                    if (messages.Count > 0)
                    {
                        Console.WriteLine($"{messages.Count} messages received");
                        foreach (var msg in messages)
                        {
                            try
                            {
                                Console.WriteLine(msg.Body);
                                var order = JsonSerializer.Deserialize<Order>(msg.Body);
                                Console.WriteLine($"Order {order?.Id}, {order?.Items.Count} items");
                            }
                            catch (InvalidOperationException ex)
                            {
                                Console.WriteLine($"Exception deserializing message: {ex}");
                            }
                        }

                        Console.WriteLine("Deleting queue messages");
                        foreach (var msg in messages)
                        {
                            await _client.DeleteMessageAsync(_queueUrl, msg.ReceiptHandle);
                        }
                    }
                }
                else
                {
                    Console.WriteLine($"HTTP status {response.HttpStatusCode}");
                }
            }
        }

    }

    public class Order
    {
        public string Id { get; set; } = null!;
        public List<string> Items { get; set; } = null!;

        public Order() { }
        public Order(string id, string[] items)
        {
            Id = id;
            Items = new List<string>(items);
        }
    }
}