﻿namespace Bars.Gkh.ClaimWork.Regions.Smolensk.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.Gkh.Entities;
    using Sobits.RosReg.Entities;

    public class ExtractEgrnViewModel : BaseViewModel<ExtractEgrn>
    {
        public override IDataResult List(IDomainService<ExtractEgrn> domain, BaseParams baseParams)
        {
            var rights = this.Container.Resolve<IDomainService<ExtractEgrnRight>>();
            var loadParams = this.GetLoadParam(baseParams);

            
            var countFilter = loadParams.DataFilter?.Filters.FirstOrDefault(x => x.DataIndex == "RightsCount")?.Value;
            var rightCountDict = rights.GetAll().GroupBy(x => x.EgrnId).ToDictionary(x => x.Key.Id, y => y.Count());

            if (countFilter != null)
            {
                 var dataFiltCount = domain.GetAll()
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
                      //x.RoomId,
                      ExtractId = x.ExtractId.Id,
                      x.IsMerged,
                      RoomAdr = (x.RoomId != null) ? x.RoomId.RealityObject.Address + ", кв. " + x.RoomId.RoomNum : "не привязано",
                      RightsCount = rightCountDict.ContainsKey(x.Id) ? rightCountDict[x.Id] : 0
                  })
                   .AsEnumerable()
                   .Select(x => new
                   {
                       x.Id,
                       x.CadastralNumber,
                       x.Area,
                       x.Type,
                       x.Purpose,
                       x.Address,
                       x.ExtractDate,
                       x.ExtractNumber,
                      // x.RoomId,
                       x.ExtractId,
                       x.IsMerged,
                       x.RoomAdr,
                       x.RightsCount
                   })
                   .AsQueryable()
                   .Filter(loadParams, this.Container)
                   ;
                return new ListDataResult(dataFiltCount.Order(loadParams).Paging(loadParams).ToList(), dataFiltCount.Count());
            }
               

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
                      //x.RoomId,
                      x.ExtractId,
                      x.IsMerged,
                      RoomAdr = (x.RoomId != null) ? x.RoomId.RealityObject.Address + ", кв. " + x.RoomId.RoomNum : "не привязано",
                      RightsCount = rightCountDict.ContainsKey(x.Id) ? rightCountDict[x.Id] : 0
                  })                  
                   .Filter(loadParams, this.Container)                   
                   ;


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