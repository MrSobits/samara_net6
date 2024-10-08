﻿namespace Bars.GkhGji.Entities
{
    using System;
    using B4.Modules.States;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class AppealCitsDecision : BaseEntity
    {
        /// <summary>
        /// Обращение
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string AppealNumber { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime AppealDate { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime DocumentDate { get; set; }

        public virtual string Established { get; set; }

        public virtual string Decided { get; set; }

        /// <summary>
        /// документ выдан
        /// </summary>
        public virtual Inspector IssuedBy { get; set; }

        /// <summary>
        /// документ выдан
        /// </summary>
        public virtual Inspector ConsideringBy { get; set; }


        /// <summary>
        /// подписан
        /// </summary>
        public virtual Inspector SignedBy { get; set; }

        /// <summary>
        /// Постановление
        /// </summary>
        public virtual DocumentGji Resolution { get; set; }

        /// <summary>
        /// заявитель
        /// </summary>
        public virtual string Apellant { get; set; }

        /// <summary>
        /// заявитель
        /// </summary>
        public virtual string ApellantPosition { get; set; }

        /// <summary>
        /// заявитель
        /// </summary>
        public virtual string ApellantPlaceWork { get; set; }

        /// <summary>
        /// Тип заявления
        /// </summary>
        public virtual TypeDecisionAnswer TypeDecisionAnswer { get; set; }

        /// <summary>
        /// Тип заявления
        /// </summary>
        public virtual TypeAppelantPresence TypeAppelantPresence { get; set; }

        /// <summary>
        /// заявитель
        /// </summary>
        public virtual string RepresentativeFio { get; set; }


    }
}