using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace WinWarCS.Graphics;

public class Font
{
    private readonly SpriteFont spriteFont;
    private readonly Dictionary<char, SpriteFont.Glyph> glyphs;

    public int LineSpacing => spriteFont.LineSpacing;

    public float Spacing
    {
        get { return spriteFont.Spacing; }
        set { spriteFont.Spacing = value; }
    }

    public Font(SpriteFont setUnderlyingFont)
    {
        spriteFont = setUnderlyingFont;
        glyphs = spriteFont.GetGlyphs();
    }

    public Vector2 MeasureString(string str)
    {
        return spriteFont.MeasureString(str);
    }

    public Vector2 MeasureChar(char c)
    {
        if (glyphs.ContainsKey(c) == false)
        {
            return Vector2.Zero;
        }

        var glyph = glyphs[c];
        return new Vector2(glyph.Width, glyph.BoundsInTexture.Height);
    }
    
    /// <summary>
    /// Line breaks a single non-whitespace containing string of characters depending on the maxWidth  
    /// </summary>
    public string[] WrapWord(string word, float maxWidth)
    {
        // NOTE: Implemented using a relatively slow approach
        // TODO: Improve

        const float indent = 0.9f;
        
        var size = MeasureString(word);
        var numLines = (int)(size.X / maxWidth * indent) + 1;
        if (numLines == 0)
        {
            numLines = 1;
        }

        int curLine = 0;
        string[] result = new string[numLines];
        for (int i = 0; i < word.Length; i++)
        {
            float lw = MeasureString(result[curLine] + word[i]).X;
            if (lw > maxWidth)
            {
                curLine++;
                result[curLine] += word[i];
            }
            else
            {
                result[curLine] += word[i];
            }
        }
        return result;
    }

    /// <summary>
    /// Breaks a text along whitespace characters into multiple lines, optionally supporting breaking
    /// longer-than-possible words into multiple lines
    /// </summary>
    public string[] WrapLines(string fullText, float maxWidth, bool wrapInline)
    {
        List<string> result = new List<string>();

        string[] paragraphs = fullText.Split("\r\n");

        foreach (string p in paragraphs)
        {
            string[] words = p.Split(' ');
            string curLine = "";
            float lineWidth = 0.0f;
            float spaceWidth = MeasureString(" ").X;

            for (int i = 0; i < words.Length; i++)
            {
                Vector2 size = MeasureString(words[i]);
                if (size.X > maxWidth && wrapInline)
                {
                    result.Add(curLine);
                    
                    string[] extras = WrapWord(words[i], maxWidth);
                    result.AddRange(extras);
                    lineWidth = 0.0f;
                    curLine = "";
                    continue;
                }
                if (lineWidth > 0.0f)
                {
                    if (lineWidth + size.X > maxWidth)
                    {
                        result.Add(curLine);
                        lineWidth = 0.0f;
                        curLine = "";
                    }
                }

                lineWidth += size.X + spaceWidth;
                curLine += words[i] + " ";
            }

            if (string.IsNullOrWhiteSpace(curLine) == false)
            {
                result.Add(curLine);
            }
        }

        return result.ToArray();
    }

    public void DrawIntoSpriteBatch(SpriteBatch spriteBatch, string text, Vector2 position, Color color, int rotation, 
        Vector2 origin, Vector2 scale, SpriteEffects effects, float layerDepth)
    {
        spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, scale, effects, layerDepth);
    }
}