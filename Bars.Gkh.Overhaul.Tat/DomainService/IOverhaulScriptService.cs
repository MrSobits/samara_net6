namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using Bars.B4;

    public interface IOverhaulScriptService
    {
        /// <summary>
        /// Метод создания структурных элементов
        /// </summary>
        BaseDataResult CreateStructElements(BaseParams baseParams);

        /// <summary>
        /// Метод обновления объемов по КЭ
        /// </summary>
        BaseDataResult UpdateVolumeStructElements(BaseParams baseParams);
    }
}