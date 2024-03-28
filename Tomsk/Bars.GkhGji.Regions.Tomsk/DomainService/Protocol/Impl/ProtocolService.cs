namespace Bars.GkhGji.Regions.Tomsk.DomainService
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Controller;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    /// <summary>
    /// Сервис протоколов Томска
    /// </summary>
    public class TomskProtocolService : ProtocolService<TomskProtocol>
    {
        /// <summary>
        /// Домен-сервис документов-инспекторов
        /// </summary>
        public IDomainService<DocumentGjiInspector> DocInspector { get; set; }

        /// <summary>
        /// Домен-сервис протоколов Томска
        /// </summary>
        public IDomainService<TomskProtocol> ProtocolDomain { get; set; }

        /// <summary>
        /// Домен-сервис "детских" документов
        /// </summary>
        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        /// <summary>
        /// Домен-сервис требований
        /// </summary>
        public IDomainService<RequirementDocument> ReqDocDomain { get; set; }

        /// <summary>
        /// Домент-сервис документов ГЖИ
        /// </summary>
        public IDomainService<DocumentGji> DocumentDomain { get; set; }

        /// <summary>
        /// Получение информации о протоколе
        /// </summary>
        /// <param name="documentId">Идентификатор документа</param>
        /// <returns>Информация о протоколе</returns>
        public override IDataResult GetInfo(long? documentId)
        {
            try
            {
                var inspectorNames = string.Empty;
                var inspectorIds = string.Empty;
                var baseName = string.Empty;

                var protocol = this.ProtocolDomain.GetAll().FirstOrDefault(x => x.Id == documentId);

                var parentDocument =
                    this.DocumentDomain.GetAll()
                                  .FirstOrDefault(
                                      x =>
                                      x.Stage.Id == (protocol != null
                                      && protocol.Stage != null && protocol.Stage.Parent != null
                                              ? protocol.Stage.Parent.Id
                                              : 0));

                // Сначала пробегаемся по инспекторам и формируем итоговую строку наименований и строку идентификаторов
                var inspectors = this.DocInspector.GetAll()
                    .Where(x => x.DocumentGji.Id == documentId)
                    .Select(x => new
                    {
                        x.Inspector.Id,
                        x.Inspector.Fio
                    })
                    .ToList();

                foreach (var item in inspectors)
                {
                    if (!string.IsNullOrEmpty(inspectorNames))
                    {
                        inspectorNames += ", ";
                    }

                    inspectorNames += item.Fio;

                    if (!string.IsNullOrEmpty(inspectorIds))
                    {
                        inspectorIds += ", ";
                    }

                    inspectorIds += item.Id;
                }

                // Пробегаемся по документам на основе которого создано предписание
                var parents = this.ChildrenDomain.GetAll()
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
                    var docName = GkhGji.Utils.Utils.GetDocumentName(doc.TypeDocumentGji);

                    if (!string.IsNullOrEmpty(baseName))
                    {
                        baseName += ", ";
                    }

                    baseName += string.Format("{0} №{1} от {2}", docName, doc.DocumentNumber, doc.DocumentDate.ToDateTime().ToShortDateString());
                }

                // Требование для протокола
                var req = this.ReqDocDomain.GetAll().FirstOrDefault(x => x.Document.Id == documentId);
                var reqName = string.Empty;
                if (req != null)
                {
                    reqName = string.Format(
                        "№{0} от {1}",
                        req.Requirement.DocumentNumber,
                        req.Requirement.DocumentDate.ToDateTime().ToShortDateString());
                }

                return new BaseDataResult(new { inspectorNames, inspectorIds, baseName, reqName, parentId = parentDocument != null ? parentDocument.Id : 0 });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult { Success = false, Message = e.Message };
            }
        }
    }
}