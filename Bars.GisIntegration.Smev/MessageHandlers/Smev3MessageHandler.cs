namespace Bars.GisIntegration.Smev.MessageHandlers
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.IoC.Lifestyles.SessionLifestyle;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.Gkh.Gis.RabbitMQ;
    using Bars.Gkh.Smev3;

    using Castle.Windsor;

    /// <summary>
    /// Обработчик сообщений очереди интеграции со шлюзом СМЭВ
    /// </summary>
    public class Smev3MessageHandler : IMessageHandler<Smev3Response>
    {
        private readonly IWindsorContainer container;

        public Smev3MessageHandler(IWindsorContainer _container)
        {
            this.container = _container;
        }

        /// <summary>
        /// Обработка сообщения
        /// Сохраненение ответа от СМЭВ для последующей обработки в SmevBaseSendDataTask.GetStateResult
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void HandleMessage(Smev3Response message)
        {
            // Сохраненение ответа от СМЭВ для последующей обработке в <see cref="IMessageHandler{T}"/>
            ExplicitSessionScope.CallInNewScope(() =>
            {
                var storableResponseDomain = this.container.ResolveDomain<StorableSmev3Response>();

                using (this.container.Using(storableResponseDomain))
                {
                    var storableResponse = storableResponseDomain.GetAll()
                            .FirstOrDefault(x => x.requestGuid.ToLower() == message.RequestId.ToLower()) 
                        ?? new StorableSmev3Response();

                    storableResponse.requestGuid = message.RequestId;
                    storableResponse.Response = message;

                    if (storableResponse.Id > 0)
                    {
                        storableResponseDomain.Update(storableResponse);
                    }
                    else
                    {
                        storableResponseDomain.Save(storableResponse);
                    }
                }
            });
        }
    }
}