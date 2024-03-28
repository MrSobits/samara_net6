namespace Bars.Gkh.Reforma.PerformerActions.GetHouseList
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Domain;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.ReformaService;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    ///     Действие получения списка домов по ИНН УО
    /// </summary>
    public class GetHouseListAction : LoggableSyncActionBase<GetHouseListParams, HouseData[]>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "GetHouseList";

        #endregion

        #region Public Properties

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public override string Id
        {
            get
            {
                return ActionId;
            }
        }

        #endregion

        #region Methods

        protected override HouseData[] Execute()
        {
            var domain = Container.ResolveDomain<RefRealityObject>();
            var service = Container.Resolve<IRobjectService>();
            try
            {
                Logger.SetActionDetails(string.Format("ИНН = {0}", Parameters.Inn));

                var result = Client.GetHouseList(Parameters.Inn);
                if (!string.IsNullOrEmpty(Parameters.RegionGuid))
                {
                    result = result.Where(x => x.full_address.region_guid == Parameters.RegionGuid).ToArray();
                }

                return result;
            }
            finally
            {
                Container.Release(domain);
                Container.Release(service);
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
        public GetHouseListAction(IWindsorContainer container, ISyncProvider syncProvider, GetHouseListParams parameters)
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
        public GetHouseListAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}