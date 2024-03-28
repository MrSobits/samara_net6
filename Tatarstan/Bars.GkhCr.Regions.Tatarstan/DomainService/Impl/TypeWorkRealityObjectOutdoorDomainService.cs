namespace Bars.GkhCr.Regions.Tatarstan.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.Gkh.Authentification;
    using Bars.GkhCr.Enums;
    using Bars.GkhCr.Regions.Tatarstan.Entities.Dict.RealityObjectOutdoorProgram;
    using Bars.GkhCr.Regions.Tatarstan.Entities.ObjectOutdoorCr;

    public class TypeWorkRealityObjectOutdoorDomainService : BaseDomainService<TypeWorkRealityObjectOutdoor>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var result = base.Save(baseParams);
            this.UpdateChangeJournal(result, TypeWorkCrHistoryAction.Creation);
            return result;
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var result = base.Update(baseParams);
            this.UpdateChangeJournal(result, TypeWorkCrHistoryAction.Modification);
            return result;
        }

        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToLongArray(baseParams.Params, "records");
            var typeWorkRealityObjectOutdoorDomain = this.Container.ResolveDomain<TypeWorkRealityObjectOutdoor>();
            var interceptors = this.Container.ResolveAll<IDomainServiceInterceptor<TypeWorkRealityObjectOutdoor>>();

            using (this.Container.Using(typeWorkRealityObjectOutdoorDomain, interceptors))
            {
                var typeWorkRealityObjectOutdoorList = typeWorkRealityObjectOutdoorDomain.GetAll()
                    .Where(x => ids.Contains(x.Id))
                    .ToList();

                this.InTransaction(() =>
                {
                    foreach (var typeWorkRealityObjectOutdoor in typeWorkRealityObjectOutdoorList)
                    {
                        typeWorkRealityObjectOutdoor.IsActive = false;

                        IDataResult result = null;

                        this.CallBeforeDeleteInterceptors(typeWorkRealityObjectOutdoor, ref result, interceptors);
                        this.UpdateEntityInternal(typeWorkRealityObjectOutdoor);
                        this.UpdateJournal(typeWorkRealityObjectOutdoor, TypeWorkCrHistoryAction.Removal);
                        this.CallAfterDeleteInterceptors(typeWorkRealityObjectOutdoor, ref result, interceptors);
                    }
                });

                return new BaseDataResult(ids);
            }
        }

        /// <summary>
        /// Обновление журнала программы.
        /// </summary>
        private void UpdateChangeJournal(IDataResult result, TypeWorkCrHistoryAction action)
        {
            // в случае успешного сохранения, добавляем запись в журнал
            if (!result.Success || !(result.Data is IEnumerable<TypeWorkRealityObjectOutdoor> data) || !data.Any())
            {
                return;
            }

            foreach (var obj in data)
            {
                this.UpdateJournal(obj, action);
            }
        }

        private void UpdateJournal(TypeWorkRealityObjectOutdoor entity, TypeWorkCrHistoryAction action)
        {
            var userManager = this.Container.Resolve<IGkhUserManager>();
            var typeWorkRealityObjectOutdoorHistoryDomain = this.Container.ResolveDomain<TypeWorkRealityObjectOutdoorHistory>();
            using (this.Container.Using(userManager, typeWorkRealityObjectOutdoorHistoryDomain))
            {
                typeWorkRealityObjectOutdoorHistoryDomain.Save(new TypeWorkRealityObjectOutdoorHistory
                {
                    TypeWorkRealityObjectOutdoor = entity,
                    Sum = entity.Sum,
                    Volume = entity.Volume,
                    TypeAction = action,
                    UserName = userManager.GetActiveUser()?.Name ?? "Администратор"
                });
            }
        }
    }
}
