namespace Bars.Gkh.Regions.Msk.Entities
{
    using System.Collections.Generic;
    using B4.DataAccess;
    using B4.Utils;

    /// <summary>
    /// Информация ДПКР
    /// </summary>
    public class DpkrInfo : BaseEntity
    {
        /// <summary>
        /// Uid
        /// </summary>
        public virtual RealityObjectInfo RealityObjectInfo { get; set; }

        /// <summary>
        /// ООИ
        /// </summary>
        public virtual CeoType CeoType { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual CeoState CeoState { get; set; }

        /// <summary>
        /// Оценка состояний ООИ (множественная т.к. у некоторых оои несколько оценок, отопление, хвс, гвс, фасад)
        /// </summary>
        public virtual List<string> CeoStates { get; set; }

        /// <summary>
        /// Просрочка ремонта
        /// </summary>
        public virtual decimal Delay { get; set; }

        /// <summary>
        /// Срок эксплуатации
        /// </summary>
        public virtual int LifeTime { get; set; }

        /// <summary>
        /// Срок последнего ремонта
        /// </summary>
        public virtual int LastRepairYear { get; set; }

        /// <summary>
        /// Период
        /// </summary>
        public virtual string Period { get; set; }

        /// <summary>
        /// Баллы
        /// </summary>
        public virtual int Point { get; set; }
    }

    public enum CeoType
    {
        [Display("Ремонт внутридомовых инженерных систем электроснабжения")]
        Electro = 1,

        [Display("Ремонт внутридомовых инженерных систем газоснабжения")]
        Gas = 2,

        [Display("Ремонт внутридомовых инженерных систем водоотведения (канализации) (стояки)")]
        Kan = 3,

        [Display("Ремонт внутридомовых инженерных систем теплоснабжения (стояки)")]
        Otoplenie = 4,

        [Display("Ремонт или замена мусоропровода")]
        Mus = 5,

        [Display("Ремонт внутридомовой системы дымоудаления и противопожарной автоматики")]
        Ppiadu = 6,

        [Display("Ремонт внутридомовых инженерных систем холодного водоснабжения (стояки)")]
        Hvs = 7,

        [Display("Ремонт пожарного водопровода")]
        Pv = 8,

        [Display("Ремонт внутридомовых инженерных систем горячего водоснабжения (стояки)")]
        Gvs = 9,

        [Display("Ремонт внутридомовых инженерных систем водоотведения (канализации) (выпуски и сборные трубопроводы)")]
        Kan_M = 10,

        [Display("Ремонт внутридомовых инженерных систем теплоснабжения (разводящие магистрали)")]
        Otoplenie_M = 11,

        [Display("Ремонт внутридомовых инженерных систем холодного водоснабжения (разводящие магистрали)")]
        Hvs_M = 12,

        [Display("Ремонт внутридомовых инженерных систем горячего водоснабжения (разводящие магистрали)")]
        Gvs_M = 13,

        [Display("Ремонт фасада")]
        Fasad = 14,

        [Display("Ремонт крыши")]
        Krov = 15,

        [Display("Подвальные помещения, относящиеся к общему имуществу")]
        Podval = 16,

        [Display("Ремонт или замена внутреннего водостока")]
        Vdsk = 17
    }

    public enum CeoState
    {
        [Display("удовлетворительное/не определялось")]
        Unknown = 0,

        [Display("неудовлетворительное")]
        Unsatisfactory = 1,

        [Display("аварийное состояние")]
        Emergency = 2
    }

}
