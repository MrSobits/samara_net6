namespace Sobits.RosReg.ExtractTypes
{
    using System;

    using Bars.B4.DataAccess;
    using Sobits.RosReg.Entities;
    using Sobits.RosReg.Enums;

    /// <summary>
    /// Переопределено в модуле регоператор
    /// </summary>
    public class EgrnReestrExtractCarParkingSpace : ExtractTypeBase
    {
        /// <inheritdoc />
        public override ExtractType Code => ExtractType.EgrnReestrExtractCarParkingSpace;

        /// <inheritdoc />
        public override ExtractCategory Category => ExtractCategory.EgrnParking;

        /// <inheritdoc />
        public override string Name => "ЕГРН - Парковочное место - CarParkingSpace";

        /// <inheritdoc />
        public override string Pattern => "<extract_about_property_car_parking_space>";

        /// <inheritdoc />
        public override string Xslt => null; //Xslt-файлов на данный момент нет и не предвидится для данного типа выписок

        /// <inheritdoc />
        public override void Parse(Extract extract)
        {
            var extractService = this.Container.ResolveDomain<Extract>();
            var extractEgrnService = this.Container.ResolveDomain<ExtractEgrn>();
            var extractEgrnRightService = this.Container.ResolveDomain<ExtractEgrnRight>();
            var extractEgrnRightIndService = this.Container.ResolveDomain<ExtractEgrnRightInd>();
            try
            {
                var newExtractEgrn = new ExtractEgrn();        

                newExtractEgrn.ExtractId = extract;
                newExtractEgrn.IsMerged = Bars.Gkh.Enums.YesNoNotSet.No;
                try
                {
                    extractEgrnService.Save(newExtractEgrn);
                }
                catch (Exception e)
                {
                    extract.Comment = e.Message + " " + e.StackTrace;
                    extract.IsParsed = true;
                    extractService.Update(extract);
                }

                extract.IsParsed = true;
                extractService.Update(extract);
            }
            catch (Exception e)
            {
                string str = e.Message;
            }
            finally
            {
                this.Container.Release(extractService);
                this.Container.Release(extractEgrnService);
                this.Container.Release(extractEgrnRightService);
                this.Container.Release(extractEgrnRightIndService);
            }
        }
    }
}