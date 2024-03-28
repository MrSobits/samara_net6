namespace Bars.B4.Modules.ESIA.OAuth20.Handlers
{
    using System;
    using System.Web;

    using Bars.B4.Application;
    using Bars.B4.Logging;
    using Bars.B4.Modules.ESIA.OAuth20.Entities;
    using EsiaNET;

    /// <summary>
    /// Обработчик запроса на логин через ЕСИА с помощью OAuth
    /// </summary>
    public class OauthSignonHandler : IHttpHandler
    {
        /// <summary>
        ///     Enables processing of HTTP Web requests by a custom HttpHandler that implements the <see cref="T:System.Web.IHttpHandler" /> interface.
        /// </summary>
        /// <param name="context">
        ///     An <see cref="T:System.Web.HttpContext" /> object that provides references to the intrinsic server objects (for example, Request, Response, Session, and Server) used to service HTTP requests.
        /// </param>
        public void ProcessRequest(HttpContext context)
        {
            var logManager = ApplicationContext.Current.Container.Resolve<ILogManager>();

            try
            {
                //Получаем клиент ЕСИА
                var esiaClient = ApplicationContext.Current.Container.Resolve<EsiaClient>();

                //Собираем запрос
                var redirectUrl = esiaClient.BuildRedirectUri();

                //переходим на логинку ЕСИА
                context.Response.Redirect(redirectUrl);
            }
            catch (Exception e)
            {
                EmailSender.Instance.TrySendIfLogEnabled(e.Message,e.InnerException+"\r\n"+e.StackTrace);
                
                logManager.Error("Ошибка логина через OAuth", e);
                context.Response.Write(e.Message);
            }
        }

        /// <summary>
        ///     Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler" /> instance.
        /// </summary>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler" /> instance is reusable; otherwise, false.
        /// </returns>
        public bool IsReusable => true;
    }
}
