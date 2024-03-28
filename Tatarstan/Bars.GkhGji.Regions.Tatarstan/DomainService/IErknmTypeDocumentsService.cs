using Bars.B4;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService
{
    public interface IErknmTypeDocumentsService
    {
        /// <summary>
        /// Получение списка типов документов ЕРКНМ без разбивки по страницам
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        JsonNetResult ListWithoutPaging(BaseParams baseParams);
    }
}
