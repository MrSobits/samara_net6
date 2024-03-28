using System;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Saha.DomainService;

namespace Bars.GkhGji.Regions.Saha.Interceptors
{
    using Bars.B4;
    using Bars.GkhGji.Interceptors;
    using Bars.GkhGji.Regions.Saha.Entities;

    public class ProtocolDefInterceptor : ProtocolDefinitionInterceptor<SahaProtocolDefinition>
    {
        public IDefinitionService DefinitionService { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<SahaProtocolDefinition> service, SahaProtocolDefinition entity)
        {
            var maxNum = DefinitionService.GetMaxDefinitionNum();

            entity.DocumentNumber = maxNum + 1;
            entity.DocumentNum = "02-03-{0}-{1}".FormatUsing(entity.DocumentNumber.ToStr().PadLeft(2, '0'), DateTime.Now.Year % 100);
            return Success();
        }
    }
}
