using System;
namespace JK.DAL.Mappings
{
    public class UserRecordDto
    {
        public string FullName { get; set; }
        public string UIFReferenceNumber { get; set; }
        public string IDNumber { get; set; }
        public DateTime EmploymentStartDate { get; set; }
        public DateTime ShutdownFrom { get; set; }
        public DateTime ShutdownTill { get; set; }
    }
}
