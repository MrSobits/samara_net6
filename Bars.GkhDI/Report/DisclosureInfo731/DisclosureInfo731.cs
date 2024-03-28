namespace Bars.GkhDi.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4;

    using Bars.B4.Modules.Reports;

    using Entities;
    using Gkh.Report;

    public partial class DisclosureInfo731 : GkhBaseReport
    {
        private long _manorgId;
        private long _periodId;

        #region .ctor

        public DisclosureInfo731() : base(new ReportTemplateBinary(Properties.Resources.FuckingReport))
        {

        }

        #endregion

        #region Properties

        public override string CodeForm
        {
            get { return "DisclosureInfo"; }
        }

        public override string Name
        {
            get { return "Отчет по раскрытию информации (731 пост.)"; }
        }

        public override string Description
        {
            get { return ""; }
        }

        public override string Id
        {
            get { return "DisclosureInfo731"; }
        }

        protected override string CodeTemplate { get; set; }

        #endregion Properties

        #region DI-Properties

        public IDomainService<DisclosureInfo> DiDomain { get; set; }

        #endregion 

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _manorgId = userParamsValues.GetValue<long>("manorgId");
            _periodId = userParamsValues.GetValue<long>("periodId");
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>();
        }

        public override string Extention
        {
            get { return "xlsx"; }
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var dinfo = DiDomain.GetAll()
                .Where(x => x.PeriodDi.Id == _periodId)
                .FirstOrDefault(x => x.ManagingOrganization.Id == _manorgId);

            if (dinfo == null)
            {
                return;
            }

            var di = DiDomain.Get(dinfo.Id);

            FillManOrgInfo(reportParams, di);

            FillRobjects(reportParams, di);
        }

        private static string FormatDate(DateTime? date)
        {
            return date.HasValue ? date.Value.ToShortDateString() : null;
        }

        private static string FormatDecimal(decimal? dec)
        {
            return dec.HasValue ? dec.Value.ToString("0.00") : null;
        }

        private static TResult GetAndRemove<TKey, TValue, TResult>(Dictionary<TKey, TValue> dict, TKey key, Func<TValue, TResult> resultSelector)
        {
            TResult result;

            if (dict.ContainsKey(key))
            {
                result = resultSelector(dict[key]);
                dict.Remove(key);
            }
            else
            {
                result = default(TResult);
            }

            return result;
        }

        private static TValue GetAndRemove<TKey, TValue>(Dictionary<TKey, TValue> dict, TKey key)
        {
            TValue result;

            if (dict.ContainsKey(key))
            {
                result = dict[key];
                dict.Remove(key);
            }
            else
            {
                result = default(TValue);
            }

            return result;
        }
    }
}