using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Text;
using System.Threading;

using Bars.B4;
using Bars.B4.Application;
using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Entities.Email;
using Bars.GkhGji.Enums;
using Castle.Windsor;
using MailKit.Net.Imap;
using MailKit.Search;
using MailKit.Security;
using MailKit;
using Newtonsoft.Json;

namespace Bars.GkhGji.Tasks
{
    using ExecutionContext = Bars.B4.Modules.Tasks.Common.Contracts.ExecutionContext;

    public class EmailGjiTaskExecutor : ITaskExecutor
    { 
        private IDomainService<B4.Modules.FileStorage.FileInfo> _fileDomain;
        private ITaskManager _taskManager;
        private IWindsorContainer container;
        

        public static string Id = MethodBase.GetCurrentMethod().DeclaringType.FullName;

        public IFileManager _fileManager { get; set; }

        public IDomainService<EmailGji> EmailGjiDomain { get; set; }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string ExecutorCode { get; private set; }

        public IDataResult Execute(
            BaseParams @params,
            ExecutionContext ctx,
            IProgressIndicator indicator,
            CancellationToken ct)
        {
            EmailGji email = EmailGjiDomain.Get((long)@params.Params["taskId"]);
            if (email == null)
                return new BaseDataResult(false, $"Запрос с ID {@params.Params["taskId"]} не найден");

            //отправка
            try
            {
                if (email.EmailDenailReason == null || email.EmailDenailReason == EmailDenailReason.NotSet)
                {
                    using (var client = new ImapClient())
                    {
                        client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        client.Connect("mail2.gov74.ru", 993, SecureSocketOptions.Auto);
                        client.Authenticate("og@gzhi.gov74.ru", "ySp4mXJ0");
                        client.Inbox.Open(FolderAccess.ReadWrite);

                        var declineFolder = client.GetFolder("Зарегистрированные");
                        var uids = client.Inbox.Search(SearchQuery.HeaderContains("Message-Id", email.Description));
                        if (uids.Count > 0)
                        {
                            client.Inbox.MoveTo(uids, declineFolder);
                        }
                        client.Disconnect(true);
                    }
                }
                else
                {
                    using (var client = new ImapClient())
                    {
                        client.SslProtocols = System.Security.Authentication.SslProtocols.Tls12;
                        client.Connect("mail2.gov74.ru", 993, SecureSocketOptions.Auto);
                        client.Authenticate("og@gzhi.gov74.ru", "ySp4mXJ0");
                        client.Inbox.Open(FolderAccess.ReadWrite);

                        var declineFolder = client.GetFolder("Отклоненные");
                        var uids = client.Inbox.Search(SearchQuery.HeaderContains("Message-Id", email.Description));
                        if (uids.Count > 0)
                        {
                            client.Inbox.MoveTo(uids, declineFolder);
                        }
                        client.Disconnect(true);
                    }
                }
               
                return new BaseDataResult(true, "Успешно");                
             
            }
            catch (Exception e)
            {
                return new BaseDataResult(false, $"Ошибка {e.InnerException}");
            }
        }
    }
}
