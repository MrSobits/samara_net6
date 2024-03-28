using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.Entities;

using Castle.Windsor;

using global::Quartz.Util;
using Sobits.GisGkh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    using Fasterflect;

    /// <summary>
    /// Провайдер задачи на массовые запросы выгрузки начислений ГИС ЖКХ
    /// </summary>
    public class ImportBillsAccMassTaskProvider : ITaskProvider
    {
        #region Constants
        const short numDocs = 100;
        #endregion

        #region Fields
        private readonly IWindsorContainer container;
        #endregion

        #region Properties
        public string TaskCode => "ImportBillsAccMassTaskProvider";

        
        #endregion

        #region Constructors
        public ImportBillsAccMassTaskProvider(IWindsorContainer container)
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
            var RealityObjectDomain = this.container.ResolveDomain<RealityObject>();
            var GisGkhPayDocDomain = this.container.ResolveDomain<GisGkhPayDoc>();
            var ChargePeriodDomain = this.container.ResolveDomain<ChargePeriod>();

            var period = ChargePeriodDomain.GetAll()
               .Where(x => x.IsClosed).OrderByDescending(x => x.Id).FirstOrDefault();

            // ЛС в группе "Не требуют выгрузки в ГИС ЖКХ"
            List<long> needGis = BasePersonalAccountDomain.GetAll()
                .Where(x => x.State.StartState)
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                .Select(x => x.Room.RealityObject.Id).Distinct()
                .ToList();

            var notNeedGis = GisGkhPayDocDomain.GetAll()
            .Where(x => x.Period == period)
            .Select(x => x.Account.Room.RealityObject.Id).Distinct().ToList();

            var roNeedExporttoGis = needGis.Except(notNeedGis).ToList();

            var count = roNeedExporttoGis.Count();

            var descrs = new List<TaskDescriptor>();

            ProcessByPortion(
                done =>
                {
                    var args = baseParams.Params.DeepClone();

                    args.SetValue("lsIds", roNeedExporttoGis.Skip(done).Take(numDocs).ToArray());

                    descrs.Add(
                        new TaskDescriptor(
                            "Формирование запросов на выгрузку начислений в ГИС ЖКХ",
                            ImportBillsAccMassTaskExecutor.Id,
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