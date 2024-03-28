namespace Bars.GkhDi.MetaValueConstructor
{
    using Bars.Gkh.MetaValueConstructor.DataFillers;
    using Bars.Gkh.MetaValueConstructor.Enums;
    using Bars.GkhDi.MetaValueConstructor.DataFillers;

    /// <summary>
    /// Маппинг источников данных Раскрытия информации
    /// </summary>
    public class DiConstructorDataFillerMap : ConstructorDataFillerMap
    {
        /// <summary>
        /// Тип конструктора
        /// </summary>
        public override DataMetaObjectType DataMetaObjectType => DataMetaObjectType.EfficientcyRating;

        /// <summary>
        /// Зарегистрировать все сущности
        /// </summary>
        public override void Map()
        {
            this.Namespace("GkhDi", "Раскрытие информации");
            this.Namespace("GkhDi.ManagingOrgs", "Сведения об УО");
            this.Namespace("GkhDi.ManagingOrgs.AdminResp", "Административная ответственность");
            this.CollectorImpl<FinesAmountForPeriodDataFiller>("GkhDi.ManagingOrgs.AdminResp.FinesAmountForPeriod", "Сумма штрафов за отчетный период");

            this.Namespace("GkhDi.ManagingOrgs.GeneralInfo", "Общие сведения");
            this.Namespace("GkhDi.ManagingOrgs.GeneralInfo.Rating", "Для рейтинга управляющих организаций");
            this.CollectorImpl<DismissedDuringPeriodDataFiller>("GkhDi.ManagingOrgs.GeneralInfo.Rating.DismissedDuringPeriod", "Уволено за отчетный период");

            this.Namespace("GkhDi.ManagingOrgs.FinActivity", "Финансовая деятельность");
            this.Namespace("GkhDi.ManagingOrgs.FinActivity.HouseManaging", "Управление по домам");
            this.CollectorImpl<AreaManagedPropertyDataFiller>("GkhDi.ManagingOrgs.FinActivity.HouseManaging.AreaManagedProperty", "Общая площадь МКД - Итого");
            this.CollectorImpl<TotalCostsCompanyDataFiller>("GkhDi.ManagingOrgs.FinActivity.HouseManaging.TotalCostsCompany", "Сумма фактических расходов");
            this.CollectorImpl<TotalIncomeCompanyDataFiller>("GkhDi.ManagingOrgs.FinActivity.HouseManaging.TotalIncomeCompany", "Сумма дохода от управления");
            this.CollectorImpl<FactIncomeCompanyDataFiller>("GkhDi.ManagingOrgs.FinActivity.HouseManaging.FactIncomeCompany", "Получено за предоставленные услуги");
            this.CollectorImpl<TotalPresentedToRepayCompanyDataFiller>("GkhDi.ManagingOrgs.FinActivity.HouseManaging.TotalPresentedToRepayCompany", "Предъявлено к оплате");

            this.Namespace("GkhDi.RealityObject", "Сведения о Доме");
            this.Namespace("GkhDi.RealityObject.Services", "Сведения об услугах");
            this.Namespace("GkhDi.RealityObject.Services.ColdWater", "Холодное водоснабжение");
            this.Namespace("GkhDi.RealityObject.Services.HotWater", "Горячее водоснабжение");
            this.Namespace("GkhDi.RealityObject.Services.Heating", "Отопление");
            this.Namespace("GkhDi.RealityObject.Services.Electricity", "Электроснабжение");

            this.CollectorImpl<ColdWaterNormConsumptionDataFiller>(
                "GkhDi.RealityObject.Services.ColdWater.NormConsumption", "Норматив потребления коммунальной услуги в жилых помещениях");
            this.CollectorImpl<HotWaterNormConsumptionDataFiller>(
                "GkhDi.RealityObject.Services.HotWater.NormConsumption", "Норматив потребления коммунальной услуги в жилых помещениях");
            this.CollectorImpl<HeatingNormConsumptionDataFiller>(
                "GkhDi.RealityObject.Services.Heating.NormConsumption", "Норматив потребления коммунальной услуги в жилых помещениях");
            this.CollectorImpl<ElectricityNormConsumptionDataFiller>(
                "GkhDi.RealityObject.Services.Electricity.NormConsumption", "Норматив потребления коммунальной услуги в жилых помещениях");
        }
    }
}