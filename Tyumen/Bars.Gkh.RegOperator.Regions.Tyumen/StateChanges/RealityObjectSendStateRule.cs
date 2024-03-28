namespace Bars.Gkh.RegOperator.Regions.Tyumen.StateChanges
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.States;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Mapping;
    using Bars.B4.Modules.Security;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Helpers;
    using Bars.Gkh.Utils;

    using Castle.Windsor;
    using Entities;

    public class RealityObjectSendStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id => "RealityObjectSendStateRule";

        public string Name => "Отправка уведомления о смене статуса автору запроса";

        public IEMailSender EMailSender { get; set; }

        public string TypeId => "gkh_real_obj";

        public string Description => "Отправка уведомления, если по данному дому был запрос на смену статуса";

        /// <inheritdoc />
        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var ro = statefulEntity as RealityObject;

            if (ro == null)
            {
                return ValidateResult.No("Внутренняя ошибка.");
            }
            try
            {
                var reqStateDomain = this.Container.Resolve<IDomainService<RequestState>>();
                var requestsThisRO = reqStateDomain.GetAll()
                    .Where(x => x.RealityObject != null && x.RealityObject.Id == ro.Id && !x.NotifiedUser).ToList();
                if (requestsThisRO != null)
                {
                    List<EmailRecipient> emailList = new List<EmailRecipient>();
                    foreach (RequestState rs in requestsThisRO)
                    {
                        var thisUser = this.Container.Resolve<IDomainService<Operator>>().GetAll().Where(x => x.User.Id == rs.UserId).FirstOrDefault();
                        if (!string.IsNullOrEmpty(thisUser.User.Email))
                        {
                            EmailRecipient er = new EmailRecipient
                            {
                                Email = thisUser.User.Email,
                                Name = thisUser.User.Name
                            };
                            emailList.Add(er);
                        }
                    }

                    foreach (EmailRecipient rsp in emailList)
                    {
                        EMailSender.Send(rsp.Email,
                                         $"Уведомление о смене статуса дома {ro.Address}",
                                         $"Уважаемый(ая) {rsp.Name}, по вашему запросу изменен статус дома по адрсу {ro.Address}");
                    }

                    foreach (RequestState rs in requestsThisRO)
                    {
                        rs.NotifiedUser = true;
                        reqStateDomain.Update(rs);
                    }

                }

            }
            catch (Exception e)
            {

            }

            return ValidateResult.Yes();

        }

 
  
    }
    public class EmailRecipient
    {
        public string Email { get; set; }

        public string Name { get; set; }
    }
}
