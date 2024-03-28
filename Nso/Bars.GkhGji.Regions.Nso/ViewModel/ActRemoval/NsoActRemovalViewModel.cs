namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.GkhGji.ViewModel;
    using Entities;
    /// <summary>
    /// Переопределенный модель 
    /// </summary>
	public class NsoActRemovalViewModel : ActRemovalViewModel<NsoActRemoval>
    {
        /// <summary>
        /// Переопределенный get запрос для NSO
        /// </summary>
        /// <param name="domainService">Сервер домена </param>
        /// <param name="baseParams"> Индекс </param>
        /// <returns></returns>
        public override IDataResult Get(IDomainService<NsoActRemoval> domainService, BaseParams baseParams)
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
                    obj.LiteralNum,
                    obj.DocumentSubNum,
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