namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.PayDoc.SnapshotCreators
{
    using System;
    using System.Text;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Config;
    using Bars.Gkh.Domain.Cache;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DataProviders.Meta;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;

    /// <summary>
    /// Создатель слепков данных
    /// </summary>
    [System.Obsolete("use class SnapshotCreator")]
    internal class InvoiceInfoSnapshotCreatorBase<TModel> : BaseInvoiceSnapshotCreator<TModel>
        where TModel : InvoiceInfo, new()
    {
        /// <summary>
        /// Конструктор класса
        /// </summary>
        /// <param name="container">IoC контейнер</param>
        /// <param name="cache">Кэш данных</param>
        /// <param name="configProv">Провайдер конфигурации</param>
        /// <param name="period">Период сборки</param>
        /*public InvoiceInfoSnapshotCreatorBase(IWindsorContainer container,
            GkhCache cache,
            IGkhConfigProvider configProv,
            ChargePeriod period)
            : base(container, cache, configProv, period)
        {
        }

        protected void ApplyAddressInfo(TModel record, FiasAddress address)
        {
            if (address == null) return;

            if (record is InvoiceAndActInfo)
            {
                record.As<InvoiceAndActInfo>().ПочтовыйИндексКонтрагента = address.PostCode;
            }
            else if (record is InvoiceRegistryAndActInfo)
            {
                record.As<InvoiceRegistryAndActInfo>().ПочтовыйИндексКонтрагента = address.PostCode;
            }

            // По задаче 44880 адрес должен выводится не так: Камчатский край, Карагинский р-н, с. Карага, ул. Лукашевского, д. 14
            // а вот так: ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край
            var mailingAdress = address.AddressName;

            var streetAdderss = address.StreetName.IsNotEmpty()
                ? mailingAdress.Substring(
                    mailingAdress.IndexOf(
                        address.StreetName,
                        StringComparison.Ordinal))
                : string.Empty;

            var other = streetAdderss.IsNotEmpty()
                ? mailingAdress.Replace(streetAdderss, string.Empty)
                : mailingAdress;

            if (record is InvoiceAndActInfo)
            {
                record.As<InvoiceAndActInfo>().ПочтовыйАдресКонтрагента = streetAdderss;
            }
            else if (record is InvoiceRegistryAndActInfo)
            {
                record.As<InvoiceRegistryAndActInfo>().ПочтовыйАдресКонтрагента = streetAdderss;
            }

            var splitOther = other.Split(',');

            var idx = splitOther.Length - 1;

            // теперь оставшуюся част ьадреса пишем в обратнубю сторону чтобы получилось так
            // ул. Лукашевского, д. 14, с. Карага, Карагинский р-н, Камчатский край

            var addressBuilder = new StringBuilder();
            addressBuilder.Append(streetAdderss);
            while (idx >= 0)
            {
                var item = splitOther[idx];

                if (item.IsNotEmpty())
                {
                    addressBuilder.AppendFormat(", {0}", item.Trim());
                }

                idx--;
            }

            if (record is InvoiceAndActInfo)
            {
                record.As<InvoiceAndActInfo>().ПочтовыйАдресКонтрагента = addressBuilder.ToString();
            }
            else if (record is InvoiceRegistryAndActInfo)
            {
                record.As<InvoiceRegistryAndActInfo>().ПочтовыйАдресКонтрагента = addressBuilder.ToString();
            }
        }*/
    }
}