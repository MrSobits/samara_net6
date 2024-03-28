namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Intf;

    /// <summary>
    /// Получить тарифы из ЦХД
    /// </summary>
    public class TariffHandler : ITariff
    {
        public IDataResult ApplyTariffs(ref CalcTypes.ParamCalc param, string TargetTable)
        {
            //todo переопределяем тарифы данными из ЦХД
            throw new System.NotImplementedException();
        }
    }


    /// <summary>
    /// Получить тарифы из УК
    /// </summary>
    public class FakeTariffHandler : ITariff
    {
        public IDataResult ApplyTariffs(ref CalcTypes.ParamCalc param, string TargetTable)
        {
            //пока ничего не делаем, оставляем данные УК в блоке ЦХД
            return new BaseDataResult();
        }
    }
}
