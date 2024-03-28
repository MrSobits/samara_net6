using Bars.Gkh.Overhaul.Hmao.Entities;

namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    public interface IFakePublicationService
    {
        /// <summary>
        /// Создать ДПКР для публикации
        /// </summary>
        void CreateDpkrForPublish(ProgramVersion version);
    }
}
