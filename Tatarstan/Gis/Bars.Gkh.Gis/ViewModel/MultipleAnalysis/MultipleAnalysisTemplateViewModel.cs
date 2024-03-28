namespace Bars.Gkh.Gis.ViewModel.MultipleAnalysis
{
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Utils;
    using Entities.Register.MultipleAnalysis;
    using NHibernate.Linq;

    public class MultipleAnalysisTemplateViewModel : BaseViewModel<MultipleAnalysisTemplate>
    {
        protected IRepository<Fias> FiasRepository;

        public MultipleAnalysisTemplateViewModel(
            IRepository<Fias> fiasRepository
            )
        {
            FiasRepository = fiasRepository;
        }

        public override IDataResult List(IDomainService<MultipleAnalysisTemplate> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var data = domainService
                .GetAll()
                .Filter(loadParam, Container)
                .Select(x => new MultipleAnalysisTemplateProxy
                {
                    Id = x.Id,
                    RealEstateTypeId = x.RealEstateType.Id,
                    RealEstateTypeName = x.RealEstateType.Name,
                    TypeCondition = x.TypeCondition,
                    Email = x.Email,
                    FormDay = x.FormDay,
                    LastFormDate = x.LastFormDate,
                    MunicipalAreaGuid = x.MunicipalAreaGuid,
                    MunicipalAreaName = "",
                    SettlementGuid = x.SettlementGuid,
                    SettlementName = "",
                    StreetGuid = x.StreetGuid,
                    StreetName = "",
                    MonthYear = x.MonthYear
                });

            // заполнение по guid'ам
            var filteredData = data.Paging(loadParam).ToList();
            filteredData
                .Select(x => x.MunicipalAreaGuid)
                .Distinct()
                .ForEach(x =>
                {
                    var fiasObject = FiasRepository.GetAll().FirstOrDefault(y => y.AOGuid == x && y.ActStatus == FiasActualStatusEnum.Actual);
                    if (fiasObject != null)
                    {
                        filteredData.Where(y => y.MunicipalAreaGuid == x)
                            .ForEach(
                                y => y.MunicipalAreaName =
                                    string.Format("{0}. {1}", fiasObject.ShortName, fiasObject.OffName));
                    }
                });
            filteredData
                .Select(x => x.SettlementGuid)
                .Distinct()
                .ForEach(x =>
                {
                    var fiasObject = FiasRepository.GetAll().FirstOrDefault(y => y.AOGuid == x && y.ActStatus == FiasActualStatusEnum.Actual);
                    if (fiasObject != null)
                    {
                        filteredData.Where(y => y.SettlementGuid == x)
                            .ForEach(
                                y => y.SettlementName =
                                    string.Format("{0}. {1}", fiasObject.ShortName, fiasObject.OffName));
                    }
                });
            filteredData
                .Select(x => x.StreetGuid)
                .Distinct()
                .ForEach(x =>
                {
                    var fiasObject = FiasRepository.GetAll().FirstOrDefault(y => y.AOGuid == x && y.ActStatus == FiasActualStatusEnum.Actual);
                    if (fiasObject != null)
                    {
                        filteredData.Where(y => y.StreetGuid == x)
                            .ForEach(
                                y => y.StreetName =
                                    string.Format("{0}. {1}", fiasObject.ShortName, fiasObject.OffName));
                    }
                });
            

            return new ListDataResult(filteredData, data.Count());
        }

        private class MultipleAnalysisTemplateProxy : MultipleAnalysisTemplate
        {
            public long RealEstateTypeId { get; set; }
            public string RealEstateTypeName { get; set; }
            public string MunicipalAreaName { get; set; }
            public string SettlementName { get; set; }
            public string StreetName { get; set; }
        }
    }
}