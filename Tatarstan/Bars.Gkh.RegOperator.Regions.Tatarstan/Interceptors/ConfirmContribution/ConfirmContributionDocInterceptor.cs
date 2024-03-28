namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    public class ConfirmContributionDocInterceptor : EmptyDomainInterceptor<ConfirmContributionDoc>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ConfirmContributionDoc> service, ConfirmContributionDoc entity)
        {
            // проверяю, есть ли уже документы по этому дому за этот месяц
            return service.GetAll()
                          .Where(x => x.ConfirmContribution.Id == entity.ConfirmContribution.Id
                                   && x.RealityObject.Id == entity.RealityObject.Id)
                       .AsEnumerable()
                       .Any(x => x.TransferDate.Value.Month == entity.TransferDate.Value.Month 
                              && x.TransferDate.Value.Year == entity.TransferDate.Value.Year)
                           ? Failure("По данному дому за выбранный период платежное поручение уже было создано")
                           : Success(); 
        }
    }
}