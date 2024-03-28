namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;
    using B4;

    using Bars.Gkh.Gis.KP_legacy;

    using Castle.Windsor;
    using Dapper;
    using Entities.CalcVerification;
    using Intf;

    public class TransferHandler : ITransfer
    {
        protected IWindsorContainer Container;
        protected IDbConnection Connection;
        public TransferHandler(IWindsorContainer container, IDbConnection connection)
        {
            Container = container;
            Connection = connection;
        }

        public CalcVerificationParams Params { get; set; }
        /// <summary>
        /// Перенос данных из УК в ЦХД
        /// </summary>
        /// <returns></returns>
        public IDataResult TransferCharge()
        {
            try
            {
                var connectionChd = Container.Resolve<IDbConnection>("ConnectionCHD");
                var month = Params.DateCalc.Value.Month;
                var year = Params.DateCalc.Value.Year;
                var table = string.Format("{0}_charge_{1}.chd_charge_{2}", Params.Pref, year % 1000, month.ToString("00"));
                var tableChd = string.Format("chd_charge_{0}.chd_charge_{1}", year % 1000, month.ToString("00"));

                var supplierTable = string.Format("{0}.supplier", Params.Pref + "_kernel");
                var serviceTable = string.Format("{0}.services", Params.Pref + "_kernel");
                var meashureTable = string.Format("{0}.s_measure", Params.Pref + "_kernel");
                var formulTable = string.Format("{0}.formuls", Params.Pref + "_kernel");
                var kvarTable = string.Format("{0}.kvar", Params.Pref + "_data");
                var wherePersonalAccount = Params.PersonalAccountId > 0
                    ? " and ch.nzp_kvar = " + Params.PersonalAccountId
                    : String.Empty;

                var sql = string.Format(" SELECT DISTINCT ch.*,supp.name_supp AS supplier,serv.service,f.name_frm AS formula,m.measure,m.nzp_measure " +
                                  " FROM {0} ch " +
                                        " LEFT OUTER JOIN {2} supp ON ch.nzp_supp = supp.nzp_supp " +
                                        " LEFT OUTER JOIN {3} serv ON ch.nzp_serv = serv.nzp_serv " +
                                        " LEFT OUTER JOIN {5} f ON ch.nzp_frm = f.nzp_frm " +
                                        " LEFT OUTER JOIN {6} m ON f.nzp_measure = m.nzp_measure " +
                                        " LEFT OUTER JOIN {7} k ON ch.nzp_kvar = k.nzp_kvar " +
                                  " WHERE k.nzp_dom = {1} {4} and ch.nzp_serv > 1 "
                                  , table, Params.HouseId, supplierTable, serviceTable, wherePersonalAccount, formulTable, meashureTable, kvarTable);
                var charges = Connection.Query<ChdCharge>(sql);
                #region Очистка и проверка на существование
                connectionChd.Execute(string.Format("CREATE SCHEMA IF NOT EXISTS {0}_{1};", "chd_charge", year % 1000));
                sql = string.Format(" DELETE FROM {0} ch WHERE  ch.nzp_dom = {1} {2} ", tableChd, Params.HouseId, wherePersonalAccount);
                ExecSQLWithCheckExist(connectionChd, sql, Params.ParamCalc);
                #endregion

                foreach (var item in charges)
                {
                    sql = string.Format(@"INSERT INTO {0}(
                        billing_house_code, service, supplier, measure,formula, 
                        chd_point_id, nzp_kvar, nzp_dom, num_ls, nzp_serv, nzp_supp, nzp_measure, 
                        nzp_frm, dat_charge, is_device, tarif, tarif_chd, c_calc, c_calc_chd, 
                        rsum_tarif, rsum_tarif_chd, sum_nedop, sum_nedop_chd, sum_real, 
                        sum_real_chd, reval, reval_chd, real_charge, sum_insaldo, sum_money, 
                        sum_outsaldo, sum_outsaldo_chd, sum_charge,squ1,gil,rash_norm_one,rash_norm_one_chd)
                            VALUES (@billing_house_code, @service, @supplier, @measure, @formula,
                        {1}, @nzp_kvar, @nzp_dom, @num_ls, @nzp_serv, @nzp_supp, @nzp_measure, 
                        @nzp_frm, @dat_charge, @is_device, @tarif, @tarif_chd, @c_calc, @c_calc_chd, 
                        @rsum_tarif, @rsum_tarif_chd, @sum_nedop, @sum_nedop_chd, @sum_real, 
                        @sum_real_chd, @reval, @reval_chd, @real_charge, @sum_insaldo, @sum_money, 
                        @sum_outsaldo, @sum_outsaldo_chd, @sum_charge, @squ1, @gil, @rash_norm_one, @rash_norm_one_chd);", tableChd, Params.SchemaId);
                    connectionChd.Execute(sql, item);
                }
            }
            catch (Exception)
            {
                return new BaseDataResult(false, "Перенос данных не выполнен, произошла ошибка");
            }
            return new BaseDataResult();
        }


        private void ExecSQLWithCheckExist(IDbConnection connection, string sql, CalcTypes.ParamCalc param)
        {
            try
            {
                connection.Execute(sql);
            }
            catch (Exception)
            {
                Container.Resolve<ICalcPreparation>().CreateChargesTable(connection, param, true);
                connection.Execute(sql);
            }
        }
    }

    public class TransferHandlerOverride : ITransfer
    {
        protected IWindsorContainer Container;
        protected IDbConnection Connection;
        public TransferHandlerOverride(IWindsorContainer container, IDbConnection connection)
        {
            Container = container;
            Connection = connection;
        }

        public CalcVerificationParams Params { get; set; }
        /// <summary>
        /// Перенос данных из УК в ЦХД
        /// </summary>
        /// <returns></returns>
        public IDataResult TransferCharge()
        {
            try
            {
                var connectionChd = Container.Resolve<IDbConnection>("ConnectionCHD");
                var month = Params.DateCalc.Value.Month;
                var year = Params.DateCalc.Value.Year;
                var table = string.Format("{0}.chd_charge_{1}", Params.Pref, month.ToString("00"));
                var tableChd = string.Format("chd_charge_{0}.chd_charge_{1}", year % 1000, month.ToString("00"));

                var supplierTable = string.Format("{0}.supplier", Params.Pref);
                var serviceTable = string.Format("{0}.services", Params.Pref);
                var meashureTable = string.Format("{0}.s_measure", Params.Pref);
                var formulTable = string.Format("{0}.formuls", Params.Pref);
                var kvarTable = string.Format("{0}.kvar", Params.Pref);
                var wherePersonalAccount = Params.PersonalAccountId > 0
                    ? " and ch.nzp_kvar = " + Params.PersonalAccountId
                    : String.Empty;

                var sql = string.Format(" SELECT DISTINCT ch.*,supp.name_supp AS supplier,serv.service,f.name_frm AS formula,m.measure,m.nzp_measure " +
                                  " FROM {0} ch " +
                                        " LEFT OUTER JOIN {2} supp ON ch.nzp_supp = supp.nzp_supp " +
                                        " LEFT OUTER JOIN {3} serv ON ch.nzp_serv = serv.nzp_serv " +
                                        " LEFT OUTER JOIN {5} f ON ch.nzp_frm = f.nzp_frm " +
                                        " LEFT OUTER JOIN {6} m ON f.nzp_measure = m.nzp_measure " +
                                        " LEFT OUTER JOIN {7} k ON ch.nzp_kvar = k.nzp_kvar " +
                                  " WHERE k.nzp_dom = {1} {4} and ch.nzp_serv > 1 "
                                  , table, Params.HouseId, supplierTable, serviceTable, wherePersonalAccount, formulTable, meashureTable, kvarTable);
                var charges = Connection.Query<ChdCharge>(sql);
                #region Очистка и проверка на существование
                connectionChd.Execute(string.Format("CREATE SCHEMA IF NOT EXISTS {0}_{1};", "chd_charge", year % 1000));
                sql = string.Format(" DELETE FROM {0} ch WHERE  ch.nzp_dom = {1} {2} ", tableChd, Params.HouseId, wherePersonalAccount);
                ExecSQLWithCheckExist(connectionChd, sql, Params.ParamCalc);
                #endregion

                foreach (var item in charges)
                {
                    sql = string.Format(@"INSERT INTO {0}(
                        billing_house_code, service, supplier, measure,formula, 
                        chd_point_id, nzp_kvar, nzp_dom, num_ls, nzp_serv, nzp_supp, nzp_measure, 
                        nzp_frm, dat_charge, is_device, tarif, tarif_chd, c_calc, c_calc_chd, 
                        rsum_tarif, rsum_tarif_chd, sum_nedop, sum_nedop_chd, sum_real, 
                        sum_real_chd, reval, reval_chd, real_charge, sum_insaldo, sum_money, 
                        sum_outsaldo, sum_outsaldo_chd, sum_charge,squ1,gil,rash_norm_one,rash_norm_one_chd)
                            VALUES (@billing_house_code, @service, @supplier, @measure, @formula,
                        {1}, @nzp_kvar, @nzp_dom, @num_ls, @nzp_serv, @nzp_supp, @nzp_measure, 
                        @nzp_frm, @dat_charge, @is_device, @tarif, @tarif_chd, @c_calc, @c_calc_chd, 
                        @rsum_tarif, @rsum_tarif_chd, @sum_nedop, @sum_nedop_chd, @sum_real, 
                        @sum_real_chd, @reval, @reval_chd, @real_charge, @sum_insaldo, @sum_money, 
                        @sum_outsaldo, @sum_outsaldo_chd, @sum_charge, @squ1, @gil, @rash_norm_one, @rash_norm_one_chd);", tableChd, Params.SchemaId);
                    connectionChd.Execute(sql, item);
                }
            }
            catch (Exception)
            {
                return new BaseDataResult(false, "Перенос данных не выполнен, произошла ошибка");
            }
            return new BaseDataResult();
        }


        private void ExecSQLWithCheckExist(IDbConnection connection, string sql, CalcTypes.ParamCalc param)
        {
            try
            {
                connection.Execute(sql);
            }
            catch (Exception)
            {
                Container.Resolve<ICalcPreparation>().CreateChargesTable(connection, param, true);
                connection.Execute(sql);
            }
        }
    }
}
