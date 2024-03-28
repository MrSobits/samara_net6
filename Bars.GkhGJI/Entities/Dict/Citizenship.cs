using Bars.B4.DataAccess;

namespace Bars.GkhGji.Entities.Dict
{
    /// <summary>
    /// Справочник гражданств согласно ОКСМ
    /// </summary>
    public class Citizenship : BaseEntity
    {
        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Полное наименование
        /// </summary>
        public virtual string FullName { get; set; }

        /// <summary>
        /// Код ОКСМ (2-букв.)
        /// </summary>
        public virtual string Oksm2 { get; set; }

        /// <summary>
        /// Код ОКСМ (3-букв.)
        /// </summary>
        public virtual string Oksm3 { get; set; }

        /// <summary>
        /// Код ОКСМ (цифр.)
        /// </summary>
        public virtual int OksmCode { get; set; }
    }
}
