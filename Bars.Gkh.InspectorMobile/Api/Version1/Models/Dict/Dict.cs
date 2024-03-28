namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Dict
{
    using System.Collections.Generic;

    /// <summary>
    /// Справочник ГИС МЖФ РТ
    /// </summary>
    public class Dict
    {
        /// <summary>
        /// Уникальный код справочника ГИС МЖФ РТ
        /// </summary>
        public DictCode Code { get; set; }

        /// <summary>
        /// Записи справочника
        /// </summary>
        public IEnumerable<DictEntry> DictEntry { get; set; }
    }
}