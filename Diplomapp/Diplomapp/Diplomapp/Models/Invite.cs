using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Diplomapp.Models
{
    public class Invite
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int Projectid { get; set; }
        public string OwnerEmail { get; set; }
        public string Invitation { get; set; }
        public string Role { get; set; }
        public string Position { get; set; }

        public string Userid { get; set; }
    }
}