namespace Bars.Gkh.DomainService.TechPassport
{
    using Domain.TechPassport;
    using Enums.BasePassport;

    /// <summary>
    /// Описатель значений компонентов паспорта дома.
    ///  Т.к все значения всех элементов паспорта жилого дома хранятся в xml,
    ///  был задуман описатель который хранит сопоставления компонентов и их коды
    /// </summary>
    public interface ITechPassportElementDescriptorService
    {
        /// <summary>
        /// Получить метаданные компонента по типу компонента
        /// </summary>
        /// <param name="typeComponent">Тип компонента</param>
        /// <returns>Описатель компонента</returns>
        TechPassportComponent GetComponent(TypeTechPassportComponent typeComponent);
    }
}