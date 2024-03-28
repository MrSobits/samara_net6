namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Inspection.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    public class WarningDocViolationsDomainService : BaseDomainService<WarningDocViolations>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var result = base.Save(baseParams);
            return result.Success ? this.UpdateDetail(baseParams, result) : result;
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var result = base.Update(baseParams);
            return result.Success ? this.UpdateDetail(baseParams, result) : result;
        }

        private IDataResult UpdateDetail(BaseParams baseParams, IDataResult saveResult)
        {
            var warningDocViolations = (saveResult.Data as IEnumerable<WarningDocViolations>)?.FirstOrDefault();
            if (warningDocViolations == null)
            {
                return BaseDataResult.Error("Не удалось сохранить нарушение требований");
            }

            var saveParams = baseParams.Params.GetAs<DynamicDictionary[]>("records")[0];
            var warningDocViolationId = saveParams.GetAsId();
            var violationIds = saveParams.GetAs<long[]>("Violations").ToHashSet();
            var detailDomain = this.Container.ResolveDomain<WarningDocViolationsDetail>();
            var violationsDomain = this.Container.ResolveDomain<ViolationGji>();

            using (this.Container.Using(detailDomain, violationsDomain))
            {
                var count = violationsDomain.GetAll()
                    .WhereContains(x => x.Id, violationIds)
                    .Count();

                if (count != violationIds.Count)
                {
                    return BaseDataResult.Error("Не корректный идентификатор нарушения");
                }

                this.Container.InTransaction(() =>
                {
                    detailDomain.GetAll()
                        .Where(x => x.WarningDocViolations.Id == warningDocViolationId)
                        .Select(x => x.Id)
                        .ForEach(x => detailDomain.Delete(x));

                    foreach (var violationsId in violationIds)
                    {
                        detailDomain.Save(new WarningDocViolationsDetail
                        {
                            WarningDocViolations = warningDocViolations,
                            ViolationGji = new ViolationGji { Id = violationsId }
                        });
                    }
                });

                return new BaseDataResult(count);
            }
        }
    }
}