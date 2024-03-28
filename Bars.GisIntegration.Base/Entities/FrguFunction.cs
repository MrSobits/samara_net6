namespace Bars.GisIntegration.Base.Entities
{
    using System.ComponentModel;

    using Bars.B4.DataAccess;

    [DisplayName("Функции из ФРГУ")]
    public class FrguFunction : BaseEntity
    {
        /// <summary>
        /// Наименование контрольно-надзорной функции из ФРГУ
        /// </summary>
        [DisplayName("Наименование контрольно-надзорной функции из ФРГУ")]
        public virtual string Name { get; set; }

        /// <summary>
        /// Идентификатор контрольно-надзорной функции из ФРГУ
        /// </summary>
        [DisplayName("Идентификатор контрольно-надзорной функции из ФРГУ")]
        public virtual string FrguId { get; set; }

        /// <summary>
        /// Идентификатор контрольно-надзорной функции формата GUID
        /// </summary>
        [DisplayName("Идентификатор контрольно-надзорной функции формата GUID")]
        public virtual string Guid { get; set; }
    }
}