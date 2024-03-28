namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Bars.Gkh.Entities;
    using Gkh.Authentification;
    using System.Collections.Generic;
    using B4.Utils;
    using System;
    using Enums;

    public class SpecialAccountReportViewModel : BaseViewModel<SpecialAccountReport>
    {
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult List(IDomainService<SpecialAccountReport> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
           
           
            var contragentList = UserManager.GetContragentIds();
            
            if (contragentList.Contains(84367374))
                contragentList = new List<long>();

            var data = domainService.GetAll()
                 .WhereIf(contragentList.Any(), x => contragentList.Contains(x.Contragent.Id))
               .Select(x => new
               {
                   x.Id,
                   Contragent = x.Contragent.Name,
                   x.MonthEnums,
                   x.YearEnums,
                   x.Sertificate,
                   x.Author,
                   x.Contragent.Inn,
                   x.SignedXMLFile,
                   x.DateAccept,
                   ObjectEditDate = x.SignedXMLFile != null || x.Sertificate == "Отчет сдан в письменном виде" ? x.ObjectEditDate : DateTime.Now
               })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public override IDataResult Get(IDomainService<SpecialAccountReport> domain, BaseParams baseParams)
        {
            Dictionary<string, string> MonthEnumsDict = new Dictionary<string, string>();
            Dictionary<string, string> YearEnumsDict = new Dictionary<string, string>();
            MonthEnumsDict.Add("10", MonthEnums.January.GetDisplayName());
            MonthEnumsDict.Add("20", MonthEnums.February.GetDisplayName());
            MonthEnumsDict.Add("30", MonthEnums.March.GetDisplayName());
            MonthEnumsDict.Add("40", MonthEnums.April.GetDisplayName());
            MonthEnumsDict.Add("50", MonthEnums.May.GetDisplayName());
            MonthEnumsDict.Add("60", MonthEnums.June.GetDisplayName());
            MonthEnumsDict.Add("70", MonthEnums.July.GetDisplayName());
            MonthEnumsDict.Add("80", MonthEnums.August.GetDisplayName());
            MonthEnumsDict.Add("90", MonthEnums.September.GetDisplayName());
            MonthEnumsDict.Add("100", MonthEnums.October.GetDisplayName());
            MonthEnumsDict.Add("110", MonthEnums.November.GetDisplayName());
            MonthEnumsDict.Add("120", MonthEnums.December.GetDisplayName());
            MonthEnumsDict.Add("140", MonthEnums.Quarter1.GetDisplayName());
            MonthEnumsDict.Add("240", MonthEnums.Quarter2.GetDisplayName());
            MonthEnumsDict.Add("340", MonthEnums.Quarter3.GetDisplayName());
            MonthEnumsDict.Add("440", MonthEnums.Quarter4.GetDisplayName());


            YearEnumsDict.Add("2018", YearEnums.y2018.GetDisplayName());
            YearEnumsDict.Add("2019", YearEnums.y2019.GetDisplayName());
            YearEnumsDict.Add("2020", YearEnums.y2020.GetDisplayName());
            YearEnumsDict.Add("2021", YearEnums.y2021.GetDisplayName());
            YearEnumsDict.Add("2022", YearEnums.y2022.GetDisplayName());
            YearEnumsDict.Add("2023", YearEnums.y2023.GetDisplayName());
            YearEnumsDict.Add("2024", YearEnums.y2024.GetDisplayName());
            YearEnumsDict.Add("2025", YearEnums.y2025.GetDisplayName());
            YearEnumsDict.Add("2026", YearEnums.y2026.GetDisplayName());
            YearEnumsDict.Add("2027", YearEnums.y2027.GetDisplayName());
            YearEnumsDict.Add("2028", YearEnums.y2028.GetDisplayName());
            YearEnumsDict.Add("2029", YearEnums.y2029.GetDisplayName());


            var loadParams = baseParams.GetLoadParam();
            int id = Convert.ToInt32(baseParams.Params.Get("id"));

            var data = domain.GetAll()
                 .WhereIf(id != 0, x => x.Id == id)
                   .Select(x => new
                   {
                       x.Id,
                       x.Contragent,
                       ContragentName = x.Contragent.Name,                     
                       x.Contragent.Inn,
                       x.DateAccept,
                       OP = MonthEnumsDict[x.MonthEnums.ToString()] + " " + YearEnumsDict[x.YearEnums.ToString()],
                       x.Sertificate,
                       x.AmmountMeasurement,
                       x.SignedXMLFile,
                       x.Signature,
                       x.Certificate,
                       x.File,
                       x.Executor
                   })
                .AsQueryable()
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}