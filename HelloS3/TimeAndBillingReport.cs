using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

namespace HelloS3;

public class TimeAndBillingReport
{
    const string BucketName = "time-and-billing";

    private List<TimeRecord> Records { get; set; } = new();
    private readonly AmazonS3Client _client;

    public TimeAndBillingReport()
    {
        Console.WriteLine("Creating S3 client");

        AmazonS3Config config = new AmazonS3Config
        {
            RegionEndpoint = RegionEndpoint.USWest1
        };

        _client = new AmazonS3Client(config);
    }

    /// <summary>
    /// Read S3 timesheet CSV files from S3 and return as a TimeRecord collection.
    /// </summary>
    /// <returns></returns>
    public async Task ReadTimesheetsFromBucket()
    {
        Records = new List<TimeRecord>();

        Console.WriteLine("Listing objects in bucket");

        var request = new ListObjectsRequest
        {
            BucketName = BucketName,
        };

        ListObjectsResponse result;

        do
        {
            result = await _client.ListObjectsAsync(request);
            foreach (var o in result.S3Objects)
            {
                if (!o.Key.Contains("timesheet", StringComparison.OrdinalIgnoreCase)) 
                    continue;
                
                var response = await _client.GetObjectAsync(BucketName, o.Key);
                var reader = new StreamReader(response.ResponseStream);
                var content = await reader.ReadToEndAsync();
                await File.WriteAllTextAsync(o.Key, content);
                Records.AddRange(TimeRecord.ReadCsvFile(o.Key));
                File.Delete(o.Key);
            }
        }
        while (result.IsTruncated);
    }

    /// <summary>
    /// Generate monthly time and billing report. Creates file billing-report-[year]-[month].csv and uploaded to S3 bucket.
    /// </summary>
    /// <returns></returns>
    public async Task GenerateReport()
    {

        foreach (var record in Records)
        {
            Console.WriteLine($"date:{record.Date:yyyy-MM-dd} name:{record.LastName},{record.FirstName} hours:{record.Hours} client:{record.Client} project:{record.Project}");
        }

        var filename = $"billing-report-{DateTime.Today.Year}-{DateTime.Today.Month}.csv";

        Console.WriteLine("Creating " + filename);

        var sortedRecords = Records.OrderBy(r => r.Date).ThenBy(r => r.LastName).ThenBy(r => r.FirstName);

        TimeRecord.WriteCsvFile(filename, sortedRecords.ToList());

        await _client.PutObjectAsync(new PutObjectRequest { BucketName = BucketName, Key = filename, ContentBody = await File.ReadAllTextAsync(filename) });

        File.Delete(filename);
    }

}