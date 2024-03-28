namespace Bars.Gkh.ImportExport.Impl
{
    using System;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;

    using Castle.Windsor;

    public class ImportExportLogger : IImportExportLogger
    {
        private readonly IDomainService<ImportExport> _domain;

        private readonly IWindsorContainer _container;

        private readonly IFileManager _fileManager;

        private DateTime _dateStart;

        private ImportExportType _type;

        private StringBuilder _errors,
                              _messages;

        private int _errorCount,
                    _messageCount;

        public ImportExportLogger(IWindsorContainer container, IDomainService<ImportExport> domain, IFileManager fileManager)
        {
            _container = container;
            _domain = domain;
            _fileManager = fileManager;
        }

        public void Warn(string message)
        {
            _messages.AppendLine(message);
            _messageCount++;
        }

        public void Info(string message)
        {
            _messages.AppendLine(message);
            _messageCount++;
        }

        public void Error(string message)
        {
            _errors.AppendLine(message);
            _errorCount++;
        }

        public string GetErrors()
        {
            return _errors.ToString();
        }

        public string GetMessages()
        {
            return _messages.ToString();
        }

        public void Save()
        {
            try
            {
                var errors = this.GetErrors();
                var messages = this.GetMessages();

                var sb = new StringBuilder();
                sb.AppendLine("Ошибки\t" + errors);
                sb.AppendLine("Сообщения\t" + messages);

                var bytes = Encoding.UTF8.GetBytes(sb.ToString());

                _container.UsingForResolved<IDataTransaction>((c, tr) =>
                {
                    try
                    {
                        var entity = new ImportExport
                        {
                            HasErrors = _errorCount > 0,
                            HasMessages = _messageCount > 0,
                            Type = _type,
                            FileInfo =
                                _fileManager.SaveFile(_type == ImportExportType.EXPORT ? "export" : "import",
                                    "csv",
                                    bytes),
                            DateStart = _dateStart
                        };

                        _domain.Save(entity);

                        tr.Commit();
                    }
                    catch
                    {
                        tr.Rollback();
                        throw;
                    }
                });
            }
            finally
            {
                this.Begin(_type);
            }
        }

        public void Begin(ImportExportType type)
        {
            _dateStart = DateTime.Now;
            _type = type;
            _errors = new StringBuilder();
            _messages = new StringBuilder();
            _errorCount = _messageCount = 0;
        }
    }
}