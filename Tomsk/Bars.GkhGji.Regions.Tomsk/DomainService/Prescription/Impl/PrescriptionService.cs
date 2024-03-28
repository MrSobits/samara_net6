using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;

    public class PrescriptionService : Bars.GkhGji.DomainService.PrescriptionService
    {
        public IDomainService<DocumentGjiInspector> docInspectorDomain { get; set; }

        public IDomainService<DocumentGjiChildren> docChildrenDomain { get; set; }
        
        public override IDataResult GetInfo(long? documentId)
        {
            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;
                
                var dataInspectors = docInspectorDomain.GetAll()
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

                    baseName += string.Format("{0} №{1} от {2}", docName, doc.DocumentNumber, doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(new PrescriptionGetInfoProxy { inspectorNames = inspectorNames, inspectorIds = inspectorIds, baseName = baseName, parentId = parentId });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(false, e.Message);
            }
        }
    }
}
