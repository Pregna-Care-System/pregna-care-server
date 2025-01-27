using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Role
{
    public Guid Id { get; set; }

    public string RoleName { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<UserRole> UserRoles { get; set; } = new List<UserRole>();
}
