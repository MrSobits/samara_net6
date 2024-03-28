namespace Bars.GkhGji.Regions.Habarovsk.Report
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using B4.Modules.Reports;
    using B4.Utils;
    using Entities;
    using Bars.GkhGji.Entities;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Utils;
    using Gkh.Report;
    using System;
    using B4;
    using System.Xml.Linq;
    using System.IO;
    using System.Xml;
    using Bars.B4.Modules.FileStorage;
    using System.Security.Cryptography.X509Certificates;

    /// <summary>
    /// Извещение
    /// </summary>
    public class SMEVMVDReport : GkhBaseStimulReport
    {
        #region .ctor

        /// <summary>
        /// .ctor
        /// </summary>
        public SMEVMVDReport()
            : base(new ReportTemplateBinary(Properties.Resources.SMEV_MVD_PF))
        {
        }

        #endregion .ctor

        #region Private fields

        private long _SMEVMVDId;

        static XNamespace ns2 = "urn://x-artefacts-smev-gov-ru/services/message-exchange/types/1.2";
        static XNamespace ns5 = "http://www.w3.org/2000/09/xmldsig#";

        #endregion Private fields

        #region Protected properties

        /// <summary>
        /// Код шаблона (файла)
        /// </summary>
        protected override string CodeTemplate { get; set; }

        #endregion Protected properties

        #region Public properties

        /// <summary>
        /// Наименование отчета
        /// </summary>
        public override string Name
        {
            get { return "Запрос в МВД"; }
        }

        /// <summary>
        /// Описание отчета
        /// </summary>
        public override string Description
        {
            get { return "Запрос в МВД"; }
        }

        /// <summary>
        /// Идентификатор отчета
        /// </summary>
        public override string Id
        {
            get { return "SMEVMVD"; }
        }

        /// <summary>
        /// Код формы, на которой находится кнопка печати
        /// </summary>
        public override string CodeForm
        {
            get { return "SMEVMVD"; }
        }

        /// <summary>Формат печатной формы</summary>
        public override StiExportFormat ExportFormat
        {
            get { return StiExportFormat.Word2007; }
            set { }
        }

        #endregion Public properties

        #region Public methods

        /// <summary>
        /// Подготовить параметры отчета
        /// </summary>
        /// <param name="reportParams"></param>
        public override void PrepareReport(ReportParams reportParams)
        {
            var SMEVMVDDomain = Container.ResolveDomain<SMEVMVD>();
            var SMEVMVDFileDomain = Container.ResolveDomain<SMEVMVDFile>();

            var data = SMEVMVDDomain.GetAll()
                .Where(x => x.Id == _SMEVMVDId).FirstOrDefault();

            try
            {
                this.ReportParams["ReqId"] = data.Id;
                //ПОлучаем файл с ответом МВД
                var file = SMEVMVDFileDomain.GetAll()
                    .Where(x => x.SMEVMVD == data)
                    .FirstOrDefault(x => x.FileInfo.Name == "SmevMessage");
                if (file != null)
                {
                    var fileManager = Container.Resolve<IFileManager>();
                    Stream docStream = fileManager.GetFile(file.FileInfo);
                    var xDoc = LoadFromStream(docStream);
                    var Xelement = XElement.Parse(xDoc.ToString());
                    var messageElement = Xelement.Element(ns2 + "GetResponseResponse")?.Element(ns2 + "ResponseMessage");
                    if (messageElement != null)
                    {
                        var persSig = messageElement.Element(ns2 + "Response")?.Element(ns2 + "SenderProvidedResponseData")?.Element(ns2 + "PersonalSignature");
                        if (persSig != null)
                        {
                            string sertSP = persSig.Element(ns5 + "Signature")?.Element(ns5 + "KeyInfo")?.Element(ns5 + "X509Data")?.Element(ns5 + "X509Certificate")?.Value;
                            var bytes = Convert.FromBase64String(sertSP);
                            X509Certificate2 Certificate = new X509Certificate2(bytes);
                            if (Certificate != null)
                            {
                                this.ReportParams["PeriodSP"] = $"Действителен с {Certificate.NotBefore.ToShortDateString()} по {Certificate.NotAfter.ToShortDateString()}";
                                this.ReportParams["TumpSP"] = Certificate.Thumbprint;
                                this.ReportParams["УЦЭПСП"] = GetCName(Certificate.Issuer);
                                this.ReportParams["ПолучательЭПСП"] = GetCName(Certificate.Subject);
                            }
                        }
                       
                        var smevSig = messageElement.Element(ns2 + "SMEVSignature");
                        if (smevSig != null)
                        {
                            string sertSP = smevSig.Element(ns5 + "Signature")?.Element(ns5 + "KeyInfo")?.Element(ns5 + "X509Data")?.Element(ns5 + "X509Certificate")?.Value;
                            var bytes = Convert.FromBase64String(sertSP);
                            X509Certificate2 Certificate = new X509Certificate2(bytes);
                            if (Certificate != null)
                            {
                                this.ReportParams["PeriodOV"] = $"Действителен с {Certificate.NotBefore.ToShortDateString()} по {Certificate.NotAfter.ToShortDateString()}";
                                this.ReportParams["TumpOV"] = Certificate.Thumbprint;
                                this.ReportParams["УЦЭПОВ"] = GetCName(Certificate.Issuer);
                                this.ReportParams["ПолучательЭПОВ"] = GetCName(Certificate.Subject);
                            }
                        }
                    }
                }

            }
            finally
            {

            }

            
        }

        /// <summary>
        /// Установить пользовательские параметры
        /// </summary>
        /// <param name="userParamsValues"></param>
        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            _SMEVMVDId = userParamsValues.GetValue<long>("Id");
        }

        /// <summary>
        /// Получить список шаблонов
        /// </summary>
        /// <returns></returns>
        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
            {
                new TemplateInfo
                {
                    Name = "Запрос МВД",
                    Description = "Запрос МВД",
                    Code = "SMEVMVDReport",
                    Template = Properties.Resources.SMEV_MVD_PF
                }
            };
        }

        private string GetCName(string subject)
        {
            var rows = subject.Split(',');
            if (rows != null && rows.Length > 0)
            {
                foreach (string rec in rows.ToList())
                {
                    if (rec.Contains("CN="))
                    {
                        return rec.Replace("CN=", "");
                    }
                }
            }
            return "";
        }

        private static XDocument LoadFromStream(Stream stream)
        {
            using (XmlReader reader = XmlReader.Create(stream))
            {
                return XDocument.Load(reader);
            }
        }

        #endregion Public methods
    }
}