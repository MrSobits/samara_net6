namespace Bars.GisIntegration.Smev.Tasks.SendData.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Smev.DomainService;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.GisIntegration.Smev.Entity.ERKNM;
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;
    using Bars.Gkh.Smev3.Attachments;

    using Fasterflect;

    public abstract class ErknmSendDataTask<TServiceResponseType> : SmevBaseSendDataTask<TServiceResponseType, LetterFromErknmType, ErknmEntity>
    {
        /// <inheritdoc />
        protected override string SmevCommunicationUrl => "urn://ru.gov.proc.erknm.communication/6.0.2";

        /// <summary>
        /// Обновить гуиды ЕРКНМ
        /// </summary>
        /// <remarks>Вложенность ограничена: GUID из object.[Ключ].[Значение]</remarks>
        protected virtual void UpdateErknmGuids(object response, Dictionary<Type, string[]> guidObjectsDict)
        {
            foreach (var keyValuePair in guidObjectsDict)
            {
                var items = response?.TryGetValue(keyValuePair.Key.Name);

                if (items.IsNotNull())
                {
                    var objType = items.GetType();
                    var objList = new List<object>();

                    if (objType.IsArray && items is object[] objArray)
                    {
                        objList.AddRange(objArray);
                    }
                    else
                    {
                        objList.Add(items);
                    }

                    foreach (var obj in objList)
                    {
                        foreach (var propertyName in keyValuePair.Value)
                        {
                            var newGuid = obj.TryGetValue(propertyName);

                            if (newGuid.IsNotNull())
                            {
                                this.UpdateGuid(newGuid.ToString());
                            }
                        }
                    }
                }
            }
            
            this.AdditionalObjectProcessing(response);
        }

        /// <summary>
        /// Дополнительная обработка
        /// </summary>
        protected virtual void AdditionalObjectProcessing(object obj)
        {
        }

        /// <inheritdoc />
        protected override Smev3Attachment[] GetAttachments()
        {
            var erknmEntityDomain = this.Container.ResolveDomain<ErknmEntity>();
            var fileMetadataDomain = this.Container.ResolveDomain<FileMetadata>();
            var taskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();

            var attachmentList = new List<Smev3Attachment>();

            using (this.Container.Using(erknmEntityDomain, fileMetadataDomain, taskTriggerDomain))
            {
                var task = taskTriggerDomain.FirstOrDefault(x => x.Trigger.Id == this.StorableTrigger.Id)?.Task;

                var filesMetaDataDict = erknmEntityDomain.GetAll()
                    .Where(x => x.Task.Id == task.Id && x.EntityType == typeof(FileMetadata).FullName)
                    .Select(x => new
                    {
                        x.EntityId,
                        x.Guid
                    })
                    .ToDictionary(x => x.EntityId, x => x.Guid);

                var files = fileMetadataDomain.GetAll()
                    .Where(x => filesMetaDataDict.Keys.Contains(x.Id));

                foreach (var fileMetaData in files)
                {
                    attachmentList.Add( new Smev3RequestAttachment
                    {
                        AttachmentId = filesMetaDataDict.Get(fileMetaData.Id),
                        FileId = fileMetaData.Id,
                        FileName = fileMetaData.Name + "." + fileMetaData.Extension
                    });
                }

                return attachmentList.ToArray();
            }
        }
    }
}