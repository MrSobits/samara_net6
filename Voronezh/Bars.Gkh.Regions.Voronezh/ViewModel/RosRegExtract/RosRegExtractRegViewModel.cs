namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Entities;

    public class RosRegExtractRegViewModel : BaseViewModel<RosRegExtractReg>
    {
        public override IDataResult List(IDomainService<RosRegExtractReg> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
               .Select(x => new
               {
                   x.Id,
                   x.Reg_ID_Record,
                   x.Reg_RegNumber,
                   x.Reg_Type,
                   x.Reg_Name,
                   x.Reg_RegDate,
                   x.Reg_ShareText
               })
                .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<RosRegExtractReg> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Reg_ID_Record,
                        obj.Reg_RegNumber,
                        obj.Reg_Type,
                        obj.Reg_Name,
                        obj.Reg_RegDate,
                        obj.Reg_ShareText
                    });
            }
            return new BaseDataResult();
        }
    }
}
