namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;
    using Dapper;
    using Entities.CalcVerification;
    using Intf;
    using Npgsql;
    using Remotion.Linq.Utilities;

    using Component = Castle.MicroKernel.Registration.Component;

    public class InstanceHandler : IDisposable
    {
        public readonly IWindsorContainer Container;
        public IDbConnection ConnectionCHD;
        public IDbConnection ConnectionMO;
        public readonly CalcVerificationParams Params;

        private InstanceHandler()
        {
        }

        public InstanceHandler(BaseParams baseParam)
        {
            var chdConnectionString = baseParam.Params.GetAs<string>("ChdConnectionString");
            var moConnectionString = baseParam.Params.GetAs<string>("MoConnectionString");
            Params = baseParam.Params.GetAs<CalcVerificationParams>("CalcVerificationParams");

            Container = new WindsorContainer();
            ConnectionCHD = new NpgsqlConnection(chdConnectionString);
            ConnectionMO = new NpgsqlConnection(moConnectionString);
            OpenConnection();
            GetHouseId();

            Container.Register(Component.For<IWindsorContainer>().UsingFactoryMethod(() => Container));
            Container.Register(Component.For<ITransfer>().ImplementedBy<TransferHandler>());
            Container.Register(Component.For<ICalcTenant>().ImplementedBy<CalcTenantHandler>());
            Container.Register(Component.For<ICalcServices>().ImplementedBy<CalcServicesHandler>());
            Container.Register(Component.For<ICalcConsumptions>().ImplementedBy<CalcConsumptionsHandler>());
            Container.Register(Component.For<ICalcCharge>().ImplementedBy<CalcChargeHandler>());
            Container.Register(Component.For<INormative>().ImplementedBy<MONormativeHandler>());
            Container.Register(Component.For<ITariff>().ImplementedBy<FakeTariffHandler>());
            Container.Register(Component.For<ICalcPreparation>().ImplementedBy<CalcPreparationHandler>());
            Container.Register(Component.For<BillingInstrumentary>().ImplementedBy<BillingInstrumentary>());
            Container.Register(Component.For<TempTablesLifeTime>().UsingFactoryMethod(
                    () => new TempTablesLifeTime(Container.Resolve<BillingInstrumentary>())));

            Container.Register(Component.For<CalcVerificationParams>().UsingFactoryMethod(() => Params));
            Container.Register(Component.For<IDbConnection>().UsingFactoryMethod(() => ConnectionMO).IsDefault(),
                Component.For<IDbConnection>().UsingFactoryMethod(() => ConnectionCHD).Named("ConnectionCHD"));
        }

       

        private void OpenConnection()
        {
            if (ConnectionCHD.State != ConnectionState.Open)
            {
                ConnectionCHD.Open();
            }
            if (ConnectionMO.State != ConnectionState.Open)
            {
                ConnectionMO.Open();
            }
        }

   
        public IDataResult Run()
        {
            try
            {
                #region Создание и регистрация класса с параметрами(в него включен paramcalc)
                Points.PointList = new List<_Point>();
                Params.ParamCalc = new CalcTypes.ParamCalc((int)Convert.ToInt64(Params.PersonalAccountId), Convert.ToInt32(Params.HouseId),
                    Params.Pref, Params.DateCalc.Value.Year, Params.DateCalc.Value.Month,
                      Params.DateCalc.Value.Year, Params.DateCalc.Value.Month);

                #endregion

                var paramcalc = Params.ParamCalc;

                using (Container.Resolve<TempTablesLifeTime>())
                {
                    var calcPreparation = Container.Resolve<ICalcPreparation>();
                    //подготовили данные
                    calcPreparation.PrepareCalc(ref paramcalc);
                    //получение параметров для лс
                    calcPreparation.LoadTempTablesForMonth(ref paramcalc);
                    //очистка данных
                    calcPreparation.ClearData(ref paramcalc);
                    //посчитали жильцов
                    Container.Resolve<ICalcTenant>().CalcTenant(ref paramcalc);
                    //вытащили начисления УК
                    var TableMOCharges = calcPreparation.GetMOCharges(ref paramcalc);
                    //рассчитали расходы по услугам
                    Container.Resolve<ICalcServices>().CalcServices(TableMOCharges);
                    //рассчитали начисления по услугам
                    Container.Resolve<ICalcCharge>().CalcCharge(ref paramcalc, TableMOCharges);
                    //перенос начислений в ЦХД
                    Container.Resolve<ITransfer>().TransferCharge();
                }
            }
            catch (Exception ex)
            {
                return new BaseDataResult(false, "Во время расчета произошла ошибка");
            }
            finally
            {
                Container.Release(ConnectionMO);
                Container.Release(ConnectionCHD);
            }
            return new BaseDataResult() { Success = true, Message = "Расчет успешно завершен" };
        }

        private void GetHouseId()
        {
            var prmTable = string.Format("{0}_data.prm_4", Params.Pref);
            var sql = string.Format("(select cast(nzp as integer) from {0} " +
                                    " where cast(val_prm as bigint) = {1} and is_actual <> 100 and nzp_prm = 890 and '{2}' between dat_s and dat_po) ",
                prmTable, Params.BillingHouseCode, Params.DateCalc.Value.ToShortDateString());
            Params.HouseId = ConnectionMO.ExecuteScalar<int>(sql);
            if (Params.HouseId == 0)
            {
                throw new ArgumentException("Не определен код дома!");
            }
        }

        public void Dispose()
        {
            Container.Dispose();
            ConnectionCHD.Close();
            ConnectionCHD = null;
            ConnectionMO.Close();
            ConnectionMO = null;
        }
    }
}
