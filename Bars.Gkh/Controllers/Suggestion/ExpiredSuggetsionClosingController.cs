namespace Bars.Gkh.Controllers.Suggestion
{
    using System;
    using B4.IoC;
    using Bars.B4;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4.Application;
    using Bars.Gkh.Domain.Suggestions;

    /// <summary>
    /// Контроллер используется только для тестирования и демонстрации.
    /// </summary>
    public class ExpiredSuggetsionClosingController : BaseController
    {
        /// <summary>
        /// Закрытие обращений, по которым прошел срок ожидания.
        /// На ожидании считаются обращения, которые не закрыты (статус с кодом "end") и находятся в конечном статусе.
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult CloseExpired(BaseParams baseParams)
        {
            try
            {
                var waitDays = baseParams.Params.GetAs("waitDays", ApplicationContext.Current.Configuration.AppSettings.GetAs("PortalResponseWaitDays", () => 10));
                Container.UsingForResolved<IExpiredSuggestionCloser>((container, service) => service.Close(waitDays));
                return JsSuccess();
            }
            catch (Exception e)
            {
                return JsFailure(e.Message);
            }
        }
    }
}
