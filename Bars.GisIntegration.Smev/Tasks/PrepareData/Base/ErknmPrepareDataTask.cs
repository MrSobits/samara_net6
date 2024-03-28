namespace Bars.GisIntegration.Smev.Tasks.PrepareData.Base
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml.Serialization;

    using Bars.B4.Config;
    using Bars.B4.DataAccess;
    using Bars.B4.DataModels;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Smev.DomainService;
    using Bars.GisIntegration.Smev.Entity;
    using Bars.GisIntegration.Smev.Entity.ERKNM;
    using Bars.GisIntegration.Smev.SmevExchangeService.ERKNM;
    using Bars.Gkh.Entities.Base;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Decision;

    public abstract class ErknmPrepareDataTask<TContentModel> : SmevPrepareDataTask<TContentModel, Decision>
    {
        /// <summary>
        /// Сервис для работы с вложениями СМЭВ
        /// </summary>
        public IAttachmentManager AttachmentManager { get; set; }

        private readonly List<ErknmEntity> erknmEntityList = new List<ErknmEntity>();

        /// <inheritdoc />
        protected override XmlSerializerNamespaces GetXmlSerializerNamespaces()
        {
            var namespaces = new XmlSerializerNamespaces();
            namespaces.Add("xsi", "http://www.w3.org/2001/XMLSchema-instance");
            namespaces.Add("erknm", "urn://ru.gov.proc.erknm.communication/6.0.2");
            namespaces.Add("erknm_types", "urn://ru.gov.proc.erknm.communication/types/6.0.2");
            return namespaces;
        }
        
        protected IAttachment[] InitAttachment(IHaveId entity, FileInfo file, Type type)
        {
            var fileMetaDataDomain = this.Container.ResolveDomain<FileMetadata>();

            using (this.Container.Using(fileMetaDataDomain))
            {
                var guid = this.erknmEntityList.FirstOrDefault(x => x.EntityId == entity.Id && x.EntityType == type.FullName)?.Guid;
                var appSettings = this.Container.Resolve<IConfigProvider>().GetConfig().GetModuleConfig("Bars.GisIntegration.Smev");
                var ftpEnabled = appSettings?.GetAs<bool>("ftpEnabled") ?? false;

                if (ftpEnabled)
                {
                    var fileMetaData = this.AttachmentManager.PackFiles(file, guid);
                    fileMetaData.Uid = Guid.Parse(guid);
                    fileMetaData.ChecksumHashAlgorithm = "MD5";
                    fileMetaDataDomain.Save(fileMetaData);
                    
                    var erknmEntity = new ErknmEntity()
                    {
                        EntityId = fileMetaData.Id,
                        EntityType = fileMetaData.GetType().FullName,
                        AssemblyType = fileMetaData.GetType().Assembly.ToString(),
                        Guid = guid,
                        FieldName = "ErknmGuid"
                    };
            
                    this.erknmEntityList.Add(erknmEntity);
                }

                return new[]
                {
                    new IAttachment
                    {
                        fileName = file.FullName,
                        guid = guid
                    }
                };   
            }
        }

        /// <summary>
        /// Проверяет существование GUID для сущностей в списке <see cref="erknmEntityList"/> и возвращает его
        /// Если в списке отсутствует запись, генерирует GUID и добавляет запись в <see cref="erknmEntityList"/>
        /// </summary>
        /// <param name="entity">Сущность для которой генерируем GUID</param>
        /// <param name="type">Тип сущности</param>
        /// <returns>Возвращает GUID</returns>
        protected string GetErknmGuid(IHaveId entity, Type type) => 
            this.GetErknmGuid(new [] { entity }, type, "ErknmGuid");
        
        /// <summary>
        /// Проверяет существование GUID для сущностей в списке <see cref="erknmEntityList"/> и возвращает его
        /// Если в списке отсутствует , генерирует GUID и добавляет запись в <see cref="erknmEntityList"/>
        /// </summary>
        /// <param name="entities">Список сущностей, для которых генерируем одинаковый GUID</param>
        /// <param name="type">Тип сущности</param>
        /// <returns>Возвращает GUID</returns>
        protected string GetErknmGuid(IEnumerable<IHaveId> entities, Type type) => 
            this.GetErknmGuid(entities, type, "ErknmGuid");

        /// <summary>
        /// Проверяет существование GUID для сущностей в списке <see cref="erknmEntityList"/> и возвращает его
        /// Если в списке отсутствует запись, генерирует GUID и добавляет запись в <see cref="erknmEntityList"/>
        /// </summary>
        /// <param name="entity">Сущность для которой генерируем GUID</param>
        /// <param name="type">Тип сущности</param>
        /// <param name="fieldName">Дополнительный параметр для гуидов отличных от ErknmGuid</param>
        /// <returns>Возвращает GUID</returns>
        protected string GetErknmGuid(IHaveId entity, Type type, string fieldName) => 
            this.GetErknmGuid(new [] { entity }, type, fieldName);
        
        /// <summary>
        /// Проверяет существование GUID для сущностей в списке <see cref="erknmEntityList"/> и возвращает его
        /// Если в списке отсутствует , генерирует GUID и добавляет запись в <see cref="erknmEntityList"/>
        /// </summary>
        /// <param name="entities">Список сущностей, для которых генерируем одинаковый GUID</param>
        /// <param name="type">Тип сущности</param>
        /// <param name="fieldName">Дополнительный параметр для гуидов отличных от ErknmGuid</param>
        /// <returns>Возвращает GUID</returns>
        protected string GetErknmGuid(IEnumerable<IHaveId> entities, Type type, string fieldName)
        {
            var entityIds = entities.Select(x => x.Id).ToArray();
            var guid = this.FindEntity(type, fieldName, entityIds)?.Guid;

            return string.IsNullOrEmpty(guid) ? this.AddToErknmEntityList(entities, type, string.Empty, fieldName) : guid;
        }
        
        /// <summary>
        /// Добавляет сущности в список <see cref="erknmEntityList"/>
        /// </summary>
        /// <param name="entities">Список сущностей</param>
        /// <param name="type">Тип сущности</param>
        /// <param name="guid">Guid</param>
        /// <param name="fieldName">Дополнительный параметр для гуидов отличных от ErknmGuid</param>
        /// <returns>Возвращает Guid</returns>
        protected string AddToErknmEntityList(IEnumerable<IHaveId> entities, Type type, string guid, string fieldName = "ErknmGuid")
        {
            guid = string.IsNullOrEmpty(guid) ? Guid.NewGuid().ToString().ToUpper() : guid;

            foreach (var entity in entities)
            {
                var erknmEntity = this.FindEntity(type, fieldName, entity.Id) ?? new ErknmEntity
                {
                    EntityId = entity.Id,
                    EntityType = type.FullName,
                    AssemblyType = type.Assembly.ToString(),
                    Guid = guid,
                    FieldName = fieldName
                };

                this.erknmEntityList.Add(erknmEntity);
            }

            return guid;
        }
        
        /// <summary>
        /// Проверяем есть ли значение в поле ErknmGuid
        /// </summary>
        /// <param name="entity">Сущность</param>
        /// <returns>true если содержит значение, иначе false</returns>
        protected bool HasErknmGuid(IEntityUsedInErknm entity) => !string.IsNullOrEmpty(entity.ErknmGuid);
        
        /// <summary>
        /// Сохраняет сущности ErknmEntity из списка <see cref="erknmEntityList"/>
        /// </summary>
        protected void SaveErknmEntities()
        {
            var taskTriggerDomain = this.Container.ResolveDomain<RisTaskTrigger>();
            var erknmEntityDomain = this.Container.ResolveDomain<ErknmEntity>();

            using (this.Container.Using(taskTriggerDomain, erknmEntityDomain))
            {
                var task = taskTriggerDomain.FirstOrDefault(x => x.Trigger.Id == this.StorableTrigger.Id)?.Task;

                foreach (var erknmEntity in this.erknmEntityList)
                {
                    erknmEntity.Task = task;
                    erknmEntityDomain.Save(erknmEntity);
                }
            }
        }

        /// <summary>
        /// Поиск и получение сущности из списка по параметрам  <see cref="erknmEntityList"/>
        /// </summary>
        /// <param name="type">Тип сущности</param>
        /// <param name="fieldName">Имя поля</param>
        /// <param name="entityIds">Идентификаторы сущностей</param>
        /// <returns></returns>
        private ErknmEntity FindEntity(Type type, string fieldName, params long[] entityIds) =>
            this.erknmEntityList.FirstOrDefault(f =>
                entityIds.Contains(f.EntityId) &&
                f.EntityType == type.FullName &&
                f.FieldName == fieldName);
    }
}