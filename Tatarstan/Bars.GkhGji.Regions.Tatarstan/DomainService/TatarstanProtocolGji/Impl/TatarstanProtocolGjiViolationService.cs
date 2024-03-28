namespace Bars.GkhGji.Regions.Tatarstan.DomainService.TatarstanProtocolGji.Impl
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji;

    using Castle.Windsor;

    public class TatarstanProtocolGjiViolationService : ITatarstanProtocolGjiViolationService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult SaveViolations(BaseParams baseParams)
        {
            var protocolId = baseParams.Params.GetAsId("protocolId");
            var violationIds = baseParams.Params.GetAs<long[]>("violationIds");

            if (protocolId == default(long) || violationIds == null || violationIds.Length == 0)
            {
                return new BaseDataResult
                {
                    Success = false,
                    Message = "Некорректные входные данные"
                };
            }

            var serviceViolations = this.Container.Resolve<IDomainService<TatarstanProtocolGjiViolation>>();
            using (this.Container.Using(serviceViolations))
            {
                var violationIdsHash = serviceViolations.GetAll()
                    .Where(x => x.TatarstanProtocolGji.Id == protocolId)
                    .Select(x => x.ViolationGji.Id)
                    .ToHashSet();

                foreach (var id in violationIds)
                {
                    // Если среди существующих статей уже есть такая статья, то пропускаем
                    if (id == 0 || violationIdsHash.Contains(id))
                        continue;

                    var newObj = new TatarstanProtocolGjiViolation
                    {
                        TatarstanProtocolGji = new TatarstanProtocolGji { Id = protocolId },
                        ViolationGji = new ViolationGji { Id = id }
                    };

                    serviceViolations.Save(newObj);
                }

                return new BaseDataResult();
            }
        }
    }
}
