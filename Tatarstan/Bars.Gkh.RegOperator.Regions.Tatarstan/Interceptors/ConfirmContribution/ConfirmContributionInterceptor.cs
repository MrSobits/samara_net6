namespace Bars.Gkh.RegOperator.Regions.Tatarstan.Interceptors
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.RegOperator.Regions.Tatarstan.Entities;

    public class ConfirmContributionInterceptor : EmptyDomainInterceptor<ConfirmContribution>
    {
        public override IDataResult BeforeDeleteAction(IDomainService<ConfirmContribution> service, ConfirmContribution entity)
        {
            var confContribDocServ = Container.Resolve<IDomainService<ConfirmContributionDoc>>();

            try
            {
                var confContribDocList =
                    confContribDocServ.GetAll()
                        .Where(x => x.ConfirmContribution.Id == entity.Id)
                        .Select(x => x.Id)
                        .ToArray();
                foreach (var value in confContribDocList)
                {
                    confContribDocServ.Delete(value);
                }

                return Success();
            }
            catch (Exception)
            {
                return Failure("Не удалось удалить связанные записи");
            }
            finally
            {
                Container.Release(confContribDocServ);
            }
        }
    }
}