using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class UserFeature
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int FeatureId { get; set; }

    public DateTime? ActivatedAt { get; set; }

    public DateTime? ExpiryDate { get; set; }

    public bool? IsActive { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual Feature Feature { get; set; }

    public virtual User User { get; set; }
}
