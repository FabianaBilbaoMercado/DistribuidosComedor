using System;
using System.Collections.Generic;

namespace DistribuidosAlgoritmos.Entities;

public partial class Employee
{
    public int Id { get; set; }

    public int ExternalId { get; set; }

    public string FullName { get; set; } = null!;

    public int DepartmentId { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<AttendanceRecord> AttendanceRecords { get; set; } = new List<AttendanceRecord>();

    public virtual Department Department { get; set; } = null!;
}
