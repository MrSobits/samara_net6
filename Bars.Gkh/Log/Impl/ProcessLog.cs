namespace Bars.Gkh.Log.Impl
{
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using B4.Modules.FileStorage;

    using B4.Utils;

    using Bars.Gkh.Enums;

    using Ionic.Zip;
    using Utils;
    using FileInfo = B4.Modules.FileStorage.FileInfo;

    /// <summary>
    /// лог процесса
    /// </summary>
    public class ProcessLog : IProcessLog
    {
        private readonly IFileManager _fileManager;
        private readonly LinkedList<Message> _messages;

        private string _logName;

        /// <summary>
        /// .ctor
        /// </summary>
        public ProcessLog(IFileManager fileManager)
        {
            _fileManager = fileManager;
            _messages = new LinkedList<Message>();
        }

        /// <summary>
        /// Установить название файла лога
        /// </summary>
        /// <param name="logname"></param>
        public void SetLogName(string logname)
        {
            _logName = logname;
        }

        /// <summary>
        /// Добавить сообщение с информацией
        /// </summary>
        public void Info(object message, object obj = null)
        {
            AddMessage(obj, message, LogMessageType.Info);
        }

        /// <summary>
        /// Добавить отладочное сообщение
        /// </summary>
        public void Debug(object message, object obj = null)
        {
            AddMessage(obj, message, LogMessageType.Debug);
        }

        /// <summary>
        /// Добавить сообщение об ошибке
        /// </summary>
        public void Error(object message, object obj = null)
        {
            AddMessage(obj, message, LogMessageType.Error);
        }

        /// <summary>
        /// Добавить сообщение о предупреждении
        /// </summary>
        public void Warning(object message, object obj = null)
        {
            AddMessage(obj, message, LogMessageType.Warning);
        }

        private void AddMessage(object obj, object message, LogMessageType messageType)
        {
            if(message.IsNull()) return;

            var o = obj.ToStr();

            _messages.AddLast(new Message(o, message.ToString(), messageType));
        }

        /// <summary>
        /// Сохранить лог
        /// </summary>
        /// <returns></returns>
        public FileInfo Save()
        {
            if (_messages.Count < 1)
            {
                return null;
            }

            var sb = new StringBuilder();

            sb.AppendFormat("{0};{1};{2}\n", "Тип", "Объект", "Сообщение");

            foreach (var message in _messages)
            {
                sb.AppendFormat("{0};\"{1}\";\"{2}\"\n",
                    message.MessageType.GetEnumMeta().Display,
                    message.Obj,
                    message.Text);
            }

            FileInfo file;

            using (var stream = new MemoryStream(sb.ToString().GetBytes(Encoding.GetEncoding("windows-1251"))))
            {
                using (var ms = new MemoryStream())
                {
                    using (var zip = new ZipFile(Encoding.UTF8))
                    {
                        zip.AddEntry(GetLogName() + ".csv", stream);
                        zip.Save(ms);

                        file = _fileManager.SaveFile(ms, GetLogName() + ".zip");
                    }
                }
            }

            return file;
        }

        private string GetLogName()
        {
            return _logName.IsEmpty() ? "process_log" : _logName;
        }

        private class Message
        {
            public Message(string obj, string text, LogMessageType messageType)
            {
                Obj = obj;
                Text = text;
                this.MessageType = messageType;
            }

            /// <summary>
            /// Объект, к которому привязано сообщение
            /// </summary>
            public string Obj { get; private set; }

            /// <summary>
            /// Текст сообщения
            /// </summary>
            public string Text { get; private set; }

            /// <summary>
            /// Тип сообщения
            /// </summary>
            public LogMessageType MessageType { get; private set; }
        }
    }
}