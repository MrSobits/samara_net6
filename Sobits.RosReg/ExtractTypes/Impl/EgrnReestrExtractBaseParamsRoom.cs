namespace Sobits.RosReg.ExtractTypes
{
    using Sobits.RosReg.Enums;

    public class EgrnReestrExtractBaseParamsRoom : EgrpReestrExtractAboutPropertyRoom
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnReestrExtractBaseParamsRoom;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Помещения (eapr_v02)";

        /// <inheritdoc />
        public override string Pattern =>
            "<extract_base_params_room";
    }
}