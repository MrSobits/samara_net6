namespace Bars.GkhRf.Report
{
    using System;
    using System.IO;
    using System.Data;
    using System.Linq;
    using System.Collections.Generic;
    using B4;
    using B4.DataAccess;
    using B4.Modules.Pivot;
    using B4.Modules.Pivot.Enum;
    using B4.Modules.Reports;
    using B4.Utils;
    using Gkh.Enums;
    using Gkh.Entities;
    using Enums;
    using Entities;
    using Castle.Windsor;

    /// <summary>
    /// Olap-отчет Журнал начислений и оплат 
    /// </summary>
    public class JournalAcPayment : IPrintForm, IPivotModel
    {
        #region Свойства

        public IWindsorContainer Container { get; set; }

        public string Params { get; set; }

        public object Data { get; set; }

        public string Name
        {
            get
            {
                return "Журнал начислений и оплат";
            }
        }

        public IList<string> ReportFormats { get; private set; }

        public string Desciption
        {
            get
            {
                return "Журнал начислений и оплат";
            }
        }

        public string GroupName
        {
            get
            {
                return "Региональный фонд";
            }
        }

        public string ParamsController
        {
            get
            {
                return "B4.controller.report.JournalAcPayment";
            }
        }

        public string RequiredPermission
        {
            get
            {
                return "Reports.RF.JournalAcPayment";
            }
        }

        #endregion

        public void SetUserParams(BaseParams baseParams)
        {
            
        }

        public string ReportGenerator { get; set; }

        public Stream GetTemplate()
        {
            throw new NotImplementedException();
        }

        public void PrepareReport(ReportParams reportParams)
        {

        }

        public void LoadData()
        {
            if (string.IsNullOrEmpty(Params))
            {
                return;
            }

            var dynDict = DynamicDictionary.FromString(Params);

            var municipalities = dynDict.ContainsKey("municipalityIds")
                                     ? dynDict["municipalityIds"].ToStr().Split("%2c+").Select(x => x.ToLong()).ToList()
                                     : new List<long>();

            var reportDateStart = dynDict.ContainsKey("reportDateStart")
                                      ? Uri.UnescapeDataString(dynDict["reportDateStart"].ToString()).ToDateTime()
                                      : DateTime.MinValue;

            var reportDateFinish = dynDict.ContainsKey("reportDateFinish")
                                       ? Uri.UnescapeDataString(dynDict["reportDateFinish"].ToString()).ToDateTime()
                                       : DateTime.MaxValue;

            var reportDateEnd = new DateTime(reportDateFinish.Year, reportDateFinish.Month, 1);

            var table = new DataTable();

            table.Columns.Add(new DataColumn("ManOrgGisu"));                         // УО по договору с ГИСУ
            table.Columns.Add(new DataColumn("DocumentNum"));                        // Номер договора
            table.Columns.Add(new DataColumn("DocumentDate", typeof(DateTime)));     // Дата договора
            table.Columns.Add(new DataColumn("AreaMkd", typeof(decimal)));           // Общая площадь МКД
            table.Columns.Add(new DataColumn("ManOrg"));                             // УО
            table.Columns.Add(new DataColumn("Inn"));                                // ИНН

            table.Columns.Add(new DataColumn("Municipality"));                       // МО
            table.Columns.Add(new DataColumn("Address"));                            // Адрес
            table.Columns.Add(new DataColumn("TypePayment"));                        // Вид начислений
            table.Columns.Add(new DataColumn("IncomeBalance", typeof(decimal)));     // Входящее сальдо
            table.Columns.Add(new DataColumn("Charge", typeof(decimal)));            // Начислено
            table.Columns.Add(new DataColumn("Recalculation", typeof(decimal)));     // Перерасчет за прошлый период
            table.Columns.Add(new DataColumn("Paid", typeof(decimal)));              // Оплачено
            table.Columns.Add(new DataColumn("OutgoingBalance", typeof(decimal)));   // Исходящее сальдо

            //информация по договорам регфонда
            var contractsRf = Container.Resolve<IRepository<ContractRfObject>>().GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                .Where(x => (x.IncludeDate <= reportDateEnd && (!x.ExcludeDate.HasValue || x.ExcludeDate >= reportDateEnd))
                    || (x.IncludeDate <= reportDateStart && (!x.ExcludeDate.HasValue || x.ExcludeDate >= reportDateStart)))
                .Where(y =>
                    Container.Resolve<IDomainService<PaymentItem>>().GetAll()
                        .Any(x => x.Payment.RealityObject.Id == y.RealityObject.Id
                            && reportDateStart <= x.ChargeDate
                            && reportDateEnd >= x.ChargeDate))
                .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        ManOrgGisu = x.ContractRf.ManagingOrganization.Contragent.Name,
                        x.ContractRf.DocumentNum,
                        x.ContractRf.DocumentDate,
                        x.IncludeDate,
                        x.ExcludeDate
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y
                    .Select(x => new
                        {
                            x.RoId,
                            x.ManOrgGisu,
                            x.DocumentDate,
                            x.DocumentNum,
                            PeriodDateStart = MaxDate(x.IncludeDate, reportDateStart),
                            PeriodDateEnd = MinDate(x.ExcludeDate, reportDateEnd)
                        })
                    .ToList());

            //информация по оплатам
            var paymentsRf = Container.Resolve<IRepository<PaymentItem>>().GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.Payment.RealityObject.Municipality.Id))
                .Where(x => reportDateStart <= x.ChargeDate && x.ChargeDate <= reportDateEnd)
                .Select(x => new
                    {
                        RoId = x.Payment.RealityObject.Id,
                        x.Payment.RealityObject.Address,
                        x.Payment.RealityObject.AreaMkd,
                        MuName = x.Payment.RealityObject.Municipality.Name,
                        x.ChargeDate,
                        x.TypePayment,
                        x.IncomeBalance,
                        x.ChargePopulation,
                        x.Recalculation,
                        x.PaidPopulation,
                        x.OutgoingBalance
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, z => z
                    .Select(x => new
                        {
                            x.Address,
                            x.AreaMkd,
                            x.MuName,
                            x.TypePayment,
                            x.IncomeBalance,
                            x.ChargePopulation,
                            x.Recalculation,
                            x.PaidPopulation,
                            x.OutgoingBalance,
                            x.ChargeDate
                        })
                    .ToList());

            //информация по договорам управления
            var contractManagOrg = Container.Resolve<IRepository<ManOrgContractRealityObject>>().GetAll()
                .WhereIf(municipalities.Count > 0, x => municipalities.Contains(x.RealityObject.Municipality.Id))
                .Where(x => x.ManOrgContract.TypeContractManOrgRealObj != TypeContractManOrg.DirectManag)
                .Where(x => (x.ManOrgContract.StartDate <= reportDateEnd && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= reportDateEnd))
                    || (x.ManOrgContract.StartDate <= reportDateStart && (!x.ManOrgContract.EndDate.HasValue || x.ManOrgContract.EndDate >= reportDateStart)))
                .Select(x => new
                    {
                        RoId = x.RealityObject.Id,
                        ManOrg = x.ManOrgContract.ManagingOrganization.Contragent.Name,
                        x.ManOrgContract.ManagingOrganization.Contragent.Inn,
                        x.ManOrgContract.StartDate,
                        x.ManOrgContract.EndDate,
                        ContractId = x.ManOrgContract.Id,
                        x.ManOrgContract.TypeContractManOrgRealObj
                    })
                .AsEnumerable()
                .GroupBy(x => x.RoId)
                .ToDictionary(x => x.Key, y => y.Select(x => new 
                        {
                            x.ManOrg,
                            x.Inn,
                            x.StartDate,
                            x.EndDate,
                            x.ContractId,
                            x.TypeContractManOrgRealObj
                        })
                    .ToList());

            var listContractTsj = new List<ContractInfo>();

            var contractIds = new List<long>();

            foreach (var contract in contractManagOrg)
                contractIds.AddRange(contract.Value.Select(x => x.ContractId).ToList());

            contractIds = contractIds.Distinct().ToList();

            var count = contractIds.Count;

            var contractJskTsjService = Container.Resolve<IDomainService<ManOrgJskTsjContract>>();

            for (int i = 0; i < count; i += 1000)
            {
                var tempIds = contractIds.Skip(i).Take(count - i < 1000 ? count - i : 1000).ToList();

                listContractTsj.AddRange(contractJskTsjService.GetAll()
                    .Where(x => tempIds.Contains(x.Id) && x.IsTransferredManagement == YesNoNotSet.Yes && x.ManOrgTransferredManagement.Contragent.Name != null)
                    .Select(x => new ContractInfo
                        {
                            Id = x.Id,
                            ManOrgTransferManagName = x.ManOrgTransferredManagement.Contragent.Name
                        })
                    .AsEnumerable());
            }

            var dictContractTsj = listContractTsj
                .GroupBy(x => x.Id)
                .ToDictionary(x => x.Key, y => y.Select(x => x.ManOrgTransferManagName).FirstOrDefault());

            var keys = paymentsRf.Keys;

            var result = new List<PaymentInfo>();

            foreach (var roId in keys)
            {
                foreach (var payment in paymentsRf[roId])
                {
                    var info = new PaymentInfo
                        {
                            Address = payment.Address,
                            Municipality = payment.MuName,
                            AreaMkd = payment.AreaMkd,
                            TypePayment = payment.TypePayment,
                            ChargePopulation = payment.ChargePopulation,
                            IncomeBalance = payment.IncomeBalance,
                            OutgoingBalance = payment.OutgoingBalance,
                            PaidPopulation = payment.PaidPopulation,
                            Recalculation = payment.Recalculation
                        };

                    var cd = payment.ChargeDate;

                    if (contractsRf.ContainsKey(roId))
                    {
                        var firstContract = contractsRf[roId].FirstOrDefault(x => x.PeriodDateStart <= cd && x.PeriodDateEnd >= cd);

                        if (firstContract != null)
                        {
                            info.DocumentDate = firstContract.DocumentDate;
                            info.DocumentNum = firstContract.DocumentNum;
                            info.ManOrgGisu = firstContract.ManOrgGisu;
                        }
                    }

                    if (contractManagOrg.ContainsKey(roId))
                    {
                        var firstContract = contractManagOrg[roId].FirstOrDefault(x => x.StartDate <= cd && (!x.EndDate.HasValue || x.EndDate >= cd));

                        if (firstContract != null)
                        {
                            info.ManOrg = firstContract.ManOrg;
                            info.Inn = firstContract.Inn;

                            if (firstContract.TypeContractManOrgRealObj == TypeContractManOrg.JskTsj 
                                && dictContractTsj.ContainsKey(firstContract.ContractId)
                                && !string.IsNullOrEmpty(dictContractTsj[firstContract.ContractId]))
                            {
                                info.ManOrg = string.Format("{0} ({1})", info.ManOrg, dictContractTsj[firstContract.ContractId]);
                            }
                        }
                    }

                    result.Add(info);
                }
            }

            foreach (var paymentInfo in result)
            {
                var row = table.NewRow();

                row["Address"] = paymentInfo.Address;
                row["Municipality"] = paymentInfo.Municipality;
                row["ManOrgGisu"] = paymentInfo.ManOrgGisu;
                row["DocumentNum"] = paymentInfo.DocumentNum;
                row["DocumentDate"] = paymentInfo.DocumentDate.HasValue ? paymentInfo.DocumentDate.Value.Date : DateTime.MinValue;
                row["AreaMkd"] = paymentInfo.AreaMkd.ToDecimal().RoundDecimal(2);
                row["ManOrg"] = paymentInfo.ManOrg;
                row["Inn"] = paymentInfo.Inn;
                row["TypePayment"] = paymentInfo.TypePayment.GetEnumMeta().Display;
                row["OutgoingBalance"] = paymentInfo.OutgoingBalance.ToDecimal().RoundDecimal(2);
                row["Paid"] = paymentInfo.PaidPopulation.ToDecimal().RoundDecimal(2);
                row["Charge"] = paymentInfo.ChargePopulation.ToDecimal().RoundDecimal(2);
                row["IncomeBalance"] = paymentInfo.IncomeBalance.ToDecimal().RoundDecimal(2);
                row["Recalculation"] = paymentInfo.Recalculation.ToDecimal().RoundDecimal(2);

                table.Rows.Add(row);
            }

            Data = table;
        }

        public PivotConfiguration GetConfiguration()
        {
            var numericCellFormat = new CellFormat { FormatString = "n", FormatType = FormatType.Numeric };
            var datetimeCellFormat = new CellFormat { FormatString = "d", FormatType = FormatType.DateTime };

            var datetimeValueFormat = new ValueFormat { FormatString = "d", FormatType = FormatType.DateTime };

            return new PivotConfiguration
                {
                    ModelName = "RF Report.JournalAcPayment",
                    Name = "JournalAcPayment",
                    Fields =
                        new List<Field>
                            {
                                new Field
                                    {
                                        Area = Area.RowArea,
                                        DisplayName = "Адрес",
                                        Name = "Address"
                                    },
                                new Field
                                    {
                                        Area = Area.RowArea,
                                        DisplayName = "Муниципальное образование",
                                        Name = "Municipality"
                                    },
                                new Field
                                    {
                                        Area = Area.FilterArea,
                                        DisplayName = "УО по договору с ГИСУ",
                                        Name = "ManOrgGisu"
                                    },
                                new Field
                                    {
                                        Area = Area.FilterArea,
                                        DisplayName = "Номер договора",
                                        Name = "DocumentNum"
                                    },
                                new Field
                                    {
                                        Area = Area.FilterArea,
                                        DisplayName = "Дата договора",
                                        Name = "DocumentDate",
                                        SummaryType = SummaryType.Max,
                                        CellFormat = datetimeCellFormat,
                                        ValueFormat = datetimeValueFormat
                                    },
                                new Field
                                    {
                                        Area = Area.FilterArea,
                                        DisplayName = "Общая площадь МКД",
                                        Name = "AreaMkd",
                                        SummaryType = SummaryType.Sum,
                                        CellFormat = numericCellFormat
                                    },
                                new Field
                                    {
                                        Area = Area.FilterArea,
                                        DisplayName = "Управляющая организация",
                                        Name = "ManOrg"
                                    },
                                new Field
                                    {
                                        Area = Area.FilterArea,
                                        DisplayName = "ИНН",
                                        Name = "Inn"
                                    },
                                new Field
                                    {
                                        Area = Area.ColumnArea,
                                        DisplayName = "Вид начислений",
                                        Name = "TypePayment"
                                    },
                                new Field
                                    {
                                        Area = Area.DataArea,
                                        DisplayName = "Исходящее сальдо",
                                        Name = "OutgoingBalance",
                                        SummaryType = SummaryType.Sum,
                                        CellFormat = numericCellFormat
                                    },
                                new Field
                                    {
                                        Area = Area.DataArea,
                                        DisplayName = "Оплачено",
                                        Name = "Paid",
                                        SummaryType = SummaryType.Sum,
                                        CellFormat = numericCellFormat
                                    },
                                new Field
                                    {
                                        Area = Area.DataArea,
                                        DisplayName = "Перерасчет за прошлый период",
                                        Name = "Recalculation",
                                        SummaryType = SummaryType.Sum,
                                        CellFormat = numericCellFormat
                                    },
                                new Field
                                    {
                                        Area = Area.DataArea,
                                        DisplayName = "Начислено",
                                        Name = "Charge",
                                        SummaryType = SummaryType.Sum,
                                        CellFormat = numericCellFormat
                                    },
                                new Field
                                    {
                                        Area = Area.DataArea,
                                        DisplayName = "Входящее сальдо",
                                        Name = "IncomeBalance",
                                        SummaryType = SummaryType.Sum,
                                        CellFormat = numericCellFormat
                                    }
                            }
                };
        }

        private static DateTime? MinDate(DateTime? date1, DateTime? date2)
        {
            var d1 = date1.HasValue ? date1.ToDateTime() : DateTime.MaxValue;
            var d2 = date2.HasValue ? date2.ToDateTime() : DateTime.MaxValue;

            return d1 >= d2 ? date2 : date1;
        }

        private static DateTime? MaxDate(DateTime? date1, DateTime? date2)
        {
            var d1 = date1.HasValue ? date1.ToDateTime() : DateTime.MinValue;
            var d2 = date2.HasValue ? date2.ToDateTime() : DateTime.MinValue;

            return d1 >= d2 ? date1 : date2;
        }

        private class PaymentInfo
        {
            public string Address;
            public string Municipality;
            public decimal? AreaMkd;
            public string ManOrgGisu;
            public string DocumentNum;
            public DateTime? DocumentDate;
            public TypePayment TypePayment = 0;
            public decimal? IncomeBalance = 0;
            public decimal? ChargePopulation = 0;
            public decimal? Recalculation = 0;
            public decimal? PaidPopulation = 0;
            public decimal? OutgoingBalance = 0;
            public string ManOrg;
            public string Inn;
        }

        private class ContractInfo
        {
            public long Id = 0;
            public string ManOrgTransferManagName = null;
        }
    }
}