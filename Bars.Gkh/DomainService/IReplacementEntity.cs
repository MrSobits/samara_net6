namespace Bars.Gkh.DomainService
{
    using Bars.B4.DataAccess;

    /// <summary>
    /// Интерфейс приведения дочерней сущности к родительской 
    /// </summary>
    public interface IReplacementEntity<Tbase, Tderived>
        where Tbase : class, IEntity
        where Tderived : class, Tbase, new ()
    {
        Tderived ToDerived(Tbase tBase);
    }
}
