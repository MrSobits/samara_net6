namespace Bars.Gkh.Decisions.Nso.Domain
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Интерфейс, содержащий общие свойства 2-х протоколов
    /// </summary>
    public interface IDecisionProtocol
    {
        /// <summary>
        /// Id
        /// </summary>
        long Id { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        RealityObject RealityObject { get; set; }
    }
}