﻿using PregnaCare.Common.Api;
using PregnaCare.Core.DTOs;

namespace PregnaCare.Api.Models.Responses.AccountResponseModel
{
    public class AccountListResponse : AbstractApiResponse<IEnumerable<AccountDTO>>
    {
        public override IEnumerable<AccountDTO> Response { get; set; }
    }
}
