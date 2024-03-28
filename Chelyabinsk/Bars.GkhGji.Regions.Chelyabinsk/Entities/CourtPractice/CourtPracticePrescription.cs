
namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.GkhGji.Entities;
    using Enums;
    using System;

    public class CourtPracticePrescription : BaseEntity
    {
        /// <summary>
        /// CourtPractice
        /// </summary>
        public virtual CourtPractice CourtPractice { get; set; }

        /// <summary>
        ///Предписание
        /// </summary>
        public virtual  DocumentGji DocumentGji { get; set; }       
    }
}
