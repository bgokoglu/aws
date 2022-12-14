using System.Text.Json;
using Amazon;
using Amazon.SQS;
using Amazon.SQS.Model;

namespace helloSqs
{
    internal static class Program
    {
        private const string OrderQueue = "orders";

        private static AmazonSQSClient _client = null!;
        static string _queueUrl = null!;
        private static readonly Random Random = new();

        // Specify number of orders to create on the command line (default: 1)
        static async Task Main(string[] args)
        {
            var orderCount = args.Length > 0 ? Convert.ToInt32(args[0]) : 0;

            Console.WriteLine("Connecting to SQS");

            var config = new AmazonSQSConfig
            {
                RegionEndpoint = RegionEndpoint.USEast1
            };
            _client = new AmazonSQSClient(config);

            _queueUrl = await GetOrCreateQueue();

            if (orderCount > 0)
            {
                Console.WriteLine("Generating orders");
                for (int orderNo = 1; orderNo <= orderCount; orderNo++)
                {
                    var order = GenerateRandomOrder(orderNo.ToString());
                    Console.WriteLine($"Order {order?.Id}, {order?.Items.Count} items");
                    var message = JsonSerializer.Serialize(order);
                    Console.WriteLine(message);
                    await SendMessage(message);
                }
            }
        }

        // Create orders queue if it doesn't exist, and return queue URL.
        static async Task<string> GetOrCreateQueue()
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

        static async Task SendMessage(string message)
        {
            var sendMessageRequest = new SendMessageRequest
            {
                QueueUrl = _queueUrl,
                MessageBody = message
            };
            var sendMessageResponse = await _client.SendMessageAsync(sendMessageRequest);
            Console.WriteLine(sendMessageResponse.MessageId);
        }

        static Order GenerateRandomOrder(string id)
        {
            var items = new List<string>();
            for (int i = 0; i < Random.Next(5) + 1; i++)
            {
                switch (Random.Next(5))
                {
                    case 0:
                        items.Add("Widget");
                        break;
                    case 1:
                        items.Add("Sprocket");
                        break;
                    case 2:
                        items.Add("Gasket");
                        break;
                    case 3:
                        items.Add("Washer");
                        break;
                    case 4:
                        items.Add("Spring");
                        break;
                }
            }

            Order order = new Order
            {
                Id = id,
                Items = items
            };

            return order;
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

//Interesting issue
//https://stackoverflow.com/questions/62161664/unable-to-get-iam-security-credentials-for-aws-s3
//Had to create a default profile otherwise it would not work