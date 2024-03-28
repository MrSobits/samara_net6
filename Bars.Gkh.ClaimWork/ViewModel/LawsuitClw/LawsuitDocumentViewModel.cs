namespace Bars.Gkh.ClaimWork.ViewModel.LawsuitClw
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Modules.ClaimWork.Entities;

    /// <summary>
    /// Представление для <see cref="LawsuitDocument"/>
    /// </summary>
    public class LawsuitDocumentViewModel : BaseViewModel<LawsuitDocument>
    {
        /// <summary>Получить список</summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат получения списка</returns>
        public override IDataResult List(IDomainService<LawsuitDocument> domainService, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var docId = loadParams.Filter.GetAsId("docId");

            var data = domainService.GetAll()
                .Where(x => x.Lawsuit.Id == docId)
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}