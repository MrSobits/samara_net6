namespace Bars.GkhGji.Entities.Dict
{
    using B4.DataAccess;

    /// <summary>
    /// Направление деятельности субъекта проверки
    /// </summary>
    public class ActivityDirection : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }
    }
}