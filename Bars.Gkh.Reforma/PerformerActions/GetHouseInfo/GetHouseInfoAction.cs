namespace Bars.Gkh.Reforma.PerformerActions.GetHouseInfo
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Reforma.Utils;

    using Castle.Windsor;

    /// <summary>
    ///     Действие получения информации о жилом доме в Реформе по его идентификатору в ЖКХ
    /// </summary>
    public class GetHouseInfoAction : LoggableSyncActionBase<long, HouseInfo[]>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "GetHouseInfo";

        #endregion

        #region Public Properties

        public override string Id
        {
            get
            {
                return GetHouseInfoAction.ActionId;
            }
        }

        #endregion

        #region Methods

        protected override HouseInfo[] Execute()
        {
            var repo = this.Container.Resolve<IRepository<RealityObject>>();
            var service = this.Container.ResolveDomain<RefRealityObject>();
            var moService = this.Container.Resolve<IManOrgService>();
            try
            {
                var robject = repo.Get(this.Parameters);
                if (robject == null)
                {
                    throw new Exception($"Не найден жилой дом с Id = {this.Parameters}");
                }

                this.Logger.SetActionDetails($"Адрес = {robject.Address}");

                var results = this.Client.GetHouseInfo(robject.FiasAddress.ToReformaFias(this.Container));
                if (results.Length == 0)
                {
                    return null;
                }

                foreach (var result in results)
                {
                    var entity = service.GetAll().FirstOrDefault(x => x.ExternalId == result.house_id);
                    if (entity == null)
                    {
                        var orgId = !result.inn.IsEmpty() ? moService.GetRefManOrgIdByInn(result.inn) : 0;
                        service.Save(
                            new RefRealityObject { ExternalId = result.house_id, RealityObject = robject, RefManagingOrganization = orgId > 0 ? new RefManagingOrganization { Id = orgId } : null });
                    }
                    else if (entity.RealityObject.Id != robject.Id)
                    {
                        throw new Exception(
                            $"Внешнему идентификатору дома {result.house_id} уже соответствует другой дом в системе: {entity.RealityObject.FiasAddress.AddressName} (Id = {entity.RealityObject.Id})");
                    }
                }

                // если по адресу нам пришло меньше домов, чем у нас заведено с реформой, то грохаем связи,
                // чтобы потом лишний раз по ним не интегрироваться
                foreach (var refRobject in service.GetAll().Where(x => x.RealityObject.Id == this.Parameters))
                {
                    if (results.All(x => x.house_id != refRobject.ExternalId))
                    {
                        service.Delete(refRobject.Id);
                    }
                }

                return results;
            }
            finally
            {
                this.Container.Release(repo);
                this.Container.Release(service);
                this.Container.Release(moService);
            }
        }

        #endregion

        #region Constructors and Destructors

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        /// <param name="parameters">
        ///     Параметры действия
        /// </param>
        public GetHouseInfoAction(IWindsorContainer container, ISyncProvider syncProvider, long parameters)
            : base(container, syncProvider, parameters)
        {
        }

        /// <summary>
        ///     Конструктор
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="syncProvider">
        ///     Провайдер синхронизации
        /// </param>
        public GetHouseInfoAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}