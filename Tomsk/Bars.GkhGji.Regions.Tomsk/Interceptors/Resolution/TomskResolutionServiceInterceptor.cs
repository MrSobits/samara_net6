namespace Bars.GkhGji.Regions.Tomsk.Interceptors
{

    using Bars.B4;

    using System.Linq;

    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities.Inspection;

    public class TomskResolutionServiceInterceptor : Bars.GkhGji.Interceptors.ResolutionServiceInterceptor<TomskResolution>
    {
        public IDomainService<DocumentGjiInspector> DocumentGjiInspectorDomainService { get; set; }

        public IDomainService<PrimaryBaseStatementAppealCits> PrimaryBaseStatementAppealCitsDomainService { get; set; }

        public IDomainService<TomskResolutionDescription> DescriptionDomain { get; set; }

        public override IDataResult BeforeUpdateAction(IDomainService<TomskResolution> service, TomskResolution entity)
        {
            if (!entity.DocumentDate.HasValue)
            {
                return new BaseDataResult(false, "Необходимо заполнить дату постановления");
            }

            if (!string.IsNullOrEmpty(entity.DocumentNumber))
            {
                var str = entity.DocumentNumber.Trim();

                // В томске необходимо чтобы Номер был уникальным в году
                if (
                    service.GetAll()
                        .Where(x => x.Id != entity.Id && x.DocumentDate.HasValue)
                        .Any(x => x.Id != entity.Id && x.DocumentDate.Value.Year == entity.DocumentDate.Value.Year && x.DocumentNumber == str))
                {
                    return new BaseDataResult(false, string.Format("Номер постановления  {0} уже существует в {1} году", entity.DocumentNumber.Trim(), entity.DocumentDate.ToDateTime().Year));
                }
            }

            return base.BeforeUpdateAction(service, entity);
        }

        public override IDataResult AfterCreateAction(IDomainService<TomskResolution> service, TomskResolution entity)
        {
            var primaryAppealCitsContragent =
                PrimaryBaseStatementAppealCitsDomainService.GetAll()
                    .Where(x => x.BaseStatementAppealCits.Inspection.Id == entity.Inspection.Id)
                    .Select(x => x.BaseStatementAppealCits.Inspection.Contragent)
                    .OrderByDescending(x => x.ObjectCreateDate)
                    .FirstOrDefault();

            if (primaryAppealCitsContragent != null)
            {
                entity.Contragent = primaryAppealCitsContragent;
            }

            return base.AfterCreateAction(service, entity);
        }

        public override IDataResult BeforeDeleteAction(IDomainService<TomskResolution> service, TomskResolution entity)
        {
            var res = base.BeforeDeleteAction(service, entity);
            if (res.Success)
            {
                var description = this.DescriptionDomain.GetAll().FirstOrDefault(x => x.Resolution.Id == entity.Id);
                if (description != null)
                {
                    this.DescriptionDomain.Delete(description.Id);
                }
            }

            return res;
        }
    }
}