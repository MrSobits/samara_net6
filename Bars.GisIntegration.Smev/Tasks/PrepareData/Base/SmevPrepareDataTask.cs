namespace Bars.GisIntegration.Smev.Tasks.PrepareData.Base
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using System.Xml.Serialization;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Tasks.PrepareData;
    using Bars.GisIntegration.Smev.SmevExchangeService;

#if !DEBUG
    using System.Diagnostics;
#endif

    public abstract class SmevPrepareDataTask<TContentModel, TDocumentGji> : BasePrepareDataTask<ServiceRequestType>
    {
        private bool isTestMessage;

        /// <summary>
        /// Если стоит true, то заставляет erp присылать тестовый ответ
        /// </summary>
        public bool IsTestMessage
        {
            get
            {
                return this.isTestMessage;
            }

            protected set
            {
                this.isTestMessage = value;
            }
        }

        /// <inheritdoc />
        protected sealed override Dictionary<ServiceRequestType, Dictionary<Type, Dictionary<string, long>>> GetRequestData()
        {
            var objectId = 0L;

            var requestObject = this.GetRequestObject(ref this.isTestMessage, out objectId);
            var serializedRequest = this.SerializeRequest(requestObject);

            this.SaveRequestXml(objectId, serializedRequest);

            var request = new ServiceRequestType
            {
                base64Request = Convert.ToBase64String(Encoding.UTF8.GetBytes(serializedRequest.ToString())),
                isTestMessage = this.IsTestMessage
            };

            return new Dictionary<ServiceRequestType, Dictionary<Type, Dictionary<string, long>>>
            {
                {
                    request,
                    new Dictionary<Type, Dictionary<string, long>> { { typeof(TDocumentGji), new Dictionary<string, long> { { "Id", objectId } } } }
                }
            };
        }

        /// <summary>
        /// Сериализовать объект в <see cref="System.Xml.Linq.XElement"/>
        /// </summary>
        /// <param name="requestObject">Объект запроса</param>
        /// <returns></returns>
        protected virtual XElement SerializeRequest(TContentModel requestObject)
        {
            using (var memoryStream = new MemoryStream())
            using (TextWriter streamWriter = new StreamWriter(memoryStream))
            {
                var xmlSerializer = new XmlSerializer(typeof(TContentModel));
                xmlSerializer.Serialize(streamWriter, requestObject, this.GetXmlSerializerNamespaces());
                return XElement.Parse(Encoding.UTF8.GetString(memoryStream.ToArray()));
            }
        }

        protected abstract XmlSerializerNamespaces GetXmlSerializerNamespaces();

        /// <summary>
        /// Получить объект запроса
        /// </summary>
        /// <param name="isTestMessage"></param>
        /// <param name="objectId"></param>
        /// <returns></returns>
        protected abstract TContentModel GetRequestObject(ref bool isTestMessage, out long objectId);

        protected SmevPrepareDataTask()
        {
#if DEBUG
            this.IsTestMessage = true;
#else
            this.IsTestMessage = Debugger.IsAttached;
#endif
        }

        /// <summary>
        /// Сохраняет файл запроса.
        /// </summary>
        /// <param name="disposalId">Идентификатор распоряжения.</param>
        /// <param name="serializedRequest">Файл запроса.</param>
        private void SaveRequestXml(long disposalId, XElement serializedRequest)
        {
            var fileManager = this.Container.Resolve<IFileManager>();
            var taskDomain = this.Container.ResolveDomain<RisTask>();
            var taskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();
            using (this.Container.Using(fileManager, taskDomain, taskTriggerDomain))
            {
                var fileData = new FileData($"Request_xml_{disposalId}", "xml", Encoding.UTF8.GetBytes(serializedRequest.ToString()));
                var file = fileManager.SaveFile(fileData);
                var task = taskTriggerDomain.FirstOrDefault(x => x.Trigger.Id == this.StorableTrigger.Id)?.Task;

                if (task == null)
                {
                    return;
                }

                task.RequestXmlFile = file;
                taskDomain.Update(task);
            }
        }
    }
}