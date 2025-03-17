using System;
using System.Collections.Generic;

namespace DistribuidosAlgoritmos.Entities;

public partial class AttendanceRecord
{
    public int Id { get; set; }

    public int EmployeeId { get; set; }

    public DateTime RecordTime { get; set; }

    public string RecordType { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Employee Employee { get; set; } = null!;
}
