using Bars.B4;
using Bars.B4.IoC;
using Bars.Gkh.PrintForm.Entities;
using Bars.Gkh.PrintForm.Services;

namespace Bars.Gkh.PrintForm
{
    public class Module : AssemblyDefinedModule
    {
        /// <inheritdoc />
        public override void Install()
        {
            Container.RegisterTransient<IPdfTunerService, PdfTunerService>();

            this.Container.RegisterFileStorageDomainService<PdfSignInfo>();
        }
    }
}