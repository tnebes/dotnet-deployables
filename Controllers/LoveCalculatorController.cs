#region

using dotnet_deployables.Util;
using Microsoft.AspNetCore.Mvc;

#endregion

namespace dotnet_deployables.Controllers;

[ApiController]
[Route("api/v1/love")]
[Produces("application/json")]
public sealed class LoveCalculatorController : ControllerBase
{
    [HttpGet]
    public IActionResult CalculateLove([FromQuery] string name1, [FromQuery] string name2)
    {
        try
        {
            if (string.IsNullOrEmpty(name1?.Trim()) || string.IsNullOrEmpty(name2?.Trim()))
            {
                return BadRequest("Both names must be non-empty.");
            }
            string[] letterOccurrence = this.CalculateLetterOccurrence(name1, name2);
            int result = this.reduce(letterOccurrence);
            return Ok(result);
        } catch (Exception e)
        {
            return BadRequest(e);
        }
    }

    private int reduce(string[] letterOccurrence)
    {
        if (letterOccurrence.Length > 2)
        {
            List<string> reducedList = new List<string>();
            int n = letterOccurrence.Length;
            for (int i = 0; i < n / 2; i++)
            {
                int first = int.Parse(letterOccurrence[i]);
                int last = int.Parse(letterOccurrence[n - 1 - i]);
                int sum = first + last;
                reducedList.Add(sum.ToString());
            }
            if (n % 2 != 0)
            {
                reducedList.Add(letterOccurrence[n / 2]);
            }
            return reduce(reducedList.ToArray());
        }
        return int.Parse(letterOccurrence[0] + letterOccurrence[1]);
    }

    private string[] CalculateLetterOccurrence(string name1, string name2)
    {
        string combinedNames = name1.ToLower() + name2.ToLower();
        string[] numbers = new string[combinedNames.Length];
        int count = 0;

        for (int i = 0; i < combinedNames.Length - 1; i++)
        {
            count = 0;
            for (int j = 0; j < combinedNames.Length - 1; j++)
            {
                if (combinedNames[i] == combinedNames[j])
                {
                    count++;
                }
            }
            numbers[i] = count.ToString();
        }

        return numbers;
    }
}