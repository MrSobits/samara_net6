namespace Bars.Gkh.Gis.RepositoryDomain.Impl
{
    using System;
    using System.Globalization;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.Utils;
    using Castle.Windsor;
    using DomainService;
    using Entities;
    using NHibernate.Transform;

    public class AnalysisReportService : IAnalysisReportService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult GetAnalysisReportData(BaseParams baseParams)
        {
            return BaseDataResult.Error("Не реализовано");

            // не используется, оставляю до будущих времен для перепиливания
            var res = new AnalysisReport();
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            var apartmentId = baseParams.Params.GetAs<long>("apartmentId");
            var serviceId = baseParams.Params.GetAs<long>("serviceId");
            var supplierId = baseParams.Params.GetAs<long>("supplierId");

            var month = baseParams.Params.GetAs<int>("month");
            var year = baseParams.Params.GetAs<int>("year");
            decimal tariff = 0;
            decimal consumption = 0;
            decimal chargetree = 0;
            string measure = "";
            // var date = new DateTime(year, month, 1);

            #region формиррование заголовочной информации

            #region ORM
            //Container.Resolve<IDomainService<PersonalAccountAccruals>>().GetAll()
            //    .Where(x => x.ApartmentId == apartmentId && x.ServiceId == serviceId && x.Date == date)
            //    .Select(x => new
            //    {
            //        x.ApartmentId,
            //        x.Date,
            //        x.Service,
            //        x.Supplier
            //    })
            //    .ForEach(x =>
            //    {
            //        res.ApartmentId = x.ApartmentId;
            //        res.CalcMonth = x.Date.ToString("MMMM yyyy");
            //        res.Service = x.Service;
            //        res.Supplier = x.Supplier;
            //    });

            //Container.Resolve<IDomainService<House>>().GetAll()
            //    .Where(x => x.Id == apartmentId)
            //    .ForEach(x =>
            //    {
            //        res.Address = x.Address;
            //    });


            // выбираем начисления/тип расчета по ЛС,

            //var query = " select nzp_kvar, dat_month, service, name_supp, h.address from personal_accounts_accruals paa " +
            //                " join houses h on paa.nzp_kvar = h.id " +
            //            " where nzp_kvar = " + apartmentId + " and nzp_serv = " + serviceId + " and dat_month = '" + date + "'";
            //session.CreateSQLQuery(query)
            //    .SetResultTransformer(Transformers.AliasToBean<AnalysisReportTransformer>())
            //    .List<AnalysisReportTransformer>()
            //    .AsQueryable().ForEach(x =>
            //    {
            //        res.ApartmentId = x.ApartmentId;
            //        res.CalcMonth = x.Date.ToString("MMMM yyyy");
            //        res.Service = x.Service;
            //        res.Supplier = x.Supplier;
            //        res.Address = x.Address;
            //    });
            #endregion

            var query = " select pref from personal_accounts where id=" + apartmentId;
            var lsparameter = new LsParameter();
            session.CreateSQLQuery(query).SetResultTransformer(Transformers.AliasToBean<LsParameter>()).List<LsParameter>().AsQueryable().ForEach(
                x =>
                {
                    //lsparameter.Id = x.Id;
                    lsparameter.Prefix = x.Prefix;
                });

            string prefix = lsparameter.Prefix;
            string data = prefix + "_data.";
            string kernel = prefix + "_kernel.";
            string charge = prefix + "_charge_" + year.ToString().Substring(2) + ".";

            query = "Select (	T .town || TRIM (COALESCE (ulicareg, 'улица')) " +
                    " || ' ' || TRIM (COALESCE(u.ulica, '')) || ' / ' || TRIM (COALESCE(r.rajon, '')) " +
                    " || '   дом ' || TRIM (COALESCE(ndom, '')) || '  корп. ' || TRIM (COALESCE(nkor, '')) " +
                    " || '  кв. ' || TRIM (COALESCE(nkvar, '')) || '  ком. ' || TRIM (COALESCE(nkvar_n, ''))	) AS address, k.num_ls,  " +
                    " C .tarif,	C .rsum_tarif, c.c_calc, c.dat_charge, s.service as Service, sup.name_supp, m.measure " +
                    " From " + charge + "charge_" + month.ToString("00") + " c, " +
                    data + "kvar k, " +
                    data + "dom d, " +
                    data + "s_ulica u, " +
                    data + "s_rajon r, " +
                    data + "s_town t, " +
                    kernel + "services s, " +
                    kernel + "supplier sup, " +
                    kernel + "s_measure m," +
                    kernel + "formuls f " +
                    " Where " +
                    " 	K .nzp_dom = d.nzp_dom And d.nzp_ul = u.nzp_ul And u.nzp_raj = r.nzp_raj " +
                    " And T .nzp_town = r.nzp_town And s.nzp_serv=c.nzp_serv And sup.nzp_supp=c.nzp_supp " +
                    " And c.nzp_frm=f.nzp_frm And f.nzp_measure=m.nzp_measure And k.nzp_kvar=c.nzp_kvar " +
                    " And K .nzp_kvar =" + apartmentId +
                    " And c.nzp_serv=" + serviceId +
                    " And c.nzp_supp=" + supplierId;
            session.CreateSQLQuery(query)
                .SetResultTransformer(Transformers.AliasToBean<AnalysisReportTransformer>())
                .List<AnalysisReportTransformer>()
                .AsQueryable().ForEach(x =>
                {
                    res.ApartmentId = x.ApartmentId;
                    res.Address = x.Address;
                    res.CalcMonth = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(month) + " " + year;
                    res.Service = x.Service;
                    res.Supplier = x.Supplier;
                    chargetree = x.Charge;
                    tariff = x.Tariff;
                    consumption = x.Consumption;
                    measure = x.Measure;
                });

            #endregion
            #region формирование дерева расчета

            res.ReportTree = new AnalysisReportNode()
            {
                Title = "Начислено, руб",
                Charge = chargetree,
                ChildsOperation = "*",
                Left = new AnalysisReportNode()

                {
                    Title = "Расход, " + measure,
                    Charge = consumption
                },
                Right = new AnalysisReportNode()
                {
                    Title = "Тариф, руб",
                    Charge = tariff
                }
            };
            #endregion

            return new BaseDataResult(res);
        }

        internal class AnalysisReportTransformer
        {
            private int num_ls;
            private string service;
            private string name_supp;
            private string address;
            private decimal tarif;
            private decimal c_calc;
            private decimal rsum_tarif;
            private DateTime dat_charge;
            private string measure;

            //nzp_kvar
            public int ApartmentId { get { return num_ls; } set { num_ls = value; } }
            public string Service { get { return service ?? service.Trim(); } set { service = value; } }
            public string Supplier { get { return name_supp ?? name_supp.Trim(); } set { name_supp = value; } }
            public string Address { get { return address ?? address.Trim(); } set { address = value; } }
            public decimal Tariff { get { return tarif; } set { tarif = value; } }
            public decimal Consumption { get { return c_calc; } set { c_calc = value; } }
            public decimal Charge { get { return rsum_tarif; } set { rsum_tarif = value; } }
            public string Measure { get { return measure ?? measure.Trim(); } set { measure = value; } }

        }


        internal class ChargesClass
        {
            private string service;
            private string name_supp;
            private string measure;
            private string nzp_frm;
            private string nzp_frm_typ;
            private string nzp_frm_typrs;
            private string rsum_tarif;
            private string sum_nedop;
            private string is_device;


            public string Service { get { return service ?? service.Trim(); } set { service = value; } }
            public string SupplierName { get { return name_supp ?? name_supp.Trim(); } set { name_supp = value; } }
            public string Measure { get { return measure ?? measure.Trim(); } set { measure = value; } }
            public string IdFormula { get { return nzp_frm ?? nzp_frm.Trim(); } set { nzp_frm = value; } }
            public string IdFormulaType { get { return nzp_frm_typ ?? nzp_frm_typ.Trim(); } set { nzp_frm_typ = value; } }
            public string IdExpenseType
            {
                get { return nzp_frm_typrs ?? nzp_frm_typrs.Trim(); }
                set
                {
                    nzp_frm_typrs = value;
                }
            }
            public string SumTariff { get { return rsum_tarif ?? rsum_tarif.Trim(); } set { rsum_tarif = value; } }
            public string SumBackorder { get { return sum_nedop ?? sum_nedop.Trim(); } set { sum_nedop = value; } }
            public string IsDevice { get { return is_device ?? is_device.Trim(); } set { is_device = value; } }
        }
        internal class SumBackorderClass
        {
            //decimal koef = 0;
            //            if (reader["koef"] != DBNull.Value) koef = ((decimal)reader["koef"]);
            //            string cnts = "0";
            //            if (reader["cnts"] != DBNull.Value) cnts = Convert.ToString(reader["cnts"]);
            //                //((int)reader["cnts"]);
            //            decimal perc = 0;
            //            if (reader["perc"] != DBNull.Value) perc = ((decimal)reader["perc"]);

            //            val_param[8] = val_param[8].Trim() +
            //                " Тип недопоставки:" + ((string)reader["name"]) + "." +
            //                " Доля возврата = " + koef.ToString() + "." +
            //                " Количество дней недопоставки для учета = " + cnts + "." +
            //                " % возврата = " + perc.ToString() + "." +
            //                " Период: с " + 
            //                Convert.ToString(reader["dat_s"]).Trim() +
            //                " по " +
            //                Convert.ToString(reader["dat_po"]).Trim() + "." +
            //                "";

            private string koef, name, cnts, perc, dat_s, dat_po;
            public string Coefficient { get { return koef ?? koef.Trim(); } set { koef = value; } }
            public string Name { get { return name ?? name.Trim(); } set { name = value; } }
            public string Percent { get { return perc ?? perc.Trim(); } set { perc = value; } }
            public string PeriodBeginning { get { return dat_s ?? dat_s.Trim(); } set { dat_s = value; } }
            public string PeriodEnd { get { return dat_po ?? dat_po.Trim(); } set { dat_po = value; } }
        }
        internal class LsParameter
        {
            private string id, num_ls, pkod, dat_s, dat_po, pref;
            public string Id { get { return id ?? id.Trim(); } set { id = value; } }
            public string NumLs { get { return num_ls ?? num_ls.Trim(); } set { num_ls = value; } }
            public string Pkod { get { return pkod ?? pkod.Trim(); } set { pkod = value; } }
            public string Prefix { get { return pref ?? pref.Trim(); } set { pref = value; } }
        }
    }
}
