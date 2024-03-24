using Microsoft.UI.Text;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WritingAssistant
{
    class Comment
    {
        public string CommentText { get; set; }
        public string StoryText { get; set; }
        private ITextRange range = null;
        private string id = "";


        public Comment() { }

        public Comment(ITextRange r)
        {
            range = r;
        }


        


    }
}
