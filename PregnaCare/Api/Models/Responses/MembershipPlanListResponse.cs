﻿using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Api.Models.Responses
{
    public class MembershipPlanListResponse : AbstractApiResponse<IEnumerable<MembershipPlanFeatureDTO>>
    {
        public override IEnumerable<MembershipPlanFeatureDTO> Response { get; set; }
    }
}
