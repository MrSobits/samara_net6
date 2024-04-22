using Bars.Gkh.App;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;

var app = WebHost.CreateDefaultBuilder()
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
    })
    .UseNLog()
    .UseStartup<Startup>()
    .UseKestrel(o =>
    {
        o.Limits.MaxRequestBodySize = null;
    })
    .UseIIS()
    .Build();

app.Run();