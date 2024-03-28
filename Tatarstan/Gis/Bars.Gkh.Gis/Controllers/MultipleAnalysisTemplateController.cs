namespace Bars.Gkh.Gis.Controllers
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using DomainService.Analysis;
    using DomainService.RealEstate;
    using Entities.IndicatorServiceComparison;
    using Entities.RealEstate.GisRealEstateType;
    using Entities.Register.MultipleAnalysis;
    using Enum;

    public class MultipleAnalysisTemplateController : B4.Alt.BaseDataController<MultipleAnalysisTemplate>
    {
        private class TemplateProxy
        {
            public long id { get; set; }
            public long typeHouse { get; set; }
            public GisTypeCondition typeCondition { get; set; }
            public short formDay { get; set; }
            public string email { get; set; }
            public List<MultipleAnalysisProxy> indicators { get; set; }
            public string municipalArea { get; set; }
            public string settlement { get; set; }
            public string street { get; set; }
            public DateTime? monthYear { get; set; }
        }

        protected IRepository<MultipleAnalysisIndicator> MultipleAnalysisIndicatorRepository;
        protected IRealEstateTypeCommonParamService RealEstateTypeCommonParamService;
        protected IRepository<GisRealEstateType> RealEstateTypeRepository;
        protected IRepository<IndicatorServiceComparison> IndicatorGroupingRepository;
        protected IRepository<MultipleAnalysisTemplate> MultipleAnalysisTemplateRepository;
        protected IMultipleAnalysisService MultipleAnalysisService;

        public MultipleAnalysisTemplateController(
            IRepository<MultipleAnalysisIndicator> multipleAnalysisIndicatorRepository,
            IRealEstateTypeCommonParamService realEstateTypeCommonParamService,
            IRepository<GisRealEstateType> realEstateTypeRepository,
            IRepository<IndicatorServiceComparison> indicatorGroupingRepository,
            IRepository<MultipleAnalysisTemplate> multipleAnalysisTemplateRepository,
            IMultipleAnalysisService multipleAnalysisService)
        {
            MultipleAnalysisIndicatorRepository = multipleAnalysisIndicatorRepository;
            RealEstateTypeCommonParamService = realEstateTypeCommonParamService;
            RealEstateTypeRepository = realEstateTypeRepository;
            MultipleAnalysisService = multipleAnalysisService;
            IndicatorGroupingRepository = indicatorGroupingRepository;
            MultipleAnalysisTemplateRepository = multipleAnalysisTemplateRepository;
        }

        public ActionResult GetIndicators(BaseParams baseParams)
        {
            return new JsonNetResult(MultipleAnalysisService.GetTemplateIndicatorTree(baseParams));
        }

        public ActionResult GetReport(BaseParams baseParams)
        {
            var typeHouse = baseParams.Params.GetAs<long>("typeHouse");
            var typeCondition = baseParams.Params.GetAs<GisTypeCondition>("typeCondition");
            var date = baseParams.Params.GetAs<DateTime>("date");
            var indicators = baseParams.Params.GetAs<List<MultipleAnalysisProxy>>("indicators");
            var municipalArea = baseParams.Params.GetAs<string>("municipalArea");
            var settlement = baseParams.Params.GetAs<string>("settlement");
            var street = baseParams.Params.GetAs<string>("street");

            IndicatorGroupingRepository
                .GetAll()
                .ToList()
                .Where(x => indicators.Any(y => x.Id == y.Id))
                .ToList()
                .ForEach(x => indicators.First(y => x.Id == y.Id).IndicatorServiceComparison = x);

            var houses = MultipleAnalysisService.GetHouseIdByType(typeHouse, municipalArea, settlement, street);
            var columns = MultipleAnalysisService.GetReportColumns(indicators, " ");
            var data = MultipleAnalysisService.GetReportData(indicators, houses, date, typeCondition);
            var totalHouseCount = houses.Count;

            return
                new JsonNetResult(
                    new
                    {
                        success = true,
                        columns = columns.Select(x => new {dataIndex = x.Key, text = x.Value}),
                        data,
                        totalHouseCount
                    });
        }

        public ActionResult SaveTemplate(BaseParams baseParams)
        {
            var proxy = baseParams.Params.GetAs<TemplateProxy>("record");

            var realEstateType = RealEstateTypeRepository.Get(proxy.typeHouse);
            if (realEstateType == null)
            {
                return JsonNetResult.Failure("Не найден тип дома!");
            }
            //if (proxy.email.IsEmpty())
            //{
            //    return JsonNetResult.Failure("Не указан E-mail");
            //}
            if (proxy.indicators.Count == 0)
            {
                return JsonNetResult.Failure("Не выбраны индикаторы!");
            }

            var template = proxy.id == 0
                ? new MultipleAnalysisTemplate()
                : MultipleAnalysisTemplateRepository.Get(proxy.id);
            template.RealEstateType = realEstateType;
            template.TypeCondition = proxy.typeCondition;
            template.FormDay = proxy.formDay;
            template.Email = proxy.email;
            template.MunicipalAreaGuid = proxy.municipalArea;
            template.SettlementGuid = proxy.settlement;
            template.StreetGuid = proxy.street;
            template.MonthYear = proxy.monthYear;

            if (proxy.id == 0)
            {
                MultipleAnalysisTemplateRepository.Save(template);
            }
            else
            {
                MultipleAnalysisTemplateRepository.Update(template);
            }

            if (template.Id == 0)
            {
                return JsonNetResult.Failure("Не удалось сохранить шаблон!");
            }

            if (proxy.id != 0)
            {
                // удаляем старые индикаторы
                var oldIndicators =
                    MultipleAnalysisIndicatorRepository.GetAll()
                        .Where(x => x.MultipleAnalysisTemplate.Id == proxy.id)
                        .ToList();
                oldIndicators.ForEach(x =>
                    MultipleAnalysisIndicatorRepository.Delete(x.Id)
                    );
            }

            foreach (var indicator in proxy.indicators)
            {
                var newIndicator = new MultipleAnalysisIndicator
                {
                    MultipleAnalysisTemplate = template,
                    IndicatorServiceComparison = IndicatorGroupingRepository.Get(indicator.Id),
                    MinValue = indicator.MinValue,
                    MaxValue = indicator.MaxValue,
                    DeviationPercent = indicator.DeviationPercent,
                    ExactValue = indicator.ExactValue
                };
                MultipleAnalysisIndicatorRepository.Save(newIndicator);
            }

            return JsonNetResult.Success;
        }

        public ActionResult Delete(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            try
            {
                var indicators =
                    MultipleAnalysisIndicatorRepository.GetAll()
                        .Where(x => x.MultipleAnalysisTemplate.Id == id)
                        .ToList();
                indicators.ForEach(x =>
                    MultipleAnalysisIndicatorRepository.Delete(x.Id)
                    );
                MultipleAnalysisTemplateRepository.Delete(id);
                return JsonNetResult.Success;
            }
            catch (Exception)
            {
                return JsonNetResult.Failure("Ошибка удаления шаблона!");
            }
        }

        public ActionResult ListFiasArea(BaseParams baseParams)
        {
            var result = (ListDataResult)MultipleAnalysisService.ListFiasArea(baseParams);
            return new JsonListResult((IList)result.Data, result.TotalCount);
        }
    }
}