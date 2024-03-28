namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.Gkh.Domain;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    using Castle.Windsor;

    public class TypeWorkRealityObjectOutdoorHistoryService : ITypeWorkRealityObjectOutdoorHistoryService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult Recover(BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var typeWorkRealityObjectOutdoorHistoryDomain = this.Container.ResolveDomain<TypeWorkRealityObjectOutdoorHistory>();
            var typeWorkRealityObjectOutdoorRepository = this.Container.ResolveRepository<TypeWorkRealityObjectOutdoor>();

            using (this.Container.Using(typeWorkRealityObjectOutdoorHistoryDomain, typeWorkRealityObjectOutdoorRepository))
            {
                var typeWorkHistory = typeWorkRealityObjectOutdoorHistoryDomain.Get(id);
                if (typeWorkHistory == null)
                {
                    return new BaseDataResult(false, $"Не удалось получить запись истории по Id = {id}");
                }

                var typeWork = typeWorkRealityObjectOutdoorRepository.Get(typeWorkHistory.TypeWorkRealityObjectOutdoor.Id);
                if (typeWork.IsActive)
                {
                    return new BaseDataResult(false, "Данный вид работы уже восстановлен и не является удаленным");
                }

                typeWork.IsActive = true;
                typeWorkRealityObjectOutdoorRepository.Update(typeWork);
                // удаляем запись об удалении так как ее восстановили
                typeWorkRealityObjectOutdoorHistoryDomain.Delete(id);

                return new BaseDataResult();
            }
        }
    }
}
