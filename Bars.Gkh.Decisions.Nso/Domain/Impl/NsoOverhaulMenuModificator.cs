namespace Bars.Gkh.Decisions.Nso.Domain.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Navigation;

    public class NsoOverhaulMenuModificator : IMenuModificator
    {
        public string Key
        {
            get { return RealityObjMenuKey.Key; }
        }

        public IEnumerable<MenuItem> Modify(IEnumerable<MenuItem> menuItems)
        {
            var items = menuItems.ToList();
            var currentOverhaul = items.FindIndex(x => x.Caption.ToLower() == "текущий ремонт");
            if (currentOverhaul < 0)
            {
                return items;
            }

            var activeDecision = items.FindIndex(x => x.Caption.ToLower() == "действующие решения");
            if (0 <= activeDecision)
            {
                var item = items[activeDecision];
                items.RemoveAt(activeDecision);

                // если удаляем элемент до нового положения - то новое положение сдвигается
                if (activeDecision < currentOverhaul)
                {
                    currentOverhaul--;
                }

                items.Insert(currentOverhaul + 1, item);
            }

            var govDecision = items.FindIndex(x => x.Caption.ToLower() == "протокол решений органа гос. власти");
            if (0 <= govDecision)
            {
                var item = items[govDecision];
                items.RemoveAt(govDecision);

                // если удаляем элемент до нового положения - то новое положение сдвигается
                if (govDecision < currentOverhaul)
                {
                    currentOverhaul--;
                }

                items.Insert(currentOverhaul + 1, item);
            }

            var protocolDecision = items.FindIndex(x => x.Caption.ToLower() == "протоколы решений");
            if (0 <= protocolDecision)
            {
                var item = items[protocolDecision];
                items.RemoveAt(protocolDecision);
                // если удаляем элемент до нового положения - то новое положение сдвигается
                if (govDecision < currentOverhaul)
                {
                    currentOverhaul--;
                }

                items.Insert(currentOverhaul + 1, item);
            }

            return items;
        }
    }
}