namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    public class BaseStatementAppealCitsServiceInterceptor : GkhGji.Interceptors.BaseStatementAppealCitsServiceInterceptor
    {
        public IDomainService<PrimaryBaseStatementAppealCits> PrimaryDomain { get; set; }

        public override IDataResult BeforeDeleteAction(IDomainService<InspectionAppealCits> service, InspectionAppealCits entity)
        {
            var ids = PrimaryDomain.GetAll()
                .Where(x => x.BaseStatementAppealCits.Id == entity.Id)
                .Select(x => x.Id)
                .ToList();

            ids.ForEach(x => PrimaryDomain.Delete(x));

            return base.BeforeDeleteAction(service, entity);
        }
    }
}
