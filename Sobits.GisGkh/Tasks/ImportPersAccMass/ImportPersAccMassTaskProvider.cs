using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.RegOperator.Entities;

using Castle.Windsor;

using global::Quartz.Util;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    using Fasterflect;

    /// <summary>
    /// Провайдер задачи на массовые запросы получения информации об организациях-абонентах из ГИС ЖКХ
    /// </summary>
    public class ImportPersAccMassTaskProvider : ITaskProvider
    {
        #region Constants
        const short numDocs = 100;
        #endregion

        #region Fields
        private readonly IWindsorContainer container;
        #endregion

        #region Properties
        public string TaskCode => "ImportPersAccMassTaskProvider";

        
        #endregion

        #region Constructors
        public ImportPersAccMassTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }
        #endregion

        #region Public methods
        /// <summary>
        /// Создает задачу на созданеие массовых запросов на получения информации об организациях-абонентах из ГИС ЖКХ
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var BasePersonalAccountDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var PersAccGroupRelationRepo = this.container.Resolve<IRepository<PersAccGroupRelation>>();


            // ЛС в группе "Не требуют выгрузки в ГИС ЖКХ"
            List<long> notNeedGis = PersAccGroupRelationRepo.GetAll()
                .Where(x => x.Group.Name == "Не требуют выгрузки в ГИС ЖКХ")
                .Select(x => x.PersonalAccount.Id)
                .ToList();

            List<long> lsNeedExporttoGis = BasePersonalAccountDomain.GetAll()
                .Where(x => x.State.StartState)
                .Where(x => x.Room.GisGkhPremisesGUID != null && x.Room.GisGkhPremisesGUID != "")
                .Where(x => x.GisGkhGuid == null || x.GisGkhGuid == "")
                .Where(x => !notNeedGis.Contains(x.Id))
                .Select(x => x.Id)
                .ToList();

            var count = lsNeedExporttoGis.Count();

            var descrs = new List<TaskDescriptor>();

            ProcessByPortion(
                done =>
                {
                    var args = baseParams.Params.DeepClone();

                    args.SetValue("lsIds", lsNeedExporttoGis.Skip(done).Take(numDocs).ToArray());

                    descrs.Add(
                        new TaskDescriptor(
                            "Формирование запросов на выгрузку ЛС в ГИС ЖКХ",
                            ImportPersAccMassTaskExecutor.Id,
                            new BaseParams { Params = args })
                    );
                },
                count,
                numDocs);

            var result = new CreateTasksResult(descrs.ToArray());

            return result;
        }

        public static void ProcessByPortion(Action<int> action, int totalCount, int portion)
        {
            var done = 0;
            while (done < totalCount)
            {
                action(done);

                done += portion;
            }
        }
        #endregion
    }
}