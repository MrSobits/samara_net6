namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using Bars.B4;

    /// <summary>
    /// Сервис работы с ControlListTypicalQuestion
    /// </summary>
    public interface IControlListTypicalQuestionService
    {
        /// <summary>
        /// Обновляет ссылку на MandatoryReq
        /// </summary>
        IDataResult UpdateControlListTypicalQuestion(BaseParams baseParams);
    }
}
