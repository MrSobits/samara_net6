namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using B4.DataAccess;

    public class GjiTatParam : BaseEntity
    {
        /// <summary>
        /// Префикс параметра
        /// </summary>
        public virtual string Prefix { get; set; }

        /// <summary>
        /// Идентификатор параметра (должен быть уникален с учетом префикса)
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// Значение параметра
        /// </summary>
        public virtual string Value { get; set; }
    }
}