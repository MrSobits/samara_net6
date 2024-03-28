namespace Bars.GkhGji.Regions.BaseChelyabinsk.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    public class SMEVComplaintsStep : BaseEntity
    {
        /// <summary>
        /// SMEVComplaints
        /// </summary>
        public virtual SMEVComplaints SMEVComplaints { get; set; }
        /// <summary>
        /// Reason
        /// </summary>
        public virtual string Reason { get; set; }

        /// <summary>
        /// AddDocList
        /// </summary>
        public virtual string AddDocList { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        public virtual FileInfo FileInfo { get; set; }

        /// <summary>
        /// NewDate
        /// </summary>
        public virtual DateTime? NewDate { get; set; }

        /// <summary>
        ///DOPetitionResult
        /// </summary>
        public virtual DOPetitionResult DOPetitionResult { get; set; }

        /// <summary>
        /// DOTypeStep
        /// </summary>
        public virtual DOTypeStep DOTypeStep { get; set; }

        /// <summary>
        /// DOTypeStep
        /// </summary>
        public virtual YesNo YesNo { get; set; }


    }
}
