namespace Bars.Gkh1468.DataFiller.Service.Impl
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.Entities;
    using Castle.MicroKernel;

    using Castle.Windsor;

    public class DataFillerService : IDataFillerService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult Test(BaseParams baseParams)
        {
            var code = baseParams.Params.Get("code", string.Empty);
            var attributeId = baseParams.Params.Get("atrId", (long)0);
            var realityObjId = baseParams.Params.Get("roId", (long)0);

            var metaAttribute = Container.Resolve<IDomainService<MetaAttribute>>().Get(attributeId);
            var realityObject = Container.Resolve<IDomainService<RealityObject>>().Get(realityObjId);

            var result = new List<BaseProviderPassportRow>();

            var service = Container.Resolve<IDataFiller>(
                code,
                new Arguments {{ "RealityObject", realityObject },  { "MetaAttribute", metaAttribute }, { "Result", result } });


            service.To1468();

            foreach (var baseProviderPassportRow in result)
            {
                new OkiProviderPassportRow
                {
                    MetaAttribute = baseProviderPassportRow.MetaAttribute,
                    Value = baseProviderPassportRow.Value,
                    ProviderPassport = new OkiProviderPassport()
                };
            }



            return new ListDataResult(result, result.Count);
        }
    }
}