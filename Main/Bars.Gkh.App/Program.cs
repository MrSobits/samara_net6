using Bars.Gkh.App;

using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;

var app = WebHost.CreateDefaultBuilder()
    .ConfigureLogging(builder =>
    {
        builder.ClearProviders();
    })
    .UseStartup<Startup>()
    .UseKestrel(o =>
    {
        o.Limits.MaxRequestBodySize = null;
    })
    .UseIIS()
    .Build();

app.Run();