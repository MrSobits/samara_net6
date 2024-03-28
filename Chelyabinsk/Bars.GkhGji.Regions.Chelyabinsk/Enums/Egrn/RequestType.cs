using Bars.B4.Utils;

namespace Bars.GkhGji.Regions.Chelyabinsk.Enums.Egrn
{
    //Фактически, на данный момент это requestDetails
    //reuestType нужен для запроса данных путём предоставления доступа, если будем его юзать - то лучше всё привести к соответствующему виду и использовать 2 енума
    public enum RequestType
    {
        //[Display("о переходе прав на объект недвижимости")]
        //[Description("requiredRealtyList")]
        //requiredRealtyList = 10,

        //[Display("о правах отдельного лица на имеющиеся (имевшиеся) у него объекты недвижимости")]
        //[Description("requiredDataSubject")]
        //requiredDataSubject = 20,

        //[Display("об объекте недвижимости (нескольких объектах недвижимости) и зарегистрированных правах на него (них)")]
        //[Description("requiredDataRealty")]
        //requiredDataRealty = 30,

        //[Display("о зоне или территории")]
        //[Description("requiredArea")]
        //requiredArea = 40,

        //[Display("о границах")]
        //[Description("requiredBorder")]
        //requiredBorder = 50,

        //[Display("о признании правообладателя недееспособным или ограниченно дееспособным")]
        //[Description("requiredDataIncapacity")]
        //requiredDataIncapacity = 60


        [Display("Запрос копии документа")]
        [Description("requestCopyAction")]
        requestCopyAction = 10,

        [Display("Запрос на предоставление сведений об объектах недвижимости и (или) правообладателях, о зонах и территориях")]
        [Description("requestEGRNDataAction")]
        requestEGRNDataAction = 20,

        [Display("Запрос на предоставление доступа в ФГИС ЕГРН")]
        [Description("requestEGRNAccessAction")]
        requestEGRNAccessAction = 30,

        [Display("Запрос на получение уведомлений об изменении содержащихся в ЕГРН сведений")]
        [Description("requestEGRNNotification")]
        requestEGRNNotification = 40
    }
}
