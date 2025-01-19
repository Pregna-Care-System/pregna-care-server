using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class UserOauth
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int OauthProviderId { get; set; }

    public string OauthToken { get; set; } = string.Empty;

    public DateTime OauthTokenExpiry { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual OauthProvider OauthProvider { get; set; }

    public virtual User User { get; set; }
}
