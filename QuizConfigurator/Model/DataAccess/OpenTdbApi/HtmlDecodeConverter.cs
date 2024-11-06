using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;

public class HtmlDecodeConverter : JsonConverter<string>
{
    public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        string? rawValue = reader.GetString();

        return CustomHtmlDecode(rawValue);
    }

    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }

    private string CustomHtmlDecode(string input)
    {
        if (string.IsNullOrEmpty(input))
            return input;

        input = Regex.Replace(input, @"&quot;", "\"");
        input = Regex.Replace(input, @"&apos;", "'");
        input = Regex.Replace(input, @"&amp;", "&");
        input = Regex.Replace(input, @"&lt;", "<");
        input = Regex.Replace(input, @"&gt;", ">");
        input = Regex.Replace(input, @"&ldquo;", "“");
        input = Regex.Replace(input, @"&rdquo;", "”");
        input = Regex.Replace(input, @"&hellip;", "…");
        input = Regex.Replace(input, @"&copy;", "©");
        input = Regex.Replace(input, @"&reg;", "®");
        input = Regex.Replace(input, @"&trade;", "™");
        input = Regex.Replace(input, @"&#(\d+);", match =>
        {
            int code = int.Parse(match.Groups[1].Value);
            return char.ConvertFromUtf32(code);
        });

        return input;
    }
}
