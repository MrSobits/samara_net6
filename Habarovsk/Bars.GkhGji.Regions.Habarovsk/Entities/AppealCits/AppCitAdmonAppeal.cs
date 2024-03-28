namespace Bars.GkhGji.Regions.Habarovsk.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Обращениям граждан - Предостережение
    /// </summary>
    public class AppCitAdmonAppeal : BaseGkhEntity
    {
        /// <summary>
        /// Предостережение
        /// </summary>
        public virtual AppealCitsAdmonition AppealCitsAdmonition { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }
      
    }
}