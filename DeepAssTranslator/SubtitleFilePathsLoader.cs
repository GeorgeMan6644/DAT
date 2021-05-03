using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DeepAssTranslator
{
    public static class SubtitleFilePathsLoader
    {
        public static List<string> GetSubtitleTextFilePaths()
        {
            string executableFolder = Configuration.GetBasePath();
            List<string> subtitleTextFilePaths = Directory.GetFiles(executableFolder, $"*.ass").ToList();

            List<string> alreadyTranslatedSubtitleTextFilePaths = new List<string>();
            foreach (string subtitleTextFilePath in subtitleTextFilePaths)
            {
                if (subtitleTextFilePath.EndsWith(Configuration.TRANSLATED_FILE_SUFFIX))
                {
                    continue;
                }

                if (subtitleTextFilePaths.Any(x => x == subtitleTextFilePath.Replace(".ass", Configuration.TRANSLATED_FILE_SUFFIX)))
                {
                    alreadyTranslatedSubtitleTextFilePaths.Add(subtitleTextFilePath);
                }
            }

            foreach (string alreadyTranslatedSubtitleTextFilePath in alreadyTranslatedSubtitleTextFilePaths)
            {
                subtitleTextFilePaths.Remove(alreadyTranslatedSubtitleTextFilePath);
            }

            subtitleTextFilePaths.RemoveAll(x => x.EndsWith(Configuration.TRANSLATED_FILE_SUFFIX));
            return subtitleTextFilePaths;
        }
    }
}
