namespace Bars.GkhGji.DomainService.FuelInfo.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для работы со сведениями о наличии и расходе топлива
    /// </summary>
    public class FuelInfoService : IFuelInfoService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Домен-сервис для Период сведений о наличии и расходе топлива
        /// </summary>
        public IDomainService<FuelInfoPeriod> FuelInfoPeriodDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Поставка, расход и остатки топлива
        /// </summary>
        public IDomainService<FuelAmountInfo> FuelAmountInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Расстояние от места добычи топлива до потребителя.
        /// </summary>
        public IDomainService<FuelExtractionDistanceInfo> FuelExtractionDistanceInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Выполнение договорных обязательств по поставкам топлива
        /// </summary>
        public IDomainService<FuelContractObligationInfo> FuelContractObligationInfoDomain { get; set; }

        /// <summary>
        /// Домен-сервис для Задолженность за ранее потребленные топливно-энергетические ресурсы (ТЭР)
        /// по состоянию на конец отчетного периода
        /// </summary>
        public IDomainService<FuelEnergyDebtInfo> FuelEnergyDebtInfoDomain { get; set; }

        /// <summary>
        /// Сервис "Муниципальный район и Муниципальный образование"
        /// </summary>
        public IMunicipalityService MunicipalityService { get; set; }

        /// <inheritdoc />
        public IDataResult MassCreate(BaseParams baseParams)
        {
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<byte>("month");
            var municipalityIds = baseParams.Params.GetAs<List<long>>("municipality");

            var municipalities = this.MunicipalityService.ListByParamAndOperatorQueryable();

            var periods = municipalities
                .WhereIf(municipalityIds != null && municipalityIds.Count > 0, x => municipalityIds.Contains(x.Id))
                .Select(
                    x => new FuelInfoPeriod
                    {
                        Municipality = new Municipality {Id = x.Id},
                        Year = year,
                        Month = month
                    })
                .ToList();

            using (var transaction = this.Container.Resolve<IDataTransaction>())
            {
                foreach (var fuelInfoPeriod in periods)
                {
                    this.FuelInfoPeriodDomain.Save(fuelInfoPeriod);
                    this.CreatePeriodInfo(fuelInfoPeriod);
                }

                transaction.Commit();
            }

            return new BaseDataResult();
        }

        private void CreatePeriodInfo(FuelInfoPeriod period)
        {
            var fuelAmountInfos = new[]
            {
                new FuelAmountInfo {RowNumber = 1, Mark = "Фактически поставлено с начала отопительного периода"},
                new FuelAmountInfo {RowNumber = 2, Mark = "Фактически израсходовано с начала отопительного периода"},
                new FuelAmountInfo {RowNumber = 3, Mark = "Объемы запасов топлива на конец отчетного периода"}
            };

            var fuelExtractionDistanceInfos = new[]
            {
                new FuelExtractionDistanceInfo {RowNumber = 4, Mark = "Уголь"},
                new FuelExtractionDistanceInfo {RowNumber = 5, Mark = "Дрова"},
                new FuelExtractionDistanceInfo {RowNumber = 6, Mark = "Мазут (темные нефтепродукты)"},
                new FuelExtractionDistanceInfo {RowNumber = 7, Mark = "Газ"},
                new FuelExtractionDistanceInfo {RowNumber = 8, Mark = "Другие виды топлива"}
            };

            var fuelContractObligationInfos = new[]
            {
                new FuelContractObligationInfo {RowNumber = 9, Mark = "Объем договорных обязательств по поставкам топлива"},
                new FuelContractObligationInfo {RowNumber = 10, Mark = "Объем поставок местного топлива"},
                new FuelContractObligationInfo {RowNumber = 11, Mark = "Среднесложившаяся по договорам стоимость топлива, руб/т"}
            };

            var fuelEnergyDebtInfos = new[]
            {
                new FuelEnergyDebtInfo {RowNumber = 12, Mark = "Задолженность за ранее потребленные ТЭР, всего:"},
                new FuelEnergyDebtInfo {RowNumber = 13, Mark = "в том числе уголь"},
                new FuelEnergyDebtInfo {RowNumber = 14, Mark = "в том числе жидкое топливо"},
                new FuelEnergyDebtInfo {RowNumber = 15, Mark = "в том числе  газ"},
                new FuelEnergyDebtInfo {RowNumber = 16, Mark = "в том числе теплоэнергию"},
                new FuelEnergyDebtInfo {RowNumber = 17, Mark = "в том числе электроэнергию"}
            };

            foreach (var fuelAmountInfo in fuelAmountInfos)
            {
                fuelAmountInfo.FuelInfoPeriod = period;
                this.FuelAmountInfoDomain.Save(fuelAmountInfo);
            }

            foreach (var fuelExtractionDistanceInfo in fuelExtractionDistanceInfos)
            {
                fuelExtractionDistanceInfo.FuelInfoPeriod = period;
                this.FuelExtractionDistanceInfoDomain.Save(fuelExtractionDistanceInfo);
            }

            foreach (var fuelContractObligationInfo in fuelContractObligationInfos)
            {
                fuelContractObligationInfo.FuelInfoPeriod = period;
                this.FuelContractObligationInfoDomain.Save(fuelContractObligationInfo);
            }

            foreach (var fuelEnergyDebtInfo in fuelEnergyDebtInfos)
            {
                fuelEnergyDebtInfo.FuelInfoPeriod = period;
                this.FuelEnergyDebtInfoDomain.Save(fuelEnergyDebtInfo);
            }
        }
    }
}