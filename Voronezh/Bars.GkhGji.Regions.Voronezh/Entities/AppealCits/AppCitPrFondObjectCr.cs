namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Обращениям граждан - Предписание Виды Работ
    /// </summary>
    public class AppCitPrFondObjectCr : BaseGkhEntity
    {
        /// <summary>
        /// Предостережение
        /// </summary>
        public virtual AppealCitsPrescriptionFond AppealCitsPrescriptionFond { get; set; }

        /// <summary>
        /// Виды работ
        /// </summary>
        public virtual ObjectCr ObjectCr { get; set; }
    }
}