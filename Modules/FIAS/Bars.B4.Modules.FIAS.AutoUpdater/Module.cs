namespace Bars.B4.Modules.FIAS.AutoUpdater
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader;
    using Bars.B4.Modules.FIAS.AutoUpdater.ArchiveReader.Impl;
    using Bars.B4.Modules.FIAS.AutoUpdater.Converter;
    using Bars.B4.Modules.FIAS.AutoUpdater.Converter.Impl;
    using Bars.B4.Modules.FIAS.AutoUpdater.DownloadService;
    using Bars.B4.Modules.FIAS.AutoUpdater.DownloadService.Impl;
    using Bars.B4.Modules.FIAS.AutoUpdater.ExecutionActions;
    using Bars.B4.Modules.FIAS.AutoUpdater.Impl;
<<<<<<< HEAD
=======
    using Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater;
    using Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater.Impl;
    using Bars.Gkh.Utils;

    using Castle.MicroKernel.Registration;
>>>>>>> net6

    public class Module : AssemblyDefinedModule
    {
        public override void Install()
        {
            this.Container.RegisterTransient<IFiasAutoUpdater, FiasAutoUpdater>();
            this.Container.RegisterTransient<IFiasDownloadService, FiasDownloadService>();
            this.Container.RegisterTransient<IFiasArchiveReader, FiasArchiveReader>();
<<<<<<< HEAD
            this.Container.RegisterTransient<IFiasDbConverter, FiasDbConverter>();
=======
            this.Container.RegisterTransient<ITableUpdater, NpgsqlTableUpdater>();
            this.Container.RegisterTransient<ITableUpdateHelper<Fias>, FiasUpdateHelper>();
            this.Container.RegisterTransient<ITableUpdateHelper<FiasHouse>, FiasHouseUpdateHelper>();
            this.Container.Register(Component.For<IFiasArchiveReader>().ImplementedBy<FiasDbfArchiveReader>().LifestyleTransient().Named("DbfFiasArchiveReader"));
            this.Container.Register(Component.For<IFiasArchiveReader>().ImplementedBy<FiasGarArchiveReader>().LifestyleTransient().Named("GarFiasArchiveReader"));
            this.Container.Register(Component.For<IFiasDbConverter>().ImplementedBy<FiasDbfFormatDbConverter>().LifestyleTransient().Named("DbfFiasConverter"));
            this.Container.Register(Component.For<IFiasDbConverter>().ImplementedBy<FiasGarFormatDbConverter>().LifestyleTransient().Named("GarFiasConverter"));
            this.Container.RegisterExecutionAction<FiasAutoUpdaterAction>();
>>>>>>> net6
        }
    }
}