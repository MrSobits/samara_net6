namespace Bars.Gkh.Reforma.PerformerActions.SetHouseUnlinkFromOrganization
{
    using System;
    using Bars.Gkh.Enums;

    public class SetUnlinkFromOrganizationParams
    {
        /// <summary>
        ///     Идентификатор дома
        /// </summary>
        public int ExternalId { get; set; }

        /// <summary>
        ///     Дата окончания управления
        /// </summary>
        public DateTime DateEnd { get; set; }

        /// <summary>
        ///     Тип причины окончания
        /// </summary>
        public ContractStopReasonEnum ReasonType { get; set; }

        /// <summary>
        ///     Причина окончания
        /// </summary>
        public string Reason { get; set; }
    }
}