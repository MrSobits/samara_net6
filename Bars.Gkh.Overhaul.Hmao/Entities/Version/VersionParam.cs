namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities;

    public class VersionParam : BaseImportableEntity
    {
        public virtual ProgramVersion ProgramVersion { get; set; }

        /// <summary>
        /// Код
        /// </summary>
        public virtual string Code { get; set; }

        /// <summary>
        /// Вес
        /// </summary>
        public virtual int Weight { get; set; }

        /// <summary>
        /// Муниципальное образование
        /// </summary>
        public virtual Municipality Municipality { get; set; }
    }
}