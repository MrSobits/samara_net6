namespace Bars.Gkh.Modules.ClaimWork.DomainService.Lawsuit
{
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Авто селектор значения в исковом заявлении
    /// </summary>
    public interface ILawsuitAutoSelector
    {
        /// <summary>
        /// попытаться проставить все значения
        /// </summary>
        /// <param name="lawsuit"></param>
        /// <returns></returns>
        bool TrySetAll(Lawsuit lawsuit);
    }
}