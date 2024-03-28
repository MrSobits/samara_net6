namespace Bars.KP60.Protocol.DomainService.Impl
{
    using Castle.Windsor;

    /// <summary>
    /// Реализация дерева протокола расчетов по коммунальным услугам
    /// </summary>
    public class BuildingOthers : Building
    {
        public BuildingOthers(IWindsorContainer container)
        {
            Container = container;
        }

    }
}


