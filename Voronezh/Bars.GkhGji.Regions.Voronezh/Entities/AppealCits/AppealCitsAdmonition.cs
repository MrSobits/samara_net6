namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Entities.Dict;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Voronezh.Enums;

    /// <summary>
    /// Обращениям граждан - Предостережение
    /// </summary>
    public class AppealCitsAdmonition : BaseGkhEntity
    {
        /// <summary>
        /// РО
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Обращение граждан
        /// </summary>
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Документ
        /// </summary>
        public virtual string DocumentName { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Дата исполнения
        /// </summary>
        public virtual DateTime? PerfomanceDate { get; set; }

        /// <summary>
        /// Дата фактического исполнения
        /// </summary>
        public virtual DateTime? PerfomanceFactDate { get; set; }
        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Подписанный файл
        /// </summary>
        public virtual FileInfo SignedFile { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }

        /// <summary>
        /// Файл ответа
        /// </summary>
        public virtual FileInfo AnswerFile { get; set; }

        /// <summary>
        /// Подписанный файл ответа
        /// </summary>
        public virtual FileInfo SignedAnswerFile { get; set; }

        /// <summary>
        /// Подпись ответа
        /// </summary>
        public virtual FileInfo AnswerSignature { get; set; }

        /// <summary>
        /// Сертификат ответа
        /// </summary>
        public virtual FileInfo AnswerCertificate { get; set; }

        public virtual Inspector Inspector { get; set; }

        public virtual Inspector Executor { get; set; }

        public virtual KindKND KindKND { get; set; }
        /// <summary>
        /// Тип плательщика
        /// </summary>
        public virtual PayerType PayerType { get; set; }

        //--------------параметры юр.лица / ИП--------------

        public virtual string INN { get; set; }

        public virtual string KPP { get; set; }

        //---------------------------параметры физ.лица----------------------------

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FIO { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FizAddress { get; set; }

        /// <summary>
        /// ФИО
        /// </summary>
        public virtual string FizINN { get; set; }

        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public virtual PhysicalPersonDocType PhysicalPersonDocType { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        public virtual string DocumentNumberFiz { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        public virtual string DocumentSerial { get; set; }

        /// <summary>
        /// Номер в ЕРКНМ
        /// </summary>
        public virtual string ERKNMID { get; set; }

        /// <summary>
        /// GUID в ЕРКНМ
        /// </summary>
        public virtual string ERKNMGUID { get; set; }

        /// <summary>
        /// выгружено в еркнм
        /// </summary>
        public virtual bool SentToERKNM { get; set; }

        /// <summary>
        /// Аксес гуид в ЕРКНМ
        /// </summary>
        public virtual string AccessGuid { get; set; }

        /// <summary>
        /// Тип исполниителя
        /// </summary>
        public virtual GkhGji.Enums.RiskCategory RiskCategory { get; set; }

        /// <summary>
        /// выгружено в еркнм
        /// </summary>
        public virtual SurveySubject SurveySubject { get; set; }

        /// <summary>
        /// Основания проведения КНМД
        /// </summary>
        public virtual InspectionReasonERKNM InspectionReasonERKNM { get; set; }
    }
}