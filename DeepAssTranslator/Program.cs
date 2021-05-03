using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace DeepAssTranslator
{
    class Program
    {
        static async Task Main()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("DeepAssTranslator");
            Console.WriteLine("-----------------");
            Console.ForegroundColor = ConsoleColor.White;

            List<string> subtitleTextFilePaths = SubtitleFilePathsLoader.GetSubtitleTextFilePaths();

            foreach (string subtitleTextFilePath in subtitleTextFilePaths)
            {
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Subtitle file: {subtitleTextFilePath} will be translated, please wait...");

                string[] subtitleTextLines = File.ReadAllLines(subtitleTextFilePath);
                List<Translation> translations = AssParser.GetTextsForTranslate(subtitleTextLines);
                await TranslationService.TranslateTextsAsync(translations);
                AssParser.ProcessTags(translations);

                foreach (Translation translation in translations)
                {
                    subtitleTextLines[translation.LineNumber] = subtitleTextLines[translation.LineNumber].Replace(translation.TextForTranslate, translation.TranslatedText);
                }

                string translatedSubtitleTextFilePath = subtitleTextFilePath.Replace(".ass", Configuration.TRANSLATED_FILE_SUFFIX);
                File.WriteAllLines(translatedSubtitleTextFilePath, subtitleTextLines);

                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"Subtitle file: {subtitleTextFilePath} was translated!");
                Console.WriteLine("-----------------------------------------------------");
            }

            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("Done! Press any key to exit...");
            Console.ReadKey();
        }
    }
}
