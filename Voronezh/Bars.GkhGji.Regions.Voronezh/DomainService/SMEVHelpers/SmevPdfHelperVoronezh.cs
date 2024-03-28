namespace Bars.GkhGji.Regions.Voronezh.DomainService.SMEVHelpers
{
    using System.Drawing;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers;

    public class SmevPdfHelperVoronezh : SmevPrintPdfHelper
    {
        protected override Bitmap Stamp => Properties.Resources.stamp_Voronezh;
    }
}