namespace Bars.Gkh.DomainService.Administration.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.FormatDataExport;
    using Bars.Gkh.Enums.Administration.FormatDataExport;

    using Castle.Windsor;

    using NLog;

    public class FormatDataExportEntityInfoService : IFormatDataExportEntityInfoService
    {
        public IWindsorContainer Container { get; set; }

        private static readonly Logger Log = LogManager.GetLogger(typeof(FormatDataExportEntityInfoService).FullName);

        /// <inheritdoc />
        public IDataResult UpdateExportEntitiesInfo(BaseParams baseParams)
        {
            var data = baseParams.Params.GetAs<ICollection<DynamicDictionary>>("data");

            if (data == null || !data.Any())
            {
                Log.Error(new Exception("data == null || !data.Any()"), "Некорректные входные данные");
                return new BaseDataResult(false, "Некорректные входные данные");
            }

            var entityDomainService = this.Container.ResolveDomain<FormatDataExportEntity>();
            var infoDomainService = this.Container.ResolveDomain<FormatDataExportInfo>();

            using (this.Container.Using(entityDomainService, infoDomainService))
            {
                var entities = new List<FormatDataExportEntity>();

                foreach (var dict in data)
                {
                    var entityType = dict.GetAs<int>("EntityType");
                    if (!Enum.IsDefined(typeof(EntityType), entityType))
                    {
                        continue;
                    }

                    var entityId = dict.GetAs<string>("EntityId");
                    var externalGuid = dict.GetAs<Guid?>("ExternalGuid");
                    var exportDate = dict.GetAs<DateTime?>("ExportDate");
                    var exportState = dict.GetAs<FormatDataExportEntityState>("ExportState");
                    var errorMessage = dict.GetAs<string>("ErrorMessage");

                    var entity = new FormatDataExportEntity
                    {
                        EntityId = entityId,
                        ExternalGuid = externalGuid,
                        EntityType = (EntityType) entityType,
                        ExportDate = exportDate,
                        ExportEntityState = exportState,
                        ErrorMessage = errorMessage
                    };

                    entities.Add(entity);
                }

                if (!entities.Any())
                {
                    Log.Error(new Exception("!entities.Any()"), "Некорректные входные данные");
                    return new BaseDataResult(false, "Некорректные входные данные");
                }

                var info = new FormatDataExportInfo
                {
                    State = entities.All(x => x.ExportEntityState == FormatDataExportEntityState.Success) 
                        ? FormatDataExportState.Success 
                        : FormatDataExportState.Failure,
                    LoadDate = DateTime.Now,
                    ObjectType = entities.Any(x => x.EntityType == EntityType.CrProgramHousePlanWork)
                        ? FormatDataExportObjectType.CrProgramWorks
                        : FormatDataExportObjectType.CrProgram
                };

                infoDomainService.Save(info);

                entities.ForEach(x =>
                {
                    x.FormatDataExportInfo = info;
                    entityDomainService.Save(x);
                });
            }

            return new BaseDataResult();
        }
    }
}
