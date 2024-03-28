namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Инспекторы протокола ОСС
    /// </summary>
    public class PropertyOwnerProtocolInspector : BaseGkhEntity
    {
        /// <summary>
        /// Документ ГЖИ
        /// </summary>
        public virtual PropertyOwnerProtocols PropertyOwnerProtocols { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// Порядковый номер инспектора для текущего документа
        /// </summary>
        public virtual int Order { get; set; }
    }
}