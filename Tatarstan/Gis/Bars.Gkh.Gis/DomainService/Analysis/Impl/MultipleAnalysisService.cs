namespace Bars.Gkh.Gis.DomainService.Analysis.Impl
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Net.Mail;
    using B4;
    using B4.DataAccess;
    using B4.Modules.FIAS;
    using B4.Modules.Messenger;
    using B4.Utils;

    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Entities;
    using Bars.B4.Modules.Analytics.Reports;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Modules.Analytics.Reports.Enums;

    using Castle.Windsor;
    using Entities.IndicatorServiceComparison;
    using Entities.RealEstate.GisRealEstateType;
    using Entities.Register.HouseRegister;
    using Entities.Register.HouseServiceRegister;
    using Entities.Register.MultipleAnalysis;
    using Enum;
    using Enums;
    using Gkh.Entities;
    using Indicator;
    using Properties;
    using RealEstate;

    public class MultipleAnalysisService : IMultipleAnalysisService
    {
        protected IWindsorContainer Container;
        protected IMailSender MailSender;
        protected IIndicatorService IndicatorService;
        protected IRepository<MultipleAnalysisIndicator> MultipleAnalysisIndicatorRepository;
        protected IRepository<HouseServiceRegister> HouseServiceRegisterRepository;
        protected IRepository<GisRealEstateType> RealEstateTypeRepository;
        protected IRepository<IndicatorServiceComparison> IndicatorGroupingRepository;
        protected IRepository<MultipleAnalysisTemplate> MultipleAnalysisTemplateRepository;
        protected IRealEstateTypeCommonParamService RealEstateTypeCommonParamService;
        protected BackgroundWorker BackgroundWorker;
        protected IRepository<FiasAddress> FiasAddressRepository;
        protected IRepository<HouseRegister> HouseRepository;
        protected IRepository<Fias> FiasRepository;
        protected IRepository<Municipality> MunicipalityRepository;

        public MultipleAnalysisService(
            IWindsorContainer container,
            IMailSender mailSender,
            IIndicatorService indicatorService,
            IRepository<MultipleAnalysisIndicator> multipleAnalysisIndicatorRepository,
            IRepository<HouseServiceRegister> houseServiceRegisterRepository,
            IRepository<GisRealEstateType> realEstateTypeRepository,
            IRepository<IndicatorServiceComparison> indicatorGroupingRepository,
            IRepository<MultipleAnalysisTemplate> multipleAnalysisTemplateRepository,
            IRealEstateTypeCommonParamService realEstateTypeCommonParamService,
            IRepository<FiasAddress> fiasAddressRepository,
            IRepository<HouseRegister> houseRepository,
            IRepository<Fias> fiasRepository,
            IRepository<Municipality> municipalityRepository)
        {
            Container = container;
            MailSender = mailSender;
            IndicatorService = indicatorService;
            MultipleAnalysisIndicatorRepository = multipleAnalysisIndicatorRepository;
            HouseServiceRegisterRepository = houseServiceRegisterRepository;
            RealEstateTypeRepository = realEstateTypeRepository;
            RealEstateTypeCommonParamService = realEstateTypeCommonParamService;
            IndicatorGroupingRepository = indicatorGroupingRepository;
            MultipleAnalysisTemplateRepository = multipleAnalysisTemplateRepository;
            FiasAddressRepository = fiasAddressRepository;
            HouseRepository = houseRepository;
            FiasRepository = fiasRepository;
            MunicipalityRepository = municipalityRepository;
        }

        #region Методы анализа
        public IList<MultipleAnalysisIndicatorProxy> AnalyzeMinMaxValue(IList<HouseServiceRegister> houseService, GisTypeIndicator indicator, decimal? min, decimal? max)
        {
            Check(houseService);

            IEnumerable<MultipleAnalysisIndicatorProxy> analizedHouses;

            switch (indicator)
            {
                case GisTypeIndicator.Volume:
                    analizedHouses = houseService
                        .Where(x => x.TotalVolume.HasValue)
                        .GroupBy(x => x.House)
                        .Select(x => new MultipleAnalysisIndicatorProxy
                        {
                            House = x.Key,
                            GisTypeIndicator = indicator,
                            Value = x.Sum(y => y.TotalVolume.Value)
                        });
                    break;
                case GisTypeIndicator.Charge:
                case GisTypeIndicator.SummaryCharge:
                    analizedHouses = houseService
                        .Where(x => x.Charge.HasValue)
                        .GroupBy(x => x.House)
                        .Select(x => new MultipleAnalysisIndicatorProxy
                        {
                            House = x.Key,
                            GisTypeIndicator = indicator,
                            Value = x.Sum(y => y.Charge.Value)
                        });
                    break;
                case GisTypeIndicator.Payment:
                case GisTypeIndicator.SummaryPayment:
                    analizedHouses = houseService
                        .Where(x => x.Payment.HasValue)
                        .GroupBy(x => x.House)
                        .Select(x => new MultipleAnalysisIndicatorProxy
                        {
                            House = x.Key,
                            GisTypeIndicator = indicator,
                            Value = x.Sum(y => y.Payment.Value)
                        });
                    break;
                case GisTypeIndicator.CoefOdn:
                    analizedHouses = houseService
                        .Where(x => x.CoefOdn.HasValue)
                        .GroupBy(x => x.House)
                        .Select(x => new MultipleAnalysisIndicatorProxy
                        {
                            House = x.Key,
                            GisTypeIndicator = indicator,
                            Value = x.Sum(y => y.CoefOdn.Value)
                        });
                    break;
                case GisTypeIndicator.DistibutedVolume:
                    analizedHouses = houseService
                        .Where(x => x.VolumeDistributed.HasValue)
                        .GroupBy(x => x.House)
                        .Select(x => new MultipleAnalysisIndicatorProxy
                        {
                            House = x.Key,
                            GisTypeIndicator = indicator,
                            Value = x.Sum(y => y.VolumeDistributed.Value)
                        });
                    break;
                case GisTypeIndicator.NotDistibutedVolume:
                    analizedHouses = houseService
                        .Where(x => x.VolumeNotDistributed.HasValue)
                        .GroupBy(x => x.House)
                        .Select(x => new MultipleAnalysisIndicatorProxy
                        {
                            House = x.Key,
                            GisTypeIndicator = indicator,
                            Value = x.Sum(y => y.VolumeNotDistributed.Value)
                        });
                    break;
                default:
                    throw new NotImplementedException("Не реализован анализ для данного индикатора!");
            }

            return analizedHouses
                        .Where(x => min.HasValue && x.Value < min.Value || max.HasValue && x.Value > max.Value)
                        .ToList();
        }

        public IList<MultipleAnalysisIndicatorProxy> AnalyzeDeviationPercent(IList<HouseServiceRegister> houseService, GisTypeIndicator indicator, decimal percent)
        {
            Check(houseService);

            decimal average = 0.0M;
            int count;

            switch (indicator)
            {
                case GisTypeIndicator.Volume:
                    count = houseService.Count(x => x.TotalVolume.HasValue);
                    if (count == 0) break;
                    average = houseService.Where(x => x.TotalVolume.HasValue).Sum(x => x.TotalVolume.Value) /
                              houseService.Count(x => x.TotalVolume.HasValue);
                    break;
                case GisTypeIndicator.Charge:
                case GisTypeIndicator.SummaryCharge:
                    count = houseService.Count(x => x.Charge.HasValue);
                    if (count == 0) break;
                    average = houseService.Where(x => x.Charge.HasValue).Sum(x => x.Charge.Value) / houseService.Count(x => x.Charge.HasValue);
                    break;
                case GisTypeIndicator.Payment:
                case GisTypeIndicator.SummaryPayment:
                    count = houseService.Count(x => x.Payment.HasValue);
                    if (count == 0) break;
                    average = houseService.Where(x => x.Payment.HasValue).Sum(x => x.Payment.Value) / houseService.Count(x => x.Payment.HasValue);
                    break;
                case GisTypeIndicator.CoefOdn:
                    count = houseService.Count(x => x.CoefOdn.HasValue);
                    if (count == 0) break;
                    average = houseService.Where(x => x.CoefOdn.HasValue).Sum(x => x.CoefOdn.Value) / houseService.Count(x => x.CoefOdn.HasValue);
                    break;
                case GisTypeIndicator.DistibutedVolume:
                    count = houseService.Count(x => x.VolumeDistributed.HasValue);
                    if (count == 0) break;
                    average = houseService.Where(x => x.VolumeDistributed.HasValue).Sum(x => x.VolumeDistributed.Value) /
                              houseService.Count(x => x.VolumeDistributed.HasValue);
                    break;
                case GisTypeIndicator.NotDistibutedVolume:
                    count = houseService.Count(x => x.VolumeNotDistributed.HasValue);
                    if (count == 0) break;
                    average =
                        houseService.Where(x => x.VolumeNotDistributed.HasValue).Sum(x => x.VolumeNotDistributed.Value) /
                        houseService.Count();
                    break;
                default:
                    throw new NotImplementedException("Не реализован анализ для данного индикатора!");
            }

            var min = average * (100 - percent) / 100;
            var max = average * (100 + percent) / 100;
            return AnalyzeMinMaxValue(houseService, indicator, min, max);
        }

        public IList<MultipleAnalysisIndicatorProxy> AnalyzeExactValue(IList<HouseServiceRegister> houseService, GisTypeIndicator indicator, decimal value)
        {
            Check(houseService);

            const decimal tolerance = 0.0000000001M;
            var min = value - tolerance;
            var max = value + tolerance;

            return AnalyzeMinMaxValue(houseService, indicator, min, max);
        }
        #endregion

        public IList<MultipleAnalysisGroupProxy> GetTemplateIndicatorTree(BaseParams baseParams)
        {
            var indicatorTree = IndicatorService.GetIndicatorTree(baseParams);

            var id = baseParams.Params.GetAs<long>("id");
            var checkedParams = id == 0
                ? new List<MultipleAnalysisIndicator>()
                : MultipleAnalysisIndicatorRepository.GetAll().Where(x => x.MultipleAnalysisTemplate.Id == id).ToList();

            var res = new List<MultipleAnalysisGroupProxy>();
            indicatorTree.ForEach(x =>
            {
                var group = new MultipleAnalysisGroupProxy
                {
                    Id = string.Format("Id_{0}", x.Service.Code),
                    Name = x.Service.Name
                };
                group.children = new List<MultipleAnalysisProxy>();
                x.Indicators.ForEach(y =>
                {
                    var indicator = new MultipleAnalysisProxy
                    {
                        Id = y.Value,
                        Name = y.Key.GetEnumMeta().Display
                    };
                    var checkedParam = checkedParams.FirstOrDefault(z => z.IndicatorServiceComparison.Id == y.Value);
                    if (checkedParam != null)
                    {
                        indicator.MinValue = checkedParam.MinValue;
                        indicator.MaxValue = checkedParam.MaxValue;
                        indicator.DeviationPercent = checkedParam.DeviationPercent;
                        indicator.ExactValue = checkedParam.ExactValue;
                    }
                    group.children.Add(indicator);
                });
                res.Add(group);
            });

            return res;
        }

        #region Методы для отчета

        public Dictionary<string, string> GetReportColumns(List<MultipleAnalysisProxy> indicators, string separator)
        {
            return indicators.ToDictionary(k => "Ind" + k.Id,
                v => string.Format("{0}{1}{2}{1}{3}",
                    v.IndicatorServiceComparison.Service.Name,
                    separator,
                    v.IndicatorServiceComparison.GisTypeIndicator.GetEnumMeta().Display,
                    (v.MinValue.HasValue ? "от " + v.MinValue.Value.ToString("G") + " " : "") +
                    (v.MaxValue.HasValue ? "до " + v.MaxValue.Value.ToString("G") : "") +
                    (v.DeviationPercent.HasValue
                        ? "отклонение " + v.DeviationPercent.Value.ToString("G") + "%"
                        : "") +
                    (v.ExactValue.HasValue ? "равно " + v.ExactValue.Value.ToString("G") : "")));
        }

        public IList<long> GetHouseIdByType(long typeHouse, string municipalArea, string settlement, string street)
        {
            return RealEstateTypeCommonParamService
                .GetHouseRegistersByRealEstateType(RealEstateTypeRepository.Get(typeHouse))
                .Join(FiasRepository.GetAll(), x => x.FiasAddress.PlaceGuidId, y => y.AOGuid, (x, y) => new { house = x, fias = y })
                .WhereIf(!municipalArea.IsEmpty(),
                    x =>
                        x.house.FiasAddress != null && x.house.FiasAddress.PlaceGuidId == municipalArea 
                        || x.fias != null && x.fias.ParentGuid == municipalArea
                        || x.fias != null && x.fias.MirrorGuid == municipalArea)
                .WhereIf(!settlement.IsEmpty(),
                    x => x.house.FiasAddress != null && x.house.FiasAddress.PlaceGuidId == settlement)
                .WhereIf(!street.IsEmpty(),
                    x => x.house.FiasAddress != null && x.house.FiasAddress.StreetGuidId == street)
                .Select(x => x.house.Id)
                .Distinct()
                .ToList();
        }

        public DataTable GetReportData(List<MultipleAnalysisProxy> indicators, IList<long> housesIdByType, DateTime date, GisTypeCondition typeCondition)
        {
            var houseServices =
                HouseServiceRegisterRepository.GetAll()
                    .Where(
                        x =>
                            housesIdByType.Contains(x.House.Id) &&
                            x.CalculationDate.Month == date.Month
                            && x.CalculationDate.Year == date.Year);

            var data = new DataTable();
            data.Columns.AddRange(new[]
            {
                new DataColumn("Id", typeof (long)),
                new DataColumn("Address", typeof (string))
            });

            var analyzedHouseService = new List<MultipleAnalysisIndicatorProxy>();
            foreach (var indicator in indicators)
            {
                #region Заполнение колонок
                data.Columns.Add(new DataColumn("Ind" + indicator.Id, typeof(double)));
                #endregion

                #region Анализ

                //var avaliableServicesId =
                //    IndicatorService.GetServicesByIndicatorsGroup(indicator.IndicatorServiceComparison.Service)
                //        .Select(x => x.Id)
                //        .ToList();
                //if (!avaliableServicesId.Any()) continue;
                var avaliableHouseServices =
                    houseServices.Where(x => x.Service.Id == indicator.IndicatorServiceComparison.Service.Id).ToList();
                if (!avaliableHouseServices.Any()) continue;
                List<MultipleAnalysisIndicatorProxy> multipleAnalysisIndicatorList;

                if (indicator.DeviationPercent.HasValue)
                {
                    multipleAnalysisIndicatorList =
                        AnalyzeDeviationPercent(avaliableHouseServices,
                            indicator.IndicatorServiceComparison.GisTypeIndicator,
                            indicator.DeviationPercent.Value).ToList();
                }
                else if (indicator.ExactValue.HasValue)
                {
                    multipleAnalysisIndicatorList =
                        AnalyzeExactValue(avaliableHouseServices,
                            indicator.IndicatorServiceComparison.GisTypeIndicator,
                            indicator.ExactValue.Value).ToList();
                }
                else
                {
                    multipleAnalysisIndicatorList =
                        AnalyzeMinMaxValue(avaliableHouseServices,
                            indicator.IndicatorServiceComparison.GisTypeIndicator,
                            indicator.MinValue, indicator.MaxValue).ToList();
                }
                //multipleAnalysisIndicatorList.ForEach(
                //    x => x.Service.Id = indicator.IndicatorServiceComparison.Service.Id);
                analyzedHouseService.AddRange(multipleAnalysisIndicatorList);

                #endregion
            }

            #region Проверка условия и/или

            var houses = analyzedHouseService.Select(x => x.House).Distinct().ToList();

            if (typeCondition == GisTypeCondition.And)
            {
                var houseToRemove = analyzedHouseService
                    .GroupBy(x => x.House)
                    .Select(x => new { House = x.Key, Count = x.Count() })
                    .Where(x => x.Count != indicators.Count)
                    .ToList();
                houses.RemoveAll(x => houseToRemove.Any(y => x.Id == y.House.Id));
            }

            #endregion

            var id = 1;
            foreach (var house in houses)
            {
                var r = data.NewRow();
                r["Id"] = id++;
                r["Address"] = string.Format("{0} {1} {2} {3} {4} {5}", house.Region.Return(x => x),
                    house.Area.Return(x => x),
                    house.City.Return(x => x), house.Street.Return(x => x), house.HouseNum.Return(x => x),
                    house.BuildNum.Return(x => x));
                foreach (var indicator in indicators)
                {
                    var analyzeResult = analyzedHouseService.FirstOrDefault(
                        x =>
                            x.House.Id == house.Id 
                            && x.GisTypeIndicator == indicator.IndicatorServiceComparison.GisTypeIndicator 
                            /*&& x.Service.Id == indicator.IndicatorServiceComparison.Service.Id*/);
                    if (analyzeResult != null)
                    {
                        r["Ind" + indicator.Id] = analyzeResult.Value;
                    }
                }
                data.Rows.Add(r);
            }

            var dv = data.DefaultView;
            dv.Sort = "Address asc";
            return dv.ToTable();
        }

        public Stream GetExcelReport(MultipleAnalysisTemplate template, DateTime date)
        {
            var indicators = MultipleAnalysisIndicatorRepository
                .GetAll()
                .Where(x => x.MultipleAnalysisTemplate.Id == template.Id)
                .Select(x => new MultipleAnalysisProxy
                {
                    Id = x.IndicatorServiceComparison.Id,
                    Name = x.IndicatorServiceComparison.GisTypeIndicator.GetEnumMeta().Display,
                    MinValue = x.MinValue,
                    MaxValue = x.MaxValue,
                    ExactValue = x.ExactValue,
                    DeviationPercent = x.DeviationPercent
                })
                .ToList();

            IndicatorGroupingRepository
                .GetAll()
                .ToList()
                .Where(x => indicators.Any(y => x.Id == y.Id))
                .ToList()
                .ForEach(x => indicators.First(y => x.Id == y.Id).IndicatorServiceComparison = x);

            var houses = GetHouseIdByType(template.RealEstateType.Id, template.MunicipalAreaGuid,
                template.SettlementGuid, template.StreetGuid);
            var columns = GetReportColumns(indicators, Environment.NewLine);
            var data = GetReportData(indicators, houses, date, template.TypeCondition);
            var totalHouseCount = houses.Count;

            var dataSet = new DataSet("Data");

            var columnsTable = new DataTable("Columns");
            columnsTable.Columns.AddRange(new[]
            {
                new DataColumn("Id", typeof (int)),
                new DataColumn("Name", typeof (string))
            });
            var i = 1;
            columns.ForEach(x => columnsTable.Rows.Add(i++, x.Value));
            dataSet.Tables.Add(columnsTable);

            var rowTable = new DataTable("Rows");
            rowTable.Columns.AddRange(new[]
            {
                new DataColumn("Id", typeof (int)),
                new DataColumn("Name", typeof (string))
            });
            i = 1;
            data.AsEnumerable().ForEach(x => rowTable.Rows.Add(i++, x["Address"]));
            dataSet.Tables.Add(rowTable);

            var dataTable = new DataTable("Data");
            dataTable.Columns.AddRange(new[]
            {
                new DataColumn("ColumnId", typeof (int)),
                new DataColumn("RowId", typeof (int)),
                new DataColumn("Value", typeof (double))
            });

            for (var r = 0; r < data.Rows.Count; r++)
                for (var c = 2; c < data.Columns.Count; c++)
                {
                    dataTable.Rows.Add(c - 1, r + 1, data.Rows[r][c]);
                }
            dataSet.Tables.Add(dataTable);

            #region Формирование отчета

            var dataSources = new List<IDataSource>
            {
                new DataSource(new CustomSingleDataProvider<DataSet>("Data", dataSet)),
                new DataSource(new CustomSingleDataProvider<object>("ПараметрыОтчета", new { totalHouseCount })),
            };
            
            var report = new CustomReport(dataSources,
                Array.Empty<IParam>(),
                this.GetType().Name,
                "MultipleAnalysis",
                new MemoryStream(Resources.MultipleAnalysis));

            #endregion

            var remoteReportService = this.Container.Resolve<IRemoteReportService>();
            using (this.Container.Using(remoteReportService))
            {
                return remoteReportService.Generate(report,
                    report.GetTemplate(),
                    new BaseParams(),
                    ReportPrintFormat.xlsx,
                    new Dictionary<string, object>());
            }
        }

        #endregion

        private void Check(IList<HouseServiceRegister> houseService)
        {
            if (houseService == null)
            {
                throw new ArgumentNullException("houseService", @"Аргумент не может принимать значение null!");
            }
        }

        public bool StartBackgroundProcess()
        {
            try
            {
                //Task.Factory.StartNew(CheckTemplates);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool SendEmail(MultipleAnalysisTemplate template, Stream report, DateTime date)
        {
            var email = new[] { template.Email };
            var subject = string.Format("Результат множественного анализа за {0}", date.ToString("MM.yyyy"));
            const string body = "Данное письмо сгенерировано автоматически, отвечать на него не нужно.";
            var fileName = string.Format("{0}.{1}", subject, "xls");
            report.Position = 0;
            var attachment = new Attachment(report, fileName, "text/xls");
            var content = attachment.ContentDisposition;
            content.CreationDate = content.ModificationDate = content.ReadDate = DateTime.Now;
            var message = new MailMessage();
            message.Attachments.Add(attachment);

            try
            {
                MailSender.SendMessage(email, subject, body, message.Attachments);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public IDataResult ListFiasArea(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var list = MunicipalityRepository.GetAll()
                .Where(x => x.ParentMo == null || x.Level == TypeMunicipality.MunicipalArea)
                .Where(x => !x.Name.ToLower().Contains("казань"))
                .ToList()
                .Select(x => new FiasAreaProxy
                {
                    Id = x.Id,
                    FiasId = x.FiasId,
                    Name = x.Name
                })
                .ToList();

            list.Add(new FiasAreaProxy
            {
                Id = 1,
                FiasId = "93b3df57-4c89-44df-ac42-96f05e9cd3b9",
                Name = "г. Казань"
            });

            var data = list
                .AsQueryable()
                .Filter(loadParams, Container)
                .OrderIf(loadParams.Order.Length == 0, true, x => x.Name);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        public class FiasAreaProxy
        {
            public long Id { get; set; }
            public string FiasId { get; set; }
            public string Name { get; set; }
        }
    }
}