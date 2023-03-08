using System.Collections.Generic;

namespace WinWarGame.Platform;

public static class CommandLine
{
    public static Dictionary<string, string> ParseKeyValuePairs(string text)
    {
        Dictionary<string, string> result = new Dictionary<string, string>();

        string currentKey = string.Empty;
        string currentValue = string.Empty;
        bool isInString = false;
        bool isInKey = true;
        for (int i = 0; i < text.Length; i++)
        {
            char c = text[i];

            if (c == '\"')
            {
                currentValue += c;
                if (isInString)
                {
                    isInString = false;
                }
                else
                {
                    isInString = true;
                }
                continue;
            }
            else if (c == ' ')
            {
                if (isInString == false)
                {
                    result[currentKey] = currentValue;
                    currentKey = string.Empty;
                    currentValue = string.Empty;
                    isInKey = true;
                }
                else
                {
                    currentValue += c;
                }
            }
            else if (c == '=')
            {
                if (isInString == false)
                {
                    isInKey = false;
                    continue;
                }
                else
                {
                    currentValue += c;
                }
            }
            else
            {
                if (isInKey)
                {
                    currentKey += c;
                }
                else
                {
                    currentValue += c;
                }
            }
        }

        if (string.IsNullOrWhiteSpace(currentKey) == false)
        {
            result[currentKey] = currentValue;
        }

        return result;
    }
}