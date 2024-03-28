using Bars.Gkh.Overhaul.Entities;
using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version
{
    public interface IAddVersionRecord
    {
        /// <summary>
        /// Добавить запись в версию
        /// </summary>
        void Add(ProgramVersion version, RealityObjectStructuralElement rostructel, decimal sum, decimal volume, short year);
    }
}
