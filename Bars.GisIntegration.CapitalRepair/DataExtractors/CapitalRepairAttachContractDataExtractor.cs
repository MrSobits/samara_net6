namespace Bars.GisIntegration.CapitalRepair.DataExtractors
{
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.CapitalRepair;
    using Bars.GisIntegration.Base.Service;
    using Bars.GkhCr.Entities;
    using Gkh.Quartz.Scheduler.Log;

    /// <summary>
    /// Экстрактор данных файлов договора
    /// </summary>
    public class CapitalRepairAttachContractDataExtractor : BaseDataExtractor<RisCrAttachContract, BuildContract>
    {
        private Dictionary<BuildContract, RisCrContract> contracts;

        /// <summary>
        /// Получить сущности сторонней системы
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы</returns>
        public override List<BuildContract> GetExternalEntities(DynamicDictionary parameters)
        {
            var risCrContracts = parameters.GetAs<IEnumerable<RisCrContract>>("risCrContracts");
            var buildContractTypeWorkDomain = this.Container.ResolveDomain<BuildContractTypeWork>();
            var contractCrDomain = this.Container.ResolveDomain<ContractCr>();

            try
            {
                this.contracts =
                    (from risContract in risCrContracts
                        join gkhContract in buildContractTypeWorkDomain.GetAll().Where(x=>x.BuildContract.DocumentFile!=null)
                            on risContract.ExternalSystemEntityId equals gkhContract.Id
                        select new {risContract, gkhContract}).ToDictionary(x => x.gkhContract.BuildContract, x => x.risContract);

                return this.contracts.Keys.ToList();
            }
            finally
            {
                this.Container.Release(contractCrDomain);
            }
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="externalEntity">Сущность внешней системы</param>
        /// <param name="risEntity">Ris сущность</param>
        protected override void UpdateRisEntity(BuildContract externalEntity, RisCrAttachContract risEntity)
        {
            var fileUploadService = this.Container.Resolve<IAttachmentService>();

            risEntity.Contract = this.contracts[externalEntity];
            risEntity.ExternalSystemName = "gkh";
            risEntity.ExternalSystemEntityId = externalEntity.Id;
            try
            {
                risEntity.Attachment = fileUploadService.CreateAttachment(
                    externalEntity.DocumentFile,
                    externalEntity.Description);

            }
            catch (FileNotFoundException)
            {
                risEntity.Attachment = null;
                this.Log.Add(new BaseLogRecord(MessageType.Info, "Файл договора " + externalEntity.Description + " не найден"));
            }
            finally
            {
                this.Container.Release(fileUploadService);
            }
        }
    }
}