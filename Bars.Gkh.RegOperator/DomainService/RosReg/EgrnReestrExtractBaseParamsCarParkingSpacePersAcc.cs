namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using Sobits.RosReg.Enums;

    public class EgrnReestrExtractBaseParamsCarParkingSpacePersAcc : EgrnReestrExtractCarParkingSpacePersAcc
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