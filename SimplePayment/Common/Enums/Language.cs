namespace SimplePayment.Common.Enums
{
    public class Language
    {
        public string LanguageText;

        public Language(string language)
        {
            LanguageText = language;
        }

        public static Language HU => new Language("hu");
        public static Language EN = new Language("en");
    }
}