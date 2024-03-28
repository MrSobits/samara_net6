namespace Bars.GkhGji.ViewModel.BoilerRooms
{
    using System;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities.BoilerRooms;

    public class BoilerRoomViewModel : BaseViewModel<BoilerRoom>
    {
        private readonly IDomainService<HeatingPeriod> _heatPeriodDomain;
        private readonly IDomainService<UnactivePeriod> _unactivePeriodDomain;
        private readonly IDomainService<Fias> _fiasDomain;
        private readonly IRepository<Municipality> _municiplaityDomain;

        public BoilerRoomViewModel(
            IDomainService<HeatingPeriod> heatPeriodDomain,
            IDomainService<UnactivePeriod> unactivePeriodDomain,
            IRepository<Municipality> municiplaityDomain,
            IDomainService<Fias> fiasDomain)
        {
            _heatPeriodDomain = heatPeriodDomain;
            _unactivePeriodDomain = unactivePeriodDomain;
            _municiplaityDomain = municiplaityDomain;
            _fiasDomain = fiasDomain;
        }

        public override IDataResult List(IDomainService<BoilerRoom> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            // Только находящиеся в МО, добавленных к опреатору
            var result = domainService.GetAll().Select(x => new
            {
                x.Address,
                x.Id,
                IsActive =
                    _unactivePeriodDomain.GetAll()
                        .Where(p => p.BoilerRoom.Id == x.Id)
                        .All(
                            p =>
                                !(DateTime.Now >= p.Start.GetValueOrDefault(DateTime.MinValue) &&
                                  DateTime.Now <= p.End.GetValueOrDefault(DateTime.MaxValue)))
                        ? YesNo.Yes
                        : YesNo.No,
                IsRunning =
                    _heatPeriodDomain.GetAll()
                        .Where(p => p.BoilerRoom.Id == x.Id)
                        .Any(p => DateTime.Now >= p.Start &&
                                  DateTime.Now <= p.End.GetValueOrDefault(DateTime.MaxValue))
                        ? YesNo.Yes
                        : YesNo.No
            });


            var totalCount = result.Count();
            var data = result.Filter(loadParams, Container).Order(loadParams).Paging(loadParams).ToList();


            var dataPlaceGuidIds = data.Select(x => x.Address.PlaceGuidId).ToList();
            var dataOktmoList =
                _fiasDomain.GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => dataPlaceGuidIds.Contains(x.AOGuid))
                    .Select(x => x.OKTMO)
                    .ToList();


            var dataWithMo = data.Select(
                x =>
                    new
                    {
                        Municipality =
                            _municiplaityDomain.FirstOrDefault(
                                mo =>
                                    dataOktmoList.Contains(mo.Oktmo.ToString()) || dataPlaceGuidIds.Contains(mo.FiasId)),
                        x.Address,
                        x.Id,
                        x.IsActive,
                        x.IsRunning
                    }).Select(x => new
                    {
                        Municipality = x.Municipality != null ? x.Municipality.Name : string.Empty,
                        Address = x.Address.AddressName,
                        x.Id,
                        x.IsActive,
                        x.IsRunning
                    });

            return new ListDataResult(dataWithMo, totalCount);
        }

        public override IDataResult Get(IDomainService<BoilerRoom> domainService, BaseParams baseParams)
        {

            var entity = domainService.Get(baseParams.Params["id"].To<long>());
            var entityFias = _fiasDomain.GetAll()
                    .FirstOrDefault(x => x.ActStatus == FiasActualStatusEnum.Actual && entity.Address.PlaceGuidId == x.AOGuid);
            var mo = _municiplaityDomain.FirstOrDefault(
                x =>
                    x.FiasId == entity.Address.PlaceGuidId ||
                    (entityFias != null && entityFias.OKTMO == x.Oktmo.ToString()));
            var value = new
            {
                entity.Id,
                entity.Address,
                Municipality = mo != null ? mo.Name : string.Empty
            };

            return new BaseDataResult(value);
        }
    }
}