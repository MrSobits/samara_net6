namespace Bars.Gkh.Diagnostic.DomainServices.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.Text;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Queue;
    using Bars.B4.Modules.Queue.Contracts;
    using Bars.Gkh.Diagnostic.Entities;
    using Bars.Gkh.Diagnostic.Enums;
    using Bars.Gkh.Diagnostic.Helpers;

    using Castle.Windsor;

    public class CalculationServerDiagnostic : IDiagnostic
    {
        public IWindsorContainer Container { get; set; }

        public string Name
        {
            get
            {
                return "Диагностика сервера расчётов";
            }
        }

        public string Id
        {
            get
            {
                return "CalculationServerDiagnostic";
            }
        }

        public void Run(DiagnosticResult diagnostic)
        {
            BinaryFormatter binForm = new BinaryFormatter();

            var client = Container.Resolve<IQueueClient>();
            using (Container.Using(client))
            {
                var msg = new QueueMessage
                              {
                                  RoutingKey = "diagnosticRequest",
                                  Body = Encoding.UTF8.GetBytes("getCalcAssemblies")
                              };

                client.Post(msg);

                var stopWatch = new Stopwatch();
                stopWatch.Start();

                QueueMessage response;

                do
                {
                    response = client.Get("diagnosticResponse", false);

                    if (response != null)
                    {
                        client.NotifyQueue(
                            new NotifyOptions { MessageId = response.Id, MessageState = MessageState.Success });

                        break;
                    }
                }
                while (stopWatch.Elapsed < TimeSpan.FromMinutes(5));

                if (stopWatch.Elapsed > TimeSpan.FromMinutes(5))
                {
                    diagnostic.State = DiagnosticResultState.Fail;
                    diagnostic.AddMessage("Ответ от сервера расчётов не был получен");

                    return;
                }

                stopWatch.Stop();

                var memStream = new MemoryStream();

                memStream.Write(response.Body, 0, response.Body.Length);
                memStream.Seek(0, SeekOrigin.Begin);
                var calcServerAssemblies = (List<SerializableAssemblyInfo>)binForm.Deserialize(memStream);

                var calcServerAssembliesNames = calcServerAssemblies.Select(x => x.AssemblyName);

                var appAssemblies = LoadedAssembliesHelper.GetLoadedAssemblies();

                var appAssembliesNames = appAssemblies.Select(x => x.AssemblyName);

                var excessAssemblies =
                    calcServerAssemblies.Where(x => !appAssembliesNames.Contains(x.AssemblyName)).ToList();

                var missedAssemblies =
                    appAssemblies.Where(x => !calcServerAssembliesNames.Contains(x.AssemblyName)).ToList();

                var wrongVersionedAssemblies =
                    appAssemblies.Except(missedAssemblies)
                        .Select(
                            x =>
                            new CommonAssembly
                                {
                                    AssemblyName = x.AssemblyName,
                                    VersionInApp = x.AssemblyVersion,
                                    VersionInCalcServer =
                                        calcServerAssemblies.Single(y => y.AssemblyName == x.AssemblyName)
                                        .AssemblyVersion
                                }).Where(x => x.VersionInApp != x.VersionInCalcServer).ToList();

                if (excessAssemblies.Any())
                {
                    var excessMainAssemblies = excessAssemblies.Where(x => x.AssemblyName.StartsWith("Bars"));

                    if (excessMainAssemblies.Any())
                    {
                        diagnostic.State = DiagnosticResultState.Fail;
                    }

                    diagnostic.AddMessage(
                        string.Format(
                            "</br> На сервере расчётов есть лишние сборки: {0}</br>",
                            string.Join(
                                ", ",
                                excessAssemblies.Select(
                                    x => string.Format("имя {0} версия {1}", x.AssemblyName, x.AssemblyVersion)))));
                }

                if (missedAssemblies.Any())
                {
                    var missedMainAssemblies = missedAssemblies.Where(x => x.AssemblyName.StartsWith("Bars"));

                    if (missedMainAssemblies.Any())
                    {
                        diagnostic.State = DiagnosticResultState.Fail;
                    }

                    diagnostic.AddMessage(
                        string.Format(
                            "</br> На сервере расчётов отсутствуют сборки: {0}</br>",
                            string.Join(
                                ", ",
                                missedAssemblies.Select(
                                    x => string.Format("имя {0} версия {1}", x.AssemblyName, x.AssemblyVersion)))));
                }

                if (wrongVersionedAssemblies.Any())
                {
                    var wrongVersionedMainAssemblies =
                        wrongVersionedAssemblies.Where(x => x.AssemblyName.StartsWith("Bars"));

                    if (wrongVersionedMainAssemblies.Any())
                    {
                        diagnostic.State = DiagnosticResultState.Fail;
                    }

                    diagnostic.AddMessage(
                        string.Format(
                            "</br> На сервере расчётов не правильные версии сборок: {0}</br>",
                            string.Join(
                                ", ",
                                wrongVersionedAssemblies.Select(
                                    x =>
                                    string.Format(
                                        "имя {0} версия {1} вместо {2}",
                                        x.AssemblyName,
                                        x.VersionInCalcServer,
                                        x.VersionInApp)))));
                }

                if (diagnostic.State != DiagnosticResultState.Fail)
                {
                    diagnostic.State = DiagnosticResultState.Success;

                    diagnostic.AddMessage("Успешно");
                }
                var dsDiagnosticResult = Container.Resolve<IDomainService<DiagnosticResult>>();

                try
                {
                    dsDiagnosticResult.Update(diagnostic);
                }
                finally
                {
                    Container.Release(dsDiagnosticResult);
                }
            }
        }

        private class CommonAssembly
        {
            public string AssemblyName { get; set; }

            public string VersionInApp { get; set; }

            public string VersionInCalcServer { get; set; }
        }
    }
}
