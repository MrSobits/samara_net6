namespace Bars.Gkh1468.DomainService.Passport
{
    using Bars.B4;
    using Bars.Gkh1468.Entities;

    /// <summary>
    /// Интерфейс для взаимодействия с метаатрибутами
    /// </summary>
    public interface IMetaAttributeService
    {
        /// <summary>
        /// Удаляет метаатрибут
        /// </summary>
        /// <param name="atrId">Идентифкатор атрибута</param>
        /// <param name="domainService">Сервис домена метаатрибутов</param>
        /// <returns>Результат операции</returns>
        IDataResult RemoveMetaAttribute(long atrId, IDomainService<MetaAttribute> domainService);
    }
}
