using System;
using System.Collections.Generic;

namespace PregnaCare.Core.Models;

public partial class OauthProvider
{
    public int Id { get; set; }

    public string ProviderName { get; set; }

    public string ClientId { get; set; }

    public string ClientSecret { get; set; }

    public string RedirectUri { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<UserOauth> UserOauths { get; set; } = new List<UserOauth>();
}
