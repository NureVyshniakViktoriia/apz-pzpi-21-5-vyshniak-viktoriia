using System;
using System.Collections.Generic;
using System.Linq;

namespace WebAPI.Infrastructure.Extensions;
public static class GuidExtensions
{
    private static readonly Random random = new((int)(DateTime.Now.Ticks % 1900001));
    private static string symbols => "qwertyuiopasdfghjklzxcvbnm1234567890";
    private static HashSet<int> guidIndexes => new() { 8, 13, 18, 23 };

    public static string EncodeToken(this Guid guid)
    {
        return new string(guid.ToString()
            .Select((x, i) => guidIndexes.Contains(i)
                ? symbols[random.Next(0, symbols.Length)]
                : x
            )
            .ToArray()
        );
    }

    public static Guid DecodeToken(this string guid)
    {
        return Guid.Parse(guid
            .Select((x, i) => guidIndexes.Contains(i)
                ? '-'
                : x
            )
            .ToArray()
        );
    }
}
