namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel.ActRemoval
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.ActRemoval;
    using Bars.GkhGji.ViewModel;

    /// <summary>
    /// Переопределенный модель 
    /// </summary>
	public class ChelyabinskActRemovalViewModel : ActRemovalViewModel<ChelyabinskActRemoval>
    {
        /// <summary>
        /// Переопределенный get запрос для NSO
        /// </summary>
        /// <param name="domainService">Сервер домена </param>
        /// <param name="baseParams"> Индекс </param>
        /// <returns></returns>
        public override IDataResult Get(IDomainService<ChelyabinskActRemoval> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            return obj != null ? new BaseDataResult(
                new
                {
                    obj.Id,
                    obj.AcquaintedWithDisposalCopy,
                    obj.Area,
                    obj.DocumentDate,
                    obj.DocumentDateStr,
                    obj.DocumentNum,
                    obj.DocumentNumber,
                    obj.DocumentPlace,
                    obj.DocumentSubNum,
                    obj.LiteralNum,
                    obj.DocumentYear,
                    obj.Flat,
                    obj.Stage,
                    obj.State,
                    obj.TypeDocumentGji,
                    obj.TypeRemoval,
                    obj.Description,
                    DocumentTime = obj.DocumentTime.HasValue ? obj.DocumentTime.Value.ToShortTimeString() : string.Empty,
                }) : new BaseDataResult();
        }
    }
}