namespace Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    using Castle.Windsor;

    public class TatarstanProtocolGjiRealityObjectService : ITatarstanProtocolGjiRealityObjectService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult SaveRealityObjects(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("protocolId");
            var roIds = baseParams.Params.GetAs<long[]>("roIds");

            if (protocolId == default(long) || roIds == null || roIds.Length == 0)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Некорректные входные данные"
                };
            }

            var serviceRealityObjects = this.Container.Resolve<IDomainService<TatarstanProtocolGjiRealityObject>>();
            using (this.Container.Using(serviceRealityObjects))
            {
                var realityObjectIdsHash = serviceRealityObjects.GetAll()
                    .Where(x => x.TatarstanProtocolGji.Id == protocolId)
                    .Select(x => x.RealityObject.Id)
                    .ToHashSet();

                foreach (var id in roIds)
                {
                    // Если среди существующих статей уже есть такая статья, то пропускаем
                    if (id == 0 || realityObjectIdsHash.Contains(id))
                    {
                        continue;
                    }

                    var newObj = new TatarstanProtocolGjiRealityObject
                    {
                        TatarstanProtocolGji = new TatarstanProtocolGji { Id = protocolId },
                        RealityObject = new RealityObject { Id = id }
                    };

                    serviceRealityObjects.Save(newObj);
                }

                return new BaseDataResult();
            }
        }
    }
}
