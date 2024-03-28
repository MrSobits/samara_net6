using System.Collections.Generic;
using Bars.B4.DataAccess;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Regions.Zabaykalye.DomainService;
using Bars.GkhGji.Regions.Zabaykalye.Entities;

namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Linq;
    using Castle.Windsor;
    using Entities;

    public class DefinitionService : IDefinitionService
    {
        public IWindsorContainer Container { get; set; }


        public int GetMaxDefinitionNum(int year)
        {
            var resolProsDefDomain = Container.ResolveDomain<ResolProsDefinition>();
            var actCheckDefDomain = Container.ResolveDomain<ActCheckDefinition>();
            var protocolDefDomain = Container.ResolveDomain<ProtocolDefinition>();
            var protocolMhcDefDomain = Container.ResolveDomain<ProtocolMhcDefinition>();
            var resolutionDefDomain = Container.ResolveDomain<ResolutionDefinition>();

            try
            {
                var listNums = new List<int>();

                listNums.Add(resolProsDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(actCheckDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(protocolDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(protocolMhcDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);
                listNums.Add(resolutionDefDomain.GetAll().Where(x => x.DocumentDate.HasValue && x.DocumentDate.Value.Year == year).SafeMax(x => x.DocumentNumber) ?? 0);

                return listNums.SafeMax(x => x);
            }
            finally
            {
                Container.Release(resolProsDefDomain);
                Container.Release(actCheckDefDomain);
                Container.Release(protocolDefDomain);
                Container.Release(protocolMhcDefDomain);
                Container.Release(resolutionDefDomain);
            }
        }
    }
}