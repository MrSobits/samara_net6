namespace Bars.GkhDi.Interceptors
{
    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.GkhDi.Entities;

    /// <summary>
    /// Интерцептор для сущности "Документы сведений об УО объекта недвижимости"
    /// </summary>
    public class DocumentsRealityObjInterceptor : EmptyDomainInterceptor<DocumentsRealityObj>
    {
        /// <summary>
        /// Метод вызывается после создания объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeCreateAction(IDomainService<DocumentsRealityObj> service, DocumentsRealityObj entity)
        {
            if (entity.HasGeneralMeetingOfOwners == 0)
            {
                entity.HasGeneralMeetingOfOwners = YesNoNotSet.NotSet;
            }

            return base.BeforeCreateAction(service, entity);
        }

        /// <summary>
        /// Метод вызывается перед обновлением объекта
        /// </summary>
        /// <param name="service">Домен</param><param name="entity">Объект</param>
        /// <returns>
        /// Результат выполнения
        /// </returns>
        public override IDataResult BeforeUpdateAction(IDomainService<DocumentsRealityObj> service, DocumentsRealityObj entity)
        {
            if (entity.HasGeneralMeetingOfOwners == 0)
            {
                entity.HasGeneralMeetingOfOwners = YesNoNotSet.NotSet;
            }
            return base.BeforeUpdateAction(service, entity);
        }
    }
}
