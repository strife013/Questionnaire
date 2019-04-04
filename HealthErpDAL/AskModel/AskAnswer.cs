using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OUDAL
{

    /// <summary>
    /// 单题结果
    /// </summary>
    public class AskAnswer
    {
        public int TopicId { get; set; }
        public string TopicName { get; set; }

        public string SelectResult { get; set; }
        public decimal Score { get; set; }

        public int UserId { get; set; }
    }

}
