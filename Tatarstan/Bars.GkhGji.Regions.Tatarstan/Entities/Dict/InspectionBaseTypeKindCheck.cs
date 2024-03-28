namespace Bars.GkhGji.Regions.Tatarstan.Entities.Dict
{
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;

    /// <summary>
    /// Сущность-связка для <see cref="Bars.GkhGji.Entities.Dict.InspectionBaseType"/> и <see cref="Bars.GkhGji.Enums.TypeDocumentGji"/>
    /// </summary>
    public class InspectionBaseTypeKindCheck : PersistentObject
    {
        /// <summary>
        /// Справочник. Основание проверки
        /// </summary>
        public virtual InspectionBaseType InspectionBaseType { get; set; }
        
        /// <summary>
        /// Вид проверки
        /// </summary>
        public virtual KindCheckGji KindCheck { get; set; }
    }
}