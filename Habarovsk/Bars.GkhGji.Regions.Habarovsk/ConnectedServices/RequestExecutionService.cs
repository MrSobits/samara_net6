﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------
namespace Bars.GkhGji.Regions.Habarovsk.ASDOU
{


    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
   // [System.Web.Services.WebServiceBindingAttribute(Name = "RequestExecutionServiceSoapBinding", Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
     public partial class RequestExecutionService 
 //   public partial class RequestExecutionService : System.Web.Services.Protocols.SoapHttpClientProtocol
    {

        private System.Threading.SendOrPostCallback SendFinalReportOperationCompleted;

        private System.Threading.SendOrPostCallback SendReportOperationCompleted;

        /// <remarks/>
        public RequestExecutionService()
        {
          //  this.Url = "http://192.168.190.217:8080/cm-integrator/ws/RequestExecutionService";
        }

        /// <remarks/>
        public event SendFinalReportCompletedEventHandler SendFinalReportCompleted;

        /// <remarks/>
        public event SendReportCompletedEventHandler SendReportCompleted;

        /// <remarks/>
       // [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestElementName = "SendFinalReportInput", RequestNamespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0", ResponseElementName = "SendFinalReportOutput", ResponseNamespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SendResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SendResult SendFinalReport([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] FinalReport FinalReport)
        {
        //    object[] results = this.Invoke("SendFinalReport", new object[] {
        //            FinalReport});
         //   return ((SendResult)(results[0]));
         return null;
        }

        /// <remarks/>
        public System.IAsyncResult BeginSendFinalReport(FinalReport FinalReport, System.AsyncCallback callback, object asyncState)
        {
        //    return this.BeginInvoke("SendFinalReport", new object[] {
       //             FinalReport}, callback, asyncState);
       return null;
        }

        /// <remarks/>
        public SendResult EndSendFinalReport(System.IAsyncResult asyncResult)
        {
      //      object[] results = this.EndInvoke(asyncResult);
      //      return ((SendResult)(results[0]));
      return null;
        }

        /// <remarks/>
        public void SendFinalReportAsync(FinalReport FinalReport)
        {
            this.SendFinalReportAsync(FinalReport, null);
        }

        /// <remarks/>
        public void SendFinalReportAsync(FinalReport FinalReport, object userState)
        {
            if ((this.SendFinalReportOperationCompleted == null))
            {
                this.SendFinalReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendFinalReportOperationCompleted);
            }
      //      this.InvokeAsync("SendFinalReport", new object[] {
       //             FinalReport}, this.SendFinalReportOperationCompleted, userState);
        }

