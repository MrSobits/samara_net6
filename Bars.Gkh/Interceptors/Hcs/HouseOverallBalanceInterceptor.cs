namespace Bars.Gkh.Interceptors
{
    using Bars.B4;
    using Entities.Hcs;

    public class HouseOverallBalanceInterceptor : EmptyDomainInterceptor<HouseOverallBalance>
    {
        public override IDataResult BeforeCreateAction(IDomainService<HouseOverallBalance> service, HouseOverallBalance entity)
        {
            this.SetCompositeKey(entity);

            return Success();
        }

        public override IDataResult BeforeUpdateAction(IDomainService<HouseOverallBalance> service, HouseOverallBalance entity)
        {
            this.SetCompositeKey(entity);

            return Success();
        }

        //формируем составной ключ
        private void SetCompositeKey(HouseOverallBalance entity)
        {
            entity.CompositeKey = string.Format("{0}#{1}#{2}",
                entity.RealityObject.CodeErc,
                entity.DateCharging.ToString("dd-MM-yyyy"),
                entity.Service != null ? entity.Service.ToLower().Replace(" ", "") : "пусто");
        }
    }
}