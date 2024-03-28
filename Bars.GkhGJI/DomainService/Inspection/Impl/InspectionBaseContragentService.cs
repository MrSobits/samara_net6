namespace Bars.GkhGji.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы с Органами совместной проверки
    /// </summary>
    public class InspectionBaseContragentService : IInspectionBaseContragentService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<InspectionBaseContragent> InspectionBaseContragentDomain { get; set; }

        public IDomainService<Contragent> ContragentDomain { get; set; }

        /// <inheritdoc />
        public IDataResult AddContragents(BaseParams baseParams)
        {
            var inspectionId = baseParams.Params.GetAsId("inspectionId");
            var contragentIds = baseParams.Params.GetAs<long[]>("contragentIds");

            this.Container.InTransaction(
                () =>
                {
                    var inspection = new InspectionGji { Id = inspectionId};

                    var existsContragentQuery = this.InspectionBaseContragentDomain.GetAll()
                        .Where(x => x.InspectionGji.Id == inspectionId);

                    var recordsForSave = this.ContragentDomain.GetAll()
                        .WhereContains(x => x.Id, contragentIds)
                        .Where(x => !existsContragentQuery.Any(y => y.Contragent == x))
                        .AsEnumerable()
                        .Select(
                            x => new InspectionBaseContragent
                            {
                                Contragent = x,
                                InspectionGji = inspection 
                            })
                        .ToList();

                    recordsForSave.ForEach(this.InspectionBaseContragentDomain.Save);
                });

            return new BaseDataResult();
        }

        /// <inheritdoc />
        public IDataResult ListContragentRisk(BaseParams baseParams)
        {
            var inspectionRiskDomain = this.Container.ResolveDomain<InspectionRisk>();

            using (this.Container.Using(inspectionRiskDomain))
            {
                var contragentId = baseParams.Params.GetAsId("contragentId");

                return inspectionRiskDomain.GetAll()
                    .Where(x => x.Inspection.Contragent.Id == contragentId)
                    .Select(x => new
                    {
                        x.Id,
                        Number = x.Inspection.InspectionNumber,
                        RiskCategory = x.RiskCategory.Name,
                        x.StartDate,
                        x.EndDate
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}