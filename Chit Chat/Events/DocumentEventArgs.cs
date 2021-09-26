using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;

namespace ChitChat.Events
{
   public class DocumentEventArgs
    {
        public FlowDocument FlowDocument { get; set; }

        public DocumentEventArgs(FlowDocument flowDocument)
        {
            FlowDocument = flowDocument;
        }
    }
}
