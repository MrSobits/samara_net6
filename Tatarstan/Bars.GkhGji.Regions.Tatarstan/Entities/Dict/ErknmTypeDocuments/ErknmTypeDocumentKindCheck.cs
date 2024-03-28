namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Сущность множественного выбора поля "KindCheck" сущности <see cref="ErknmTypeDocument"/>
    /// </summary>
    public class ErknmTypeDocumentKindCheck : BaseEntity
    {
        /// <summary>
        /// Справочник "Тип документов ЕРКНМ"
        /// </summary>
        public virtual ErknmTypeDocument ErknmTypeDocument { get; set; }
        
        /// <summary>
        /// Используется для видов проверок
        /// </summary>
        public virtual KindCheckGji KindCheck { get; set; }
    }
}