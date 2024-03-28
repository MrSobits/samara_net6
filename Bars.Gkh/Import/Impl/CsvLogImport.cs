namespace Bars.Gkh.Import
{
    using System;
    using System.IO;
    using System.Text;

    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Реализация интерфейса “ILogImport” для формирования файла лога в формате csv
    /// </summary>
    public class CsvLogImport : ILogImport
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public CsvLogImport()
        {
            this.FileLog = new MemoryStream();
            this.Write($"Тип сообщения;Заголовок;Описание{Environment.NewLine}");
            this.FileErrorsRow = new MemoryStream();
        }

        /// <summary>
        /// .ctor
        /// </summary>
        /// <param name="logHeader">Шапка импорта</param>
        public CsvLogImport(string logHeader)
        {
            this.FileLog = new MemoryStream();
            this.Write(logHeader);
        }

        public string UserName { get; set; }

        public Stream FileLog { get; private set; }

        /// <inheritdoc />
        public string Key => MainLogImportInfo.Key;

        /// <inheritdoc />
        public string FileName { get; private set; }

        /// <inheritdoc />
        public Operator Operator { get; set; }

        /// <inheritdoc />
        public bool IsImported { get; set; }

        /// <inheritdoc />
        public string ImportKey { get; set; }

        /// <inheritdoc />
        public int CountWarning { get; set; }

        /// <inheritdoc />
        public int CountError { get; set; }

        /// <inheritdoc />
        public int CountAddedRows { get; set; }

        /// <inheritdoc />
        public int CountChangedRows { get; set; }

        /// <inheritdoc />
        public int CountImportedRows => this.CountAddedRows + this.CountChangedRows;

        public Stream FileErrorsRow { get; set; }

        /// <inheritdoc />
        public void SetFileName(string fileNameWithoutExtention)
        {
            this.FileName = $"{fileNameWithoutExtention}.log.csv";
        }

        /// <inheritdoc />
        public void Error(string title, string description, params string[] args)
        {
            this.CountError++;
            description = description.Replace(";", ".");

            this.Write($"Ошибка;{title};{description};{string.Join(";", args)}\n");
        }

        /// <inheritdoc />
        public void Warn(string title, string description, params string[] args)
        {
            this.CountWarning++;
            description = description.Replace(";", ".");

            this.Write($"Предупреждение;{title};{description};{string.Join(";", args)}\n");
        }

        /// <inheritdoc />
        public void Info(string title, string description, params string[] args)
        {
            description = description.Replace(";", ".");
            this.Write($"Информация;{title};{description};{string.Join(";", args)}\n");
        }

        /// <inheritdoc />
        public void Info(string title, string description, LogTypeChanged logTypeChanged)
        {
            switch (logTypeChanged)
            {
                case LogTypeChanged.Added:
                    this.CountAddedRows++;
                    break;
                case LogTypeChanged.Changed:
                    this.CountChangedRows++;
                    break;
                default:
                    this.CountAddedRows++;
                    break;
            }

            this.Info(title, description);
        }

        /// <inheritdoc />
        public void PlacingResults()
        {
            if (this.CountError == 0)
            {
                this.Info(string.Empty, $"Общее количество добавленных строк {this.CountAddedRows}");
                this.Info(string.Empty, $"Общее количество измененных строк {this.CountChangedRows}");
                this.Info(string.Empty, $"Общее количество предупреждений {this.CountWarning}");
            }
            else
            {
                this.IsImported = false;

                this.Info(string.Empty, $"Общее количество ошибок {this.CountError}");
                this.Info(string.Empty, $"Общее количество предупреждений {this.CountWarning}");
            }
        }

        /// <inheritdoc />
        public void Debug(string title, string description, params string[] args)
        {
#if (JSDEBUG || DEBUG)
            description = description.Replace(";", ".");
            this.Write($"Отладочная информация;{title};{description};{string.Join(";", args)}\n");
#endif
        }

        /// <inheritdoc />
        public Stream GetFile()
        {
            return this.FileLog;
        }

        /// <inheritdoc />
        public Stream GetErrorRows()
        {
            return this.FileErrorsRow.Length > 0 ? this.FileErrorsRow : null;
        }

        public void AddErrorsRow(params string[] row)
        {
            if (this.FileErrorsRow.Length == 0)
            {
                this.AddErrorsHeader(Environment.NewLine);
            }

            var msg = Encoding.GetEncoding(1251).GetBytes($"{string.Join("; ", row)}{Environment.NewLine}");
            this.FileErrorsRow.Write(msg, 0, msg.Length);
        }

        public void AddErrorsHeader(params string[] header)
        {
            var hedaer = Encoding.GetEncoding(1251).GetBytes(string.Join(";", header));
            this.FileErrorsRow.Write(hedaer, 0, hedaer.Length);
        }

        /// <summary>
        /// Записать сообщение в лог
        /// </summary>
        /// <param name="message">Сообщение</param>
        public void Write(string message)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var msg = Encoding.GetEncoding(1251).GetBytes(message);
            this.FileLog.Write(msg, 0, msg.Length);
        }
    }
}