using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OUDAL
{
    public class PageFull
    {
        public AskPage Page { get; set; }
        public PageFull()
        {
            Topics = new List<TopicFull>();
        }
        public List<TopicFull> Topics { get; set; }
    }


    public class TopicFull
    {
        public List<AskTopic> allTopic { get; set; }
        public AskTopic Topic { get; set; }
        public TopicFull()
        {
            Contents = new List<AskContent>();
        }
        public List<AskContent> Contents { get; set; }
    }

}
