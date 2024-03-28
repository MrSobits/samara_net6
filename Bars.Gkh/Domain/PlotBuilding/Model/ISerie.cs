namespace Bars.Gkh.Domain.PlotBuilding.Model
{
    /// <summary>
    /// График
    /// </summary>
    public interface ISerie
    {
        /// <summary>
        /// Наименование
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Данные
        /// </summary>
        /// <remarks>([[x1,y1], [x2,y2], ..., [xn,yn]])</remarks>
        object[][] Data { get; }
    }
}