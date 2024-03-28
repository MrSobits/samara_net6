namespace Bars.GkhGji.Regions.BaseChelyabinsk
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

    using NHibernate.Util;

    /// <summary>
    /// Зависимости модуля
    /// </summary>
    public class ModuleDependencies : BaseModuleDependencies
    {
        /// <summary>
        /// Конструктор класса <see cref="T:Bars.B4.DataAccess.BaseModuleDependencies"/>.
        /// </summary>
        /// <param name="container">Контейнер</param>
        public ModuleDependencies(IWindsorContainer container)
            : base(container)
        {
        }

        /// <summary>
        /// Инициализировать
        /// </summary>
        public override IModuleDependencies Init()
        {
            this.References.Add(
                new EntityReference
                {
                    ReferenceName = "Требования НПА проверки",
                    BaseEntity = typeof(DisposalInspFoundationCheck),
                    DeleteAnyDependences = id =>
                        this.Container.UsingForResolved<IDomainService<DisposalInspFoundCheckNormDocItem>>(
                            (container, domainService) =>
                            {
                                domainService.GetAll()
                                    .Where(x => x.DisposalInspFoundationCheck.Id == id)
                                    .ForEach(x => domainService.Delete(x.Id));
                            })
                });

            return this;
        }
    }
}