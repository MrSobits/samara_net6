namespace Bars.GkhGji.DomainService
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Entities;
    using Enums;
    using Newtonsoft.Json;
    using Castle.Windsor;
    using System.Text;
    using Bars.Gkh.Authentification;
    using Bars.B4.Modules.NH.Extentions;
    using Newtonsoft.Json.Linq;

    public class LogEntityHistoryService : ILogEntityHistoryService
    {
        public IWindsorContainer Container { get; set; }
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<EntityChangeLogRecord> EntityChangeLogRecordDomain { get; set; }

        public void UpdateLog(object entity, TypeEntityLogging typeEL, long entityId, Type entityType, Dictionary<string, string> valuesDict, string baseValue)
        {
            try
            {
                var entityOldValuesDomain = this.Container.ResolveDomain<EntityOldValueHistory>();

                if (!string.IsNullOrEmpty(baseValue) && baseValue.Length > 149)
                {
                    baseValue = baseValue.Substring(0, 149);
                }
                else if (string.IsNullOrEmpty(baseValue))
                {
                    baseValue = "";
                }
                if (entity == null)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    var oldEntityRecord = entityOldValuesDomain.GetAll().FirstOrDefault(x => x.TypeEntityLogging == typeEL && x.EntityId == entityId);
                    var oldEntityJson = Encoding.UTF8.GetString(oldEntityRecord.OldValue);
                    var oldEntity = JsonConvert.DeserializeObject(oldEntityJson, entityType);

                    if (thisOperator != null)
                    {
                        LoggingPropertyInfo propInfo = new LoggingPropertyInfo
                        {
                            PropName = "",
                            PropType = "",
                            TypeEntityLogging = typeEL,
                            Login = thisOperator.User.Login,
                            Username = thisOperator.User.Name,
                            OperatorId = thisOperator.Id
                        };
                        CreateDeleteRecord(oldEntity, baseValue, entityId, propInfo);
                    }
                }
                else
                {
                    var oldValue = entityOldValuesDomain.GetAll().FirstOrDefault(x => x.TypeEntityLogging == typeEL && x.EntityId == entityId);
                    if (oldValue == null)
                    {
                        entityOldValuesDomain.Save(new EntityOldValueHistory
                        {
                            EntityId = entityId,
                            TypeEntityLogging = typeEL,
                            OldValue = GetJsonBytea(entity)
                        });
                        Operator thisOperator = UserManager.GetActiveOperator();
                        if (thisOperator != null)
                        {
                            LoggingPropertyInfo propInfo = new LoggingPropertyInfo
                            {
                                PropName = "",
                                PropType = "",
                                TypeEntityLogging = typeEL,
                                Login = thisOperator.User.Login,
                                Username = thisOperator.User.Name,
                                OperatorId = thisOperator.Id
                            };
                            CreateCreateRecord(entity, baseValue, entityId, propInfo);
                        }
                    }
                    else
                    {
                        var entityOldJson = Encoding.UTF8.GetString(oldValue.OldValue);
                        try
                        {
                            var entityOld = JsonConvert.DeserializeObject(entityOldJson, entityType.BaseType);
                            EquateValues(entity, entityOld, typeEL, valuesDict, baseValue, entityId);
                            oldValue.OldValue = GetJsonBytea(entity);
                            entityOldValuesDomain.Update(oldValue);
                        }
                        catch (Exception)
                        {

                        }
                    }
                }
            }
            catch { }

        }

        public virtual IDataResult GetAppealHistory(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var entityOldValuesDomain = Container.ResolveDomain<EntityOldValueHistory>();
            var appealCitsId = baseParams.Params.GetAs<long>("appealCitizensId");

            //значения енума, связанные с обращениями
            var appealTypeEntityLogging = new List<int>()
            {
                1, 2, 3, 46, 47, 48, 49, 50, 51, 52, 53, 54, 55, 56, 57
            };

            var preData = EntityChangeLogRecordDomain.GetAll()
                .Where(x => appealTypeEntityLogging.Contains((int)x.TypeEntityLogging))
                .Where(x => x.ParrentEntity == appealCitsId || (x.TypeEntityLogging == TypeEntityLogging.AppealCits && x.EntityId == appealCitsId))
                .AsQueryable();

            var data = new List<AppealCitsHistoryInfo>();

            foreach (var record in preData)
            {
                var appealHistoryInfo = new AppealCitsHistoryInfo()
                {
                    Id = record.Id,
                    AuditDate = record.ObjectCreateDate,
                    OperatorLogin = record.OperatorLogin,
                    TypeEntityLogging = record.TypeEntityLogging,
                    OperationType = record.OperationType,
                    OperatorName = record.OperatorName,
                    Wording = StringBuilder(record)
                };

                if (record.OperationType == AppealOperationType.AppealDelete)
                {
                    var oldValue = entityOldValuesDomain.GetAll()
                        .Where(x => x.EntityId == record.EntityId)
                        .FirstOrDefault();

                    if (oldValue != null)
                    {
                        var oldEntityJsonString = Encoding.UTF8.GetString(oldValue.OldValue);
                        JObject oldEntityJson = JObject.Parse(oldEntityJsonString);

                        foreach(var child in oldEntityJson.Children())
                        {
                            var childString = child.First().ToString();
                            try 
                            {
                                var appealFromJson = JsonConvert.DeserializeObject<AppealCits>(childString);
                                if (appealFromJson?.DateFrom != null && appealFromJson?.DateFrom != DateTime.MinValue)
                                {
                                    oldEntityJson.Remove(((JProperty)child).Name);
                                    var withHtml = oldEntityJson.ToString().Replace("\r", "<br>").Replace("\n", "&nbsp;&nbsp;&nbsp;&nbsp");
                                    appealHistoryInfo.JsonString = withHtml;
                                    break;
                                }
                            }
                            catch
                            {
                            
                            }
                        }
                    }
                }

                data.Add(appealHistoryInfo);
            }

            return new ListDataResult(data.AsQueryable().Filter(loadParams, Container).Order(loadParams).Paging(loadParams), data.Count());
        }

        private void EquateValues(object newEntity, object oldEntity, TypeEntityLogging typeEL, Dictionary<string, string> valuesDict, string baseValue, long entityId)
        {
            Operator thisOperator = UserManager.GetActiveOperator();

            if (thisOperator != null)
            {
                foreach (var item in newEntity.GetType().GetProperties())
                {

                    if (valuesDict.ContainsKey(item.Name))
                    {
                        LoggingPropertyInfo propInfo = new LoggingPropertyInfo
                        {
                            PropName = valuesDict[item.Name],
                            PropType = item.Name,
                            TypeEntityLogging = typeEL,
                            Login = thisOperator.User.Login,
                            Username = thisOperator.User.Name,
                            OperatorId = thisOperator.Id
                        };
                        var vNew = item.GetValue(newEntity, null);
                        var voldProp = oldEntity.GetType().GetProperty(item.Name);
                        var vold = voldProp.GetValue(oldEntity, null);
                        if (vNew is IEntity entity)
                        {
                            if (vold == null && vNew != null)
                            {
                                var vNewObj = entity;
                                CreateUpdateRecord(newEntity, "Пусто", vNewObj.Id.ToString(), propInfo, baseValue, entityId);
                            }
                            else if (vold != null && vNew == null)
                            {
                                var vOldObj = (IEntity)vold;
                                CreateUpdateRecord(newEntity, vOldObj.Id.ToString(), "Пусто", propInfo, baseValue, entityId);
                            }
                            else if (vold != null && vNew != null)
                            {
                                var vNewObj = (PersistentObject)vNew;
                                var vOldObj = (PersistentObject)vold;
                                if (vOldObj.Id != vNewObj.Id)
                                {
                                    CreateUpdateRecord(newEntity, vOldObj.Id.ToString(), vNewObj.Id.ToString(), propInfo, baseValue, entityId);
                                }
                            }
                        }
                        else
                        {
                            var newLimitedValue = vNew == null ? string.Empty : vNew.ToString().Substring(0, Math.Min(150, vNew.ToString().Length));
                            var oldLimitedValue = vold == null ? string.Empty : vold.ToString().Substring(0, Math.Min(150, vold.ToString().Length));
                            if (newLimitedValue != oldLimitedValue)
                            {
                                CreateUpdateRecord(newEntity, oldLimitedValue, newLimitedValue, propInfo, baseValue, entityId);
                            }
                        }

                    }

                }
            }


        }
        private void CreateUpdateRecord(object entity, string oldValue, string newValue, LoggingPropertyInfo propInfo, string baseValue, long entityId)
        {
            var properties = entity.GetType().GetProperties();
            long? parrentEntityId = null;

            foreach (var proprty in properties)
            {
                if (proprty.PropertyType.Name == "AppealCits")
                {
                    var parrentEntity = proprty.GetValue(entity, null);
                    parrentEntityId = parrentEntity?.GetType().GetProperty("Id").GetValue(parrentEntity, null) as long?;
                    break;
                }

                if (proprty.PropertyType.BaseType.Name == "DocumentGji")
                {
                    var parrentEntity = proprty.GetValue(entity, null);
                    parrentEntityId = parrentEntity?.GetType().GetProperty("Id").GetValue(parrentEntity, null) as long?;
                }
            }

            this.Container.InTransaction(() =>
            {
                EntityChangeLogRecordDomain.Save(new EntityChangeLogRecord
                {
                    DocumentValue = baseValue,
                    EntityId = entityId,
                    NewValue = newValue,
                    OldValue = oldValue,
                    OperationType = AppealOperationType.AppealEdit,
                    OperatorId = propInfo.OperatorId,
                    OperatorLogin = propInfo.Login,
                    OperatorName = propInfo.Username,
                    PropertyName = propInfo.PropName,
                    PropertyType = propInfo.PropType,
                    TypeEntityLogging = propInfo.TypeEntityLogging,
                    ParrentEntity = parrentEntityId
                });
            });

        }

        private void CreateCreateRecord(object entity, string baseValue, long entityId, LoggingPropertyInfo propInfo)
        {
            var properties = entity.GetType().GetProperties();
            long? parrentEntityId = null;

            foreach(var proprty in properties)
            {
                if(proprty.PropertyType.Name == "AppealCits")
                {
                    var parrentEntity = proprty.GetValue(entity, null);
                    parrentEntityId = parrentEntity?.GetType().GetProperty("Id").GetValue(parrentEntity, null) as long?;
                    break;
                }

                if (proprty.PropertyType.BaseType.Name == "DocumentGji")
                {
                    var parrentEntity = proprty.GetValue(entity, null);
                    parrentEntityId = parrentEntity?.GetType().GetProperty("Id").GetValue(parrentEntity, null) as long?;
                }
            }

            this.Container.InTransaction(() =>
            {
                EntityChangeLogRecordDomain.Save(new EntityChangeLogRecord
                {
                    DocumentValue = baseValue,
                    EntityId = entityId,
                    OldValue = "",
                    NewValue = "",
                    OperationType = AppealOperationType.AppealCreate,
                    OperatorId = propInfo.OperatorId,
                    OperatorLogin = propInfo.Login,
                    OperatorName = propInfo.Username,
                    PropertyName = "",
                    PropertyType = "",
                    TypeEntityLogging = propInfo.TypeEntityLogging,
                    ParrentEntity = parrentEntityId
                });
            });

        }

        private void CreateDeleteRecord(object entity, string baseValue, long entityId, LoggingPropertyInfo propInfo)
        {
            var properties = entity.GetType().GetProperties();
            long? parrentEntityId = null;

            foreach (var proprty in properties)
            {
                if (proprty.PropertyType.Name == "AppealCits")
                {
                    var parrentEntity = proprty.GetValue(entity, null);
                    parrentEntityId = parrentEntity?.GetType().GetProperty("Id").GetValue(parrentEntity, null) as long?;
                    break;
                }

                if (proprty.PropertyType.BaseType.Name == "DocumentGji")
                {
                    var parrentEntity = proprty.GetValue(entity, null);
                    parrentEntityId = parrentEntity?.GetType().GetProperty("Id").GetValue(parrentEntity, null) as long?;
                }
            }

            this.Container.InTransaction(() =>
            {
                EntityChangeLogRecordDomain.Save(new EntityChangeLogRecord
                {
                    DocumentValue = baseValue,
                    EntityId = entityId,
                    OldValue = "",
                    NewValue = "",
                    OperationType = AppealOperationType.AppealDelete,
                    OperatorId = propInfo.OperatorId,
                    OperatorLogin = propInfo.Login,
                    OperatorName = propInfo.Username,
                    PropertyName = "",
                    PropertyType = "",
                    TypeEntityLogging = propInfo.TypeEntityLogging,
                    ParrentEntity = parrentEntityId
                });
            });

        }

        private static byte[] GetJsonBytea(object entity)
        {
            var jsonValue = JsonConvert.SerializeObject(entity, new JsonSerializerSettings()
            {
                ContractResolver = new NHibernateContractResolver()
            });
            return Encoding.UTF8.GetBytes(jsonValue);
        }

        protected static string StringBuilder(EntityChangeLogRecord record)
        {
            var entityName = record.TypeEntityLogging.GetDisplayName();
            var idString = record.DocumentValue;

            if (record.TypeEntityLogging != TypeEntityLogging.AppealCits)
            {
                entityName = record.TypeEntityLogging.GetDisplayName().Substring(19);
            }

            switch (record.OperationType)
            {
                case AppealOperationType.AppealCreate:
                    {
                        return $"{record.ObjectCreateDate} {record.OperatorName} создал \"{entityName}\", строка идентификации: {idString}";
                    }
                case AppealOperationType.AppealEdit:
                    {
                        return $"{record.ObjectCreateDate} {record.OperatorName} изменил \"{record.PropertyName}\" в \"{entityName}\", строка идентификации: {idString}, старое значение: {record.OldValue ?? "пусто"}, новое значение: {record.NewValue ?? "пусто"}";
                    }
                case AppealOperationType.AppealDelete:
                    {
                        return $"{record.ObjectCreateDate} {record.OperatorName} удалил \"{entityName}\", строка идентификации: {idString}, данные удаленной сущности прилагаются в Json строке";
                    }
                default:
                    {
                        return "";
                    }
            }
        }

        public class LoggingPropertyInfo
        {
            public string PropType { get; set; }
            public string PropName { get; set; }
            public TypeEntityLogging TypeEntityLogging { get; set; }
            public string Login { get; set; }
            public string Username { get; set; }
            public long OperatorId { get; set; }
        }

        protected class AppealCitsHistoryInfo
        {
            public long Id { get; set; }
            public DateTime AuditDate { get; set; }
            public string OperatorLogin { get; set; }
            public TypeEntityLogging TypeEntityLogging { get; set; }
            public AppealOperationType OperationType { get; set; }
            public string OperatorName { get; set; }
            public string Wording { get; set; }
            public string JsonString { get; set; }
        }
    }
}