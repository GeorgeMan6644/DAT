namespace DeepAssTranslator
{
    public class Translation
    {
        public int LineNumber;
        public string TextForTranslate;
        public string TranslatedText;

        public Translation(int lineNumber, string textForTranslate)
        {
            LineNumber = lineNumber;
            TextForTranslate = textForTranslate;
        }
    }
}
