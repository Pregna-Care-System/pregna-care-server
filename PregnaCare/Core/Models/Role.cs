﻿using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class Role
{
    public int Id { get; set; }

    public string RoleName { get; set; }

    public string Description { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
