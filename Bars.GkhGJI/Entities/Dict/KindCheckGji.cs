namespace Bars.GkhGji.Entities
{
    using B4.DataAccess;
    using Enums;

    /// <summary>
    /// Вид проверки
    /// </summary>
    public class KindCheckGji : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Тип проверки, в этом справочнике он выполняет роль кода
        /// </summary>
        public virtual TypeCheck Code { get; set; }
    }
}