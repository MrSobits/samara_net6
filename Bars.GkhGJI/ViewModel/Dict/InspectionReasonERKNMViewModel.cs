namespace Bars.GkhGji.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Вьюмодель для НПА проверки
    /// </summary>
    public class InspectionReasonERKNMViewModel : BaseViewModel<InspectionReasonERKNM>
    {
        /// <summary>
        /// Получить список
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<InspectionReasonERKNM> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            var docType = baseParams.Params.GetAs("ERKNMDocumentType", ERKNMDocumentType.NotSet);

            var data = domainService.GetAll()
                .WhereIf(docType != ERKNMDocumentType.NotSet, x => x.ERKNMDocumentType == docType)
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}