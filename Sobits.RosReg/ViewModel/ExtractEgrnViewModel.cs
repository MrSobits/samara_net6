namespace Sobits.RosReg.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Sobits.RosReg.Entities;

    public class ExtractEgrnViewModel : BaseViewModel<ExtractEgrn>
    {

        public override IDataResult List(IDomainService<ExtractEgrn> domain, BaseParams baseParams)
        {
            var rights = this.Container.Resolve<IDomainService<ExtractEgrnRight>>();
            var loadParams = this.GetLoadParam(baseParams);
            
            var data = domain.GetAll()
                   .Select(
                  x => new
                  {
                      x.Id,
                      x.CadastralNumber,
                      x.Area,
                      x.Type,
                      x.Purpose,
                      x.Address,
                      x.ExtractDate,
                      x.ExtractNumber,
                      x.RoomId,
                      x.ExtractId,
                      x.IsMerged,
                      RoomAdr = (x.RoomId != null) ? x.RoomId.RealityObject.Address + ", кв. " + x.RoomId.RoomNum : "не привязано"
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
        public override IDataResult Get(IDomainService<ExtractEgrn> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var roomDomain = this.Container.Resolve<IDomainService<Room>>();
            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.CadastralNumber,
                        obj.Area,
                        obj.Type,
                        obj.Purpose,
                        obj.Address,
                        obj.ExtractDate,
                        obj.ExtractNumber,
                        obj.RoomId,
                        Room_id = obj.RoomId != null ? new { Id = obj.RoomId.Id, Address = obj.FullAddress } : null,
                        obj.ExtractId

                    });
            }

            return new BaseDataResult();
        }
    }
}