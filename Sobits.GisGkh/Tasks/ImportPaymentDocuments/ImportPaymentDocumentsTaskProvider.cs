using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Bars.B4.Utils;
using Bars.Gkh.Entities;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;
using Castle.Windsor;
using global::Quartz.Util;
using Sobits.GisGkh.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Sobits.GisGkh.Tasks.ProcessGisGkhAnswers
{
    using Castle.Core;

    using Fasterflect;

    /// <summary>
    /// Провайдер задачи на массовые запросы получения информации об организациях-абонентах из ГИС ЖКХ
    /// </summary>
    public class ImportPaymentDocumentsTaskProvider : ITaskProvider
    {
        #region Constants

        const short numDocs = 300;

        #endregion

        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ImportPaymentDocumentsTaskProvider";

        #endregion

        #region Constructors

        public ImportPaymentDocumentsTaskProvider(IWindsorContainer container)
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
            //baseParams.Params.Add("reqId", req.Id);
            //baseParams.Params.Add("chargePeriodId", chargePeriodId);
            //baseParams.Params.Add("roId", houseId);
            //baseParams.Params.Add("rewrite", rewriteFlag);

            var GisGkhRequestsDomain = this.container.ResolveDomain<GisGkhRequests>();
            var ChargePeriodDomain = this.container.ResolveDomain<ChargePeriod>();
            var RealityObjectDomain = this.container.ResolveDomain<RealityObject>();
            var BasePersonalAccountDomain = this.container.ResolveDomain<BasePersonalAccount>();
            var GisGkhPayDocDomain = this.container.ResolveDomain<GisGkhPayDoc>();
            var AccountPaymentInfoSnapshotDomain = this.container.ResolveDomain<AccountPaymentInfoSnapshot>();
            var PersAccGroupRelationRepo = this.container.Resolve<IRepository<PersAccGroupRelation>>();

            long reqId = baseParams.Params.GetAs<long>("reqId");
            string chargePeriodId = baseParams.Params.GetAs<string>("chargePeriodId");
            long? roId = baseParams.Params.GetAs<long?>("roId");
            bool rewrite = baseParams.Params.GetAs<bool>("rewrite");

            var chargePeriod = ChargePeriodDomain.Get(long.Parse(chargePeriodId));
            if (chargePeriod.IsClosing)
            {
                throw new Exception("Ошибка: Выбранный период в стадии закрытия");
            }
            if (!chargePeriod.IsClosed)
            {
                throw new Exception("Ошибка: Выбранный период не закрыт");
            }

            // ЛС в группе "Не требуют выгрузки в ГИС ЖКХ"
            List<long> notNeedGis = PersAccGroupRelationRepo.GetAll()
                .Where(x => x.Group.Name == "Не требуют выгрузки в ГИС ЖКХ")
                .Select(x => x.PersonalAccount.Id).ToList();

            List<long> accsHavePayDoc = new List<long>();
            if (!rewrite)
            {
                // список ЛС, по которым выгружены начисления в данном периоде
                accsHavePayDoc = GisGkhPayDocDomain.GetAll()
                .Where(x => x.Period == chargePeriod)
                .WhereIf(roId != null, x => x.Account.Room.RealityObject.Id == roId)
                .Select(x => x.Account.Id).ToList();
            }

            // список сопоставленных ЛС в начальном статусе без выгруженных начислений
            // за исключением группы не требующих выгрузки
            List<long> gisAccs = BasePersonalAccountDomain.GetAll()
                .Where(x => x.State.StartState)
                .Where(x => x.GisGkhGuid != null && x.GisGkhGuid != "")
                //.WhereIf(!rewrite, x =>
                //    !accsHavePayDoc.Contains(x.Id))
                //.WhereIf(!rewrite, x =>
                //!GisGkhPayDocDomain.GetAll()
                //.Where(y => y.Period == chargePeriod)
                //.Where(y => y.Account == x).Any())
                .WhereIf(roId != null, x => x.Room.RealityObject.Id == roId)
                .Select(x => x.Id).ToList().ToList();

            if (gisAccs.Count == 0)
            {
                throw new Exception("Не найдено лс для начислений" + roId + " " + rewrite);
            }

            //// по каким ЛС выгружать
            //List<long> needAccs = AccountPaymentInfoSnapshotDomain.GetAll()
            //        .Where(x => x.Snapshot.Period == chargePeriod)
            //        .Where(x => gisAccs.Contains(x.AccountId))
            //        .Select(x => x.AccountId
            //        )
            //        .ToList();

            //Dictionary<long, string> needData = AccountPaymentInfoSnapshotDomain.GetAll()
            //        .Where(x => x.Snapshot.Period == chargePeriod)
            //        .Where(x => gisAccs.Contains(x.AccountId))
            //        .Select(x => new
            //        {
            //            x.AccountId,
            //            x.Snapshot.Data
            //        }
            //        )
            //        .ToDictionary(x => x.AccountId, x => x.Data);

            //List<string> needData = AccountPaymentInfoSnapshotDomain.GetAll()
            //        .Where(x => x.Snapshot.Period == chargePeriod)
            //        .Where(x => gisAccs.Contains(x.AccountId))
            //        .Select(x =>
            //            // берёт AccountInfo из AccountPaymentInfoSnapshot и InvoiceInfo из PaymentDocumentSnapshot
            //            x.Data != null ? $"{x.Snapshot.Data}&{x.Data}" : x.Snapshot.Data
            //        )
            //        .ToList();

            List<Pair<string, string>> needData = AccountPaymentInfoSnapshotDomain.GetAll()
                    .Where(x => x.Snapshot.Period == chargePeriod)
                    .Where(x => gisAccs.Contains(x.AccountId))
                    .Select(x => new Pair<string, string>(x.Snapshot.Data, x.Data == null? null:x.Data)
                    // берёт AccountInfo из AccountPaymentInfoSnapshot и InvoiceInfo из PaymentDocumentSnapshot
                    //x.Data != null ? $"{x.Snapshot.Data}&{x.Data}" : x.Snapshot.Data
                    )
                    .ToList();

            var count = needData.Count();

            if (count == 0)
            {
                throw new Exception("Не найдено начислений для выгрузки " + roId + " период " + chargePeriodId);
            }

            //var count = AccountPaymentInfoSnapshotDomain.GetAll()
            //        .Where(x => x.Snapshot.Period == chargePeriod)
            //        .Where(x => gisAccs.Contains(x.AccountId))
            //        .Count();

            var descrs = new List<TaskDescriptor>();

            ProcessByPortion(
                done =>
                {
                    //var args = DynamicDictionary.Create();
                    var args = baseParams.Params.DeepClone();

                    //args.SetValue("take", numDocs);
                    args.SetValue("first", done == 0 ? true : false);
                    args.SetValue("snaps", needData.Skip(done).Take(numDocs).ToArray());

                    descrs.Add(
                        new TaskDescriptor(
                           "Формирование запросов на выгрузку начислений в ГИС ЖКХ",
                           ImportPaymentDocumentsTaskExecutor.Id,
                            new BaseParams { Params = args })
                        );
                },
                count,
                numDocs);

            var result = new CreateTasksResult(descrs.ToArray());

            return result;

            //var @params = baseParams.Params.DeepClone();

            //return new CreateTasksResult(
            //    new TaskDescriptor[] {
            //        new TaskDescriptor(
            //            "Формирование запросов на выгрузку начислений в ГИС ЖКХ",
            //            ImportPaymentDocumentsTaskExecutor.Id,
            //               new BaseParams { Params = @params })
            //    }
            //);
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
