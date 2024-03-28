namespace Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji
{
    using System;

    using Bars.B4;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;
    using System.Collections.Generic;

    using Bars.B4.Modules.States;
    using Bars.GkhGji.Regions.Tatarstan.Enums;

    public interface ITatarstanProtocolGjiService
    {
        IDataResult GetInfo(BaseParams baseParams);

        List<ResultTatarstanProtocolGji> GetListResult(IDomainService<TatarstanProtocolGji> domainService, BaseParams baseParams);
    }

    public class ResultTatarstanProtocolGji
    {
        public long Id { get; set; }
        public State State { get; set; }
        public string DocumentNumber { get; set; }
        public DateTime? DocumentDate { get; set; }
        public string GisUin { get; set; }
        public decimal? PenaltyAmount { get; set; }
        public TypeDocObject Executant { get; set; }
        public string MunicipalityName { get; set; }
        public string Inspectors { get; set; }
        public string Fio { get; set; }

        //свойство для контроллера js
        public long? InspectionId { get; set; }
    }
}
