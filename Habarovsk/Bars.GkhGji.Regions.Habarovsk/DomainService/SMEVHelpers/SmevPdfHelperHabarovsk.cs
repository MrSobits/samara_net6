namespace Bars.GkhGji.Regions.Habarovsk.DomainService.SMEVHelpers
{
    using System.Drawing;
    using Bars.GkhGji.Regions.BaseChelyabinsk.DomainService.SMEVHelpers;

    public class SmevPdfHelperHabarovsk : SmevPrintPdfHelper
    {
        protected override Bitmap Stamp => Properties.Resources.stamp_Voronezh;
    }
}