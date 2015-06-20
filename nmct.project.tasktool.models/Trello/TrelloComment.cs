using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models.Trello
{
    public class TrelloComment
    {
        public string Id { get; set; }
        public TrelloDataComment Data { get; set; }
        public DateTime Date { get; set; }
        public TrelloMember MemberCreator { get; set; }
        private string _dateCulture;

        public string DateCulture
        {
            get { return Date.ToString(CultureInfo.CurrentCulture); }
            set { _dateCulture = value; }
        }
        
        
        private string _comment;
        public string Comment
        {
            get 
            { 
                return Data.Text; 
            }
            set { _comment = value; }
        }
        
    }

    public class TrelloDataComment
    {
        public string Text { get; set; }
    }
}
