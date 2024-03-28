namespace Bars.GkhCr.DomainService
{
    using B4;
    using Entities;
	using System.Linq;

	/// <summary>
	/// ViewModel для <see cref="CompetitionLotBid"/>
	/// </summary>
	public class CompetitionLotBidViewModel : BaseViewModel<CompetitionLotBid>
    {
		/// <summary>
		/// Получить список
		/// </summary>
		/// <param name="domainService">Домен-сервис</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
        public override IDataResult List(IDomainService<CompetitionLotBid> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var lotId = loadParams.Filter.GetAs("lotId", 0l);

            var data = domainService.GetAll()
                        .Where(x => x.Lot.Id == lotId)
                        .Select(x => new
                        {
                            x.Id,
                            Builder = x.Builder.Contragent.Name,
                            x.IncomeDate,
                            Price = x.Price > 0 ? x.Price : x.PriceNds,
                            x.Points,
                            x.IsWinner
                        })
                        .Filter(loadParams, this.Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

		/// <summary>
		/// Получить объект
		/// </summary>
		/// <param name="domainService">Домен-сервис</param>
		/// <param name="baseParams">Базовые параметры</param>
		/// <returns>Результат выполнения запроса</returns>
		public override IDataResult Get(IDomainService<CompetitionLotBid> domainService, BaseParams baseParams)
        {
            
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.GetAll()
                                .Where(x => x.Id == id)
                                .Select(
                                    x =>
                                    new
                                    {
                                        x.Id,
                                        Lot = x.Lot.Id,
                                        Builder = x.Builder != null ? new { Id = x.Builder.Id , ContragentName = x.Builder.Contragent.Name } : null,
                                        x.IncomeDate,
                                        x.Points,
                                        x.Price,
                                        x.PriceNds,
                                        x.IsWinner
                                    })
                                .FirstOrDefault();

            return new BaseDataResult(obj);

        }
    }
}