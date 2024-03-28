namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Enums;
    using System;

    /// <summary>
    /// Юристы
    /// </summary>
    public class CourtPracticeInspector : BaseEntity
    {
        /// <summary>
        /// CourtPractice
        /// </summary>
        public virtual CourtPractice CourtPractice { get; set; }

        /// <summary>
        /// Inspector
        /// </summary>
        public virtual Inspector Inspector { get; set; }      

        /// <summary>
        /// Юрист инспектор
        /// </summary>
        public virtual LawyerInspector LawyerInspector { get; set; }     

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Discription { get; set; }

       


    }
}