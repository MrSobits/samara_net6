namespace Bars.GkhGji.Regions.Tatarstan.DomainService.InspectionActionIsolated.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.ActionIsolated;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для <see cref="InspectionActionIsolated"/>
    /// </summary>
    public class InspectionActionIsolatedService : IInspectionActionIsolatedService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }
        
        /// <inheritdoc />
        public IDataResult ListTaskActionIsolated(BaseParams baseParams)
        {
            var documentGjidDomain = this.Container.ResolveDomain<DocumentGji>();
            var taskActionIsolatedDomain = this.Container.ResolveDomain<TaskActionIsolated>();
            var motivatedPresentationActionIsolatedDomain = this.Container.ResolveDomain<MotivatedPresentation>();

            using (this.Container.Using(taskActionIsolatedDomain, motivatedPresentationActionIsolatedDomain, documentGjidDomain))
            {
                var inspectionIds = motivatedPresentationActionIsolatedDomain.GetAll()
                    .Select(x => x.Inspection.Id);

                return documentGjidDomain.GetAll()
                    .Where(x => inspectionIds.Any(y => y == x.Inspection.Id) && x.TypeDocumentGji == TypeDocumentGji.ActActionIsolated)
                    .Join(documentGjidDomain.GetAll(),
                    x => new { x.Inspection.Id, TypeDocumentGji = TypeDocumentGji.TaskActionIsolated },
                    y => new { y.Inspection.Id, y.TypeDocumentGji },
                    (a, b) => new
                    {
                        b.Id,
                        b.DocumentDate,
                        InspectionId = a.Inspection.Id,
                        ActActionIsolatedNumber = a.DocumentNumber,
                        ActActionIsolatedDocumentDate = a.DocumentDate
                    })
                    .Join(taskActionIsolatedDomain.GetAll(),
                    x => x.Id,
                    y => y.Id,
                    (a, b) => new
                    {
                        a.InspectionId,
                        a.ActActionIsolatedNumber,
                        a.ActActionIsolatedDocumentDate,
                        TaskActionIsolatedDocumentDate = a.DocumentDate,
                        Contragent = b.Contragent.Name,
                        b.PersonName,
                        b.TypeJurPerson,
                        b.TypeObject
                    })
                    .Select(x => new
                    {
                        x.InspectionId,
                        x.ActActionIsolatedNumber,
                        x.ActActionIsolatedDocumentDate,
                        x.TaskActionIsolatedDocumentDate,
                        x.Contragent,
                        x.PersonName,
                        x.TypeJurPerson,
                        x.TypeObject
                    })
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
        }
    }
}