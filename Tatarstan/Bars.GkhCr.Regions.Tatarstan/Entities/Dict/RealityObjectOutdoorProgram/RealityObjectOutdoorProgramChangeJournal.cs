namespace Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram
{
    using System;

    using Bars.B4.DataAccess;

    /// <summary>
    /// Журнал изменений программы благоустройства двора.
    /// </summary>
    public class RealityObjectOutdoorProgramChangeJournal : BaseEntity
    {
        /// <summary>
        /// Программа.
        /// </summary>
        public virtual RealityObjectOutdoorProgram RealityObjectOutdoorProgram { get; set; }
        
        /// <summary>
        /// Дата.
        /// </summary>
        public virtual DateTime ChangeDate { get; set; }

        /// <summary>
        /// Количество МО.
        /// </summary>
        public virtual int? MuCount { get; set; }

        /// <summary>
        /// Способ формирования.
        /// </summary>
        public virtual string UserName { get; set; }

        /// <summary>
        /// Примечание.
        /// </summary>
        public virtual string Description { get; set; }
    }
}
