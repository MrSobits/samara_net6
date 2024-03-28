namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Entities;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class ROMCategoryViewModel : BaseViewModel<ROMCategory>
    {
        public override IDataResult List(IDomainService<ROMCategory> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var yearEnum = baseParams.Params.GetAs<string>("yearEnum");
            Dictionary<string, YearEnums> enumsDict = new Dictionary<string, YearEnums>();
            enumsDict.Add("2015", YearEnums.y2015);
            enumsDict.Add("2016", YearEnums.y2016);
            enumsDict.Add("2017", YearEnums.y2017);
            enumsDict.Add("2018", YearEnums.y2018);
            enumsDict.Add("2019", YearEnums.y2019);
            enumsDict.Add("2020", YearEnums.y2020);
            enumsDict.Add("2021", YearEnums.y2021);
            enumsDict.Add("2022", YearEnums.y2022);
            enumsDict.Add("2023", YearEnums.y2023);
            enumsDict.Add("2024", YearEnums.y2024);


            var data = domainService.GetAll()
                .Where(x=> x.YearEnums == enumsDict[yearEnum])
                .Select(x => new
                    {
                    x.Id,
                    Contragent = x.Contragent.Name,
                    x.Contragent.Ogrn,
                    x.Contragent.JuridicalAddress,
                    ContragentINN = x.Contragent.Inn,
                    x.KindKND,
                    x.CalcDate,
                    x.Result,
                    Inspector = x.Inspector.Fio,
                    x.YearEnums,
                    x.RiskCategory,
                    x.SeverityGroup,
                    x.ProbabilityGroup,
                    x.State
                })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}