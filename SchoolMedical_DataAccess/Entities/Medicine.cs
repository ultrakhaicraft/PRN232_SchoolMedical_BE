using System;
using System.Collections.Generic;

namespace SchoolMedical_DataAccess.Entities;

//Missing Created Date & Updated Date (Ignore for now)
public partial class Medicine
{
    public string Id { get; set; } = null!;

    public string CreatedBy { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public int Amount { get; set; }

    public bool? IsAvailable { get; set; }

    public bool IsDeleted { get; set; }

    public virtual Account CreatedByNavigation { get; set; } = null!;
}
