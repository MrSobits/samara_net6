namespace Bars.Gkh.RegOperator.Quartz
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Quartz;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount;
    using Microsoft.Extensions.Logging;

    using Microsoft.Extensions.Logging;

    public class UnactivatePersonalAccountTask : BaseTask
    {
        public override void Execute(DynamicDictionary @params)
        {
            var deffUnactDomain = Container.Resolve<IDomainService<DefferedUnactivation>>();
            var accountDomain = Container.Resolve<IDomainService<BasePersonalAccount>>();
            var stateDomainService = Container.Resolve<IDomainService<State>>();
            var logger = Container.Resolve<ILogger>();
            logger.LogInformation("UnactivatePersonalAccountTask started");


            using (Container.Using(deffUnactDomain, accountDomain, deffUnactDomain))
            {
                var now = DateTime.Now;
                var deffereds = deffUnactDomain.GetAll().Where(x => !x.Processed)
                    .Where(x => x.UnactivationDate <= now);

                var unactiveState =
                    stateDomainService.FirstOrDefault(
                        x => x.TypeId == "gkh_regop_personal_account" && x.Code == "4");

                foreach (var deffered in deffereds)
                {
                    var acc = deffered.PersonalAccount;
                    acc.State = unactiveState;
                    accountDomain.Update(acc);
                }
            }
        }
    }
}