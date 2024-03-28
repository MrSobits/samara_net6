namespace Bars.Gkh.MetaValueConstructor.DataFillers
{
    using Bars.Gkh.Entities.EfficiencyRating;

    /// <summary>
    /// Общая численность сотрудников
    /// </summary>
    public class NumberEmployeesDataFiller : BaseManagingOrganizationDataFiller
    {
        /// <summary>
        /// Метод заполняет значение объекта УО поля/атрибута из внешнего источника (системы)
        /// </summary>
        /// <param name="value">Объект</param>
        protected override void SetValue(ManagingOrganizationDataValue value)
        {
            value.Value = value.EfManagingOrganization.ManagingOrganization.NumberEmployees ?? 0;
        }
    }
}