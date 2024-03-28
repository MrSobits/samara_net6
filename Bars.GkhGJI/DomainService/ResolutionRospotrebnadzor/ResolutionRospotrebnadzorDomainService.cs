namespace Bars.GkhGji.DomainService
{
    using Bars.B4;
    using Bars.GkhGji.Entities;

    /// <summary>
    /// Расширение сущности <see cref="ActCheckDomainService"/> в модулях регионов
    /// </summary>
    /// <remarks>
    /// такую пустышку делаю чтобы в регионах заменять , но для этого надо чтобы именно она была зарегистрирована в основном модуле
    /// </remarks>
    public class ResolutionRospotrebnadzorDomainService : ResolutionRospotrebnadzorDomainService<ResolutionRospotrebnadzor>
    {
        // Внимание !! Код override нужно писать не в этом классе а в ActCheckDomainService<T>
    }

    /// <summary>
    /// Расширение сущности <see cref="ActCheck"/> в модулях регионов
    /// </summary>
    /// <typeparam name="T"><see cref="ResolutionRospotrebnadzor"/></typeparam>
    public class ResolutionRospotrebnadzorDomainService<T> : BaseDomainService<T>
        where T : ResolutionRospotrebnadzor
    {
    }
}