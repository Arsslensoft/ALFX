using System;
using System.Xml;
using System.Text;

namespace Al.AI.AIML.AIMLTagHandlers
{
    /// <summary>
    /// The date element tells the AIML interpreter that it should substitute the system local 
    /// date and time. No formatting constraints on the output are specified.
    /// 
    /// The date element does not have any content. 
    /// </summary>
    public class date : Al.AI.AIML.Utils.AIMLTagHandler
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="bot">The bot involved in this request</param>
        /// <param name="user">The user making the request</param>
        /// <param name="query">The query that originated this node</param>
        /// <param name="request">The request inputted into the system</param>
        /// <param name="result">The result to be passed to the user</param>
        /// <param name="templateNode">The node to be processed</param>
        public date(Al.AI.AIML.Bot bot,
                        Al.AI.AIML.User user,
                        Al.AI.AIML.Utils.SubQuery query,
                        Al.AI.AIML.Request request,
                        Al.AI.AIML.Result result,
                        XmlNode templateNode)
            : base(bot, user, query, request, result, templateNode)
        {
        }

        protected override string ProcessChange()
        {
            if (this.templateNode.Name.ToLower() == "date")
            {
                return DateTime.Now.ToString(this.bot.Locale);
            }
            return string.Empty;
        }
    }
}
