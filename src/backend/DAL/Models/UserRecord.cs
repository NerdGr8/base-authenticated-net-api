using System;
using System.ComponentModel.DataAnnotations;

namespace JK.DAL.Models
{
    public class UserRecord
    {
        [Key]
        public Guid ID{ get; set; }
        public string FullName { get; set; }
        public string UIFReferenceNumber { get; set; }
        public string IDNumber { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public DateTime ShutdownFrom { get; set; }
        public DateTime ShutdownTill { get; set; }
    }
}
