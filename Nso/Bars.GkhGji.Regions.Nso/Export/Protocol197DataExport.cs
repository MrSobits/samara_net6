namespace Bars.GkhGji.Regions.Nso.Export
{
	using System;
	using System.Collections;
	using System.Linq;
	using Bars.B4;
	using Bars.B4.Modules.DataExport.Domain;
	using Bars.B4.Utils;
	using Bars.GkhGji.DomainService;
	using Bars.GkhGji.Regions.Nso.DomainService;

	public class Protocol197DataExport : BaseDataExportService
    {
		public IProtocol197Service Protocol197Service { get; set; }

        public override IList GetExportData(BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.ContainsKey("dateStart")
                                   ? baseParams.Params["dateStart"].To<DateTime>()
                                   : DateTime.MinValue;

            var dateEnd = baseParams.Params.ContainsKey("dateEnd")
                                   ? baseParams.Params["dateEnd"].To<DateTime>()
                                   : DateTime.MaxValue;

            var realityObjectId = baseParams.Params.ContainsKey("realityObjectId")
                                   ? baseParams.Params["realityObjectId"].ToLong()
                                   : 0;

			return Protocol197Service.GetViewList()
                .WhereIf(dateStart != DateTime.MinValue, x => x.DocumentDate >= dateStart)
                .WhereIf(dateEnd != DateTime.MaxValue, x => x.DocumentDate <= dateEnd)
                .WhereIf(realityObjectId > 0, x => x.RealityObjectIds.Contains("/" + realityObjectId.ToString() + "/"))
                .Filter(loadParam, Container)
                .Order(loadParam)
                .Select(x => new
                {
					x.Id,
					x.State,
					x.ContragentName,
					MunicipalityNames = x.MunicipalityId != null ? x.MunicipalityNames : x.ContragentMuName,
					MoSettlement = x.MoNames,
					PlaceName = x.PlaceNames,
					MunicipalityId = x.MunicipalityId ?? x.ContragentMuId,
					x.TypeExecutant,
					x.CountViolation,
					x.InspectorNames,
					x.DocumentDate,
					x.DocumentNumber,
					x.DocumentNum,
					x.InspectionId,
					x.TypeBase,
					x.TypeDocumentGji
                })
                .ToList();
        }
    }
}