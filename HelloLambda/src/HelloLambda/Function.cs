using Amazon.Lambda.Core;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace HelloLambda;

public class Function
{
    /// <summary>
    /// Returns the letter combinations for a phone number.
    /// </summary>
    /// <param name="digits">digits-only string</param>
    /// <param name="context">ILambdaContext</param>
    /// <returns>string array of letter combinations</returns>
    public IEnumerable<string> FunctionHandler(string digits, ILambdaContext context)
    {
        if (string.IsNullOrEmpty(digits) || digits.Any(c => c is < '0' or > '9'))
            return new List<string>();

        string[] phone = {"0", "1", "ABC", "DEF", "GHI", "JKL", "MNO", "PQRS", "TUV", "WXYZ"};
        return digits.Skip(1)
            .Select(d => d - '0')
            .Aggregate(phone[digits[0] - '0']
            .Select(c => c.ToString()), (acc, i) => phone[i].SelectMany(c => acc.Select(a => $"{a}{c}")));
    }
}