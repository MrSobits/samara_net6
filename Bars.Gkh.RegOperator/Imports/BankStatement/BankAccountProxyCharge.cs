namespace Bars.Gkh.RegOperator.Imports.BankStatement
{
    using System;

    using Bars.Gkh.RegOperator.Entities;

    /// <summary>
    /// Класс посредник банковской выписки
    /// </summary>
    public class BankAccountStatementProxyCharge
    {
        /// <summary>
        /// Поле дата
        /// </summary>
        public DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Поле дата поступления/списания
        /// </summary>
        public DateTime? DateReceipt { get; set; }

        /// <summary>
        /// Поле номер
        /// </summary>
        public string DocumentNum { get; set; }

        /// <summary>
        /// Поле плательщик счет
        /// </summary>
        public string PayerAccountNum { get; set; }

        /// <summary>
        /// Поле получатель счет
        /// </summary>
        public string RecipientAccountNum { get; set; }

        /// <summary>
        /// Поле сумма
        /// </summary>
        public decimal? Sum { get; set; }
        
        /// <summary>
        /// Поле назначение платежа
        /// </summary>
        public string PaymentDetails { get; set; }

        /// <summary>
        /// Поле получатель1
        /// </summary>
        public string RecipientName { get; set; }

        /// <summary>
        /// Поле получатель инн
        /// </summary>
        public string RecipientInn { get; set; }

        /// <summary>
        /// Поле получатель бик
        /// </summary>
        public string RecipientBik { get; set; }

        /// <summary>
        /// Поле получатель корсчет
        /// </summary>
        public string RecipientCorrAccount { get; set; }

        /// <summary>
        /// Поле получатель банк1
        /// </summary>
        public string RecipientBank { get; set; }

        /// <summary>
        /// Поле получатель кпп
        /// </summary>
        public string RecipientKpp { get; set; }

        /// <summary>
        /// Поле плательщик
        /// </summary>
        public string PayerFull { get; set; }

        /// <summary>
        /// Поле плательщик1
        /// </summary>
        public string PayerName { get; set; }

        /// <summary>
        /// Поле плательщик инн
        /// </summary>
        public string PayerInn { get; set; }

        /// <summary>
        /// Поле плательщик бик
        /// </summary>
        public string PayerBik { get; set; }

        /// <summary>
        /// Поле плательщик корсчет
        /// </summary>
        public string PayerCorrAccount { get; set; }

        /// <summary>
        /// Поле плательщик банк1
        /// </summary>
        public string PayerBank { get; set; }
        
        /// <summary>
        /// Поле плательщик кпп
        /// </summary>
        public string PayerKpp { get; set; }

        /// <summary>
        /// Клеим плательщик инн и плательщик кпп
        /// </summary>
        public string Payer {
            get
            {
                var value = string.Empty;
                if (!string.IsNullOrEmpty(this.PayerInn) && !string.IsNullOrEmpty(this.PayerKpp))
                {
                    value = $"{this.PayerInn.Trim()}#{this.PayerKpp.Trim()}";
                }

                return value;
            } 
        }

        /// <summary>
        /// Поле комплексное получатель
        /// </summary>
        public string Recipient
        {
            get
            {
                var value = string.Empty;
                if (!string.IsNullOrEmpty(this.RecipientInn) && !string.IsNullOrEmpty(this.RecipientKpp))
                {
                    value = $"{this.RecipientInn.Trim()}#{this.RecipientKpp.Trim()}";
                }

                return value;
            }
        }

        /// <summary>
        /// Банковская выписка, которая будет создана из текущего прокси объекта
        /// </summary>
        public BankAccountStatement BankStatement { get; set; }
    }
}
