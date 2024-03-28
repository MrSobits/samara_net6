namespace Bars.Gkh.Gis.Services.DataContracts.OpenTatarstan
{
    using System;
    using System.CodeDom.Compiler;
    using System.ComponentModel;
    using System.Diagnostics;
    using System.Xml.Serialization;

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://open.tatarstan.ru")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://open.tatarstan.ru", IsNullable = false)]
    public partial class import_indicator
    {

        private indicator requestField;

        /// <remarks/>
        [XmlElement("azazaza")]
        public indicator request
        {
            get
            {
                return this.requestField;
            }
            set
            {
                this.requestField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class indicator
    {

        private indicator_passport indicator_passportField;

        private item[] indicator_valuesField;

        private string agentField;

        /// <remarks/>
        public indicator_passport indicator_passport
        {
            get
            {
                return this.indicator_passportField;
            }
            set
            {
                this.indicator_passportField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public item[] indicator_values
        {
            get
            {
                return this.indicator_valuesField;
            }
            set
            {
                this.indicator_valuesField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string agent
        {
            get
            {
                return this.agentField;
            }
            set
            {
                this.agentField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class indicator_passport
    {

        private string nameField;

        private string descriptionField;

        private bool activeField;

        private bool increscent_totalField;

        private bool increscent_totalFieldSpecified;

        private dimension[] ranksField;

        private reglament reglamentField;

        private period[] activity_periodsField;

        private string frequencyIdField;

        private string groupIdField;

        private string idField;

        private string measureIdField;

        /// <remarks/>
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
        public string description
        {
            get
            {
                return this.descriptionField;
            }
            set
            {
                this.descriptionField = value;
            }
        }

        /// <remarks/>
        public bool active
        {
            get
            {
                return this.activeField;
            }
            set
            {
                this.activeField = value;
            }
        }

        /// <remarks/>
        public bool increscent_total
        {
            get
            {
                return this.increscent_totalField;
            }
            set
            {
                this.increscent_totalField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool increscent_totalSpecified
        {
            get
            {
                return this.increscent_totalFieldSpecified;
            }
            set
            {
                this.increscent_totalFieldSpecified = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public dimension[] ranks
        {
            get
            {
                return this.ranksField;
            }
            set
            {
                this.ranksField = value;
            }
        }

        /// <remarks/>
        public reglament reglament
        {
            get
            {
                return this.reglamentField;
            }
            set
            {
                this.reglamentField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute(IsNullable = false)]
        public period[] activity_periods
        {
            get
            {
                return this.activity_periodsField;
            }
            set
            {
                this.activity_periodsField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string frequencyId
        {
            get
            {
                return this.frequencyIdField;
            }
            set
            {
                this.frequencyIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string groupId
        {
            get
            {
                return this.groupIdField;
            }
            set
            {
                this.groupIdField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string id
        {
            get
            {
                return this.idField;
            }
            set
            {
                this.idField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string measureId
        {
            get
            {
                return this.measureIdField;
            }
            set
            {
                this.measureIdField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class dimension
    {

        private rank[] rankField;

        private string nField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("rank")]
        public rank[] rank
        {
            get
            {
                return this.rankField;
            }
            set
            {
                this.rankField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string n
        {
            get
            {
                return this.nField;
            }
            set
            {
                this.nField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class rank
    {

        private rank rank1Field;

        private string codeField;

        private string dimensionField;

        private string keyField;

        private string nameField;

        private string typeField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("rank")]
        public rank rank1
        {
            get
            {
                return this.rank1Field;
            }
            set
            {
                this.rank1Field = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string code
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
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string dimension
        {
            get
            {
                return this.dimensionField;
            }
            set
            {
                this.dimensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
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
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string type
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
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class indicator_filter
    {

        private string itemField;

        private ItemChoiceType itemElementNameField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("description", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("frequencyId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("groupId", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("measureId", typeof(string), DataType = "integer")]
        [System.Xml.Serialization.XmlElementAttribute("name", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru", IncludeInSchema = false)]
    public enum ItemChoiceType
    {

        /// <remarks/>
        description,

        /// <remarks/>
        frequencyId,

        /// <remarks/>
        groupId,

        /// <remarks/>
        measureId,

        /// <remarks/>
        name,
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class ArrayOfIndicator
    {

        private indicator[] indicatorField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("indicator")]
        public indicator[] indicator
        {
            get
            {
                return this.indicatorField;
            }
            set
            {
                this.indicatorField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class RankReference
    {

        private object[] rankField;

        private string dimensionField;

        private string keyField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("rank")]
        public object[] rank
        {
            get
            {
                return this.rankField;
            }
            set
            {
                this.rankField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute(DataType = "integer")]
        public string dimension
        {
            get
            {
                return this.dimensionField;
            }
            set
            {
                this.dimensionField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlAttributeAttribute()]
        public string key
        {
            get
            {
                return this.keyField;
            }
            set
            {
                this.keyField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class item
    {

        private RankReference[] ranksField;

        private decimal valueField;

        private System.DateTime start_dateField;

        private System.DateTime end_dateField;

        private string input_dateField;

        private string userField;

        /// <remarks/>
        [System.Xml.Serialization.XmlArrayItemAttribute("rank", IsNullable = false)]
        public RankReference[] ranks
        {
            get
            {
                return this.ranksField;
            }
            set
            {
                this.ranksField = value;
            }
        }

        /// <remarks/>
        public decimal value
        {
            get
            {
                return this.valueField;
            }
            set
            {
                this.valueField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime start_date
        {
            get
            {
                return this.start_dateField;
            }
            set
            {
                this.start_dateField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime end_date
        {
            get
            {
                return this.end_dateField;
            }
            set
            {
                this.end_dateField = value;
            }
        }

        /// <remarks/>
        public string input_date
        {
            get
            {
                return this.input_dateField;
            }
            set
            {
                this.input_dateField = value;
            }
        }

        /// <remarks/>
        public string user
        {
            get
            {
                return this.userField;
            }
            set
            {
                this.userField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class period
    {

        private System.DateTime startField;

        private System.DateTime endField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime start
        {
            get
            {
                return this.startField;
            }
            set
            {
                this.startField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "date")]
        public System.DateTime end
        {
            get
            {
                return this.endField;
            }
            set
            {
                this.endField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru")]
    public partial class reglament
    {

        private string begin_periodField;

        private string last_enter_dateField;

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string begin_period
        {
            get
            {
                return this.begin_periodField;
            }
            set
            {
                this.begin_periodField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType = "integer")]
        public string last_enter_date
        {
            get
            {
                return this.last_enter_dateField;
            }
            set
            {
                this.last_enter_dateField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://open.tatarstan.ru")]
    [System.Xml.Serialization.XmlRootAttribute(Namespace = "http://open.tatarstan.ru", IsNullable = false)]
    public partial class import_indicatorResponse
    {

        private import_indicatorResponseImport_indicatorResult import_indicatorResultField;

        /// <remarks/>
        public import_indicatorResponseImport_indicatorResult import_indicatorResult
        {
            get
            {
                return this.import_indicatorResultField;
            }
            set
            {
                this.import_indicatorResultField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(AnonymousType = true, Namespace = "http://open.tatarstan.ru")]
    public partial class import_indicatorResponseImport_indicatorResult
    {

        private string error_codeField;

        private string itemField;

        private ItemChoiceType1 itemElementNameField;

        /// <remarks/>
        public string error_code
        {
            get
            {
                return this.error_codeField;
            }
            set
            {
                this.error_codeField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("error_details", typeof(string))]
        [System.Xml.Serialization.XmlElementAttribute("error_name", typeof(string))]
        [System.Xml.Serialization.XmlChoiceIdentifierAttribute("ItemElementName")]
        public string Item
        {
            get
            {
                return this.itemField;
            }
            set
            {
                this.itemField = value;
            }
        }

        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public ItemChoiceType1 ItemElementName
        {
            get
            {
                return this.itemElementNameField;
            }
            set
            {
                this.itemElementNameField = value;
            }
        }
    }

    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("xsd", "2.0.50727.3038")]
    [System.SerializableAttribute()]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace = "http://open.tatarstan.ru", IncludeInSchema = false)]
    public enum ItemChoiceType1
    {

        /// <remarks/>
        error_details,

        /// <remarks/>
        error_name,
    }

}
