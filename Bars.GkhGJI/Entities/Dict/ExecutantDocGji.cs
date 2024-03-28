namespace Bars.GkhGji.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Исполнитель документа ГЖИ
    /// </summary>
    public class ExecutantDocGji : BaseGkhEntity
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
        /// Код в ЕРКНМ
        /// </summary>
        public virtual string ErknmCode { get; set; }
    }
}