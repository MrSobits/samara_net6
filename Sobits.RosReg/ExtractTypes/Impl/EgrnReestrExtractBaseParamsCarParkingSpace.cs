namespace Sobits.RosReg.ExtractTypes
{
    using Sobits.RosReg.Enums;

    /// <summary>
    /// Переопределено в модуле регоператор
    /// </summary>
    public class EgrnReestrExtractBaseParamsCarParkingSpace : EgrnReestrExtractCarParkingSpace
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnReestrExtractBaseParamsCarParkingSpace;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Парковочное место - BaseParamsCarParkingSpace";

        /// <inheritdoc />
        public override string Pattern =>
            "<extract_base_params_car_parking_space";

    }
}