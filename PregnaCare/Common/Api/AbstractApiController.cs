using Microsoft.AspNetCore.Mvc;

namespace PregnaCare.Common.Api
{
    /// <summary>
    /// AbstractApiController
    /// </summary>
    /// <typeparam name="TRequest"></typeparam>
    /// <typeparam name="TRsponse"></typeparam>
    public abstract class AbstractApiController<TRequest, TRsponse> : ControllerBase
    {
        public abstract Task<TRsponse> Exec(TRequest request);
    }
}
