namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Санкция ГЖИ
    /// </summary>
    public class SanctionGji : BaseGkhEntity, IEntityUsedInErknm
    {
        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public virtual string Name { get; set; }
        
        /// <summary>
        /// Идентификатор в ЕРКНМ
        /// </summary>
        public virtual string ErknmGuid { get; set; }
    }
}