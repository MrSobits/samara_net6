namespace Bars.Gkh.Diagnostic
{
    using System;
    using System.IO;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;
    using System.Threading;

    using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.Events;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Queue;
    using Bars.B4.Modules.Queue.Contracts;
    using Bars.B4.Windsor;
    using Bars.Gkh.Diagnostic.Controllers;
    using Bars.Gkh.Diagnostic.DomainServices;
    using Bars.Gkh.Diagnostic.DomainServices.Impl;
    using Bars.Gkh.Diagnostic.Entities;
    using Bars.Gkh.Diagnostic.Helpers;
    using Bars.Gkh.Diagnostic.ViewModels;
    using Castle.MicroKernel.Registration;
    using Permissions;

    public class Module : AssemblyDefinedModule
    {
        private Thread Thread;
        private volatile bool RunThread = true;

        public override void Install()
        {
            this.Container.RegisterTransient<IDiagnostic, CalculationServerDiagnostic>();
            this.Container.RegisterTransient<IDiagnostic, MigrationsDiagnostic>();
            this.Container.RegisterTransient<IDiagnostic, ModulesDiagnostic>();

            this.Container.RegisterController<CollectedDiagnosticResultController>();
            this.Container.RegisterAltDataController<DiagnosticResult>();

            this.Container.RegisterViewModel<CollectedDiagnosticResult, CollectedDiagnosticResultViewModel>();
            this.Container.RegisterViewModel<DiagnosticResult, DiagnosticResultViewModel>();
            this.Container.RegisterTransient<IRunDiagnosticService, RunDiagnosticService>();

            this.Container.Register(Component.For<IPermissionSource>().ImplementedBy<DiagnosticPermissionMap>());

            this.Container.RegisterTransient<IResourceManifest, ResourceManifest>("GkhDiagnostic res");

            this.Container.RegisterTransient<INavigationProvider, NavigationProvider>();
            this.Container.RegisterTransient<IClientRouteMapRegistrar, ClientRouteMapRegistrar>();

            if (!AppDomain.CurrentDomain.IsWebApp())
            {
                ApplicationContext.Current.Events.GetEvent<AppStartEvent>().Subscribe(
                    e =>
                        {
                            this.Thread = new Thread(ListenDiagnosticQueue);

                            this.Thread.Start();
                        });
            }
        }

        public void ListenDiagnosticQueue()
        {
            var client = this.Container.Resolve<IQueueClient>();

            while (this.RunThread)
            {
                var message = client.Get("diagnosticRequest", true);

                if (message == null)
                {
                    return;
                }

                if (Encoding.UTF8.GetString(message.Body) == "getCalcAssemblies")
                {
                    client.NotifyQueue(new NotifyOptions { MessageId = message.Id, MessageState = MessageState.Success });

                    var assemblies = LoadedAssembliesHelper.GetLoadedAssemblies();

                    var binFormatter = new BinaryFormatter();
                    var mStream = new MemoryStream();
                    binFormatter.Serialize(mStream, assemblies);
                    var binaryAssemblies = mStream.ToArray();

                    var response = new QueueMessage
                                       {
                                           RoutingKey = "diagnosticResponse",
                                           Body = binaryAssemblies
                                       };

                    client.Post(response);
                }
            }
        }
    }
}
