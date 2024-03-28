namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using Bars.Gkh.Config.Attributes.DataAnnotations;

    /// <summary>
    /// Информация о статусах
    /// </summary>
    public class DocStatus
    {
        /// <summary>
        /// Уникальный идентификатор статуса
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип объекта
        /// </summary>
        [PossibleValues("\"gji_heatseason_document\" = Документ подготовки к отопительному сезону", "\"cr_obj_defect_list\" = Дефектная ведомость", 
            "\"cr_obj_estimate_calc\" = Смета КР", "\"cr_obj_contract\" = Договор на услуги объекта КР", "\"cr_obj_build_contract\" = Договор подряда", 
            "\"cr_obj_performed_work_act\" = Акт выполненных работ")]
        public string Type { get; set; }
    }
}