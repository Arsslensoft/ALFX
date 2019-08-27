using DialogueMaster.Babel;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Xml;

namespace Al.AI.NLP
{
    /// <summary>
    /// Base class of the natural human language interface
    /// </summary>
   public class NaturalLanguage
    {
        SpamFilter filter;
        Translator t;
       /// <summary>
       /// Initializes the natural language class
       /// </summary>
       /// <param name="spamfile">Spam words definition text file</param>
        /// <param name="goodfile">Good words definition text file</param>
        public NaturalLanguage(string spamfile,string goodfile)
        {
            Corpus bad = new Corpus();
            Corpus good = new Corpus();
            t = new Translator();

            bad.LoadFromFile(spamfile);
            good.LoadFromFile(goodfile);

            filter = new SpamFilter();
            filter.Load(good, bad);

        }
       /// <summary>
       /// Get's the sentiment from a text
       /// </summary>
       /// <param name="text"></param>
       /// <param name="uclassifyapi"></param>
       /// <param name="uclassifyurl"></param>
       /// <returns></returns>
        public double GetSentiment(string text, string uclassifyapi, string uclassifyurl = "http://ucassify.com/browse/uClassify/Sentiment/ClassifyText?readkey=")
        {
            double s = 0;
            //http://ucassify.com/browse/uClassify/Sentiment/ClassifyText?readkey=9w2Tx0Ph3iJUIqoUn1LmabeIY&text=This+is+the+text+to+classify&version=1.01
            try
            {
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(uclassifyurl + uclassifyapi + text + "&version=1.01");
                req.Accept = "gzip,deflate";
                req.AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate;
                req.Proxy = null;
                req.Timeout = 20000;
                WebResponse resp = req.GetResponse();
                XmlDocument doc = new XmlDocument();
                doc.Load(resp.GetResponseStream());
                foreach (XmlElement el in doc.DocumentElement.GetElementsByTagName("class"))
                {
                    if (el.GetAttribute("className") == "negative")
                        s -= double.Parse(el.GetAttribute("p").Replace(".", ","));
                    else
                        s += double.Parse(el.GetAttribute("p").Replace(".", ","));
                }
                return s;
            }
            catch
            {

            }
            return -1;
        }
      /// <summary>
      /// Translate's a text 
      /// </summary>
      /// <param name="text"></param>
      /// <param name="from"></param>
      /// <param name="to"></param>
      /// <returns></returns>
       public string Translate(string text, string from, string to)
        {

            t.SourceLanguage = from;
            t.TargetLanguage = to;
            t.SourceText = text;
            t.Translate();
            return t.Translation;
        }
       /// <summary>
       /// Scores a text
       /// </summary>
       /// <param name="text"></param>
       /// <returns></returns>
        public double Classify(string text)
        {

            return filter.Test(text);

        }
       /// <summary>
       /// Detect the text Language
       /// </summary>
       /// <param name="text"></param>
       /// <param name="score"></param>
       /// <returns></returns>
        public string DetectLanguage(string text, out double score)
        {
            score = 0;
            if (text.Length > 0)
            {
                BabelModel model = BabelModel._SmallModel;
                // classify it 
                DialogueMaster.Classification.ICategoryList result = model.ClassifyText(text, 10);
                // and dump the result
                double max = 0;
                string lang = null;
                foreach (DialogueMaster.Classification.ICategory category in result)
                {
                    if (category.Score > max)
                    {
                        lang = category.Name;
                        max = category.Score;

                    }
                }
                score = max;
                return lang;

            }
            else
                return null;
        }
    }
}