        private void OnSendFinalReportOperationCompleted(object arg)
        {
            if ((this.SendFinalReportCompleted != null))
            {
        //        System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
         //       this.SendFinalReportCompleted(this, new SendFinalReportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
      //  [System.Web.Services.Protocols.SoapDocumentMethodAttribute("", RequestElementName = "SendReportInput", RequestNamespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0", ResponseElementName = "SendReportOutput", ResponseNamespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0", Use = System.Web.Services.Description.SoapBindingUse.Literal, ParameterStyle = System.Web.Services.Protocols.SoapParameterStyle.Wrapped)]
        [return: System.Xml.Serialization.XmlElementAttribute("SendResult", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public SendResult SendReport([System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)] Report Report)
        {
       //     object[] results = this.Invoke("SendReport", new object[] {
       //             Report});
        //    return ((SendResult)(results[0]));
        return null;
       }

        /// <remarks/>
        public System.IAsyncResult BeginSendReport(Report Report, System.AsyncCallback callback, object asyncState)
        {
      //     return this.BeginInvoke("SendReport", new object[] {
      //              Report}, callback, asyncState);
      return null;
        }
        

        /// <remarks/>
        public SendResult EndSendReport(System.IAsyncResult asyncResult)
        {
     //       object[] results = this.EndInvoke(asyncResult);
      //      return ((SendResult)(results[0]));
      return null;
        }

        /// <remarks/>
        public void SendReportAsync(Report Report)
        {
            this.SendReportAsync(Report, null);
        }

        /// <remarks/>
        public void SendReportAsync(Report Report, object userState)
        {
            if ((this.SendReportOperationCompleted == null))
            {
                this.SendReportOperationCompleted = new System.Threading.SendOrPostCallback(this.OnSendReportOperationCompleted);
            }
     //       this.InvokeAsync("SendReport", new object[] {
      //              Report}, this.SendReportOperationCompleted, userState);
        }

        private void OnSendReportOperationCompleted(object arg)
        {
            if ((this.SendReportCompleted != null))
            {
       //         System.Web.Services.Protocols.InvokeCompletedEventArgs invokeArgs = ((System.Web.Services.Protocols.InvokeCompletedEventArgs)(arg));
        //        this.SendReportCompleted(this, new SendReportCompletedEventArgs(invokeArgs.Results, invokeArgs.Error, invokeArgs.Cancelled, invokeArgs.UserState));
            }
        }

        /// <remarks/>
        public new void CancelAsync(object userState)
        {
     //       base.CancelAsync(userState);
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class FinalReport : Report
    {

        private string[] clCodeListField;

        private ReviewResult reviewResultField;

        private string[] infosField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("clCodeList", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] clCodeList
        {
            get
            {
                return this.clCodeListField;
            }
            set
            {
                this.clCodeListField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ReviewResult reviewResult
        {
            get
            {
                return this.reviewResultField;
            }
            set
            {
                this.reviewResultField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("infos", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string[] infos
        {
            get
            {
                return this.infosField;
            }
            set
            {
                this.infosField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class ReviewResult
    {

        private ReviewResultType typeField;

        private System.DateTime replyDateField;

        private bool replyDateFieldSpecified;

        private bool measuresTakenField;

        private bool measuresTakenFieldSpecified;

        private ResultDocument resultDocumentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ReviewResultType type
        {
            get
            {
                return this.typeField;
            }
            set
            {
                this.typeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime replyDate
        {
            get
            {
                return this.replyDateField;
            }
            set
            {
                this.replyDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool replyDateSpecified
        {
            get
            {
                return this.replyDateFieldSpecified;
            }
            set
            {
                this.replyDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public bool measuresTaken
        {
            get
            {
                return this.measuresTakenField;
            }
            set
            {
                this.measuresTakenField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool measuresTakenSpecified
        {
            get
            {
                return this.measuresTakenFieldSpecified;
            }
            set
            {
                this.measuresTakenFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public ResultDocument resultDocument
        {
            get
            {
                return this.resultDocumentField;
            }
            set
            {
                this.resultDocumentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public enum ReviewResultType
    {

        /// <remarks/>
        EXPLAINED,

        /// <remarks/>
        SUPPORTED,

        /// <remarks/>
        NOTSUPPORTED,

        /// <remarks/>
        TRANSFERRED,

        /// <remarks/>
        ANSWERED,

        /// <remarks/>
        LEFTWITHOUTANSWER,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class ResultDocument
    {

        private string regNumberField;

        private System.DateTime regDateField;

        private bool regDateFieldSpecified;

        private string orgNameField;

        private string orgSSTUIdField;

        private Attachment[] attachmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string regNumber
        {
            get
            {
                return this.regNumberField;
            }
            set
            {
                this.regNumberField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime regDate
        {
            get
            {
                return this.regDateField;
            }
            set
            {
                this.regDateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool regDateSpecified
        {
            get
            {
                return this.regDateFieldSpecified;
            }
            set
            {
                this.regDateFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string orgName
        {
            get
            {
                return this.orgNameField;
            }
            set
            {
                this.orgNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string orgSSTUId
        {
            get
            {
                return this.orgSSTUIdField;
            }
            set
            {
                this.orgSSTUIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attachments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Attachment[] attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class Attachment
    {

        private string nameField;

        private byte[] contentField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string name
        {
            get
            {
                return this.nameField;
            }
            set
            {
                this.nameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified, DataType = "base64Binary")]
        public byte[] content
        {
            get
            {
                return this.contentField;
            }
            set
            {
                this.contentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class SendResult
    {

        private int codeField;

        private string messageField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public int code
        {
            get
            {
                return this.codeField;
            }
            set
            {
                this.codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string message
        {
            get
            {
                return this.messageField;
            }
            set
            {
                this.messageField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class Employee
    {

        private string lastNameField;

        private string firstNameField;

        private string middleNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string lastName
        {
            get
            {
                return this.lastNameField;
            }
            set
            {
                this.lastNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string firstName
        {
            get
            {
                return this.firstNameField;
            }
            set
            {
                this.firstNameField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string middleName
        {
            get
            {
                return this.middleNameField;
            }
            set
            {
                this.middleNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.Xml.Serialization.XmlIncludeAttribute(typeof(FinalReport))]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "urn:ru.intertrust.cmi.solution.integration.ws.server/1.0")]
    public partial class Report
    {

        private string importIdField;

        private string parentIdField;

        private string textField;

        private System.DateTime dateField;

        private Employee authorField;

        private Attachment[] attachmentsField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string importId
        {
            get
            {
                return this.importIdField;
            }
            set
            {
                this.importIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string parentId
        {
            get
            {
                return this.parentIdField;
            }
            set
            {
                this.parentIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public string text
        {
            get
            {
                return this.textField;
            }
            set
            {
                this.textField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public System.DateTime date
        {
            get
            {
                return this.dateField;
            }
            set
            {
                this.dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Employee author
        {
            get
            {
                return this.authorField;
            }
            set
            {
                this.authorField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("attachments", Form = System.Xml.Schema.XmlSchemaForm.Unqualified)]
        public Attachment[] attachments
        {
            get
            {
                return this.attachmentsField;
            }
            set
            {
                this.attachmentsField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    public delegate void SendFinalReportCompletedEventHandler(object sender, SendFinalReportCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendFinalReportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendFinalReportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SendResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SendResult)(this.results[0]));
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    public delegate void SendReportCompletedEventHandler(object sender, SendReportCompletedEventArgs e);

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("wsdl", "4.8.3928.0")]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    public partial class SendReportCompletedEventArgs : System.ComponentModel.AsyncCompletedEventArgs
    {

        private object[] results;

        internal SendReportCompletedEventArgs(object[] results, System.Exception exception, bool cancelled, object userState) :
                base(exception, cancelled, userState)
        {
            this.results = results;
        }

        /// <remarks/>
        public SendResult Result
        {
            get
            {
                this.RaiseExceptionIfNecessary();
                return ((SendResult)(this.results[0]));
            }
        }
    }
}