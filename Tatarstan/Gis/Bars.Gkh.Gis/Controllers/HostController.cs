namespace Bars.Gkh.Gis.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using Microsoft.AspNetCore.Mvc;
    using B4;
    using B4.DataAccess;
    using B4.Utils.Web;

    using Bars.Gkh.Gis.Controllers.Report;
    using Bars.Gkh.Gis.DomainService.BilConnection;
    using Bars.Gkh.Gis.Enum;
    using Bars.Gkh.Gis.KP_legacy;

    using DomainService.CalcVerification.Intf;
    using DomainService.House;
    using Entities.CalcVerification;
    using Entities.Kp50;

    public class HostController : BaseController
    {
        public IBilConnectionService BilConnectionService { get; set; }
        /// <summary>
        /// Расчет лицевого счета
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult Calculate(BaseParams baseParams)
        {
            var personalAccount = baseParams.Params.GetAs<long>("personalAccount");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<int>("month");

            var houseStorage =
                Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId) ??
                Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);

            //дом не найден ни в хранилище КП20, ни от сторонних систем
            if (houseStorage == null)
            {
                return
                    JsFailure(
                        "Для выбранного дома МЖФ не определен дом из системы биллинга (либо не произведено сопоставление).");
            }

            //дом найден в хранилище данных от сторонних систем
            if (houseStorage.DataBankStorage.DataBankId != 0)
            {
                //TODO: реализовать расчет для сторонних систем
                return JsonNetResult.Success;
            }

            //расчет по структуре КП20
            var billingSchemaInfo = (
                from s in Container.Resolve<IRepository<BilDictSchema>>().GetAll()
                join l in Container.Resolve<IRepository<BilHouseCodeStorage>>().GetAll()
                    on s.Id equals l.Schema.Id
                where
                    l.BillingHouseCode ==
                    Convert.ToInt64(Container.Resolve<IRepository<Gkh.Entities.RealityObject>>().Get(realityObjectId).CodeErc)
                select
                    new {s.Id, s.CentralSchemaPrefix, s.LocalSchemaPrefix, s.ConnectionString, l.BillingHouseCode})
                .SingleOrDefault();

            if (billingSchemaInfo == null)
                return
                    JsFailure(
                        "Для выбранного дома МЖФ не определен дом из системы биллинга (либо не произведено сопоставление).");

            //вытащить параметры из baseParams
            baseParams.Params.Add(new KeyValuePair<string, object>("CalcVerificationParams",
                new CalcVerificationParams
                {
                    PersonalAccountId = personalAccount,
                    BillingHouseCode = billingSchemaInfo.BillingHouseCode,
                    CentralPref = billingSchemaInfo.CentralSchemaPrefix,
                    Pref = billingSchemaInfo.LocalSchemaPrefix,
                    DateCalc = new DateTime(year, month, 1),
                    SchemaId = billingSchemaInfo.Id
                }));
            
            baseParams.Params.Add(new KeyValuePair<string, object>("MoConnectionString",
                billingSchemaInfo.ConnectionString));
            baseParams.Params.Add(new KeyValuePair<string, object>("ChdConnectionString",
                BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)));

            var data = Container.Resolve<ICalcVerificationService>().VerificationCalc(baseParams);

            return new JsonNetResult(new
            {
                success = data.Success,
                data = data.Message
            });
        }

        /// <summary>
        /// Расчет дома
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public virtual ActionResult CalculateHouse(BaseParams baseParams)
        {
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<int>("month");

            var houseStorage =
                Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId) ??
                Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);

            //дом не найден
            if (houseStorage == null)
            {
                //дом не найден ни в хранилище КП20, ни от сторонних систем
                return
                    JsFailure(
                        "Для выбранного дома МЖФ не определен дом из системы биллинга (либо не произведено сопоставление).");
            }

            //дом найден в хранилище данных от сторонних систем
            if (houseStorage.DataBankStorage.DataBankId != 0)
            {
                //TODO: реализовать расчет для сторонних систем
                return JsonNetResult.Success;
            }

            //расчет по структуре КП20
            var billingSchemaInfo = (
                from s in Container.Resolve<IRepository<BilDictSchema>>().GetAll()
                join l in Container.Resolve<IRepository<BilHouseCodeStorage>>().GetAll()
                    on s.Id equals l.Schema.Id
                where
                    l.BillingHouseCode ==
                    Convert.ToInt64(Container.Resolve<IRepository<Gkh.Entities.RealityObject>>().Get(realityObjectId).CodeErc)
                select
                    new { s.Id, s.CentralSchemaPrefix, s.LocalSchemaPrefix, s.ConnectionString, l.BillingHouseCode })
                .SingleOrDefault();

            if (billingSchemaInfo == null)
                return
                    JsFailure(
                        "Для выбранного дома МЖФ не определен дом из системы биллинга (либо не произведено сопоставление).");
            
            baseParams.Params.Add(new KeyValuePair<string, object>("CalcVerificationParams", new CalcVerificationParams
            {
                BillingHouseCode = billingSchemaInfo.BillingHouseCode,
                CentralPref = billingSchemaInfo.CentralSchemaPrefix,
                Pref = billingSchemaInfo.LocalSchemaPrefix,
                DateCalc = new DateTime(year, month, 1),
                SchemaId = billingSchemaInfo.Id
            }));
           
            baseParams.Params.Add(new KeyValuePair<string, object>("MoConnectionString", billingSchemaInfo.ConnectionString));
            baseParams.Params.Add(new KeyValuePair<string, object>("ChdConnectionString", this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)));
            var data =  Container.Resolve<ICalcVerificationService>().VerificationCalc(baseParams);

            return new JsonNetResult(new
            {
                success = data.Success,
                data = data.Message
            });
        }

        /// <summary>
        /// Проверочный расчет
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ActionResult Listing(BaseParams baseParams)
        {
            var user = Container.Resolve<IUserIdentity>(); //текущий юзер
            if (user.UserId == 0)
            {
                return JsFailure("Не определен пользователь. Вам необходимо зарегистрироваться в системе");
            }

            var loadParam = baseParams.GetLoadParam();
            var personalAccount = baseParams.Params.GetAs<long>("personalAccountId");
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId");
            var year = baseParams.Params.GetAs<int>("year");
            var month = baseParams.Params.GetAs<int>("month");

            var groupByService = baseParams.Params.GetAs<bool>("groupByService");
            var groupBySupplier = baseParams.Params.GetAs<bool>("groupBySupplier");
            var groupByFormula = baseParams.Params.GetAs<bool>("groupByFormula");
            var showPrev = baseParams.Params.GetAs<bool>("showPrev");
            var showNulls = baseParams.Params.GetAs<bool>("showNulls");

            var groupby = new List<string>
                {
                    string.Format("{0}", (int) TypeAccrualGroupingSettings.ActShowSaldo),
                    //добавим безусловно показ сальдовых сумм
                    string.Format("{0}", (int) TypeAccrualGroupingSettings.ActGroupByMonth) //помесячная группировка
                };

            if (groupByService)
            {
                groupby.Add(string.Format("{0}", (int)TypeAccrualGroupingSettings.ActGroupByService));
            }
            if (groupBySupplier)
            {
                groupby.Add(string.Format("{0}", (int)TypeAccrualGroupingSettings.ActGroupBySupplier));
            }
            if (groupByFormula)
            {
                groupby.Add(string.Format("{0}", (int)TypeAccrualGroupingSettings.ActGroupByFormula));
            }

            var chargeIn = new ChargeProxyIn
                {
                    UserId = user.UserId,
                    PersonalAccountId = personalAccount,

                    Year = year,
                    Month = month,

                    ShowPrev = showPrev,
                    GroupBy = groupby,
                    GetFromCash = false
                };


            try
            {
                var houseStorage =
                 Container.Resolve<IGisHouseService>("GisHouseService").GetHouseStorage(realityObjectId) ??
                 Container.Resolve<IGisHouseService>("KpHouseService").GetHouseStorage(realityObjectId);

                //дом не найден
                if (houseStorage == null)
                {
                    //дом не найден ни в хранилище КП20, ни от сторонних систем
                    return
                        JsFailure(
                            "Для выбранного дома МЖФ не определен дом из системы биллинга (либо не произведено сопоставление).");
                }

                //дом найден в хранилище данных от сторонних систем
                if (houseStorage.DataBankStorage.DataBankId != 0)
                {
                    //TODO: реализовать расчет для сторонних систем
                    return JsonNetResult.Success;
                }

                //расчет по структуре КП20
                var billingSchemaInfo = (
                    from s in Container.Resolve<IRepository<BilDictSchema>>().GetAll()
                    join l in Container.Resolve<IRepository<BilHouseCodeStorage>>().GetAll()
                        on s.Id equals l.Schema.Id
                    where
                        l.BillingHouseCode ==
                        Convert.ToInt64(Container.Resolve<IRepository<Gkh.Entities.RealityObject>>().Get(realityObjectId).CodeErc)
                    select
                        new { s.Id, s.CentralSchemaPrefix, s.LocalSchemaPrefix, s.ConnectionString, l.BillingHouseCode })
                    .SingleOrDefault();

                if (billingSchemaInfo == null)
                    return
                        JsFailure(
                            "Для выбранного дома МЖФ не определен дом из системы биллинга (либо не произведено сопоставление).");
                
                var param = new DeltaParams
                {
                    CentralPref = billingSchemaInfo.CentralSchemaPrefix,
                    Pref = billingSchemaInfo.LocalSchemaPrefix,
                    ConnectionString = this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports),
                    PersonalAccountId = personalAccount,
                    BillingHouseCode = billingSchemaInfo.BillingHouseCode,
                    ShowNulls = showNulls,
                    ActGroupByFormula = groupByFormula,
                    ActGroupByService = groupByService,
                    ActGroupBySupplier = groupBySupplier
                };

                var result = Container.Resolve<ICalcVerificationService>().Delta(chargeIn, param);
                return new JsonNetResult(new { success = true, data = result.Data.AsQueryable().Order(loadParam).ToArray() });
            }
            catch
            {
                return new JsonNetResult(new { success = false, data = new { } });
            }
        }

        //Протокол расчета
        public InlineFileResult GetProtocol(long id, long realityObjectId, int year, int month, int serviceId, int supplierId, bool isGis)
        {
            
            var baseParams = new BaseParams();
            baseParams.Params.Add(new KeyValuePair<string, object>("personalAccountId", id));
            baseParams.Params.Add(new KeyValuePair<string, object>("billingHouseId", Container.Resolve<IRepository<Gkh.Entities.RealityObject>>().Get(realityObjectId).CodeErc));
            baseParams.Params.Add(new KeyValuePair<string, object>("serviceId", serviceId));
            baseParams.Params.Add(new KeyValuePair<string, object>("supplierId", supplierId));
            baseParams.Params.Add(new KeyValuePair<string, object>("isGis", isGis));
            baseParams.Params.Add(new KeyValuePair<string, object>("year", year));
            baseParams.Params.Add(new KeyValuePair<string, object>("month", month));
            baseParams.Params.Add(new KeyValuePair<string, object>("ChdConnectionString", this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)));

            byte[] binaryData = Encoding.UTF8.GetBytes(Container.Resolve<ICalcVerificationService>().GetProtocol(baseParams));

            using (var memStream = new MemoryStream(binaryData.Length))
            {
                memStream.SetLength(binaryData.Length);
                memStream.Write(binaryData, 0, binaryData.Length);
                memStream.Position = 0;

                return new InlineFileResult(memStream.ToArray(), "Protocol.html")
                {
                    ResultCode = ResultCode.Success
                };
            }
        }

        /// <summary>
        /// ДРЕВО расчета
        /// </summary>
        /// <returns></returns>
        public ActionResult GetTree(long id, long realityObjectId, int year, int month, int serviceId, int supplierId, bool isGis)
        {
            
            var baseParams = new BaseParams();
            baseParams.Params.Add(new KeyValuePair<string, object>("personalAccountId", id));
            baseParams.Params.Add(new KeyValuePair<string, object>("billingHouseId", Container.Resolve<IRepository<Gkh.Entities.RealityObject>>().Get(realityObjectId).CodeErc));
            baseParams.Params.Add(new KeyValuePair<string, object>("serviceId", serviceId));
            baseParams.Params.Add(new KeyValuePair<string, object>("supplierId", supplierId));
            baseParams.Params.Add(new KeyValuePair<string, object>("isGis", isGis));
            baseParams.Params.Add(new KeyValuePair<string, object>("year", year));
            baseParams.Params.Add(new KeyValuePair<string, object>("month", month));
            baseParams.Params.Add(new KeyValuePair<string, object>("ChdConnectionString", this.BilConnectionService.GetConnection(ConnectionType.GisConnStringReports)));

            var data = Container.Resolve<ICalcVerificationService>().GetTree(baseParams);

            return new JsonNetResult(new { success = true, data });
        }
    }
}
