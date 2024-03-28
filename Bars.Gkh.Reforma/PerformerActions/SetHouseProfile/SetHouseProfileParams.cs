namespace Bars.Gkh.Reforma.PerformerActions.SetHouseProfile
{
    public class SetHouseProfileParams
    {
        /// <summary>
        /// Внешний идентификатор дома
        /// </summary>
        public long RobjectId { get; set; }

        /// <summary>
        /// Идентификатор периода раскрытия
        /// </summary>
        public long PeriodDiId { get; set; }

        /// <summary>
        /// Идентификатор УК
        /// </summary>
        public long ManOrgId { get; set; }
    }
}