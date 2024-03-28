namespace Bars.GkhGji.Regions.Voronezh.Interceptors
{
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Entities;
    using System;

    class ProtocolOSPRequestInterceptor : EmptyDomainInterceptor<ProtocolOSPRequest>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ProtocolOSPRequest> service, ProtocolOSPRequest entity)
        {
            try
            {
                var servStateProvider = Container.Resolve<IStateProvider>();

                try
                {
                    servStateProvider.SetDefaultState(entity);
                }
                finally
                {
                    Container.Release(servStateProvider);
                }
                if (!string.IsNullOrEmpty(entity.Email))
                {
                    try
                    {

                    }
                    catch { }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure($"Ошибка интерцептора BeforeCreateAction<ProtocolOSPRequest>: {e.Message}");
            }
        }

        public override IDataResult AfterCreateAction(IDomainService<ProtocolOSPRequest> service, ProtocolOSPRequest entity)
        {
            if (!string.IsNullOrEmpty(entity.Email))
            {
                try
                {
                    EmailSender emailSender = EmailSender.Instance;
                    emailSender.Send(entity.Email, "Рассмотрение заявления", MakeMessageBody(), null);
                }
                catch { }
            }
            return Success();
        }
        public override IDataResult BeforeUpdateAction(IDomainService<ProtocolOSPRequest> service, ProtocolOSPRequest entity)
        {
            if (entity.RealityObject != null)
            {
                entity.Address = entity.RealityObject.Address;
                entity.Municipality = entity.RealityObject.Municipality.Name;
            }          

            if (entity.State == null)
            {
                var servStateProvider = Container.Resolve<IStateProvider>();

                try
                {
                    servStateProvider.SetDefaultState(entity);
                }
                finally
                {
                    Container.Release(servStateProvider);
                }
            }

            return Success();
        }

        private string MakeMessageBody()
        {
            string body = $"Уважаемый(ая) заявитель!\r\n";
            body += $"Государственная жилищная инспекция Воронежской области уведомляет Вас о том, что Ваше заявление принято к рассмотрению. Срок рассмотрения заявления составляет 10 дней\r\n";
            body += $"Данный почтовый адрес используется для автоматического уведомления пользователей системы электронного документооборота и не предназначен для приема какого-либо рода электронных сообщений (обращений)";
            return body;
        }


    }
}
