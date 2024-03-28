namespace Bars.Gkh.Controllers
{
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.B4.Modules.FIAS;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Контроллер работы с ФИАС
    /// </summary>
    public class FiasController : B4.Modules.FIAS.FiasController
    {
        /// <summary>
        /// Список муниципальных образований
        /// </summary>
        public ActionResult ListMo(StoreLoadParams storeParams)
        {
            var service = Resolve<IDomainService<Fias>>();

            var ids = storeParams.Params.GetAs("Id", string.Empty);

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var fiasService = service.GetAll();
         //   var adressservice = Resolve<IDomainService<FiasAddress>>().GetAll();

            var data = fiasService
                 .Join(
                    fiasService,
                    x => x.ParentGuid,
                    x => x.AOGuid,
                    (a, b) => new { child = a, parent = b })
                .Where(x => (x.child.AOLevel == FiasLevelEnum.Raion || x.child.AOLevel == FiasLevelEnum.City || x.child.AOLevel == FiasLevelEnum.Place) && x.child.ActStatus == FiasActualStatusEnum.Actual && x.parent.ActStatus == FiasActualStatusEnum.Actual)
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.child.Id))
                .Select(x => new
                {
                    x.child.Id,
                    Name = x.child.OffName + " " + x.child.ShortName,
                    x.child.CodeRecord,
                    x.child.AOLevel,
                    x.child.AOGuid,
                    ParentName = x.parent.OffName + " " + x.parent.ShortName,
                    x.child.TypeRecord,
                    x.child.ActStatus,
                    x.child.MirrorGuid
                })
                .Filter(storeParams, Container);

            int totalCount = data.Count();

            data = data.Order(storeParams).Paging(storeParams);

            return new JsonListResult(data.ToList(), totalCount);
        }

        /// <summary>
        /// Список муниципальных образований
        /// </summary>
        //public ActionResult ListMo(StoreLoadParams storeParams)
        //{
        //    var service = Resolve<IDomainService<Fias>>();

        //    var ids = storeParams.Params.GetAs("Id", string.Empty);

        //    var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

        //    var data = service.GetAll()
        //        .Where(x => (x.AOLevel == FiasLevelEnum.Raion || x.AOLevel == FiasLevelEnum.City || x.AOLevel == FiasLevelEnum.Place) && x.ActStatus == FiasActualStatusEnum.Actual)
        //        .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
        //        .Select(x => new
        //        {
        //            x.Id,
        //            Name = x.OffName + " " + x.ShortName,
        //            x.CodeRecord,
        //            x.AOLevel,
        //            x.AOGuid,
        //            x.TypeRecord,
        //            x.ActStatus,
        //            x.MirrorGuid
        //        })
        //        .Filter(storeParams, Container);

        //    int totalCount = data.Count();

        //    data = data.Order(storeParams).Paging(storeParams);

        //    return new JsonListResult(data.ToList(), totalCount);
        //}

        /// <summary>
        /// Список населенных пунктов
        /// </summary>
        public ActionResult ListPlaces(StoreLoadParams storeParams)
        {
            var municipalityRepository = Container.Resolve<IRepository<Municipality>>();
            var fiasRepository = Container.Resolve<IFiasRepository>();

            try
            {
                var municipalityId = storeParams.Params.GetAs<long>("municipalityId");

                var fiasGuid = municipalityRepository.GetAll()
                    .Where(x => x.Id == municipalityId)
                    .Select(x => x.FiasId ?? x.ParentMo.FiasId)
                    .FirstOrDefault();

                //ищем также по mirrorGuid, так как, например, для г. Казань районы добавляются вручную 
                //и по parentGuid не найдутся их населенные пункты
                var dynamicAddress = fiasGuid.IsNotEmpty() ? fiasRepository.GetDinamicAddress(fiasGuid) : null;
                var mirrorGuid = dynamicAddress != null ? dynamicAddress.MirrorGuid : null;
                var parentGuidId = dynamicAddress != null ? dynamicAddress.ParentGuidId : null;

                var data = DomainService.GetAll()
                    .Where(x => x.AOLevel == FiasLevelEnum.Place || x.AOLevel == FiasLevelEnum.City)
                    .Where(x => x.ParentGuid == fiasGuid || x.ParentGuid == mirrorGuid || x.ParentGuid == parentGuidId)
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Filter(storeParams, Container);

                var totalCount = data.Count();
                data = data.Order(storeParams).Paging(storeParams);

                return new JsonListResult(data.ToList(), totalCount);
            }
            finally
            {
                Container.Release(municipalityRepository);
                Container.Release(fiasRepository);
            }
        }
    }
}