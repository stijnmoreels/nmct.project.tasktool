using Newtonsoft.Json;
using nmct.project.tasktool.models.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nmct.project.tasktool.models
{
    public class Room : ILabel
    {
        [NotMapped]
        public string Id { get; set; }

        [Key]
        [JsonIgnore]
        public int RoomId { get; set; }
        
        public string Name { get; set; }

        //[NotMapped]
        //[JsonIgnore]
        //public string Description { get; set; }

        [NotMapped]
        public string BoardId { get; set; }
        
        [JsonIgnore]
        public String RoomInitials { get; set; }

        [JsonIgnore]
        public virtual int CampusId { get; set; }

        [JsonIgnore]
        public Campus Campus { get; set; }

        public Room()
        {

        }

        public Room(string name)
        {
            this.Name = name;
        }

        public Room(string id, string name, string boardId)
        {
            this.Id = id;
            this.Name = name;
            this.BoardId = boardId;
        }
    }
}
