using Newtonsoft.Json;
using nmct.project.tasktool.models.Constants;
using nmct.project.tasktool.models.Interfaces;
using nmct.project.tasktool.models.Reports;
using nmct.project.tasktool.models.Trello;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using nmct.project.tasktool.models.Extensions;
using System.Globalization;

namespace nmct.project.tasktool.models.Archive
{
    public class TrelloArchiveCard : ITrelloCard
    {
        private TrelloCard card;

        public TrelloArchiveCard()
        {

        }

        public TrelloArchiveCard(TrelloCard card)
        {
            this.CommentsCount = card.Badges.Comments;
            this.AttachmentsCount = card.Badges.Attachments;
            this.Badges = card.Badges;
            this.Desc = card.Desc;
            this.Due = card.Due;
            this.IdBoard = card.IdBoard;
            this.IdList = card.IdList;
            this.Id = card.Id;
            this.TrelloCardId = card.Id;
            this.Labels = card.Labels;
            this.ShortLink = card.ShortLink;
            this.Name = card.Name;
            this.IdMembers = card.IdMembers;
            this.MembersCount = card.IdMembers.Length;
        }
        
        public bool InArchive { get; set; }

        [JsonIgnore]
        public List<TrelloNet.Card.Attachment> Attachments { get;  set; }

        [JsonIgnore]
        public TrelloNet.Card.CardBadges Badges { get;  set; }

        public string Desc { get;  set; }

        public DateTime? Due { get;  set; }

        public string Id { get; set; }

        public string IdBoard { get;  set; }

        public string IdList { get;  set; }

        public TrelloList List { get; set; }

        [JsonIgnore]
        public string[] IdMembers { get; set; }

        [JsonIgnore]
        public List<TrelloLabel> Labels { get; set; }

        public Magnitude Magnitude { get; set; }
        public Room Room { get; set; }
        public Urgency Urgency { get; set; }

        public string Name { get; set; }

        public string ShortLink { get; set; }

        public void InsertReactionRuntime(ReactionRuntime reactionRuntime)
        {
            this.CardCreated = reactionRuntime.CardCreated.ToString(CultureInfo.CurrentCulture);
            this.CardIsDone = reactionRuntime.CardIsDone;
            this.CardOnProgress = reactionRuntime.CardOnProgress;
            this.TrelloMembers = reactionRuntime.TrelloMembers;
            this.TrelloCardId = reactionRuntime.TrelloCardId;
            this.TrelloCampusId = reactionRuntime.TrelloCampusId;
            this.TrelloCampus = reactionRuntime.TrelloCampus;
            this.MemberIds = reactionRuntime.MemberIds;
            this.TrelloMembers = reactionRuntime.TrelloMembers;
        }

        public string CardCreated { get; set; }
        public DateTime? CardIsDone { get; set; }

        public DateTime? CardOnProgress { get; set; }


        public string[] MemberIds { get; set; }

        public TrelloCampus TrelloCampus { get; set; }

        public int TrelloCampusId { get; set; }

        public string TrelloCardId { get; set; }

        public List<TrelloMember> TrelloMembers { get; set; }

        public int AttachmentsCount { get; set; }
        public int CommentsCount { get; set; }
        public int MembersCount { get; set; }

    }
}
