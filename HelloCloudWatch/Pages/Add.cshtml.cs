using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HelloCloudWatch.Pages;

public class AddModel : PageModel
{
    private readonly ILogger<AddModel> _logger;
    public int N1 { get; set; }
    public int N2 { get; set; }
    public int Sum { get; set; }

    public AddModel(ILogger<AddModel> logger)
    {
        _logger = logger;
    }

    public void OnGet(int n1, int n2)
    {
        _logger.LogInformation("Add n1={N1}, n2={N2}", n1, n2);
        N1 = n1;
        N2 = n2;
        Sum = n1 + n2;
        _logger.LogInformation("Sum: {Sum}", Sum);
    }
}