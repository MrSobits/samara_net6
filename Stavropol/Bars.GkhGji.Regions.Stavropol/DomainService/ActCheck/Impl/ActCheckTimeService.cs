namespace Bars.GkhGji.Regions.Stavropol.DomainService.ActCheck.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Stavropol.Entities;

    public class ActCheckTimeService : IActCheckTimeService
    {
        public IDomainService<ActCheckTime> ServiceActCheckTime { get; set; } 
        public IDataResult CreateActCheckTime(BaseParams baseParams)
        {
            var actCheckId = baseParams.Params.GetAs<long>("actCheckId");
            var creationTime = baseParams.Params.GetAs<DateTime?>("creationtime");

            if (actCheckId == 0)
            {
                return new BaseDataResult(false, "Необходимо указать идентификатор Акта");
            }

            var existingValue = this.ServiceActCheckTime.GetAll().FirstOrDefault(x => x.ActCheck.Id == actCheckId);

            var hour = 0;
            var minute = 0;

            if (creationTime.HasValue)
            {
                hour = creationTime.Value.Hour;
                minute = creationTime.Value.Minute;
            }

            if (existingValue != null)
            {
                existingValue.Hour = hour;
                existingValue.Minute = minute;
            }
            else
            {
                existingValue = new ActCheckTime
                                    {
                                        ActCheck = new ActCheck { Id = actCheckId },
                                        Hour = hour,
                                        Minute = minute
                                    };
            }

            this.ServiceActCheckTime.Save(existingValue);

            return new BaseDataResult();
        }
    }
}
