namespace Bars.Gkh.RegOperator.DomainService.Impl
{
    using System;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.Caching.LinqExtensions;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.RegOperator.Entities;
    using Castle.Windsor;
    using Gkh.Entities;
    using Ionic.Zip;
    using Ionic.Zlib;
    using RealityObjectAccount;

    /// <summary>
    /// Сервис выгрузки начислений
    /// </summary>
    public class UnacceptedChargesExportService : IUnacceptedChargesExportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер файлов
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="PersonalAccountCharge"/>
        /// </summary>
        public IDomainService<PersonalAccountCharge> PersonalAccountChargeDomain { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="ChargePeriod"/>
        /// </summary>
        public IDomainService<ChargePeriod> ChargePeriodDomain { get; set; }

        /// <summary>
        /// Интерфейс для получения данных банка
        /// </summary>
        public IBankAccountDataProvider BankAccountDataProvider { get; set; }

        /// <summary>
        /// Выгрузить начисления
        /// </summary>
        public Stream UnacceptedChargesExport(BaseParams baseParams)
        {
            var packId = baseParams.Params.GetAs("packId", 0);

            var data = this.GetUnacceptedChargesData(packId);
            var utf8 = Encoding.GetEncoding("utf-8");
            var utf8Bytes = utf8.GetBytes(data);
            var zipStream = new MemoryStream();

            using (var fileZip = new ZipFile(Encoding.GetEncoding("utf-8"))
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("utf-8")
            })
            {
                var currentDate = DateTime.Now.ToString("ddMMyyyy");

                fileZip.AddEntry($"{currentDate}{packId:D5}.csv", utf8Bytes);
                fileZip.Save(zipStream);
            }

            return zipStream;
        }

        private string GetUnacceptedChargesData(int packId)
        {
            var decToStr = new Func<decimal, string>(x => x.ToString("0.00", CultureInfo.InvariantCulture));

            var charges = this.PersonalAccountChargeDomain.GetAll()
                .Where(x => x.Packet != null && x.BasePersonalAccount != null)
                .Where(x => x.Packet.Id == packId)
                .Select(x => new
                {
                    RoId = x.BasePersonalAccount.Room.RealityObject.Id,
                    x.BasePersonalAccount.PersonalAccountNum,
                    x.Charge,
                    x.ChargeTariff,
                    Recalc = x.RecalcByBaseTariff,
                    x.Penalty
                })
                .ToList();

            var builder = new StringBuilder();
            builder.AppendLine("№" + packId.ToString("D5") + ";" + DateTime.Now.ToString("dd.MM.yyyy"));

            var openPeriodStartDate =
                this.ChargePeriodDomain.GetAll()
                    .FirstOrDefault(x => !x.IsClosed)
                    .Return(x => x.StartDate.ToString("MM.yyyy"), string.Empty);

            foreach (var unacceptedChargePart in charges.Split(2500))
            {
                var bankAccountNumbers = this.BankAccountDataProvider.GetBankNumbersForCollection(
                        unacceptedChargePart.Select(x => x.RoId).Distinct().Select(x => new RealityObject { Id = x }));

                foreach (var unacceptedCharge in unacceptedChargePart)
                {
                    builder.AppendLine(
                        string.Format(
                            "{0};{1};{2};{3};{4};{5};{6}",
                            unacceptedCharge.PersonalAccountNum,
                            decToStr(unacceptedCharge.Charge),
                            decToStr(unacceptedCharge.ChargeTariff),
                            decToStr(unacceptedCharge.Recalc),
                            decToStr(unacceptedCharge.Penalty),
                            openPeriodStartDate,
                            bankAccountNumbers[unacceptedCharge.RoId]));
                }
            }

            var data = builder.ToString();

            return data;
        }
    }
}
