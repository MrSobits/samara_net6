namespace Bars.GkhGji.Regions.Tomsk.Entities
{
    using Bars.GkhGji.Regions.Tomsk.Enums;

    using GkhGji.Entities;

    public class TomskResolution : Resolution
    {
        /// <summary>
        /// Пол физ лица
        /// </summary>
        public virtual TypeGender PhysicalPersonGender { get; set; }

        /// <summary>
        /// Текст постановления
        /// </summary>
        public virtual string ResolutionText { get; set; }


        /// <summary>
        /// Имеется ли ходатайство
        /// </summary>
        public virtual TypeResolutionPetitions HasPetition { get; set; }


        /// <summary>
        /// Текст ходатайства
        /// </summary>
        public virtual string PetitionText { get; set; }


        /// <summary>
        /// ФИО Присутствовавших
        /// </summary>
        public virtual string FioAttend { get; set; }


        /// <summary>
        /// Текст постановления
        /// </summary>
        public virtual string ExplanationText { get; set; }
        
    }
}
