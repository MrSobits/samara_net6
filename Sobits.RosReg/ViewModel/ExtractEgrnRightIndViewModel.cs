namespace Sobits.RosReg.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;

    using Sobits.RosReg.Entities;

    public class ExtractEgrnRightIndViewModel : BaseViewModel<ExtractEgrnRightInd>
    {
        public override IDataResult List(IDomainService<ExtractEgrnRightInd> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var parentId = loadParams.Filter.GetAs("parentId", 0L);
            var extractEgrnDomainRight = this.Container.ResolveDomain<ExtractEgrnRight>();
            var indIds = extractEgrnDomainRight.GetAll().Where(x => x.EgrnId.Id == parentId).ToList();
            var data = domain.GetAll()
                .Where(x=>indIds.Contains(x.RightId))
                .Select(
                    x => new
                    {
                        x.FirstName,
                        x.Surname,
                        x.Patronymic,
                        x.BirthDate,
                        x.BirthPlace,
                        x.Snils,
                        x.RightId.Number,
                        x.RightId.Share,
                        x.RightId.Type, 
                        x.DocIndName,
                        x.DocIndSerial,
                        x.DocIndNumber
                    })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <inheritdoc />
        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<ExtractEgrnRightInd> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                    });
            }

            return new BaseDataResult();
        }
    }
}