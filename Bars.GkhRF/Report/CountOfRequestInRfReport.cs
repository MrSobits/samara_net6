namespace Bars.GkhRf.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using B4.Modules.Reports;
    using Bars.Gkh.Entities;
    using Bars.GkhRf.Entities;

    using Castle.Windsor;

    public class CountOfRequestInRfReport : BasePrintForm
    {

        #region Входящие параметры отчета
        private List<long> municipalityIds;
        private DateTime dateStart;
        private DateTime dateEnd;
        #endregion

        public Dictionary<string, string> dictNameState = new Dictionary<string, string>();

        public IWindsorContainer Container { get; set; }

        public CountOfRequestInRfReport() 
            : base(new ReportTemplateBinary(Properties.Resources.RegFond_RequestsCount))
        {
        }

        public override string Name
        {
            get
            {
                return "Количество заявок в Региональный фонд";
            }
        }

        public override string Desciption
        {
            get
            {
                return "Количество заявок в Региональный фонд";
            }
        }

        public override string GroupName
        {
            get
            {
                return "Ход капремонта";
            }
        }

        public override string ParamsController
        {
            get
            {
                return "B4.controller.report.CountOfRequestInRf";
            }
        }

        public override string RequiredPermission
        {
            get
            {
                return "Reports.RF.CountOfRequestInRf";
            }
        }

        public override void SetUserParams(BaseParams baseParams)
        {
            var strMunicipalIds = baseParams.Params.GetAs("municipalityIds", string.Empty);

            municipalityIds = !string.IsNullOrEmpty(strMunicipalIds) ? strMunicipalIds.Split(',').Select(x => x.ToLong()).ToList() : new List<long>();

            dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
        }

        public override string ReportGenerator { get; set; }

        public override void PrepareReport(ReportParams reportParams)
        {
            //получение Суммы по Id из "Перечисление ден средств средств рег. фонда"
            var querySumRequest = Container.Resolve<IDomainService<TransferFundsRf>>().GetAll().Select(x => new { x.RequestTransferRf.Id, x.Sum });

            //Получение списка полей из "Заявка на перечисление средств"
            var listRequestTransferRf = Container.Resolve<IDomainService<RequestTransferRf>>().GetAll()
                .Where(x => x.DateFrom != null && x.DateFrom >= dateStart && x.DateFrom <= dateEnd)
                .Where(x => x.ManagingOrganization != null && x.ManagingOrganization.Contragent != null)
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.ManagingOrganization.Contragent.Municipality.Id))
                .Select(x => new
                {
                    IdRequest = x.Id,
                    IdState = (long?)x.State.Id ?? -1,
                    MunicipalityId = (long?)x.ManagingOrganization.Contragent.Municipality.Id ?? -1,
                    MunicipalityName = x.ManagingOrganization.Contragent.Municipality.Name,
                    Sum = querySumRequest.Where(y => y.Id == x.Id).Sum(y => y.Sum)
                })
                .ToList();

            // Словарь Муниципальных образований из Id и имени 
            var dictMunName = Container.Resolve<IDomainService<Municipality>>().GetAll()
                .WhereIf(this.municipalityIds.Count > 0, x => this.municipalityIds.Contains(x.Id))
                .Select(x => new { x.Id, x.Name })
                .OrderBy(x => x.Name)
                .ToDictionary(x => x.Id, v => v.Name);

            // Словарь сгруппированный по Id "Заявка на перечисление средств" с полями количества и суммы
            var dictRequestTranferRf = listRequestTransferRf
                .GroupBy(x => x.MunicipalityId)
                .ToDictionary(
                    x => x.Key,
                    v => v.GroupBy(x => x.IdState)
                        .ToDictionary(
                            x => x.Key,
                            z => new RequestCountSumProxy
                                        {
                                            Count = z.Count(), 
                                            Sum = z.Sum(s => s.Sum).ToDecimal()
                                        }));
            
            dictNameState.Add("В работе ГИСУ", "ВработеГИСУ");
            dictNameState.Add("В работе ДК", "ВработеДК");
            dictNameState.Add("В работе Казань", "ВработеКазань");
            dictNameState.Add("В работе УК, ТСЖ, ЖСК", "ВработеУКТСЖЖСК");
            dictNameState.Add("Возврат", "Возврат");
            dictNameState.Add("Не принято ГИСУ", "НепринятоГИСУ");
            dictNameState.Add("Нет финансирования", "Нетфинансирования");
            dictNameState.Add("Обработка завершена", "Обработказавершена");
            dictNameState.Add("Подписано УК, ТСЖ, ЖСК", "ПодписаноУКТСЖЖСК");
            dictNameState.Add("Проверено ГИСУ", "ПровереноГИСУ");
            dictNameState.Add("Согласовано Казань", "СогласованоКазань");

            // Словарь статусов по именам
            var dictState = Container.Resolve<IDomainService<State>>().GetAll()
                .Where(x => dictNameState.Keys.Contains(x.Name) && x.TypeId == "rf_request_transfer")
                .Select(x => new { x.Name, x.Id })
                .AsEnumerable()
                .ToDictionary(k => k.Name, v => v.Id);

            var section = reportParams.ComplexReportParams.ДобавитьСекцию("Секция");

            var num = 0;
            foreach (var mu in dictMunName)
            {
                section.ДобавитьСтроку();
                section["Номер"] = ++num;
                section["Район"] = mu.Value;

                if (!dictRequestTranferRf.ContainsKey(mu.Key))
                {
                    continue;
                }

                foreach (var recordState in dictState.Where(rec => dictRequestTranferRf[mu.Key].ContainsKey(rec.Value)))
                {
                    this.FillSection(section, recordState.Key, dictRequestTranferRf[mu.Key][recordState.Value]);
                }

                section["Итог1"] = dictRequestTranferRf[mu.Key].Sum(rec => rec.Value.Count);
                section["Итог2"] = dictRequestTranferRf[mu.Key].Sum(rec => rec.Value.Sum);
            }

            // Итого
            reportParams.SimpleReportParams["Период"] = string.Format("с {0} до {1}", dateStart.ToShortDateString(), dateEnd.ToShortDateString());
            foreach (var recState in dictState)
            {                
                reportParams.SimpleReportParams[dictNameState[recState.Key] + "1Итого"] = dictRequestTranferRf.Where(record => record.Value.ContainsKey(recState.Value))
                                                                                                            .Sum(record => record.Value[recState.Value].Count);
                reportParams.SimpleReportParams[dictNameState[recState.Key] + "2Итого"] = dictRequestTranferRf.Where(record => record.Value.ContainsKey(recState.Value))
                                                                                                            .Sum(record => record.Value[recState.Value].Sum);

                reportParams.SimpleReportParams["Итог1Итого"] = dictRequestTranferRf.Sum(record => record.Value.Sum(x => x.Value.Count));
                reportParams.SimpleReportParams["Итог2Итого"] = dictRequestTranferRf.Sum(record => record.Value.Sum(x => x.Value.Sum));
            }
        }

        private void FillSection(Section section, string nameState, RequestCountSumProxy item)
        {
            section[dictNameState[nameState] + "1"] = item.Count;
            section[dictNameState[nameState] + "2"] = item.Sum;
        }

        private class RequestCountSumProxy
        {
            public int Count;

            public decimal Sum;
        }
    }
}