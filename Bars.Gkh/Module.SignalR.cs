namespace Bars.Gkh;

using Bars.Gkh.Extensions;
using Bars.Gkh.SignalR;

using Microsoft.AspNetCore.Builder;

public partial class Module
{
    private void RegisterSignalR(IApplicationBuilder app)
    {
        app.UseEndpoints(e =>
        {
            e.RegisterHub<ReportStatusHub>();
            e.RegisterHub<CountCacheHub>();
            e.RegisterHub<GkhConfigHub>();
            e.RegisterHub<GkhParamsHub>();
            e.RegisterHub<NotifyHub>();
            e.RegisterHub<ProsecutorsOfficeHub>();
        });
    }

}