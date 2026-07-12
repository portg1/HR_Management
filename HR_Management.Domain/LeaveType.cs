namespace HR_Management.Domain;

public class LeaveType
{
      public int Id { get; set; }
      public string Name { get; set; }
      public int DefaultDay { get; set; }
      public DateTime DateCreated { get; set; }
}