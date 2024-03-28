namespace Bars.Gkh.Overhaul.DomainService
{
    using Bars.B4;

    public interface IRealityObjectMissingCeoService
    {
        /// <summary>
        /// Добавление отсутствующих ООИ
        /// </summary>
        IDataResult AddMissingCeo(BaseParams baseParams);
    }
}