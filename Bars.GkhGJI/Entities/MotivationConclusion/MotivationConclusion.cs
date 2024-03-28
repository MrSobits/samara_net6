namespace Bars.GkhGji.Entities
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    using Newtonsoft.Json;

    /// <summary>
    /// Мотивировочное заключение
    /// </summary>
    public class MotivationConclusion : DocumentGji
    {
        /// <summary>
        /// Базовый документ
        /// </summary>
        public virtual DocumentGji BaseDocument { get; set; }

        /// <summary>
        /// Должностное лицо (ДЛ) вынесшее распоряжение
        /// </summary>
        public virtual Inspector Autor { get; set; }

        /// <summary>
        /// Ответственный за исполнение
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Документы
        /// </summary>
        [JsonIgnore]
        public virtual IList<MotivationConclusionAnnex> AnnexList
        {
            get { return this.annexList; }
            set
            {
                this.annexList.Clear();
                if (value != null)
                {
                    this.annexList.AddRange(value);
                }
            }
        }

        private readonly List<MotivationConclusionAnnex> annexList = new List<MotivationConclusionAnnex>();

        public MotivationConclusion()
        {
            base.TypeDocumentGji = TypeDocumentGji.MotivationConclusion;
        }
    }
}