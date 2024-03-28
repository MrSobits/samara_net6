namespace Bars.Gkh.DomainService
{
    using Bars.B4;

    using Castle.Windsor;

    /// <summary>
    /// Заполняет поля в карточке дома
    /// </summary>
    public interface IRealityObjectFieldsService
    {
        /// <summary>
        /// IoC
        /// </summary>
        IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получает значение поля для заполнения
        /// </summary>
        /// <param name="baseParams">Id - id дома, FieldName - поле для заполнения</param>
        IDataResult GetFieldValue(BaseParams baseParams);
    }
}