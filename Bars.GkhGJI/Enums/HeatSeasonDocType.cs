namespace Bars.GkhGji.Enums
{
    using Bars.B4.Utils;

    /// <summary>
    /// Тип документа отопительного сезона
    /// </summary>
    public enum HeatSeasonDocType
    {
        [Display("Акт промывки системы отопления")]
        ActFlushingHeatingSystem = 10,

        [Display("Акт опрессовки системы отопления")]
        ActPressingHeatingSystem = 20,

        [Display("Акт проверки вентиляционных каналов")]
        ActCheckVentilation = 30,

        [Display("Акт проверки дымоходов")]
        ActCheckChimney = 40,

        [Display("Паспорт готовности")]
        Passport = 50,

        // Данный тип добавлен для НСО 
        // Поскольку расширяемость слабовата при работе с енумами
        // То енумы доабвляются в базовый а на клиент давется 
        // выбор не из енума а из метода которй длякаждого региона свой
        [Display("Акт готовности теплотехнического оборудования")]
        ActReadyHeatingDevices = 60
    }
}