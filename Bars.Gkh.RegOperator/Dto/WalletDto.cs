using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.Dto
{
    /// <summary>
    /// DTO для гуидов кошельков
    /// </summary>
    public class WalletDto
    {
        /// <summary>
        /// Id ЛС 
        /// </summary>
        public long PersonalAccountId { get; set; }

        /// <summary>
        /// Гуид кошелька по базовому тарифу
        /// </summary>
        public string BaseTariffWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька по тарифу решения
        /// </summary>
        public string DecisionTariffWalletGuid { get; set; }

        /// <summary>
        /// Гуид кошелька на пени
        /// </summary>
        public string PenaltyWalletGuid { get; set; }
    }
}
