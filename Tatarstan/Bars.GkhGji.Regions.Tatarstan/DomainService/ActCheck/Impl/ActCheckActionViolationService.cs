using Bars.B4;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities.ActCheckAction;
using Castle.Windsor;
using System.Collections.Generic;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService.ActCheck.Impl
{
    using System.Linq;

    using Bars.B4.IoC;
    using Bars.Gkh.Domain;

    public class ActCheckActionViolationService : IActCheckActionViolationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddViolations(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var violationIds = baseParams.Params.GetAs<long[]>("violationIds");
            var violationsList = new List<ActCheckActionViolation>();

            var actCheckActionViolationDomain = this.Container.Resolve<IDomainService<ActCheckActionViolation>>();
            var actCheckActionDomain = this.Container.Resolve<IDomainService<ActCheckAction>>();
            var violationDomain = this.Container.Resolve<IDomainService<ViolationGji>>();

            using (this.Container.Using(actCheckActionDomain, violationDomain, actCheckActionViolationDomain))
            {
                var actCheckAction = actCheckActionDomain.Get(id);

                var violations = violationDomain.GetAll()
                    .Where(x => violationIds.Contains(x.Id))
                    .AsEnumerable();

                var existRecords = actCheckActionViolationDomain.GetAll()
                    .Where(x => x.ActCheckAction == actCheckAction && violationIds.Contains(x.Violation.Id))
                    .Select(x => x.Violation)
                    .AsEnumerable();

                var violationsForAdd = violations.Except(existRecords);

                foreach (var violation in violationsForAdd)
                {
                    violationsList.Add(new ActCheckActionViolation
                    {
                        ActCheckAction = actCheckAction,
                        Violation = violation
                    });
                }

                TransactionHelper.InsertInManyTransactions(this.Container, violationsList, 10000, true, true);
            }

            return new BaseDataResult();
        }
    }
}
