namespace Sobits.RosReg.Enums
{
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr.Enums;

    public enum ExtractType
    {
        [Display("Не определено")]
        NotSet = 0,

        [Display("ЕГРН - Помещения - ExtractObject06")]
        EgrnExtractObject06 = 100,

        [Display("ЕГРН - Помещения юр.лиц - ExtractSubject06")]
        EgrnExtractSubject06 = 101,

        [Display("ЕГРН - Помещения - Reestr_Extract_List07")]
        EgrnExtractList07 = 102,

        [Display("ЕГРН - Помещения - ExtractBig")]
        EgrnExtractBig = 200,

        [Display("ЕГРН - Помещения - eapr01")]
        EgrnExtractAboutPropertyRoom = 300,
        
        [Display("ЕГРН - Помещения - eapr02")]
        EgrnReestrExtractBaseParamsRoom = 400,

        [Display("ЕГРН - Смена собственника - TransferRights")]
        EgrnReestrExtractTransferRights = 500,

        [Display("ЕГРН - Парковочное место - CarParkingSpace")]
        EgrnReestrExtractCarParkingSpace = 600,

        [Display("ЕГРН - Парковочное место - BaseParamsCarParkingSpace")]
        EgrnReestrExtractBaseParamsCarParkingSpace = 601
    }
}