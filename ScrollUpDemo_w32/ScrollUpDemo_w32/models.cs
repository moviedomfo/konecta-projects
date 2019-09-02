using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrollUpDemo_w32
{
    
    public class CaseCommentList : List<CaseCommentBE>
    { }

  
    public class CaseCommentBE 
    {
        public System.Int32? CaseCommentId { get; set; }

        public System.String CaseCommentText { get; set; }

        public System.Int32 CaseId { get; set; }

        public System.DateTime CaseCommentCreated { get; set; }
        public System.DateTime CreatedRowOrigenLog { get; set; }
        public System.Guid CaseCommentGUID { get; set; }
        public System.Int32 UserChannelId { get; set; }
        



    }
}
