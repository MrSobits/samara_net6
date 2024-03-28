using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;

namespace Bars.GkhGji.Regions.Tatarstan.Entities
{
    using Bars.Gkh.Entities.Base;

    /// <summary>
    /// Расширение сущности "Приложения распоряжения ГЖИ" для Татарстана
    /// </summary>
    public class TatDisposalAnnex : DisposalAnnex, IEntityUsedInErknm
    {
        /// <summary>
        /// Тип документа ЕРКНМ
        /// </summary>
        public virtual ErknmTypeDocument ErknmTypeDocument { get; set; }

        /// <inheritdoc />
        public virtual string ErknmGuid { get; set; }
    }
}
