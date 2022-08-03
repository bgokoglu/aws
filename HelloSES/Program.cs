using Amazon;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;

if (args.Length < 3)
{
    Console.WriteLine("Usage: dotnet run -- [destination-address] [access-key] [secret-access-key]");
    Environment.Exit(0);
}

var region = RegionEndpoint.USEast1;

string source = "your-email-address@somewhere.com";

// Get recipient, access key, and secret key from the command line
string recipient = args[0];
var awsAccessKey = args[1];
var awsSecretKey = args[2];

// Send the email to the recipients via Amazon SES.
using var client = new AmazonSimpleEmailServiceClient(awsAccessKey, awsSecretKey, region);

// Create a send email message request
var dest = new Destination(new List<string> { recipient });
var subject = new Content("Testing Amazon SES through the API");
var textBody = new Content("This is a test message sent by Amazon SES from the AWS SDK for .NET.");
var body = new Body(textBody);
var message = new Message(subject, body);
var request = new SendEmailRequest(source, dest, message);

// Send the email to recipients via Amazon SES
Console.WriteLine($"Sending message to {source}");
var response = await client.SendEmailAsync(request);
Console.WriteLine($"Response - HttpStatusCode: {response.HttpStatusCode}, MessageId: {response.MessageId}");