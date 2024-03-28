namespace Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Prescription;

    public class ChelyabinskPrescriptionService : Bars.GkhGji.DomainService.PrescriptionService
    {
        public override IDataResult GetInfo(long? documentId)
        {
            var serviceDisposalInspector = this.Container.Resolve<IDomainService<DocumentGjiInspector>>();
            var docChildrenDomain = this.Container.ResolveDomain<DocumentGjiChildren>();
            var activityDirectionDomain = this.Container.ResolveDomain<PrescriptionActivityDirection>();

            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var directionNames = string.Empty;
                var directionIds = string.Empty;
                var baseName = string.Empty;

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var dataInspectors = serviceDisposalInspector.GetAll()
                    .Where(x => x.DocumentGji.Id == documentId)
                    .Select(x => new
                    {
                        InspectorId = x.Inspector.Id,
                        FIO = x.Inspector.Fio
                    })
                    .ToList();
                    
                foreach (var item in dataInspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.FIO;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.InspectorId.ToString();
                }

                var dataDirections = activityDirectionDomain.GetAll()
                    .Where(x => x.Prescription.Id == documentId)
                    .Select(x => new
                    {
                        ActivityDirectionId = x.ActivityDirection.Id,
                        ActivityDirectionName = x.ActivityDirection.Name
                    })
                    .ToList();

                foreach (var item in dataDirections)
                {
                    if (!string.IsNullOrEmpty(directionNames))
                    {
                        directionNames += ", ";
                    }

                    directionNames += item.ActivityDirectionName;

                    if (!string.IsNullOrEmpty(directionIds))
                    {
                        directionIds += ", ";
                    }

                    directionIds += item.ActivityDirectionId.ToString();
                }

                // Пробегаемся по документам на основе которого создано предписание
                var parents = docChildrenDomain.GetAll()
                    .Where(x => x.Children.Id == documentId)
                    .Select(x => new
                    {
                        parentId = x.Parent.Id,
                        x.Parent.TypeDocumentGji,
                        x.Parent.DocumentDate,
                        x.Parent.DocumentNumber
                    })
                    .ToList();

                var parentId = parents.Select(x => x.parentId).Distinct().FirstOrDefault();

                foreach (var doc in parents)
                {
                    var docName = Bars.GkhGji.Utils.Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (!string.IsNullOrEmpty(baseName))
                    {
                        baseName += ", ";
                    }

                    baseName += string.Format("{0} №{1} от {2}", docName, doc.DocumentNumber,
                        doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return
                    new BaseDataResult(new 
                    {
                        inspectorNames,
                        inspectorIds,
                        directionNames,
                        directionIds,
                        baseName,
                        parentId
                    });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
            finally
            {
                this.Container.Release(serviceDisposalInspector);
                this.Container.Release(docChildrenDomain);
                this.Container.Release(activityDirectionDomain);
            }
        }
    }
}
