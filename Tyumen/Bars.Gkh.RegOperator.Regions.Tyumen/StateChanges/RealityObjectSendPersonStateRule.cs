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

    public class RealityObjectSendPersonStateRule : IRuleChangeStatus
    {
        public virtual IWindsorContainer Container { get; set; }

        public string Id => "RealityObjectSendPersonStateRule";

        public string Name => "Отправка уведомления о возвращении статуса дома";

        public IEMailSender EMailSender { get; set; }

        public IDomainService<RequestStatePerson> RequestStatePersonDomain { get; set; }

        public string TypeId => "gkh_real_obj";

        public string Description => "Отправка уведомления о возвращении статуса, если по данному дому был сменен статус по запросу";

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
                    .Where(x => x.RealityObject != null && x.RealityObject.Id == ro.Id && x.NotifiedUser && !x.NotifiedPerson).ToList();
                if (requestsThisRO != null)
                {
                    var emailList = RequestStatePersonDomain.GetAll()
                     .Where(x => x.Status == Enums.RequestStatePersonEnum.Edit).ToList();

                    foreach (RequestStatePerson rsp in emailList)
                    {
                        EMailSender.Send(rsp.Email,
                                         $"Уведомление о возврате дома в статус {newState.Name}",
                                         $"Уважаемый (ая) {rsp.Name}, уведомляем Вас о возврате статуса дома по адресу {ro.Address} в статус {newState.Name}");
                    }

                    foreach (RequestState rs in requestsThisRO)
                    {
                        rs.NotifiedPerson = true;
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
  
}
