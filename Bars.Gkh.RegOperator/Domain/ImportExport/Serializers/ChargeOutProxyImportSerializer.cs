namespace Bars.Gkh.RegOperator.Domain.ImportExport.Serializers
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using Castle.Windsor;
    using Mapping;
    using ProxyEntity;

    public class ChargeOutProxyImportSerializer : DefaultImportExportSerializer<ChargeOutProxy>
    {
        public ChargeOutProxyImportSerializer(IWindsorContainer container) : base(container)
        {
        }

        public override Stream Serialize(List<ChargeOutProxy> data, IImportMap format)
        {
            if (format.Format.ToLowerInvariant() == "txt" && format.ProviderCode.ToLowerInvariant() == "fs_gorod")
            {
                return SerializeForFsGorod(data);
            }

            return base.Serialize(data, format);
        }

        private Stream SerializeForFsGorod(List<ChargeOutProxy> data)
        {
            var sb = new StringBuilder();

            sb.AppendLine("#FILESUM {0}".FormatUsing(data.Sum(x => Math.Round(x.charged_sum,2)).ToString("F", CultureInfo.GetCultureInfo("en-us"))));
            sb.AppendLine("#TYPE 7");
            sb.AppendLine("#SERVICE 65689");

            foreach (var entry in data)
            {
                sb.AppendFormat("{0};{1},{2},{3}{4}{5}{6},{7};{8};{9};;;;{10}:{11}:{12}",
                    entry.persacc_name,
                    entry.city,
                    entry.street,
                    entry.house,
                    entry.housing.IsEmpty() ? "" : " корп. " + entry.housing,
                    entry.liter.IsEmpty() ? "" : " лит. " + entry.liter,
                    entry.building.IsEmpty() ? "" : "/" + entry.building,
                    entry.room_num.IsEmpty() ? "" : "кв. " + entry.room_num,
                    entry.personal_acc,
                    (entry.charged_sum + entry.charged_penalty).ToString("F", CultureInfo.GetCultureInfo("en-us")),
                    "625764",
                    entry.charged_sum.ToString("F", CultureInfo.GetCultureInfo("en-us")),
                    entry.charged_penalty.ToString("F", CultureInfo.GetCultureInfo("en-us")));
                sb.AppendLine();
            }

            return new MemoryStream(Encoding.GetEncoding(1251).GetBytes(sb.ToString()));
        }
    }
}