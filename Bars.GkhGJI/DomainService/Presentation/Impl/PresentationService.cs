namespace Bars.GkhGji.DomainService
{
    using System.Text;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.B4.Modules.DataExport.Domain;

    using Entities;
    using Utils;

    using Gkh.Authentification;

    using Castle.Windsor;

    public class PresentationService : IPresentationService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            try
            {
                var documentId = baseParams.Params.GetAs<long>("documentId");

                var baseName = new StringBuilder();

                // Пробегаемся по документам на основе которого создано постановление
                var parents = 
                    Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                        .Where(x => x.Children.Id == documentId)
                        .Select(x => new
                            {
                                parentId = x.Parent.Id,
                                x.Parent.TypeDocumentGji,
                                x.Parent.DocumentDate,
                                x.Parent.DocumentNumber
                            })
                        .ToList();

                foreach (var doc in parents)
                {
                    var docName = Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (baseName.Length > 0)
                        baseName.Append(", ");

                    baseName.AppendFormat(
                        "{0} №{1} от {2}",
                        docName,
                        doc.DocumentNumber,
                        doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(new { success = true, baseName = baseName.ToString() });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, e.Message });
            }
        }

        public IQueryable<Presentation> GetFilteredByOperator(IDomainService<Presentation> domainService)
        {
            var userManager = Container.Resolve<IGkhUserManager>();

            var inspectorIds = userManager.GetInspectorIds();
            var municipalityIds = userManager.GetMunicipalityIds();

            var serviceInspectionRobject = Container.Resolve<IDomainService<InspectionGjiRealityObject>>();

            return domainService.GetAll()
                .WhereIf(inspectorIds.Count > 0, x => inspectorIds.Contains(x.Official.Id))
                .WhereIf(municipalityIds.Count > 0, 
                    y => serviceInspectionRobject.GetAll()
                            .Any(x => x.Inspection.Id == y.Inspection.Id && municipalityIds.Contains(x.RealityObject.Municipality.Id)));
        }
        
        /// <inheritdoc />
        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>(this.GetDataExportRegistrationName());

            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Получить наименование, под которым зарегистрирован сервис
        /// </summary>
        /// <returns></returns>
        protected virtual string GetDataExportRegistrationName() => "PresentationDataExport";
    }
}