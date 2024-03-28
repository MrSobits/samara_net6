namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using B4;
    using Entities;
    using Gkh.Authentification;
    using System.Collections.Generic;
    using B4.Utils;
    using System;

    public class SpecialAccountRowViewModel : BaseViewModel<SpecialAccountRow>
    {
        public IGkhUserManager UserManager { get; set; }

        public override IDataResult List(IDomainService<SpecialAccountRow> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var id = loadParams.Filter.GetAs("specialAccountReportId", 0L);
            //var id = loadParams..ContainsKey("specialAccountReportId")
            //                      ? baseParams.Params["specialAccountReportId"].ToLong()
            //                      : 0;

            var data = domainService.GetAll()
                 .Where(x => x.SpecialAccountReport.Id == id)
               .Select(x => new
               {
                   x.Id,
                   ContragentName = x.SpecialAccountReport.Contragent.Name,
                   ContragentInn = x.SpecialAccountReport.Contragent.Inn,
                   RealityObject = x.RealityObject.Address,
                   x.AmmountDebt,
                   x.Ballance,
                   x.Incoming,
                   x.SpecialAccountNum,
                   x.Transfer,
                   x.AccuracyArea,
                   x.Accured,
                   x.AccuredTotal,
                   x.Contracts,
                   x.IncomingTotal,
                   x.StartDate,
                   x.TransferTotal,
                   x.Tariff,
                   x.AmountDebtCredit,
                   x.AmountDebtForPeriod,
                   Perscent = GetPerscent(x.AccuredTotal, x.IncomingTotal),
                   TotalDebt = x.Accured + x.AccuredTotal - x.Incoming - x.IncomingTotal
                   //Perscent = x.AccuredTotal>0m? Decimal.Round(((x.AccuredTotal - x.IncomingTotal)/x.AccuredTotal)*100,2).ToString(): "Ошибка"
               })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public string GetPerscent(decimal accuredTotal, decimal incomingTotal)
        {
            string result = "";
            if (accuredTotal > 0)
            {
                decimal res = (accuredTotal - incomingTotal) / accuredTotal * 100;
                result = Decimal.Round(res, 2).ToString().Replace('.', ',');
            }
            else
            {
                result = "Ошибка";
            }
            return result;
        }
    }
}