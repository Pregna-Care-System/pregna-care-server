using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class JwtToken
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public string RefreshToken { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime ExpiresAt { get; set; }

    public virtual User User { get; set; }
}
