﻿using PregnaCare.Common.Api;

namespace PregnaCare.Api.Models.Requests
{
    public class CommunityRequest : AbstractApiRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
