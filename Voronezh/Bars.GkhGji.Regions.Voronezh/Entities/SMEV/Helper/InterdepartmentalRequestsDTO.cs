using Bars.Gkh.Entities;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums;
using System;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

namespace Bars.GkhGji.Regions.Voronezh.Entities.SMEV.Helper
{
    public class InterdepartmentalRequestsDTO
    {
        /// <summary>
        /// Дата запроса
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Дата запроса
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// Номер запроса
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Ведомость
        /// </summary>
        public string Department { get; set; }

        /// <summary>
        /// ответ
        /// </summary>
        public string Answer { get; set; }

        /// <summary>
        /// Инспектор
        /// </summary>
        public string Inspector { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public RequestState RequestState { get; set; }


        /// <summary>
        /// Наименование межведомственного департамента
        /// </summary>
        public NameOfInterdepartmentalDepartment NameOfInterdepartmentalDepartment { get; set; }

        public string FrontControllerName { get; set; }

        public string FrontModelName { get; set; }
    }
}
