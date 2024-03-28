namespace Bars.Gkh.Regions.Tyumen.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерцептор для сущности Жилой дом
    /// </summary>
    public class RealityObjectInterceptor : EmptyDomainInterceptor<RealityObject>
    {
        /// <summary>
        /// Действие, выполняемое до обновления сущности
        /// </summary>
        /// <param name="service">Домен-сервис "Жилой дом"</param>
        /// <param name="entity">Сущность</param>
        /// <returns>Результат операции</returns>
        public override IDataResult BeforeUpdateAction(IDomainService<RealityObject> service, RealityObject entity)
        {
            entity.GkhCode = entity.Id.ToString();

            return Success();
        }
    }
}