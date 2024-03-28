namespace Bars.GkhCr.DomainService.GisGkhRegional
{
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Enums;

    /// <summary>
    /// Сервис для получения региональных ранных при работе с ГИС ЖКХ
    /// </summary>
    public interface IGisGkhRegionalService
    {
        /// <summary>
        /// Получение типа финансирования работ
        /// </summary>
        /// <param name="work">Работа</param>
        GisGkhWorkFinancingType GetWorkFinancingType(TypeWorkCr work);
    }
}