namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Linq;
    using B4.IoC;
    using Castle.Windsor;
    using Export.ExportToEbir;

    internal class ExportToEbirService : IExportToEbirService
    {
        private readonly IWindsorContainer _container;

        public ExportToEbirService(IWindsorContainer container)
        {
            _container = container;
        }

        /// <summary>
        /// Экспортировать
        /// </summary>
        public long Export(string type, long periodId)
        {
            var ebirExports = _container.ResolveAll<IEbirExport>();

            using (_container.Using((object)ebirExports))
            {
                var typedExport = ebirExports.FirstOrDefault(x => x.Format == type);

                if (typedExport == null)
                {
                    throw new Exception("Неизвестный формат");
                }

                return typedExport.GetExportFileId(periodId);
            }
        }
    }
}