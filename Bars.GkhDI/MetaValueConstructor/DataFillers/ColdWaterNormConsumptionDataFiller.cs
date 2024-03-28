namespace Bars.GkhDi.MetaValueConstructor.DataFillers
{
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Нормативы потребления ХВС
    /// </summary>
    public class ColdWaterNormConsumptionDataFiller : AbstractNormConsumptionDataFiller
    {
        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = this.normConsumptionDict.Get(value.EfManagingOrganization.ManagingOrganization.Id)?.ColdWater ?? 0;
        }
    }
}