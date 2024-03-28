namespace Bars.Gkh.RegOperator.DomainService.Period.CloseCheckers
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.Gkh.RegOperator.Enums;

    public partial class TransferWalletIncosistencyChecker
    {
        private IDictionary<WalletType, TransferReasonPacket> transferInfo = new Dictionary<WalletType, TransferReasonPacket>
        {
            {
                WalletType.BaseTariffWallet, new TransferReasonPacket
                {
                    ChargeTransfers = new[]
                    {
                        "Начисление по базовому тарифу",
                        "Перерасчет по базовому тарифу",
                        "Отмена начислений по базовому тарифу",
                        "Отмена ручной корректировки по базовому тарифу",
                        "Перенос долга при слиянии",
                        "Установка/изменение сальдо по базовому тарифу",
                        "Зачет средств за выполненные работы"
                    },
                    IncomingTransfers = new[]
                    {
                        "Оплата по базовому тарифу",
                        "Зачисление по базовому тарифу в счет отмены возврата средств",
                        "Корректировка оплат по базовому тарифу",
                        "Перенос долга при слиянии"
                    },
                    OutcomingTransfers = new[]
                    {
                        "Отмена оплаты по базовому тарифу",
                        "Возврат оплаты по базовому тарифу",
                        "Возврат взносов на КР",
                        "Возврат взносов на КР по базовому тарифу",
                        "Перенос долга при слиянии"
                    }
                }
            },
            {
                WalletType.DecisionTariffWallet, new TransferReasonPacket
                {
                    ChargeTransfers = new[]
                    {
                        "Начисление по тарифу решения",
                        "Перерасчет по тарифу решения",
                        "Отмена начислений по тарифу решений",
                        "Отмена ручной корректировки по тарифу решения",
                        "Перенос долга при слиянии",
                        "Установка/изменение сальдо по тарифу решения",
                        "Зачет средств за выполненные работы"
                    },
                    IncomingTransfers = new[]
                    {
                        "Оплата по тарифу решения",
                        "Зачисление по тарифу решения в счет отмены возврата средств",
                        "Корректировка оплат по тарифу решения",
                        "Перенос долга при слиянии"
                    },
                    OutcomingTransfers = new[]
                    {
                        "Отмена оплаты по тарифу решения",
                        "Возврат оплаты по тарифу решения",
                        "Возврат взносов на КР",
                        "Возврат взносов на КР по тарифу решения",
                        "Перенос долга при слиянии"
                    }
                }
            },
            {
                WalletType.PenaltyWallet, new TransferReasonPacket
                {
                    ChargeTransfers = new[]
                    {
                        "Начисление пени",
                        "Перерасчет пени",
                        "Отмена ручной корректировки пени",
                        "Отмена начисления пени",
                        "Установка/изменение пени",
                        "Установка/изменение сальдо по пени",
                        "Перенос долга при слиянии"
                    },
                    IncomingTransfers = new[]
                    {
                        "Оплата пени",
                        "Зачисление по пени в счет отмены возврата средств",
                        "Зачисление по пеням в счет отмены возврата",
                        "Корректировка оплат по пени",
                        "Перенос долга при слиянии"
                    },
                    OutcomingTransfers = new[]
                    {
                        "Возврат пени",
                        "Отмена оплаты пени",
                        "Перенос долга при слиянии"
                    }
                }
            },
            {
                WalletType.SocialSupportWallet, new TransferReasonPacket
                {
                    ChargeTransfers = null,
                    IncomingTransfers = new[] { "Поступление денег соц. поддержки" },
                    OutcomingTransfers = new[]
                    {
                        "Возврат МСП",
                        "Отмена поступления по соц. поддержке"
                    }
                }
            },
            {
                WalletType.AccumulatedFundWallet, new TransferReasonPacket
                {
                    ChargeTransfers = null,
                    IncomingTransfers =  new[]
                    {
                        "Ранее накопленные средства",
                        "Поступление ранее накопленных средств"
                    },
                    OutcomingTransfers = new[] { "Отмены поступления ранее накопленных средств" }
                }
            },
            {
                WalletType.RentWallet, new TransferReasonPacket
                {
                    ChargeTransfers = null,
                    IncomingTransfers =  new[] { "Поступление оплаты аренды" },
                    OutcomingTransfers = new[] { "Отмена поступления за аренду" }
                }
            },
            {
                WalletType.PreviosWorkPaymentWallet, new TransferReasonPacket
                {
                    ChargeTransfers = null,
                    IncomingTransfers = new[] { "Поступление за проделанные работы" },
                    OutcomingTransfers = new[] { "Отмена поступления средств за ранее выполненные работы" }
                }
            },
            {
                WalletType.RestructAmicableAgreementWallet, new TransferReasonPacket
                {
                    ChargeTransfers = null,
                    IncomingTransfers = new[] { "Оплата по мировому соглашению" },
                    OutcomingTransfers = new[] { "Отмена оплаты по мировому соглашению" }
                }
            }
        };


        private class TransferReasonPacket
        {
            public string[] ChargeTransfers { get; set; }

            public string[] IncomingTransfers { get; set; }

            public string[] OutcomingTransfers { get; set; }

            public string ChargeTransfersString => string.Join(",", this.ChargeTransfers.Select(x => '\'' + x + '\''));
            public string IncomingTransfersString => string.Join(",", this.IncomingTransfers.Select(x => '\'' + x + '\''));
            public string OutcomingTransfersString => string.Join(",", this.OutcomingTransfers.Select(x => '\'' + x + '\''));

            public IEnumerable<string> GetReasonByTableType(string table)
            {
                switch (table)
                {
                    case "regop_transfer":
                    case "regop_reality_transfer":
                        return this.IncomingTransfers.Union(this.OutcomingTransfers);

                    case "regop_charge_transfer":
                        return this.ChargeTransfers;
                }

                return null;
            }
        }
    }
}