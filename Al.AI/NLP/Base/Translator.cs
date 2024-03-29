// Copyright (c) 2010 Ravi Bhavnani
// License: Code Project Open License
// http://www.codeproject.com/info/cpol10.aspx

using System;
using System.Collections.Generic;
using RavSoft;

namespace Al.AI
{
    /// <summary>
    /// Translates text using Google's online language tools.
    /// </summary>
    public class Translator : WebResourceProvider
    {
        #region Constructor

            /// <summary>
            /// Initializes a new instance of the <see cref="Translator"/> class.
            /// </summary>
            public Translator()
            {
                this.SourceLanguage = "English";
                this.TargetLanguage = "French";
                this.Referer = "http://www.google.com";
            }

        #endregion

        #region Properties

            /// <summary>
            /// Gets or sets the source "
            /// </summary>
            /// <value>The source "</value>
            public string SourceLanguage {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the target "
            /// </summary>
            /// <value>The target "</value>
            public string TargetLanguage {
                get;
                set;
            }

            /// <summary>
            /// Gets or sets the source text.
            /// </summary>
            /// <value>The source text.</value>
            public string SourceText {
                get;
                set;
            }

            /// <summary>
            /// Gets the translation.
            /// </summary>
            /// <value>The translated text.</value>
            public string Translation {
                get;
                private set;
            }

            /// <summary>
            /// Gets the reverse translation.
            /// </summary>
            /// <value>The reverse translated text.</value>
            public string ReverseTranslation {
                get;
                private set;
            }

        #endregion        

        #region Public methods

            /// <summary>
            /// Attempts to translate the text.
            /// </summary>
            public void Translate()
            {
                // Validate source and target languages
                if (string.IsNullOrEmpty (this.SourceLanguage) ||
                    string.IsNullOrEmpty (this.TargetLanguage) ||
                    this.SourceLanguage.Trim().Equals (this.TargetLanguage.Trim())) {
                    throw new Exception ("An invalid source or target language was specified.");
                }

                // Delegate to base class
                this.fetchResource();
            }

        #endregion

        #region WebResourceProvider implementation

            /// <summary>
            /// Returns the url to be fetched.
            /// </summary>
            /// <returns>The url to be fetched.</returns>
            protected override string getFetchUrl()
            {
              return "http://translate.google.com/translate_t";
            }

            /// <summary>
            /// Retrieves the POST data (if any) to be sent to the url to be fetched.
            /// The data is returned as a string of the form "arg=val[&arg=val]...".
            /// </summary>
            /// <returns>A string containing the POST data or null if none.</returns>
            protected override string getPostData()
            {
              // Set translation mode
              string strPostData = string.Format ("hl=en&ie=UTF8&oe=UTF8submit=Translate&langpair={0}|{1}",
                                                   Translator.LanguageEnumToIdentifier (this.SourceLanguage),
                                                   Translator.LanguageEnumToIdentifier (this.TargetLanguage));

              // Set text to be translated
              strPostData += "&text=\"" + this.SourceText + "\"";
              return strPostData;
            }

            /// <summary>
            /// Parses the fetched content.
            /// </summary>
            protected override void parseContent()
            {
                // Initialize the scraper
                this.Translation = string.Empty;
                string strContent = this.Content;
                RavSoft.StringParser parser = new RavSoft.StringParser (strContent);

                // Scrape the translation
                string strTranslation = string.Empty;
                if (parser.skipToEndOf ("<span id=result_box")) {
                    if (parser.skipToEndOf ("onmouseout=\"this.style.backgroundColor='#fff'\">")) {
                        if (parser.extractTo("</span>", ref strTranslation)) {
                            strTranslation = StringParser.removeHtml (strTranslation);
                        }
                    }
                }

                #region Fix up the translation
                    int startClean = 0;
                    int endClean = 0;
                    int i=0;
                    while (i < strTranslation.Length) {
                        if (Char.IsLetterOrDigit (strTranslation[i])) {
                            startClean = i;
                            break;
                        }
                        i++;
                    }
                    i = strTranslation.Length - 1;
                    while (i > 0) {
                        char ch = strTranslation[i];
                        if (Char.IsLetterOrDigit (ch) ||
                            (Char.IsPunctuation (ch) && (ch != '\"'))) {
                            endClean = i;
                            break;
                        }
                        i--;
                    }
                    this.Translation = strTranslation.Substring (startClean, endClean - startClean + 1).Replace ("\"", "");
                #endregion
            }

        #endregion

        #region Private methods

            /// <summary>
            /// Converts a language to its identifier.
            /// </summary>
            /// <param name="language">The language."</param>
            /// <returns>The identifier or <see cref="string.Empty"/> if none.</returns>
            private static string LanguageEnumToIdentifier
                (string language)
            {
                if (Translator._languageModeMap == null) {
                    Translator._languageModeMap = new Dictionary<string,string>();
                    Translator._languageModeMap.Add ("Afrikaans",   "af");
                    Translator._languageModeMap.Add ("Albanian",    "sq");
                    Translator._languageModeMap.Add ("Arabic",      "ar");
                    Translator._languageModeMap.Add ("Belarusian",  "be");
                    Translator._languageModeMap.Add ("Bulgarian",   "bg");
                    Translator._languageModeMap.Add ("Catalan",     "ca");
                    Translator._languageModeMap.Add ("Chinese",     "zh-CN");
                    Translator._languageModeMap.Add ("Croatian",    "hr");
                    Translator._languageModeMap.Add ("Czech",       "cs");
                    Translator._languageModeMap.Add ("Danish",      "da");
                    Translator._languageModeMap.Add ("Dutch",       "nl");
                    Translator._languageModeMap.Add ("English",     "en");
                    Translator._languageModeMap.Add ("Estonian",    "et");
                    Translator._languageModeMap.Add ("Filipino",    "tl");
                    Translator._languageModeMap.Add ("Finnish",     "fi");
                    Translator._languageModeMap.Add ("French",      "fr");
                    Translator._languageModeMap.Add ("Galician",    "gl");
                    Translator._languageModeMap.Add ("German",      "de");
                    Translator._languageModeMap.Add ("Greek",       "el");
                    Translator._languageModeMap.Add ("Haitian Creole ALPHA",    "ht");
                    Translator._languageModeMap.Add ("Hebrew",      "iw");
                    Translator._languageModeMap.Add ("Hindi",       "hi");
                    Translator._languageModeMap.Add ("Hungarian",   "hu");
                    Translator._languageModeMap.Add ("Icelandic",   "is");
                    Translator._languageModeMap.Add ("Indonesian",  "id");
                    Translator._languageModeMap.Add ("Irish",       "ga");
                    Translator._languageModeMap.Add ("Italian",     "it");
                    Translator._languageModeMap.Add ("Japanese",    "ja");
                    Translator._languageModeMap.Add ("Korean",      "ko");
                    Translator._languageModeMap.Add ("Latvian",     "lv");
                    Translator._languageModeMap.Add ("Lithuanian",  "lt");
                    Translator._languageModeMap.Add ("Macedonian",  "mk");
                    Translator._languageModeMap.Add ("Malay",       "ms");
                    Translator._languageModeMap.Add ("Maltese",     "mt");
                    Translator._languageModeMap.Add ("Norwegian",   "no");
                    Translator._languageModeMap.Add ("Persian",     "fa");
                    Translator._languageModeMap.Add ("Polish",      "pl");
                    Translator._languageModeMap.Add ("Portuguese",  "pt");
                    Translator._languageModeMap.Add ("Romanian",    "ro");
                    Translator._languageModeMap.Add ("Russian",     "ru");
                    Translator._languageModeMap.Add ("Serbian",     "sr");
                    Translator._languageModeMap.Add ("Slovak",      "sk");
                    Translator._languageModeMap.Add ("Slovenian",   "sl");
                    Translator._languageModeMap.Add ("Spanish",     "es");
                    Translator._languageModeMap.Add ("Swahili",     "sw");
                    Translator._languageModeMap.Add ("Swedish",     "sv");
                    Translator._languageModeMap.Add ("Thai",        "th");
                    Translator._languageModeMap.Add ("Turkish",     "tr");
                    Translator._languageModeMap.Add ("Ukrainian",   "uk");
                    Translator._languageModeMap.Add ("Vietnamese",  "vi");
                    Translator._languageModeMap.Add ("Welsh",       "cy");
                    Translator._languageModeMap.Add ("Yiddish",     "yi");
                }
                string mode = string.Empty;
                Translator._languageModeMap.TryGetValue (language, out mode);
                return mode;
            }

        #endregion

        #region Fields

            /// <summary>
            /// The language to translation mode map.
            /// </summary>
            private static Dictionary<string, string> _languageModeMap;

        #endregion
    }
}
