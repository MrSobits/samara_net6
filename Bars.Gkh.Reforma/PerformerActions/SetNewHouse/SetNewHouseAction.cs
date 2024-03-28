namespace Bars.Gkh.Reforma.PerformerActions.SetNewHouse
{
    using System;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Reforma.Entities;
    using Bars.Gkh.Reforma.Impl.Performer.Action;
    using Bars.Gkh.Reforma.Interface;
    using Bars.Gkh.Reforma.Utils;

    using Castle.Windsor;

    /// <summary>
    ///     Действие создания нового дома в Реформе. Принимает идентификатор жилого дома в Системе,
    ///     возвращает идентификатор дома в Реформе
    /// </summary>
    public class SetNewHouseAction : LoggableSyncActionBase<long, int>
    {
        #region Constants

        /// <summary>
        ///     Идентификатор действия
        /// </summary>
        public const string ActionId = "SetNewHouse";

        #endregion

        #region Public Properties

        public override string Id
        {
            get
            {
                return ActionId;
            }
        }

        #endregion

        #region Methods

        protected override int Execute()
        {
            var service = Container.ResolveDomain<RefRealityObject>();
            var repo = Container.Resolve<IRepository<RealityObject>>();
            try
            {
                var robject = repo.Get(Parameters);
                if (robject == null)
                {
                    throw new Exception(string.Format("Не удалось найти жилой дом с Id = {0}", Parameters));
                }

                Logger.SetActionDetails(string.Format("Адрес = {0}", robject.Address));

                if (service.GetAll().Any(x => x.RealityObject.Id == robject.Id))
                {
                    throw new Exception("Жилой дом уже существует");
                }

                var result = Client.SetNewHouse(robject.FiasAddress.ToReformaFias(Container));

                service.Save(new RefRealityObject { RealityObject = robject, ExternalId = result });

                return result;
            }
            finally
            {
                Container.Release(service);
                Container.Release(repo);
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
        public SetNewHouseAction(IWindsorContainer container, ISyncProvider syncProvider, long parameters)
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
        public SetNewHouseAction(IWindsorContainer container, ISyncProvider syncProvider)
            : base(container, syncProvider)
        {
        }

        #endregion
    }
}