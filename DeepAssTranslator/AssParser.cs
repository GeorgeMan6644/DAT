using System.Collections.Generic;

namespace DeepAssTranslator
{
    public static class AssParser
    {
        public static List<Translation> GetTextsForTranslate(string[] subtitleTextLines)
        {
            List<Translation> textsForTranslate = new List<Translation>();

            for (int i = 0; i < subtitleTextLines.Length; i++)
            {
                if (!GetTextForTranslate(subtitleTextLines[i], out string textForTranslate))
                {
                    continue;
                }

                textsForTranslate.Add(new Translation(i, textForTranslate));
            }

            return textsForTranslate;
        }

        public static void ProcessTags(List<Translation> translations)
        {
            foreach (Translation translation in translations)
            {
                translation.TextForTranslate = translation.TextForTranslate.Replace($"<ctrl1>{Configuration.NEW_LINE}</ctrl1>", Configuration.NEW_LINE);
                translation.TranslatedText = translation.TranslatedText.Replace($"<ctrl1>{Configuration.NEW_LINE}</ctrl1>", Configuration.NEW_LINE);
            }
        }

        private static bool GetTextForTranslate(string subtitleTextLine, out string textForTranslate)
        {
            textForTranslate = string.Empty;
            if (!subtitleTextLine.StartsWith("Dialogue:"))
            {
                return false;
            }

            textForTranslate = subtitleTextLine.Substring(subtitleTextLine.LastIndexOf(",,") + 2);
            if (string.IsNullOrEmpty(textForTranslate))
            {
                return false;
            }

            textForTranslate = textForTranslate.Replace(Configuration.NEW_LINE, $"<ctrl1>{Configuration.NEW_LINE}</ctrl1>");
            return true;
        }
    }
}
