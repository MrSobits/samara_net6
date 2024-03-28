namespace Bars.Gkh.Overhaul.Tat.DomainService.FormingOfCr
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.DomainEvent.Infrastructure;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.DomainService.RegionalFormingOfCr;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccountDto;

    public class RealtyObjectAccountFormationService : IRealtyObjectAccountFormationService
    {
        public ITypeOfFormingCrProvider TypeOfFormingCrProvider { get; set; }
        public IRepository<RealityObject> RealityObjectRepository { get; set; }

        /// <summary>
        /// Актуализировать способ формирования фонда в сущности дома
        /// </summary>
        /// <param name="realityObjectId">Идентификатор дома</param>
        public void ActualizeAccountFormationType(long realityObjectId)
        {
            var realityObject = this.RealityObjectRepository.Get(realityObjectId);

            if (realityObject == null)
            {
                return;
            }

            realityObject.AccountFormationVariant = this.TypeOfFormingCrProvider.GetTypeOfFormingCr(realityObject);

            this.RealityObjectRepository.Update(realityObject);
            DomainEvents.Raise(new RealityObjectForDtoChangeEvent(realityObject));
        }
    }
}
