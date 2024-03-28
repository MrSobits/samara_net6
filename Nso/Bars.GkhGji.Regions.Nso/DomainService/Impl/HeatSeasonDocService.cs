namespace Bars.GkhGji.Regions.Nso.DomainService
{
    using GkhGji.Enums;

    public class HeatSeasonDocService : GkhGji.DomainService.HeatSeasonDocService
    {
        public override HeatSeasonDocType[] DocumentTypes()
        {
            // Поскольку в НСО добавляется еще несколкьо типов то переопределяем метод Возможных типов дял региона
            return new []
            {
                HeatSeasonDocType.ActFlushingHeatingSystem,
                HeatSeasonDocType.ActPressingHeatingSystem,
                HeatSeasonDocType.ActCheckVentilation,
                HeatSeasonDocType.ActCheckChimney,
                HeatSeasonDocType.ActReadyHeatingDevices,
                HeatSeasonDocType.Passport
            };
        }
    }
}