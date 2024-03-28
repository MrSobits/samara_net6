namespace Bars.Gkh.FormatDataExport.ProxySelectors.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.FormatDataExport.ProxyEntities;
    using Bars.Gkh.Utils;

    /// <summary>
    /// Сервис получения <see cref="PremisesProxy"/>
    /// </summary>
    public class PremisesSelectorService : BaseProxySelectorService<PremisesProxy>
    {
        /// <inheritdoc />
        protected override ICollection<PremisesProxy> GetAdditionalCache()
        {
            var roomRepository = this.Container.ResolveRepository<Room>();
            using (this.Container.Using(roomRepository))
            {
                return this.GetProxies(roomRepository.GetAll().WhereContainsBulked(x => x.Id, this.AdditionalIds));
            }
        }

        protected override IDictionary<long, PremisesProxy> GetCache()
        {
            var roomRepository = this.Container.ResolveRepository<Room>();
            using (this.Container.Using(roomRepository))
            {
                var roQuery = this.FilterService.GetFiltredQuery<RealityObject>();

                var query = roomRepository.GetAll()
                    .Where(x => roQuery.Any(r => r.Id == x.RealityObject.Id));

                return this.GetProxies(query)
                    .ToDictionary(x => x.Id);
            }
        }

        protected ICollection<PremisesProxy> GetProxies(IQueryable<Room> roomQuery)
        {
            return roomQuery
                .Select(x => new
                {
                    x.Id,
                    RoId = (long?) x.RealityObject.Id,
                    EntranceId = (long?) x.Entrance.Id,
                    x.RoomNum,
                    x.Type,
                    TypeHouse = (TypeHouse?) x.RealityObject.TypeHouse,
                    x.CadastralNumber,
                    x.PrevAssignedRegNumber,
                    x.Floor,
                    x.Area,
                    x.LivingArea,
                    x.IsRoomCommonPropertyInMcd,
                    x.IsCommunal,
                    x.RecognizedUnfit,
                    x.RecognizedUnfitReason,
                    x.RecognizedUnfitDocDate,
                    x.RecognizedUnfitDocNumber,
                    x.HasSeparateEntrance
                })
                .AsEnumerable()
                .Select(x => new PremisesProxy
                {
                    Id = x.Id,
                    HouseId = x.RoId,
                    Type = this.GetType(x.Type),
                    HasSeparateEntrace = x.HasSeparateEntrance ? 1 : 2,
                    EntraceId = x.EntranceId,
                    Number = x.RoomNum,
                    IsCommonProperty = x.IsRoomCommonPropertyInMcd ? 1 : 2,
                    TypeHouse = this.GetTypeHouse(x.TypeHouse, x.IsCommunal),
                    Area = x.Area > 0 ? x.Area : default(decimal?),
                    HasLivingArea = x.Type == RoomType.Living
                        ? (x.LivingArea > 0 ? 1 : 2)
                        : default(int?),
                    LivingArea = x.LivingArea > 0 ? x.LivingArea : default(decimal?),
                    HasCadastralHouseNumber = !string.IsNullOrWhiteSpace(x.CadastralNumber) || x.PrevAssignedRegNumber.HasValue ? 1 : 2,
                    CadastralHouseNumber = x.CadastralNumber,
                    EigrpNumber = string.IsNullOrWhiteSpace(x.CadastralNumber) ? x.PrevAssignedRegNumber.ToStr() : null,
                    Floor = x.Floor,
                    HasRecognizedUnfit = x.RecognizedUnfit == YesNo.Yes ? 1 : 2,
                    RecognizedUnfitReason = x.RecognizedUnfitReason,
                    RecognizedUnfitDocDate = x.RecognizedUnfitDocDate,
                    RecognizedUnfitDocNumber = x.RecognizedUnfitDocNumber.ToStr(),
                    IsSupplierConfirmed = 1,
                    IsDeviceNotInstalled = 2,
                })
                .ToList();
        }

        private int? GetType(RoomType? roomType)
        {
            switch (roomType)
            {
                case RoomType.Living:
                    return 1;
                case RoomType.NonLiving:
                    return 2;
                default:
                    return null;
            }
        }

        private int? GetTypeHouse(TypeHouse? typeHouse, bool isCommunal)
        {
            switch (typeHouse)
            {
                case TypeHouse.SocialBehavior:
                    return 3;
                default:
                    if (isCommunal)
                    {
                        return 2;
                    }

                    return 1;
            }
        }
    }
}