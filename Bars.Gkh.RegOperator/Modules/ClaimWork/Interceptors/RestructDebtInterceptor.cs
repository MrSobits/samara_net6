namespace Bars.Gkh.ClaimWork.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.ClaimWork.Entities;

    public class RestructDebtInterceptor : EmptyDomainInterceptor<RestructDebt>
    {
        /// <inheritdoc />
        public override IDataResult BeforeDeleteAction(IDomainService<RestructDebt> service, RestructDebt entity)
        {
            var restructScheduleDomain = this.Container.ResolveDomain<RestructDebtSchedule>();
            var detailRepository = this.Container.ResolveRepository<RestructDebtScheduleDetail>();

            using (this.Container.Using(restructScheduleDomain, detailRepository))
            {
                detailRepository.GetAll()
                    .Where(x => x.ScheduleRecord.RestructDebt.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => detailRepository.Delete(x));

                restructScheduleDomain.GetAll()
                    .Where(x => x.RestructDebt.Id == entity.Id)
                    .Select(x => x.Id)
                    .ToList()
                    .ForEach(x => restructScheduleDomain.Delete(x));
            }

            return this.Success();
        }
    }
}