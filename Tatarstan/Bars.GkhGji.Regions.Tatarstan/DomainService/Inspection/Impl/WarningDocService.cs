namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Inspection.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Entities;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class WarningDocService : IWarningDocService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetInfo(long? documentId)
        {
            try
            {
                //var documentId = baseParams.Params.ContainsKey("documentId") ? baseParams.Params["documentId"].ToLong() : 0;
                var baseName = "";

                // Пробегаемся по документам на основе которого создано постановление
                var parents = this.Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
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

                return new BaseDataResult(new { success = true, baseName });
            }
            catch (ValidationException e)
            {
                return new BaseDataResult(new { success = false, message = e.Message });
            }
        }

        public IDataResult ListView(BaseParams baseParams)
        {
            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs<DateTime>("dateStart");
            var dateEnd = baseParams.Params.GetAs<DateTime>("dateEnd");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var isExport = baseParams.Params.GetAs<bool>("isExport");

            return this.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId + "/"))
                .ToListDataResult(baseParams.GetLoadParam(), this.Container, usePaging: !isExport);
        }

        public IQueryable<ViewWarningDoc> GetViewList()
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();

            var municipalityList = userManager.GetMunicipalityIds();

            return this.Container.ResolveDomain<ViewWarningDoc>().GetAll()
                .WhereIf(municipalityList.Count > 0,
                    x => x.MunicipalityId.HasValue && municipalityList.Contains(x.MunicipalityId.Value));
        }

        public IDataResult ListForStage(BaseParams baseParams)
        {
            var warningDocService = this.Container.ResolveDomain<WarningDoc>();
            var warningDocRealObjService = this.Container.ResolveDomain<WarningDocRealObj>();

            try
            {
                var stageId = baseParams.Params.GetAs("stageId", 0L);

                var dictRoAddress = warningDocRealObjService.GetAll()
                    .Where(x => x.WarningDoc.Stage.Id == stageId)
                    .Where(x => x.RealityObject != null)
                    .Select(x =>
                        new
                        {
                            WarningDocId = x.WarningDoc.Id,
                            x.RealityObject.Address
                        })
                    .AsEnumerable()
                    .GroupBy(x => x.WarningDocId)
                    .ToDictionary(x => x.Key, y => y.Select(x => x.Address).FirstOrDefault());

                return warningDocService.GetAll()
                    .Where(x => x.Stage.Id == stageId)
                    .Select(x => new { x.Id, x.TypeDocumentGji, x.DocumentDate, x.DocumentNumber, x.State })
                    .AsEnumerable()
                    .Select(x =>
                        new
                        {
                            x.Id,
                            DocumentId = x.Id,
                            x.TypeDocumentGji,
                            x.DocumentDate,
                            x.DocumentNumber,
                            x.State,
                            Address = dictRoAddress.ContainsKey(x.Id) ? dictRoAddress[x.Id] : null
                        })
                    .AsQueryable()
                    .ToListDataResult(baseParams.GetLoadParam(), this.Container);
            }
            finally
            {
                this.Container.Release(warningDocService);
                this.Container.Release(warningDocRealObjService);
            }
        }
    }
}