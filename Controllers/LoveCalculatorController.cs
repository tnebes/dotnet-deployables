#region

using dotnet_deployables.Util;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

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
            if (string.IsNullOrWhiteSpace(name1) || string.IsNullOrWhiteSpace(name2))
            {
                return BadRequest("Both names must be non-empty.");
            }

            string[] letterOccurrence = CalculateLetterOccurrence(name1, name2);
            int result = Reduce(letterOccurrence);
            return Ok(new { result });
        }
        catch (Exception ex)
        {
            return BadRequest("An error occurred while processing your request.");
        }
    }

    private int Reduce(string[] letterOccurrence)
    {
        string[] digits = letterOccurrence
            .SelectMany(num => num.ToCharArray()
            .Select(c => c.ToString()))
            .ToArray();

        if (digits.Length > 2)
        {
            List<string> reducedList = new List<string>();
            int length = digits.Length;

            for (int i = 0; i < length / 2; i++)
            {
                int first = int.Parse(digits[i]);
                int last = int.Parse(digits[length - 1 - i]);
                int sum = first + last;

                foreach (char digit in sum.ToString())
                {
                    reducedList.Add(digit.ToString());
                }
            }

            if (length % 2 != 0)
            {
                reducedList.Add(digits[length / 2]);
            }

            return Reduce(reducedList.ToArray());
        }

        return int.Parse(digits[0] + digits[1]);
    }


    private string[] CalculateLetterOccurrence(string name1, string name2)
    {
        string combinedNames = name1.ToLower() + name2.ToLower();
        string[] numbers = new string[combinedNames.Length];

        for (int i = 0; i < combinedNames.Length; i++)
        {
            int count = 0;
            for (int j = 0; j < combinedNames.Length; j++)
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
