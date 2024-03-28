using System;
using Bars.B4;
using Bars.B4.Application;
using Bars.B4.IoC;
using Bars.B4.Modules.FileStorage;
using Bars.B4.Utils;

namespace Bars.Gkh.RegOperator.DomainEvent.Events.PersonalAccount
{
    /// <summary>
    /// Информация для истории изменений ЛС.
    /// </summary>
    public class PersonalAccountChangeInfo
    {
        /// <summary>
        /// Дата начала действия.
        /// </summary>
        public DateTime DateActual { get; set; }

        /// <summary>
        /// Причина изменения.
        /// </summary>
        public string Reason { get; set; }

        /// <summary>
        /// Документ-основание.
        /// </summary>
        public FileInfo Document { get; set; }

        /// <summary>
        /// Создать новый ЛС?
        /// </summary>
        public bool NewLS { get; set; }

        public static PersonalAccountChangeInfo FromParams(BaseParams baseParams)
        {
            var fileManager = ApplicationContext.Current.Container.Resolve<IFileManager>();
            var fileInfoDomain = ApplicationContext.Current.Container.Resolve<IDomainService<FileInfo>>();
            using (ApplicationContext.Current.Container.Using(fileManager, fileInfoDomain))
            {
                var doc = baseParams.Files?.Get("Document");
                FileInfo fileInfo = null;
                if (doc != null)
                {
                    fileInfo = fileManager.SaveFile(doc);
                    fileInfoDomain.Save(fileInfo);
                }

                var changeInfo = new PersonalAccountChangeInfo
                {
                    DateActual = baseParams.Params.GetAs<DateTime>("ActualFrom"),
                    Reason = baseParams.Params.GetAs<string>("Reason"),
                    Document = fileInfo,
                    NewLS = baseParams.Params.GetAs<bool>("NewLSCheckBox"),
                };
                return changeInfo;
            }
        }
    }
}
