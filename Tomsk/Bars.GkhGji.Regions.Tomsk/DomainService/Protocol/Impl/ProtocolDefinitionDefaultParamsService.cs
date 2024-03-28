namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System;

    using B4;

    using Castle.Windsor;
    using GkhGji.Entities;
    using System.Linq;

    public class ProtocolDefinitionDefaultParamsService : IProtocolDefinitionDefaultParamsService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetDefaultParams(BaseParams baseParams)
        {
            var protocolService = Container.Resolve<IDomainService<Protocol>>();

            try
            {
                var protocolId = baseParams.Params.GetAs("protocolId", 0L);

                var protocol = protocolService.GetAll().FirstOrDefault(x => x.Id == protocolId);

                DateTime? dateOfProc = null;
                string timeDef = string.Empty;

                if (protocol != null)
                {
                    dateOfProc = protocol.DateOfProceedings;

                    if (protocol.HourOfProceedings > 0 || protocol.MinuteOfProceedings > 0)
                    {
                        var date = dateOfProc.HasValue ? dateOfProc.Value : DateTime.Today;

                        date = new DateTime(date.Year, date.Month, date.Day, protocol.HourOfProceedings, protocol.MinuteOfProceedings, 0);

                        timeDef = date.ToShortTimeString();
                    }

                }

                return new BaseDataResult(new { dateOfProc, timeDef });
            }
            finally 
            {
                Container.Release(protocolService);
            }
        }
    }
}
