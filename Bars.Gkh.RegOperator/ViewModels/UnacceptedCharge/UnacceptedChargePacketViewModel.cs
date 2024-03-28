namespace Bars.Gkh.RegOperator.ViewModels.UnacceptedCharge
{
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.B4.IoC;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Domain.Repository;
    using Entities;

    /// <summary>
    /// Представление неподтвержденных начислений (в связи с рефакторингом в 1.32 они всегда имеют статус подтвержден)
    /// </summary>
    public class UnacceptedChargePacketViewModel : BaseViewModel<UnacceptedChargePacket>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<UnacceptedChargePacket> domainService, BaseParams baseParams)
        {
            var loadParam = this.GetLoadParam(baseParams);

            var chargesService = this.Container.Resolve<IDomainService<PersonalAccountCharge>>();
            var chargePeriodRepo = this.Container.Resolve<IChargePeriodRepository>();

            using (this.Container.Using(chargePeriodRepo, chargesService))
            {
                var currentPeriod = chargePeriodRepo.GetCurrentPeriod();

                var filteredData = domainService.GetAll().Filter(loadParam, this.Container);

                var pagedData = filteredData.Order(loadParam)
                    .Paging(loadParam)
                    .Select(x => new
                    {
                        x.Id,
                        x.CreateDate,
                        x.Description,
                        x.PacketState,
                        x.UserName
                    })
                    .ToList();

                var packetIds = pagedData.Select(x => x.Id).ToList();

                var chargesCountByPacket = chargesService.GetAll()
                    .Where(x => x.ChargePeriod.Id == currentPeriod.Id && packetIds.Contains(x.Packet.Id))
                    .GroupBy(x => x.Packet.Id)
                    .Select(x => new
                    {
                        PacketId = x.Key,
                        Count = x.Count()
                    })
                    .ToDictionary(x => x.PacketId, x => x.Count);

                var result = pagedData
                    .Select(x => new
                    {
                        x.Id,
                        CreateDate = x.CreateDate.ToUniversalTime(),
                        x.Description,
                        ChargesCount = chargesCountByPacket.Get(x.Id),
                        x.PacketState,
                        x.UserName
                    })
                    .Select(x => new
                    {
                        x.Id,
                        CreateDate = x.CreateDate.ToUniversalTime(),
                        Description = string.IsNullOrEmpty(x.Description)
                            ? $"Начисление по {x.ChargesCount} лицевым счетам за период {currentPeriod.Name}"
                            : x.Description,
                        x.PacketState,
                        x.UserName
                    })
                    .ToList();

                return new ListDataResult(result, filteredData.Count());
            }
        }
    }
}