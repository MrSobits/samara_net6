namespace Bars.Gkh.Regions.Perm.DomainService
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    using Castle.Windsor;
    using Domain;

    /// <summary>
    /// Сервис для синхронизации реестров жилых и аварийных домов
    /// </summary>
    public class EmergencyObjectSyncService : IEmergencyObjectSyncService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<EmergencyObject> EmergencyObjectDomain { get; set; }

        public IDomainService<EntityLogLight> LogLightDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IFileManager FileManager { get; set; }

        public IUserIdentity UserIdentity { get; set; }

        public IRepository<User> UserRepo { get; set; }

        [ThreadStatic]
        private static bool synchronized;

        /// <inheritdoc />
        public void UpdateConditionHouse(BaseParams baseParams)
        {
            if (EmergencyObjectSyncService.synchronized)
            {
                return;
            }

            var value = baseParams.Params.GetAs<object>("value", ignoreCase: true);
            var id = baseParams.Params.GetAs<long>("entityId", ignoreCase: true);
            var factDate = baseParams.Params.GetAs<DateTime>("factDate", ignoreCase: true);

            // не дает обновить версионируемое поле при создании entityLogLight, 
            // если дата начала действия значения больше текущей
            var deferredUpdate = baseParams.Params.GetAs<bool>("deferredUpdate", false, ignoreCase: true);

            if (id == 0)
            {
                return;
            }
            
            this.Container.InTransaction(() =>
            {
                var file = baseParams.Files.FirstOrDefault();
                FileInfo fileInfo = null;
                if (file.Value != null)
                {
                    fileInfo = this.FileManager.SaveFile(file.Value);
                }

                var entityLogLight = this.CreateEntityLogLight(id, value.ToStr(), factDate);
                entityLogLight.Document = fileInfo;
                this.LogLightDomain.Save(entityLogLight);

                if (!deferredUpdate || factDate.Date <= DateTime.Now.Date)
                {
                    try
                    {
                        var realityObject = this.RealityObjectDomain.Get(id);
                        ConditionHouse enumValue;
                        if (Enum.TryParse(value.ToString(), out enumValue))
                        {
                            EmergencyObjectSyncService.synchronized = true;

                            realityObject.ConditionHouse = enumValue;

                            this.SyncEmergencyObjectConditionInternal(realityObject, entityLogLight);
                            this.RealityObjectDomain.Update(realityObject);
                        }
                    }
                    finally
                    {
                        EmergencyObjectSyncService.synchronized = false;
                    }
                }
            });
        }

        /// <inheritdoc />
        public void SyncEmergencyObject(RealityObject realityObject)
        {
            if (EmergencyObjectSyncService.synchronized)
            {
                return;
            }

            try
            {
                EmergencyObjectSyncService.synchronized = true;

                var emergencyObject = this.EmergencyObjectDomain.GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);

                //существующий аварийный дом всегда синхронизируем
                if (emergencyObject != null)
                {
                    emergencyObject.CadastralNumber = realityObject.CadastreNumber;
                    emergencyObject.FactDemolitionDate = realityObject.DateDemolition;

                    this.EmergencyObjectDomain.Update(emergencyObject);
                }
                else
                {
                    //новый аварийный дом создаем, только если жилой дом в статусе "Аварийный"
                    if (realityObject.ConditionHouse == ConditionHouse.Emergency)
                    {
                        var log = this.LogLightDomain.GetAll()
                            .Where(x => x.EntityId == realityObject.Id)
                            .Where(x => x.ClassName == "RealityObject" && x.PropertyName == "ConditionHouse")
                            .Where(x => x.DateActualChange.Date <= DateTime.Now.Date)
                            .OrderByDescending(x => x.DateApplied)
                            .FirstOrDefault();

                        if (log != null)
                        {
                            emergencyObject = this.CreateNewEmergencyObject(realityObject, log);
                            this.EmergencyObjectDomain.Save(emergencyObject);
                        }
                    }
                }
            }
            finally
            {
                EmergencyObjectSyncService.synchronized = false;
            }
        }

        /// <inheritdoc />
        public void SyncEmergencyObjectCondition(RealityObject realityObject, EntityLogLight logLight)
        {
            if (EmergencyObjectSyncService.synchronized)
            {
                return;
            }

            try
            {
                EmergencyObjectSyncService.synchronized = true;
                this.SyncEmergencyObjectConditionInternal(realityObject, logLight);
            }
            finally
            {
                EmergencyObjectSyncService.synchronized = false;
            }
        } 
        
        /// <inheritdoc />
        public void SyncRealityObject(EmergencyObject emergencyObject)
        {
            if (EmergencyObjectSyncService.synchronized)
            {
                return;
            }

            try
            {
                EmergencyObjectSyncService.synchronized = true;

                var realityObject = emergencyObject.RealityObject;

                if (realityObject.ConditionHouse != emergencyObject.ConditionHouse)
                {
                    realityObject.ConditionHouse = emergencyObject.ConditionHouse;

                    var entityLogLight = this.CreateEntityLogLight(realityObject.Id, ((int) emergencyObject.ConditionHouse).ToString(), DateTime.Now.Date);

                    this.LogLightDomain.Save(entityLogLight);
                }

                realityObject.DateDemolition = emergencyObject.FactDemolitionDate;
                realityObject.ResidentsEvicted = emergencyObject.FactResettlementDate.HasValue
                    && emergencyObject.FactResettlementDate != DateTime.MinValue;

                this.RealityObjectDomain.Update(realityObject);
            }
            finally
            {
                EmergencyObjectSyncService.synchronized = false;
            }
        }

        /// <inheritdoc />
        public void QuiteSyncEmergencyObject(EmergencyObject emergencyObject)
        {
            if (EmergencyObjectSyncService.synchronized)
            {
                return;
            }
           
            var realityObject = emergencyObject.RealityObject;
            emergencyObject.ConditionHouse = realityObject.ConditionHouse;

            var log = this.LogLightDomain.GetAll()
                    .Where(x => x.EntityId == realityObject.Id)
                    .Where(x => x.ClassName == "RealityObject" && x.PropertyName == "ConditionHouse")
                    .Where(x => x.DateActualChange.Date <= DateTime.Now.Date)
                    .OrderByDescending(x => x.DateApplied)
                    .FirstOrDefault();

            if (log != null)
            {
                emergencyObject.EmergencyDocumentName = log.Document?.Name;
                emergencyObject.EmergencyDocumentDate = log.DateActualChange;
                emergencyObject.EmergencyFileInfo = log.Document;              
            }          

            emergencyObject.CadastralNumber = realityObject.CadastreNumber;
            emergencyObject.FactDemolitionDate = realityObject.DateDemolition;
        }

        private void SyncEmergencyObjectConditionInternal(RealityObject realityObject, EntityLogLight logLight)
        {
            var emergencyObject = this.EmergencyObjectDomain.GetAll()
                    .FirstOrDefault(x => x.RealityObject.Id == realityObject.Id);

            //существующий аварийный дом всегда синхронизируем
            if (emergencyObject != null)
            {
                emergencyObject.ConditionHouse = realityObject.ConditionHouse;
                emergencyObject.EmergencyDocumentName = logLight.Document.Name;
                emergencyObject.EmergencyDocumentDate = logLight.DateActualChange;
                emergencyObject.EmergencyFileInfo = logLight.Document;

                emergencyObject.CadastralNumber = realityObject.CadastreNumber;
                emergencyObject.FactDemolitionDate = realityObject.DateDemolition;

                this.EmergencyObjectDomain.Update(emergencyObject);
            }
            else
            {
                //новый аварийный дом создаем, только если жилой дом в статусе "Аварийный"
                if (realityObject.ConditionHouse == ConditionHouse.Emergency)
                {
                    emergencyObject = this.CreateNewEmergencyObject(realityObject, logLight);
                    this.EmergencyObjectDomain.Save(emergencyObject);
                }
            }
        }

        private EmergencyObject CreateNewEmergencyObject(RealityObject realityObject, EntityLogLight log)
        {
            var emergencyObject = new EmergencyObject
            {
                RealityObject = realityObject,
                ConditionHouse = realityObject.ConditionHouse,
                EmergencyDocumentName = log.Document?.Name,
                EmergencyDocumentDate = log.DateActualChange,
                EmergencyFileInfo = log.Document,
                CadastralNumber = realityObject.CadastreNumber,
                FactDemolitionDate = realityObject.DateDemolition
            };

            return emergencyObject;
        }

        private EntityLogLight CreateEntityLogLight(long entityId, string value, DateTime factDate)
        {
            var login = this.UserRepo.Get(this.UserIdentity.UserId).Return(u => u.Login);

            return new EntityLogLight
            {
                ClassName = "RealityObject",
                EntityId = entityId,
                PropertyName = "ConditionHouse",
                PropertyValue = value,
                DateActualChange = factDate,
                DateApplied = DateTime.Now.Date,
                ParameterName = "real_obj_condition_house",
                User = login.IsEmpty() ? "anonymous" : login
            };
        }
    }
}