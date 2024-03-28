namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Enums;

    public interface ITatarstanProtocolMvdService
    {
        /// <summary>
        /// Получить протоколы МВД
        /// </summary>
        List<TatarstanProtocolMvdServiceDto> GetList(BaseParams baseParams , bool isExport);
    }

    public class TatarstanProtocolMvdServiceDto
    {
        public long Id { get; set; }
        public State State { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public TypeExecutantProtocolMvd Executant { get; set; }
        public long InspectionId { get; set; }
        public string PhysicalPerson { get; set; }
        public string Official { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public string GisUin { get; set; }

    }
}
