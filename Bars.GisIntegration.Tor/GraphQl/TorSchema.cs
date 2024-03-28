namespace Bars.GisIntegration.Tor.GraphQl
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Runtime.Serialization;
    using System.Text;
    using System.Text.RegularExpressions;
    using Newtonsoft.Json.Linq;

    #region base classes
    public class FieldMetadata
    {
        public string Name { get; set; }
        public bool IsComplex { get; set; }
        public Type QueryBuilderType { get; set; }
    }

    public enum Formatting
    {
        None,
        Indented
    }

    internal static class GraphQlQueryHelper
    {
        private static readonly Regex RegexWhiteSpace = new Regex(@"\s", RegexOptions.Compiled);

        public static string GetIndentation(int level, byte indentationSize)
        {
            return new String(' ', level * indentationSize);
        }

        public static string BuildArgumentValue(object value, Formatting formatting, int level, byte indentationSize)
        {
            if (value is JValue jValue)
            {
                switch (jValue.Type)
                {
                    case JTokenType.Null: return "null";
                    case JTokenType.Integer:
                    case JTokenType.Float:
                    case JTokenType.Boolean:
                        return BuildArgumentValue(jValue.Value, formatting, level, indentationSize);
                    default:
                        return $"\"{jValue.Value}\"";
                }
            }

            if (value is Enum @enum)
                return ConvertEnumToString(@enum);

            if (value is bool @bool)
                return @bool ? "true" : "false";

            if (value is DateTime dateTime)
                return $"\"{dateTime:O}\"";

            if (value is DateTimeOffset dateTimeOffset)
                return $"\"{dateTimeOffset:O}\"";

            if (value is IGraphQlInputObject inputObject)
                return BuildInputObject(inputObject, formatting, level + 2, indentationSize);

            if (value is String || value is Guid)
                return $"\"{value}\"";

            if (value is JProperty jProperty)
            {
                if (RegexWhiteSpace.IsMatch(jProperty.Name))
                    throw new ArgumentException($"JSON object keys used as GraphQL arguments must not contain whitespace; key: {jProperty.Name}");

                return $"{jProperty.Name}:{(formatting == Formatting.Indented ? " " : null)}{BuildArgumentValue(jProperty.Value, formatting, level, indentationSize)}";
            }

            if (value is JObject jObject)
                return BuildEnumerableArgument(jObject, formatting, level + 1, indentationSize, '{', '}');

            if (value is IEnumerable enumerable)
                return BuildEnumerableArgument(enumerable, formatting, level, indentationSize, '[', ']');

            if (value is short || value is ushort || value is byte || value is int || value is uint || value is long || value is ulong || value is float || value is double || value is decimal)
                return Convert.ToString(value, CultureInfo.InvariantCulture);

            var argumentValue = Convert.ToString(value, CultureInfo.InvariantCulture);
            return $"\"{argumentValue}\"";
        }

        private static string BuildEnumerableArgument(IEnumerable enumerable, Formatting formatting, int level, byte indentationSize, char openingSymbol, char closingSymbol)
        {
            var builder = new StringBuilder();
            builder.Append(openingSymbol);
            var delimiter = String.Empty;
            foreach (var item in enumerable)
            {
                builder.Append(delimiter);

                if (formatting == Formatting.Indented)
                {
                    builder.AppendLine();
                    builder.Append(GetIndentation(level + 1, indentationSize));
                }

                builder.Append(BuildArgumentValue(item, formatting, level, indentationSize));
                delimiter = ",";
            }

            builder.Append(closingSymbol);
            return builder.ToString();
        }

        public static string BuildInputObject(IGraphQlInputObject inputObject, Formatting formatting, int level, byte indentationSize)
        {
            var builder = new StringBuilder();
            builder.Append("{");

            var isIndentedFormatting = formatting == Formatting.Indented;
            string valueSeparator;
            if (isIndentedFormatting)
            {
                builder.AppendLine();
                valueSeparator = ": ";
            }
            else
                valueSeparator = ":";

            var separator = String.Empty;
            foreach (var propertyValue in inputObject.GetPropertyValues().Where(p => p.Value != null))
            {
                var value = BuildArgumentValue(propertyValue.Value, formatting, level, indentationSize);
                builder.Append(isIndentedFormatting ? GetIndentation(level, indentationSize) : separator);
                builder.Append(propertyValue.Name);
                builder.Append(valueSeparator);
                builder.Append(value);

                separator = ",";

                if (isIndentedFormatting)
                    builder.AppendLine();
            }

            if (isIndentedFormatting)
                builder.Append(GetIndentation(level - 1, indentationSize));

            builder.Append("}");

            return builder.ToString();
        }

        private static string ConvertEnumToString(Enum @enum)
        {
            var enumMember = @enum.GetType().GetTypeInfo().GetField(@enum.ToString());
            if (enumMember == null)
                throw new InvalidOperationException("enum member resolution failed");

            var enumMemberAttribute = (EnumMemberAttribute)enumMember.GetCustomAttribute(typeof(EnumMemberAttribute));

            return enumMemberAttribute == null
                ? @enum.ToString()
                : enumMemberAttribute.Value;
        }
    }

    public struct InputPropertyInfo
    {
        public string Name { get; set; }
        public object Value { get; set; }
    }

    internal interface IGraphQlInputObject
    {
        IEnumerable<InputPropertyInfo> GetPropertyValues();
    }

    public interface IGraphQlQueryBuilder
    {
        void Clear();
        void IncludeAllFields();
        string Build(Formatting formatting = Formatting.None, byte indentationSize = 2);
    }

    public abstract class GraphQlQueryBuilder : IGraphQlQueryBuilder
    {
        private readonly Dictionary<string, GraphQlFieldCriteria> _fieldCriteria = new Dictionary<string, GraphQlFieldCriteria>();

        protected virtual string Prefix { get { return null; } }

        protected abstract IList<FieldMetadata> AllFields { get; }

        public virtual void Clear()
        {
            _fieldCriteria.Clear();
        }

        public virtual void IncludeAllFields()
        {
            IncludeFields(AllFields);
        }

        public string Build(Formatting formatting = Formatting.None, byte indentationSize = 2)
        {
            return Build(formatting, 1, indentationSize);
        }

        protected virtual string Build(Formatting formatting, int level, byte indentationSize)
        {
            var isIndentedFormatting = formatting == Formatting.Indented;

            var builder = new StringBuilder();

            if (!String.IsNullOrEmpty(Prefix))
            {
                builder.Append(Prefix);

                if (isIndentedFormatting)
                    builder.Append(" ");
            }

            builder.Append("{");

            if (isIndentedFormatting)
                builder.AppendLine();

            var separator = String.Empty;
            foreach (var criteria in _fieldCriteria.Values)
            {
                var fieldCriteria = criteria.Build(formatting, level, indentationSize);
                if (isIndentedFormatting)
                    builder.AppendLine(fieldCriteria);
                else if (!String.IsNullOrEmpty(fieldCriteria))
                {
                    builder.Append(separator);
                    builder.Append(fieldCriteria);
                }

                separator = ",";
            }

            if (isIndentedFormatting)
                builder.Append(GraphQlQueryHelper.GetIndentation(level - 1, indentationSize));

            builder.Append("}");
            return builder.ToString();
        }

        protected void IncludeScalarField(string fieldName, IDictionary<string, object> args)
        {
            _fieldCriteria[fieldName] = new GraphQlScalarFieldCriteria(fieldName, args);
        }

        protected void IncludeObjectField(string fieldName, GraphQlQueryBuilder objectFieldQueryBuilder, IDictionary<string, object> args)
        {
            _fieldCriteria[fieldName] = new GraphQlObjectFieldCriteria(fieldName, objectFieldQueryBuilder, args);
        }

        protected void IncludeFields(IEnumerable<FieldMetadata> fields)
        {
            foreach (var field in fields)
            {
                if (field.QueryBuilderType == null)
                    IncludeScalarField(field.Name, null);
                else
                {
                    var queryBuilder = (GraphQlQueryBuilder)Activator.CreateInstance(field.QueryBuilderType);
                    queryBuilder.IncludeAllFields();
                    IncludeObjectField(field.Name, queryBuilder, null);
                }
            }
        }

        private abstract class GraphQlFieldCriteria
        {
            protected readonly string FieldName;
            private readonly IDictionary<string, object> _args;

            protected GraphQlFieldCriteria(string fieldName, IDictionary<string, object> args)
            {
                FieldName = fieldName;
                _args = args;
            }

            public abstract string Build(Formatting formatting, int level, byte indentationSize);

            protected string BuildArgumentClause(Formatting formatting, int level, byte indentationSize)
            {
                var separator = formatting == Formatting.Indented ? " " : null;
                return
                    _args?.Count > 0
                        ? $"({String.Join($",{separator}", _args.Select(kvp => $"{kvp.Key}:{separator}{GraphQlQueryHelper.BuildArgumentValue(kvp.Value, formatting, level, indentationSize)}"))}){separator}"
                        : String.Empty;
            }
        }

        private class GraphQlScalarFieldCriteria : GraphQlFieldCriteria
        {
            public GraphQlScalarFieldCriteria(string fieldName, IDictionary<string, object> args) : base(fieldName, args)
            {
            }

            public override string Build(Formatting formatting, int level, byte indentationSize)
            {
                var builder = new StringBuilder();
                if (formatting == Formatting.Indented)
                    builder.Append(GraphQlQueryHelper.GetIndentation(level, indentationSize));

                builder.Append(FieldName);
                builder.Append(BuildArgumentClause(formatting, level, indentationSize));
                return builder.ToString();
            }
        }

        private class GraphQlObjectFieldCriteria : GraphQlFieldCriteria
        {
            private readonly GraphQlQueryBuilder _objectQueryBuilder;

            public GraphQlObjectFieldCriteria(string fieldName, GraphQlQueryBuilder objectQueryBuilder, IDictionary<string, object> args) : base(fieldName, args)
            {
                _objectQueryBuilder = objectQueryBuilder;
            }

            public override string Build(Formatting formatting, int level, byte indentationSize)
            {
                if (_objectQueryBuilder._fieldCriteria.Count == 0)
                    return String.Empty;

                var builder = new StringBuilder();
                var fieldName = FieldName;
                if (formatting == Formatting.Indented)
                    fieldName = $"{GraphQlQueryHelper.GetIndentation(level, indentationSize)}{FieldName} ";

                builder.Append(fieldName);
                builder.Append(BuildArgumentClause(formatting, level, indentationSize));
                builder.Append(_objectQueryBuilder.Build(formatting, level + 1, indentationSize));
                return builder.ToString();
            }
        }
    }

    public abstract class GraphQlQueryBuilder<TQueryBuilder> : GraphQlQueryBuilder where TQueryBuilder : GraphQlQueryBuilder<TQueryBuilder>
    {
        public TQueryBuilder WithAllFields()
        {
            IncludeAllFields();
            return (TQueryBuilder)this;
        }

        public TQueryBuilder WithAllScalarFields()
        {
            IncludeFields(AllFields.Where(f => !f.IsComplex));
            return (TQueryBuilder)this;
        }

        protected TQueryBuilder WithScalarField(string fieldName, IDictionary<string, object> args = null)
        {
            IncludeScalarField(fieldName, args);
            return (TQueryBuilder)this;
        }

        protected TQueryBuilder WithObjectField(string fieldName, GraphQlQueryBuilder queryBuilder, IDictionary<string, object> args = null)
        {
            IncludeObjectField(fieldName, queryBuilder, args);
            return (TQueryBuilder)this;
        }
    }
    #endregion

    #region shared types
    public enum dicHazardClassSort
    {
        id_ASC,
        id_DESC,
        HazardClassName_ASC,
        HazardClassName_DESC
    }

    public enum ControlObjectSort
    {
        id_ASC,
        id_DESC,
        ControlObjectName_ASC,
        ControlObjectName_DESC
    }

    public enum dicRoleSort
    {
        id_ASC,
        id_DESC,
        RoleName_ASC,
        RoleName_DESC
    }

    public enum dicOKVEDSort
    {
        id_ASC,
        id_DESC,
        ActivityType_ASC,
        ActivityType_DESC,
        Decipher_ASC,
        Decipher_DESC
    }

    public enum ViolationSort
    {
        id_ASC,
        id_DESC,
        ViolationSpecificComment_ASC,
        ViolationSpecificComment_DESC
    }

    public enum docControlOrderSort
    {
        id_ASC,
        id_DESC,
        ControlOrderCreateDate_ASC,
        ControlOrderCreateDate_DESC,
        ControlOrderSignDate_ASC,
        ControlOrderSignDate_DESC,
        ControlOrderNumber_ASC,
        ControlOrderNumber_DESC
    }

    public enum dicStatusPlanSort
    {
        id_ASC,
        id_DESC,
        StatusPlanName_ASC,
        StatusPlanName_DESC
    }

    public enum PersonAppointmentSort
    {
        id_ASC,
        id_DESC,
        AppStartDate_ASC,
        AppStartDate_DESC,
        AppEndDate_ASC,
        AppEndDate_DESC
    }

    public enum CheckOutEGRSMSPSort
    {
        id_ASC,
        id_DESC,
        dateRMSP_ASC,
        dateRMSP_DESC,
        caseNumber_ASC,
        caseNumber_DESC,
        IsInRMSP_ASC,
        IsInRMSP_DESC,
        LastRenewData_ASC,
        LastRenewData_DESC
    }

    public enum ControlItemResultSort
    {
        id_ASC,
        id_DESC,
        sumAdmFine_ASC,
        sumAdmFine_DESC,
        SumAdmFineStatus_ASC,
        SumAdmFineStatus_DESC
    }

    public enum dicKNMFormSort
    {
        id_ASC,
        id_DESC,
        KNMFormName_ASC,
        KNMFormName_DESC
    }

    public enum docControlPlanSort
    {
        id_ASC,
        id_DESC,
        CreateDate_ASC,
        CreateDate_DESC,
        ControlPlanName_ASC,
        ControlPlanName_DESC,
        ControlPlanApproveData_ASC,
        ControlPlanApproveData_DESC,
        FGISERPGlobalPlanGUID_ASC,
        FGISERPGlobalPlanGUID_DESC,
        ControlPlanYear_ASC,
        ControlPlanYear_DESC
    }

    public enum dicOKTMOSort
    {
        id_ASC,
        id_DESC,
        CodeOKTMO_ASC,
        CodeOKTMO_DESC
    }

    public enum CitizenRequestSort
    {
        id_ASC,
        id_DESC,
        ApplicantName_ASC,
        ApplicantName_DESC,
        ApplicantSurname_ASC,
        ApplicantSurname_DESC,
        SecondName_ASC,
        SecondName_DESC,
        ApplicantEmail_ASC,
        ApplicantEmail_DESC,
        ApplicantPhone_ASC,
        ApplicantPhone_DESC,
        RequestContent_ASC,
        RequestContent_DESC,
        ViolationCondition_ASC,
        ViolationCondition_DESC,
        RequestGetDate_ASC,
        RequestGetDate_DESC,
        TargetControlOrganization_ASC,
        TargetControlOrganization_DESC,
        TargetControlOrganizationOGRN_ASC,
        TargetControlOrganizationOGRN_DESC
    }

    public enum dicLicencedActivityTypesSort
    {
        id_ASC,
        id_DESC,
        LicensedActivityCode_ASC,
        LicensedActivityCode_DESC,
        LicensedActivityTypeName_ASC,
        LicensedActivityTypeName_DESC
    }

    public enum ControlJournalsSort
    {
        id_ASC,
        id_DESC,
        JournalCreationDate_ASC,
        JournalCreationDate_DESC,
        JournalName_ASC,
        JournalName_DESC
    }

    public enum DicStatesSort
    {
        id_ASC,
        id_DESC,
        state_ASC,
        state_DESC,
        step_ASC,
        step_DESC,
        infotxt_ASC,
        infotxt_DESC
    }

    public enum dicControlReasonDenySort
    {
        id_ASC,
        id_DESC,
        ControlReasonDeny_ASC,
        ControlReasonDeny_DESC
    }

    public enum dicControlFileStatusSort
    {
        id_ASC,
        id_DESC,
        ControlFileStatusName_ASC,
        ControlFileStatusName_DESC
    }

    public enum DicMeasureUnitsTypeSort
    {
        id_ASC,
        id_DESC,
        MeasureUnitTypeName_ASC,
        MeasureUnitTypeName_DESC
    }

    public enum docControlActSort
    {
        id_ASC,
        id_DESC,
        ControlActCreateDate_ASC,
        ControlActCreateDate_DESC
    }

    public enum RiskCatSort
    {
        id_ASC,
        id_DESC,
        CategoryClassLevel_ASC,
        CategoryClassLevel_DESC,
        RiskCatStartDate_ASC,
        RiskCatStartDate_DESC,
        RiskCatEndDate_ASC,
        RiskCatEndDate_DESC
    }

    public enum dicNPATypesSort
    {
        id_ASC,
        id_DESC,
        NPATypeName_ASC,
        NPATypeName_DESC
    }

    public enum CheckOutEGRULSort
    {
        id_ASC,
        id_DESC,
        LastRenewData_ASC,
        LastRenewData_DESC,
        dateUpdateEGRUL_ASC,
        dateUpdateEGRUL_DESC,
        [EnumMember(Value = "OGRNUL_ASC")] OgrnulAsc,
        [EnumMember(Value = "OGRNUL_DESC")] OgrnulDesc,
        OGRNULdata_ASC,
        OGRNULdata_DESC,
        [EnumMember(Value = "INNUL_ASC")] InnulAsc,
        [EnumMember(Value = "INNUL_DESC")] InnulDesc,
        dateRegYL_ASC,
        dateRegYL_DESC,
        orgNameRegYL_ASC,
        orgNameRegYL_DESC,
        addressRegOrgYL_ASC,
        addressRegOrgYL_DESC,
        grnOrgLocationYL_ASC,
        grnOrgLocationYL_DESC,
        grnDateOrgLocationYL_ASC,
        grnDateOrgLocationYL_DESC,
        dateTaxReg_ASC,
        dateTaxReg_DESC,
        taxOrg_ASC,
        taxOrg_DESC,
        grnTaxOrg_ASC,
        grnTaxOrg_DESC,
        dateGRNTaxOrg_ASC,
        dateGRNTaxOrg_DESC,
        RegNumberPFRYL_ASC,
        RegNumberPFRYL_DESC,
        dateRegNumPFRYL_ASC,
        dateRegNumPFRYL_DESC,
        branchPFRYL_ASC,
        branchPFRYL_DESC,
        GrnRegPFRYL_ASC,
        GrnRegPFRYL_DESC,
        dateGrnRegPFRYL_ASC,
        dateGrnRegPFRYL_DESC,
        grnRegExecPFRYL_ASC,
        grnRegExecPFRYL_DESC,
        regNumberExecPFRYL_ASC,
        regNumberExecPFRYL_DESC,
        dateRegNumExecPFRYL_ASC,
        dateRegNumExecPFRYL_DESC,
        branchExecSSYL_ASC,
        branchExecSSYL_DESC,
        grnRegExecSSYL_ASC,
        grnRegExecSSYL_DESC,
        dateGrnExecSSYL_ASC,
        dateGrnExecSSYL_DESC,
        typeCharterCapital_ASC,
        typeCharterCapital_DESC,
        sumCharterCapital_ASC,
        sumCharterCapital_DESC,
        grnCharterCapital_ASC,
        grnCharterCapital_DESC,
        dateGRNCharterCapital_ASC,
        dateGRNCharterCapital_DESC,
        RepresentativeName_ASC,
        RepresentativeName_DESC,
        RepresentativeSecondName_ASC,
        RepresentativeSecondName_DESC,
        RepresentativeSurname_ASC,
        RepresentativeSurname_DESC,
        [EnumMember(Value = "KPP_ASC")] KppAsc,
        [EnumMember(Value = "KPP_DESC")] KppDesc,
        ShortNameOrg_ASC,
        ShortNameOrg_DESC
    }

    public enum MandatoryReqsSort
    {
        id_ASC,
        id_DESC,
        MandratoryReqContent_ASC,
        MandratoryReqContent_DESC,
        StartDateMandatory_ASC,
        StartDateMandatory_DESC,
        EndDateMandatory_ASC,
        EndDateMandatory_DESC
    }

    public enum FileSort
    {
        id_ASC,
        id_DESC,
        HashMD5_ASC,
        HashMD5_DESC,
        FileName_ASC,
        FileName_DESC,
        FileType_ASC,
        FileType_DESC,
        FileSize_ASC,
        FileSize_DESC,
        TimeStamp_ASC,
        TimeStamp_DESC,
        DownloadLink_ASC,
        DownloadLink_DESC
    }

    public enum dicProsecSort
    {
        id_ASC,
        id_DESC,
        ProsecName_ASC,
        ProsecName_DESC
    }

    public enum dicSubjectTypeSort
    {
        id_ASC,
        id_DESC,
        SubjectTypeName_ASC,
        SubjectTypeName_DESC
    }

    public enum DicControlPlanTypeSort
    {
        id_ASC,
        id_DESC,
        ControlPlanType_ASC,
        ControlPlanType_DESC
    }

    public enum PersonSort
    {
        id_ASC,
        id_DESC,
        PersonName_ASC,
        PersonName_DESC,
        PersonSurName_ASC,
        PersonSurName_DESC,
        PersonSecondName_ASC,
        PersonSecondName_DESC
    }

    public enum RandEParameterValueSort
    {
        id_ASC,
        id_DESC,
        ParameterName_ASC,
        ParameterName_DESC,
        ParameterValue_ASC,
        ParameterValue_DESC
    }

    public enum dicPunishmentTypeSort
    {
        id_ASC,
        id_DESC,
        PunishmentType_ASC,
        PunishmentType_DESC
    }

    public enum dicDamageTypeSort
    {
        id_ASC,
        id_DESC,
        DamageType_ASC,
        DamageType_DESC
    }

    public enum dicControlBaseSort
    {
        id_ASC,
        id_DESC,
        ControlBaseName_ASC,
        ControlBaseName_DESC
    }

    public enum dicNPALevelsSort
    {
        id_ASC,
        id_DESC,
        NPALevelsName_ASC,
        NPALevelsName_DESC
    }

    public enum LicenceSort
    {
        id_ASC,
        id_DESC,
        LicenceName_ASC,
        LicenceName_DESC,
        Series_ASC,
        Series_DESC,
        Number_ASC,
        Number_DESC,
        startDate_ASC,
        startDate_DESC,
        stopDate_ASC,
        stopDate_DESC,
        endDate_ASC,
        endDate_DESC,
        BlankNumber_ASC,
        BlankNumber_DESC,
        licenseFinalDecision_ASC,
        licenseFinalDecision_DESC,
        licenseFinalStartDate_ASC,
        licenseFinalStartDate_DESC,
        licenseFinalEndDate_ASC,
        licenseFinalEndDate_DESC
    }

    public enum docControlListSort
    {
        id_ASC,
        id_DESC,
        ControlListStartDate_ASC,
        ControlListStartDate_DESC,
        ControlListEndDate_ASC,
        ControlListEndDate_DESC
    }

    public enum ControlItemPassportSort
    {
        id_ASC,
        id_DESC,
        CreateDate_ASC,
        CreateDate_DESC,
        IsInPlanYear_ASC,
        IsInPlanYear_DESC
    }

    public enum NPASort
    {
        id_ASC,
        id_DESC,
        NPAName_ASC,
        NPAName_DESC,
        ApproveDate_ASC,
        ApproveDate_DESC,
        ApproveEntity_ASC,
        ApproveEntity_DESC,
        NPAEndDate_ASC,
        NPAEndDate_DESC,
        Number_ASC,
        Number_DESC,
        Body_ASC,
        Body_DESC
    }

    public enum dicRiskCategorySort
    {
        id_ASC,
        id_DESC,
        RiskCategoryName_ASC,
        RiskCategoryName_DESC
    }

    public enum dicViolationTypesSort
    {
        id_ASC,
        id_DESC,
        Violation_ASC,
        Violation_DESC
    }

    public enum AddressSort
    {
        id_ASC,
        id_DESC,
        CodeKLADR_ASC,
        CodeKLADR_DESC,
        CodeFIAS_ASC,
        CodeFIAS_DESC,
        Address_ASC,
        Address_DESC,
        PostIndex_ASC,
        PostIndex_DESC,
        AddressFact_ASC,
        AddressFact_DESC
    }

    public enum docProcStatementSort
    {
        id_ASC,
        id_DESC,
        SendToProcDate_ASC,
        SendToProcDate_DESC,
        ProcApproveFIO_ASC,
        ProcApproveFIO_DESC,
        ProcApproveRole_ASC,
        ProcApproveRole_DESC,
        ProcApprovePlace_ASC,
        ProcApprovePlace_DESC
    }

    public enum ControlProgramSort
    {
        id_ASC,
        id_DESC,
        ProgramName_ASC,
        ProgramName_DESC
    }

    public enum DicControlItemBaseTypeSort
    {
        id_ASC,
        id_DESC,
        ControlItemBaseName_ASC,
        ControlItemBaseName_DESC
    }

    public enum dicOKOPFSort
    {
        id_ASC,
        id_DESC,
        SubjectName_ASC,
        SubjectName_DESC,
        CodeOKOPF_ASC,
        CodeOKOPF_DESC
    }

    public enum docRegulationSort
    {
        id_ASC,
        id_DESC,
        RegulationCreateDate_ASC,
        RegulationCreateDate_DESC,
        RegulationExecutionDate_ASC,
        RegulationExecutionDate_DESC,
        Result_ASC,
        Result_DESC
    }

    public enum dicLicenceStatusSort
    {
        id_ASC,
        id_DESC,
        LicenceStatusName_ASC,
        LicenceStatusName_DESC
    }

    public enum ControlItemSort
    {
        id_ASC,
        id_DESC,
        ControlItemName_ASC,
        ControlItemName_DESC,
        ControlDate_ASC,
        ControlDate_DESC,
        ControlItemResult_ASC,
        ControlItemResult_DESC
    }

    public enum dicProcStatmentStatusSort
    {
        id_ASC,
        id_DESC,
        ProcStatementStatusName_ASC,
        ProcStatementStatusName_DESC
    }

    public enum ControlOrganizationSort
    {
        id_ASC,
        id_DESC,
        ControlOrganizationName_ASC,
        ControlOrganizationName_DESC,
        [EnumMember(Value = "OGRN_KNO_ASC")] OgrnKnoAsc,
        [EnumMember(Value = "OGRN_KNO_DESC")] OgrnKnoDesc
    }

    public enum OrganizationUnitSort
    {
        id_ASC,
        id_DESC,
        OrganizationUnitName_ASC,
        OrganizationUnitName_DESC
    }

    public enum ControlFileSort
    {
        id_ASC,
        id_DESC,
        ControlFileNumber_ASC,
        ControlFileNumber_DESC,
        ControlFileStartDate_ASC,
        ControlFileStartDate_DESC
    }

    public enum RandEParameterSort
    {
        id_ASC,
        id_DESC,
        Name_ASC,
        Name_DESC
    }

    public enum ControlCardSort
    {
        id_ASC,
        id_DESC,
        ProcDataAgreement_ASC,
        ProcDataAgreement_DESC,
        ControlStartDate_ASC,
        ControlStartDate_DESC,
        ControlStatus_ASC,
        ControlStatus_DESC,
        ControlDuration_ASC,
        ControlDuration_DESC,
        DurationProlong_ASC,
        DurationProlong_DESC,
        ControlEndDate_ASC,
        ControlEndDate_DESC,
        ControlPurpose_ASC,
        ControlPurpose_DESC,
        IsJoint_ASC,
        IsJoint_DESC,
        NumberFGISERP_ASC,
        NumberFGISERP_DESC,
        FGISERPRegData_ASC,
        FGISERPRegData_DESC,
        LastEndControlDate_ASC,
        LastEndControlDate_DESC,
        CheckControlRestrict_ASC,
        CheckControlRestrict_DESC,
        ControlCancelInfo_ASC,
        ControlCancelInfo_DESC,
        InternalNumberFGISERP_ASC,
        InternalNumberFGISERP_DESC,
        ControlFactStartDate_ASC,
        ControlFactStartDate_DESC,
        ControlFactEndDate_ASC,
        ControlFactEndDate_DESC,
        FactControlPeriod_ASC,
        FactControlPeriod_DESC,
        FactControlPeriodUnit_ASC,
        FactControlPeriodUnit_DESC,
        JointControlOrganization_ASC,
        JointControlOrganization_DESC
    }

    public enum ControlListQuestionsSort
    {
        id_ASC,
        id_DESC,
        QuestionContent_ASC,
        QuestionContent_DESC,
        ouid_bmQuestionInspection_ASC,
        ouid_bmQuestionInspection_DESC,
        ouid_bmInspectionListResult_ASC,
        ouid_bmInspectionListResult_DESC
    }

    public enum ActivitySort
    {
        id_ASC,
        id_DESC,
        ActivityStartDate_ASC,
        ActivityStartDate_DESC
    }

    public enum dicControlListStatusSort
    {
        id_ASC,
        id_DESC,
        ControlListStatusName_ASC,
        ControlListStatusName_DESC
    }

    public enum dicKNMTypesSort
    {
        id_ASC,
        id_DESC,
        KNMTypeName_ASC,
        KNMTypeName_DESC
    }

    public enum dicOKSMSort
    {
        id_ASC,
        id_DESC,
        Code_ASC,
        Code_DESC,
        FullCountryName_ASC,
        FullCountryName_DESC,
        ShortCountryName_ASC,
        ShortCountryName_DESC,
        LetterCodeAlpha2_ASC,
        LetterCodeAlpha2_DESC,
        LetterCodeAlpha3_ASC,
        LetterCodeAlpha3_DESC
    }

    public enum ExtendedAttribSort
    {
        id_ASC,
        id_DESC,
        ExtAttributeName_ASC,
        ExtAttributeName_DESC,
        ExtAttributeContentUnit_ASC,
        ExtAttributeContentUnit_DESC,
        ExtAttributeTitle_ASC,
        ExtAttributeTitle_DESC
    }

    public enum dicControlTypesSort
    {
        id_ASC,
        id_DESC,
        ControlTypeName_ASC,
        ControlTypeName_DESC,
        ControlLevelName_ASC,
        ControlLevelName_DESC
    }

    public enum ExtendedAttribValueSort
    {
        id_ASC,
        id_DESC,
        ExtAttributeContent_ASC,
        ExtAttributeContent_DESC
    }

    public enum SubjectSort
    {
        id_ASC,
        id_DESC,
        mainName_ASC,
        mainName_DESC,
        Address_ASC,
        Address_DESC,
        Citizenship_ASC,
        Citizenship_DESC,
        RepresentativeSurname_ASC,
        RepresentativeSurname_DESC,
        RepresentativeName_ASC,
        RepresentativeName_DESC,
        RepresentativeSecondName_ASC,
        RepresentativeSecondName_DESC,
        NameOrg_ASC,
        NameOrg_DESC
    }

    public enum HazardClassSort
    {
        id_ASC,
        id_DESC,
        HazardClassLevel_ASC,
        HazardClassLevel_DESC,
        HazardClassStartDate_ASC,
        HazardClassStartDate_DESC,
        HazardClassEndDate_ASC,
        HazardClassEndDate_DESC
    }

    public enum DicQuestionAnswersSort
    {
        id_ASC,
        id_DESC,
        QuestionAnswer_ASC,
        QuestionAnswer_DESC
    }

    public enum MaterialsSort
    {
        id_ASC,
        id_DESC,
        MaterialName_ASC,
        MaterialName_DESC
    }

    public enum JournalAttributesSort
    {
        id_ASC,
        id_DESC,
        JournalAttirbuteName_ASC,
        JournalAttirbuteName_DESC,
        JournalAttributeValue_ASC,
        JournalAttributeValue_DESC
    }

    public enum CheckOutEGRIPSort
    {
        id_ASC,
        id_DESC,
        LastRenewData_ASC,
        LastRenewData_DESC,
        [EnumMember(Value = "OGRNIP_ASC")] OgrnipAsc,
        [EnumMember(Value = "OGRNIP_DESC")] OgrnipDesc,
        [EnumMember(Value = "INNIP_ASC")] InnipAsc,
        [EnumMember(Value = "INNIP_DESC")] InnipDesc,
        OGRNIPdate_ASC,
        OGRNIPdate_DESC,
        grnEgripDate_ASC,
        grnEgripDate_DESC,
        regIpOld_ASC,
        regIpOld_DESC,
        orgRegLocationIp_ASC,
        orgRegLocationIp_DESC,
        addressOrgRegLocIp_ASC,
        addressOrgRegLocIp_DESC,
        regNumberPfrIp_ASC,
        regNumberPfrIp_DESC,
        dateRegNumPfrIp_ASC,
        dateRegNumPfrIp_DESC,
        branchPFRIP_ASC,
        branchPFRIP_DESC,
        grnRegPFRIP_ASC,
        grnRegPFRIP_DESC,
        dateGrnRegPfrIp_ASC,
        dateGrnRegPfrIp_DESC,
        dateUpdateEgrIP_ASC,
        dateUpdateEgrIP_DESC
    }

    public enum dicStatusKNMSort
    {
        id_ASC,
        id_DESC,
        StatusKNMName_ASC,
        StatusKNMName_DESC
    }

    #endregion

    #region builder classes
    public partial class dicOKSMQueryBuilder : GraphQlQueryBuilder<dicOKSMQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "Code" },
                new FieldMetadata { Name = "FullCountryName" },
                new FieldMetadata { Name = "ShortCountryName" },
                new FieldMetadata { Name = "LetterCodeAlpha2" },
                new FieldMetadata { Name = "LetterCodeAlpha3" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicOKSMQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicOKSMQueryBuilder WithCode()
        {
            return WithScalarField("Code");
        }

        public dicOKSMQueryBuilder WithFullCountryName()
        {
            return WithScalarField("FullCountryName");
        }

        public dicOKSMQueryBuilder WithShortCountryName()
        {
            return WithScalarField("ShortCountryName");
        }

        public dicOKSMQueryBuilder WithLetterCodeAlpha2()
        {
            return WithScalarField("LetterCodeAlpha2");
        }

        public dicOKSMQueryBuilder WithLetterCodeAlpha3()
        {
            return WithScalarField("LetterCodeAlpha3");
        }

        public dicOKSMQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicOKSMQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class QueryQueryBuilder : GraphQlQueryBuilder<QueryQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "getDicQuestionAnswers", IsComplex = true, QueryBuilderType = typeof(DicQuestionAnswersQueryBuilder) },
                new FieldMetadata { Name = "findAllDicQuestionAnswers", IsComplex = true, QueryBuilderType = typeof(DicQuestionAnswersQueryBuilder) },
                new FieldMetadata { Name = "getdicProcStatmentStatus", IsComplex = true, QueryBuilderType = typeof(dicProcStatmentStatusQueryBuilder) },
                new FieldMetadata { Name = "findAlldicProcStatmentStatus", IsComplex = true, QueryBuilderType = typeof(dicProcStatmentStatusQueryBuilder) },
                new FieldMetadata { Name = "getdocProcStatement", IsComplex = true, QueryBuilderType = typeof(docProcStatementQueryBuilder) },
                new FieldMetadata { Name = "findAlldocProcStatement", IsComplex = true, QueryBuilderType = typeof(docProcStatementQueryBuilder) },
                new FieldMetadata { Name = "getdocControlOrder", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "findAlldocControlOrder", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "getControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "findAllControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "getPerson", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "findAllPerson", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "getdocControlPlan", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "findAlldocControlPlan", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "getdicStatusPlan", IsComplex = true, QueryBuilderType = typeof(dicStatusPlanQueryBuilder) },
                new FieldMetadata { Name = "findAlldicStatusPlan", IsComplex = true, QueryBuilderType = typeof(dicStatusPlanQueryBuilder) },
                new FieldMetadata { Name = "getdocControlAct", IsComplex = true, QueryBuilderType = typeof(docControlActQueryBuilder) },
                new FieldMetadata { Name = "findAlldocControlAct", IsComplex = true, QueryBuilderType = typeof(docControlActQueryBuilder) },
                new FieldMetadata { Name = "getAddress", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "findAllAddress", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "getdocControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "findAlldocControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "getControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "findAllControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "getMandatoryReqs", IsComplex = true, QueryBuilderType = typeof(MandatoryReqsQueryBuilder) },
                new FieldMetadata { Name = "findAllMandatoryReqs", IsComplex = true, QueryBuilderType = typeof(MandatoryReqsQueryBuilder) },
                new FieldMetadata { Name = "getNPA", IsComplex = true, QueryBuilderType = typeof(NPAQueryBuilder) },
                new FieldMetadata { Name = "findAllNPA", IsComplex = true, QueryBuilderType = typeof(NPAQueryBuilder) },
                new FieldMetadata { Name = "getdicControlTypes", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "findAlldicControlTypes", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "getControlItemResult", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "findAllControlItemResult", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "getMaterials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "findAllMaterials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "getViolation", IsComplex = true, QueryBuilderType = typeof(ViolationQueryBuilder) },
                new FieldMetadata { Name = "findAllViolation", IsComplex = true, QueryBuilderType = typeof(ViolationQueryBuilder) },
                new FieldMetadata { Name = "getdicViolationTypes", IsComplex = true, QueryBuilderType = typeof(dicViolationTypesQueryBuilder) },
                new FieldMetadata { Name = "findAlldicViolationTypes", IsComplex = true, QueryBuilderType = typeof(dicViolationTypesQueryBuilder) },
                new FieldMetadata { Name = "getdicPunishmentType", IsComplex = true, QueryBuilderType = typeof(dicPunishmentTypeQueryBuilder) },
                new FieldMetadata { Name = "findAlldicPunishmentType", IsComplex = true, QueryBuilderType = typeof(dicPunishmentTypeQueryBuilder) },
                new FieldMetadata { Name = "getdicDamageType", IsComplex = true, QueryBuilderType = typeof(dicDamageTypeQueryBuilder) },
                new FieldMetadata { Name = "findAlldicDamageType", IsComplex = true, QueryBuilderType = typeof(dicDamageTypeQueryBuilder) },
                new FieldMetadata { Name = "getdicControlListStatus", IsComplex = true, QueryBuilderType = typeof(dicControlListStatusQueryBuilder) },
                new FieldMetadata { Name = "findAlldicControlListStatus", IsComplex = true, QueryBuilderType = typeof(dicControlListStatusQueryBuilder) },
                new FieldMetadata { Name = "getdicKNMForm", IsComplex = true, QueryBuilderType = typeof(dicKNMFormQueryBuilder) },
                new FieldMetadata { Name = "findAlldicKNMForm", IsComplex = true, QueryBuilderType = typeof(dicKNMFormQueryBuilder) },
                new FieldMetadata { Name = "getControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "findAllControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "getdicRole", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "findAlldicRole", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "getdicControlBase", IsComplex = true, QueryBuilderType = typeof(dicControlBaseQueryBuilder) },
                new FieldMetadata { Name = "findAlldicControlBase", IsComplex = true, QueryBuilderType = typeof(dicControlBaseQueryBuilder) },
                new FieldMetadata { Name = "getdicControlReasonDeny", IsComplex = true, QueryBuilderType = typeof(dicControlReasonDenyQueryBuilder) },
                new FieldMetadata { Name = "findAlldicControlReasonDeny", IsComplex = true, QueryBuilderType = typeof(dicControlReasonDenyQueryBuilder) },
                new FieldMetadata { Name = "getdicProsec", IsComplex = true, QueryBuilderType = typeof(dicProsecQueryBuilder) },
                new FieldMetadata { Name = "findAlldicProsec", IsComplex = true, QueryBuilderType = typeof(dicProsecQueryBuilder) },
                new FieldMetadata { Name = "getdocRegulation", IsComplex = true, QueryBuilderType = typeof(docRegulationQueryBuilder) },
                new FieldMetadata { Name = "findAlldocRegulation", IsComplex = true, QueryBuilderType = typeof(docRegulationQueryBuilder) },
                new FieldMetadata { Name = "getRandEParameterValue", IsComplex = true, QueryBuilderType = typeof(RandEParameterValueQueryBuilder) },
                new FieldMetadata { Name = "findAllRandEParameterValue", IsComplex = true, QueryBuilderType = typeof(RandEParameterValueQueryBuilder) },
                new FieldMetadata { Name = "getJournalAttributes", IsComplex = true, QueryBuilderType = typeof(JournalAttributesQueryBuilder) },
                new FieldMetadata { Name = "findAllJournalAttributes", IsComplex = true, QueryBuilderType = typeof(JournalAttributesQueryBuilder) },
                new FieldMetadata { Name = "getdicControlFileStatus", IsComplex = true, QueryBuilderType = typeof(dicControlFileStatusQueryBuilder) },
                new FieldMetadata { Name = "findAlldicControlFileStatus", IsComplex = true, QueryBuilderType = typeof(dicControlFileStatusQueryBuilder) },
                new FieldMetadata { Name = "getControlItem", IsComplex = true, QueryBuilderType = typeof(ControlItemQueryBuilder) },
                new FieldMetadata { Name = "findAllControlItem", IsComplex = true, QueryBuilderType = typeof(ControlItemQueryBuilder) },
                new FieldMetadata { Name = "getCitizenRequest", IsComplex = true, QueryBuilderType = typeof(CitizenRequestQueryBuilder) },
                new FieldMetadata { Name = "findAllCitizenRequest", IsComplex = true, QueryBuilderType = typeof(CitizenRequestQueryBuilder) },
                new FieldMetadata { Name = "getdicLicenceStatus", IsComplex = true, QueryBuilderType = typeof(dicLicenceStatusQueryBuilder) },
                new FieldMetadata { Name = "findAlldicLicenceStatus", IsComplex = true, QueryBuilderType = typeof(dicLicenceStatusQueryBuilder) },
                new FieldMetadata { Name = "getRiskCat", IsComplex = true, QueryBuilderType = typeof(RiskCatQueryBuilder) },
                new FieldMetadata { Name = "findAllRiskCat", IsComplex = true, QueryBuilderType = typeof(RiskCatQueryBuilder) },
                new FieldMetadata { Name = "getdicOKOPF", IsComplex = true, QueryBuilderType = typeof(dicOKOPFQueryBuilder) },
                new FieldMetadata { Name = "findAlldicOKOPF", IsComplex = true, QueryBuilderType = typeof(dicOKOPFQueryBuilder) },
                new FieldMetadata { Name = "getCheckOutEGRUL", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRULQueryBuilder) },
                new FieldMetadata { Name = "findAllCheckOutEGRUL", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRULQueryBuilder) },
                new FieldMetadata { Name = "getdicOKSM", IsComplex = true, QueryBuilderType = typeof(dicOKSMQueryBuilder) },
                new FieldMetadata { Name = "findAlldicOKSM", IsComplex = true, QueryBuilderType = typeof(dicOKSMQueryBuilder) },
                new FieldMetadata { Name = "getdicSubjectType", IsComplex = true, QueryBuilderType = typeof(dicSubjectTypeQueryBuilder) },
                new FieldMetadata { Name = "findAlldicSubjectType", IsComplex = true, QueryBuilderType = typeof(dicSubjectTypeQueryBuilder) },
                new FieldMetadata { Name = "getExtendedAttrib", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribQueryBuilder) },
                new FieldMetadata { Name = "findAllExtendedAttrib", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribQueryBuilder) },
                new FieldMetadata { Name = "getRandEParameter", IsComplex = true, QueryBuilderType = typeof(RandEParameterQueryBuilder) },
                new FieldMetadata { Name = "findAllRandEParameter", IsComplex = true, QueryBuilderType = typeof(RandEParameterQueryBuilder) },
                new FieldMetadata { Name = "getdicStatusKNM", IsComplex = true, QueryBuilderType = typeof(dicStatusKNMQueryBuilder) },
                new FieldMetadata { Name = "findAlldicStatusKNM", IsComplex = true, QueryBuilderType = typeof(dicStatusKNMQueryBuilder) },
                new FieldMetadata { Name = "getPersonAppointment", IsComplex = true, QueryBuilderType = typeof(PersonAppointmentQueryBuilder) },
                new FieldMetadata { Name = "findAllPersonAppointment", IsComplex = true, QueryBuilderType = typeof(PersonAppointmentQueryBuilder) },
                new FieldMetadata { Name = "getdicKNMTypes", IsComplex = true, QueryBuilderType = typeof(dicKNMTypesQueryBuilder) },
                new FieldMetadata { Name = "findAlldicKNMTypes", IsComplex = true, QueryBuilderType = typeof(dicKNMTypesQueryBuilder) },
                new FieldMetadata { Name = "getControlJournals", IsComplex = true, QueryBuilderType = typeof(ControlJournalsQueryBuilder) },
                new FieldMetadata { Name = "findAllControlJournals", IsComplex = true, QueryBuilderType = typeof(ControlJournalsQueryBuilder) },
                new FieldMetadata { Name = "getControlProgram", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "findAllControlProgram", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "getControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "findAllControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "getControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "findAllControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "getdicOKTMO", IsComplex = true, QueryBuilderType = typeof(dicOKTMOQueryBuilder) },
                new FieldMetadata { Name = "findAlldicOKTMO", IsComplex = true, QueryBuilderType = typeof(dicOKTMOQueryBuilder) },
                new FieldMetadata { Name = "getActivity", IsComplex = true, QueryBuilderType = typeof(ActivityQueryBuilder) },
                new FieldMetadata { Name = "findAllActivity", IsComplex = true, QueryBuilderType = typeof(ActivityQueryBuilder) },
                new FieldMetadata { Name = "getdicOKVED", IsComplex = true, QueryBuilderType = typeof(dicOKVEDQueryBuilder) },
                new FieldMetadata { Name = "findAlldicOKVED", IsComplex = true, QueryBuilderType = typeof(dicOKVEDQueryBuilder) },
                new FieldMetadata { Name = "getLicence", IsComplex = true, QueryBuilderType = typeof(LicenceQueryBuilder) },
                new FieldMetadata { Name = "findAllLicence", IsComplex = true, QueryBuilderType = typeof(LicenceQueryBuilder) },
                new FieldMetadata { Name = "getControlItemPassport", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "findAllControlItemPassport", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "getSubject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "findAllSubject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "getCheckOutEGRIP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRIPQueryBuilder) },
                new FieldMetadata { Name = "findAllCheckOutEGRIP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRIPQueryBuilder) },
                new FieldMetadata { Name = "getCheckOutEGRSMSP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRSMSPQueryBuilder) },
                new FieldMetadata { Name = "findAllCheckOutEGRSMSP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRSMSPQueryBuilder) },
                new FieldMetadata { Name = "getdicNPALevels", IsComplex = true, QueryBuilderType = typeof(dicNPALevelsQueryBuilder) },
                new FieldMetadata { Name = "findAlldicNPALevels", IsComplex = true, QueryBuilderType = typeof(dicNPALevelsQueryBuilder) },
                new FieldMetadata { Name = "getdicNPATypes", IsComplex = true, QueryBuilderType = typeof(dicNPATypesQueryBuilder) },
                new FieldMetadata { Name = "findAlldicNPATypes", IsComplex = true, QueryBuilderType = typeof(dicNPATypesQueryBuilder) },
                new FieldMetadata { Name = "getFile", IsComplex = true, QueryBuilderType = typeof(FileQueryBuilder) },
                new FieldMetadata { Name = "findAllFile", IsComplex = true, QueryBuilderType = typeof(FileQueryBuilder) },
                new FieldMetadata { Name = "getDicMeasureUnitsType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "findAllDicMeasureUnitsType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "getDicControlItemBaseType", IsComplex = true, QueryBuilderType = typeof(DicControlItemBaseTypeQueryBuilder) },
                new FieldMetadata { Name = "findAllDicControlItemBaseType", IsComplex = true, QueryBuilderType = typeof(DicControlItemBaseTypeQueryBuilder) },
                new FieldMetadata { Name = "getDicControlPlanType", IsComplex = true, QueryBuilderType = typeof(DicControlPlanTypeQueryBuilder) },
                new FieldMetadata { Name = "findAllDicControlPlanType", IsComplex = true, QueryBuilderType = typeof(DicControlPlanTypeQueryBuilder) },
                new FieldMetadata { Name = "getOrganizationUnit", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "findAllOrganizationUnit", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "getdicLicencedActivityTypes", IsComplex = true, QueryBuilderType = typeof(dicLicencedActivityTypesQueryBuilder) },
                new FieldMetadata { Name = "findAlldicLicencedActivityTypes", IsComplex = true, QueryBuilderType = typeof(dicLicencedActivityTypesQueryBuilder) },
                new FieldMetadata { Name = "getdicHazardClass", IsComplex = true, QueryBuilderType = typeof(dicHazardClassQueryBuilder) },
                new FieldMetadata { Name = "findAlldicHazardClass", IsComplex = true, QueryBuilderType = typeof(dicHazardClassQueryBuilder) },
                new FieldMetadata { Name = "getdicRiskCategory", IsComplex = true, QueryBuilderType = typeof(dicRiskCategoryQueryBuilder) },
                new FieldMetadata { Name = "findAlldicRiskCategory", IsComplex = true, QueryBuilderType = typeof(dicRiskCategoryQueryBuilder) },
                new FieldMetadata { Name = "getExtendedAttribValue", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribValueQueryBuilder) },
                new FieldMetadata { Name = "findAllExtendedAttribValue", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribValueQueryBuilder) },
                new FieldMetadata { Name = "getHazardClass", IsComplex = true, QueryBuilderType = typeof(HazardClassQueryBuilder) },
                new FieldMetadata { Name = "findAllHazardClass", IsComplex = true, QueryBuilderType = typeof(HazardClassQueryBuilder) },
                new FieldMetadata { Name = "getDicStates", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "findAllDicStates", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public QueryQueryBuilder WithGetDicQuestionAnswers(DicQuestionAnswersQueryBuilder dicQuestionAnswersQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getDicQuestionAnswers", dicQuestionAnswersQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllDicQuestionAnswers(DicQuestionAnswersQueryBuilder dicQuestionAnswersQueryBuilder, DicQuestionAnswersCondition condition = null, DicQuestionAnswersFilter filter = null, IEnumerable<DicQuestionAnswersSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllDicQuestionAnswers", dicQuestionAnswersQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicProcStatmentStatus(dicProcStatmentStatusQueryBuilder dicProcStatmentStatusQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicProcStatmentStatus", dicProcStatmentStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicProcStatmentStatus(dicProcStatmentStatusQueryBuilder dicProcStatmentStatusQueryBuilder, dicProcStatmentStatusCondition condition = null, dicProcStatmentStatusFilter filter = null, IEnumerable<dicProcStatmentStatusSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicProcStatmentStatus", dicProcStatmentStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdocProcStatement(docProcStatementQueryBuilder docProcStatementQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdocProcStatement", docProcStatementQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldocProcStatement(docProcStatementQueryBuilder docProcStatementQueryBuilder, docProcStatementCondition condition = null, docProcStatementFilter filter = null, IEnumerable<docProcStatementSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldocProcStatement", docProcStatementQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdocControlOrder(docControlOrderQueryBuilder docControlOrderQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdocControlOrder", docControlOrderQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldocControlOrder(docControlOrderQueryBuilder docControlOrderQueryBuilder, docControlOrderCondition condition = null, docControlOrderFilter filter = null, IEnumerable<docControlOrderSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldocControlOrder", docControlOrderQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlCard(ControlCardQueryBuilder controlCardQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlCard", controlCardQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlCard(ControlCardQueryBuilder controlCardQueryBuilder, ControlCardCondition condition = null, ControlCardFilter filter = null, IEnumerable<ControlCardSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlCard", controlCardQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetPerson(PersonQueryBuilder personQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getPerson", personQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllPerson(PersonQueryBuilder personQueryBuilder, PersonCondition condition = null, PersonFilter filter = null, IEnumerable<PersonSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllPerson", personQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdocControlPlan(docControlPlanQueryBuilder docControlPlanQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdocControlPlan", docControlPlanQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldocControlPlan(docControlPlanQueryBuilder docControlPlanQueryBuilder, docControlPlanCondition condition = null, docControlPlanFilter filter = null, IEnumerable<docControlPlanSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldocControlPlan", docControlPlanQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicStatusPlan(dicStatusPlanQueryBuilder dicStatusPlanQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicStatusPlan", dicStatusPlanQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicStatusPlan(dicStatusPlanQueryBuilder dicStatusPlanQueryBuilder, dicStatusPlanCondition condition = null, dicStatusPlanFilter filter = null, IEnumerable<dicStatusPlanSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicStatusPlan", dicStatusPlanQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdocControlAct(docControlActQueryBuilder docControlActQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdocControlAct", docControlActQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldocControlAct(docControlActQueryBuilder docControlActQueryBuilder, docControlActCondition condition = null, docControlActFilter filter = null, IEnumerable<docControlActSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldocControlAct", docControlActQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetAddress(AddressQueryBuilder addressQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getAddress", addressQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllAddress(AddressQueryBuilder addressQueryBuilder, AddressCondition condition = null, AddressFilter filter = null, IEnumerable<AddressSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllAddress", addressQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdocControlList(docControlListQueryBuilder docControlListQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdocControlList", docControlListQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldocControlList(docControlListQueryBuilder docControlListQueryBuilder, docControlListCondition condition = null, docControlListFilter filter = null, IEnumerable<docControlListSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldocControlList", docControlListQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsCondition condition = null, ControlListQuestionsFilter filter = null, IEnumerable<ControlListQuestionsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetMandatoryReqs(MandatoryReqsQueryBuilder mandatoryReqsQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getMandatoryReqs", mandatoryReqsQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllMandatoryReqs(MandatoryReqsQueryBuilder mandatoryReqsQueryBuilder, MandatoryReqsCondition condition = null, MandatoryReqsFilter filter = null, IEnumerable<MandatoryReqsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllMandatoryReqs", mandatoryReqsQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetNpa(NPAQueryBuilder nPAQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getNPA", nPAQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllNpa(NPAQueryBuilder nPAQueryBuilder, NPACondition condition = null, NPAFilter filter = null, IEnumerable<NPASort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllNPA", nPAQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicControlTypes(dicControlTypesQueryBuilder dicControlTypesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicControlTypes", dicControlTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicControlTypes(dicControlTypesQueryBuilder dicControlTypesQueryBuilder, dicControlTypesCondition condition = null, dicControlTypesFilter filter = null, IEnumerable<dicControlTypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicControlTypes", dicControlTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlItemResult(ControlItemResultQueryBuilder controlItemResultQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlItemResult", controlItemResultQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlItemResult(ControlItemResultQueryBuilder controlItemResultQueryBuilder, ControlItemResultCondition condition = null, ControlItemResultFilter filter = null, IEnumerable<ControlItemResultSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlItemResult", controlItemResultQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetMaterials(MaterialsQueryBuilder materialsQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getMaterials", materialsQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllMaterials", materialsQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetViolation(ViolationQueryBuilder violationQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getViolation", violationQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllViolation(ViolationQueryBuilder violationQueryBuilder, ViolationCondition condition = null, ViolationFilter filter = null, IEnumerable<ViolationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllViolation", violationQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicViolationTypes(dicViolationTypesQueryBuilder dicViolationTypesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicViolationTypes", dicViolationTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicViolationTypes(dicViolationTypesQueryBuilder dicViolationTypesQueryBuilder, dicViolationTypesCondition condition = null, dicViolationTypesFilter filter = null, IEnumerable<dicViolationTypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicViolationTypes", dicViolationTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicPunishmentType(dicPunishmentTypeQueryBuilder dicPunishmentTypeQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicPunishmentType", dicPunishmentTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicPunishmentType(dicPunishmentTypeQueryBuilder dicPunishmentTypeQueryBuilder, dicPunishmentTypeCondition condition = null, dicPunishmentTypeFilter filter = null, IEnumerable<dicPunishmentTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicPunishmentType", dicPunishmentTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicDamageType(dicDamageTypeQueryBuilder dicDamageTypeQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicDamageType", dicDamageTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicDamageType(dicDamageTypeQueryBuilder dicDamageTypeQueryBuilder, dicDamageTypeCondition condition = null, dicDamageTypeFilter filter = null, IEnumerable<dicDamageTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicDamageType", dicDamageTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicControlListStatus(dicControlListStatusQueryBuilder dicControlListStatusQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicControlListStatus", dicControlListStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicControlListStatus(dicControlListStatusQueryBuilder dicControlListStatusQueryBuilder, dicControlListStatusCondition condition = null, dicControlListStatusFilter filter = null, IEnumerable<dicControlListStatusSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicControlListStatus", dicControlListStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicKnmForm(dicKNMFormQueryBuilder dicKNMFormQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicKNMForm", dicKNMFormQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicKnmForm(dicKNMFormQueryBuilder dicKNMFormQueryBuilder, dicKNMFormCondition condition = null, dicKNMFormFilter filter = null, IEnumerable<dicKNMFormSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicKNMForm", dicKNMFormQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlOrganization", controlOrganizationQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, ControlOrganizationCondition condition = null, ControlOrganizationFilter filter = null, IEnumerable<ControlOrganizationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlOrganization", controlOrganizationQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicRole(dicRoleQueryBuilder dicRoleQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicRole", dicRoleQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicRole(dicRoleQueryBuilder dicRoleQueryBuilder, dicRoleCondition condition = null, dicRoleFilter filter = null, IEnumerable<dicRoleSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicRole", dicRoleQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicControlBase(dicControlBaseQueryBuilder dicControlBaseQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicControlBase", dicControlBaseQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicControlBase(dicControlBaseQueryBuilder dicControlBaseQueryBuilder, dicControlBaseCondition condition = null, dicControlBaseFilter filter = null, IEnumerable<dicControlBaseSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicControlBase", dicControlBaseQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicControlReasonDeny(dicControlReasonDenyQueryBuilder dicControlReasonDenyQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicControlReasonDeny", dicControlReasonDenyQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicControlReasonDeny(dicControlReasonDenyQueryBuilder dicControlReasonDenyQueryBuilder, dicControlReasonDenyCondition condition = null, dicControlReasonDenyFilter filter = null, IEnumerable<dicControlReasonDenySort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicControlReasonDeny", dicControlReasonDenyQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicProsec(dicProsecQueryBuilder dicProsecQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicProsec", dicProsecQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicProsec(dicProsecQueryBuilder dicProsecQueryBuilder, dicProsecCondition condition = null, dicProsecFilter filter = null, IEnumerable<dicProsecSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicProsec", dicProsecQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdocRegulation(docRegulationQueryBuilder docRegulationQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdocRegulation", docRegulationQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldocRegulation(docRegulationQueryBuilder docRegulationQueryBuilder, docRegulationCondition condition = null, docRegulationFilter filter = null, IEnumerable<docRegulationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldocRegulation", docRegulationQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetRandEParameterValue(RandEParameterValueQueryBuilder randEParameterValueQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getRandEParameterValue", randEParameterValueQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllRandEParameterValue(RandEParameterValueQueryBuilder randEParameterValueQueryBuilder, RandEParameterValueCondition condition = null, RandEParameterValueFilter filter = null, IEnumerable<RandEParameterValueSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllRandEParameterValue", randEParameterValueQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetJournalAttributes(JournalAttributesQueryBuilder journalAttributesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getJournalAttributes", journalAttributesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllJournalAttributes(JournalAttributesQueryBuilder journalAttributesQueryBuilder, JournalAttributesCondition condition = null, JournalAttributesFilter filter = null, IEnumerable<JournalAttributesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllJournalAttributes", journalAttributesQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicControlFileStatus(dicControlFileStatusQueryBuilder dicControlFileStatusQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicControlFileStatus", dicControlFileStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicControlFileStatus(dicControlFileStatusQueryBuilder dicControlFileStatusQueryBuilder, dicControlFileStatusCondition condition = null, dicControlFileStatusFilter filter = null, IEnumerable<dicControlFileStatusSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicControlFileStatus", dicControlFileStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlItem(ControlItemQueryBuilder controlItemQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlItem", controlItemQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlItem(ControlItemQueryBuilder controlItemQueryBuilder, ControlItemCondition condition = null, ControlItemFilter filter = null, IEnumerable<ControlItemSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlItem", controlItemQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetCitizenRequest(CitizenRequestQueryBuilder citizenRequestQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getCitizenRequest", citizenRequestQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllCitizenRequest(CitizenRequestQueryBuilder citizenRequestQueryBuilder, CitizenRequestCondition condition = null, CitizenRequestFilter filter = null, IEnumerable<CitizenRequestSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllCitizenRequest", citizenRequestQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicLicenceStatus(dicLicenceStatusQueryBuilder dicLicenceStatusQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicLicenceStatus", dicLicenceStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicLicenceStatus(dicLicenceStatusQueryBuilder dicLicenceStatusQueryBuilder, dicLicenceStatusCondition condition = null, dicLicenceStatusFilter filter = null, IEnumerable<dicLicenceStatusSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicLicenceStatus", dicLicenceStatusQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetRiskCat(RiskCatQueryBuilder riskCatQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getRiskCat", riskCatQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllRiskCat(RiskCatQueryBuilder riskCatQueryBuilder, RiskCatCondition condition = null, RiskCatFilter filter = null, IEnumerable<RiskCatSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllRiskCat", riskCatQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicOkopf(dicOKOPFQueryBuilder dicOKOPFQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicOKOPF", dicOKOPFQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicOkopf(dicOKOPFQueryBuilder dicOKOPFQueryBuilder, dicOKOPFCondition condition = null, dicOKOPFFilter filter = null, IEnumerable<dicOKOPFSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicOKOPF", dicOKOPFQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetCheckOutEgrul(CheckOutEGRULQueryBuilder checkOutEGRULQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getCheckOutEGRUL", checkOutEGRULQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllCheckOutEgrul(CheckOutEGRULQueryBuilder checkOutEGRULQueryBuilder, CheckOutEGRULCondition condition = null, CheckOutEGRULFilter filter = null, IEnumerable<CheckOutEGRULSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllCheckOutEGRUL", checkOutEGRULQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicOksm(dicOKSMQueryBuilder dicOKSMQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicOKSM", dicOKSMQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicOksm(dicOKSMQueryBuilder dicOKSMQueryBuilder, dicOKSMCondition condition = null, dicOKSMFilter filter = null, IEnumerable<dicOKSMSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicOKSM", dicOKSMQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicSubjectType(dicSubjectTypeQueryBuilder dicSubjectTypeQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicSubjectType", dicSubjectTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicSubjectType(dicSubjectTypeQueryBuilder dicSubjectTypeQueryBuilder, dicSubjectTypeCondition condition = null, dicSubjectTypeFilter filter = null, IEnumerable<dicSubjectTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicSubjectType", dicSubjectTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetExtendedAttrib(ExtendedAttribQueryBuilder extendedAttribQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getExtendedAttrib", extendedAttribQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllExtendedAttrib(ExtendedAttribQueryBuilder extendedAttribQueryBuilder, ExtendedAttribCondition condition = null, ExtendedAttribFilter filter = null, IEnumerable<ExtendedAttribSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllExtendedAttrib", extendedAttribQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetRandEParameter(RandEParameterQueryBuilder randEParameterQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getRandEParameter", randEParameterQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllRandEParameter(RandEParameterQueryBuilder randEParameterQueryBuilder, RandEParameterCondition condition = null, RandEParameterFilter filter = null, IEnumerable<RandEParameterSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllRandEParameter", randEParameterQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicStatusKnm(dicStatusKNMQueryBuilder dicStatusKNMQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicStatusKNM", dicStatusKNMQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicStatusKnm(dicStatusKNMQueryBuilder dicStatusKNMQueryBuilder, dicStatusKNMCondition condition = null, dicStatusKNMFilter filter = null, IEnumerable<dicStatusKNMSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicStatusKNM", dicStatusKNMQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetPersonAppointment(PersonAppointmentQueryBuilder personAppointmentQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getPersonAppointment", personAppointmentQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllPersonAppointment(PersonAppointmentQueryBuilder personAppointmentQueryBuilder, PersonAppointmentCondition condition = null, PersonAppointmentFilter filter = null, IEnumerable<PersonAppointmentSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllPersonAppointment", personAppointmentQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicKnmTypes(dicKNMTypesQueryBuilder dicKNMTypesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicKNMTypes", dicKNMTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicKnmTypes(dicKNMTypesQueryBuilder dicKNMTypesQueryBuilder, dicKNMTypesCondition condition = null, dicKNMTypesFilter filter = null, IEnumerable<dicKNMTypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicKNMTypes", dicKNMTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlJournals(ControlJournalsQueryBuilder controlJournalsQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlJournals", controlJournalsQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlJournals(ControlJournalsQueryBuilder controlJournalsQueryBuilder, ControlJournalsCondition condition = null, ControlJournalsFilter filter = null, IEnumerable<ControlJournalsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlJournals", controlJournalsQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlProgram(ControlProgramQueryBuilder controlProgramQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlProgram", controlProgramQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlProgram(ControlProgramQueryBuilder controlProgramQueryBuilder, ControlProgramCondition condition = null, ControlProgramFilter filter = null, IEnumerable<ControlProgramSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlProgram", controlProgramQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlFile(ControlFileQueryBuilder controlFileQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlFile", controlFileQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlFile(ControlFileQueryBuilder controlFileQueryBuilder, ControlFileCondition condition = null, ControlFileFilter filter = null, IEnumerable<ControlFileSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlFile", controlFileQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlObject", controlObjectQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder, ControlObjectCondition condition = null, ControlObjectFilter filter = null, IEnumerable<ControlObjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlObject", controlObjectQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicOktmo(dicOKTMOQueryBuilder dicOKTMOQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicOKTMO", dicOKTMOQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicOktmo(dicOKTMOQueryBuilder dicOKTMOQueryBuilder, dicOKTMOCondition condition = null, dicOKTMOFilter filter = null, IEnumerable<dicOKTMOSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicOKTMO", dicOKTMOQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetActivity(ActivityQueryBuilder activityQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getActivity", activityQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllActivity(ActivityQueryBuilder activityQueryBuilder, ActivityCondition condition = null, ActivityFilter filter = null, IEnumerable<ActivitySort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllActivity", activityQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicOkved(dicOKVEDQueryBuilder dicOKVEDQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicOKVED", dicOKVEDQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicOkved(dicOKVEDQueryBuilder dicOKVEDQueryBuilder, dicOKVEDCondition condition = null, dicOKVEDFilter filter = null, IEnumerable<dicOKVEDSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicOKVED", dicOKVEDQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetLicence(LicenceQueryBuilder licenceQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getLicence", licenceQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllLicence(LicenceQueryBuilder licenceQueryBuilder, LicenceCondition condition = null, LicenceFilter filter = null, IEnumerable<LicenceSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllLicence", licenceQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetControlItemPassport(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getControlItemPassport", controlItemPassportQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllControlItemPassport(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder, ControlItemPassportCondition condition = null, ControlItemPassportFilter filter = null, IEnumerable<ControlItemPassportSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllControlItemPassport", controlItemPassportQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetSubject(SubjectQueryBuilder subjectQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getSubject", subjectQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllSubject(SubjectQueryBuilder subjectQueryBuilder, SubjectCondition condition = null, SubjectFilter filter = null, IEnumerable<SubjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllSubject", subjectQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetCheckOutEgrip(CheckOutEGRIPQueryBuilder checkOutEGRIPQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getCheckOutEGRIP", checkOutEGRIPQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllCheckOutEgrip(CheckOutEGRIPQueryBuilder checkOutEGRIPQueryBuilder, CheckOutEGRIPCondition condition = null, CheckOutEGRIPFilter filter = null, IEnumerable<CheckOutEGRIPSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllCheckOutEGRIP", checkOutEGRIPQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetCheckOutEgrsmsp(CheckOutEGRSMSPQueryBuilder checkOutEGRSMSPQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getCheckOutEGRSMSP", checkOutEGRSMSPQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllCheckOutEgrsmsp(CheckOutEGRSMSPQueryBuilder checkOutEGRSMSPQueryBuilder, CheckOutEGRSMSPCondition condition = null, CheckOutEGRSMSPFilter filter = null, IEnumerable<CheckOutEGRSMSPSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllCheckOutEGRSMSP", checkOutEGRSMSPQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicNpaLevels(dicNPALevelsQueryBuilder dicNPALevelsQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicNPALevels", dicNPALevelsQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicNpaLevels(dicNPALevelsQueryBuilder dicNPALevelsQueryBuilder, dicNPALevelsCondition condition = null, dicNPALevelsFilter filter = null, IEnumerable<dicNPALevelsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicNPALevels", dicNPALevelsQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicNpaTypes(dicNPATypesQueryBuilder dicNPATypesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicNPATypes", dicNPATypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicNpaTypes(dicNPATypesQueryBuilder dicNPATypesQueryBuilder, dicNPATypesCondition condition = null, dicNPATypesFilter filter = null, IEnumerable<dicNPATypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicNPATypes", dicNPATypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetFile(FileQueryBuilder fileQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getFile", fileQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllFile(FileQueryBuilder fileQueryBuilder, FileCondition condition = null, FileFilter filter = null, IEnumerable<FileSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllFile", fileQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetDicMeasureUnitsType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getDicMeasureUnitsType", dicMeasureUnitsTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllDicMeasureUnitsType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder, DicMeasureUnitsTypeCondition condition = null, DicMeasureUnitsTypeFilter filter = null, IEnumerable<DicMeasureUnitsTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllDicMeasureUnitsType", dicMeasureUnitsTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetDicControlItemBaseType(DicControlItemBaseTypeQueryBuilder dicControlItemBaseTypeQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getDicControlItemBaseType", dicControlItemBaseTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllDicControlItemBaseType(DicControlItemBaseTypeQueryBuilder dicControlItemBaseTypeQueryBuilder, DicControlItemBaseTypeCondition condition = null, DicControlItemBaseTypeFilter filter = null, IEnumerable<DicControlItemBaseTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllDicControlItemBaseType", dicControlItemBaseTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetDicControlPlanType(DicControlPlanTypeQueryBuilder dicControlPlanTypeQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getDicControlPlanType", dicControlPlanTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllDicControlPlanType(DicControlPlanTypeQueryBuilder dicControlPlanTypeQueryBuilder, DicControlPlanTypeCondition condition = null, DicControlPlanTypeFilter filter = null, IEnumerable<DicControlPlanTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllDicControlPlanType", dicControlPlanTypeQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetOrganizationUnit(OrganizationUnitQueryBuilder organizationUnitQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getOrganizationUnit", organizationUnitQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllOrganizationUnit(OrganizationUnitQueryBuilder organizationUnitQueryBuilder, OrganizationUnitCondition condition = null, OrganizationUnitFilter filter = null, IEnumerable<OrganizationUnitSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllOrganizationUnit", organizationUnitQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicLicencedActivityTypes(dicLicencedActivityTypesQueryBuilder dicLicencedActivityTypesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicLicencedActivityTypes", dicLicencedActivityTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicLicencedActivityTypes(dicLicencedActivityTypesQueryBuilder dicLicencedActivityTypesQueryBuilder, dicLicencedActivityTypesCondition condition = null, dicLicencedActivityTypesFilter filter = null, IEnumerable<dicLicencedActivityTypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicLicencedActivityTypes", dicLicencedActivityTypesQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicHazardClass(dicHazardClassQueryBuilder dicHazardClassQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicHazardClass", dicHazardClassQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicHazardClass(dicHazardClassQueryBuilder dicHazardClassQueryBuilder, dicHazardClassCondition condition = null, dicHazardClassFilter filter = null, IEnumerable<dicHazardClassSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicHazardClass", dicHazardClassQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetdicRiskCategory(dicRiskCategoryQueryBuilder dicRiskCategoryQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getdicRiskCategory", dicRiskCategoryQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAlldicRiskCategory(dicRiskCategoryQueryBuilder dicRiskCategoryQueryBuilder, dicRiskCategoryCondition condition = null, dicRiskCategoryFilter filter = null, IEnumerable<dicRiskCategorySort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAlldicRiskCategory", dicRiskCategoryQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetExtendedAttribValue(ExtendedAttribValueQueryBuilder extendedAttribValueQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getExtendedAttribValue", extendedAttribValueQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllExtendedAttribValue(ExtendedAttribValueQueryBuilder extendedAttribValueQueryBuilder, ExtendedAttribValueCondition condition = null, ExtendedAttribValueFilter filter = null, IEnumerable<ExtendedAttribValueSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllExtendedAttribValue", extendedAttribValueQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetHazardClass(HazardClassQueryBuilder hazardClassQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getHazardClass", hazardClassQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllHazardClass(HazardClassQueryBuilder hazardClassQueryBuilder, HazardClassCondition condition = null, HazardClassFilter filter = null, IEnumerable<HazardClassSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllHazardClass", hazardClassQueryBuilder, args);
        }

        public QueryQueryBuilder WithGetDicStates(DicStatesQueryBuilder dicStatesQueryBuilder, Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithObjectField("getDicStates", dicStatesQueryBuilder, args);
        }

        public QueryQueryBuilder WithFindAllDicStates(DicStatesQueryBuilder dicStatesQueryBuilder, DicStatesCondition condition = null, DicStatesFilter filter = null, IEnumerable<DicStatesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("findAllDicStates", dicStatesQueryBuilder, args);
        }
    }

    public partial class MaterialsQueryBuilder : GraphQlQueryBuilder<MaterialsQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "MaterialName" },
                new FieldMetadata { Name = "Subject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "LicenceFile", IsComplex = true, QueryBuilderType = typeof(LicenceQueryBuilder) },
                new FieldMetadata { Name = "ControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "ControlProgramFile", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "ResultsFile", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "CitizenRequest", IsComplex = true, QueryBuilderType = typeof(CitizenRequestQueryBuilder) },
                new FieldMetadata { Name = "ControlActFile", IsComplex = true, QueryBuilderType = typeof(docControlActQueryBuilder) },
                new FieldMetadata { Name = "ControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "ControlOrderFile", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "LinkedFile", IsComplex = true, QueryBuilderType = typeof(FileQueryBuilder) },
                new FieldMetadata { Name = "docRegulation", IsComplex = true, QueryBuilderType = typeof(docRegulationQueryBuilder) },
                new FieldMetadata { Name = "docControlPlan", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "docProcStatement", IsComplex = true, QueryBuilderType = typeof(docProcStatementQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public MaterialsQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public MaterialsQueryBuilder WithMaterialName()
        {
            return WithScalarField("MaterialName");
        }

        public MaterialsQueryBuilder WithSubject(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("Subject", subjectQueryBuilder);
        }

        public MaterialsQueryBuilder WithLicenceFile(LicenceQueryBuilder licenceQueryBuilder)
        {
            return WithObjectField("LicenceFile", licenceQueryBuilder);
        }

        public MaterialsQueryBuilder WithControlFile(ControlFileQueryBuilder controlFileQueryBuilder)
        {
            return WithObjectField("ControlFile", controlFileQueryBuilder);
        }

        public MaterialsQueryBuilder WithControlProgramFile(ControlProgramQueryBuilder controlProgramQueryBuilder)
        {
            return WithObjectField("ControlProgramFile", controlProgramQueryBuilder);
        }

        public MaterialsQueryBuilder WithResultsFile(ControlItemResultQueryBuilder controlItemResultQueryBuilder)
        {
            return WithObjectField("ResultsFile", controlItemResultQueryBuilder);
        }

        public MaterialsQueryBuilder WithCitizenRequest(CitizenRequestQueryBuilder citizenRequestQueryBuilder)
        {
            return WithObjectField("CitizenRequest", citizenRequestQueryBuilder);
        }

        public MaterialsQueryBuilder WithControlActFile(docControlActQueryBuilder docControlActQueryBuilder)
        {
            return WithObjectField("ControlActFile", docControlActQueryBuilder);
        }

        public MaterialsQueryBuilder WithControlList(docControlListQueryBuilder docControlListQueryBuilder)
        {
            return WithObjectField("ControlList", docControlListQueryBuilder);
        }

        public MaterialsQueryBuilder WithControlOrderFile(docControlOrderQueryBuilder docControlOrderQueryBuilder)
        {
            return WithObjectField("ControlOrderFile", docControlOrderQueryBuilder);
        }

        public MaterialsQueryBuilder WithLinkedFile(FileQueryBuilder fileQueryBuilder)
        {
            return WithObjectField("LinkedFile", fileQueryBuilder);
        }

        public MaterialsQueryBuilder WithDocRegulation(docRegulationQueryBuilder docRegulationQueryBuilder)
        {
            return WithObjectField("docRegulation", docRegulationQueryBuilder);
        }

        public MaterialsQueryBuilder WithDocControlPlan(docControlPlanQueryBuilder docControlPlanQueryBuilder)
        {
            return WithObjectField("docControlPlan", docControlPlanQueryBuilder);
        }

        public MaterialsQueryBuilder WithDocProcStatement(docProcStatementQueryBuilder docProcStatementQueryBuilder)
        {
            return WithObjectField("docProcStatement", docProcStatementQueryBuilder);
        }

        public MaterialsQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public MaterialsQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicOKTMOQueryBuilder : GraphQlQueryBuilder<dicOKTMOQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "CodeOKTMO" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicOKTMOQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicOKTMOQueryBuilder WithCodeOktmo()
        {
            return WithScalarField("CodeOKTMO");
        }

        public dicOKTMOQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicOKTMOQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class DicControlItemBaseTypeQueryBuilder : GraphQlQueryBuilder<DicControlItemBaseTypeQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlItemBaseName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public DicControlItemBaseTypeQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public DicControlItemBaseTypeQueryBuilder WithControlItemBaseName()
        {
            return WithScalarField("ControlItemBaseName");
        }

        public DicControlItemBaseTypeQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public DicControlItemBaseTypeQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class DicControlPlanTypeQueryBuilder : GraphQlQueryBuilder<DicControlPlanTypeQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlPlanType" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public DicControlPlanTypeQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public DicControlPlanTypeQueryBuilder WithControlPlanType()
        {
            return WithScalarField("ControlPlanType");
        }

        public DicControlPlanTypeQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public DicControlPlanTypeQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class CheckOutEGRULQueryBuilder : GraphQlQueryBuilder<CheckOutEGRULQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "LastRenewData", IsComplex = true },
                new FieldMetadata { Name = "dateUpdateEGRUL", IsComplex = true },
                new FieldMetadata { Name = "OGRNUL" },
                new FieldMetadata { Name = "OGRNULdata", IsComplex = true },
                new FieldMetadata { Name = "INNUL" },
                new FieldMetadata { Name = "dateRegYL", IsComplex = true },
                new FieldMetadata { Name = "orgNameRegYL" },
                new FieldMetadata { Name = "addressRegOrgYL" },
                new FieldMetadata { Name = "grnOrgLocationYL" },
                new FieldMetadata { Name = "grnDateOrgLocationYL", IsComplex = true },
                new FieldMetadata { Name = "dateTaxReg", IsComplex = true },
                new FieldMetadata { Name = "taxOrg" },
                new FieldMetadata { Name = "grnTaxOrg" },
                new FieldMetadata { Name = "dateGRNTaxOrg", IsComplex = true },
                new FieldMetadata { Name = "RegNumberPFRYL" },
                new FieldMetadata { Name = "dateRegNumPFRYL", IsComplex = true },
                new FieldMetadata { Name = "branchPFRYL" },
                new FieldMetadata { Name = "GrnRegPFRYL" },
                new FieldMetadata { Name = "dateGrnRegPFRYL", IsComplex = true },
                new FieldMetadata { Name = "grnRegExecPFRYL" },
                new FieldMetadata { Name = "regNumberExecPFRYL" },
                new FieldMetadata { Name = "dateRegNumExecPFRYL", IsComplex = true },
                new FieldMetadata { Name = "branchExecSSYL" },
                new FieldMetadata { Name = "grnRegExecSSYL" },
                new FieldMetadata { Name = "dateGrnExecSSYL", IsComplex = true },
                new FieldMetadata { Name = "typeCharterCapital" },
                new FieldMetadata { Name = "sumCharterCapital" },
                new FieldMetadata { Name = "grnCharterCapital" },
                new FieldMetadata { Name = "dateGRNCharterCapital", IsComplex = true },
                new FieldMetadata { Name = "RepresentativeName" },
                new FieldMetadata { Name = "RepresentativeSecondName" },
                new FieldMetadata { Name = "RepresentativeSurname" },
                new FieldMetadata { Name = "KPP" },
                new FieldMetadata { Name = "ShortNameOrg" },
                new FieldMetadata { Name = "SubjectEGRUL", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "Address", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "CodeOKOPF", IsComplex = true, QueryBuilderType = typeof(dicOKOPFQueryBuilder) },
                new FieldMetadata { Name = "Citizenships", IsComplex = true, QueryBuilderType = typeof(dicOKSMQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public CheckOutEGRULQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public CheckOutEGRULQueryBuilder WithLastRenewData()
        {
            return WithScalarField("LastRenewData");
        }

        public CheckOutEGRULQueryBuilder WithDateUpdateEgrul()
        {
            return WithScalarField("dateUpdateEGRUL");
        }

        public CheckOutEGRULQueryBuilder WithOgrnul()
        {
            return WithScalarField("OGRNUL");
        }

        public CheckOutEGRULQueryBuilder WithOgrnuLdata()
        {
            return WithScalarField("OGRNULdata");
        }

        public CheckOutEGRULQueryBuilder WithInnul()
        {
            return WithScalarField("INNUL");
        }

        public CheckOutEGRULQueryBuilder WithDateRegYl()
        {
            return WithScalarField("dateRegYL");
        }

        public CheckOutEGRULQueryBuilder WithOrgNameRegYl()
        {
            return WithScalarField("orgNameRegYL");
        }

        public CheckOutEGRULQueryBuilder WithAddressRegOrgYl()
        {
            return WithScalarField("addressRegOrgYL");
        }

        public CheckOutEGRULQueryBuilder WithGrnOrgLocationYl()
        {
            return WithScalarField("grnOrgLocationYL");
        }

        public CheckOutEGRULQueryBuilder WithGrnDateOrgLocationYl()
        {
            return WithScalarField("grnDateOrgLocationYL");
        }

        public CheckOutEGRULQueryBuilder WithDateTaxReg()
        {
            return WithScalarField("dateTaxReg");
        }

        public CheckOutEGRULQueryBuilder WithTaxOrg()
        {
            return WithScalarField("taxOrg");
        }

        public CheckOutEGRULQueryBuilder WithGrnTaxOrg()
        {
            return WithScalarField("grnTaxOrg");
        }

        public CheckOutEGRULQueryBuilder WithDateGrnTaxOrg()
        {
            return WithScalarField("dateGRNTaxOrg");
        }

        public CheckOutEGRULQueryBuilder WithRegNumberPfryl()
        {
            return WithScalarField("RegNumberPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithDateRegNumPfryl()
        {
            return WithScalarField("dateRegNumPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithBranchPfryl()
        {
            return WithScalarField("branchPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithGrnRegPfryl()
        {
            return WithScalarField("GrnRegPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithDateGrnRegPfryl()
        {
            return WithScalarField("dateGrnRegPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithGrnRegExecPfryl()
        {
            return WithScalarField("grnRegExecPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithRegNumberExecPfryl()
        {
            return WithScalarField("regNumberExecPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithDateRegNumExecPfryl()
        {
            return WithScalarField("dateRegNumExecPFRYL");
        }

        public CheckOutEGRULQueryBuilder WithBranchExecSsyl()
        {
            return WithScalarField("branchExecSSYL");
        }

        public CheckOutEGRULQueryBuilder WithGrnRegExecSsyl()
        {
            return WithScalarField("grnRegExecSSYL");
        }

        public CheckOutEGRULQueryBuilder WithDateGrnExecSsyl()
        {
            return WithScalarField("dateGrnExecSSYL");
        }

        public CheckOutEGRULQueryBuilder WithTypeCharterCapital()
        {
            return WithScalarField("typeCharterCapital");
        }

        public CheckOutEGRULQueryBuilder WithSumCharterCapital()
        {
            return WithScalarField("sumCharterCapital");
        }

        public CheckOutEGRULQueryBuilder WithGrnCharterCapital()
        {
            return WithScalarField("grnCharterCapital");
        }

        public CheckOutEGRULQueryBuilder WithDateGrnCharterCapital()
        {
            return WithScalarField("dateGRNCharterCapital");
        }

        public CheckOutEGRULQueryBuilder WithRepresentativeName()
        {
            return WithScalarField("RepresentativeName");
        }

        public CheckOutEGRULQueryBuilder WithRepresentativeSecondName()
        {
            return WithScalarField("RepresentativeSecondName");
        }

        public CheckOutEGRULQueryBuilder WithRepresentativeSurname()
        {
            return WithScalarField("RepresentativeSurname");
        }

        public CheckOutEGRULQueryBuilder WithKpp()
        {
            return WithScalarField("KPP");
        }

        public CheckOutEGRULQueryBuilder WithShortNameOrg()
        {
            return WithScalarField("ShortNameOrg");
        }

        public CheckOutEGRULQueryBuilder WithSubjectEgrul(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("SubjectEGRUL", subjectQueryBuilder);
        }

        public CheckOutEGRULQueryBuilder WithAddress(AddressQueryBuilder addressQueryBuilder)
        {
            return WithObjectField("Address", addressQueryBuilder);
        }

        public CheckOutEGRULQueryBuilder WithCodeOkopf(dicOKOPFQueryBuilder dicOKOPFQueryBuilder)
        {
            return WithObjectField("CodeOKOPF", dicOKOPFQueryBuilder);
        }

        public CheckOutEGRULQueryBuilder WithCitizenships(dicOKSMQueryBuilder dicOKSMQueryBuilder)
        {
            return WithObjectField("Citizenships", dicOKSMQueryBuilder);
        }

        public CheckOutEGRULQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public CheckOutEGRULQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicStatusPlanQueryBuilder : GraphQlQueryBuilder<dicStatusPlanQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "StatusPlanName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicStatusPlanQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicStatusPlanQueryBuilder WithStatusPlanName()
        {
            return WithScalarField("StatusPlanName");
        }

        public dicStatusPlanQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicStatusPlanQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class DicMeasureUnitsTypeQueryBuilder : GraphQlQueryBuilder<DicMeasureUnitsTypeQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "MeasureUnitTypeName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public DicMeasureUnitsTypeQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public DicMeasureUnitsTypeQueryBuilder WithMeasureUnitTypeName()
        {
            return WithScalarField("MeasureUnitTypeName");
        }

        public DicMeasureUnitsTypeQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public DicMeasureUnitsTypeQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicStatusKNMQueryBuilder : GraphQlQueryBuilder<dicStatusKNMQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "StatusKNMName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicStatusKNMQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicStatusKNMQueryBuilder WithStatusKnmName()
        {
            return WithScalarField("StatusKNMName");
        }

        public dicStatusKNMQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicStatusKNMQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicSubjectTypeQueryBuilder : GraphQlQueryBuilder<dicSubjectTypeQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "SubjectTypeName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicSubjectTypeQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicSubjectTypeQueryBuilder WithSubjectTypeName()
        {
            return WithScalarField("SubjectTypeName");
        }

        public dicSubjectTypeQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicSubjectTypeQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicRoleQueryBuilder : GraphQlQueryBuilder<dicRoleQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "RoleName" },
                new FieldMetadata { Name = "ControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicRoleQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicRoleQueryBuilder WithRoleName()
        {
            return WithScalarField("RoleName");
        }

        public dicRoleQueryBuilder WithControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder)
        {
            return WithObjectField("ControlOrganization", controlOrganizationQueryBuilder);
        }

        public dicRoleQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicRoleQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicLicenceStatusQueryBuilder : GraphQlQueryBuilder<dicLicenceStatusQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "LicenceStatusName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicLicenceStatusQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicLicenceStatusQueryBuilder WithLicenceStatusName()
        {
            return WithScalarField("LicenceStatusName");
        }

        public dicLicenceStatusQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicLicenceStatusQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class JournalAttributesQueryBuilder : GraphQlQueryBuilder<JournalAttributesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "JournalAttirbuteName" },
                new FieldMetadata { Name = "JournalAttributeValue" },
                new FieldMetadata { Name = "ControlJournals", IsComplex = true, QueryBuilderType = typeof(ControlJournalsQueryBuilder) },
                new FieldMetadata { Name = "MeasureUnitType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public JournalAttributesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public JournalAttributesQueryBuilder WithJournalAttirbuteName()
        {
            return WithScalarField("JournalAttirbuteName");
        }

        public JournalAttributesQueryBuilder WithJournalAttributeValue()
        {
            return WithScalarField("JournalAttributeValue");
        }

        public JournalAttributesQueryBuilder WithControlJournals(ControlJournalsQueryBuilder controlJournalsQueryBuilder)
        {
            return WithObjectField("ControlJournals", controlJournalsQueryBuilder);
        }

        public JournalAttributesQueryBuilder WithMeasureUnitType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder)
        {
            return WithObjectField("MeasureUnitType", dicMeasureUnitsTypeQueryBuilder);
        }

        public JournalAttributesQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public JournalAttributesQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class docProcStatementQueryBuilder : GraphQlQueryBuilder<docProcStatementQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "SendToProcDate", IsComplex = true },
                new FieldMetadata { Name = "ProcApproveFIO" },
                new FieldMetadata { Name = "ProcApproveRole" },
                new FieldMetadata { Name = "ProcApprovePlace" },
                new FieldMetadata { Name = "LinkedControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "docControlOrder", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public docProcStatementQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public docProcStatementQueryBuilder WithSendToProcDate()
        {
            return WithScalarField("SendToProcDate");
        }

        public docProcStatementQueryBuilder WithProcApproveFio()
        {
            return WithScalarField("ProcApproveFIO");
        }

        public docProcStatementQueryBuilder WithProcApproveRole()
        {
            return WithScalarField("ProcApproveRole");
        }

        public docProcStatementQueryBuilder WithProcApprovePlace()
        {
            return WithScalarField("ProcApprovePlace");
        }

        public docProcStatementQueryBuilder WithLinkedControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("LinkedControlCard", controlCardQueryBuilder);
        }

        public docProcStatementQueryBuilder WithDocControlOrder(docControlOrderQueryBuilder docControlOrderQueryBuilder)
        {
            return WithObjectField("docControlOrder", docControlOrderQueryBuilder);
        }

        public docProcStatementQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder)
        {
            return WithObjectField("Materials", materialsQueryBuilder);
        }

        public docProcStatementQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public docProcStatementQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlCardQueryBuilder : GraphQlQueryBuilder<ControlCardQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ProcDataAgreement", IsComplex = true },
                new FieldMetadata { Name = "ControlStartDate", IsComplex = true },
                new FieldMetadata { Name = "ControlStatus" },
                new FieldMetadata { Name = "ControlDuration" },
                new FieldMetadata { Name = "DurationProlong" },
                new FieldMetadata { Name = "ControlEndDate", IsComplex = true },
                new FieldMetadata { Name = "ControlPurpose" },
                new FieldMetadata { Name = "IsJoint" },
                new FieldMetadata { Name = "NumberFGISERP" },
                new FieldMetadata { Name = "FGISERPRegData", IsComplex = true },
                new FieldMetadata { Name = "LastEndControlDate", IsComplex = true },
                new FieldMetadata { Name = "CheckControlRestrict" },
                new FieldMetadata { Name = "ControlCancelInfo" },
                new FieldMetadata { Name = "InternalNumberFGISERP" },
                new FieldMetadata { Name = "ControlFactStartDate", IsComplex = true },
                new FieldMetadata { Name = "ControlFactEndDate", IsComplex = true },
                new FieldMetadata { Name = "FactControlPeriod" },
                new FieldMetadata { Name = "FactControlPeriodUnit" },
                new FieldMetadata { Name = "JointControlOrganization" },
                new FieldMetadata { Name = "ControlItemResults", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "ProcedureStatements", IsComplex = true, QueryBuilderType = typeof(docProcStatementQueryBuilder) },
                new FieldMetadata { Name = "ControlItemPassport", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "ControlActs", IsComplex = true, QueryBuilderType = typeof(docControlActQueryBuilder) },
                new FieldMetadata { Name = "MeasureUnitType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "ControlForm", IsComplex = true, QueryBuilderType = typeof(dicKNMFormQueryBuilder) },
                new FieldMetadata { Name = "ControlPlan", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "ControlBase", IsComplex = true, QueryBuilderType = typeof(dicControlBaseQueryBuilder) },
                new FieldMetadata { Name = "ControlReasonDeny", IsComplex = true, QueryBuilderType = typeof(dicControlReasonDenyQueryBuilder) },
                new FieldMetadata { Name = "Prosec", IsComplex = true, QueryBuilderType = typeof(dicProsecQueryBuilder) },
                new FieldMetadata { Name = "ControlItemBaseType", IsComplex = true, QueryBuilderType = typeof(DicControlItemBaseTypeQueryBuilder) },
                new FieldMetadata { Name = "ControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "ControlOrders", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "Regulations", IsComplex = true, QueryBuilderType = typeof(docRegulationQueryBuilder) },
                new FieldMetadata { Name = "ControlCardPersons", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlCardQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlCardQueryBuilder WithProcDataAgreement()
        {
            return WithScalarField("ProcDataAgreement");
        }

        public ControlCardQueryBuilder WithControlStartDate()
        {
            return WithScalarField("ControlStartDate");
        }

        public ControlCardQueryBuilder WithControlStatus()
        {
            return WithScalarField("ControlStatus");
        }

        public ControlCardQueryBuilder WithControlDuration()
        {
            return WithScalarField("ControlDuration");
        }

        public ControlCardQueryBuilder WithDurationProlong()
        {
            return WithScalarField("DurationProlong");
        }

        public ControlCardQueryBuilder WithControlEndDate()
        {
            return WithScalarField("ControlEndDate");
        }

        public ControlCardQueryBuilder WithControlPurpose()
        {
            return WithScalarField("ControlPurpose");
        }

        public ControlCardQueryBuilder WithIsJoint()
        {
            return WithScalarField("IsJoint");
        }

        public ControlCardQueryBuilder WithNumberFgiserp()
        {
            return WithScalarField("NumberFGISERP");
        }

        public ControlCardQueryBuilder WithFgiserpRegData()
        {
            return WithScalarField("FGISERPRegData");
        }

        public ControlCardQueryBuilder WithLastEndControlDate()
        {
            return WithScalarField("LastEndControlDate");
        }

        public ControlCardQueryBuilder WithCheckControlRestrict()
        {
            return WithScalarField("CheckControlRestrict");
        }

        public ControlCardQueryBuilder WithControlCancelInfo()
        {
            return WithScalarField("ControlCancelInfo");
        }

        public ControlCardQueryBuilder WithInternalNumberFgiserp()
        {
            return WithScalarField("InternalNumberFGISERP");
        }

        public ControlCardQueryBuilder WithControlFactStartDate()
        {
            return WithScalarField("ControlFactStartDate");
        }

        public ControlCardQueryBuilder WithControlFactEndDate()
        {
            return WithScalarField("ControlFactEndDate");
        }

        public ControlCardQueryBuilder WithFactControlPeriod()
        {
            return WithScalarField("FactControlPeriod");
        }

        public ControlCardQueryBuilder WithFactControlPeriodUnit()
        {
            return WithScalarField("FactControlPeriodUnit");
        }

        public ControlCardQueryBuilder WithJointControlOrganization()
        {
            return WithScalarField("JointControlOrganization");
        }

        public ControlCardQueryBuilder WithControlItemResults(ControlItemResultQueryBuilder controlItemResultQueryBuilder, ControlItemResultCondition condition = null, ControlItemResultFilter filter = null, IEnumerable<ControlItemResultSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItemResults", controlItemResultQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithProcedureStatements(docProcStatementQueryBuilder docProcStatementQueryBuilder, docProcStatementCondition condition = null, docProcStatementFilter filter = null, IEnumerable<docProcStatementSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ProcedureStatements", docProcStatementQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithControlItemPassport(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder)
        {
            return WithObjectField("ControlItemPassport", controlItemPassportQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlActs(docControlActQueryBuilder docControlActQueryBuilder, docControlActCondition condition = null, docControlActFilter filter = null, IEnumerable<docControlActSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlActs", docControlActQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithMeasureUnitType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder)
        {
            return WithObjectField("MeasureUnitType", dicMeasureUnitsTypeQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlForm(dicKNMFormQueryBuilder dicKNMFormQueryBuilder)
        {
            return WithObjectField("ControlForm", dicKNMFormQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlPlan(docControlPlanQueryBuilder docControlPlanQueryBuilder)
        {
            return WithObjectField("ControlPlan", docControlPlanQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlBase(dicControlBaseQueryBuilder dicControlBaseQueryBuilder)
        {
            return WithObjectField("ControlBase", dicControlBaseQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlReasonDeny(dicControlReasonDenyQueryBuilder dicControlReasonDenyQueryBuilder)
        {
            return WithObjectField("ControlReasonDeny", dicControlReasonDenyQueryBuilder);
        }

        public ControlCardQueryBuilder WithProsec(dicProsecQueryBuilder dicProsecQueryBuilder)
        {
            return WithObjectField("Prosec", dicProsecQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlItemBaseType(DicControlItemBaseTypeQueryBuilder dicControlItemBaseTypeQueryBuilder)
        {
            return WithObjectField("ControlItemBaseType", dicControlItemBaseTypeQueryBuilder);
        }

        public ControlCardQueryBuilder WithControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsCondition condition = null, ControlListQuestionsFilter filter = null, IEnumerable<ControlListQuestionsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithControlOrders(docControlOrderQueryBuilder docControlOrderQueryBuilder, docControlOrderCondition condition = null, docControlOrderFilter filter = null, IEnumerable<docControlOrderSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlOrders", docControlOrderQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithRegulations(docRegulationQueryBuilder docRegulationQueryBuilder, docRegulationCondition condition = null, docRegulationFilter filter = null, IEnumerable<docRegulationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Regulations", docRegulationQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithControlCardPersons(PersonQueryBuilder personQueryBuilder, PersonCondition condition = null, PersonFilter filter = null, IEnumerable<PersonSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlCardPersons", personQueryBuilder, args);
        }

        public ControlCardQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlCardQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlItemQueryBuilder : GraphQlQueryBuilder<ControlItemQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlItemName" },
                new FieldMetadata { Name = "ControlDate", IsComplex = true },
                new FieldMetadata { Name = "ControlItemResult" },
                new FieldMetadata { Name = "ControlItemPersons", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "ControlProgram", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlItemQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlItemQueryBuilder WithControlItemName()
        {
            return WithScalarField("ControlItemName");
        }

        public ControlItemQueryBuilder WithControlDate()
        {
            return WithScalarField("ControlDate");
        }

        public ControlItemQueryBuilder WithControlItemResult()
        {
            return WithScalarField("ControlItemResult");
        }

        public ControlItemQueryBuilder WithControlItemPersons(PersonQueryBuilder personQueryBuilder, PersonCondition condition = null, PersonFilter filter = null, IEnumerable<PersonSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItemPersons", personQueryBuilder, args);
        }

        public ControlItemQueryBuilder WithControlProgram(ControlProgramQueryBuilder controlProgramQueryBuilder)
        {
            return WithObjectField("ControlProgram", controlProgramQueryBuilder);
        }

        public ControlItemQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlItemQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class docControlOrderQueryBuilder : GraphQlQueryBuilder<docControlOrderQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlOrderCreateDate", IsComplex = true },
                new FieldMetadata { Name = "ControlOrderSignDate", IsComplex = true },
                new FieldMetadata { Name = "ControlOrderNumber" },
                new FieldMetadata { Name = "ControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public docControlOrderQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public docControlOrderQueryBuilder WithControlOrderCreateDate()
        {
            return WithScalarField("ControlOrderCreateDate");
        }

        public docControlOrderQueryBuilder WithControlOrderSignDate()
        {
            return WithScalarField("ControlOrderSignDate");
        }

        public docControlOrderQueryBuilder WithControlOrderNumber()
        {
            return WithScalarField("ControlOrderNumber");
        }

        public docControlOrderQueryBuilder WithControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("ControlCard", controlCardQueryBuilder);
        }

        public docControlOrderQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder)
        {
            return WithObjectField("Materials", materialsQueryBuilder);
        }

        public docControlOrderQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public docControlOrderQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ViolationQueryBuilder : GraphQlQueryBuilder<ViolationQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ViolationSpecificComment" },
                new FieldMetadata { Name = "ControlItemResult", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "ViolationType", IsComplex = true, QueryBuilderType = typeof(dicViolationTypesQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ViolationQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ViolationQueryBuilder WithViolationSpecificComment()
        {
            return WithScalarField("ViolationSpecificComment");
        }

        public ViolationQueryBuilder WithControlItemResult(ControlItemResultQueryBuilder controlItemResultQueryBuilder)
        {
            return WithObjectField("ControlItemResult", controlItemResultQueryBuilder);
        }

        public ViolationQueryBuilder WithViolationType(dicViolationTypesQueryBuilder dicViolationTypesQueryBuilder)
        {
            return WithObjectField("ViolationType", dicViolationTypesQueryBuilder);
        }

        public ViolationQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ViolationQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class AddressQueryBuilder : GraphQlQueryBuilder<AddressQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "CodeKLADR" },
                new FieldMetadata { Name = "CodeFIAS" },
                new FieldMetadata { Name = "Address" },
                new FieldMetadata { Name = "PostIndex" },
                new FieldMetadata { Name = "AddressFact" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public AddressQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public AddressQueryBuilder WithCodeKladr()
        {
            return WithScalarField("CodeKLADR");
        }

        public AddressQueryBuilder WithCodeFias()
        {
            return WithScalarField("CodeFIAS");
        }

        public AddressQueryBuilder WithAddress()
        {
            return WithScalarField("Address");
        }

        public AddressQueryBuilder WithPostIndex()
        {
            return WithScalarField("PostIndex");
        }

        public AddressQueryBuilder WithAddressFact()
        {
            return WithScalarField("AddressFact");
        }

        public AddressQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public AddressQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class RiskCatQueryBuilder : GraphQlQueryBuilder<RiskCatQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "CategoryClassLevel" },
                new FieldMetadata { Name = "RiskCatStartDate", IsComplex = true },
                new FieldMetadata { Name = "RiskCatEndDate", IsComplex = true },
                new FieldMetadata { Name = "ControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "RiskCategory", IsComplex = true, QueryBuilderType = typeof(dicRiskCategoryQueryBuilder) },
                new FieldMetadata { Name = "Subject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public RiskCatQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public RiskCatQueryBuilder WithCategoryClassLevel()
        {
            return WithScalarField("CategoryClassLevel");
        }

        public RiskCatQueryBuilder WithRiskCatStartDate()
        {
            return WithScalarField("RiskCatStartDate");
        }

        public RiskCatQueryBuilder WithRiskCatEndDate()
        {
            return WithScalarField("RiskCatEndDate");
        }

        public RiskCatQueryBuilder WithControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder)
        {
            return WithObjectField("ControlObject", controlObjectQueryBuilder);
        }

        public RiskCatQueryBuilder WithRiskCategory(dicRiskCategoryQueryBuilder dicRiskCategoryQueryBuilder)
        {
            return WithObjectField("RiskCategory", dicRiskCategoryQueryBuilder);
        }

        public RiskCatQueryBuilder WithSubject(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("Subject", subjectQueryBuilder);
        }

        public RiskCatQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public RiskCatQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicKNMFormQueryBuilder : GraphQlQueryBuilder<dicKNMFormQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "KNMFormName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicKNMFormQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicKNMFormQueryBuilder WithKnmFormName()
        {
            return WithScalarField("KNMFormName");
        }

        public dicKNMFormQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicKNMFormQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class PersonQueryBuilder : GraphQlQueryBuilder<PersonQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "PersonName" },
                new FieldMetadata { Name = "PersonSurName" },
                new FieldMetadata { Name = "PersonSecondName" },
                new FieldMetadata { Name = "ControlItems", IsComplex = true, QueryBuilderType = typeof(ControlItemQueryBuilder) },
                new FieldMetadata { Name = "SubjectResponsibility", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "ControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "ControlPlans", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "RoleName", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "PersonAppointment", IsComplex = true, QueryBuilderType = typeof(PersonAppointmentQueryBuilder) },
                new FieldMetadata { Name = "ControlCards", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public PersonQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public PersonQueryBuilder WithPersonName()
        {
            return WithScalarField("PersonName");
        }

        public PersonQueryBuilder WithPersonSurName()
        {
            return WithScalarField("PersonSurName");
        }

        public PersonQueryBuilder WithPersonSecondName()
        {
            return WithScalarField("PersonSecondName");
        }

        public PersonQueryBuilder WithControlItems(ControlItemQueryBuilder controlItemQueryBuilder, ControlItemCondition condition = null, ControlItemFilter filter = null, IEnumerable<ControlItemSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItems", controlItemQueryBuilder, args);
        }

        public PersonQueryBuilder WithSubjectResponsibility(SubjectQueryBuilder subjectQueryBuilder, SubjectCondition condition = null, SubjectFilter filter = null, IEnumerable<SubjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("SubjectResponsibility", subjectQueryBuilder, args);
        }

        public PersonQueryBuilder WithControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder)
        {
            return WithObjectField("ControlOrganization", controlOrganizationQueryBuilder);
        }

        public PersonQueryBuilder WithControlPlans(docControlPlanQueryBuilder docControlPlanQueryBuilder, docControlPlanCondition condition = null, docControlPlanFilter filter = null, IEnumerable<docControlPlanSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlPlans", docControlPlanQueryBuilder, args);
        }

        public PersonQueryBuilder WithRoleName(dicRoleQueryBuilder dicRoleQueryBuilder)
        {
            return WithObjectField("RoleName", dicRoleQueryBuilder);
        }

        public PersonQueryBuilder WithPersonAppointment(PersonAppointmentQueryBuilder personAppointmentQueryBuilder, PersonAppointmentCondition condition = null, PersonAppointmentFilter filter = null, IEnumerable<PersonAppointmentSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("PersonAppointment", personAppointmentQueryBuilder, args);
        }

        public PersonQueryBuilder WithControlCards(ControlCardQueryBuilder controlCardQueryBuilder, ControlCardCondition condition = null, ControlCardFilter filter = null, IEnumerable<ControlCardSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlCards", controlCardQueryBuilder, args);
        }

        public PersonQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public PersonQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicProcStatmentStatusQueryBuilder : GraphQlQueryBuilder<dicProcStatmentStatusQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ProcStatementStatusName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicProcStatmentStatusQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicProcStatmentStatusQueryBuilder WithProcStatementStatusName()
        {
            return WithScalarField("ProcStatementStatusName");
        }

        public dicProcStatmentStatusQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicProcStatmentStatusQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class RandEParameterValueQueryBuilder : GraphQlQueryBuilder<RandEParameterValueQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ParameterName" },
                new FieldMetadata { Name = "ParameterValue" },
                new FieldMetadata { Name = "RandEParameter", IsComplex = true, QueryBuilderType = typeof(RandEParameterQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public RandEParameterValueQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public RandEParameterValueQueryBuilder WithParameterName()
        {
            return WithScalarField("ParameterName");
        }

        public RandEParameterValueQueryBuilder WithParameterValue()
        {
            return WithScalarField("ParameterValue");
        }

        public RandEParameterValueQueryBuilder WithRandEParameter(RandEParameterQueryBuilder randEParameterQueryBuilder)
        {
            return WithObjectField("RandEParameter", randEParameterQueryBuilder);
        }

        public RandEParameterValueQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public RandEParameterValueQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlProgramQueryBuilder : GraphQlQueryBuilder<ControlProgramQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ProgramName" },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "ControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "ControlItems", IsComplex = true, QueryBuilderType = typeof(ControlItemQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlProgramQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlProgramQueryBuilder WithProgramName()
        {
            return WithScalarField("ProgramName");
        }

        public ControlProgramQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Materials", materialsQueryBuilder, args);
        }

        public ControlProgramQueryBuilder WithControlFile(ControlFileQueryBuilder controlFileQueryBuilder)
        {
            return WithObjectField("ControlFile", controlFileQueryBuilder);
        }

        public ControlProgramQueryBuilder WithControlItems(ControlItemQueryBuilder controlItemQueryBuilder, ControlItemCondition condition = null, ControlItemFilter filter = null, IEnumerable<ControlItemSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItems", controlItemQueryBuilder, args);
        }

        public ControlProgramQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlProgramQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlFileQueryBuilder : GraphQlQueryBuilder<ControlFileQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlFileNumber" },
                new FieldMetadata { Name = "ControlFileStartDate", IsComplex = true },
                new FieldMetadata { Name = "ControlJournals", IsComplex = true, QueryBuilderType = typeof(ControlJournalsQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "ControlProgram", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "ControlFileStatus", IsComplex = true, QueryBuilderType = typeof(dicControlFileStatusQueryBuilder) },
                new FieldMetadata { Name = "ControObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlFileQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlFileQueryBuilder WithControlFileNumber()
        {
            return WithScalarField("ControlFileNumber");
        }

        public ControlFileQueryBuilder WithControlFileStartDate()
        {
            return WithScalarField("ControlFileStartDate");
        }

        public ControlFileQueryBuilder WithControlJournals(ControlJournalsQueryBuilder controlJournalsQueryBuilder, ControlJournalsCondition condition = null, ControlJournalsFilter filter = null, IEnumerable<ControlJournalsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlJournals", controlJournalsQueryBuilder, args);
        }

        public ControlFileQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Materials", materialsQueryBuilder, args);
        }

        public ControlFileQueryBuilder WithControlProgram(ControlProgramQueryBuilder controlProgramQueryBuilder)
        {
            return WithObjectField("ControlProgram", controlProgramQueryBuilder);
        }

        public ControlFileQueryBuilder WithControlFileStatus(dicControlFileStatusQueryBuilder dicControlFileStatusQueryBuilder)
        {
            return WithObjectField("ControlFileStatus", dicControlFileStatusQueryBuilder);
        }

        public ControlFileQueryBuilder WithControObject(ControlObjectQueryBuilder controlObjectQueryBuilder)
        {
            return WithObjectField("ControObject", controlObjectQueryBuilder);
        }

        public ControlFileQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlFileQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicOKVEDQueryBuilder : GraphQlQueryBuilder<dicOKVEDQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ActivityType" },
                new FieldMetadata { Name = "Decipher" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicOKVEDQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicOKVEDQueryBuilder WithActivityType()
        {
            return WithScalarField("ActivityType");
        }

        public dicOKVEDQueryBuilder WithDecipher()
        {
            return WithScalarField("Decipher");
        }

        public dicOKVEDQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicOKVEDQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class docControlPlanQueryBuilder : GraphQlQueryBuilder<docControlPlanQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "CreateDate", IsComplex = true },
                new FieldMetadata { Name = "ControlPlanName" },
                new FieldMetadata { Name = "ControlPlanApproveData", IsComplex = true },
                new FieldMetadata { Name = "FGISERPGlobalPlanGUID" },
                new FieldMetadata { Name = "ControlPlanYear" },
                new FieldMetadata { Name = "ControlCardList", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "ControlPlanStatus", IsComplex = true, QueryBuilderType = typeof(dicStatusPlanQueryBuilder) },
                new FieldMetadata { Name = "ControlPlanType", IsComplex = true, QueryBuilderType = typeof(DicControlPlanTypeQueryBuilder) },
                new FieldMetadata { Name = "ControlPlanApprovers", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "ControlPlanAuthor", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public docControlPlanQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public docControlPlanQueryBuilder WithCreateDate()
        {
            return WithScalarField("CreateDate");
        }

        public docControlPlanQueryBuilder WithControlPlanName()
        {
            return WithScalarField("ControlPlanName");
        }

        public docControlPlanQueryBuilder WithControlPlanApproveData()
        {
            return WithScalarField("ControlPlanApproveData");
        }

        public docControlPlanQueryBuilder WithFgiserpGlobalPlanGuid()
        {
            return WithScalarField("FGISERPGlobalPlanGUID");
        }

        public docControlPlanQueryBuilder WithControlPlanYear()
        {
            return WithScalarField("ControlPlanYear");
        }

        public docControlPlanQueryBuilder WithControlCardList(ControlCardQueryBuilder controlCardQueryBuilder, ControlCardCondition condition = null, ControlCardFilter filter = null, IEnumerable<ControlCardSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlCardList", controlCardQueryBuilder, args);
        }

        public docControlPlanQueryBuilder WithControlPlanStatus(dicStatusPlanQueryBuilder dicStatusPlanQueryBuilder)
        {
            return WithObjectField("ControlPlanStatus", dicStatusPlanQueryBuilder);
        }

        public docControlPlanQueryBuilder WithControlPlanType(DicControlPlanTypeQueryBuilder dicControlPlanTypeQueryBuilder)
        {
            return WithObjectField("ControlPlanType", dicControlPlanTypeQueryBuilder);
        }

        public docControlPlanQueryBuilder WithControlPlanApprovers(PersonQueryBuilder personQueryBuilder, PersonCondition condition = null, PersonFilter filter = null, IEnumerable<PersonSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlPlanApprovers", personQueryBuilder, args);
        }

        public docControlPlanQueryBuilder WithControlPlanAuthor(PersonQueryBuilder personQueryBuilder)
        {
            return WithObjectField("ControlPlanAuthor", personQueryBuilder);
        }

        public docControlPlanQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder)
        {
            return WithObjectField("Materials", materialsQueryBuilder);
        }

        public docControlPlanQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public docControlPlanQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ExtendedAttribValueQueryBuilder : GraphQlQueryBuilder<ExtendedAttribValueQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ExtAttributeContent" },
                new FieldMetadata { Name = "ControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "ExtendedAttrib", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ExtendedAttribValueQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ExtendedAttribValueQueryBuilder WithExtAttributeContent()
        {
            return WithScalarField("ExtAttributeContent");
        }

        public ExtendedAttribValueQueryBuilder WithControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder)
        {
            return WithObjectField("ControlObject", controlObjectQueryBuilder);
        }

        public ExtendedAttribValueQueryBuilder WithExtendedAttrib(ExtendedAttribQueryBuilder extendedAttribQueryBuilder)
        {
            return WithObjectField("ExtendedAttrib", extendedAttribQueryBuilder);
        }

        public ExtendedAttribValueQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ExtendedAttribValueQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class OrganizationUnitQueryBuilder : GraphQlQueryBuilder<OrganizationUnitQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "OrganizationUnitName" },
                new FieldMetadata { Name = "ControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "OrganizationUnits", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "MainOrganizationUnit", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public OrganizationUnitQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public OrganizationUnitQueryBuilder WithOrganizationUnitName()
        {
            return WithScalarField("OrganizationUnitName");
        }

        public OrganizationUnitQueryBuilder WithControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder)
        {
            return WithObjectField("ControlOrganization", controlOrganizationQueryBuilder);
        }

        public OrganizationUnitQueryBuilder WithOrganizationUnits(OrganizationUnitQueryBuilder organizationUnitQueryBuilder, OrganizationUnitCondition condition = null, OrganizationUnitFilter filter = null, IEnumerable<OrganizationUnitSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("OrganizationUnits", organizationUnitQueryBuilder, args);
        }

        public OrganizationUnitQueryBuilder WithMainOrganizationUnit(OrganizationUnitQueryBuilder organizationUnitQueryBuilder)
        {
            return WithObjectField("MainOrganizationUnit", organizationUnitQueryBuilder);
        }

        public OrganizationUnitQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public OrganizationUnitQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class MutationQueryBuilder : GraphQlQueryBuilder<MutationQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "createDicQuestionAnswers", IsComplex = true, QueryBuilderType = typeof(DicQuestionAnswersQueryBuilder) },
                new FieldMetadata { Name = "updateDicQuestionAnswers", IsComplex = true, QueryBuilderType = typeof(DicQuestionAnswersQueryBuilder) },
                new FieldMetadata { Name = "deleteDicQuestionAnswers" },
                new FieldMetadata { Name = "createdicProcStatmentStatus", IsComplex = true, QueryBuilderType = typeof(dicProcStatmentStatusQueryBuilder) },
                new FieldMetadata { Name = "updatedicProcStatmentStatus", IsComplex = true, QueryBuilderType = typeof(dicProcStatmentStatusQueryBuilder) },
                new FieldMetadata { Name = "deletedicProcStatmentStatus" },
                new FieldMetadata { Name = "createdocProcStatement", IsComplex = true, QueryBuilderType = typeof(docProcStatementQueryBuilder) },
                new FieldMetadata { Name = "updatedocProcStatement", IsComplex = true, QueryBuilderType = typeof(docProcStatementQueryBuilder) },
                new FieldMetadata { Name = "deletedocProcStatement" },
                new FieldMetadata { Name = "createdocControlOrder", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "updatedocControlOrder", IsComplex = true, QueryBuilderType = typeof(docControlOrderQueryBuilder) },
                new FieldMetadata { Name = "deletedocControlOrder" },
                new FieldMetadata { Name = "createControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "updateControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "deleteControlCard" },
                new FieldMetadata { Name = "createPerson", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "updatePerson", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "deletePerson" },
                new FieldMetadata { Name = "createdocControlPlan", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "updatedocControlPlan", IsComplex = true, QueryBuilderType = typeof(docControlPlanQueryBuilder) },
                new FieldMetadata { Name = "deletedocControlPlan" },
                new FieldMetadata { Name = "createdicStatusPlan", IsComplex = true, QueryBuilderType = typeof(dicStatusPlanQueryBuilder) },
                new FieldMetadata { Name = "updatedicStatusPlan", IsComplex = true, QueryBuilderType = typeof(dicStatusPlanQueryBuilder) },
                new FieldMetadata { Name = "deletedicStatusPlan" },
                new FieldMetadata { Name = "createdocControlAct", IsComplex = true, QueryBuilderType = typeof(docControlActQueryBuilder) },
                new FieldMetadata { Name = "updatedocControlAct", IsComplex = true, QueryBuilderType = typeof(docControlActQueryBuilder) },
                new FieldMetadata { Name = "deletedocControlAct" },
                new FieldMetadata { Name = "createAddress", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "updateAddress", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "deleteAddress" },
                new FieldMetadata { Name = "createdocControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "updatedocControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "deletedocControlList" },
                new FieldMetadata { Name = "createControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "updateControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "deleteControlListQuestions" },
                new FieldMetadata { Name = "createMandatoryReqs", IsComplex = true, QueryBuilderType = typeof(MandatoryReqsQueryBuilder) },
                new FieldMetadata { Name = "updateMandatoryReqs", IsComplex = true, QueryBuilderType = typeof(MandatoryReqsQueryBuilder) },
                new FieldMetadata { Name = "deleteMandatoryReqs" },
                new FieldMetadata { Name = "createNPA", IsComplex = true, QueryBuilderType = typeof(NPAQueryBuilder) },
                new FieldMetadata { Name = "updateNPA", IsComplex = true, QueryBuilderType = typeof(NPAQueryBuilder) },
                new FieldMetadata { Name = "deleteNPA" },
                new FieldMetadata { Name = "createdicControlTypes", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "updatedicControlTypes", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "deletedicControlTypes" },
                new FieldMetadata { Name = "createControlItemResult", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "updateControlItemResult", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "deleteControlItemResult" },
                new FieldMetadata { Name = "createMaterials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "updateMaterials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "deleteMaterials" },
                new FieldMetadata { Name = "createViolation", IsComplex = true, QueryBuilderType = typeof(ViolationQueryBuilder) },
                new FieldMetadata { Name = "updateViolation", IsComplex = true, QueryBuilderType = typeof(ViolationQueryBuilder) },
                new FieldMetadata { Name = "deleteViolation" },
                new FieldMetadata { Name = "createdicViolationTypes", IsComplex = true, QueryBuilderType = typeof(dicViolationTypesQueryBuilder) },
                new FieldMetadata { Name = "updatedicViolationTypes", IsComplex = true, QueryBuilderType = typeof(dicViolationTypesQueryBuilder) },
                new FieldMetadata { Name = "deletedicViolationTypes" },
                new FieldMetadata { Name = "createdicPunishmentType", IsComplex = true, QueryBuilderType = typeof(dicPunishmentTypeQueryBuilder) },
                new FieldMetadata { Name = "updatedicPunishmentType", IsComplex = true, QueryBuilderType = typeof(dicPunishmentTypeQueryBuilder) },
                new FieldMetadata { Name = "deletedicPunishmentType" },
                new FieldMetadata { Name = "createdicDamageType", IsComplex = true, QueryBuilderType = typeof(dicDamageTypeQueryBuilder) },
                new FieldMetadata { Name = "updatedicDamageType", IsComplex = true, QueryBuilderType = typeof(dicDamageTypeQueryBuilder) },
                new FieldMetadata { Name = "deletedicDamageType" },
                new FieldMetadata { Name = "createdicControlListStatus", IsComplex = true, QueryBuilderType = typeof(dicControlListStatusQueryBuilder) },
                new FieldMetadata { Name = "updatedicControlListStatus", IsComplex = true, QueryBuilderType = typeof(dicControlListStatusQueryBuilder) },
                new FieldMetadata { Name = "deletedicControlListStatus" },
                new FieldMetadata { Name = "createdicKNMForm", IsComplex = true, QueryBuilderType = typeof(dicKNMFormQueryBuilder) },
                new FieldMetadata { Name = "updatedicKNMForm", IsComplex = true, QueryBuilderType = typeof(dicKNMFormQueryBuilder) },
                new FieldMetadata { Name = "deletedicKNMForm" },
                new FieldMetadata { Name = "createControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "updateControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "deleteControlOrganization" },
                new FieldMetadata { Name = "createdicRole", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "updatedicRole", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "deletedicRole" },
                new FieldMetadata { Name = "createdicControlBase", IsComplex = true, QueryBuilderType = typeof(dicControlBaseQueryBuilder) },
                new FieldMetadata { Name = "updatedicControlBase", IsComplex = true, QueryBuilderType = typeof(dicControlBaseQueryBuilder) },
                new FieldMetadata { Name = "deletedicControlBase" },
                new FieldMetadata { Name = "createdicControlReasonDeny", IsComplex = true, QueryBuilderType = typeof(dicControlReasonDenyQueryBuilder) },
                new FieldMetadata { Name = "updatedicControlReasonDeny", IsComplex = true, QueryBuilderType = typeof(dicControlReasonDenyQueryBuilder) },
                new FieldMetadata { Name = "deletedicControlReasonDeny" },
                new FieldMetadata { Name = "createdicProsec", IsComplex = true, QueryBuilderType = typeof(dicProsecQueryBuilder) },
                new FieldMetadata { Name = "updatedicProsec", IsComplex = true, QueryBuilderType = typeof(dicProsecQueryBuilder) },
                new FieldMetadata { Name = "deletedicProsec" },
                new FieldMetadata { Name = "createdocRegulation", IsComplex = true, QueryBuilderType = typeof(docRegulationQueryBuilder) },
                new FieldMetadata { Name = "updatedocRegulation", IsComplex = true, QueryBuilderType = typeof(docRegulationQueryBuilder) },
                new FieldMetadata { Name = "deletedocRegulation" },
                new FieldMetadata { Name = "createRandEParameterValue", IsComplex = true, QueryBuilderType = typeof(RandEParameterValueQueryBuilder) },
                new FieldMetadata { Name = "updateRandEParameterValue", IsComplex = true, QueryBuilderType = typeof(RandEParameterValueQueryBuilder) },
                new FieldMetadata { Name = "deleteRandEParameterValue" },
                new FieldMetadata { Name = "createJournalAttributes", IsComplex = true, QueryBuilderType = typeof(JournalAttributesQueryBuilder) },
                new FieldMetadata { Name = "updateJournalAttributes", IsComplex = true, QueryBuilderType = typeof(JournalAttributesQueryBuilder) },
                new FieldMetadata { Name = "deleteJournalAttributes" },
                new FieldMetadata { Name = "createdicControlFileStatus", IsComplex = true, QueryBuilderType = typeof(dicControlFileStatusQueryBuilder) },
                new FieldMetadata { Name = "updatedicControlFileStatus", IsComplex = true, QueryBuilderType = typeof(dicControlFileStatusQueryBuilder) },
                new FieldMetadata { Name = "deletedicControlFileStatus" },
                new FieldMetadata { Name = "createControlItem", IsComplex = true, QueryBuilderType = typeof(ControlItemQueryBuilder) },
                new FieldMetadata { Name = "updateControlItem", IsComplex = true, QueryBuilderType = typeof(ControlItemQueryBuilder) },
                new FieldMetadata { Name = "deleteControlItem" },
                new FieldMetadata { Name = "createCitizenRequest", IsComplex = true, QueryBuilderType = typeof(CitizenRequestQueryBuilder) },
                new FieldMetadata { Name = "updateCitizenRequest", IsComplex = true, QueryBuilderType = typeof(CitizenRequestQueryBuilder) },
                new FieldMetadata { Name = "deleteCitizenRequest" },
                new FieldMetadata { Name = "createdicLicenceStatus", IsComplex = true, QueryBuilderType = typeof(dicLicenceStatusQueryBuilder) },
                new FieldMetadata { Name = "updatedicLicenceStatus", IsComplex = true, QueryBuilderType = typeof(dicLicenceStatusQueryBuilder) },
                new FieldMetadata { Name = "deletedicLicenceStatus" },
                new FieldMetadata { Name = "createRiskCat", IsComplex = true, QueryBuilderType = typeof(RiskCatQueryBuilder) },
                new FieldMetadata { Name = "updateRiskCat", IsComplex = true, QueryBuilderType = typeof(RiskCatQueryBuilder) },
                new FieldMetadata { Name = "deleteRiskCat" },
                new FieldMetadata { Name = "createdicOKOPF", IsComplex = true, QueryBuilderType = typeof(dicOKOPFQueryBuilder) },
                new FieldMetadata { Name = "updatedicOKOPF", IsComplex = true, QueryBuilderType = typeof(dicOKOPFQueryBuilder) },
                new FieldMetadata { Name = "deletedicOKOPF" },
                new FieldMetadata { Name = "createCheckOutEGRUL", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRULQueryBuilder) },
                new FieldMetadata { Name = "updateCheckOutEGRUL", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRULQueryBuilder) },
                new FieldMetadata { Name = "deleteCheckOutEGRUL" },
                new FieldMetadata { Name = "createdicOKSM", IsComplex = true, QueryBuilderType = typeof(dicOKSMQueryBuilder) },
                new FieldMetadata { Name = "updatedicOKSM", IsComplex = true, QueryBuilderType = typeof(dicOKSMQueryBuilder) },
                new FieldMetadata { Name = "deletedicOKSM" },
                new FieldMetadata { Name = "createdicSubjectType", IsComplex = true, QueryBuilderType = typeof(dicSubjectTypeQueryBuilder) },
                new FieldMetadata { Name = "updatedicSubjectType", IsComplex = true, QueryBuilderType = typeof(dicSubjectTypeQueryBuilder) },
                new FieldMetadata { Name = "deletedicSubjectType" },
                new FieldMetadata { Name = "createExtendedAttrib", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribQueryBuilder) },
                new FieldMetadata { Name = "updateExtendedAttrib", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribQueryBuilder) },
                new FieldMetadata { Name = "deleteExtendedAttrib" },
                new FieldMetadata { Name = "createRandEParameter", IsComplex = true, QueryBuilderType = typeof(RandEParameterQueryBuilder) },
                new FieldMetadata { Name = "updateRandEParameter", IsComplex = true, QueryBuilderType = typeof(RandEParameterQueryBuilder) },
                new FieldMetadata { Name = "deleteRandEParameter" },
                new FieldMetadata { Name = "createdicStatusKNM", IsComplex = true, QueryBuilderType = typeof(dicStatusKNMQueryBuilder) },
                new FieldMetadata { Name = "updatedicStatusKNM", IsComplex = true, QueryBuilderType = typeof(dicStatusKNMQueryBuilder) },
                new FieldMetadata { Name = "deletedicStatusKNM" },
                new FieldMetadata { Name = "createPersonAppointment", IsComplex = true, QueryBuilderType = typeof(PersonAppointmentQueryBuilder) },
                new FieldMetadata { Name = "updatePersonAppointment", IsComplex = true, QueryBuilderType = typeof(PersonAppointmentQueryBuilder) },
                new FieldMetadata { Name = "deletePersonAppointment" },
                new FieldMetadata { Name = "createdicKNMTypes", IsComplex = true, QueryBuilderType = typeof(dicKNMTypesQueryBuilder) },
                new FieldMetadata { Name = "updatedicKNMTypes", IsComplex = true, QueryBuilderType = typeof(dicKNMTypesQueryBuilder) },
                new FieldMetadata { Name = "deletedicKNMTypes" },
                new FieldMetadata { Name = "createControlJournals", IsComplex = true, QueryBuilderType = typeof(ControlJournalsQueryBuilder) },
                new FieldMetadata { Name = "updateControlJournals", IsComplex = true, QueryBuilderType = typeof(ControlJournalsQueryBuilder) },
                new FieldMetadata { Name = "deleteControlJournals" },
                new FieldMetadata { Name = "createControlProgram", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "updateControlProgram", IsComplex = true, QueryBuilderType = typeof(ControlProgramQueryBuilder) },
                new FieldMetadata { Name = "deleteControlProgram" },
                new FieldMetadata { Name = "createControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "updateControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "deleteControlFile" },
                new FieldMetadata { Name = "createControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "updateControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "deleteControlObject" },
                new FieldMetadata { Name = "createdicOKTMO", IsComplex = true, QueryBuilderType = typeof(dicOKTMOQueryBuilder) },
                new FieldMetadata { Name = "updatedicOKTMO", IsComplex = true, QueryBuilderType = typeof(dicOKTMOQueryBuilder) },
                new FieldMetadata { Name = "deletedicOKTMO" },
                new FieldMetadata { Name = "createActivity", IsComplex = true, QueryBuilderType = typeof(ActivityQueryBuilder) },
                new FieldMetadata { Name = "updateActivity", IsComplex = true, QueryBuilderType = typeof(ActivityQueryBuilder) },
                new FieldMetadata { Name = "deleteActivity" },
                new FieldMetadata { Name = "createdicOKVED", IsComplex = true, QueryBuilderType = typeof(dicOKVEDQueryBuilder) },
                new FieldMetadata { Name = "updatedicOKVED", IsComplex = true, QueryBuilderType = typeof(dicOKVEDQueryBuilder) },
                new FieldMetadata { Name = "deletedicOKVED" },
                new FieldMetadata { Name = "createLicence", IsComplex = true, QueryBuilderType = typeof(LicenceQueryBuilder) },
                new FieldMetadata { Name = "updateLicence", IsComplex = true, QueryBuilderType = typeof(LicenceQueryBuilder) },
                new FieldMetadata { Name = "deleteLicence" },
                new FieldMetadata { Name = "createControlItemPassport", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "updateControlItemPassport", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "deleteControlItemPassport" },
                new FieldMetadata { Name = "createSubject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "updateSubject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "deleteSubject" },
                new FieldMetadata { Name = "createCheckOutEGRIP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRIPQueryBuilder) },
                new FieldMetadata { Name = "updateCheckOutEGRIP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRIPQueryBuilder) },
                new FieldMetadata { Name = "deleteCheckOutEGRIP" },
                new FieldMetadata { Name = "createCheckOutEGRSMSP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRSMSPQueryBuilder) },
                new FieldMetadata { Name = "updateCheckOutEGRSMSP", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRSMSPQueryBuilder) },
                new FieldMetadata { Name = "deleteCheckOutEGRSMSP" },
                new FieldMetadata { Name = "createdicNPALevels", IsComplex = true, QueryBuilderType = typeof(dicNPALevelsQueryBuilder) },
                new FieldMetadata { Name = "updatedicNPALevels", IsComplex = true, QueryBuilderType = typeof(dicNPALevelsQueryBuilder) },
                new FieldMetadata { Name = "deletedicNPALevels" },
                new FieldMetadata { Name = "createdicNPATypes", IsComplex = true, QueryBuilderType = typeof(dicNPATypesQueryBuilder) },
                new FieldMetadata { Name = "updatedicNPATypes", IsComplex = true, QueryBuilderType = typeof(dicNPATypesQueryBuilder) },
                new FieldMetadata { Name = "deletedicNPATypes" },
                new FieldMetadata { Name = "createFile", IsComplex = true, QueryBuilderType = typeof(FileQueryBuilder) },
                new FieldMetadata { Name = "updateFile", IsComplex = true, QueryBuilderType = typeof(FileQueryBuilder) },
                new FieldMetadata { Name = "deleteFile" },
                new FieldMetadata { Name = "createDicMeasureUnitsType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "updateDicMeasureUnitsType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "deleteDicMeasureUnitsType" },
                new FieldMetadata { Name = "createDicControlItemBaseType", IsComplex = true, QueryBuilderType = typeof(DicControlItemBaseTypeQueryBuilder) },
                new FieldMetadata { Name = "updateDicControlItemBaseType", IsComplex = true, QueryBuilderType = typeof(DicControlItemBaseTypeQueryBuilder) },
                new FieldMetadata { Name = "deleteDicControlItemBaseType" },
                new FieldMetadata { Name = "createDicControlPlanType", IsComplex = true, QueryBuilderType = typeof(DicControlPlanTypeQueryBuilder) },
                new FieldMetadata { Name = "updateDicControlPlanType", IsComplex = true, QueryBuilderType = typeof(DicControlPlanTypeQueryBuilder) },
                new FieldMetadata { Name = "deleteDicControlPlanType" },
                new FieldMetadata { Name = "createOrganizationUnit", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "updateOrganizationUnit", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "deleteOrganizationUnit" },
                new FieldMetadata { Name = "createdicLicencedActivityTypes", IsComplex = true, QueryBuilderType = typeof(dicLicencedActivityTypesQueryBuilder) },
                new FieldMetadata { Name = "updatedicLicencedActivityTypes", IsComplex = true, QueryBuilderType = typeof(dicLicencedActivityTypesQueryBuilder) },
                new FieldMetadata { Name = "deletedicLicencedActivityTypes" },
                new FieldMetadata { Name = "createdicHazardClass", IsComplex = true, QueryBuilderType = typeof(dicHazardClassQueryBuilder) },
                new FieldMetadata { Name = "updatedicHazardClass", IsComplex = true, QueryBuilderType = typeof(dicHazardClassQueryBuilder) },
                new FieldMetadata { Name = "deletedicHazardClass" },
                new FieldMetadata { Name = "createdicRiskCategory", IsComplex = true, QueryBuilderType = typeof(dicRiskCategoryQueryBuilder) },
                new FieldMetadata { Name = "updatedicRiskCategory", IsComplex = true, QueryBuilderType = typeof(dicRiskCategoryQueryBuilder) },
                new FieldMetadata { Name = "deletedicRiskCategory" },
                new FieldMetadata { Name = "createExtendedAttribValue", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribValueQueryBuilder) },
                new FieldMetadata { Name = "updateExtendedAttribValue", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribValueQueryBuilder) },
                new FieldMetadata { Name = "deleteExtendedAttribValue" },
                new FieldMetadata { Name = "createHazardClass", IsComplex = true, QueryBuilderType = typeof(HazardClassQueryBuilder) },
                new FieldMetadata { Name = "updateHazardClass", IsComplex = true, QueryBuilderType = typeof(HazardClassQueryBuilder) },
                new FieldMetadata { Name = "deleteHazardClass" }
            };

        protected override string Prefix { get { return "mutation"; } }

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public MutationQueryBuilder WithCreateDicQuestionAnswers(DicQuestionAnswersQueryBuilder dicQuestionAnswersQueryBuilder, DicQuestionAnswersCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createDicQuestionAnswers", dicQuestionAnswersQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateDicQuestionAnswers(DicQuestionAnswersQueryBuilder dicQuestionAnswersQueryBuilder, DicQuestionAnswersUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateDicQuestionAnswers", dicQuestionAnswersQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteDicQuestionAnswers(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteDicQuestionAnswers", args);
        }

        public MutationQueryBuilder WithCreatedicProcStatmentStatus(dicProcStatmentStatusQueryBuilder dicProcStatmentStatusQueryBuilder, dicProcStatmentStatusCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicProcStatmentStatus", dicProcStatmentStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicProcStatmentStatus(dicProcStatmentStatusQueryBuilder dicProcStatmentStatusQueryBuilder, dicProcStatmentStatusUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicProcStatmentStatus", dicProcStatmentStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicProcStatmentStatus(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicProcStatmentStatus", args);
        }

        public MutationQueryBuilder WithCreatedocProcStatement(docProcStatementQueryBuilder docProcStatementQueryBuilder, docProcStatementCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdocProcStatement", docProcStatementQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedocProcStatement(docProcStatementQueryBuilder docProcStatementQueryBuilder, docProcStatementUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedocProcStatement", docProcStatementQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedocProcStatement(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedocProcStatement", args);
        }

        public MutationQueryBuilder WithCreatedocControlOrder(docControlOrderQueryBuilder docControlOrderQueryBuilder, docControlOrderCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdocControlOrder", docControlOrderQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedocControlOrder(docControlOrderQueryBuilder docControlOrderQueryBuilder, docControlOrderUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedocControlOrder", docControlOrderQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedocControlOrder(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedocControlOrder", args);
        }

        public MutationQueryBuilder WithCreateControlCard(ControlCardQueryBuilder controlCardQueryBuilder, ControlCardCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlCard", controlCardQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlCard(ControlCardQueryBuilder controlCardQueryBuilder, ControlCardUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlCard", controlCardQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlCard(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlCard", args);
        }

        public MutationQueryBuilder WithCreatePerson(PersonQueryBuilder personQueryBuilder, PersonCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createPerson", personQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatePerson(PersonQueryBuilder personQueryBuilder, PersonUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatePerson", personQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletePerson(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletePerson", args);
        }

        public MutationQueryBuilder WithCreatedocControlPlan(docControlPlanQueryBuilder docControlPlanQueryBuilder, docControlPlanCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdocControlPlan", docControlPlanQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedocControlPlan(docControlPlanQueryBuilder docControlPlanQueryBuilder, docControlPlanUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedocControlPlan", docControlPlanQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedocControlPlan(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedocControlPlan", args);
        }

        public MutationQueryBuilder WithCreatedicStatusPlan(dicStatusPlanQueryBuilder dicStatusPlanQueryBuilder, dicStatusPlanCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicStatusPlan", dicStatusPlanQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicStatusPlan(dicStatusPlanQueryBuilder dicStatusPlanQueryBuilder, dicStatusPlanUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicStatusPlan", dicStatusPlanQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicStatusPlan(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicStatusPlan", args);
        }

        public MutationQueryBuilder WithCreatedocControlAct(docControlActQueryBuilder docControlActQueryBuilder, docControlActCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdocControlAct", docControlActQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedocControlAct(docControlActQueryBuilder docControlActQueryBuilder, docControlActUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedocControlAct", docControlActQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedocControlAct(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedocControlAct", args);
        }

        public MutationQueryBuilder WithCreateAddress(AddressQueryBuilder addressQueryBuilder, AddressCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createAddress", addressQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateAddress(AddressQueryBuilder addressQueryBuilder, AddressUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateAddress", addressQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteAddress(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteAddress", args);
        }

        public MutationQueryBuilder WithCreatedocControlList(docControlListQueryBuilder docControlListQueryBuilder, docControlListCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdocControlList", docControlListQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedocControlList(docControlListQueryBuilder docControlListQueryBuilder, docControlListUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedocControlList", docControlListQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedocControlList(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedocControlList", args);
        }

        public MutationQueryBuilder WithCreateControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlListQuestions(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlListQuestions", args);
        }

        public MutationQueryBuilder WithCreateMandatoryReqs(MandatoryReqsQueryBuilder mandatoryReqsQueryBuilder, MandatoryReqsCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createMandatoryReqs", mandatoryReqsQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateMandatoryReqs(MandatoryReqsQueryBuilder mandatoryReqsQueryBuilder, MandatoryReqsUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateMandatoryReqs", mandatoryReqsQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteMandatoryReqs(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteMandatoryReqs", args);
        }

        public MutationQueryBuilder WithCreateNpa(NPAQueryBuilder nPAQueryBuilder, NPACreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createNPA", nPAQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateNpa(NPAQueryBuilder nPAQueryBuilder, NPAUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateNPA", nPAQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteNpa(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteNPA", args);
        }

        public MutationQueryBuilder WithCreatedicControlTypes(dicControlTypesQueryBuilder dicControlTypesQueryBuilder, dicControlTypesCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicControlTypes", dicControlTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicControlTypes(dicControlTypesQueryBuilder dicControlTypesQueryBuilder, dicControlTypesUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicControlTypes", dicControlTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicControlTypes(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicControlTypes", args);
        }

        public MutationQueryBuilder WithCreateControlItemResult(ControlItemResultQueryBuilder controlItemResultQueryBuilder, ControlItemResultCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlItemResult", controlItemResultQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlItemResult(ControlItemResultQueryBuilder controlItemResultQueryBuilder, ControlItemResultUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlItemResult", controlItemResultQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlItemResult(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlItemResult", args);
        }

        public MutationQueryBuilder WithCreateMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createMaterials", materialsQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateMaterials", materialsQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteMaterials(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteMaterials", args);
        }

        public MutationQueryBuilder WithCreateViolation(ViolationQueryBuilder violationQueryBuilder, ViolationCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createViolation", violationQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateViolation(ViolationQueryBuilder violationQueryBuilder, ViolationUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateViolation", violationQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteViolation(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteViolation", args);
        }

        public MutationQueryBuilder WithCreatedicViolationTypes(dicViolationTypesQueryBuilder dicViolationTypesQueryBuilder, dicViolationTypesCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicViolationTypes", dicViolationTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicViolationTypes(dicViolationTypesQueryBuilder dicViolationTypesQueryBuilder, dicViolationTypesUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicViolationTypes", dicViolationTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicViolationTypes(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicViolationTypes", args);
        }

        public MutationQueryBuilder WithCreatedicPunishmentType(dicPunishmentTypeQueryBuilder dicPunishmentTypeQueryBuilder, dicPunishmentTypeCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicPunishmentType", dicPunishmentTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicPunishmentType(dicPunishmentTypeQueryBuilder dicPunishmentTypeQueryBuilder, dicPunishmentTypeUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicPunishmentType", dicPunishmentTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicPunishmentType(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicPunishmentType", args);
        }

        public MutationQueryBuilder WithCreatedicDamageType(dicDamageTypeQueryBuilder dicDamageTypeQueryBuilder, dicDamageTypeCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicDamageType", dicDamageTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicDamageType(dicDamageTypeQueryBuilder dicDamageTypeQueryBuilder, dicDamageTypeUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicDamageType", dicDamageTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicDamageType(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicDamageType", args);
        }

        public MutationQueryBuilder WithCreatedicControlListStatus(dicControlListStatusQueryBuilder dicControlListStatusQueryBuilder, dicControlListStatusCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicControlListStatus", dicControlListStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicControlListStatus(dicControlListStatusQueryBuilder dicControlListStatusQueryBuilder, dicControlListStatusUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicControlListStatus", dicControlListStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicControlListStatus(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicControlListStatus", args);
        }

        public MutationQueryBuilder WithCreatedicKnmForm(dicKNMFormQueryBuilder dicKNMFormQueryBuilder, dicKNMFormCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicKNMForm", dicKNMFormQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicKnmForm(dicKNMFormQueryBuilder dicKNMFormQueryBuilder, dicKNMFormUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicKNMForm", dicKNMFormQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicKnmForm(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicKNMForm", args);
        }

        public MutationQueryBuilder WithCreateControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, ControlOrganizationCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlOrganization", controlOrganizationQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, ControlOrganizationUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlOrganization", controlOrganizationQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlOrganization(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlOrganization", args);
        }

        public MutationQueryBuilder WithCreatedicRole(dicRoleQueryBuilder dicRoleQueryBuilder, dicRoleCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicRole", dicRoleQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicRole(dicRoleQueryBuilder dicRoleQueryBuilder, dicRoleUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicRole", dicRoleQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicRole(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicRole", args);
        }

        public MutationQueryBuilder WithCreatedicControlBase(dicControlBaseQueryBuilder dicControlBaseQueryBuilder, dicControlBaseCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicControlBase", dicControlBaseQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicControlBase(dicControlBaseQueryBuilder dicControlBaseQueryBuilder, dicControlBaseUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicControlBase", dicControlBaseQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicControlBase(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicControlBase", args);
        }

        public MutationQueryBuilder WithCreatedicControlReasonDeny(dicControlReasonDenyQueryBuilder dicControlReasonDenyQueryBuilder, dicControlReasonDenyCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicControlReasonDeny", dicControlReasonDenyQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicControlReasonDeny(dicControlReasonDenyQueryBuilder dicControlReasonDenyQueryBuilder, dicControlReasonDenyUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicControlReasonDeny", dicControlReasonDenyQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicControlReasonDeny(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicControlReasonDeny", args);
        }

        public MutationQueryBuilder WithCreatedicProsec(dicProsecQueryBuilder dicProsecQueryBuilder, dicProsecCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicProsec", dicProsecQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicProsec(dicProsecQueryBuilder dicProsecQueryBuilder, dicProsecUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicProsec", dicProsecQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicProsec(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicProsec", args);
        }

        public MutationQueryBuilder WithCreatedocRegulation(docRegulationQueryBuilder docRegulationQueryBuilder, docRegulationCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdocRegulation", docRegulationQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedocRegulation(docRegulationQueryBuilder docRegulationQueryBuilder, docRegulationUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedocRegulation", docRegulationQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedocRegulation(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedocRegulation", args);
        }

        public MutationQueryBuilder WithCreateRandEParameterValue(RandEParameterValueQueryBuilder randEParameterValueQueryBuilder, RandEParameterValueCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createRandEParameterValue", randEParameterValueQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateRandEParameterValue(RandEParameterValueQueryBuilder randEParameterValueQueryBuilder, RandEParameterValueUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateRandEParameterValue", randEParameterValueQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteRandEParameterValue(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteRandEParameterValue", args);
        }

        public MutationQueryBuilder WithCreateJournalAttributes(JournalAttributesQueryBuilder journalAttributesQueryBuilder, JournalAttributesCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createJournalAttributes", journalAttributesQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateJournalAttributes(JournalAttributesQueryBuilder journalAttributesQueryBuilder, JournalAttributesUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateJournalAttributes", journalAttributesQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteJournalAttributes(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteJournalAttributes", args);
        }

        public MutationQueryBuilder WithCreatedicControlFileStatus(dicControlFileStatusQueryBuilder dicControlFileStatusQueryBuilder, dicControlFileStatusCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicControlFileStatus", dicControlFileStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicControlFileStatus(dicControlFileStatusQueryBuilder dicControlFileStatusQueryBuilder, dicControlFileStatusUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicControlFileStatus", dicControlFileStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicControlFileStatus(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicControlFileStatus", args);
        }

        public MutationQueryBuilder WithCreateControlItem(ControlItemQueryBuilder controlItemQueryBuilder, ControlItemCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlItem", controlItemQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlItem(ControlItemQueryBuilder controlItemQueryBuilder, ControlItemUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlItem", controlItemQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlItem(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlItem", args);
        }

        public MutationQueryBuilder WithCreateCitizenRequest(CitizenRequestQueryBuilder citizenRequestQueryBuilder, CitizenRequestCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createCitizenRequest", citizenRequestQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateCitizenRequest(CitizenRequestQueryBuilder citizenRequestQueryBuilder, CitizenRequestUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateCitizenRequest", citizenRequestQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteCitizenRequest(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteCitizenRequest", args);
        }

        public MutationQueryBuilder WithCreatedicLicenceStatus(dicLicenceStatusQueryBuilder dicLicenceStatusQueryBuilder, dicLicenceStatusCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicLicenceStatus", dicLicenceStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicLicenceStatus(dicLicenceStatusQueryBuilder dicLicenceStatusQueryBuilder, dicLicenceStatusUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicLicenceStatus", dicLicenceStatusQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicLicenceStatus(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicLicenceStatus", args);
        }

        public MutationQueryBuilder WithCreateRiskCat(RiskCatQueryBuilder riskCatQueryBuilder, RiskCatCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createRiskCat", riskCatQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateRiskCat(RiskCatQueryBuilder riskCatQueryBuilder, RiskCatUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateRiskCat", riskCatQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteRiskCat(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteRiskCat", args);
        }

        public MutationQueryBuilder WithCreatedicOkopf(dicOKOPFQueryBuilder dicOKOPFQueryBuilder, dicOKOPFCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicOKOPF", dicOKOPFQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicOkopf(dicOKOPFQueryBuilder dicOKOPFQueryBuilder, dicOKOPFUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicOKOPF", dicOKOPFQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicOkopf(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicOKOPF", args);
        }

        public MutationQueryBuilder WithCreateCheckOutEgrul(CheckOutEGRULQueryBuilder checkOutEGRULQueryBuilder, CheckOutEGRULCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createCheckOutEGRUL", checkOutEGRULQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateCheckOutEgrul(CheckOutEGRULQueryBuilder checkOutEGRULQueryBuilder, CheckOutEGRULUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateCheckOutEGRUL", checkOutEGRULQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteCheckOutEgrul(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteCheckOutEGRUL", args);
        }

        public MutationQueryBuilder WithCreatedicOksm(dicOKSMQueryBuilder dicOKSMQueryBuilder, dicOKSMCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicOKSM", dicOKSMQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicOksm(dicOKSMQueryBuilder dicOKSMQueryBuilder, dicOKSMUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicOKSM", dicOKSMQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicOksm(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicOKSM", args);
        }

        public MutationQueryBuilder WithCreatedicSubjectType(dicSubjectTypeQueryBuilder dicSubjectTypeQueryBuilder, dicSubjectTypeCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicSubjectType", dicSubjectTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicSubjectType(dicSubjectTypeQueryBuilder dicSubjectTypeQueryBuilder, dicSubjectTypeUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicSubjectType", dicSubjectTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicSubjectType(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicSubjectType", args);
        }

        public MutationQueryBuilder WithCreateExtendedAttrib(ExtendedAttribQueryBuilder extendedAttribQueryBuilder, ExtendedAttribCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createExtendedAttrib", extendedAttribQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateExtendedAttrib(ExtendedAttribQueryBuilder extendedAttribQueryBuilder, ExtendedAttribUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateExtendedAttrib", extendedAttribQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteExtendedAttrib(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteExtendedAttrib", args);
        }

        public MutationQueryBuilder WithCreateRandEParameter(RandEParameterQueryBuilder randEParameterQueryBuilder, RandEParameterCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createRandEParameter", randEParameterQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateRandEParameter(RandEParameterQueryBuilder randEParameterQueryBuilder, RandEParameterUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateRandEParameter", randEParameterQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteRandEParameter(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteRandEParameter", args);
        }

        public MutationQueryBuilder WithCreatedicStatusKnm(dicStatusKNMQueryBuilder dicStatusKNMQueryBuilder, dicStatusKNMCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicStatusKNM", dicStatusKNMQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicStatusKnm(dicStatusKNMQueryBuilder dicStatusKNMQueryBuilder, dicStatusKNMUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicStatusKNM", dicStatusKNMQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicStatusKnm(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicStatusKNM", args);
        }

        public MutationQueryBuilder WithCreatePersonAppointment(PersonAppointmentQueryBuilder personAppointmentQueryBuilder, PersonAppointmentCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createPersonAppointment", personAppointmentQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatePersonAppointment(PersonAppointmentQueryBuilder personAppointmentQueryBuilder, PersonAppointmentUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatePersonAppointment", personAppointmentQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletePersonAppointment(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletePersonAppointment", args);
        }

        public MutationQueryBuilder WithCreatedicKnmTypes(dicKNMTypesQueryBuilder dicKNMTypesQueryBuilder, dicKNMTypesCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicKNMTypes", dicKNMTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicKnmTypes(dicKNMTypesQueryBuilder dicKNMTypesQueryBuilder, dicKNMTypesUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicKNMTypes", dicKNMTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicKnmTypes(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicKNMTypes", args);
        }

        public MutationQueryBuilder WithCreateControlJournals(ControlJournalsQueryBuilder controlJournalsQueryBuilder, ControlJournalsCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlJournals", controlJournalsQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlJournals(ControlJournalsQueryBuilder controlJournalsQueryBuilder, ControlJournalsUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlJournals", controlJournalsQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlJournals(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlJournals", args);
        }

        public MutationQueryBuilder WithCreateControlProgram(ControlProgramQueryBuilder controlProgramQueryBuilder, ControlProgramCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlProgram", controlProgramQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlProgram(ControlProgramQueryBuilder controlProgramQueryBuilder, ControlProgramUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlProgram", controlProgramQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlProgram(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlProgram", args);
        }

        public MutationQueryBuilder WithCreateControlFile(ControlFileQueryBuilder controlFileQueryBuilder, ControlFileCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlFile", controlFileQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlFile(ControlFileQueryBuilder controlFileQueryBuilder, ControlFileUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlFile", controlFileQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlFile(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlFile", args);
        }

        public MutationQueryBuilder WithCreateControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder, ControlObjectCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlObject", controlObjectQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder, ControlObjectUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlObject", controlObjectQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlObject(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlObject", args);
        }

        public MutationQueryBuilder WithCreatedicOktmo(dicOKTMOQueryBuilder dicOKTMOQueryBuilder, dicOKTMOCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicOKTMO", dicOKTMOQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicOktmo(dicOKTMOQueryBuilder dicOKTMOQueryBuilder, dicOKTMOUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicOKTMO", dicOKTMOQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicOktmo(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicOKTMO", args);
        }

        public MutationQueryBuilder WithCreateActivity(ActivityQueryBuilder activityQueryBuilder, ActivityCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createActivity", activityQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateActivity(ActivityQueryBuilder activityQueryBuilder, ActivityUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateActivity", activityQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteActivity(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteActivity", args);
        }

        public MutationQueryBuilder WithCreatedicOkved(dicOKVEDQueryBuilder dicOKVEDQueryBuilder, dicOKVEDCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicOKVED", dicOKVEDQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicOkved(dicOKVEDQueryBuilder dicOKVEDQueryBuilder, dicOKVEDUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicOKVED", dicOKVEDQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicOkved(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicOKVED", args);
        }

        public MutationQueryBuilder WithCreateLicence(LicenceQueryBuilder licenceQueryBuilder, LicenceCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createLicence", licenceQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateLicence(LicenceQueryBuilder licenceQueryBuilder, LicenceUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateLicence", licenceQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteLicence(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteLicence", args);
        }

        public MutationQueryBuilder WithCreateControlItemPassport(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder, ControlItemPassportCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createControlItemPassport", controlItemPassportQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateControlItemPassport(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder, ControlItemPassportUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateControlItemPassport", controlItemPassportQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteControlItemPassport(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteControlItemPassport", args);
        }

        public MutationQueryBuilder WithCreateSubject(SubjectQueryBuilder subjectQueryBuilder, SubjectCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createSubject", subjectQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateSubject(SubjectQueryBuilder subjectQueryBuilder, SubjectUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateSubject", subjectQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteSubject(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteSubject", args);
        }

        public MutationQueryBuilder WithCreateCheckOutEgrip(CheckOutEGRIPQueryBuilder checkOutEGRIPQueryBuilder, CheckOutEGRIPCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createCheckOutEGRIP", checkOutEGRIPQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateCheckOutEgrip(CheckOutEGRIPQueryBuilder checkOutEGRIPQueryBuilder, CheckOutEGRIPUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateCheckOutEGRIP", checkOutEGRIPQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteCheckOutEgrip(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteCheckOutEGRIP", args);
        }

        public MutationQueryBuilder WithCreateCheckOutEgrsmsp(CheckOutEGRSMSPQueryBuilder checkOutEGRSMSPQueryBuilder, CheckOutEGRSMSPCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createCheckOutEGRSMSP", checkOutEGRSMSPQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateCheckOutEgrsmsp(CheckOutEGRSMSPQueryBuilder checkOutEGRSMSPQueryBuilder, CheckOutEGRSMSPUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateCheckOutEGRSMSP", checkOutEGRSMSPQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteCheckOutEgrsmsp(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteCheckOutEGRSMSP", args);
        }

        public MutationQueryBuilder WithCreatedicNpaLevels(dicNPALevelsQueryBuilder dicNPALevelsQueryBuilder, dicNPALevelsCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicNPALevels", dicNPALevelsQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicNpaLevels(dicNPALevelsQueryBuilder dicNPALevelsQueryBuilder, dicNPALevelsUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicNPALevels", dicNPALevelsQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicNpaLevels(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicNPALevels", args);
        }

        public MutationQueryBuilder WithCreatedicNpaTypes(dicNPATypesQueryBuilder dicNPATypesQueryBuilder, dicNPATypesCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicNPATypes", dicNPATypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicNpaTypes(dicNPATypesQueryBuilder dicNPATypesQueryBuilder, dicNPATypesUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicNPATypes", dicNPATypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicNpaTypes(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicNPATypes", args);
        }

        public MutationQueryBuilder WithCreateFile(FileQueryBuilder fileQueryBuilder, FileCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createFile", fileQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateFile(FileQueryBuilder fileQueryBuilder, FileUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateFile", fileQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteFile(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteFile", args);
        }

        public MutationQueryBuilder WithCreateDicMeasureUnitsType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder, DicMeasureUnitsTypeCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createDicMeasureUnitsType", dicMeasureUnitsTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateDicMeasureUnitsType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder, DicMeasureUnitsTypeUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateDicMeasureUnitsType", dicMeasureUnitsTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteDicMeasureUnitsType(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteDicMeasureUnitsType", args);
        }

        public MutationQueryBuilder WithCreateDicControlItemBaseType(DicControlItemBaseTypeQueryBuilder dicControlItemBaseTypeQueryBuilder, DicControlItemBaseTypeCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createDicControlItemBaseType", dicControlItemBaseTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateDicControlItemBaseType(DicControlItemBaseTypeQueryBuilder dicControlItemBaseTypeQueryBuilder, DicControlItemBaseTypeUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateDicControlItemBaseType", dicControlItemBaseTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteDicControlItemBaseType(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteDicControlItemBaseType", args);
        }

        public MutationQueryBuilder WithCreateDicControlPlanType(DicControlPlanTypeQueryBuilder dicControlPlanTypeQueryBuilder, DicControlPlanTypeCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createDicControlPlanType", dicControlPlanTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateDicControlPlanType(DicControlPlanTypeQueryBuilder dicControlPlanTypeQueryBuilder, DicControlPlanTypeUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateDicControlPlanType", dicControlPlanTypeQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteDicControlPlanType(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteDicControlPlanType", args);
        }

        public MutationQueryBuilder WithCreateOrganizationUnit(OrganizationUnitQueryBuilder organizationUnitQueryBuilder, OrganizationUnitCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createOrganizationUnit", organizationUnitQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateOrganizationUnit(OrganizationUnitQueryBuilder organizationUnitQueryBuilder, OrganizationUnitUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateOrganizationUnit", organizationUnitQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteOrganizationUnit(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteOrganizationUnit", args);
        }

        public MutationQueryBuilder WithCreatedicLicencedActivityTypes(dicLicencedActivityTypesQueryBuilder dicLicencedActivityTypesQueryBuilder, dicLicencedActivityTypesCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicLicencedActivityTypes", dicLicencedActivityTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicLicencedActivityTypes(dicLicencedActivityTypesQueryBuilder dicLicencedActivityTypesQueryBuilder, dicLicencedActivityTypesUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicLicencedActivityTypes", dicLicencedActivityTypesQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicLicencedActivityTypes(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicLicencedActivityTypes", args);
        }

        public MutationQueryBuilder WithCreatedicHazardClass(dicHazardClassQueryBuilder dicHazardClassQueryBuilder, dicHazardClassCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicHazardClass", dicHazardClassQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicHazardClass(dicHazardClassQueryBuilder dicHazardClassQueryBuilder, dicHazardClassUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicHazardClass", dicHazardClassQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicHazardClass(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicHazardClass", args);
        }

        public MutationQueryBuilder WithCreatedicRiskCategory(dicRiskCategoryQueryBuilder dicRiskCategoryQueryBuilder, dicRiskCategoryCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createdicRiskCategory", dicRiskCategoryQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdatedicRiskCategory(dicRiskCategoryQueryBuilder dicRiskCategoryQueryBuilder, dicRiskCategoryUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updatedicRiskCategory", dicRiskCategoryQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeletedicRiskCategory(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deletedicRiskCategory", args);
        }

        public MutationQueryBuilder WithCreateExtendedAttribValue(ExtendedAttribValueQueryBuilder extendedAttribValueQueryBuilder, ExtendedAttribValueCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createExtendedAttribValue", extendedAttribValueQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateExtendedAttribValue(ExtendedAttribValueQueryBuilder extendedAttribValueQueryBuilder, ExtendedAttribValueUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateExtendedAttribValue", extendedAttribValueQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteExtendedAttribValue(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteExtendedAttribValue", args);
        }

        public MutationQueryBuilder WithCreateHazardClass(HazardClassQueryBuilder hazardClassQueryBuilder, HazardClassCreateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("createHazardClass", hazardClassQueryBuilder, args);
        }

        public MutationQueryBuilder WithUpdateHazardClass(HazardClassQueryBuilder hazardClassQueryBuilder, HazardClassUpdateInput input)
        {
            var args = new Dictionary<string, object>();
            args.Add("input", input);
            return WithObjectField("updateHazardClass", hazardClassQueryBuilder, args);
        }

        public MutationQueryBuilder WithDeleteHazardClass(Guid id)
        {
            var args = new Dictionary<string, object>();
            args.Add("id", id);
            return WithScalarField("deleteHazardClass", args);
        }
    }

    public partial class FileQueryBuilder : GraphQlQueryBuilder<FileQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "HashMD5" },
                new FieldMetadata { Name = "FileName" },
                new FieldMetadata { Name = "FileType" },
                new FieldMetadata { Name = "FileSize" },
                new FieldMetadata { Name = "TimeStamp", IsComplex = true },
                new FieldMetadata { Name = "DownloadLink" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public FileQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public FileQueryBuilder WithHashMd5()
        {
            return WithScalarField("HashMD5");
        }

        public FileQueryBuilder WithFileName()
        {
            return WithScalarField("FileName");
        }

        public FileQueryBuilder WithFileType()
        {
            return WithScalarField("FileType");
        }

        public FileQueryBuilder WithFileSize()
        {
            return WithScalarField("FileSize");
        }

        public FileQueryBuilder WithTimeStamp()
        {
            return WithScalarField("TimeStamp");
        }

        public FileQueryBuilder WithDownloadLink()
        {
            return WithScalarField("DownloadLink");
        }

        public FileQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public FileQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class docRegulationQueryBuilder : GraphQlQueryBuilder<docRegulationQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "RegulationCreateDate", IsComplex = true },
                new FieldMetadata { Name = "RegulationExecutionDate", IsComplex = true },
                new FieldMetadata { Name = "Result" },
                new FieldMetadata { Name = "ControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "ProcStatmentStatus", IsComplex = true, QueryBuilderType = typeof(dicProcStatmentStatusQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public docRegulationQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public docRegulationQueryBuilder WithRegulationCreateDate()
        {
            return WithScalarField("RegulationCreateDate");
        }

        public docRegulationQueryBuilder WithRegulationExecutionDate()
        {
            return WithScalarField("RegulationExecutionDate");
        }

        public docRegulationQueryBuilder WithResult()
        {
            return WithScalarField("Result");
        }

        public docRegulationQueryBuilder WithControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("ControlCard", controlCardQueryBuilder);
        }

        public docRegulationQueryBuilder WithProcStatmentStatus(dicProcStatmentStatusQueryBuilder dicProcStatmentStatusQueryBuilder)
        {
            return WithObjectField("ProcStatmentStatus", dicProcStatmentStatusQueryBuilder);
        }

        public docRegulationQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder)
        {
            return WithObjectField("Materials", materialsQueryBuilder);
        }

        public docRegulationQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public docRegulationQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicControlFileStatusQueryBuilder : GraphQlQueryBuilder<dicControlFileStatusQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlFileStatusName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicControlFileStatusQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicControlFileStatusQueryBuilder WithControlFileStatusName()
        {
            return WithScalarField("ControlFileStatusName");
        }

        public dicControlFileStatusQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicControlFileStatusQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicControlListStatusQueryBuilder : GraphQlQueryBuilder<dicControlListStatusQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlListStatusName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicControlListStatusQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicControlListStatusQueryBuilder WithControlListStatusName()
        {
            return WithScalarField("ControlListStatusName");
        }

        public dicControlListStatusQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicControlListStatusQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlItemResultQueryBuilder : GraphQlQueryBuilder<ControlItemResultQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "sumAdmFine" },
                new FieldMetadata { Name = "SumAdmFineStatus" },
                new FieldMetadata { Name = "Violations", IsComplex = true, QueryBuilderType = typeof(ViolationQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "DamageType", IsComplex = true, QueryBuilderType = typeof(dicDamageTypeQueryBuilder) },
                new FieldMetadata { Name = "LinkedControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "ControlLists", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "PunishmentType", IsComplex = true, QueryBuilderType = typeof(dicPunishmentTypeQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlItemResultQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlItemResultQueryBuilder WithSumAdmFine()
        {
            return WithScalarField("sumAdmFine");
        }

        public ControlItemResultQueryBuilder WithSumAdmFineStatus()
        {
            return WithScalarField("SumAdmFineStatus");
        }

        public ControlItemResultQueryBuilder WithViolations(ViolationQueryBuilder violationQueryBuilder, ViolationCondition condition = null, ViolationFilter filter = null, IEnumerable<ViolationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Violations", violationQueryBuilder, args);
        }

        public ControlItemResultQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Materials", materialsQueryBuilder, args);
        }

        public ControlItemResultQueryBuilder WithDamageType(dicDamageTypeQueryBuilder dicDamageTypeQueryBuilder, dicDamageTypeCondition condition = null, dicDamageTypeFilter filter = null, IEnumerable<dicDamageTypeSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("DamageType", dicDamageTypeQueryBuilder, args);
        }

        public ControlItemResultQueryBuilder WithLinkedControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("LinkedControlCard", controlCardQueryBuilder);
        }

        public ControlItemResultQueryBuilder WithControlLists(docControlListQueryBuilder docControlListQueryBuilder, docControlListCondition condition = null, docControlListFilter filter = null, IEnumerable<docControlListSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlLists", docControlListQueryBuilder, args);
        }

        public ControlItemResultQueryBuilder WithPunishmentType(dicPunishmentTypeQueryBuilder dicPunishmentTypeQueryBuilder)
        {
            return WithObjectField("PunishmentType", dicPunishmentTypeQueryBuilder);
        }

        public ControlItemResultQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlItemResultQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlItemPassportQueryBuilder : GraphQlQueryBuilder<ControlItemPassportQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "CreateDate", IsComplex = true },
                new FieldMetadata { Name = "IsInPlanYear" },
                new FieldMetadata { Name = "ControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "ControlObjects", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "KNMType", IsComplex = true, QueryBuilderType = typeof(dicKNMTypesQueryBuilder) },
                new FieldMetadata { Name = "ControlOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "Subject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "StatusKNMName", IsComplex = true, QueryBuilderType = typeof(dicStatusKNMQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlItemPassportQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlItemPassportQueryBuilder WithCreateDate()
        {
            return WithScalarField("CreateDate");
        }

        public ControlItemPassportQueryBuilder WithIsInPlanYear()
        {
            return WithScalarField("IsInPlanYear");
        }

        public ControlItemPassportQueryBuilder WithControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("ControlCard", controlCardQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithControlObjects(ControlObjectQueryBuilder controlObjectQueryBuilder, ControlObjectCondition condition = null, ControlObjectFilter filter = null, IEnumerable<ControlObjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlObjects", controlObjectQueryBuilder, args);
        }

        public ControlItemPassportQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder)
        {
            return WithObjectField("ControlType", dicControlTypesQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithKnmType(dicKNMTypesQueryBuilder dicKNMTypesQueryBuilder)
        {
            return WithObjectField("KNMType", dicKNMTypesQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithControlOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder)
        {
            return WithObjectField("ControlOrganization", controlOrganizationQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithSubject(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("Subject", subjectQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithStatusKnmName(dicStatusKNMQueryBuilder dicStatusKNMQueryBuilder)
        {
            return WithObjectField("StatusKNMName", dicStatusKNMQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlItemPassportQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class MetaQueryBuilder : GraphQlQueryBuilder<MetaQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "creator" },
                new FieldMetadata { Name = "createTime", IsComplex = true },
                new FieldMetadata { Name = "editor" },
                new FieldMetadata { Name = "editTime", IsComplex = true }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public MetaQueryBuilder WithCreator()
        {
            return WithScalarField("creator");
        }

        public MetaQueryBuilder WithCreateTime()
        {
            return WithScalarField("createTime");
        }

        public MetaQueryBuilder WithEditor()
        {
            return WithScalarField("editor");
        }

        public MetaQueryBuilder WithEditTime()
        {
            return WithScalarField("editTime");
        }
    }

    public partial class ActivityQueryBuilder : GraphQlQueryBuilder<ActivityQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ActivityStartDate", IsComplex = true },
                new FieldMetadata { Name = "Subject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "OKVED", IsComplex = true, QueryBuilderType = typeof(dicOKVEDQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ActivityQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ActivityQueryBuilder WithActivityStartDate()
        {
            return WithScalarField("ActivityStartDate");
        }

        public ActivityQueryBuilder WithSubject(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("Subject", subjectQueryBuilder);
        }

        public ActivityQueryBuilder WithOkved(dicOKVEDQueryBuilder dicOKVEDQueryBuilder)
        {
            return WithObjectField("OKVED", dicOKVEDQueryBuilder);
        }

        public ActivityQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ActivityQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicViolationTypesQueryBuilder : GraphQlQueryBuilder<dicViolationTypesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "Violation" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicViolationTypesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicViolationTypesQueryBuilder WithViolation()
        {
            return WithScalarField("Violation");
        }

        public dicViolationTypesQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicViolationTypesQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class HazardClassQueryBuilder : GraphQlQueryBuilder<HazardClassQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "HazardClassLevel" },
                new FieldMetadata { Name = "HazardClassStartDate", IsComplex = true },
                new FieldMetadata { Name = "HazardClassEndDate", IsComplex = true },
                new FieldMetadata { Name = "ControlObject", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "HazardClass", IsComplex = true, QueryBuilderType = typeof(dicHazardClassQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public HazardClassQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public HazardClassQueryBuilder WithHazardClassLevel()
        {
            return WithScalarField("HazardClassLevel");
        }

        public HazardClassQueryBuilder WithHazardClassStartDate()
        {
            return WithScalarField("HazardClassStartDate");
        }

        public HazardClassQueryBuilder WithHazardClassEndDate()
        {
            return WithScalarField("HazardClassEndDate");
        }

        public HazardClassQueryBuilder WithControlObject(ControlObjectQueryBuilder controlObjectQueryBuilder)
        {
            return WithObjectField("ControlObject", controlObjectQueryBuilder);
        }

        public HazardClassQueryBuilder WithHazardClass(dicHazardClassQueryBuilder dicHazardClassQueryBuilder)
        {
            return WithObjectField("HazardClass", dicHazardClassQueryBuilder);
        }

        public HazardClassQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public HazardClassQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicControlTypesQueryBuilder : GraphQlQueryBuilder<dicControlTypesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlTypeName" },
                new FieldMetadata { Name = "ControlLevelName" },
                new FieldMetadata { Name = "Subjects", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "ControlOrganizations", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicControlTypesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicControlTypesQueryBuilder WithControlTypeName()
        {
            return WithScalarField("ControlTypeName");
        }

        public dicControlTypesQueryBuilder WithControlLevelName()
        {
            return WithScalarField("ControlLevelName");
        }

        public dicControlTypesQueryBuilder WithSubjects(SubjectQueryBuilder subjectQueryBuilder, SubjectCondition condition = null, SubjectFilter filter = null, IEnumerable<SubjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Subjects", subjectQueryBuilder, args);
        }

        public dicControlTypesQueryBuilder WithControlOrganizations(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, ControlOrganizationCondition condition = null, ControlOrganizationFilter filter = null, IEnumerable<ControlOrganizationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlOrganizations", controlOrganizationQueryBuilder, args);
        }

        public dicControlTypesQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicControlTypesQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class PersonAppointmentQueryBuilder : GraphQlQueryBuilder<PersonAppointmentQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "AppStartDate", IsComplex = true },
                new FieldMetadata { Name = "AppEndDate", IsComplex = true },
                new FieldMetadata { Name = "Person", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "Role", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public PersonAppointmentQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public PersonAppointmentQueryBuilder WithAppStartDate()
        {
            return WithScalarField("AppStartDate");
        }

        public PersonAppointmentQueryBuilder WithAppEndDate()
        {
            return WithScalarField("AppEndDate");
        }

        public PersonAppointmentQueryBuilder WithPerson(PersonQueryBuilder personQueryBuilder)
        {
            return WithObjectField("Person", personQueryBuilder);
        }

        public PersonAppointmentQueryBuilder WithRole(dicRoleQueryBuilder dicRoleQueryBuilder)
        {
            return WithObjectField("Role", dicRoleQueryBuilder);
        }

        public PersonAppointmentQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public PersonAppointmentQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicControlReasonDenyQueryBuilder : GraphQlQueryBuilder<dicControlReasonDenyQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlReasonDeny" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicControlReasonDenyQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicControlReasonDenyQueryBuilder WithControlReasonDeny()
        {
            return WithScalarField("ControlReasonDeny");
        }

        public dicControlReasonDenyQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicControlReasonDenyQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class CitizenRequestQueryBuilder : GraphQlQueryBuilder<CitizenRequestQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ApplicantName" },
                new FieldMetadata { Name = "ApplicantSurname" },
                new FieldMetadata { Name = "SecondName" },
                new FieldMetadata { Name = "ApplicantEmail" },
                new FieldMetadata { Name = "ApplicantPhone" },
                new FieldMetadata { Name = "RequestContent" },
                new FieldMetadata { Name = "ViolationCondition" },
                new FieldMetadata { Name = "RequestGetDate", IsComplex = true },
                new FieldMetadata { Name = "TargetControlOrganization" },
                new FieldMetadata { Name = "TargetControlOrganizationOGRN" },
                new FieldMetadata { Name = "ApplicantAddress", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public CitizenRequestQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public CitizenRequestQueryBuilder WithApplicantName()
        {
            return WithScalarField("ApplicantName");
        }

        public CitizenRequestQueryBuilder WithApplicantSurname()
        {
            return WithScalarField("ApplicantSurname");
        }

        public CitizenRequestQueryBuilder WithSecondName()
        {
            return WithScalarField("SecondName");
        }

        public CitizenRequestQueryBuilder WithApplicantEmail()
        {
            return WithScalarField("ApplicantEmail");
        }

        public CitizenRequestQueryBuilder WithApplicantPhone()
        {
            return WithScalarField("ApplicantPhone");
        }

        public CitizenRequestQueryBuilder WithRequestContent()
        {
            return WithScalarField("RequestContent");
        }

        public CitizenRequestQueryBuilder WithViolationCondition()
        {
            return WithScalarField("ViolationCondition");
        }

        public CitizenRequestQueryBuilder WithRequestGetDate()
        {
            return WithScalarField("RequestGetDate");
        }

        public CitizenRequestQueryBuilder WithTargetControlOrganization()
        {
            return WithScalarField("TargetControlOrganization");
        }

        public CitizenRequestQueryBuilder WithTargetControlOrganizationOgrn()
        {
            return WithScalarField("TargetControlOrganizationOGRN");
        }

        public CitizenRequestQueryBuilder WithApplicantAddress(AddressQueryBuilder addressQueryBuilder)
        {
            return WithObjectField("ApplicantAddress", addressQueryBuilder);
        }

        public CitizenRequestQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Materials", materialsQueryBuilder, args);
        }

        public CitizenRequestQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public CitizenRequestQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicKNMTypesQueryBuilder : GraphQlQueryBuilder<dicKNMTypesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "KNMTypeName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicKNMTypesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicKNMTypesQueryBuilder WithKnmTypeName()
        {
            return WithScalarField("KNMTypeName");
        }

        public dicKNMTypesQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicKNMTypesQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlListQuestionsQueryBuilder : GraphQlQueryBuilder<ControlListQuestionsQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "QuestionContent" },
                new FieldMetadata { Name = "ouid_bmQuestionInspection" },
                new FieldMetadata { Name = "ouid_bmInspectionListResult" },
                new FieldMetadata { Name = "LinkedControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "QuestionAnswers", IsComplex = true, QueryBuilderType = typeof(DicQuestionAnswersQueryBuilder) },
                new FieldMetadata { Name = "ControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "MandatoryReq", IsComplex = true, QueryBuilderType = typeof(MandatoryReqsQueryBuilder) },
                new FieldMetadata { Name = "NPA", IsComplex = true, QueryBuilderType = typeof(NPAQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlListQuestionsQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlListQuestionsQueryBuilder WithQuestionContent()
        {
            return WithScalarField("QuestionContent");
        }

        public ControlListQuestionsQueryBuilder WithOuidBmQuestionInspection()
        {
            return WithScalarField("ouid_bmQuestionInspection");
        }

        public ControlListQuestionsQueryBuilder WithOuidBmInspectionListResult()
        {
            return WithScalarField("ouid_bmInspectionListResult");
        }

        public ControlListQuestionsQueryBuilder WithLinkedControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("LinkedControlCard", controlCardQueryBuilder);
        }

        public ControlListQuestionsQueryBuilder WithQuestionAnswers(DicQuestionAnswersQueryBuilder dicQuestionAnswersQueryBuilder)
        {
            return WithObjectField("QuestionAnswers", dicQuestionAnswersQueryBuilder);
        }

        public ControlListQuestionsQueryBuilder WithControlList(docControlListQueryBuilder docControlListQueryBuilder)
        {
            return WithObjectField("ControlList", docControlListQueryBuilder);
        }

        public ControlListQuestionsQueryBuilder WithMandatoryReq(MandatoryReqsQueryBuilder mandatoryReqsQueryBuilder)
        {
            return WithObjectField("MandatoryReq", mandatoryReqsQueryBuilder);
        }

        public ControlListQuestionsQueryBuilder WithNpa(NPAQueryBuilder nPAQueryBuilder)
        {
            return WithObjectField("NPA", nPAQueryBuilder);
        }

        public ControlListQuestionsQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlListQuestionsQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicLicencedActivityTypesQueryBuilder : GraphQlQueryBuilder<dicLicencedActivityTypesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "LicensedActivityCode" },
                new FieldMetadata { Name = "LicensedActivityTypeName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicLicencedActivityTypesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicLicencedActivityTypesQueryBuilder WithLicensedActivityCode()
        {
            return WithScalarField("LicensedActivityCode");
        }

        public dicLicencedActivityTypesQueryBuilder WithLicensedActivityTypeName()
        {
            return WithScalarField("LicensedActivityTypeName");
        }

        public dicLicencedActivityTypesQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicLicencedActivityTypesQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlObjectQueryBuilder : GraphQlQueryBuilder<ControlObjectQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlObjectName" },
                new FieldMetadata { Name = "Address", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "Subject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "CodeOKTMO", IsComplex = true, QueryBuilderType = typeof(dicOKTMOQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "RiskCategory", IsComplex = true, QueryBuilderType = typeof(RiskCatQueryBuilder) },
                new FieldMetadata { Name = "HazardClass", IsComplex = true, QueryBuilderType = typeof(HazardClassQueryBuilder) },
                new FieldMetadata { Name = "ExtendedAttribValues", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribValueQueryBuilder) },
                new FieldMetadata { Name = "ControlItemPassport", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "ControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlObjectQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlObjectQueryBuilder WithControlObjectName()
        {
            return WithScalarField("ControlObjectName");
        }

        public ControlObjectQueryBuilder WithAddress(AddressQueryBuilder addressQueryBuilder)
        {
            return WithObjectField("Address", addressQueryBuilder);
        }

        public ControlObjectQueryBuilder WithSubject(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("Subject", subjectQueryBuilder);
        }

        public ControlObjectQueryBuilder WithCodeOktmo(dicOKTMOQueryBuilder dicOKTMOQueryBuilder)
        {
            return WithObjectField("CodeOKTMO", dicOKTMOQueryBuilder);
        }

        public ControlObjectQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder)
        {
            return WithObjectField("ControlType", dicControlTypesQueryBuilder);
        }

        public ControlObjectQueryBuilder WithRiskCategory(RiskCatQueryBuilder riskCatQueryBuilder, RiskCatCondition condition = null, RiskCatFilter filter = null, IEnumerable<RiskCatSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("RiskCategory", riskCatQueryBuilder, args);
        }

        public ControlObjectQueryBuilder WithHazardClass(HazardClassQueryBuilder hazardClassQueryBuilder, HazardClassCondition condition = null, HazardClassFilter filter = null, IEnumerable<HazardClassSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("HazardClass", hazardClassQueryBuilder, args);
        }

        public ControlObjectQueryBuilder WithExtendedAttribValues(ExtendedAttribValueQueryBuilder extendedAttribValueQueryBuilder, ExtendedAttribValueCondition condition = null, ExtendedAttribValueFilter filter = null, IEnumerable<ExtendedAttribValueSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ExtendedAttribValues", extendedAttribValueQueryBuilder, args);
        }

        public ControlObjectQueryBuilder WithControlItemPassport(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder, ControlItemPassportCondition condition = null, ControlItemPassportFilter filter = null, IEnumerable<ControlItemPassportSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItemPassport", controlItemPassportQueryBuilder, args);
        }

        public ControlObjectQueryBuilder WithControlFile(ControlFileQueryBuilder controlFileQueryBuilder)
        {
            return WithObjectField("ControlFile", controlFileQueryBuilder);
        }

        public ControlObjectQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlObjectQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicPunishmentTypeQueryBuilder : GraphQlQueryBuilder<dicPunishmentTypeQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "PunishmentType" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicPunishmentTypeQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicPunishmentTypeQueryBuilder WithPunishmentType()
        {
            return WithScalarField("PunishmentType");
        }

        public dicPunishmentTypeQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicPunishmentTypeQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicOKOPFQueryBuilder : GraphQlQueryBuilder<dicOKOPFQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "SubjectName" },
                new FieldMetadata { Name = "CodeOKOPF" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicOKOPFQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicOKOPFQueryBuilder WithSubjectName()
        {
            return WithScalarField("SubjectName");
        }

        public dicOKOPFQueryBuilder WithCodeOkopf()
        {
            return WithScalarField("CodeOKOPF");
        }

        public dicOKOPFQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicOKOPFQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicProsecQueryBuilder : GraphQlQueryBuilder<dicProsecQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ProsecName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicProsecQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicProsecQueryBuilder WithProsecName()
        {
            return WithScalarField("ProsecName");
        }

        public dicProsecQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicProsecQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class NPAQueryBuilder : GraphQlQueryBuilder<NPAQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "NPAName" },
                new FieldMetadata { Name = "ApproveDate", IsComplex = true },
                new FieldMetadata { Name = "ApproveEntity" },
                new FieldMetadata { Name = "NPAEndDate", IsComplex = true },
                new FieldMetadata { Name = "Number" },
                new FieldMetadata { Name = "Body" },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "MandatoryReqs", IsComplex = true, QueryBuilderType = typeof(MandatoryReqsQueryBuilder) },
                new FieldMetadata { Name = "ControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "NPAType", IsComplex = true, QueryBuilderType = typeof(dicNPATypesQueryBuilder) },
                new FieldMetadata { Name = "NPALevel", IsComplex = true, QueryBuilderType = typeof(dicNPALevelsQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public NPAQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public NPAQueryBuilder WithNpaName()
        {
            return WithScalarField("NPAName");
        }

        public NPAQueryBuilder WithApproveDate()
        {
            return WithScalarField("ApproveDate");
        }

        public NPAQueryBuilder WithApproveEntity()
        {
            return WithScalarField("ApproveEntity");
        }

        public NPAQueryBuilder WithNpaEndDate()
        {
            return WithScalarField("NPAEndDate");
        }

        public NPAQueryBuilder WithNumber()
        {
            return WithScalarField("Number");
        }

        public NPAQueryBuilder WithBody()
        {
            return WithScalarField("Body");
        }

        public NPAQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder)
        {
            return WithObjectField("ControlType", dicControlTypesQueryBuilder);
        }

        public NPAQueryBuilder WithMandatoryReqs(MandatoryReqsQueryBuilder mandatoryReqsQueryBuilder, MandatoryReqsCondition condition = null, MandatoryReqsFilter filter = null, IEnumerable<MandatoryReqsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("MandatoryReqs", mandatoryReqsQueryBuilder, args);
        }

        public NPAQueryBuilder WithControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsCondition condition = null, ControlListQuestionsFilter filter = null, IEnumerable<ControlListQuestionsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public NPAQueryBuilder WithNpaType(dicNPATypesQueryBuilder dicNPATypesQueryBuilder)
        {
            return WithObjectField("NPAType", dicNPATypesQueryBuilder);
        }

        public NPAQueryBuilder WithNpaLevel(dicNPALevelsQueryBuilder dicNPALevelsQueryBuilder)
        {
            return WithObjectField("NPALevel", dicNPALevelsQueryBuilder);
        }

        public NPAQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public NPAQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class DicStatesQueryBuilder : GraphQlQueryBuilder<DicStatesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "state" },
                new FieldMetadata { Name = "step" },
                new FieldMetadata { Name = "infotxt" }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public DicStatesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public DicStatesQueryBuilder WithState()
        {
            return WithScalarField("state");
        }

        public DicStatesQueryBuilder WithStep()
        {
            return WithScalarField("step");
        }

        public DicStatesQueryBuilder WithInfotxt()
        {
            return WithScalarField("infotxt");
        }
    }

    public partial class CheckOutEGRIPQueryBuilder : GraphQlQueryBuilder<CheckOutEGRIPQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "LastRenewData", IsComplex = true },
                new FieldMetadata { Name = "OGRNIP" },
                new FieldMetadata { Name = "INNIP" },
                new FieldMetadata { Name = "OGRNIPdate", IsComplex = true },
                new FieldMetadata { Name = "grnEgripDate", IsComplex = true },
                new FieldMetadata { Name = "regIpOld" },
                new FieldMetadata { Name = "orgRegLocationIp" },
                new FieldMetadata { Name = "addressOrgRegLocIp" },
                new FieldMetadata { Name = "regNumberPfrIp" },
                new FieldMetadata { Name = "dateRegNumPfrIp", IsComplex = true },
                new FieldMetadata { Name = "branchPFRIP" },
                new FieldMetadata { Name = "grnRegPFRIP" },
                new FieldMetadata { Name = "dateGrnRegPfrIp", IsComplex = true },
                new FieldMetadata { Name = "dateUpdateEgrIP", IsComplex = true },
                new FieldMetadata { Name = "SubjectEGRIP", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "Address", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public CheckOutEGRIPQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public CheckOutEGRIPQueryBuilder WithLastRenewData()
        {
            return WithScalarField("LastRenewData");
        }

        public CheckOutEGRIPQueryBuilder WithOgrnip()
        {
            return WithScalarField("OGRNIP");
        }

        public CheckOutEGRIPQueryBuilder WithInnip()
        {
            return WithScalarField("INNIP");
        }

        public CheckOutEGRIPQueryBuilder WithOgrniPdate()
        {
            return WithScalarField("OGRNIPdate");
        }

        public CheckOutEGRIPQueryBuilder WithGrnEgripDate()
        {
            return WithScalarField("grnEgripDate");
        }

        public CheckOutEGRIPQueryBuilder WithRegIpOld()
        {
            return WithScalarField("regIpOld");
        }

        public CheckOutEGRIPQueryBuilder WithOrgRegLocationIp()
        {
            return WithScalarField("orgRegLocationIp");
        }

        public CheckOutEGRIPQueryBuilder WithAddressOrgRegLocIp()
        {
            return WithScalarField("addressOrgRegLocIp");
        }

        public CheckOutEGRIPQueryBuilder WithRegNumberPfrIp()
        {
            return WithScalarField("regNumberPfrIp");
        }

        public CheckOutEGRIPQueryBuilder WithDateRegNumPfrIp()
        {
            return WithScalarField("dateRegNumPfrIp");
        }

        public CheckOutEGRIPQueryBuilder WithBranchPfrip()
        {
            return WithScalarField("branchPFRIP");
        }

        public CheckOutEGRIPQueryBuilder WithGrnRegPfrip()
        {
            return WithScalarField("grnRegPFRIP");
        }

        public CheckOutEGRIPQueryBuilder WithDateGrnRegPfrIp()
        {
            return WithScalarField("dateGrnRegPfrIp");
        }

        public CheckOutEGRIPQueryBuilder WithDateUpdateEgrIp()
        {
            return WithScalarField("dateUpdateEgrIP");
        }

        public CheckOutEGRIPQueryBuilder WithSubjectEgrip(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("SubjectEGRIP", subjectQueryBuilder);
        }

        public CheckOutEGRIPQueryBuilder WithAddress(AddressQueryBuilder addressQueryBuilder)
        {
            return WithObjectField("Address", addressQueryBuilder);
        }

        public CheckOutEGRIPQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public CheckOutEGRIPQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicDamageTypeQueryBuilder : GraphQlQueryBuilder<dicDamageTypeQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "DamageType" },
                new FieldMetadata { Name = "ControlItemResults", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicDamageTypeQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicDamageTypeQueryBuilder WithDamageType()
        {
            return WithScalarField("DamageType");
        }

        public dicDamageTypeQueryBuilder WithControlItemResults(ControlItemResultQueryBuilder controlItemResultQueryBuilder, ControlItemResultCondition condition = null, ControlItemResultFilter filter = null, IEnumerable<ControlItemResultSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItemResults", controlItemResultQueryBuilder, args);
        }

        public dicDamageTypeQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicDamageTypeQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicNPALevelsQueryBuilder : GraphQlQueryBuilder<dicNPALevelsQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "NPALevelsName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicNPALevelsQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicNPALevelsQueryBuilder WithNpaLevelsName()
        {
            return WithScalarField("NPALevelsName");
        }

        public dicNPALevelsQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicNPALevelsQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class SubjectQueryBuilder : GraphQlQueryBuilder<SubjectQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "mainName" },
                new FieldMetadata { Name = "Address" },
                new FieldMetadata { Name = "Citizenship" },
                new FieldMetadata { Name = "RepresentativeSurname" },
                new FieldMetadata { Name = "RepresentativeName" },
                new FieldMetadata { Name = "RepresentativeSecondName" },
                new FieldMetadata { Name = "NameOrg" },
                new FieldMetadata { Name = "ControlObjects", IsComplex = true, QueryBuilderType = typeof(ControlObjectQueryBuilder) },
                new FieldMetadata { Name = "CodeOKTMO", IsComplex = true, QueryBuilderType = typeof(dicOKTMOQueryBuilder) },
                new FieldMetadata { Name = "CheckOutEGRSMSPs", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRSMSPQueryBuilder) },
                new FieldMetadata { Name = "CheckOutEGRIPs", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRIPQueryBuilder) },
                new FieldMetadata { Name = "SubjectTypes", IsComplex = true, QueryBuilderType = typeof(dicSubjectTypeQueryBuilder) },
                new FieldMetadata { Name = "CodeEGRULs", IsComplex = true, QueryBuilderType = typeof(CheckOutEGRULQueryBuilder) },
                new FieldMetadata { Name = "CodeOKSM", IsComplex = true, QueryBuilderType = typeof(dicOKSMQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "Licences", IsComplex = true, QueryBuilderType = typeof(LicenceQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "CodeOKVEDs", IsComplex = true, QueryBuilderType = typeof(ActivityQueryBuilder) },
                new FieldMetadata { Name = "RiskCategory", IsComplex = true, QueryBuilderType = typeof(RiskCatQueryBuilder) },
                new FieldMetadata { Name = "AssignedOfficer", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "ControlItemPassports", IsComplex = true, QueryBuilderType = typeof(ControlItemPassportQueryBuilder) },
                new FieldMetadata { Name = "Filials", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "HeadOrganization", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public SubjectQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public SubjectQueryBuilder WithMainName()
        {
            return WithScalarField("mainName");
        }

        public SubjectQueryBuilder WithAddress()
        {
            return WithScalarField("Address");
        }

        public SubjectQueryBuilder WithCitizenship()
        {
            return WithScalarField("Citizenship");
        }

        public SubjectQueryBuilder WithRepresentativeSurname()
        {
            return WithScalarField("RepresentativeSurname");
        }

        public SubjectQueryBuilder WithRepresentativeName()
        {
            return WithScalarField("RepresentativeName");
        }

        public SubjectQueryBuilder WithRepresentativeSecondName()
        {
            return WithScalarField("RepresentativeSecondName");
        }

        public SubjectQueryBuilder WithNameOrg()
        {
            return WithScalarField("NameOrg");
        }

        public SubjectQueryBuilder WithControlObjects(ControlObjectQueryBuilder controlObjectQueryBuilder, ControlObjectCondition condition = null, ControlObjectFilter filter = null, IEnumerable<ControlObjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlObjects", controlObjectQueryBuilder, args);
        }

        public SubjectQueryBuilder WithCodeOktmo(dicOKTMOQueryBuilder dicOKTMOQueryBuilder)
        {
            return WithObjectField("CodeOKTMO", dicOKTMOQueryBuilder);
        }

        public SubjectQueryBuilder WithCheckOutEgrsmsPs(CheckOutEGRSMSPQueryBuilder checkOutEGRSMSPQueryBuilder, CheckOutEGRSMSPCondition condition = null, CheckOutEGRSMSPFilter filter = null, IEnumerable<CheckOutEGRSMSPSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("CheckOutEGRSMSPs", checkOutEGRSMSPQueryBuilder, args);
        }

        public SubjectQueryBuilder WithCheckOutEgriPs(CheckOutEGRIPQueryBuilder checkOutEGRIPQueryBuilder, CheckOutEGRIPCondition condition = null, CheckOutEGRIPFilter filter = null, IEnumerable<CheckOutEGRIPSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("CheckOutEGRIPs", checkOutEGRIPQueryBuilder, args);
        }

        public SubjectQueryBuilder WithSubjectTypes(dicSubjectTypeQueryBuilder dicSubjectTypeQueryBuilder)
        {
            return WithObjectField("SubjectTypes", dicSubjectTypeQueryBuilder);
        }

        public SubjectQueryBuilder WithCodeEgruLs(CheckOutEGRULQueryBuilder checkOutEGRULQueryBuilder, CheckOutEGRULCondition condition = null, CheckOutEGRULFilter filter = null, IEnumerable<CheckOutEGRULSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("CodeEGRULs", checkOutEGRULQueryBuilder, args);
        }

        public SubjectQueryBuilder WithCodeOksm(dicOKSMQueryBuilder dicOKSMQueryBuilder)
        {
            return WithObjectField("CodeOKSM", dicOKSMQueryBuilder);
        }

        public SubjectQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Materials", materialsQueryBuilder, args);
        }

        public SubjectQueryBuilder WithLicences(LicenceQueryBuilder licenceQueryBuilder, LicenceCondition condition = null, LicenceFilter filter = null, IEnumerable<LicenceSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Licences", licenceQueryBuilder, args);
        }

        public SubjectQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder, dicControlTypesCondition condition = null, dicControlTypesFilter filter = null, IEnumerable<dicControlTypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlType", dicControlTypesQueryBuilder, args);
        }

        public SubjectQueryBuilder WithCodeOkveDs(ActivityQueryBuilder activityQueryBuilder, ActivityCondition condition = null, ActivityFilter filter = null, IEnumerable<ActivitySort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("CodeOKVEDs", activityQueryBuilder, args);
        }

        public SubjectQueryBuilder WithRiskCategory(RiskCatQueryBuilder riskCatQueryBuilder, RiskCatCondition condition = null, RiskCatFilter filter = null, IEnumerable<RiskCatSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("RiskCategory", riskCatQueryBuilder, args);
        }

        public SubjectQueryBuilder WithAssignedOfficer(PersonQueryBuilder personQueryBuilder)
        {
            return WithObjectField("AssignedOfficer", personQueryBuilder);
        }

        public SubjectQueryBuilder WithControlItemPassports(ControlItemPassportQueryBuilder controlItemPassportQueryBuilder, ControlItemPassportCondition condition = null, ControlItemPassportFilter filter = null, IEnumerable<ControlItemPassportSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlItemPassports", controlItemPassportQueryBuilder, args);
        }

        public SubjectQueryBuilder WithFilials(SubjectQueryBuilder subjectQueryBuilder, SubjectCondition condition = null, SubjectFilter filter = null, IEnumerable<SubjectSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Filials", subjectQueryBuilder, args);
        }

        public SubjectQueryBuilder WithHeadOrganization(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("HeadOrganization", subjectQueryBuilder);
        }

        public SubjectQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public SubjectQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class RandEParameterQueryBuilder : GraphQlQueryBuilder<RandEParameterQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "Name" },
                new FieldMetadata { Name = "ControlOrganizationParameter", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "MeasureUnitType", IsComplex = true, QueryBuilderType = typeof(DicMeasureUnitsTypeQueryBuilder) },
                new FieldMetadata { Name = "RandEParameterValues", IsComplex = true, QueryBuilderType = typeof(RandEParameterValueQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public RandEParameterQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public RandEParameterQueryBuilder WithName()
        {
            return WithScalarField("Name");
        }

        public RandEParameterQueryBuilder WithControlOrganizationParameter(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, ControlOrganizationCondition condition = null, ControlOrganizationFilter filter = null, IEnumerable<ControlOrganizationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlOrganizationParameter", controlOrganizationQueryBuilder, args);
        }

        public RandEParameterQueryBuilder WithMeasureUnitType(DicMeasureUnitsTypeQueryBuilder dicMeasureUnitsTypeQueryBuilder)
        {
            return WithObjectField("MeasureUnitType", dicMeasureUnitsTypeQueryBuilder);
        }

        public RandEParameterQueryBuilder WithRandEParameterValues(RandEParameterValueQueryBuilder randEParameterValueQueryBuilder, RandEParameterValueCondition condition = null, RandEParameterValueFilter filter = null, IEnumerable<RandEParameterValueSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("RandEParameterValues", randEParameterValueQueryBuilder, args);
        }

        public RandEParameterQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public RandEParameterQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicRiskCategoryQueryBuilder : GraphQlQueryBuilder<dicRiskCategoryQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "RiskCategoryName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicRiskCategoryQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicRiskCategoryQueryBuilder WithRiskCategoryName()
        {
            return WithScalarField("RiskCategoryName");
        }

        public dicRiskCategoryQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicRiskCategoryQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class docControlListQueryBuilder : GraphQlQueryBuilder<docControlListQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlListStartDate", IsComplex = true },
                new FieldMetadata { Name = "ControlListEndDate", IsComplex = true },
                new FieldMetadata { Name = "ControlItemResult", IsComplex = true, QueryBuilderType = typeof(ControlItemResultQueryBuilder) },
                new FieldMetadata { Name = "ControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "ControlListStatus", IsComplex = true, QueryBuilderType = typeof(dicControlListStatusQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public docControlListQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public docControlListQueryBuilder WithControlListStartDate()
        {
            return WithScalarField("ControlListStartDate");
        }

        public docControlListQueryBuilder WithControlListEndDate()
        {
            return WithScalarField("ControlListEndDate");
        }

        public docControlListQueryBuilder WithControlItemResult(ControlItemResultQueryBuilder controlItemResultQueryBuilder)
        {
            return WithObjectField("ControlItemResult", controlItemResultQueryBuilder);
        }

        public docControlListQueryBuilder WithControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsCondition condition = null, ControlListQuestionsFilter filter = null, IEnumerable<ControlListQuestionsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public docControlListQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder)
        {
            return WithObjectField("Materials", materialsQueryBuilder);
        }

        public docControlListQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder)
        {
            return WithObjectField("ControlType", dicControlTypesQueryBuilder);
        }

        public docControlListQueryBuilder WithControlListStatus(dicControlListStatusQueryBuilder dicControlListStatusQueryBuilder)
        {
            return WithObjectField("ControlListStatus", dicControlListStatusQueryBuilder);
        }

        public docControlListQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public docControlListQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class LicenceQueryBuilder : GraphQlQueryBuilder<LicenceQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "LicenceName" },
                new FieldMetadata { Name = "Series" },
                new FieldMetadata { Name = "Number" },
                new FieldMetadata { Name = "startDate", IsComplex = true },
                new FieldMetadata { Name = "stopDate", IsComplex = true },
                new FieldMetadata { Name = "endDate", IsComplex = true },
                new FieldMetadata { Name = "BlankNumber" },
                new FieldMetadata { Name = "licenseFinalDecision" },
                new FieldMetadata { Name = "licenseFinalStartDate", IsComplex = true },
                new FieldMetadata { Name = "licenseFinalEndDate", IsComplex = true },
                new FieldMetadata { Name = "LicenceStatus", IsComplex = true, QueryBuilderType = typeof(dicLicenceStatusQueryBuilder) },
                new FieldMetadata { Name = "Subject", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "ActivityType", IsComplex = true, QueryBuilderType = typeof(dicLicencedActivityTypesQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public LicenceQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public LicenceQueryBuilder WithLicenceName()
        {
            return WithScalarField("LicenceName");
        }

        public LicenceQueryBuilder WithSeries()
        {
            return WithScalarField("Series");
        }

        public LicenceQueryBuilder WithNumber()
        {
            return WithScalarField("Number");
        }

        public LicenceQueryBuilder WithStartDate()
        {
            return WithScalarField("startDate");
        }

        public LicenceQueryBuilder WithStopDate()
        {
            return WithScalarField("stopDate");
        }

        public LicenceQueryBuilder WithEndDate()
        {
            return WithScalarField("endDate");
        }

        public LicenceQueryBuilder WithBlankNumber()
        {
            return WithScalarField("BlankNumber");
        }

        public LicenceQueryBuilder WithLicenseFinalDecision()
        {
            return WithScalarField("licenseFinalDecision");
        }

        public LicenceQueryBuilder WithLicenseFinalStartDate()
        {
            return WithScalarField("licenseFinalStartDate");
        }

        public LicenceQueryBuilder WithLicenseFinalEndDate()
        {
            return WithScalarField("licenseFinalEndDate");
        }

        public LicenceQueryBuilder WithLicenceStatus(dicLicenceStatusQueryBuilder dicLicenceStatusQueryBuilder)
        {
            return WithObjectField("LicenceStatus", dicLicenceStatusQueryBuilder);
        }

        public LicenceQueryBuilder WithSubject(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("Subject", subjectQueryBuilder);
        }

        public LicenceQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder, MaterialsCondition condition = null, MaterialsFilter filter = null, IEnumerable<MaterialsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Materials", materialsQueryBuilder, args);
        }

        public LicenceQueryBuilder WithActivityType(dicLicencedActivityTypesQueryBuilder dicLicencedActivityTypesQueryBuilder)
        {
            return WithObjectField("ActivityType", dicLicencedActivityTypesQueryBuilder);
        }

        public LicenceQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public LicenceQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicHazardClassQueryBuilder : GraphQlQueryBuilder<dicHazardClassQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "HazardClassName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicHazardClassQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicHazardClassQueryBuilder WithHazardClassName()
        {
            return WithScalarField("HazardClassName");
        }

        public dicHazardClassQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicHazardClassQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlOrganizationQueryBuilder : GraphQlQueryBuilder<ControlOrganizationQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlOrganizationName" },
                new FieldMetadata { Name = "OGRN_KNO" },
                new FieldMetadata { Name = "Divisions", IsComplex = true, QueryBuilderType = typeof(OrganizationUnitQueryBuilder) },
                new FieldMetadata { Name = "OrganizationRoles", IsComplex = true, QueryBuilderType = typeof(dicRoleQueryBuilder) },
                new FieldMetadata { Name = "Employees", IsComplex = true, QueryBuilderType = typeof(PersonQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "SubOrganizations", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "HeadOrganization", IsComplex = true, QueryBuilderType = typeof(ControlOrganizationQueryBuilder) },
                new FieldMetadata { Name = "RandEParameters", IsComplex = true, QueryBuilderType = typeof(RandEParameterQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlOrganizationQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlOrganizationQueryBuilder WithControlOrganizationName()
        {
            return WithScalarField("ControlOrganizationName");
        }

        public ControlOrganizationQueryBuilder WithOgrnKno()
        {
            return WithScalarField("OGRN_KNO");
        }

        public ControlOrganizationQueryBuilder WithDivisions(OrganizationUnitQueryBuilder organizationUnitQueryBuilder, OrganizationUnitCondition condition = null, OrganizationUnitFilter filter = null, IEnumerable<OrganizationUnitSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Divisions", organizationUnitQueryBuilder, args);
        }

        public ControlOrganizationQueryBuilder WithOrganizationRoles(dicRoleQueryBuilder dicRoleQueryBuilder, dicRoleCondition condition = null, dicRoleFilter filter = null, IEnumerable<dicRoleSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("OrganizationRoles", dicRoleQueryBuilder, args);
        }

        public ControlOrganizationQueryBuilder WithEmployees(PersonQueryBuilder personQueryBuilder, PersonCondition condition = null, PersonFilter filter = null, IEnumerable<PersonSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("Employees", personQueryBuilder, args);
        }

        public ControlOrganizationQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder, dicControlTypesCondition condition = null, dicControlTypesFilter filter = null, IEnumerable<dicControlTypesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlType", dicControlTypesQueryBuilder, args);
        }

        public ControlOrganizationQueryBuilder WithSubOrganizations(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder, ControlOrganizationCondition condition = null, ControlOrganizationFilter filter = null, IEnumerable<ControlOrganizationSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("SubOrganizations", controlOrganizationQueryBuilder, args);
        }

        public ControlOrganizationQueryBuilder WithHeadOrganization(ControlOrganizationQueryBuilder controlOrganizationQueryBuilder)
        {
            return WithObjectField("HeadOrganization", controlOrganizationQueryBuilder);
        }

        public ControlOrganizationQueryBuilder WithRandEParameters(RandEParameterQueryBuilder randEParameterQueryBuilder, RandEParameterCondition condition = null, RandEParameterFilter filter = null, IEnumerable<RandEParameterSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("RandEParameters", randEParameterQueryBuilder, args);
        }

        public ControlOrganizationQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlOrganizationQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class docControlActQueryBuilder : GraphQlQueryBuilder<docControlActQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlActCreateDate", IsComplex = true },
                new FieldMetadata { Name = "ActLinkedtoControlCard", IsComplex = true, QueryBuilderType = typeof(ControlCardQueryBuilder) },
                new FieldMetadata { Name = "ControlList", IsComplex = true, QueryBuilderType = typeof(docControlListQueryBuilder) },
                new FieldMetadata { Name = "Materials", IsComplex = true, QueryBuilderType = typeof(MaterialsQueryBuilder) },
                new FieldMetadata { Name = "ControlActCreatePlace", IsComplex = true, QueryBuilderType = typeof(AddressQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public docControlActQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public docControlActQueryBuilder WithControlActCreateDate()
        {
            return WithScalarField("ControlActCreateDate");
        }

        public docControlActQueryBuilder WithActLinkedtoControlCard(ControlCardQueryBuilder controlCardQueryBuilder)
        {
            return WithObjectField("ActLinkedtoControlCard", controlCardQueryBuilder);
        }

        public docControlActQueryBuilder WithControlList(docControlListQueryBuilder docControlListQueryBuilder)
        {
            return WithObjectField("ControlList", docControlListQueryBuilder);
        }

        public docControlActQueryBuilder WithMaterials(MaterialsQueryBuilder materialsQueryBuilder)
        {
            return WithObjectField("Materials", materialsQueryBuilder);
        }

        public docControlActQueryBuilder WithControlActCreatePlace(AddressQueryBuilder addressQueryBuilder)
        {
            return WithObjectField("ControlActCreatePlace", addressQueryBuilder);
        }

        public docControlActQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public docControlActQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class MandatoryReqsQueryBuilder : GraphQlQueryBuilder<MandatoryReqsQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "MandratoryReqContent" },
                new FieldMetadata { Name = "StartDateMandatory", IsComplex = true },
                new FieldMetadata { Name = "EndDateMandatory", IsComplex = true },
                new FieldMetadata { Name = "NPA", IsComplex = true, QueryBuilderType = typeof(NPAQueryBuilder) },
                new FieldMetadata { Name = "ControlListQuestions", IsComplex = true, QueryBuilderType = typeof(ControlListQuestionsQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public MandatoryReqsQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public MandatoryReqsQueryBuilder WithMandratoryReqContent()
        {
            return WithScalarField("MandratoryReqContent");
        }

        public MandatoryReqsQueryBuilder WithStartDateMandatory()
        {
            return WithScalarField("StartDateMandatory");
        }

        public MandatoryReqsQueryBuilder WithEndDateMandatory()
        {
            return WithScalarField("EndDateMandatory");
        }

        public MandatoryReqsQueryBuilder WithNpa(NPAQueryBuilder nPAQueryBuilder, NPACondition condition = null, NPAFilter filter = null, IEnumerable<NPASort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("NPA", nPAQueryBuilder, args);
        }

        public MandatoryReqsQueryBuilder WithControlListQuestions(ControlListQuestionsQueryBuilder controlListQuestionsQueryBuilder, ControlListQuestionsCondition condition = null, ControlListQuestionsFilter filter = null, IEnumerable<ControlListQuestionsSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ControlListQuestions", controlListQuestionsQueryBuilder, args);
        }

        public MandatoryReqsQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder)
        {
            return WithObjectField("ControlType", dicControlTypesQueryBuilder);
        }

        public MandatoryReqsQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public MandatoryReqsQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicNPATypesQueryBuilder : GraphQlQueryBuilder<dicNPATypesQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "NPATypeName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicNPATypesQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicNPATypesQueryBuilder WithNpaTypeName()
        {
            return WithScalarField("NPATypeName");
        }

        public dicNPATypesQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicNPATypesQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class dicControlBaseQueryBuilder : GraphQlQueryBuilder<dicControlBaseQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ControlBaseName" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public dicControlBaseQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public dicControlBaseQueryBuilder WithControlBaseName()
        {
            return WithScalarField("ControlBaseName");
        }

        public dicControlBaseQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public dicControlBaseQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class DicQuestionAnswersQueryBuilder : GraphQlQueryBuilder<DicQuestionAnswersQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "QuestionAnswer" },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public DicQuestionAnswersQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public DicQuestionAnswersQueryBuilder WithQuestionAnswer()
        {
            return WithScalarField("QuestionAnswer");
        }

        public DicQuestionAnswersQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public DicQuestionAnswersQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class CheckOutEGRSMSPQueryBuilder : GraphQlQueryBuilder<CheckOutEGRSMSPQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "dateRMSP", IsComplex = true },
                new FieldMetadata { Name = "caseNumber" },
                new FieldMetadata { Name = "IsInRMSP" },
                new FieldMetadata { Name = "LastRenewData", IsComplex = true },
                new FieldMetadata { Name = "SubjectEGRSMSP", IsComplex = true, QueryBuilderType = typeof(SubjectQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public CheckOutEGRSMSPQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public CheckOutEGRSMSPQueryBuilder WithDateRmsp()
        {
            return WithScalarField("dateRMSP");
        }

        public CheckOutEGRSMSPQueryBuilder WithCaseNumber()
        {
            return WithScalarField("caseNumber");
        }

        public CheckOutEGRSMSPQueryBuilder WithIsInRmsp()
        {
            return WithScalarField("IsInRMSP");
        }

        public CheckOutEGRSMSPQueryBuilder WithLastRenewData()
        {
            return WithScalarField("LastRenewData");
        }

        public CheckOutEGRSMSPQueryBuilder WithSubjectEgrsmsp(SubjectQueryBuilder subjectQueryBuilder)
        {
            return WithObjectField("SubjectEGRSMSP", subjectQueryBuilder);
        }

        public CheckOutEGRSMSPQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public CheckOutEGRSMSPQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ControlJournalsQueryBuilder : GraphQlQueryBuilder<ControlJournalsQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "JournalCreationDate" },
                new FieldMetadata { Name = "JournalName" },
                new FieldMetadata { Name = "ControlFile", IsComplex = true, QueryBuilderType = typeof(ControlFileQueryBuilder) },
                new FieldMetadata { Name = "JournalAttributes", IsComplex = true, QueryBuilderType = typeof(JournalAttributesQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ControlJournalsQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ControlJournalsQueryBuilder WithJournalCreationDate()
        {
            return WithScalarField("JournalCreationDate");
        }

        public ControlJournalsQueryBuilder WithJournalName()
        {
            return WithScalarField("JournalName");
        }

        public ControlJournalsQueryBuilder WithControlFile(ControlFileQueryBuilder controlFileQueryBuilder)
        {
            return WithObjectField("ControlFile", controlFileQueryBuilder);
        }

        public ControlJournalsQueryBuilder WithJournalAttributes(JournalAttributesQueryBuilder journalAttributesQueryBuilder, JournalAttributesCondition condition = null, JournalAttributesFilter filter = null, IEnumerable<JournalAttributesSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("JournalAttributes", journalAttributesQueryBuilder, args);
        }

        public ControlJournalsQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ControlJournalsQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }

    public partial class ExtendedAttribQueryBuilder : GraphQlQueryBuilder<ExtendedAttribQueryBuilder>
    {
        private static readonly FieldMetadata[] AllFieldMetadata =
            new[]
            {
                new FieldMetadata { Name = "id" },
                new FieldMetadata { Name = "ExtAttributeName" },
                new FieldMetadata { Name = "ExtAttributeContentUnit" },
                new FieldMetadata { Name = "ExtAttributeTitle" },
                new FieldMetadata { Name = "ExtendedAttribValues", IsComplex = true, QueryBuilderType = typeof(ExtendedAttribValueQueryBuilder) },
                new FieldMetadata { Name = "ControlType", IsComplex = true, QueryBuilderType = typeof(dicControlTypesQueryBuilder) },
                new FieldMetadata { Name = "State", IsComplex = true, QueryBuilderType = typeof(DicStatesQueryBuilder) },
                new FieldMetadata { Name = "_meta", IsComplex = true, QueryBuilderType = typeof(MetaQueryBuilder) }
            };

        protected override IList<FieldMetadata> AllFields { get { return AllFieldMetadata; } }

        public ExtendedAttribQueryBuilder WithId()
        {
            return WithScalarField("id");
        }

        public ExtendedAttribQueryBuilder WithExtAttributeName()
        {
            return WithScalarField("ExtAttributeName");
        }

        public ExtendedAttribQueryBuilder WithExtAttributeContentUnit()
        {
            return WithScalarField("ExtAttributeContentUnit");
        }

        public ExtendedAttribQueryBuilder WithExtAttributeTitle()
        {
            return WithScalarField("ExtAttributeTitle");
        }

        public ExtendedAttribQueryBuilder WithExtendedAttribValues(ExtendedAttribValueQueryBuilder extendedAttribValueQueryBuilder, ExtendedAttribValueCondition condition = null, ExtendedAttribValueFilter filter = null, IEnumerable<ExtendedAttribValueSort> sort = null, int? limit = null, int? offset = null)
        {
            var args = new Dictionary<string, object>();
            if (condition != null)
                args.Add("condition", condition);

            if (filter != null)
                args.Add("filter", filter);

            if (sort != null)
                args.Add("sort", sort);

            if (limit != null)
                args.Add("limit", limit);

            if (offset != null)
                args.Add("offset", offset);

            return WithObjectField("ExtendedAttribValues", extendedAttribValueQueryBuilder, args);
        }

        public ExtendedAttribQueryBuilder WithControlType(dicControlTypesQueryBuilder dicControlTypesQueryBuilder)
        {
            return WithObjectField("ControlType", dicControlTypesQueryBuilder);
        }

        public ExtendedAttribQueryBuilder WithState(DicStatesQueryBuilder dicStatesQueryBuilder)
        {
            return WithObjectField("State", dicStatesQueryBuilder);
        }

        public ExtendedAttribQueryBuilder WithMeta(MetaQueryBuilder metaQueryBuilder)
        {
            return WithObjectField("_meta", metaQueryBuilder);
        }
    }
    #endregion



    #region input classes
    public partial class ControlListQuestionsUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string QuestionContent { get; set; }
        public string OuidBmQuestionInspection { get; set; }
        public string OuidBmInspectionListResult { get; set; }
        public Guid? LinkedControlCardId { get; set; }
        public Guid? QuestionAnswersId { get; set; }
        public Guid? ControlListId { get; set; }
        public Guid? MandatoryReqId { get; set; }
        public Guid? NpaId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionContent", Value = QuestionContent };
            yield return new InputPropertyInfo { Name = "ouid_bmQuestionInspection", Value = OuidBmQuestionInspection };
            yield return new InputPropertyInfo { Name = "ouid_bmInspectionListResult", Value = OuidBmInspectionListResult };
            yield return new InputPropertyInfo { Name = "LinkedControlCardId", Value = LinkedControlCardId };
            yield return new InputPropertyInfo { Name = "QuestionAnswersId", Value = QuestionAnswersId };
            yield return new InputPropertyInfo { Name = "ControlListId", Value = ControlListId };
            yield return new InputPropertyInfo { Name = "MandatoryReqId", Value = MandatoryReqId };
            yield return new InputPropertyInfo { Name = "NPAId", Value = NpaId };
        }
    }

    public partial class dicControlTypesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlTypeName { get; set; }
        public StringFilterOperator ControlLevelName { get; set; }
        public ICollection<dicControlTypesFilter> Or { get; set; }
        public ICollection<dicControlTypesFilter> And { get; set; }
        public dicControlTypesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlTypeName", Value = ControlTypeName };
            yield return new InputPropertyInfo { Name = "ControlLevelName", Value = ControlLevelName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docControlOrderCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlOrderCreateDate { get; set; }
        public object ControlOrderSignDate { get; set; }
        public string ControlOrderNumber { get; set; }
        public Guid? ControlCardId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrderCreateDate", Value = ControlOrderCreateDate };
            yield return new InputPropertyInfo { Name = "ControlOrderSignDate", Value = ControlOrderSignDate };
            yield return new InputPropertyInfo { Name = "ControlOrderNumber", Value = ControlOrderNumber };
            yield return new InputPropertyInfo { Name = "ControlCardId", Value = ControlCardId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class NPAFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator NpaName { get; set; }
        public DateFilterOperator ApproveDate { get; set; }
        public StringFilterOperator ApproveEntity { get; set; }
        public DateFilterOperator NpaEndDate { get; set; }
        public StringFilterOperator Number { get; set; }
        public StringFilterOperator Body { get; set; }
        public ICollection<NPAFilter> Or { get; set; }
        public ICollection<NPAFilter> And { get; set; }
        public NPAFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPAName", Value = NpaName };
            yield return new InputPropertyInfo { Name = "ApproveDate", Value = ApproveDate };
            yield return new InputPropertyInfo { Name = "ApproveEntity", Value = ApproveEntity };
            yield return new InputPropertyInfo { Name = "NPAEndDate", Value = NpaEndDate };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "Body", Value = Body };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docProcStatementCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object SendToProcDate { get; set; }
        public string ProcApproveFio { get; set; }
        public string ProcApproveRole { get; set; }
        public string ProcApprovePlace { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SendToProcDate", Value = SendToProcDate };
            yield return new InputPropertyInfo { Name = "ProcApproveFIO", Value = ProcApproveFio };
            yield return new InputPropertyInfo { Name = "ProcApproveRole", Value = ProcApproveRole };
            yield return new InputPropertyInfo { Name = "ProcApprovePlace", Value = ProcApprovePlace };
        }
    }

    public partial class dicPunishmentTypeUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string PunishmentType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PunishmentType", Value = PunishmentType };
        }
    }

    public partial class MaterialsCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MaterialName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MaterialName", Value = MaterialName };
        }
    }

    public partial class dicPunishmentTypeFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator PunishmentType { get; set; }
        public ICollection<dicPunishmentTypeFilter> Or { get; set; }
        public ICollection<dicPunishmentTypeFilter> And { get; set; }
        public dicPunishmentTypeFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PunishmentType", Value = PunishmentType };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicOKTMOCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CodeOktmo { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeOKTMO", Value = CodeOktmo };
        }
    }

    public partial class dicControlBaseFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlBaseName { get; set; }
        public ICollection<dicControlBaseFilter> Or { get; set; }
        public ICollection<dicControlBaseFilter> And { get; set; }
        public dicControlBaseFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlBaseName", Value = ControlBaseName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicRoleUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string RoleName { get; set; }
        public Guid? ControlOrganizationId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RoleName", Value = RoleName };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
        }
    }

    public partial class dicStatusKNMCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string StatusKnmName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusKNMName", Value = StatusKnmName };
        }
    }

    public partial class CheckOutEGRIPFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator LastRenewData { get; set; }
        public StringFilterOperator Ogrnip { get; set; }
        public StringFilterOperator Innip { get; set; }
        public DateFilterOperator OgrniPdate { get; set; }
        public DateFilterOperator GrnEgripDate { get; set; }
        public StringFilterOperator RegIpOld { get; set; }
        public StringFilterOperator OrgRegLocationIp { get; set; }
        public StringFilterOperator AddressOrgRegLocIp { get; set; }
        public StringFilterOperator RegNumberPfrIp { get; set; }
        public DateFilterOperator DateRegNumPfrIp { get; set; }
        public StringFilterOperator BranchPfrip { get; set; }
        public StringFilterOperator GrnRegPfrip { get; set; }
        public DateFilterOperator DateGrnRegPfrIp { get; set; }
        public DateFilterOperator DateUpdateEgrIp { get; set; }
        public ICollection<CheckOutEGRIPFilter> Or { get; set; }
        public ICollection<CheckOutEGRIPFilter> And { get; set; }
        public CheckOutEGRIPFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "OGRNIP", Value = Ogrnip };
            yield return new InputPropertyInfo { Name = "INNIP", Value = Innip };
            yield return new InputPropertyInfo { Name = "OGRNIPdate", Value = OgrniPdate };
            yield return new InputPropertyInfo { Name = "grnEgripDate", Value = GrnEgripDate };
            yield return new InputPropertyInfo { Name = "regIpOld", Value = RegIpOld };
            yield return new InputPropertyInfo { Name = "orgRegLocationIp", Value = OrgRegLocationIp };
            yield return new InputPropertyInfo { Name = "addressOrgRegLocIp", Value = AddressOrgRegLocIp };
            yield return new InputPropertyInfo { Name = "regNumberPfrIp", Value = RegNumberPfrIp };
            yield return new InputPropertyInfo { Name = "dateRegNumPfrIp", Value = DateRegNumPfrIp };
            yield return new InputPropertyInfo { Name = "branchPFRIP", Value = BranchPfrip };
            yield return new InputPropertyInfo { Name = "grnRegPFRIP", Value = GrnRegPfrip };
            yield return new InputPropertyInfo { Name = "dateGrnRegPfrIp", Value = DateGrnRegPfrIp };
            yield return new InputPropertyInfo { Name = "dateUpdateEgrIP", Value = DateUpdateEgrIp };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class CitizenRequestCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantSurname { get; set; }
        public string SecondName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string RequestContent { get; set; }
        public string ViolationCondition { get; set; }
        public object RequestGetDate { get; set; }
        public string TargetControlOrganization { get; set; }
        public string TargetControlOrganizationOgrn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ApplicantName", Value = ApplicantName };
            yield return new InputPropertyInfo { Name = "ApplicantSurname", Value = ApplicantSurname };
            yield return new InputPropertyInfo { Name = "SecondName", Value = SecondName };
            yield return new InputPropertyInfo { Name = "ApplicantEmail", Value = ApplicantEmail };
            yield return new InputPropertyInfo { Name = "ApplicantPhone", Value = ApplicantPhone };
            yield return new InputPropertyInfo { Name = "RequestContent", Value = RequestContent };
            yield return new InputPropertyInfo { Name = "ViolationCondition", Value = ViolationCondition };
            yield return new InputPropertyInfo { Name = "RequestGetDate", Value = RequestGetDate };
            yield return new InputPropertyInfo { Name = "TargetControlOrganization", Value = TargetControlOrganization };
            yield return new InputPropertyInfo { Name = "TargetControlOrganizationOGRN", Value = TargetControlOrganizationOgrn };
        }
    }

    public partial class HazardClassUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object HazardClassStartDate { get; set; }
        public object HazardClassEndDate { get; set; }
        public Guid? ControlObjectId { get; set; }
        public Guid? HazardClassId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassStartDate", Value = HazardClassStartDate };
            yield return new InputPropertyInfo { Name = "HazardClassEndDate", Value = HazardClassEndDate };
            yield return new InputPropertyInfo { Name = "ControlObjectId", Value = ControlObjectId };
            yield return new InputPropertyInfo { Name = "HazardClassId", Value = HazardClassId };
        }
    }

    public partial class dicOKOPFUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string SubjectName { get; set; }
        public string CodeOkopf { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectName", Value = SubjectName };
            yield return new InputPropertyInfo { Name = "CodeOKOPF", Value = CodeOkopf };
        }
    }

    public partial class DicControlPlanTypeCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlPlanType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlPlanType", Value = ControlPlanType };
        }
    }

    public partial class docControlPlanCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlPlanName { get; set; }
        public object ControlPlanApproveData { get; set; }
        public string FgiserpGlobalPlanGuid { get; set; }
        public string ControlPlanYear { get; set; }
        public Guid? ControlPlanStatusId { get; set; }
        public Guid? ControlPlanTypeId { get; set; }
        public ICollection<Guid> ControlPlanApprovers { get; set; }
        public Guid? ControlPlanAuthorId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlPlanName", Value = ControlPlanName };
            yield return new InputPropertyInfo { Name = "ControlPlanApproveData", Value = ControlPlanApproveData };
            yield return new InputPropertyInfo { Name = "FGISERPGlobalPlanGUID", Value = FgiserpGlobalPlanGuid };
            yield return new InputPropertyInfo { Name = "ControlPlanYear", Value = ControlPlanYear };
            yield return new InputPropertyInfo { Name = "ControlPlanStatusId", Value = ControlPlanStatusId };
            yield return new InputPropertyInfo { Name = "ControlPlanTypeId", Value = ControlPlanTypeId };
            yield return new InputPropertyInfo { Name = "ControlPlanApprovers", Value = ControlPlanApprovers };
            yield return new InputPropertyInfo { Name = "ControlPlanAuthorId", Value = ControlPlanAuthorId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class dicOKSMCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string FullCountryName { get; set; }
        public string ShortCountryName { get; set; }
        public string LetterCodeAlpha2 { get; set; }
        public string LetterCodeAlpha3 { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Code", Value = Code };
            yield return new InputPropertyInfo { Name = "FullCountryName", Value = FullCountryName };
            yield return new InputPropertyInfo { Name = "ShortCountryName", Value = ShortCountryName };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha2", Value = LetterCodeAlpha2 };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha3", Value = LetterCodeAlpha3 };
        }
    }

    public partial class HazardClassFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator HazardClassLevel { get; set; }
        public DateFilterOperator HazardClassStartDate { get; set; }
        public DateFilterOperator HazardClassEndDate { get; set; }
        public ICollection<HazardClassFilter> Or { get; set; }
        public ICollection<HazardClassFilter> And { get; set; }
        public HazardClassFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassLevel", Value = HazardClassLevel };
            yield return new InputPropertyInfo { Name = "HazardClassStartDate", Value = HazardClassStartDate };
            yield return new InputPropertyInfo { Name = "HazardClassEndDate", Value = HazardClassEndDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class RandEParameterValueFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ParameterName { get; set; }
        public StringFilterOperator ParameterValue { get; set; }
        public ICollection<RandEParameterValueFilter> Or { get; set; }
        public ICollection<RandEParameterValueFilter> And { get; set; }
        public RandEParameterValueFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ParameterName", Value = ParameterName };
            yield return new InputPropertyInfo { Name = "ParameterValue", Value = ParameterValue };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlJournalsCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string JournalCreationDate { get; set; }
        public string JournalName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalCreationDate", Value = JournalCreationDate };
            yield return new InputPropertyInfo { Name = "JournalName", Value = JournalName };
        }
    }

    public partial class dicKNMTypesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator KnmTypeName { get; set; }
        public ICollection<dicKNMTypesFilter> Or { get; set; }
        public ICollection<dicKNMTypesFilter> And { get; set; }
        public dicKNMTypesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMTypeName", Value = KnmTypeName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlItemPassportFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator CreateDate { get; set; }
        public BooleanFilterOperator IsInPlanYear { get; set; }
        public ICollection<ControlItemPassportFilter> Or { get; set; }
        public ICollection<ControlItemPassportFilter> And { get; set; }
        public ControlItemPassportFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CreateDate", Value = CreateDate };
            yield return new InputPropertyInfo { Name = "IsInPlanYear", Value = IsInPlanYear };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class FloatFilterOperator : IGraphQlInputObject
    {
        public decimal? EqualTo { get; set; }
        public decimal? NotEqualTo { get; set; }
        public decimal? GreaterThan { get; set; }
        public decimal? GreaterThanOrEqual { get; set; }
        public decimal? LessThan { get; set; }
        public decimal? LessThanOrEqual { get; set; }
        public ICollection<decimal> In { get; set; }
        public ICollection<decimal> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class docRegulationUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object RegulationCreateDate { get; set; }
        public object RegulationExecutionDate { get; set; }
        public string Result { get; set; }
        public Guid? ControlCardId { get; set; }
        public Guid? ProcStatmentStatusId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RegulationCreateDate", Value = RegulationCreateDate };
            yield return new InputPropertyInfo { Name = "RegulationExecutionDate", Value = RegulationExecutionDate };
            yield return new InputPropertyInfo { Name = "Result", Value = Result };
            yield return new InputPropertyInfo { Name = "ControlCardId", Value = ControlCardId };
            yield return new InputPropertyInfo { Name = "ProcStatmentStatusId", Value = ProcStatmentStatusId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class dicOKSMUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string FullCountryName { get; set; }
        public string ShortCountryName { get; set; }
        public string LetterCodeAlpha2 { get; set; }
        public string LetterCodeAlpha3 { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Code", Value = Code };
            yield return new InputPropertyInfo { Name = "FullCountryName", Value = FullCountryName };
            yield return new InputPropertyInfo { Name = "ShortCountryName", Value = ShortCountryName };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha2", Value = LetterCodeAlpha2 };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha3", Value = LetterCodeAlpha3 };
        }
    }

    public partial class NPACreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaName { get; set; }
        public object ApproveDate { get; set; }
        public object NpaEndDate { get; set; }
        public string Number { get; set; }
        public string Body { get; set; }
        public ICollection<Guid> MandatoryReqs { get; set; }
        public Guid? NpaTypeId { get; set; }
        public Guid? NpaLevelId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPAName", Value = NpaName };
            yield return new InputPropertyInfo { Name = "ApproveDate", Value = ApproveDate };
            yield return new InputPropertyInfo { Name = "NPAEndDate", Value = NpaEndDate };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "Body", Value = Body };
            yield return new InputPropertyInfo { Name = "MandatoryReqs", Value = MandatoryReqs };
            yield return new InputPropertyInfo { Name = "NPATypeId", Value = NpaTypeId };
            yield return new InputPropertyInfo { Name = "NPALevelId", Value = NpaLevelId };
        }
    }

    public partial class dicOKTMOCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CodeOktmo { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeOKTMO", Value = CodeOktmo };
        }
    }

    public partial class DateFilterOperator : IGraphQlInputObject
    {
        public object EqualTo { get; set; }
        public object NotEqualTo { get; set; }
        public object GreaterThan { get; set; }
        public object GreaterThanOrEqual { get; set; }
        public object LessThan { get; set; }
        public object LessThanOrEqual { get; set; }
        public ICollection<object> In { get; set; }
        public ICollection<object> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class dicHazardClassCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string HazardClassName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassName", Value = HazardClassName };
        }
    }

    public partial class MaterialsFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator MaterialName { get; set; }
        public ICollection<MaterialsFilter> Or { get; set; }
        public ICollection<MaterialsFilter> And { get; set; }
        public MaterialsFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MaterialName", Value = MaterialName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class MandatoryReqsCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MandratoryReqContent { get; set; }
        public object StartDateMandatory { get; set; }
        public object EndDateMandatory { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MandratoryReqContent", Value = MandratoryReqContent };
            yield return new InputPropertyInfo { Name = "StartDateMandatory", Value = StartDateMandatory };
            yield return new InputPropertyInfo { Name = "EndDateMandatory", Value = EndDateMandatory };
        }
    }

    public partial class dicControlBaseCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlBaseName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlBaseName", Value = ControlBaseName };
        }
    }

    public partial class dicNPATypesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPATypeName", Value = NpaTypeName };
        }
    }

    public partial class DicQuestionAnswersUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string QuestionAnswer { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionAnswer", Value = QuestionAnswer };
        }
    }

    public partial class docControlActCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlActCreateDate { get; set; }
        public Guid? ActLinkedtoControlCardId { get; set; }
        public Guid? MaterialsId { get; set; }
        public Guid? ControlActCreatePlaceId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlActCreateDate", Value = ControlActCreateDate };
            yield return new InputPropertyInfo { Name = "ActLinkedtoControlCardId", Value = ActLinkedtoControlCardId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
            yield return new InputPropertyInfo { Name = "ControlActCreatePlaceId", Value = ControlActCreatePlaceId };
        }
    }

    public partial class ViolationFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ViolationSpecificComment { get; set; }
        public ICollection<ViolationFilter> Or { get; set; }
        public ICollection<ViolationFilter> And { get; set; }
        public ViolationFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ViolationSpecificComment", Value = ViolationSpecificComment };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicLicencedActivityTypesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicensedActivityCode { get; set; }
        public string LicensedActivityTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicensedActivityCode", Value = LicensedActivityCode };
            yield return new InputPropertyInfo { Name = "LicensedActivityTypeName", Value = LicensedActivityTypeName };
        }
    }

    public partial class ControlItemResultCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public decimal? SumAdmFine { get; set; }
        public bool? SumAdmFineStatus { get; set; }
        public ICollection<Guid> DamageType { get; set; }
        public Guid? LinkedControlCardId { get; set; }
        public Guid? PunishmentTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "sumAdmFine", Value = SumAdmFine };
            yield return new InputPropertyInfo { Name = "SumAdmFineStatus", Value = SumAdmFineStatus };
            yield return new InputPropertyInfo { Name = "DamageType", Value = DamageType };
            yield return new InputPropertyInfo { Name = "LinkedControlCardId", Value = LinkedControlCardId };
            yield return new InputPropertyInfo { Name = "PunishmentTypeId", Value = PunishmentTypeId };
        }
    }

    public partial class dicStatusPlanCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string StatusPlanName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusPlanName", Value = StatusPlanName };
        }
    }

    public partial class AddressCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CodeKladr { get; set; }
        public string CodeFias { get; set; }
        public string Address { get; set; }
        public string PostIndex { get; set; }
        public string AddressFact { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeKLADR", Value = CodeKladr };
            yield return new InputPropertyInfo { Name = "CodeFIAS", Value = CodeFias };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "PostIndex", Value = PostIndex };
            yield return new InputPropertyInfo { Name = "AddressFact", Value = AddressFact };
        }
    }

    public partial class docControlPlanUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlPlanName { get; set; }
        public object ControlPlanApproveData { get; set; }
        public string FgiserpGlobalPlanGuid { get; set; }
        public string ControlPlanYear { get; set; }
        public Guid? ControlPlanStatusId { get; set; }
        public Guid? ControlPlanTypeId { get; set; }
        public ICollection<Guid> ControlPlanApprovers { get; set; }
        public Guid? ControlPlanAuthorId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlPlanName", Value = ControlPlanName };
            yield return new InputPropertyInfo { Name = "ControlPlanApproveData", Value = ControlPlanApproveData };
            yield return new InputPropertyInfo { Name = "FGISERPGlobalPlanGUID", Value = FgiserpGlobalPlanGuid };
            yield return new InputPropertyInfo { Name = "ControlPlanYear", Value = ControlPlanYear };
            yield return new InputPropertyInfo { Name = "ControlPlanStatusId", Value = ControlPlanStatusId };
            yield return new InputPropertyInfo { Name = "ControlPlanTypeId", Value = ControlPlanTypeId };
            yield return new InputPropertyInfo { Name = "ControlPlanApprovers", Value = ControlPlanApprovers };
            yield return new InputPropertyInfo { Name = "ControlPlanAuthorId", Value = ControlPlanAuthorId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class dicProsecUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProsecName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProsecName", Value = ProsecName };
        }
    }

    public partial class ExtendedAttribCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ExtAttributeName { get; set; }
        public string ExtAttributeContentUnit { get; set; }
        public string ExtAttributeTitle { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeName", Value = ExtAttributeName };
            yield return new InputPropertyInfo { Name = "ExtAttributeContentUnit", Value = ExtAttributeContentUnit };
            yield return new InputPropertyInfo { Name = "ExtAttributeTitle", Value = ExtAttributeTitle };
        }
    }

    public partial class docControlListCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlListStartDate { get; set; }
        public object ControlListEndDate { get; set; }
        public Guid? ControlItemResultId { get; set; }
        public Guid? MaterialsId { get; set; }
        public Guid? ControlTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStartDate", Value = ControlListStartDate };
            yield return new InputPropertyInfo { Name = "ControlListEndDate", Value = ControlListEndDate };
            yield return new InputPropertyInfo { Name = "ControlItemResultId", Value = ControlItemResultId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
        }
    }

    public partial class ViolationUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ViolationSpecificComment { get; set; }
        public Guid? ControlItemResultId { get; set; }
        public Guid? ViolationTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ViolationSpecificComment", Value = ViolationSpecificComment };
            yield return new InputPropertyInfo { Name = "ControlItemResultId", Value = ControlItemResultId };
            yield return new InputPropertyInfo { Name = "ViolationTypeId", Value = ViolationTypeId };
        }
    }

    public partial class dicOKOPFFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator SubjectName { get; set; }
        public StringFilterOperator CodeOkopf { get; set; }
        public ICollection<dicOKOPFFilter> Or { get; set; }
        public ICollection<dicOKOPFFilter> And { get; set; }
        public dicOKOPFFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectName", Value = SubjectName };
            yield return new InputPropertyInfo { Name = "CodeOKOPF", Value = CodeOkopf };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicOKVEDCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ActivityType { get; set; }
        public string Decipher { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityType", Value = ActivityType };
            yield return new InputPropertyInfo { Name = "Decipher", Value = Decipher };
        }
    }

    public partial class ExtendedAttribFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ExtAttributeName { get; set; }
        public StringFilterOperator ExtAttributeContentUnit { get; set; }
        public StringFilterOperator ExtAttributeTitle { get; set; }
        public ICollection<ExtendedAttribFilter> Or { get; set; }
        public ICollection<ExtendedAttribFilter> And { get; set; }
        public ExtendedAttribFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeName", Value = ExtAttributeName };
            yield return new InputPropertyInfo { Name = "ExtAttributeContentUnit", Value = ExtAttributeContentUnit };
            yield return new InputPropertyInfo { Name = "ExtAttributeTitle", Value = ExtAttributeTitle };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicControlPlanTypeCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlPlanType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlPlanType", Value = ControlPlanType };
        }
    }

    public partial class PersonAppointmentUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object AppStartDate { get; set; }
        public object AppEndDate { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? RoleId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "AppStartDate", Value = AppStartDate };
            yield return new InputPropertyInfo { Name = "AppEndDate", Value = AppEndDate };
            yield return new InputPropertyInfo { Name = "PersonId", Value = PersonId };
            yield return new InputPropertyInfo { Name = "RoleId", Value = RoleId };
        }
    }

    public partial class SubjectFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator MainName { get; set; }
        public StringFilterOperator Address { get; set; }
        public StringFilterOperator Citizenship { get; set; }
        public StringFilterOperator RepresentativeSurname { get; set; }
        public StringFilterOperator RepresentativeName { get; set; }
        public StringFilterOperator RepresentativeSecondName { get; set; }
        public StringFilterOperator NameOrg { get; set; }
        public ICollection<SubjectFilter> Or { get; set; }
        public ICollection<SubjectFilter> And { get; set; }
        public SubjectFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "mainName", Value = MainName };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "Citizenship", Value = Citizenship };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "NameOrg", Value = NameOrg };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class RiskCatCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CategoryClassLevel { get; set; }
        public object RiskCatStartDate { get; set; }
        public object RiskCatEndDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CategoryClassLevel", Value = CategoryClassLevel };
            yield return new InputPropertyInfo { Name = "RiskCatStartDate", Value = RiskCatStartDate };
            yield return new InputPropertyInfo { Name = "RiskCatEndDate", Value = RiskCatEndDate };
        }
    }

    public partial class ControlItemUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlItemName { get; set; }
        public object ControlDate { get; set; }
        public string ControlItemResult { get; set; }
        public ICollection<Guid> ControlItemPersons { get; set; }
        public Guid? ControlProgramId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemName", Value = ControlItemName };
            yield return new InputPropertyInfo { Name = "ControlDate", Value = ControlDate };
            yield return new InputPropertyInfo { Name = "ControlItemResult", Value = ControlItemResult };
            yield return new InputPropertyInfo { Name = "ControlItemPersons", Value = ControlItemPersons };
            yield return new InputPropertyInfo { Name = "ControlProgramId", Value = ControlProgramId };
        }
    }

    public partial class ControlJournalsUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string JournalCreationDate { get; set; }
        public string JournalName { get; set; }
        public Guid? ControlFileId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalCreationDate", Value = JournalCreationDate };
            yield return new InputPropertyInfo { Name = "JournalName", Value = JournalName };
            yield return new InputPropertyInfo { Name = "ControlFileId", Value = ControlFileId };
        }
    }

    public partial class AddressFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator CodeKladr { get; set; }
        public StringFilterOperator CodeFias { get; set; }
        public StringFilterOperator Address { get; set; }
        public StringFilterOperator PostIndex { get; set; }
        public StringFilterOperator AddressFact { get; set; }
        public ICollection<AddressFilter> Or { get; set; }
        public ICollection<AddressFilter> And { get; set; }
        public AddressFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeKLADR", Value = CodeKladr };
            yield return new InputPropertyInfo { Name = "CodeFIAS", Value = CodeFias };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "PostIndex", Value = PostIndex };
            yield return new InputPropertyInfo { Name = "AddressFact", Value = AddressFact };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlCardFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator ProcDataAgreement { get; set; }
        public DateFilterOperator ControlStartDate { get; set; }
        public StringFilterOperator ControlStatus { get; set; }
        public FloatFilterOperator ControlDuration { get; set; }
        public FloatFilterOperator DurationProlong { get; set; }
        public DateFilterOperator ControlEndDate { get; set; }
        public StringFilterOperator ControlPurpose { get; set; }
        public BooleanFilterOperator IsJoint { get; set; }
        public StringFilterOperator NumberFgiserp { get; set; }
        public DateFilterOperator FgiserpRegData { get; set; }
        public DateFilterOperator LastEndControlDate { get; set; }
        public StringFilterOperator CheckControlRestrict { get; set; }
        public StringFilterOperator ControlCancelInfo { get; set; }
        public StringFilterOperator InternalNumberFgiserp { get; set; }
        public DateFilterOperator ControlFactStartDate { get; set; }
        public DateFilterOperator ControlFactEndDate { get; set; }
        public FloatFilterOperator FactControlPeriod { get; set; }
        public StringFilterOperator FactControlPeriodUnit { get; set; }
        public StringFilterOperator JointControlOrganization { get; set; }
        public ICollection<ControlCardFilter> Or { get; set; }
        public ICollection<ControlCardFilter> And { get; set; }
        public ControlCardFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcDataAgreement", Value = ProcDataAgreement };
            yield return new InputPropertyInfo { Name = "ControlStartDate", Value = ControlStartDate };
            yield return new InputPropertyInfo { Name = "ControlStatus", Value = ControlStatus };
            yield return new InputPropertyInfo { Name = "ControlDuration", Value = ControlDuration };
            yield return new InputPropertyInfo { Name = "DurationProlong", Value = DurationProlong };
            yield return new InputPropertyInfo { Name = "ControlEndDate", Value = ControlEndDate };
            yield return new InputPropertyInfo { Name = "ControlPurpose", Value = ControlPurpose };
            yield return new InputPropertyInfo { Name = "IsJoint", Value = IsJoint };
            yield return new InputPropertyInfo { Name = "NumberFGISERP", Value = NumberFgiserp };
            yield return new InputPropertyInfo { Name = "FGISERPRegData", Value = FgiserpRegData };
            yield return new InputPropertyInfo { Name = "LastEndControlDate", Value = LastEndControlDate };
            yield return new InputPropertyInfo { Name = "CheckControlRestrict", Value = CheckControlRestrict };
            yield return new InputPropertyInfo { Name = "ControlCancelInfo", Value = ControlCancelInfo };
            yield return new InputPropertyInfo { Name = "InternalNumberFGISERP", Value = InternalNumberFgiserp };
            yield return new InputPropertyInfo { Name = "ControlFactStartDate", Value = ControlFactStartDate };
            yield return new InputPropertyInfo { Name = "ControlFactEndDate", Value = ControlFactEndDate };
            yield return new InputPropertyInfo { Name = "FactControlPeriod", Value = FactControlPeriod };
            yield return new InputPropertyInfo { Name = "FactControlPeriodUnit", Value = FactControlPeriodUnit };
            yield return new InputPropertyInfo { Name = "JointControlOrganization", Value = JointControlOrganization };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicOKVEDFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ActivityType { get; set; }
        public StringFilterOperator Decipher { get; set; }
        public ICollection<dicOKVEDFilter> Or { get; set; }
        public ICollection<dicOKVEDFilter> And { get; set; }
        public dicOKVEDFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityType", Value = ActivityType };
            yield return new InputPropertyInfo { Name = "Decipher", Value = Decipher };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicStatusPlanUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string StatusPlanName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusPlanName", Value = StatusPlanName };
        }
    }

    public partial class dicStatusKNMFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator StatusKnmName { get; set; }
        public ICollection<dicStatusKNMFilter> Or { get; set; }
        public ICollection<dicStatusKNMFilter> And { get; set; }
        public dicStatusKNMFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusKNMName", Value = StatusKnmName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicControlTypesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlTypeName { get; set; }
        public string ControlLevelName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlTypeName", Value = ControlTypeName };
            yield return new InputPropertyInfo { Name = "ControlLevelName", Value = ControlLevelName };
        }
    }

    public partial class LicenceFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator LicenceName { get; set; }
        public StringFilterOperator Series { get; set; }
        public StringFilterOperator Number { get; set; }
        public DateFilterOperator StartDate { get; set; }
        public DateFilterOperator StopDate { get; set; }
        public DateFilterOperator EndDate { get; set; }
        public StringFilterOperator BlankNumber { get; set; }
        public StringFilterOperator LicenseFinalDecision { get; set; }
        public DateFilterOperator LicenseFinalStartDate { get; set; }
        public DateFilterOperator LicenseFinalEndDate { get; set; }
        public ICollection<LicenceFilter> Or { get; set; }
        public ICollection<LicenceFilter> And { get; set; }
        public LicenceFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceName", Value = LicenceName };
            yield return new InputPropertyInfo { Name = "Series", Value = Series };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "startDate", Value = StartDate };
            yield return new InputPropertyInfo { Name = "stopDate", Value = StopDate };
            yield return new InputPropertyInfo { Name = "endDate", Value = EndDate };
            yield return new InputPropertyInfo { Name = "BlankNumber", Value = BlankNumber };
            yield return new InputPropertyInfo { Name = "licenseFinalDecision", Value = LicenseFinalDecision };
            yield return new InputPropertyInfo { Name = "licenseFinalStartDate", Value = LicenseFinalStartDate };
            yield return new InputPropertyInfo { Name = "licenseFinalEndDate", Value = LicenseFinalEndDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicDamageTypeCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string DamageType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "DamageType", Value = DamageType };
        }
    }

    public partial class ControlFileUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlFileNumber { get; set; }
        public object ControlFileStartDate { get; set; }
        public Guid? ControlProgramId { get; set; }
        public Guid? ControlFileStatusId { get; set; }
        public Guid? ControObjectId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileNumber", Value = ControlFileNumber };
            yield return new InputPropertyInfo { Name = "ControlFileStartDate", Value = ControlFileStartDate };
            yield return new InputPropertyInfo { Name = "ControlProgramId", Value = ControlProgramId };
            yield return new InputPropertyInfo { Name = "ControlFileStatusId", Value = ControlFileStatusId };
            yield return new InputPropertyInfo { Name = "ControObjectId", Value = ControObjectId };
        }
    }

    public partial class dicProcStatmentStatusUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProcStatementStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcStatementStatusName", Value = ProcStatementStatusName };
        }
    }

    public partial class CheckOutEGRIPCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public string Ogrnip { get; set; }
        public string Innip { get; set; }
        public object OgrniPdate { get; set; }
        public object GrnEgripDate { get; set; }
        public string RegIpOld { get; set; }
        public string OrgRegLocationIp { get; set; }
        public string AddressOrgRegLocIp { get; set; }
        public string RegNumberPfrIp { get; set; }
        public object DateRegNumPfrIp { get; set; }
        public string BranchPfrip { get; set; }
        public string GrnRegPfrip { get; set; }
        public object DateGrnRegPfrIp { get; set; }
        public object DateUpdateEgrIp { get; set; }
        public Guid? SubjectEgripId { get; set; }
        public Guid? AddressId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "OGRNIP", Value = Ogrnip };
            yield return new InputPropertyInfo { Name = "INNIP", Value = Innip };
            yield return new InputPropertyInfo { Name = "OGRNIPdate", Value = OgrniPdate };
            yield return new InputPropertyInfo { Name = "grnEgripDate", Value = GrnEgripDate };
            yield return new InputPropertyInfo { Name = "regIpOld", Value = RegIpOld };
            yield return new InputPropertyInfo { Name = "orgRegLocationIp", Value = OrgRegLocationIp };
            yield return new InputPropertyInfo { Name = "addressOrgRegLocIp", Value = AddressOrgRegLocIp };
            yield return new InputPropertyInfo { Name = "regNumberPfrIp", Value = RegNumberPfrIp };
            yield return new InputPropertyInfo { Name = "dateRegNumPfrIp", Value = DateRegNumPfrIp };
            yield return new InputPropertyInfo { Name = "branchPFRIP", Value = BranchPfrip };
            yield return new InputPropertyInfo { Name = "grnRegPFRIP", Value = GrnRegPfrip };
            yield return new InputPropertyInfo { Name = "dateGrnRegPfrIp", Value = DateGrnRegPfrIp };
            yield return new InputPropertyInfo { Name = "dateUpdateEgrIP", Value = DateUpdateEgrIp };
            yield return new InputPropertyInfo { Name = "SubjectEGRIPId", Value = SubjectEgripId };
            yield return new InputPropertyInfo { Name = "AddressId", Value = AddressId };
        }
    }

    public partial class dicViolationTypesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Violation { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Violation", Value = Violation };
        }
    }

    public partial class dicRiskCategoryCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string RiskCategoryName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RiskCategoryName", Value = RiskCategoryName };
        }
    }

    public partial class ControlOrganizationCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlOrganizationName { get; set; }
        public string OgrnKno { get; set; }
        public ICollection<Guid> ControlType { get; set; }
        public Guid? HeadOrganizationId { get; set; }
        public ICollection<Guid> RandEParameters { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrganizationName", Value = ControlOrganizationName };
            yield return new InputPropertyInfo { Name = "OGRN_KNO", Value = OgrnKno };
            yield return new InputPropertyInfo { Name = "ControlType", Value = ControlType };
            yield return new InputPropertyInfo { Name = "HeadOrganizationId", Value = HeadOrganizationId };
            yield return new InputPropertyInfo { Name = "RandEParameters", Value = RandEParameters };
        }
    }

    public partial class MandatoryReqsUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MandratoryReqContent { get; set; }
        public object StartDateMandatory { get; set; }
        public object EndDateMandatory { get; set; }
        public ICollection<Guid> Npa { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MandratoryReqContent", Value = MandratoryReqContent };
            yield return new InputPropertyInfo { Name = "StartDateMandatory", Value = StartDateMandatory };
            yield return new InputPropertyInfo { Name = "EndDateMandatory", Value = EndDateMandatory };
            yield return new InputPropertyInfo { Name = "NPA", Value = Npa };
        }
    }

    public partial class ViolationCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ViolationSpecificComment { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ViolationSpecificComment", Value = ViolationSpecificComment };
        }
    }

    public partial class ControlItemCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlItemName { get; set; }
        public object ControlDate { get; set; }
        public string ControlItemResult { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemName", Value = ControlItemName };
            yield return new InputPropertyInfo { Name = "ControlDate", Value = ControlDate };
            yield return new InputPropertyInfo { Name = "ControlItemResult", Value = ControlItemResult };
        }
    }

    public partial class RandEParameterValueCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ParameterName", Value = ParameterName };
            yield return new InputPropertyInfo { Name = "ParameterValue", Value = ParameterValue };
        }
    }

    public partial class DicStatesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string State { get; set; }
        public string Step { get; set; }
        public string Infotxt { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "state", Value = State };
            yield return new InputPropertyInfo { Name = "step", Value = Step };
            yield return new InputPropertyInfo { Name = "infotxt", Value = Infotxt };
        }
    }

    public partial class CheckOutEGRULUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public object DateUpdateEgrul { get; set; }
        public string Ogrnul { get; set; }
        public object OgrnuLdata { get; set; }
        public string Innul { get; set; }
        public object DateRegYl { get; set; }
        public string OrgNameRegYl { get; set; }
        public string AddressRegOrgYl { get; set; }
        public string GrnOrgLocationYl { get; set; }
        public object GrnDateOrgLocationYl { get; set; }
        public object DateTaxReg { get; set; }
        public string TaxOrg { get; set; }
        public string GrnTaxOrg { get; set; }
        public object DateGrnTaxOrg { get; set; }
        public string RegNumberPfryl { get; set; }
        public object DateRegNumPfryl { get; set; }
        public string BranchPfryl { get; set; }
        public string GrnRegPfryl { get; set; }
        public object DateGrnRegPfryl { get; set; }
        public string GrnRegExecPfryl { get; set; }
        public string RegNumberExecPfryl { get; set; }
        public object DateRegNumExecPfryl { get; set; }
        public string BranchExecSsyl { get; set; }
        public string GrnRegExecSsyl { get; set; }
        public object DateGrnExecSsyl { get; set; }
        public string TypeCharterCapital { get; set; }
        public string SumCharterCapital { get; set; }
        public string GrnCharterCapital { get; set; }
        public object DateGrnCharterCapital { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string RepresentativeSurname { get; set; }
        public string Kpp { get; set; }
        public string ShortNameOrg { get; set; }
        public Guid? SubjectEgrulId { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? CodeOkopfId { get; set; }
        public Guid? CitizenshipsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "dateUpdateEGRUL", Value = DateUpdateEgrul };
            yield return new InputPropertyInfo { Name = "OGRNUL", Value = Ogrnul };
            yield return new InputPropertyInfo { Name = "OGRNULdata", Value = OgrnuLdata };
            yield return new InputPropertyInfo { Name = "INNUL", Value = Innul };
            yield return new InputPropertyInfo { Name = "dateRegYL", Value = DateRegYl };
            yield return new InputPropertyInfo { Name = "orgNameRegYL", Value = OrgNameRegYl };
            yield return new InputPropertyInfo { Name = "addressRegOrgYL", Value = AddressRegOrgYl };
            yield return new InputPropertyInfo { Name = "grnOrgLocationYL", Value = GrnOrgLocationYl };
            yield return new InputPropertyInfo { Name = "grnDateOrgLocationYL", Value = GrnDateOrgLocationYl };
            yield return new InputPropertyInfo { Name = "dateTaxReg", Value = DateTaxReg };
            yield return new InputPropertyInfo { Name = "taxOrg", Value = TaxOrg };
            yield return new InputPropertyInfo { Name = "grnTaxOrg", Value = GrnTaxOrg };
            yield return new InputPropertyInfo { Name = "dateGRNTaxOrg", Value = DateGrnTaxOrg };
            yield return new InputPropertyInfo { Name = "RegNumberPFRYL", Value = RegNumberPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumPFRYL", Value = DateRegNumPfryl };
            yield return new InputPropertyInfo { Name = "branchPFRYL", Value = BranchPfryl };
            yield return new InputPropertyInfo { Name = "GrnRegPFRYL", Value = GrnRegPfryl };
            yield return new InputPropertyInfo { Name = "dateGrnRegPFRYL", Value = DateGrnRegPfryl };
            yield return new InputPropertyInfo { Name = "grnRegExecPFRYL", Value = GrnRegExecPfryl };
            yield return new InputPropertyInfo { Name = "regNumberExecPFRYL", Value = RegNumberExecPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumExecPFRYL", Value = DateRegNumExecPfryl };
            yield return new InputPropertyInfo { Name = "branchExecSSYL", Value = BranchExecSsyl };
            yield return new InputPropertyInfo { Name = "grnRegExecSSYL", Value = GrnRegExecSsyl };
            yield return new InputPropertyInfo { Name = "dateGrnExecSSYL", Value = DateGrnExecSsyl };
            yield return new InputPropertyInfo { Name = "typeCharterCapital", Value = TypeCharterCapital };
            yield return new InputPropertyInfo { Name = "sumCharterCapital", Value = SumCharterCapital };
            yield return new InputPropertyInfo { Name = "grnCharterCapital", Value = GrnCharterCapital };
            yield return new InputPropertyInfo { Name = "dateGRNCharterCapital", Value = DateGrnCharterCapital };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "KPP", Value = Kpp };
            yield return new InputPropertyInfo { Name = "ShortNameOrg", Value = ShortNameOrg };
            yield return new InputPropertyInfo { Name = "SubjectEGRULId", Value = SubjectEgrulId };
            yield return new InputPropertyInfo { Name = "AddressId", Value = AddressId };
            yield return new InputPropertyInfo { Name = "CodeOKOPFId", Value = CodeOkopfId };
            yield return new InputPropertyInfo { Name = "CitizenshipsId", Value = CitizenshipsId };
        }
    }

    public partial class dicKNMFormFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator KnmFormName { get; set; }
        public ICollection<dicKNMFormFilter> Or { get; set; }
        public ICollection<dicKNMFormFilter> And { get; set; }
        public dicKNMFormFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMFormName", Value = KnmFormName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicDamageTypeCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string DamageType { get; set; }
        public ICollection<Guid> ControlItemResults { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "DamageType", Value = DamageType };
            yield return new InputPropertyInfo { Name = "ControlItemResults", Value = ControlItemResults };
        }
    }

    public partial class docControlOrderUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlOrderCreateDate { get; set; }
        public object ControlOrderSignDate { get; set; }
        public string ControlOrderNumber { get; set; }
        public Guid? ControlCardId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrderCreateDate", Value = ControlOrderCreateDate };
            yield return new InputPropertyInfo { Name = "ControlOrderSignDate", Value = ControlOrderSignDate };
            yield return new InputPropertyInfo { Name = "ControlOrderNumber", Value = ControlOrderNumber };
            yield return new InputPropertyInfo { Name = "ControlCardId", Value = ControlCardId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class DicMeasureUnitsTypeUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MeasureUnitTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeName", Value = MeasureUnitTypeName };
        }
    }

    public partial class CheckOutEGRSMSPCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object DateRmsp { get; set; }
        public string CaseNumber { get; set; }
        public bool? IsInRmsp { get; set; }
        public object LastRenewData { get; set; }
        public Guid? SubjectEgrsmspId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "dateRMSP", Value = DateRmsp };
            yield return new InputPropertyInfo { Name = "caseNumber", Value = CaseNumber };
            yield return new InputPropertyInfo { Name = "IsInRMSP", Value = IsInRmsp };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "SubjectEGRSMSPId", Value = SubjectEgrsmspId };
        }
    }

    public partial class CitizenRequestCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantSurname { get; set; }
        public string SecondName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string RequestContent { get; set; }
        public string ViolationCondition { get; set; }
        public object RequestGetDate { get; set; }
        public string TargetControlOrganization { get; set; }
        public string TargetControlOrganizationOgrn { get; set; }
        public Guid? ApplicantAddressId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ApplicantName", Value = ApplicantName };
            yield return new InputPropertyInfo { Name = "ApplicantSurname", Value = ApplicantSurname };
            yield return new InputPropertyInfo { Name = "SecondName", Value = SecondName };
            yield return new InputPropertyInfo { Name = "ApplicantEmail", Value = ApplicantEmail };
            yield return new InputPropertyInfo { Name = "ApplicantPhone", Value = ApplicantPhone };
            yield return new InputPropertyInfo { Name = "RequestContent", Value = RequestContent };
            yield return new InputPropertyInfo { Name = "ViolationCondition", Value = ViolationCondition };
            yield return new InputPropertyInfo { Name = "RequestGetDate", Value = RequestGetDate };
            yield return new InputPropertyInfo { Name = "TargetControlOrganization", Value = TargetControlOrganization };
            yield return new InputPropertyInfo { Name = "TargetControlOrganizationOGRN", Value = TargetControlOrganizationOgrn };
            yield return new InputPropertyInfo { Name = "ApplicantAddressId", Value = ApplicantAddressId };
        }
    }

    public partial class dicControlListStatusUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlListStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStatusName", Value = ControlListStatusName };
        }
    }

    public partial class dicStatusKNMCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string StatusKnmName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusKNMName", Value = StatusKnmName };
        }
    }

    public partial class DicQuestionAnswersFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator QuestionAnswer { get; set; }
        public ICollection<DicQuestionAnswersFilter> Or { get; set; }
        public ICollection<DicQuestionAnswersFilter> And { get; set; }
        public DicQuestionAnswersFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionAnswer", Value = QuestionAnswer };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicSubjectTypeCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string SubjectTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectTypeName", Value = SubjectTypeName };
        }
    }

    public partial class CheckOutEGRIPCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public string Ogrnip { get; set; }
        public string Innip { get; set; }
        public object OgrniPdate { get; set; }
        public object GrnEgripDate { get; set; }
        public string RegIpOld { get; set; }
        public string OrgRegLocationIp { get; set; }
        public string AddressOrgRegLocIp { get; set; }
        public string RegNumberPfrIp { get; set; }
        public object DateRegNumPfrIp { get; set; }
        public string BranchPfrip { get; set; }
        public string GrnRegPfrip { get; set; }
        public object DateGrnRegPfrIp { get; set; }
        public object DateUpdateEgrIp { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "OGRNIP", Value = Ogrnip };
            yield return new InputPropertyInfo { Name = "INNIP", Value = Innip };
            yield return new InputPropertyInfo { Name = "OGRNIPdate", Value = OgrniPdate };
            yield return new InputPropertyInfo { Name = "grnEgripDate", Value = GrnEgripDate };
            yield return new InputPropertyInfo { Name = "regIpOld", Value = RegIpOld };
            yield return new InputPropertyInfo { Name = "orgRegLocationIp", Value = OrgRegLocationIp };
            yield return new InputPropertyInfo { Name = "addressOrgRegLocIp", Value = AddressOrgRegLocIp };
            yield return new InputPropertyInfo { Name = "regNumberPfrIp", Value = RegNumberPfrIp };
            yield return new InputPropertyInfo { Name = "dateRegNumPfrIp", Value = DateRegNumPfrIp };
            yield return new InputPropertyInfo { Name = "branchPFRIP", Value = BranchPfrip };
            yield return new InputPropertyInfo { Name = "grnRegPFRIP", Value = GrnRegPfrip };
            yield return new InputPropertyInfo { Name = "dateGrnRegPfrIp", Value = DateGrnRegPfrIp };
            yield return new InputPropertyInfo { Name = "dateUpdateEgrIP", Value = DateUpdateEgrIp };
        }
    }

    public partial class dicSubjectTypeCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string SubjectTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectTypeName", Value = SubjectTypeName };
        }
    }

    public partial class ExtendedAttribValueFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ExtAttributeContent { get; set; }
        public ICollection<ExtendedAttribValueFilter> Or { get; set; }
        public ICollection<ExtendedAttribValueFilter> And { get; set; }
        public ExtendedAttribValueFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeContent", Value = ExtAttributeContent };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlOrganizationFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlOrganizationName { get; set; }
        public StringFilterOperator OgrnKno { get; set; }
        public ICollection<ControlOrganizationFilter> Or { get; set; }
        public ICollection<ControlOrganizationFilter> And { get; set; }
        public ControlOrganizationFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrganizationName", Value = ControlOrganizationName };
            yield return new InputPropertyInfo { Name = "OGRN_KNO", Value = OgrnKno };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlCardUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ProcDataAgreement { get; set; }
        public object ControlStartDate { get; set; }
        public decimal? ControlDuration { get; set; }
        public decimal? DurationProlong { get; set; }
        public object ControlEndDate { get; set; }
        public string ControlPurpose { get; set; }
        public bool? IsJoint { get; set; }
        public string NumberFgiserp { get; set; }
        public object FgiserpRegData { get; set; }
        public object LastEndControlDate { get; set; }
        public string CheckControlRestrict { get; set; }
        public string ControlCancelInfo { get; set; }
        public string InternalNumberFgiserp { get; set; }
        public object ControlFactStartDate { get; set; }
        public object ControlFactEndDate { get; set; }
        public decimal? FactControlPeriod { get; set; }
        public string FactControlPeriodUnit { get; set; }
        public string JointControlOrganization { get; set; }
        public Guid? ControlItemPassportId { get; set; }
        public Guid? MeasureUnitTypeId { get; set; }
        public Guid? ControlFormId { get; set; }
        public Guid? ControlPlanId { get; set; }
        public Guid? ControlBaseId { get; set; }
        public Guid? ControlReasonDenyId { get; set; }
        public Guid? ControlItemBaseTypeId { get; set; }
        public ICollection<Guid> ControlCardPersons { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcDataAgreement", Value = ProcDataAgreement };
            yield return new InputPropertyInfo { Name = "ControlStartDate", Value = ControlStartDate };
            yield return new InputPropertyInfo { Name = "ControlDuration", Value = ControlDuration };
            yield return new InputPropertyInfo { Name = "DurationProlong", Value = DurationProlong };
            yield return new InputPropertyInfo { Name = "ControlEndDate", Value = ControlEndDate };
            yield return new InputPropertyInfo { Name = "ControlPurpose", Value = ControlPurpose };
            yield return new InputPropertyInfo { Name = "IsJoint", Value = IsJoint };
            yield return new InputPropertyInfo { Name = "NumberFGISERP", Value = NumberFgiserp };
            yield return new InputPropertyInfo { Name = "FGISERPRegData", Value = FgiserpRegData };
            yield return new InputPropertyInfo { Name = "LastEndControlDate", Value = LastEndControlDate };
            yield return new InputPropertyInfo { Name = "CheckControlRestrict", Value = CheckControlRestrict };
            yield return new InputPropertyInfo { Name = "ControlCancelInfo", Value = ControlCancelInfo };
            yield return new InputPropertyInfo { Name = "InternalNumberFGISERP", Value = InternalNumberFgiserp };
            yield return new InputPropertyInfo { Name = "ControlFactStartDate", Value = ControlFactStartDate };
            yield return new InputPropertyInfo { Name = "ControlFactEndDate", Value = ControlFactEndDate };
            yield return new InputPropertyInfo { Name = "FactControlPeriod", Value = FactControlPeriod };
            yield return new InputPropertyInfo { Name = "FactControlPeriodUnit", Value = FactControlPeriodUnit };
            yield return new InputPropertyInfo { Name = "JointControlOrganization", Value = JointControlOrganization };
            yield return new InputPropertyInfo { Name = "ControlItemPassportId", Value = ControlItemPassportId };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeId", Value = MeasureUnitTypeId };
            yield return new InputPropertyInfo { Name = "ControlFormId", Value = ControlFormId };
            yield return new InputPropertyInfo { Name = "ControlPlanId", Value = ControlPlanId };
            yield return new InputPropertyInfo { Name = "ControlBaseId", Value = ControlBaseId };
            yield return new InputPropertyInfo { Name = "ControlReasonDenyId", Value = ControlReasonDenyId };
            yield return new InputPropertyInfo { Name = "ControlItemBaseTypeId", Value = ControlItemBaseTypeId };
            yield return new InputPropertyInfo { Name = "ControlCardPersons", Value = ControlCardPersons };
        }
    }

    public partial class dicControlReasonDenyFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlReasonDeny { get; set; }
        public ICollection<dicControlReasonDenyFilter> Or { get; set; }
        public ICollection<dicControlReasonDenyFilter> And { get; set; }
        public dicControlReasonDenyFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlReasonDeny", Value = ControlReasonDeny };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlItemResultFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public FloatFilterOperator SumAdmFine { get; set; }
        public BooleanFilterOperator SumAdmFineStatus { get; set; }
        public ICollection<ControlItemResultFilter> Or { get; set; }
        public ICollection<ControlItemResultFilter> And { get; set; }
        public ControlItemResultFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "sumAdmFine", Value = SumAdmFine };
            yield return new InputPropertyInfo { Name = "SumAdmFineStatus", Value = SumAdmFineStatus };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlFileCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlFileNumber { get; set; }
        public object ControlFileStartDate { get; set; }
        public Guid? ControlProgramId { get; set; }
        public Guid? ControlFileStatusId { get; set; }
        public Guid? ControObjectId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileNumber", Value = ControlFileNumber };
            yield return new InputPropertyInfo { Name = "ControlFileStartDate", Value = ControlFileStartDate };
            yield return new InputPropertyInfo { Name = "ControlProgramId", Value = ControlProgramId };
            yield return new InputPropertyInfo { Name = "ControlFileStatusId", Value = ControlFileStatusId };
            yield return new InputPropertyInfo { Name = "ControObjectId", Value = ControObjectId };
        }
    }

    public partial class dicControlFileStatusCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlFileStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileStatusName", Value = ControlFileStatusName };
        }
    }

    public partial class dicRiskCategoryCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string RiskCategoryName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RiskCategoryName", Value = RiskCategoryName };
        }
    }

    public partial class dicKNMFormCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string KnmFormName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMFormName", Value = KnmFormName };
        }
    }

    public partial class dicProsecCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProsecName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProsecName", Value = ProsecName };
        }
    }

    public partial class dicViolationTypesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Violation { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Violation", Value = Violation };
        }
    }

    public partial class dicRoleFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator RoleName { get; set; }
        public ICollection<dicRoleFilter> Or { get; set; }
        public ICollection<dicRoleFilter> And { get; set; }
        public dicRoleFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RoleName", Value = RoleName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicControlBaseCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlBaseName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlBaseName", Value = ControlBaseName };
        }
    }

    public partial class BooleanFilterOperator : IGraphQlInputObject
    {
        public bool? EqualTo { get; set; }
        public bool? NotEqualTo { get; set; }
        public bool? GreaterThan { get; set; }
        public bool? GreaterThanOrEqual { get; set; }
        public bool? LessThan { get; set; }
        public bool? LessThanOrEqual { get; set; }
        public ICollection<bool> In { get; set; }
        public ICollection<bool> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class MandatoryReqsCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MandratoryReqContent { get; set; }
        public object StartDateMandatory { get; set; }
        public object EndDateMandatory { get; set; }
        public ICollection<Guid> Npa { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MandratoryReqContent", Value = MandratoryReqContent };
            yield return new InputPropertyInfo { Name = "StartDateMandatory", Value = StartDateMandatory };
            yield return new InputPropertyInfo { Name = "EndDateMandatory", Value = EndDateMandatory };
            yield return new InputPropertyInfo { Name = "NPA", Value = Npa };
        }
    }

    public partial class ControlListQuestionsCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string QuestionContent { get; set; }
        public string OuidBmQuestionInspection { get; set; }
        public string OuidBmInspectionListResult { get; set; }
        public Guid? LinkedControlCardId { get; set; }
        public Guid? QuestionAnswersId { get; set; }
        public Guid? ControlListId { get; set; }
        public Guid? MandatoryReqId { get; set; }
        public Guid? NpaId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionContent", Value = QuestionContent };
            yield return new InputPropertyInfo { Name = "ouid_bmQuestionInspection", Value = OuidBmQuestionInspection };
            yield return new InputPropertyInfo { Name = "ouid_bmInspectionListResult", Value = OuidBmInspectionListResult };
            yield return new InputPropertyInfo { Name = "LinkedControlCardId", Value = LinkedControlCardId };
            yield return new InputPropertyInfo { Name = "QuestionAnswersId", Value = QuestionAnswersId };
            yield return new InputPropertyInfo { Name = "ControlListId", Value = ControlListId };
            yield return new InputPropertyInfo { Name = "MandatoryReqId", Value = MandatoryReqId };
            yield return new InputPropertyInfo { Name = "NPAId", Value = NpaId };
        }
    }

    public partial class OrganizationUnitCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string OrganizationUnitName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "OrganizationUnitName", Value = OrganizationUnitName };
        }
    }

    public partial class LicenceCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicenceName { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public object StartDate { get; set; }
        public object StopDate { get; set; }
        public object EndDate { get; set; }
        public string BlankNumber { get; set; }
        public string LicenseFinalDecision { get; set; }
        public object LicenseFinalStartDate { get; set; }
        public object LicenseFinalEndDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceName", Value = LicenceName };
            yield return new InputPropertyInfo { Name = "Series", Value = Series };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "startDate", Value = StartDate };
            yield return new InputPropertyInfo { Name = "stopDate", Value = StopDate };
            yield return new InputPropertyInfo { Name = "endDate", Value = EndDate };
            yield return new InputPropertyInfo { Name = "BlankNumber", Value = BlankNumber };
            yield return new InputPropertyInfo { Name = "licenseFinalDecision", Value = LicenseFinalDecision };
            yield return new InputPropertyInfo { Name = "licenseFinalStartDate", Value = LicenseFinalStartDate };
            yield return new InputPropertyInfo { Name = "licenseFinalEndDate", Value = LicenseFinalEndDate };
        }
    }

    public partial class IDFilterOperator : IGraphQlInputObject
    {
        public Guid? EqualTo { get; set; }
        public Guid? NotEqualTo { get; set; }
        public Guid? GreaterThan { get; set; }
        public Guid? GreaterThanOrEqual { get; set; }
        public Guid? LessThan { get; set; }
        public Guid? LessThanOrEqual { get; set; }
        public ICollection<Guid> In { get; set; }
        public ICollection<Guid> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class DateTimeFilterOperator : IGraphQlInputObject
    {
        public object EqualTo { get; set; }
        public object NotEqualTo { get; set; }
        public object GreaterThan { get; set; }
        public object GreaterThanOrEqual { get; set; }
        public object LessThan { get; set; }
        public object LessThanOrEqual { get; set; }
        public ICollection<object> In { get; set; }
        public ICollection<object> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class StringFilterOperator : IGraphQlInputObject
    {
        public string EqualTo { get; set; }
        public string NotEqualTo { get; set; }
        public string GreaterThan { get; set; }
        public string GreaterThanOrEqual { get; set; }
        public string LessThan { get; set; }
        public string LessThanOrEqual { get; set; }
        public ICollection<string> In { get; set; }
        public ICollection<string> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class ControlItemCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlItemName { get; set; }
        public object ControlDate { get; set; }
        public string ControlItemResult { get; set; }
        public ICollection<Guid> ControlItemPersons { get; set; }
        public Guid? ControlProgramId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemName", Value = ControlItemName };
            yield return new InputPropertyInfo { Name = "ControlDate", Value = ControlDate };
            yield return new InputPropertyInfo { Name = "ControlItemResult", Value = ControlItemResult };
            yield return new InputPropertyInfo { Name = "ControlItemPersons", Value = ControlItemPersons };
            yield return new InputPropertyInfo { Name = "ControlProgramId", Value = ControlProgramId };
        }
    }

    public partial class DicMeasureUnitsTypeCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MeasureUnitTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeName", Value = MeasureUnitTypeName };
        }
    }

    public partial class dicControlBaseUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlBaseName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlBaseName", Value = ControlBaseName };
        }
    }

    public partial class JournalAttributesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator JournalAttirbuteName { get; set; }
        public StringFilterOperator JournalAttributeValue { get; set; }
        public ICollection<JournalAttributesFilter> Or { get; set; }
        public ICollection<JournalAttributesFilter> And { get; set; }
        public JournalAttributesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalAttirbuteName", Value = JournalAttirbuteName };
            yield return new InputPropertyInfo { Name = "JournalAttributeValue", Value = JournalAttributeValue };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicLicencedActivityTypesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicensedActivityCode { get; set; }
        public string LicensedActivityTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicensedActivityCode", Value = LicensedActivityCode };
            yield return new InputPropertyInfo { Name = "LicensedActivityTypeName", Value = LicensedActivityTypeName };
        }
    }

    public partial class SubjectCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MainName { get; set; }
        public string Address { get; set; }
        public string Citizenship { get; set; }
        public string RepresentativeSurname { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string NameOrg { get; set; }
        public Guid? SubjectTypesId { get; set; }
        public ICollection<Guid> ControlType { get; set; }
        public Guid? AssignedOfficerId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "mainName", Value = MainName };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "Citizenship", Value = Citizenship };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "NameOrg", Value = NameOrg };
            yield return new InputPropertyInfo { Name = "SubjectTypesId", Value = SubjectTypesId };
            yield return new InputPropertyInfo { Name = "ControlType", Value = ControlType };
            yield return new InputPropertyInfo { Name = "AssignedOfficerId", Value = AssignedOfficerId };
        }
    }

    public partial class dicOKOPFCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string SubjectName { get; set; }
        public string CodeOkopf { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectName", Value = SubjectName };
            yield return new InputPropertyInfo { Name = "CodeOKOPF", Value = CodeOkopf };
        }
    }

    public partial class docControlPlanCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object CreateDate { get; set; }
        public string ControlPlanName { get; set; }
        public object ControlPlanApproveData { get; set; }
        public string FgiserpGlobalPlanGuid { get; set; }
        public string ControlPlanYear { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CreateDate", Value = CreateDate };
            yield return new InputPropertyInfo { Name = "ControlPlanName", Value = ControlPlanName };
            yield return new InputPropertyInfo { Name = "ControlPlanApproveData", Value = ControlPlanApproveData };
            yield return new InputPropertyInfo { Name = "FGISERPGlobalPlanGUID", Value = FgiserpGlobalPlanGuid };
            yield return new InputPropertyInfo { Name = "ControlPlanYear", Value = ControlPlanYear };
        }
    }

    public partial class dicOKSMFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator Code { get; set; }
        public StringFilterOperator FullCountryName { get; set; }
        public StringFilterOperator ShortCountryName { get; set; }
        public StringFilterOperator LetterCodeAlpha2 { get; set; }
        public StringFilterOperator LetterCodeAlpha3 { get; set; }
        public ICollection<dicOKSMFilter> Or { get; set; }
        public ICollection<dicOKSMFilter> And { get; set; }
        public dicOKSMFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Code", Value = Code };
            yield return new InputPropertyInfo { Name = "FullCountryName", Value = FullCountryName };
            yield return new InputPropertyInfo { Name = "ShortCountryName", Value = ShortCountryName };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha2", Value = LetterCodeAlpha2 };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha3", Value = LetterCodeAlpha3 };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class OrganizationUnitFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator OrganizationUnitName { get; set; }
        public ICollection<OrganizationUnitFilter> Or { get; set; }
        public ICollection<OrganizationUnitFilter> And { get; set; }
        public OrganizationUnitFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "OrganizationUnitName", Value = OrganizationUnitName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicLicenceStatusCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicenceStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceStatusName", Value = LicenceStatusName };
        }
    }

    public partial class ControlOrganizationCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlOrganizationName { get; set; }
        public string OgrnKno { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrganizationName", Value = ControlOrganizationName };
            yield return new InputPropertyInfo { Name = "OGRN_KNO", Value = OgrnKno };
        }
    }

    public partial class MaterialsUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? ControlActFileId { get; set; }
        public Guid? ControlListId { get; set; }
        public Guid? ControlOrderFileId { get; set; }
        public Guid? LinkedFileId { get; set; }
        public Guid? DocRegulationId { get; set; }
        public Guid? DocControlPlanId { get; set; }
        public Guid? DocProcStatementId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "ControlActFileId", Value = ControlActFileId };
            yield return new InputPropertyInfo { Name = "ControlListId", Value = ControlListId };
            yield return new InputPropertyInfo { Name = "ControlOrderFileId", Value = ControlOrderFileId };
            yield return new InputPropertyInfo { Name = "LinkedFileId", Value = LinkedFileId };
            yield return new InputPropertyInfo { Name = "docRegulationId", Value = DocRegulationId };
            yield return new InputPropertyInfo { Name = "docControlPlanId", Value = DocControlPlanId };
            yield return new InputPropertyInfo { Name = "docProcStatementId", Value = DocProcStatementId };
        }
    }

    public partial class dicNPALevelsUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaLevelsName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPALevelsName", Value = NpaLevelsName };
        }
    }

    public partial class dicViolationTypesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Violation { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Violation", Value = Violation };
        }
    }

    public partial class dicNPALevelsCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaLevelsName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPALevelsName", Value = NpaLevelsName };
        }
    }

    public partial class ActivityCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ActivityStartDate { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? OkvedId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityStartDate", Value = ActivityStartDate };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "OKVEDId", Value = OkvedId };
        }
    }

    public partial class dicProcStatmentStatusCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProcStatementStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcStatementStatusName", Value = ProcStatementStatusName };
        }
    }

    public partial class dicControlListStatusCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlListStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStatusName", Value = ControlListStatusName };
        }
    }

    public partial class MandatoryReqsFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator MandratoryReqContent { get; set; }
        public DateFilterOperator StartDateMandatory { get; set; }
        public DateFilterOperator EndDateMandatory { get; set; }
        public ICollection<MandatoryReqsFilter> Or { get; set; }
        public ICollection<MandatoryReqsFilter> And { get; set; }
        public MandatoryReqsFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MandratoryReqContent", Value = MandratoryReqContent };
            yield return new InputPropertyInfo { Name = "StartDateMandatory", Value = StartDateMandatory };
            yield return new InputPropertyInfo { Name = "EndDateMandatory", Value = EndDateMandatory };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicQuestionAnswersCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string QuestionAnswer { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionAnswer", Value = QuestionAnswer };
        }
    }

    public partial class ActivityFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator ActivityStartDate { get; set; }
        public ICollection<ActivityFilter> Or { get; set; }
        public ICollection<ActivityFilter> And { get; set; }
        public ActivityFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityStartDate", Value = ActivityStartDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class AddressCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CodeKladr { get; set; }
        public string CodeFias { get; set; }
        public string Address { get; set; }
        public string PostIndex { get; set; }
        public string AddressFact { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeKLADR", Value = CodeKladr };
            yield return new InputPropertyInfo { Name = "CodeFIAS", Value = CodeFias };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "PostIndex", Value = PostIndex };
            yield return new InputPropertyInfo { Name = "AddressFact", Value = AddressFact };
        }
    }

    public partial class docControlListFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator ControlListStartDate { get; set; }
        public DateFilterOperator ControlListEndDate { get; set; }
        public ICollection<docControlListFilter> Or { get; set; }
        public ICollection<docControlListFilter> And { get; set; }
        public docControlListFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStartDate", Value = ControlListStartDate };
            yield return new InputPropertyInfo { Name = "ControlListEndDate", Value = ControlListEndDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicLicencedActivityTypesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicensedActivityCode { get; set; }
        public string LicensedActivityTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicensedActivityCode", Value = LicensedActivityCode };
            yield return new InputPropertyInfo { Name = "LicensedActivityTypeName", Value = LicensedActivityTypeName };
        }
    }

    public partial class docProcStatementFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator SendToProcDate { get; set; }
        public StringFilterOperator ProcApproveFio { get; set; }
        public StringFilterOperator ProcApproveRole { get; set; }
        public StringFilterOperator ProcApprovePlace { get; set; }
        public ICollection<docProcStatementFilter> Or { get; set; }
        public ICollection<docProcStatementFilter> And { get; set; }
        public docProcStatementFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SendToProcDate", Value = SendToProcDate };
            yield return new InputPropertyInfo { Name = "ProcApproveFIO", Value = ProcApproveFio };
            yield return new InputPropertyInfo { Name = "ProcApproveRole", Value = ProcApproveRole };
            yield return new InputPropertyInfo { Name = "ProcApprovePlace", Value = ProcApprovePlace };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicControlListStatusCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlListStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStatusName", Value = ControlListStatusName };
        }
    }

    public partial class CitizenRequestUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantSurname { get; set; }
        public string SecondName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string RequestContent { get; set; }
        public string ViolationCondition { get; set; }
        public object RequestGetDate { get; set; }
        public string TargetControlOrganization { get; set; }
        public string TargetControlOrganizationOgrn { get; set; }
        public Guid? ApplicantAddressId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ApplicantName", Value = ApplicantName };
            yield return new InputPropertyInfo { Name = "ApplicantSurname", Value = ApplicantSurname };
            yield return new InputPropertyInfo { Name = "SecondName", Value = SecondName };
            yield return new InputPropertyInfo { Name = "ApplicantEmail", Value = ApplicantEmail };
            yield return new InputPropertyInfo { Name = "ApplicantPhone", Value = ApplicantPhone };
            yield return new InputPropertyInfo { Name = "RequestContent", Value = RequestContent };
            yield return new InputPropertyInfo { Name = "ViolationCondition", Value = ViolationCondition };
            yield return new InputPropertyInfo { Name = "RequestGetDate", Value = RequestGetDate };
            yield return new InputPropertyInfo { Name = "TargetControlOrganization", Value = TargetControlOrganization };
            yield return new InputPropertyInfo { Name = "TargetControlOrganizationOGRN", Value = TargetControlOrganizationOgrn };
            yield return new InputPropertyInfo { Name = "ApplicantAddressId", Value = ApplicantAddressId };
        }
    }

    public partial class ActivityCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ActivityStartDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityStartDate", Value = ActivityStartDate };
        }
    }

    public partial class dicDamageTypeUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string DamageType { get; set; }
        public ICollection<Guid> ControlItemResults { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "DamageType", Value = DamageType };
            yield return new InputPropertyInfo { Name = "ControlItemResults", Value = ControlItemResults };
        }
    }

    public partial class RandEParameterCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Name", Value = Name };
        }
    }

    public partial class dicProcStatmentStatusCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProcStatementStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcStatementStatusName", Value = ProcStatementStatusName };
        }
    }

    public partial class ControlJournalsFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator JournalCreationDate { get; set; }
        public StringFilterOperator JournalName { get; set; }
        public ICollection<ControlJournalsFilter> Or { get; set; }
        public ICollection<ControlJournalsFilter> And { get; set; }
        public ControlJournalsFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalCreationDate", Value = JournalCreationDate };
            yield return new InputPropertyInfo { Name = "JournalName", Value = JournalName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ExtendedAttribValueCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ExtAttributeContent { get; set; }
        public Guid? ControlObjectId { get; set; }
        public Guid? ExtendedAttribId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeContent", Value = ExtAttributeContent };
            yield return new InputPropertyInfo { Name = "ControlObjectId", Value = ControlObjectId };
            yield return new InputPropertyInfo { Name = "ExtendedAttribId", Value = ExtendedAttribId };
        }
    }

    public partial class DicMeasureUnitsTypeCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MeasureUnitTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeName", Value = MeasureUnitTypeName };
        }
    }

    public partial class ControlOrganizationUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlOrganizationName { get; set; }
        public string OgrnKno { get; set; }
        public ICollection<Guid> ControlType { get; set; }
        public Guid? HeadOrganizationId { get; set; }
        public ICollection<Guid> RandEParameters { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrganizationName", Value = ControlOrganizationName };
            yield return new InputPropertyInfo { Name = "OGRN_KNO", Value = OgrnKno };
            yield return new InputPropertyInfo { Name = "ControlType", Value = ControlType };
            yield return new InputPropertyInfo { Name = "HeadOrganizationId", Value = HeadOrganizationId };
            yield return new InputPropertyInfo { Name = "RandEParameters", Value = RandEParameters };
        }
    }

    public partial class ControlItemFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlItemName { get; set; }
        public DateFilterOperator ControlDate { get; set; }
        public StringFilterOperator ControlItemResult { get; set; }
        public ICollection<ControlItemFilter> Or { get; set; }
        public ICollection<ControlItemFilter> And { get; set; }
        public ControlItemFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemName", Value = ControlItemName };
            yield return new InputPropertyInfo { Name = "ControlDate", Value = ControlDate };
            yield return new InputPropertyInfo { Name = "ControlItemResult", Value = ControlItemResult };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class CheckOutEGRULCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public object DateUpdateEgrul { get; set; }
        public string Ogrnul { get; set; }
        public object OgrnuLdata { get; set; }
        public string Innul { get; set; }
        public object DateRegYl { get; set; }
        public string OrgNameRegYl { get; set; }
        public string AddressRegOrgYl { get; set; }
        public string GrnOrgLocationYl { get; set; }
        public object GrnDateOrgLocationYl { get; set; }
        public object DateTaxReg { get; set; }
        public string TaxOrg { get; set; }
        public string GrnTaxOrg { get; set; }
        public object DateGrnTaxOrg { get; set; }
        public string RegNumberPfryl { get; set; }
        public object DateRegNumPfryl { get; set; }
        public string BranchPfryl { get; set; }
        public string GrnRegPfryl { get; set; }
        public object DateGrnRegPfryl { get; set; }
        public string GrnRegExecPfryl { get; set; }
        public string RegNumberExecPfryl { get; set; }
        public object DateRegNumExecPfryl { get; set; }
        public string BranchExecSsyl { get; set; }
        public string GrnRegExecSsyl { get; set; }
        public object DateGrnExecSsyl { get; set; }
        public string TypeCharterCapital { get; set; }
        public string SumCharterCapital { get; set; }
        public string GrnCharterCapital { get; set; }
        public object DateGrnCharterCapital { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string RepresentativeSurname { get; set; }
        public string Kpp { get; set; }
        public string ShortNameOrg { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "dateUpdateEGRUL", Value = DateUpdateEgrul };
            yield return new InputPropertyInfo { Name = "OGRNUL", Value = Ogrnul };
            yield return new InputPropertyInfo { Name = "OGRNULdata", Value = OgrnuLdata };
            yield return new InputPropertyInfo { Name = "INNUL", Value = Innul };
            yield return new InputPropertyInfo { Name = "dateRegYL", Value = DateRegYl };
            yield return new InputPropertyInfo { Name = "orgNameRegYL", Value = OrgNameRegYl };
            yield return new InputPropertyInfo { Name = "addressRegOrgYL", Value = AddressRegOrgYl };
            yield return new InputPropertyInfo { Name = "grnOrgLocationYL", Value = GrnOrgLocationYl };
            yield return new InputPropertyInfo { Name = "grnDateOrgLocationYL", Value = GrnDateOrgLocationYl };
            yield return new InputPropertyInfo { Name = "dateTaxReg", Value = DateTaxReg };
            yield return new InputPropertyInfo { Name = "taxOrg", Value = TaxOrg };
            yield return new InputPropertyInfo { Name = "grnTaxOrg", Value = GrnTaxOrg };
            yield return new InputPropertyInfo { Name = "dateGRNTaxOrg", Value = DateGrnTaxOrg };
            yield return new InputPropertyInfo { Name = "RegNumberPFRYL", Value = RegNumberPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumPFRYL", Value = DateRegNumPfryl };
            yield return new InputPropertyInfo { Name = "branchPFRYL", Value = BranchPfryl };
            yield return new InputPropertyInfo { Name = "GrnRegPFRYL", Value = GrnRegPfryl };
            yield return new InputPropertyInfo { Name = "dateGrnRegPFRYL", Value = DateGrnRegPfryl };
            yield return new InputPropertyInfo { Name = "grnRegExecPFRYL", Value = GrnRegExecPfryl };
            yield return new InputPropertyInfo { Name = "regNumberExecPFRYL", Value = RegNumberExecPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumExecPFRYL", Value = DateRegNumExecPfryl };
            yield return new InputPropertyInfo { Name = "branchExecSSYL", Value = BranchExecSsyl };
            yield return new InputPropertyInfo { Name = "grnRegExecSSYL", Value = GrnRegExecSsyl };
            yield return new InputPropertyInfo { Name = "dateGrnExecSSYL", Value = DateGrnExecSsyl };
            yield return new InputPropertyInfo { Name = "typeCharterCapital", Value = TypeCharterCapital };
            yield return new InputPropertyInfo { Name = "sumCharterCapital", Value = SumCharterCapital };
            yield return new InputPropertyInfo { Name = "grnCharterCapital", Value = GrnCharterCapital };
            yield return new InputPropertyInfo { Name = "dateGRNCharterCapital", Value = DateGrnCharterCapital };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "KPP", Value = Kpp };
            yield return new InputPropertyInfo { Name = "ShortNameOrg", Value = ShortNameOrg };
        }
    }

    public partial class PersonCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string PersonSecondName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PersonName", Value = PersonName };
            yield return new InputPropertyInfo { Name = "PersonSurName", Value = PersonSurName };
            yield return new InputPropertyInfo { Name = "PersonSecondName", Value = PersonSecondName };
        }
    }

    public partial class dicProsecFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ProsecName { get; set; }
        public ICollection<dicProsecFilter> Or { get; set; }
        public ICollection<dicProsecFilter> And { get; set; }
        public dicProsecFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProsecName", Value = ProsecName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docControlOrderFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator ControlOrderCreateDate { get; set; }
        public DateFilterOperator ControlOrderSignDate { get; set; }
        public StringFilterOperator ControlOrderNumber { get; set; }
        public ICollection<docControlOrderFilter> Or { get; set; }
        public ICollection<docControlOrderFilter> And { get; set; }
        public docControlOrderFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrderCreateDate", Value = ControlOrderCreateDate };
            yield return new InputPropertyInfo { Name = "ControlOrderSignDate", Value = ControlOrderSignDate };
            yield return new InputPropertyInfo { Name = "ControlOrderNumber", Value = ControlOrderNumber };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlCardCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ProcDataAgreement { get; set; }
        public object ControlStartDate { get; set; }
        public decimal? ControlDuration { get; set; }
        public decimal? DurationProlong { get; set; }
        public object ControlEndDate { get; set; }
        public string ControlPurpose { get; set; }
        public bool? IsJoint { get; set; }
        public string NumberFgiserp { get; set; }
        public object FgiserpRegData { get; set; }
        public object LastEndControlDate { get; set; }
        public string CheckControlRestrict { get; set; }
        public string ControlCancelInfo { get; set; }
        public string InternalNumberFgiserp { get; set; }
        public object ControlFactStartDate { get; set; }
        public object ControlFactEndDate { get; set; }
        public decimal? FactControlPeriod { get; set; }
        public string FactControlPeriodUnit { get; set; }
        public string JointControlOrganization { get; set; }
        public Guid? ControlItemPassportId { get; set; }
        public Guid? MeasureUnitTypeId { get; set; }
        public Guid? ControlFormId { get; set; }
        public Guid? ControlPlanId { get; set; }
        public Guid? ControlBaseId { get; set; }
        public Guid? ControlReasonDenyId { get; set; }
        public Guid? ControlItemBaseTypeId { get; set; }
        public ICollection<Guid> ControlCardPersons { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcDataAgreement", Value = ProcDataAgreement };
            yield return new InputPropertyInfo { Name = "ControlStartDate", Value = ControlStartDate };
            yield return new InputPropertyInfo { Name = "ControlDuration", Value = ControlDuration };
            yield return new InputPropertyInfo { Name = "DurationProlong", Value = DurationProlong };
            yield return new InputPropertyInfo { Name = "ControlEndDate", Value = ControlEndDate };
            yield return new InputPropertyInfo { Name = "ControlPurpose", Value = ControlPurpose };
            yield return new InputPropertyInfo { Name = "IsJoint", Value = IsJoint };
            yield return new InputPropertyInfo { Name = "NumberFGISERP", Value = NumberFgiserp };
            yield return new InputPropertyInfo { Name = "FGISERPRegData", Value = FgiserpRegData };
            yield return new InputPropertyInfo { Name = "LastEndControlDate", Value = LastEndControlDate };
            yield return new InputPropertyInfo { Name = "CheckControlRestrict", Value = CheckControlRestrict };
            yield return new InputPropertyInfo { Name = "ControlCancelInfo", Value = ControlCancelInfo };
            yield return new InputPropertyInfo { Name = "InternalNumberFGISERP", Value = InternalNumberFgiserp };
            yield return new InputPropertyInfo { Name = "ControlFactStartDate", Value = ControlFactStartDate };
            yield return new InputPropertyInfo { Name = "ControlFactEndDate", Value = ControlFactEndDate };
            yield return new InputPropertyInfo { Name = "FactControlPeriod", Value = FactControlPeriod };
            yield return new InputPropertyInfo { Name = "FactControlPeriodUnit", Value = FactControlPeriodUnit };
            yield return new InputPropertyInfo { Name = "JointControlOrganization", Value = JointControlOrganization };
            yield return new InputPropertyInfo { Name = "ControlItemPassportId", Value = ControlItemPassportId };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeId", Value = MeasureUnitTypeId };
            yield return new InputPropertyInfo { Name = "ControlFormId", Value = ControlFormId };
            yield return new InputPropertyInfo { Name = "ControlPlanId", Value = ControlPlanId };
            yield return new InputPropertyInfo { Name = "ControlBaseId", Value = ControlBaseId };
            yield return new InputPropertyInfo { Name = "ControlReasonDenyId", Value = ControlReasonDenyId };
            yield return new InputPropertyInfo { Name = "ControlItemBaseTypeId", Value = ControlItemBaseTypeId };
            yield return new InputPropertyInfo { Name = "ControlCardPersons", Value = ControlCardPersons };
        }
    }

    public partial class dicViolationTypesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator Violation { get; set; }
        public ICollection<dicViolationTypesFilter> Or { get; set; }
        public ICollection<dicViolationTypesFilter> And { get; set; }
        public dicViolationTypesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Violation", Value = Violation };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docProcStatementCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object SendToProcDate { get; set; }
        public string ProcApproveFio { get; set; }
        public string ProcApproveRole { get; set; }
        public string ProcApprovePlace { get; set; }
        public Guid? LinkedControlCardId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SendToProcDate", Value = SendToProcDate };
            yield return new InputPropertyInfo { Name = "ProcApproveFIO", Value = ProcApproveFio };
            yield return new InputPropertyInfo { Name = "ProcApproveRole", Value = ProcApproveRole };
            yield return new InputPropertyInfo { Name = "ProcApprovePlace", Value = ProcApprovePlace };
            yield return new InputPropertyInfo { Name = "LinkedControlCardId", Value = LinkedControlCardId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class CheckOutEGRSMSPUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object DateRmsp { get; set; }
        public string CaseNumber { get; set; }
        public bool? IsInRmsp { get; set; }
        public object LastRenewData { get; set; }
        public Guid? SubjectEgrsmspId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "dateRMSP", Value = DateRmsp };
            yield return new InputPropertyInfo { Name = "caseNumber", Value = CaseNumber };
            yield return new InputPropertyInfo { Name = "IsInRMSP", Value = IsInRmsp };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "SubjectEGRSMSPId", Value = SubjectEgrsmspId };
        }
    }

    public partial class dicSubjectTypeUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string SubjectTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectTypeName", Value = SubjectTypeName };
        }
    }

    public partial class ActivityUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ActivityStartDate { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? OkvedId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityStartDate", Value = ActivityStartDate };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "OKVEDId", Value = OkvedId };
        }
    }

    public partial class dicLicencedActivityTypesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator LicensedActivityCode { get; set; }
        public StringFilterOperator LicensedActivityTypeName { get; set; }
        public ICollection<dicLicencedActivityTypesFilter> Or { get; set; }
        public ICollection<dicLicencedActivityTypesFilter> And { get; set; }
        public dicLicencedActivityTypesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicensedActivityCode", Value = LicensedActivityCode };
            yield return new InputPropertyInfo { Name = "LicensedActivityTypeName", Value = LicensedActivityTypeName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicQuestionAnswersCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string QuestionAnswer { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionAnswer", Value = QuestionAnswer };
        }
    }

    public partial class AddressUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CodeKladr { get; set; }
        public string CodeFias { get; set; }
        public string Address { get; set; }
        public string PostIndex { get; set; }
        public string AddressFact { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeKLADR", Value = CodeKladr };
            yield return new InputPropertyInfo { Name = "CodeFIAS", Value = CodeFias };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "PostIndex", Value = PostIndex };
            yield return new InputPropertyInfo { Name = "AddressFact", Value = AddressFact };
        }
    }

    public partial class ControlItemPassportCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object CreateDate { get; set; }
        public Guid? ControlCardId { get; set; }
        public ICollection<Guid> ControlObjects { get; set; }
        public Guid? ControlTypeId { get; set; }
        public Guid? ControlOrganizationId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? StatusKnmNameId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CreateDate", Value = CreateDate };
            yield return new InputPropertyInfo { Name = "ControlCardId", Value = ControlCardId };
            yield return new InputPropertyInfo { Name = "ControlObjects", Value = ControlObjects };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "StatusKNMNameId", Value = StatusKnmNameId };
        }
    }

    public partial class CheckOutEGRSMSPCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object DateRmsp { get; set; }
        public string CaseNumber { get; set; }
        public bool? IsInRmsp { get; set; }
        public object LastRenewData { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "dateRMSP", Value = DateRmsp };
            yield return new InputPropertyInfo { Name = "caseNumber", Value = CaseNumber };
            yield return new InputPropertyInfo { Name = "IsInRMSP", Value = IsInRmsp };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
        }
    }

    public partial class dicLicenceStatusFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator LicenceStatusName { get; set; }
        public ICollection<dicLicenceStatusFilter> Or { get; set; }
        public ICollection<dicLicenceStatusFilter> And { get; set; }
        public dicLicenceStatusFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceStatusName", Value = LicenceStatusName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docControlListUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlListStartDate { get; set; }
        public object ControlListEndDate { get; set; }
        public Guid? ControlItemResultId { get; set; }
        public Guid? MaterialsId { get; set; }
        public Guid? ControlTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStartDate", Value = ControlListStartDate };
            yield return new InputPropertyInfo { Name = "ControlListEndDate", Value = ControlListEndDate };
            yield return new InputPropertyInfo { Name = "ControlItemResultId", Value = ControlItemResultId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
        }
    }

    public partial class ViolationCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ViolationSpecificComment { get; set; }
        public Guid? ControlItemResultId { get; set; }
        public Guid? ViolationTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ViolationSpecificComment", Value = ViolationSpecificComment };
            yield return new InputPropertyInfo { Name = "ControlItemResultId", Value = ControlItemResultId };
            yield return new InputPropertyInfo { Name = "ViolationTypeId", Value = ViolationTypeId };
        }
    }

    public partial class PersonAppointmentCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object AppStartDate { get; set; }
        public object AppEndDate { get; set; }
        public Guid? PersonId { get; set; }
        public Guid? RoleId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "AppStartDate", Value = AppStartDate };
            yield return new InputPropertyInfo { Name = "AppEndDate", Value = AppEndDate };
            yield return new InputPropertyInfo { Name = "PersonId", Value = PersonId };
            yield return new InputPropertyInfo { Name = "RoleId", Value = RoleId };
        }
    }

    public partial class CheckOutEGRIPUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public string Ogrnip { get; set; }
        public string Innip { get; set; }
        public object OgrniPdate { get; set; }
        public object GrnEgripDate { get; set; }
        public string RegIpOld { get; set; }
        public string OrgRegLocationIp { get; set; }
        public string AddressOrgRegLocIp { get; set; }
        public string RegNumberPfrIp { get; set; }
        public object DateRegNumPfrIp { get; set; }
        public string BranchPfrip { get; set; }
        public string GrnRegPfrip { get; set; }
        public object DateGrnRegPfrIp { get; set; }
        public object DateUpdateEgrIp { get; set; }
        public Guid? SubjectEgripId { get; set; }
        public Guid? AddressId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "OGRNIP", Value = Ogrnip };
            yield return new InputPropertyInfo { Name = "INNIP", Value = Innip };
            yield return new InputPropertyInfo { Name = "OGRNIPdate", Value = OgrniPdate };
            yield return new InputPropertyInfo { Name = "grnEgripDate", Value = GrnEgripDate };
            yield return new InputPropertyInfo { Name = "regIpOld", Value = RegIpOld };
            yield return new InputPropertyInfo { Name = "orgRegLocationIp", Value = OrgRegLocationIp };
            yield return new InputPropertyInfo { Name = "addressOrgRegLocIp", Value = AddressOrgRegLocIp };
            yield return new InputPropertyInfo { Name = "regNumberPfrIp", Value = RegNumberPfrIp };
            yield return new InputPropertyInfo { Name = "dateRegNumPfrIp", Value = DateRegNumPfrIp };
            yield return new InputPropertyInfo { Name = "branchPFRIP", Value = BranchPfrip };
            yield return new InputPropertyInfo { Name = "grnRegPFRIP", Value = GrnRegPfrip };
            yield return new InputPropertyInfo { Name = "dateGrnRegPfrIp", Value = DateGrnRegPfrIp };
            yield return new InputPropertyInfo { Name = "dateUpdateEgrIP", Value = DateUpdateEgrIp };
            yield return new InputPropertyInfo { Name = "SubjectEGRIPId", Value = SubjectEgripId };
            yield return new InputPropertyInfo { Name = "AddressId", Value = AddressId };
        }
    }

    public partial class dicOKTMOUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string CodeOktmo { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeOKTMO", Value = CodeOktmo };
        }
    }

    public partial class ControlCardCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ProcDataAgreement { get; set; }
        public object ControlStartDate { get; set; }
        public string ControlStatus { get; set; }
        public decimal? ControlDuration { get; set; }
        public decimal? DurationProlong { get; set; }
        public object ControlEndDate { get; set; }
        public string ControlPurpose { get; set; }
        public bool? IsJoint { get; set; }
        public string NumberFgiserp { get; set; }
        public object FgiserpRegData { get; set; }
        public object LastEndControlDate { get; set; }
        public string CheckControlRestrict { get; set; }
        public string ControlCancelInfo { get; set; }
        public string InternalNumberFgiserp { get; set; }
        public object ControlFactStartDate { get; set; }
        public object ControlFactEndDate { get; set; }
        public decimal? FactControlPeriod { get; set; }
        public string FactControlPeriodUnit { get; set; }
        public string JointControlOrganization { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcDataAgreement", Value = ProcDataAgreement };
            yield return new InputPropertyInfo { Name = "ControlStartDate", Value = ControlStartDate };
            yield return new InputPropertyInfo { Name = "ControlStatus", Value = ControlStatus };
            yield return new InputPropertyInfo { Name = "ControlDuration", Value = ControlDuration };
            yield return new InputPropertyInfo { Name = "DurationProlong", Value = DurationProlong };
            yield return new InputPropertyInfo { Name = "ControlEndDate", Value = ControlEndDate };
            yield return new InputPropertyInfo { Name = "ControlPurpose", Value = ControlPurpose };
            yield return new InputPropertyInfo { Name = "IsJoint", Value = IsJoint };
            yield return new InputPropertyInfo { Name = "NumberFGISERP", Value = NumberFgiserp };
            yield return new InputPropertyInfo { Name = "FGISERPRegData", Value = FgiserpRegData };
            yield return new InputPropertyInfo { Name = "LastEndControlDate", Value = LastEndControlDate };
            yield return new InputPropertyInfo { Name = "CheckControlRestrict", Value = CheckControlRestrict };
            yield return new InputPropertyInfo { Name = "ControlCancelInfo", Value = ControlCancelInfo };
            yield return new InputPropertyInfo { Name = "InternalNumberFGISERP", Value = InternalNumberFgiserp };
            yield return new InputPropertyInfo { Name = "ControlFactStartDate", Value = ControlFactStartDate };
            yield return new InputPropertyInfo { Name = "ControlFactEndDate", Value = ControlFactEndDate };
            yield return new InputPropertyInfo { Name = "FactControlPeriod", Value = FactControlPeriod };
            yield return new InputPropertyInfo { Name = "FactControlPeriodUnit", Value = FactControlPeriodUnit };
            yield return new InputPropertyInfo { Name = "JointControlOrganization", Value = JointControlOrganization };
        }
    }

    public partial class docControlActCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlActCreateDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlActCreateDate", Value = ControlActCreateDate };
        }
    }

    public partial class OrganizationUnitUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string OrganizationUnitName { get; set; }
        public Guid? ControlOrganizationId { get; set; }
        public Guid? MainOrganizationUnitId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "OrganizationUnitName", Value = OrganizationUnitName };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
            yield return new InputPropertyInfo { Name = "MainOrganizationUnitId", Value = MainOrganizationUnitId };
        }
    }

    public partial class dicHazardClassUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string HazardClassName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassName", Value = HazardClassName };
        }
    }

    public partial class RandEParameterCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> ControlOrganizationParameter { get; set; }
        public Guid? MeasureUnitTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Name", Value = Name };
            yield return new InputPropertyInfo { Name = "ControlOrganizationParameter", Value = ControlOrganizationParameter };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeId", Value = MeasureUnitTypeId };
        }
    }

    public partial class ControlFileFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlFileNumber { get; set; }
        public DateFilterOperator ControlFileStartDate { get; set; }
        public ICollection<ControlFileFilter> Or { get; set; }
        public ICollection<ControlFileFilter> And { get; set; }
        public ControlFileFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileNumber", Value = ControlFileNumber };
            yield return new InputPropertyInfo { Name = "ControlFileStartDate", Value = ControlFileStartDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class CheckOutEGRULFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator LastRenewData { get; set; }
        public DateFilterOperator DateUpdateEgrul { get; set; }
        public StringFilterOperator Ogrnul { get; set; }
        public DateFilterOperator OgrnuLdata { get; set; }
        public StringFilterOperator Innul { get; set; }
        public DateFilterOperator DateRegYl { get; set; }
        public StringFilterOperator OrgNameRegYl { get; set; }
        public StringFilterOperator AddressRegOrgYl { get; set; }
        public StringFilterOperator GrnOrgLocationYl { get; set; }
        public DateFilterOperator GrnDateOrgLocationYl { get; set; }
        public DateFilterOperator DateTaxReg { get; set; }
        public StringFilterOperator TaxOrg { get; set; }
        public StringFilterOperator GrnTaxOrg { get; set; }
        public DateFilterOperator DateGrnTaxOrg { get; set; }
        public StringFilterOperator RegNumberPfryl { get; set; }
        public DateFilterOperator DateRegNumPfryl { get; set; }
        public StringFilterOperator BranchPfryl { get; set; }
        public StringFilterOperator GrnRegPfryl { get; set; }
        public DateFilterOperator DateGrnRegPfryl { get; set; }
        public StringFilterOperator GrnRegExecPfryl { get; set; }
        public StringFilterOperator RegNumberExecPfryl { get; set; }
        public DateFilterOperator DateRegNumExecPfryl { get; set; }
        public StringFilterOperator BranchExecSsyl { get; set; }
        public StringFilterOperator GrnRegExecSsyl { get; set; }
        public DateFilterOperator DateGrnExecSsyl { get; set; }
        public StringFilterOperator TypeCharterCapital { get; set; }
        public StringFilterOperator SumCharterCapital { get; set; }
        public StringFilterOperator GrnCharterCapital { get; set; }
        public DateFilterOperator DateGrnCharterCapital { get; set; }
        public StringFilterOperator RepresentativeName { get; set; }
        public StringFilterOperator RepresentativeSecondName { get; set; }
        public StringFilterOperator RepresentativeSurname { get; set; }
        public StringFilterOperator Kpp { get; set; }
        public StringFilterOperator ShortNameOrg { get; set; }
        public ICollection<CheckOutEGRULFilter> Or { get; set; }
        public ICollection<CheckOutEGRULFilter> And { get; set; }
        public CheckOutEGRULFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "dateUpdateEGRUL", Value = DateUpdateEgrul };
            yield return new InputPropertyInfo { Name = "OGRNUL", Value = Ogrnul };
            yield return new InputPropertyInfo { Name = "OGRNULdata", Value = OgrnuLdata };
            yield return new InputPropertyInfo { Name = "INNUL", Value = Innul };
            yield return new InputPropertyInfo { Name = "dateRegYL", Value = DateRegYl };
            yield return new InputPropertyInfo { Name = "orgNameRegYL", Value = OrgNameRegYl };
            yield return new InputPropertyInfo { Name = "addressRegOrgYL", Value = AddressRegOrgYl };
            yield return new InputPropertyInfo { Name = "grnOrgLocationYL", Value = GrnOrgLocationYl };
            yield return new InputPropertyInfo { Name = "grnDateOrgLocationYL", Value = GrnDateOrgLocationYl };
            yield return new InputPropertyInfo { Name = "dateTaxReg", Value = DateTaxReg };
            yield return new InputPropertyInfo { Name = "taxOrg", Value = TaxOrg };
            yield return new InputPropertyInfo { Name = "grnTaxOrg", Value = GrnTaxOrg };
            yield return new InputPropertyInfo { Name = "dateGRNTaxOrg", Value = DateGrnTaxOrg };
            yield return new InputPropertyInfo { Name = "RegNumberPFRYL", Value = RegNumberPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumPFRYL", Value = DateRegNumPfryl };
            yield return new InputPropertyInfo { Name = "branchPFRYL", Value = BranchPfryl };
            yield return new InputPropertyInfo { Name = "GrnRegPFRYL", Value = GrnRegPfryl };
            yield return new InputPropertyInfo { Name = "dateGrnRegPFRYL", Value = DateGrnRegPfryl };
            yield return new InputPropertyInfo { Name = "grnRegExecPFRYL", Value = GrnRegExecPfryl };
            yield return new InputPropertyInfo { Name = "regNumberExecPFRYL", Value = RegNumberExecPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumExecPFRYL", Value = DateRegNumExecPfryl };
            yield return new InputPropertyInfo { Name = "branchExecSSYL", Value = BranchExecSsyl };
            yield return new InputPropertyInfo { Name = "grnRegExecSSYL", Value = GrnRegExecSsyl };
            yield return new InputPropertyInfo { Name = "dateGrnExecSSYL", Value = DateGrnExecSsyl };
            yield return new InputPropertyInfo { Name = "typeCharterCapital", Value = TypeCharterCapital };
            yield return new InputPropertyInfo { Name = "sumCharterCapital", Value = SumCharterCapital };
            yield return new InputPropertyInfo { Name = "grnCharterCapital", Value = GrnCharterCapital };
            yield return new InputPropertyInfo { Name = "dateGRNCharterCapital", Value = DateGrnCharterCapital };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "KPP", Value = Kpp };
            yield return new InputPropertyInfo { Name = "ShortNameOrg", Value = ShortNameOrg };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlJournalsCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string JournalCreationDate { get; set; }
        public string JournalName { get; set; }
        public Guid? ControlFileId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalCreationDate", Value = JournalCreationDate };
            yield return new InputPropertyInfo { Name = "JournalName", Value = JournalName };
            yield return new InputPropertyInfo { Name = "ControlFileId", Value = ControlFileId };
        }
    }

    public partial class LicenceUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicenceName { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public object StartDate { get; set; }
        public object StopDate { get; set; }
        public object EndDate { get; set; }
        public string BlankNumber { get; set; }
        public string LicenseFinalDecision { get; set; }
        public object LicenseFinalStartDate { get; set; }
        public object LicenseFinalEndDate { get; set; }
        public Guid? LicenceStatusId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? ActivityTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceName", Value = LicenceName };
            yield return new InputPropertyInfo { Name = "Series", Value = Series };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "startDate", Value = StartDate };
            yield return new InputPropertyInfo { Name = "stopDate", Value = StopDate };
            yield return new InputPropertyInfo { Name = "endDate", Value = EndDate };
            yield return new InputPropertyInfo { Name = "BlankNumber", Value = BlankNumber };
            yield return new InputPropertyInfo { Name = "licenseFinalDecision", Value = LicenseFinalDecision };
            yield return new InputPropertyInfo { Name = "licenseFinalStartDate", Value = LicenseFinalStartDate };
            yield return new InputPropertyInfo { Name = "licenseFinalEndDate", Value = LicenseFinalEndDate };
            yield return new InputPropertyInfo { Name = "LicenceStatusId", Value = LicenceStatusId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "ActivityTypeId", Value = ActivityTypeId };
        }
    }

    public partial class dicLicenceStatusCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicenceStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceStatusName", Value = LicenceStatusName };
        }
    }

    public partial class SubjectUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MainName { get; set; }
        public string Address { get; set; }
        public string Citizenship { get; set; }
        public string RepresentativeSurname { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string NameOrg { get; set; }
        public Guid? SubjectTypesId { get; set; }
        public ICollection<Guid> ControlType { get; set; }
        public Guid? AssignedOfficerId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "mainName", Value = MainName };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "Citizenship", Value = Citizenship };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "NameOrg", Value = NameOrg };
            yield return new InputPropertyInfo { Name = "SubjectTypesId", Value = SubjectTypesId };
            yield return new InputPropertyInfo { Name = "ControlType", Value = ControlType };
            yield return new InputPropertyInfo { Name = "AssignedOfficerId", Value = AssignedOfficerId };
        }
    }

    public partial class PersonCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string PersonSecondName { get; set; }
        public ICollection<Guid> ControlItems { get; set; }
        public Guid? ControlOrganizationId { get; set; }
        public ICollection<Guid> ControlPlans { get; set; }
        public Guid? RoleNameId { get; set; }
        public ICollection<Guid> ControlCards { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PersonName", Value = PersonName };
            yield return new InputPropertyInfo { Name = "PersonSurName", Value = PersonSurName };
            yield return new InputPropertyInfo { Name = "PersonSecondName", Value = PersonSecondName };
            yield return new InputPropertyInfo { Name = "ControlItems", Value = ControlItems };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
            yield return new InputPropertyInfo { Name = "ControlPlans", Value = ControlPlans };
            yield return new InputPropertyInfo { Name = "RoleNameId", Value = RoleNameId };
            yield return new InputPropertyInfo { Name = "ControlCards", Value = ControlCards };
        }
    }

    public partial class ControlObjectCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlObjectName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlObjectName", Value = ControlObjectName };
        }
    }

    public partial class CitizenRequestFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ApplicantName { get; set; }
        public StringFilterOperator ApplicantSurname { get; set; }
        public StringFilterOperator SecondName { get; set; }
        public StringFilterOperator ApplicantEmail { get; set; }
        public StringFilterOperator ApplicantPhone { get; set; }
        public StringFilterOperator RequestContent { get; set; }
        public StringFilterOperator ViolationCondition { get; set; }
        public DateFilterOperator RequestGetDate { get; set; }
        public StringFilterOperator TargetControlOrganization { get; set; }
        public StringFilterOperator TargetControlOrganizationOgrn { get; set; }
        public ICollection<CitizenRequestFilter> Or { get; set; }
        public ICollection<CitizenRequestFilter> And { get; set; }
        public CitizenRequestFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ApplicantName", Value = ApplicantName };
            yield return new InputPropertyInfo { Name = "ApplicantSurname", Value = ApplicantSurname };
            yield return new InputPropertyInfo { Name = "SecondName", Value = SecondName };
            yield return new InputPropertyInfo { Name = "ApplicantEmail", Value = ApplicantEmail };
            yield return new InputPropertyInfo { Name = "ApplicantPhone", Value = ApplicantPhone };
            yield return new InputPropertyInfo { Name = "RequestContent", Value = RequestContent };
            yield return new InputPropertyInfo { Name = "ViolationCondition", Value = ViolationCondition };
            yield return new InputPropertyInfo { Name = "RequestGetDate", Value = RequestGetDate };
            yield return new InputPropertyInfo { Name = "TargetControlOrganization", Value = TargetControlOrganization };
            yield return new InputPropertyInfo { Name = "TargetControlOrganizationOGRN", Value = TargetControlOrganizationOgrn };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlItemResultUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public decimal? SumAdmFine { get; set; }
        public bool? SumAdmFineStatus { get; set; }
        public ICollection<Guid> DamageType { get; set; }
        public Guid? LinkedControlCardId { get; set; }
        public Guid? PunishmentTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "sumAdmFine", Value = SumAdmFine };
            yield return new InputPropertyInfo { Name = "SumAdmFineStatus", Value = SumAdmFineStatus };
            yield return new InputPropertyInfo { Name = "DamageType", Value = DamageType };
            yield return new InputPropertyInfo { Name = "LinkedControlCardId", Value = LinkedControlCardId };
            yield return new InputPropertyInfo { Name = "PunishmentTypeId", Value = PunishmentTypeId };
        }
    }

    public partial class dicLicenceStatusUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicenceStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceStatusName", Value = LicenceStatusName };
        }
    }

    public partial class dicOKSMCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string FullCountryName { get; set; }
        public string ShortCountryName { get; set; }
        public string LetterCodeAlpha2 { get; set; }
        public string LetterCodeAlpha3 { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Code", Value = Code };
            yield return new InputPropertyInfo { Name = "FullCountryName", Value = FullCountryName };
            yield return new InputPropertyInfo { Name = "ShortCountryName", Value = ShortCountryName };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha2", Value = LetterCodeAlpha2 };
            yield return new InputPropertyInfo { Name = "LetterCodeAlpha3", Value = LetterCodeAlpha3 };
        }
    }

    public partial class JournalAttributesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string JournalAttirbuteName { get; set; }
        public string JournalAttributeValue { get; set; }
        public Guid? ControlJournalsId { get; set; }
        public Guid? MeasureUnitTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalAttirbuteName", Value = JournalAttirbuteName };
            yield return new InputPropertyInfo { Name = "JournalAttributeValue", Value = JournalAttributeValue };
            yield return new InputPropertyInfo { Name = "ControlJournalsId", Value = ControlJournalsId };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeId", Value = MeasureUnitTypeId };
        }
    }

    public partial class MaterialsCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? ControlActFileId { get; set; }
        public Guid? ControlListId { get; set; }
        public Guid? ControlOrderFileId { get; set; }
        public Guid? LinkedFileId { get; set; }
        public Guid? DocRegulationId { get; set; }
        public Guid? DocControlPlanId { get; set; }
        public Guid? DocProcStatementId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "ControlActFileId", Value = ControlActFileId };
            yield return new InputPropertyInfo { Name = "ControlListId", Value = ControlListId };
            yield return new InputPropertyInfo { Name = "ControlOrderFileId", Value = ControlOrderFileId };
            yield return new InputPropertyInfo { Name = "LinkedFileId", Value = LinkedFileId };
            yield return new InputPropertyInfo { Name = "docRegulationId", Value = DocRegulationId };
            yield return new InputPropertyInfo { Name = "docControlPlanId", Value = DocControlPlanId };
            yield return new InputPropertyInfo { Name = "docProcStatementId", Value = DocProcStatementId };
        }
    }

    public partial class ExtendedAttribUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ExtAttributeName { get; set; }
        public string ExtAttributeContentUnit { get; set; }
        public string ExtAttributeTitle { get; set; }
        public Guid? ControlTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeName", Value = ExtAttributeName };
            yield return new InputPropertyInfo { Name = "ExtAttributeContentUnit", Value = ExtAttributeContentUnit };
            yield return new InputPropertyInfo { Name = "ExtAttributeTitle", Value = ExtAttributeTitle };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
        }
    }

    public partial class PersonAppointmentCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object AppStartDate { get; set; }
        public object AppEndDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "AppStartDate", Value = AppStartDate };
            yield return new InputPropertyInfo { Name = "AppEndDate", Value = AppEndDate };
        }
    }

    public partial class dicKNMTypesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string KnmTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMTypeName", Value = KnmTypeName };
        }
    }

    public partial class docRegulationFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator RegulationCreateDate { get; set; }
        public DateFilterOperator RegulationExecutionDate { get; set; }
        public StringFilterOperator Result { get; set; }
        public ICollection<docRegulationFilter> Or { get; set; }
        public ICollection<docRegulationFilter> And { get; set; }
        public docRegulationFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RegulationCreateDate", Value = RegulationCreateDate };
            yield return new InputPropertyInfo { Name = "RegulationExecutionDate", Value = RegulationExecutionDate };
            yield return new InputPropertyInfo { Name = "Result", Value = Result };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicNPALevelsFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator NpaLevelsName { get; set; }
        public ICollection<dicNPALevelsFilter> Or { get; set; }
        public ICollection<dicNPALevelsFilter> And { get; set; }
        public dicNPALevelsFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPALevelsName", Value = NpaLevelsName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class SubjectCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string MainName { get; set; }
        public string Address { get; set; }
        public string Citizenship { get; set; }
        public string RepresentativeSurname { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string NameOrg { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "mainName", Value = MainName };
            yield return new InputPropertyInfo { Name = "Address", Value = Address };
            yield return new InputPropertyInfo { Name = "Citizenship", Value = Citizenship };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "NameOrg", Value = NameOrg };
        }
    }

    public partial class JournalAttributesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string JournalAttirbuteName { get; set; }
        public string JournalAttributeValue { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalAttirbuteName", Value = JournalAttirbuteName };
            yield return new InputPropertyInfo { Name = "JournalAttributeValue", Value = JournalAttributeValue };
        }
    }

    public partial class ControlItemResultCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public decimal? SumAdmFine { get; set; }
        public bool? SumAdmFineStatus { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "sumAdmFine", Value = SumAdmFine };
            yield return new InputPropertyInfo { Name = "SumAdmFineStatus", Value = SumAdmFineStatus };
        }
    }

    public partial class dicControlFileStatusFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlFileStatusName { get; set; }
        public ICollection<dicControlFileStatusFilter> Or { get; set; }
        public ICollection<dicControlFileStatusFilter> And { get; set; }
        public dicControlFileStatusFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileStatusName", Value = ControlFileStatusName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicNPATypesCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPATypeName", Value = NpaTypeName };
        }
    }

    public partial class dicControlFileStatusUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlFileStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileStatusName", Value = ControlFileStatusName };
        }
    }

    public partial class docControlActFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator ControlActCreateDate { get; set; }
        public ICollection<docControlActFilter> Or { get; set; }
        public ICollection<docControlActFilter> And { get; set; }
        public docControlActFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlActCreateDate", Value = ControlActCreateDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docControlOrderCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlOrderCreateDate { get; set; }
        public object ControlOrderSignDate { get; set; }
        public string ControlOrderNumber { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlOrderCreateDate", Value = ControlOrderCreateDate };
            yield return new InputPropertyInfo { Name = "ControlOrderSignDate", Value = ControlOrderSignDate };
            yield return new InputPropertyInfo { Name = "ControlOrderNumber", Value = ControlOrderNumber };
        }
    }

    public partial class dicControlListStatusFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlListStatusName { get; set; }
        public ICollection<dicControlListStatusFilter> Or { get; set; }
        public ICollection<dicControlListStatusFilter> And { get; set; }
        public dicControlListStatusFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStatusName", Value = ControlListStatusName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicControlFileStatusCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlFileStatusName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileStatusName", Value = ControlFileStatusName };
        }
    }

    public partial class RiskCatCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object RiskCatStartDate { get; set; }
        public object RiskCatEndDate { get; set; }
        public Guid? ControlObjectId { get; set; }
        public Guid? RiskCategoryId { get; set; }
        public Guid? SubjectId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RiskCatStartDate", Value = RiskCatStartDate };
            yield return new InputPropertyInfo { Name = "RiskCatEndDate", Value = RiskCatEndDate };
            yield return new InputPropertyInfo { Name = "ControlObjectId", Value = ControlObjectId };
            yield return new InputPropertyInfo { Name = "RiskCategoryId", Value = RiskCategoryId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
        }
    }

    public partial class dicOKOPFCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string SubjectName { get; set; }
        public string CodeOkopf { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectName", Value = SubjectName };
            yield return new InputPropertyInfo { Name = "CodeOKOPF", Value = CodeOkopf };
        }
    }

    public partial class ControlItemPassportUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object CreateDate { get; set; }
        public Guid? ControlCardId { get; set; }
        public ICollection<Guid> ControlObjects { get; set; }
        public Guid? ControlTypeId { get; set; }
        public Guid? ControlOrganizationId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? StatusKnmNameId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CreateDate", Value = CreateDate };
            yield return new InputPropertyInfo { Name = "ControlCardId", Value = ControlCardId };
            yield return new InputPropertyInfo { Name = "ControlObjects", Value = ControlObjects };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "StatusKNMNameId", Value = StatusKnmNameId };
        }
    }

    public partial class dicNPALevelsCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaLevelsName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPALevelsName", Value = NpaLevelsName };
        }
    }

    public partial class NPAUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaName { get; set; }
        public object ApproveDate { get; set; }
        public object NpaEndDate { get; set; }
        public string Number { get; set; }
        public string Body { get; set; }
        public ICollection<Guid> MandatoryReqs { get; set; }
        public Guid? NpaTypeId { get; set; }
        public Guid? NpaLevelId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPAName", Value = NpaName };
            yield return new InputPropertyInfo { Name = "ApproveDate", Value = ApproveDate };
            yield return new InputPropertyInfo { Name = "NPAEndDate", Value = NpaEndDate };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "Body", Value = Body };
            yield return new InputPropertyInfo { Name = "MandatoryReqs", Value = MandatoryReqs };
            yield return new InputPropertyInfo { Name = "NPATypeId", Value = NpaTypeId };
            yield return new InputPropertyInfo { Name = "NPALevelId", Value = NpaLevelId };
        }
    }

    public partial class CheckOutEGRULCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public object DateUpdateEgrul { get; set; }
        public string Ogrnul { get; set; }
        public object OgrnuLdata { get; set; }
        public string Innul { get; set; }
        public object DateRegYl { get; set; }
        public string OrgNameRegYl { get; set; }
        public string AddressRegOrgYl { get; set; }
        public string GrnOrgLocationYl { get; set; }
        public object GrnDateOrgLocationYl { get; set; }
        public object DateTaxReg { get; set; }
        public string TaxOrg { get; set; }
        public string GrnTaxOrg { get; set; }
        public object DateGrnTaxOrg { get; set; }
        public string RegNumberPfryl { get; set; }
        public object DateRegNumPfryl { get; set; }
        public string BranchPfryl { get; set; }
        public string GrnRegPfryl { get; set; }
        public object DateGrnRegPfryl { get; set; }
        public string GrnRegExecPfryl { get; set; }
        public string RegNumberExecPfryl { get; set; }
        public object DateRegNumExecPfryl { get; set; }
        public string BranchExecSsyl { get; set; }
        public string GrnRegExecSsyl { get; set; }
        public object DateGrnExecSsyl { get; set; }
        public string TypeCharterCapital { get; set; }
        public string SumCharterCapital { get; set; }
        public string GrnCharterCapital { get; set; }
        public object DateGrnCharterCapital { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string RepresentativeSurname { get; set; }
        public string Kpp { get; set; }
        public string ShortNameOrg { get; set; }
        public Guid? SubjectEgrulId { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? CodeOkopfId { get; set; }
        public Guid? CitizenshipsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "dateUpdateEGRUL", Value = DateUpdateEgrul };
            yield return new InputPropertyInfo { Name = "OGRNUL", Value = Ogrnul };
            yield return new InputPropertyInfo { Name = "OGRNULdata", Value = OgrnuLdata };
            yield return new InputPropertyInfo { Name = "INNUL", Value = Innul };
            yield return new InputPropertyInfo { Name = "dateRegYL", Value = DateRegYl };
            yield return new InputPropertyInfo { Name = "orgNameRegYL", Value = OrgNameRegYl };
            yield return new InputPropertyInfo { Name = "addressRegOrgYL", Value = AddressRegOrgYl };
            yield return new InputPropertyInfo { Name = "grnOrgLocationYL", Value = GrnOrgLocationYl };
            yield return new InputPropertyInfo { Name = "grnDateOrgLocationYL", Value = GrnDateOrgLocationYl };
            yield return new InputPropertyInfo { Name = "dateTaxReg", Value = DateTaxReg };
            yield return new InputPropertyInfo { Name = "taxOrg", Value = TaxOrg };
            yield return new InputPropertyInfo { Name = "grnTaxOrg", Value = GrnTaxOrg };
            yield return new InputPropertyInfo { Name = "dateGRNTaxOrg", Value = DateGrnTaxOrg };
            yield return new InputPropertyInfo { Name = "RegNumberPFRYL", Value = RegNumberPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumPFRYL", Value = DateRegNumPfryl };
            yield return new InputPropertyInfo { Name = "branchPFRYL", Value = BranchPfryl };
            yield return new InputPropertyInfo { Name = "GrnRegPFRYL", Value = GrnRegPfryl };
            yield return new InputPropertyInfo { Name = "dateGrnRegPFRYL", Value = DateGrnRegPfryl };
            yield return new InputPropertyInfo { Name = "grnRegExecPFRYL", Value = GrnRegExecPfryl };
            yield return new InputPropertyInfo { Name = "regNumberExecPFRYL", Value = RegNumberExecPfryl };
            yield return new InputPropertyInfo { Name = "dateRegNumExecPFRYL", Value = DateRegNumExecPfryl };
            yield return new InputPropertyInfo { Name = "branchExecSSYL", Value = BranchExecSsyl };
            yield return new InputPropertyInfo { Name = "grnRegExecSSYL", Value = GrnRegExecSsyl };
            yield return new InputPropertyInfo { Name = "dateGrnExecSSYL", Value = DateGrnExecSsyl };
            yield return new InputPropertyInfo { Name = "typeCharterCapital", Value = TypeCharterCapital };
            yield return new InputPropertyInfo { Name = "sumCharterCapital", Value = SumCharterCapital };
            yield return new InputPropertyInfo { Name = "grnCharterCapital", Value = GrnCharterCapital };
            yield return new InputPropertyInfo { Name = "dateGRNCharterCapital", Value = DateGrnCharterCapital };
            yield return new InputPropertyInfo { Name = "RepresentativeName", Value = RepresentativeName };
            yield return new InputPropertyInfo { Name = "RepresentativeSecondName", Value = RepresentativeSecondName };
            yield return new InputPropertyInfo { Name = "RepresentativeSurname", Value = RepresentativeSurname };
            yield return new InputPropertyInfo { Name = "KPP", Value = Kpp };
            yield return new InputPropertyInfo { Name = "ShortNameOrg", Value = ShortNameOrg };
            yield return new InputPropertyInfo { Name = "SubjectEGRULId", Value = SubjectEgrulId };
            yield return new InputPropertyInfo { Name = "AddressId", Value = AddressId };
            yield return new InputPropertyInfo { Name = "CodeOKOPFId", Value = CodeOkopfId };
            yield return new InputPropertyInfo { Name = "CitizenshipsId", Value = CitizenshipsId };
        }
    }

    public partial class docControlListCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlListStartDate { get; set; }
        public object ControlListEndDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlListStartDate", Value = ControlListStartDate };
            yield return new InputPropertyInfo { Name = "ControlListEndDate", Value = ControlListEndDate };
        }
    }

    public partial class PersonUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string PersonSecondName { get; set; }
        public ICollection<Guid> ControlItems { get; set; }
        public Guid? ControlOrganizationId { get; set; }
        public ICollection<Guid> ControlPlans { get; set; }
        public Guid? RoleNameId { get; set; }
        public ICollection<Guid> ControlCards { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PersonName", Value = PersonName };
            yield return new InputPropertyInfo { Name = "PersonSurName", Value = PersonSurName };
            yield return new InputPropertyInfo { Name = "PersonSecondName", Value = PersonSecondName };
            yield return new InputPropertyInfo { Name = "ControlItems", Value = ControlItems };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
            yield return new InputPropertyInfo { Name = "ControlPlans", Value = ControlPlans };
            yield return new InputPropertyInfo { Name = "RoleNameId", Value = RoleNameId };
            yield return new InputPropertyInfo { Name = "ControlCards", Value = ControlCards };
        }
    }

    public partial class docRegulationCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object RegulationCreateDate { get; set; }
        public object RegulationExecutionDate { get; set; }
        public string Result { get; set; }
        public Guid? ControlCardId { get; set; }
        public Guid? ProcStatmentStatusId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RegulationCreateDate", Value = RegulationCreateDate };
            yield return new InputPropertyInfo { Name = "RegulationExecutionDate", Value = RegulationExecutionDate };
            yield return new InputPropertyInfo { Name = "Result", Value = Result };
            yield return new InputPropertyInfo { Name = "ControlCardId", Value = ControlCardId };
            yield return new InputPropertyInfo { Name = "ProcStatmentStatusId", Value = ProcStatmentStatusId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class LicenceCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string LicenceName { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public object StartDate { get; set; }
        public object StopDate { get; set; }
        public object EndDate { get; set; }
        public string BlankNumber { get; set; }
        public string LicenseFinalDecision { get; set; }
        public object LicenseFinalStartDate { get; set; }
        public object LicenseFinalEndDate { get; set; }
        public Guid? LicenceStatusId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? ActivityTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "LicenceName", Value = LicenceName };
            yield return new InputPropertyInfo { Name = "Series", Value = Series };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "startDate", Value = StartDate };
            yield return new InputPropertyInfo { Name = "stopDate", Value = StopDate };
            yield return new InputPropertyInfo { Name = "endDate", Value = EndDate };
            yield return new InputPropertyInfo { Name = "BlankNumber", Value = BlankNumber };
            yield return new InputPropertyInfo { Name = "licenseFinalDecision", Value = LicenseFinalDecision };
            yield return new InputPropertyInfo { Name = "licenseFinalStartDate", Value = LicenseFinalStartDate };
            yield return new InputPropertyInfo { Name = "licenseFinalEndDate", Value = LicenseFinalEndDate };
            yield return new InputPropertyInfo { Name = "LicenceStatusId", Value = LicenceStatusId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "ActivityTypeId", Value = ActivityTypeId };
        }
    }

    public partial class DicStatesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string State { get; set; }
        public string Step { get; set; }
        public string Infotxt { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "state", Value = State };
            yield return new InputPropertyInfo { Name = "step", Value = Step };
            yield return new InputPropertyInfo { Name = "infotxt", Value = Infotxt };
        }
    }

    public partial class dicKNMFormUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string KnmFormName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMFormName", Value = KnmFormName };
        }
    }

    public partial class ControlListQuestionsCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string QuestionContent { get; set; }
        public string OuidBmQuestionInspection { get; set; }
        public string OuidBmInspectionListResult { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionContent", Value = QuestionContent };
            yield return new InputPropertyInfo { Name = "ouid_bmQuestionInspection", Value = OuidBmQuestionInspection };
            yield return new InputPropertyInfo { Name = "ouid_bmInspectionListResult", Value = OuidBmInspectionListResult };
        }
    }

    public partial class ExtendedAttribValueUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ExtAttributeContent { get; set; }
        public Guid? ControlObjectId { get; set; }
        public Guid? ExtendedAttribId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeContent", Value = ExtAttributeContent };
            yield return new InputPropertyInfo { Name = "ControlObjectId", Value = ControlObjectId };
            yield return new InputPropertyInfo { Name = "ExtendedAttribId", Value = ExtendedAttribId };
        }
    }

    public partial class ControlObjectCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlObjectName { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? CodeOktmoId { get; set; }
        public Guid? ControlTypeId { get; set; }
        public ICollection<Guid> ControlItemPassport { get; set; }
        public Guid? ControlFileId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlObjectName", Value = ControlObjectName };
            yield return new InputPropertyInfo { Name = "AddressId", Value = AddressId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "CodeOKTMOId", Value = CodeOktmoId };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
            yield return new InputPropertyInfo { Name = "ControlItemPassport", Value = ControlItemPassport };
            yield return new InputPropertyInfo { Name = "ControlFileId", Value = ControlFileId };
        }
    }

    public partial class ControlFileCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlFileNumber { get; set; }
        public object ControlFileStartDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlFileNumber", Value = ControlFileNumber };
            yield return new InputPropertyInfo { Name = "ControlFileStartDate", Value = ControlFileStartDate };
        }
    }

    public partial class dicPunishmentTypeCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string PunishmentType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PunishmentType", Value = PunishmentType };
        }
    }

    public partial class FileUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "FileName", Value = FileName };
            yield return new InputPropertyInfo { Name = "FileType", Value = FileType };
        }
    }

    public partial class ControlListQuestionsFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator QuestionContent { get; set; }
        public StringFilterOperator OuidBmQuestionInspection { get; set; }
        public StringFilterOperator OuidBmInspectionListResult { get; set; }
        public ICollection<ControlListQuestionsFilter> Or { get; set; }
        public ICollection<ControlListQuestionsFilter> And { get; set; }
        public ControlListQuestionsFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "QuestionContent", Value = QuestionContent };
            yield return new InputPropertyInfo { Name = "ouid_bmQuestionInspection", Value = OuidBmQuestionInspection };
            yield return new InputPropertyInfo { Name = "ouid_bmInspectionListResult", Value = OuidBmInspectionListResult };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicNPATypesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator NpaTypeName { get; set; }
        public ICollection<dicNPATypesFilter> Or { get; set; }
        public ICollection<dicNPATypesFilter> And { get; set; }
        public dicNPATypesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPATypeName", Value = NpaTypeName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docProcStatementUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object SendToProcDate { get; set; }
        public string ProcApproveFio { get; set; }
        public string ProcApproveRole { get; set; }
        public string ProcApprovePlace { get; set; }
        public Guid? LinkedControlCardId { get; set; }
        public Guid? MaterialsId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SendToProcDate", Value = SendToProcDate };
            yield return new InputPropertyInfo { Name = "ProcApproveFIO", Value = ProcApproveFio };
            yield return new InputPropertyInfo { Name = "ProcApproveRole", Value = ProcApproveRole };
            yield return new InputPropertyInfo { Name = "ProcApprovePlace", Value = ProcApprovePlace };
            yield return new InputPropertyInfo { Name = "LinkedControlCardId", Value = LinkedControlCardId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
        }
    }

    public partial class IntFilterOperator : IGraphQlInputObject
    {
        public int? EqualTo { get; set; }
        public int? NotEqualTo { get; set; }
        public int? GreaterThan { get; set; }
        public int? GreaterThanOrEqual { get; set; }
        public int? LessThan { get; set; }
        public int? LessThanOrEqual { get; set; }
        public ICollection<int> In { get; set; }
        public ICollection<int> NotIn { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "equalTo", Value = EqualTo };
            yield return new InputPropertyInfo { Name = "notEqualTo", Value = NotEqualTo };
            yield return new InputPropertyInfo { Name = "greaterThan", Value = GreaterThan };
            yield return new InputPropertyInfo { Name = "greaterThanOrEqual", Value = GreaterThanOrEqual };
            yield return new InputPropertyInfo { Name = "lessThan", Value = LessThan };
            yield return new InputPropertyInfo { Name = "lessThanOrEqual", Value = LessThanOrEqual };
            yield return new InputPropertyInfo { Name = "in", Value = In };
            yield return new InputPropertyInfo { Name = "notIn", Value = NotIn };
        }
    }

    public partial class HazardClassCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object HazardClassStartDate { get; set; }
        public object HazardClassEndDate { get; set; }
        public Guid? ControlObjectId { get; set; }
        public Guid? HazardClassId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassStartDate", Value = HazardClassStartDate };
            yield return new InputPropertyInfo { Name = "HazardClassEndDate", Value = HazardClassEndDate };
            yield return new InputPropertyInfo { Name = "ControlObjectId", Value = ControlObjectId };
            yield return new InputPropertyInfo { Name = "HazardClassId", Value = HazardClassId };
        }
    }

    public partial class dicPunishmentTypeCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string PunishmentType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PunishmentType", Value = PunishmentType };
        }
    }

    public partial class dicRoleCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string RoleName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RoleName", Value = RoleName };
        }
    }

    public partial class CheckOutEGRSMSPFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator DateRmsp { get; set; }
        public StringFilterOperator CaseNumber { get; set; }
        public BooleanFilterOperator IsInRmsp { get; set; }
        public DateFilterOperator LastRenewData { get; set; }
        public ICollection<CheckOutEGRSMSPFilter> Or { get; set; }
        public ICollection<CheckOutEGRSMSPFilter> And { get; set; }
        public CheckOutEGRSMSPFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "dateRMSP", Value = DateRmsp };
            yield return new InputPropertyInfo { Name = "caseNumber", Value = CaseNumber };
            yield return new InputPropertyInfo { Name = "IsInRMSP", Value = IsInRmsp };
            yield return new InputPropertyInfo { Name = "LastRenewData", Value = LastRenewData };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicControlItemBaseTypeCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlItemBaseName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemBaseName", Value = ControlItemBaseName };
        }
    }

    public partial class dicRiskCategoryFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator RiskCategoryName { get; set; }
        public ICollection<dicRiskCategoryFilter> Or { get; set; }
        public ICollection<dicRiskCategoryFilter> And { get; set; }
        public dicRiskCategoryFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RiskCategoryName", Value = RiskCategoryName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicStatesFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator State { get; set; }
        public StringFilterOperator Step { get; set; }
        public StringFilterOperator Infotxt { get; set; }
        public ICollection<DicStatesFilter> Or { get; set; }
        public ICollection<DicStatesFilter> And { get; set; }
        public DicStatesFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "state", Value = State };
            yield return new InputPropertyInfo { Name = "step", Value = Step };
            yield return new InputPropertyInfo { Name = "infotxt", Value = Infotxt };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicStatusKNMUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string StatusKnmName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusKNMName", Value = StatusKnmName };
        }
    }

    public partial class PersonAppointmentFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator AppStartDate { get; set; }
        public DateFilterOperator AppEndDate { get; set; }
        public ICollection<PersonAppointmentFilter> Or { get; set; }
        public ICollection<PersonAppointmentFilter> And { get; set; }
        public PersonAppointmentFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "AppStartDate", Value = AppStartDate };
            yield return new InputPropertyInfo { Name = "AppEndDate", Value = AppEndDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlItemPassportCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object CreateDate { get; set; }
        public bool? IsInPlanYear { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CreateDate", Value = CreateDate };
            yield return new InputPropertyInfo { Name = "IsInPlanYear", Value = IsInPlanYear };
        }
    }

    public partial class dicOKVEDUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ActivityType { get; set; }
        public string Decipher { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityType", Value = ActivityType };
            yield return new InputPropertyInfo { Name = "Decipher", Value = Decipher };
        }
    }

    public partial class NPACondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaName { get; set; }
        public object ApproveDate { get; set; }
        public string ApproveEntity { get; set; }
        public object NpaEndDate { get; set; }
        public string Number { get; set; }
        public string Body { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPAName", Value = NpaName };
            yield return new InputPropertyInfo { Name = "ApproveDate", Value = ApproveDate };
            yield return new InputPropertyInfo { Name = "ApproveEntity", Value = ApproveEntity };
            yield return new InputPropertyInfo { Name = "NPAEndDate", Value = NpaEndDate };
            yield return new InputPropertyInfo { Name = "Number", Value = Number };
            yield return new InputPropertyInfo { Name = "Body", Value = Body };
        }
    }

    public partial class dicDamageTypeFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator DamageType { get; set; }
        public ICollection<dicDamageTypeFilter> Or { get; set; }
        public ICollection<dicDamageTypeFilter> And { get; set; }
        public dicDamageTypeFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "DamageType", Value = DamageType };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlProgramUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProgramName { get; set; }
        public Guid? ControlFileId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProgramName", Value = ProgramName };
            yield return new InputPropertyInfo { Name = "ControlFileId", Value = ControlFileId };
        }
    }

    public partial class HazardClassCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string HazardClassLevel { get; set; }
        public object HazardClassStartDate { get; set; }
        public object HazardClassEndDate { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassLevel", Value = HazardClassLevel };
            yield return new InputPropertyInfo { Name = "HazardClassStartDate", Value = HazardClassStartDate };
            yield return new InputPropertyInfo { Name = "HazardClassEndDate", Value = HazardClassEndDate };
        }
    }

    public partial class ControlProgramFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ProgramName { get; set; }
        public ICollection<ControlProgramFilter> Or { get; set; }
        public ICollection<ControlProgramFilter> And { get; set; }
        public ControlProgramFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProgramName", Value = ProgramName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class RiskCatUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object RiskCatStartDate { get; set; }
        public object RiskCatEndDate { get; set; }
        public Guid? ControlObjectId { get; set; }
        public Guid? RiskCategoryId { get; set; }
        public Guid? SubjectId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RiskCatStartDate", Value = RiskCatStartDate };
            yield return new InputPropertyInfo { Name = "RiskCatEndDate", Value = RiskCatEndDate };
            yield return new InputPropertyInfo { Name = "ControlObjectId", Value = ControlObjectId };
            yield return new InputPropertyInfo { Name = "RiskCategoryId", Value = RiskCategoryId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
        }
    }

    public partial class DicStatesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string State { get; set; }
        public string Step { get; set; }
        public string Infotxt { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "state", Value = State };
            yield return new InputPropertyInfo { Name = "step", Value = Step };
            yield return new InputPropertyInfo { Name = "infotxt", Value = Infotxt };
        }
    }

    public partial class dicKNMTypesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string KnmTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMTypeName", Value = KnmTypeName };
        }
    }

    public partial class dicProsecCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProsecName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProsecName", Value = ProsecName };
        }
    }

    public partial class dicStatusPlanCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string StatusPlanName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusPlanName", Value = StatusPlanName };
        }
    }

    public partial class FileFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator HashMd5 { get; set; }
        public StringFilterOperator FileName { get; set; }
        public StringFilterOperator FileType { get; set; }
        public StringFilterOperator FileSize { get; set; }
        public DateTimeFilterOperator TimeStamp { get; set; }
        public StringFilterOperator DownloadLink { get; set; }
        public ICollection<FileFilter> Or { get; set; }
        public ICollection<FileFilter> And { get; set; }
        public FileFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HashMD5", Value = HashMd5 };
            yield return new InputPropertyInfo { Name = "FileName", Value = FileName };
            yield return new InputPropertyInfo { Name = "FileType", Value = FileType };
            yield return new InputPropertyInfo { Name = "FileSize", Value = FileSize };
            yield return new InputPropertyInfo { Name = "TimeStamp", Value = TimeStamp };
            yield return new InputPropertyInfo { Name = "DownloadLink", Value = DownloadLink };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicControlTypesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlTypeName { get; set; }
        public ICollection<Guid> Subjects { get; set; }
        public ICollection<Guid> ControlOrganizations { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlTypeName", Value = ControlTypeName };
            yield return new InputPropertyInfo { Name = "Subjects", Value = Subjects };
            yield return new InputPropertyInfo { Name = "ControlOrganizations", Value = ControlOrganizations };
        }
    }

    public partial class PersonFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator PersonName { get; set; }
        public StringFilterOperator PersonSurName { get; set; }
        public StringFilterOperator PersonSecondName { get; set; }
        public ICollection<PersonFilter> Or { get; set; }
        public ICollection<PersonFilter> And { get; set; }
        public PersonFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "PersonName", Value = PersonName };
            yield return new InputPropertyInfo { Name = "PersonSurName", Value = PersonSurName };
            yield return new InputPropertyInfo { Name = "PersonSecondName", Value = PersonSecondName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicHazardClassFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator HazardClassName { get; set; }
        public ICollection<dicHazardClassFilter> Or { get; set; }
        public ICollection<dicHazardClassFilter> And { get; set; }
        public dicHazardClassFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassName", Value = HazardClassName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class ControlProgramCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProgramName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProgramName", Value = ProgramName };
        }
    }

    public partial class dicKNMFormCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string KnmFormName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMFormName", Value = KnmFormName };
        }
    }

    public partial class dicRiskCategoryUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string RiskCategoryName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RiskCategoryName", Value = RiskCategoryName };
        }
    }

    public partial class RandEParameterValueUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public Guid? RandEParameterId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ParameterName", Value = ParameterName };
            yield return new InputPropertyInfo { Name = "ParameterValue", Value = ParameterValue };
            yield return new InputPropertyInfo { Name = "RandEParameterId", Value = RandEParameterId };
        }
    }

    public partial class RiskCatFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator CategoryClassLevel { get; set; }
        public DateFilterOperator RiskCatStartDate { get; set; }
        public DateFilterOperator RiskCatEndDate { get; set; }
        public ICollection<RiskCatFilter> Or { get; set; }
        public ICollection<RiskCatFilter> And { get; set; }
        public RiskCatFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CategoryClassLevel", Value = CategoryClassLevel };
            yield return new InputPropertyInfo { Name = "RiskCatStartDate", Value = RiskCatStartDate };
            yield return new InputPropertyInfo { Name = "RiskCatEndDate", Value = RiskCatEndDate };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicControlReasonDenyUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlReasonDeny { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlReasonDeny", Value = ControlReasonDeny };
        }
    }

    public partial class dicNPATypesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string NpaTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "NPATypeName", Value = NpaTypeName };
        }
    }

    public partial class DicControlItemBaseTypeUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlItemBaseName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemBaseName", Value = ControlItemBaseName };
        }
    }

    public partial class OrganizationUnitCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string OrganizationUnitName { get; set; }
        public Guid? ControlOrganizationId { get; set; }
        public Guid? MainOrganizationUnitId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "OrganizationUnitName", Value = OrganizationUnitName };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
            yield return new InputPropertyInfo { Name = "MainOrganizationUnitId", Value = MainOrganizationUnitId };
        }
    }

    public partial class DicControlPlanTypeFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlPlanType { get; set; }
        public ICollection<DicControlPlanTypeFilter> Or { get; set; }
        public ICollection<DicControlPlanTypeFilter> And { get; set; }
        public DicControlPlanTypeFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlPlanType", Value = ControlPlanType };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class RandEParameterUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public ICollection<Guid> ControlOrganizationParameter { get; set; }
        public Guid? MeasureUnitTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Name", Value = Name };
            yield return new InputPropertyInfo { Name = "ControlOrganizationParameter", Value = ControlOrganizationParameter };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeId", Value = MeasureUnitTypeId };
        }
    }

    public partial class dicStatusPlanFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator StatusPlanName { get; set; }
        public ICollection<dicStatusPlanFilter> Or { get; set; }
        public ICollection<dicStatusPlanFilter> And { get; set; }
        public dicStatusPlanFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "StatusPlanName", Value = StatusPlanName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicSubjectTypeFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator SubjectTypeName { get; set; }
        public ICollection<dicSubjectTypeFilter> Or { get; set; }
        public ICollection<dicSubjectTypeFilter> And { get; set; }
        public dicSubjectTypeFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "SubjectTypeName", Value = SubjectTypeName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class FileCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string HashMd5 { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public object TimeStamp { get; set; }
        public string DownloadLink { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HashMD5", Value = HashMd5 };
            yield return new InputPropertyInfo { Name = "FileName", Value = FileName };
            yield return new InputPropertyInfo { Name = "FileType", Value = FileType };
            yield return new InputPropertyInfo { Name = "FileSize", Value = FileSize };
            yield return new InputPropertyInfo { Name = "TimeStamp", Value = TimeStamp };
            yield return new InputPropertyInfo { Name = "DownloadLink", Value = DownloadLink };
        }
    }

    public partial class DicControlPlanTypeUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlPlanType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlPlanType", Value = ControlPlanType };
        }
    }

    public partial class dicControlReasonDenyCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlReasonDeny { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlReasonDeny", Value = ControlReasonDeny };
        }
    }

    public partial class DicControlItemBaseTypeFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlItemBaseName { get; set; }
        public ICollection<DicControlItemBaseTypeFilter> Or { get; set; }
        public ICollection<DicControlItemBaseTypeFilter> And { get; set; }
        public DicControlItemBaseTypeFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemBaseName", Value = ControlItemBaseName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicOKTMOFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator CodeOktmo { get; set; }
        public ICollection<dicOKTMOFilter> Or { get; set; }
        public ICollection<dicOKTMOFilter> And { get; set; }
        public dicOKTMOFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CodeOKTMO", Value = CodeOktmo };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicMeasureUnitsTypeFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator MeasureUnitTypeName { get; set; }
        public ICollection<DicMeasureUnitsTypeFilter> Or { get; set; }
        public ICollection<DicMeasureUnitsTypeFilter> And { get; set; }
        public DicMeasureUnitsTypeFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeName", Value = MeasureUnitTypeName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class dicHazardClassCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string HazardClassName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HazardClassName", Value = HazardClassName };
        }
    }

    public partial class dicProcStatmentStatusFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ProcStatementStatusName { get; set; }
        public ICollection<dicProcStatmentStatusFilter> Or { get; set; }
        public ICollection<dicProcStatmentStatusFilter> And { get; set; }
        public dicProcStatmentStatusFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProcStatementStatusName", Value = ProcStatementStatusName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class JournalAttributesUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string JournalAttirbuteName { get; set; }
        public string JournalAttributeValue { get; set; }
        public Guid? ControlJournalsId { get; set; }
        public Guid? MeasureUnitTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "JournalAttirbuteName", Value = JournalAttirbuteName };
            yield return new InputPropertyInfo { Name = "JournalAttributeValue", Value = JournalAttributeValue };
            yield return new InputPropertyInfo { Name = "ControlJournalsId", Value = ControlJournalsId };
            yield return new InputPropertyInfo { Name = "MeasureUnitTypeId", Value = MeasureUnitTypeId };
        }
    }

    public partial class ExtendedAttribCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ExtAttributeName { get; set; }
        public string ExtAttributeContentUnit { get; set; }
        public string ExtAttributeTitle { get; set; }
        public Guid? ControlTypeId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeName", Value = ExtAttributeName };
            yield return new InputPropertyInfo { Name = "ExtAttributeContentUnit", Value = ExtAttributeContentUnit };
            yield return new InputPropertyInfo { Name = "ExtAttributeTitle", Value = ExtAttributeTitle };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
        }
    }

    public partial class docControlPlanFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public DateFilterOperator CreateDate { get; set; }
        public StringFilterOperator ControlPlanName { get; set; }
        public DateFilterOperator ControlPlanApproveData { get; set; }
        public StringFilterOperator FgiserpGlobalPlanGuid { get; set; }
        public StringFilterOperator ControlPlanYear { get; set; }
        public ICollection<docControlPlanFilter> Or { get; set; }
        public ICollection<docControlPlanFilter> And { get; set; }
        public docControlPlanFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "CreateDate", Value = CreateDate };
            yield return new InputPropertyInfo { Name = "ControlPlanName", Value = ControlPlanName };
            yield return new InputPropertyInfo { Name = "ControlPlanApproveData", Value = ControlPlanApproveData };
            yield return new InputPropertyInfo { Name = "FGISERPGlobalPlanGUID", Value = FgiserpGlobalPlanGuid };
            yield return new InputPropertyInfo { Name = "ControlPlanYear", Value = ControlPlanYear };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class DicControlItemBaseTypeCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlItemBaseName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlItemBaseName", Value = ControlItemBaseName };
        }
    }

    public partial class docControlActUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object ControlActCreateDate { get; set; }
        public Guid? ActLinkedtoControlCardId { get; set; }
        public Guid? MaterialsId { get; set; }
        public Guid? ControlActCreatePlaceId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlActCreateDate", Value = ControlActCreateDate };
            yield return new InputPropertyInfo { Name = "ActLinkedtoControlCardId", Value = ActLinkedtoControlCardId };
            yield return new InputPropertyInfo { Name = "MaterialsId", Value = MaterialsId };
            yield return new InputPropertyInfo { Name = "ControlActCreatePlaceId", Value = ControlActCreatePlaceId };
        }
    }

    public partial class dicKNMTypesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string KnmTypeName { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "KNMTypeName", Value = KnmTypeName };
        }
    }

    public partial class ControlObjectFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator ControlObjectName { get; set; }
        public ICollection<ControlObjectFilter> Or { get; set; }
        public ICollection<ControlObjectFilter> And { get; set; }
        public ControlObjectFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlObjectName", Value = ControlObjectName };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class docRegulationCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public object RegulationCreateDate { get; set; }
        public object RegulationExecutionDate { get; set; }
        public string Result { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RegulationCreateDate", Value = RegulationCreateDate };
            yield return new InputPropertyInfo { Name = "RegulationExecutionDate", Value = RegulationExecutionDate };
            yield return new InputPropertyInfo { Name = "Result", Value = Result };
        }
    }

    public partial class dicControlReasonDenyCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlReasonDeny { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlReasonDeny", Value = ControlReasonDeny };
        }
    }

    public partial class dicRoleCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string RoleName { get; set; }
        public Guid? ControlOrganizationId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "RoleName", Value = RoleName };
            yield return new InputPropertyInfo { Name = "ControlOrganizationId", Value = ControlOrganizationId };
        }
    }

    public partial class ControlProgramCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ProgramName { get; set; }
        public Guid? ControlFileId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ProgramName", Value = ProgramName };
            yield return new InputPropertyInfo { Name = "ControlFileId", Value = ControlFileId };
        }
    }

    public partial class dicControlTypesCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlTypeName { get; set; }
        public ICollection<Guid> Subjects { get; set; }
        public ICollection<Guid> ControlOrganizations { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlTypeName", Value = ControlTypeName };
            yield return new InputPropertyInfo { Name = "Subjects", Value = Subjects };
            yield return new InputPropertyInfo { Name = "ControlOrganizations", Value = ControlOrganizations };
        }
    }

    public partial class RandEParameterFilter : IGraphQlInputObject
    {
        public IDFilterOperator Id { get; set; }
        public StringFilterOperator Name { get; set; }
        public ICollection<RandEParameterFilter> Or { get; set; }
        public ICollection<RandEParameterFilter> And { get; set; }
        public RandEParameterFilter Not { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "Name", Value = Name };
            yield return new InputPropertyInfo { Name = "or", Value = Or };
            yield return new InputPropertyInfo { Name = "and", Value = And };
            yield return new InputPropertyInfo { Name = "not", Value = Not };
        }
    }

    public partial class FileCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string HashMd5 { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "HashMD5", Value = HashMd5 };
            yield return new InputPropertyInfo { Name = "FileName", Value = FileName };
            yield return new InputPropertyInfo { Name = "FileType", Value = FileType };
        }
    }

    public partial class RandEParameterValueCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public Guid? RandEParameterId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ParameterName", Value = ParameterName };
            yield return new InputPropertyInfo { Name = "ParameterValue", Value = ParameterValue };
            yield return new InputPropertyInfo { Name = "RandEParameterId", Value = RandEParameterId };
        }
    }

    public partial class ExtendedAttribValueCondition : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ExtAttributeContent { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ExtAttributeContent", Value = ExtAttributeContent };
        }
    }

    public partial class ControlObjectUpdateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ControlObjectName { get; set; }
        public Guid? AddressId { get; set; }
        public Guid? SubjectId { get; set; }
        public Guid? CodeOktmoId { get; set; }
        public Guid? ControlTypeId { get; set; }
        public ICollection<Guid> ControlItemPassport { get; set; }
        public Guid? ControlFileId { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ControlObjectName", Value = ControlObjectName };
            yield return new InputPropertyInfo { Name = "AddressId", Value = AddressId };
            yield return new InputPropertyInfo { Name = "SubjectId", Value = SubjectId };
            yield return new InputPropertyInfo { Name = "CodeOKTMOId", Value = CodeOktmoId };
            yield return new InputPropertyInfo { Name = "ControlTypeId", Value = ControlTypeId };
            yield return new InputPropertyInfo { Name = "ControlItemPassport", Value = ControlItemPassport };
            yield return new InputPropertyInfo { Name = "ControlFileId", Value = ControlFileId };
        }
    }

    public partial class dicOKVEDCreateInput : IGraphQlInputObject
    {
        public Guid? Id { get; set; }
        public string ActivityType { get; set; }
        public string Decipher { get; set; }

        IEnumerable<InputPropertyInfo> IGraphQlInputObject.GetPropertyValues()
        {
            yield return new InputPropertyInfo { Name = "id", Value = Id };
            yield return new InputPropertyInfo { Name = "ActivityType", Value = ActivityType };
            yield return new InputPropertyInfo { Name = "Decipher", Value = Decipher };
        }
    }
    #endregion

    #region data classes
    public partial class dicOKSM
    {
        public Guid? Id { get; set; }
        public string Code { get; set; }
        public string FullCountryName { get; set; }
        public string ShortCountryName { get; set; }
        public string LetterCodeAlpha2 { get; set; }
        public string LetterCodeAlpha3 { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Query
    {
        public DicQuestionAnswers GetDicQuestionAnswers { get; set; }
        public ICollection<DicQuestionAnswers> FindAllDicQuestionAnswers { get; set; }
        public dicProcStatmentStatus GetdicProcStatmentStatus { get; set; }
        public ICollection<dicProcStatmentStatus> FindAlldicProcStatmentStatus { get; set; }
        public docProcStatement GetdocProcStatement { get; set; }
        public ICollection<docProcStatement> FindAlldocProcStatement { get; set; }
        public docControlOrder GetdocControlOrder { get; set; }
        public ICollection<docControlOrder> FindAlldocControlOrder { get; set; }
        public ControlCard GetControlCard { get; set; }
        public ICollection<ControlCard> FindAllControlCard { get; set; }
        public Person GetPerson { get; set; }
        public ICollection<Person> FindAllPerson { get; set; }
        public docControlPlan GetdocControlPlan { get; set; }
        public ICollection<docControlPlan> FindAlldocControlPlan { get; set; }
        public dicStatusPlan GetdicStatusPlan { get; set; }
        public ICollection<dicStatusPlan> FindAlldicStatusPlan { get; set; }
        public docControlAct GetdocControlAct { get; set; }
        public ICollection<docControlAct> FindAlldocControlAct { get; set; }
        public AddressClass GetAddress { get; set; }
        public ICollection<AddressClass> FindAllAddress { get; set; }
        public docControlList GetdocControlList { get; set; }
        public ICollection<docControlList> FindAlldocControlList { get; set; }
        public ControlListQuestions GetControlListQuestions { get; set; }
        public ICollection<ControlListQuestions> FindAllControlListQuestions { get; set; }
        public MandatoryReqs GetMandatoryReqs { get; set; }
        public ICollection<MandatoryReqs> FindAllMandatoryReqs { get; set; }
        public NPA GetNpa { get; set; }
        public ICollection<NPA> FindAllNpa { get; set; }
        public dicControlTypes GetdicControlTypes { get; set; }
        public ICollection<dicControlTypes> FindAlldicControlTypes { get; set; }
        public ControlItemResult GetControlItemResult { get; set; }
        public ICollection<ControlItemResult> FindAllControlItemResult { get; set; }
        public Materials GetMaterials { get; set; }
        public ICollection<Materials> FindAllMaterials { get; set; }
        public Violation GetViolation { get; set; }
        public ICollection<Violation> FindAllViolation { get; set; }
        public dicViolationTypes GetdicViolationTypes { get; set; }
        public ICollection<dicViolationTypes> FindAlldicViolationTypes { get; set; }
        public dicPunishmentType GetdicPunishmentType { get; set; }
        public ICollection<dicPunishmentType> FindAlldicPunishmentType { get; set; }
        public dicDamageType GetdicDamageType { get; set; }
        public ICollection<dicDamageType> FindAlldicDamageType { get; set; }
        public dicControlListStatus GetdicControlListStatus { get; set; }
        public ICollection<dicControlListStatus> FindAlldicControlListStatus { get; set; }
        public dicKNMForm GetdicKnmForm { get; set; }
        public ICollection<dicKNMForm> FindAlldicKnmForm { get; set; }
        public ControlOrganization GetControlOrganization { get; set; }
        public ICollection<ControlOrganization> FindAllControlOrganization { get; set; }
        public dicRole GetdicRole { get; set; }
        public ICollection<dicRole> FindAlldicRole { get; set; }
        public dicControlBase GetdicControlBase { get; set; }
        public ICollection<dicControlBase> FindAlldicControlBase { get; set; }
        public dicControlReasonDeny GetdicControlReasonDeny { get; set; }
        public ICollection<dicControlReasonDeny> FindAlldicControlReasonDeny { get; set; }
        public dicProsec GetdicProsec { get; set; }
        public ICollection<dicProsec> FindAlldicProsec { get; set; }
        public docRegulation GetdocRegulation { get; set; }
        public ICollection<docRegulation> FindAlldocRegulation { get; set; }
        public RandEParameterValue GetRandEParameterValue { get; set; }
        public ICollection<RandEParameterValue> FindAllRandEParameterValue { get; set; }
        public JournalAttributes GetJournalAttributes { get; set; }
        public ICollection<JournalAttributes> FindAllJournalAttributes { get; set; }
        public dicControlFileStatus GetdicControlFileStatus { get; set; }
        public ICollection<dicControlFileStatus> FindAlldicControlFileStatus { get; set; }
        public ControlItem GetControlItem { get; set; }
        public ICollection<ControlItem> FindAllControlItem { get; set; }
        public CitizenRequest GetCitizenRequest { get; set; }
        public ICollection<CitizenRequest> FindAllCitizenRequest { get; set; }
        public dicLicenceStatus GetdicLicenceStatus { get; set; }
        public ICollection<dicLicenceStatus> FindAlldicLicenceStatus { get; set; }
        public RiskCat GetRiskCat { get; set; }
        public ICollection<RiskCat> FindAllRiskCat { get; set; }
        public dicOKOPF GetdicOkopf { get; set; }
        public ICollection<dicOKOPF> FindAlldicOkopf { get; set; }
        public CheckOutEGRUL GetCheckOutEgrul { get; set; }
        public ICollection<CheckOutEGRUL> FindAllCheckOutEgrul { get; set; }
        public dicOKSM GetdicOksm { get; set; }
        public ICollection<dicOKSM> FindAlldicOksm { get; set; }
        public dicSubjectType GetdicSubjectType { get; set; }
        public ICollection<dicSubjectType> FindAlldicSubjectType { get; set; }
        public ExtendedAttrib GetExtendedAttrib { get; set; }
        public ICollection<ExtendedAttrib> FindAllExtendedAttrib { get; set; }
        public RandEParameter GetRandEParameter { get; set; }
        public ICollection<RandEParameter> FindAllRandEParameter { get; set; }
        public dicStatusKNM GetdicStatusKnm { get; set; }
        public ICollection<dicStatusKNM> FindAlldicStatusKnm { get; set; }
        public PersonAppointment GetPersonAppointment { get; set; }
        public ICollection<PersonAppointment> FindAllPersonAppointment { get; set; }
        public dicKNMTypes GetdicKnmTypes { get; set; }
        public ICollection<dicKNMTypes> FindAlldicKnmTypes { get; set; }
        public ControlJournals GetControlJournals { get; set; }
        public ICollection<ControlJournals> FindAllControlJournals { get; set; }
        public ControlProgram GetControlProgram { get; set; }
        public ICollection<ControlProgram> FindAllControlProgram { get; set; }
        public ControlFile GetControlFile { get; set; }
        public ICollection<ControlFile> FindAllControlFile { get; set; }
        public ControlObject GetControlObject { get; set; }
        public ICollection<ControlObject> FindAllControlObject { get; set; }
        public dicOKTMO GetdicOktmo { get; set; }
        public ICollection<dicOKTMO> FindAlldicOktmo { get; set; }
        public Activity GetActivity { get; set; }
        public ICollection<Activity> FindAllActivity { get; set; }
        public dicOKVED GetdicOkved { get; set; }
        public ICollection<dicOKVED> FindAlldicOkved { get; set; }
        public Licence GetLicence { get; set; }
        public ICollection<Licence> FindAllLicence { get; set; }
        public ControlItemPassport GetControlItemPassport { get; set; }
        public ICollection<ControlItemPassport> FindAllControlItemPassport { get; set; }
        public Subject GetSubject { get; set; }
        public ICollection<Subject> FindAllSubject { get; set; }
        public CheckOutEGRIP GetCheckOutEgrip { get; set; }
        public ICollection<CheckOutEGRIP> FindAllCheckOutEgrip { get; set; }
        public CheckOutEGRSMSP GetCheckOutEgrsmsp { get; set; }
        public ICollection<CheckOutEGRSMSP> FindAllCheckOutEgrsmsp { get; set; }
        public dicNPALevels GetdicNpaLevels { get; set; }
        public ICollection<dicNPALevels> FindAlldicNpaLevels { get; set; }
        public dicNPATypes GetdicNpaTypes { get; set; }
        public ICollection<dicNPATypes> FindAlldicNpaTypes { get; set; }
        public File GetFile { get; set; }
        public ICollection<File> FindAllFile { get; set; }
        public DicMeasureUnitsType GetDicMeasureUnitsType { get; set; }
        public ICollection<DicMeasureUnitsType> FindAllDicMeasureUnitsType { get; set; }
        public DicControlItemBaseType GetDicControlItemBaseType { get; set; }
        public ICollection<DicControlItemBaseType> FindAllDicControlItemBaseType { get; set; }
        public DicControlPlanType GetDicControlPlanType { get; set; }
        public ICollection<DicControlPlanType> FindAllDicControlPlanType { get; set; }
        public OrganizationUnit GetOrganizationUnit { get; set; }
        public ICollection<OrganizationUnit> FindAllOrganizationUnit { get; set; }
        public dicLicencedActivityTypes GetdicLicencedActivityTypes { get; set; }
        public ICollection<dicLicencedActivityTypes> FindAlldicLicencedActivityTypes { get; set; }
        public dicHazardClass GetdicHazardClass { get; set; }
        public ICollection<dicHazardClass> FindAlldicHazardClass { get; set; }
        public dicRiskCategory GetdicRiskCategory { get; set; }
        public ICollection<dicRiskCategory> FindAlldicRiskCategory { get; set; }
        public ExtendedAttribValue GetExtendedAttribValue { get; set; }
        public ICollection<ExtendedAttribValue> FindAllExtendedAttribValue { get; set; }
        public ClassHazardClass GetHazardClass { get; set; }
        public ICollection<ClassHazardClass> FindAllHazardClass { get; set; }
        public DicStates GetDicStates { get; set; }
        public ICollection<DicStates> FindAllDicStates { get; set; }
    }

    public partial class Materials
    {
        public Guid? Id { get; set; }
        public string MaterialName { get; set; }
        public Subject Subject { get; set; }
        public Licence LicenceFile { get; set; }
        public ControlFile ControlFile { get; set; }
        public ControlProgram ControlProgramFile { get; set; }
        public ControlItemResult ResultsFile { get; set; }
        public CitizenRequest CitizenRequest { get; set; }
        public docControlAct ControlActFile { get; set; }
        public docControlList ControlList { get; set; }
        public docControlOrder ControlOrderFile { get; set; }
        public File LinkedFile { get; set; }
        public docRegulation DocRegulation { get; set; }
        public docControlPlan DocControlPlan { get; set; }
        public docProcStatement DocProcStatement { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicOKTMO
    {
        public Guid? Id { get; set; }
        public string CodeOktmo { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class DicControlItemBaseType
    {
        public Guid? Id { get; set; }
        public string ControlItemBaseName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class DicControlPlanType
    {
        public Guid? Id { get; set; }
        public string ControlPlanType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class CheckOutEGRUL
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public object DateUpdateEgrul { get; set; }
        public string Ogrnul { get; set; }
        public object OgrnuLdata { get; set; }
        public string Innul { get; set; }
        public object DateRegYl { get; set; }
        public string OrgNameRegYl { get; set; }
        public string AddressRegOrgYl { get; set; }
        public string GrnOrgLocationYl { get; set; }
        public object GrnDateOrgLocationYl { get; set; }
        public object DateTaxReg { get; set; }
        public string TaxOrg { get; set; }
        public string GrnTaxOrg { get; set; }
        public object DateGrnTaxOrg { get; set; }
        public string RegNumberPfryl { get; set; }
        public object DateRegNumPfryl { get; set; }
        public string BranchPfryl { get; set; }
        public string GrnRegPfryl { get; set; }
        public object DateGrnRegPfryl { get; set; }
        public string GrnRegExecPfryl { get; set; }
        public string RegNumberExecPfryl { get; set; }
        public object DateRegNumExecPfryl { get; set; }
        public string BranchExecSsyl { get; set; }
        public string GrnRegExecSsyl { get; set; }
        public object DateGrnExecSsyl { get; set; }
        public string TypeCharterCapital { get; set; }
        public string SumCharterCapital { get; set; }
        public string GrnCharterCapital { get; set; }
        public object DateGrnCharterCapital { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string RepresentativeSurname { get; set; }
        public string Kpp { get; set; }
        public string ShortNameOrg { get; set; }
        public Subject SubjectEgrul { get; set; }
        public AddressClass Address { get; set; }
        public dicOKOPF CodeOkopf { get; set; }
        public dicOKSM Citizenships { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicStatusPlan
    {
        public Guid? Id { get; set; }
        public string StatusPlanName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class DicMeasureUnitsType
    {
        public Guid? Id { get; set; }
        public string MeasureUnitTypeName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicStatusKNM
    {
        public Guid? Id { get; set; }
        public string StatusKnmName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicSubjectType
    {
        public Guid? Id { get; set; }
        public string SubjectTypeName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicRole
    {
        public Guid? Id { get; set; }
        public string RoleName { get; set; }
        public ControlOrganization ControlOrganization { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicLicenceStatus
    {
        public Guid? Id { get; set; }
        public string LicenceStatusName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class JournalAttributes
    {
        public Guid? Id { get; set; }
        public string JournalAttirbuteName { get; set; }
        public string JournalAttributeValue { get; set; }
        public ControlJournals ControlJournals { get; set; }
        public DicMeasureUnitsType MeasureUnitType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class docProcStatement
    {
        public Guid? Id { get; set; }
        public object SendToProcDate { get; set; }
        public string ProcApproveFio { get; set; }
        public string ProcApproveRole { get; set; }
        public string ProcApprovePlace { get; set; }
        public ControlCard LinkedControlCard { get; set; }
        public docControlOrder DocControlOrder { get; set; }
        public Materials Materials { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlCard
    {
        public Guid? Id { get; set; }
        public object ProcDataAgreement { get; set; }
        public object ControlStartDate { get; set; }
        public string ControlStatus { get; set; }
        public decimal? ControlDuration { get; set; }
        public decimal? DurationProlong { get; set; }
        public object ControlEndDate { get; set; }
        public string ControlPurpose { get; set; }
        public bool? IsJoint { get; set; }
        public string NumberFgiserp { get; set; }
        public object FgiserpRegData { get; set; }
        public object LastEndControlDate { get; set; }
        public string CheckControlRestrict { get; set; }
        public string ControlCancelInfo { get; set; }
        public string InternalNumberFgiserp { get; set; }
        public object ControlFactStartDate { get; set; }
        public object ControlFactEndDate { get; set; }
        public decimal? FactControlPeriod { get; set; }
        public string FactControlPeriodUnit { get; set; }
        public string JointControlOrganization { get; set; }
        public ICollection<ControlItemResult> ControlItemResults { get; set; }
        public ICollection<docProcStatement> ProcedureStatements { get; set; }
        public ControlItemPassport ControlItemPassport { get; set; }
        public ICollection<docControlAct> ControlActs { get; set; }
        public DicMeasureUnitsType MeasureUnitType { get; set; }
        public dicKNMForm ControlForm { get; set; }
        public docControlPlan ControlPlan { get; set; }
        public dicControlBase ControlBase { get; set; }
        public dicControlReasonDeny ControlReasonDeny { get; set; }
        public dicProsec Prosec { get; set; }
        public DicControlItemBaseType ControlItemBaseType { get; set; }
        public ICollection<ControlListQuestions> ControlListQuestions { get; set; }
        public ICollection<docControlOrder> ControlOrders { get; set; }
        public ICollection<docRegulation> Regulations { get; set; }
        public ICollection<Person> ControlCardPersons { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlItem
    {
        public Guid? Id { get; set; }
        public string ControlItemName { get; set; }
        public object ControlDate { get; set; }
        public string ControlItemResult { get; set; }
        public ICollection<Person> ControlItemPersons { get; set; }
        public ControlProgram ControlProgram { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class docControlOrder
    {
        public Guid? Id { get; set; }
        public object ControlOrderCreateDate { get; set; }
        public object ControlOrderSignDate { get; set; }
        public string ControlOrderNumber { get; set; }
        public ControlCard ControlCard { get; set; }
        public Materials Materials { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Violation
    {
        public Guid? Id { get; set; }
        public string ViolationSpecificComment { get; set; }
        public ControlItemResult ControlItemResult { get; set; }
        public dicViolationTypes ViolationType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class AddressClass
	{
        public Guid? Id { get; set; }
        public string CodeKladr { get; set; }
        public string CodeFias { get; set; }
        public string Address { get; set; }
        public string PostIndex { get; set; }
        public string AddressFact { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class RiskCat
    {
        public Guid? Id { get; set; }
        public string CategoryClassLevel { get; set; }
        public object RiskCatStartDate { get; set; }
        public object RiskCatEndDate { get; set; }
        public ControlObject ControlObject { get; set; }
        public dicRiskCategory RiskCategory { get; set; }
        public Subject Subject { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicKNMForm
    {
        public Guid? Id { get; set; }
        public string KnmFormName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Person
    {
        public Guid? Id { get; set; }
        public string PersonName { get; set; }
        public string PersonSurName { get; set; }
        public string PersonSecondName { get; set; }
        public ICollection<ControlItem> ControlItems { get; set; }
        public ICollection<Subject> SubjectResponsibility { get; set; }
        public ControlOrganization ControlOrganization { get; set; }
        public ICollection<docControlPlan> ControlPlans { get; set; }
        public dicRole RoleName { get; set; }
        public ICollection<PersonAppointment> PersonAppointment { get; set; }
        public ICollection<ControlCard> ControlCards { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicProcStatmentStatus
    {
        public Guid? Id { get; set; }
        public string ProcStatementStatusName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class RandEParameterValue
    {
        public Guid? Id { get; set; }
        public string ParameterName { get; set; }
        public string ParameterValue { get; set; }
        public RandEParameter RandEParameter { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlProgram
    {
        public Guid? Id { get; set; }
        public string ProgramName { get; set; }
        public ICollection<Materials> Materials { get; set; }
        public ControlFile ControlFile { get; set; }
        public ICollection<ControlItem> ControlItems { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlFile
    {
        public Guid? Id { get; set; }
        public string ControlFileNumber { get; set; }
        public object ControlFileStartDate { get; set; }
        public ICollection<ControlJournals> ControlJournals { get; set; }
        public ICollection<Materials> Materials { get; set; }
        public ControlProgram ControlProgram { get; set; }
        public dicControlFileStatus ControlFileStatus { get; set; }
        public ControlObject ControObject { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicOKVED
    {
        public Guid? Id { get; set; }
        public string ActivityType { get; set; }
        public string Decipher { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class docControlPlan
    {
        public Guid? Id { get; set; }
        public object CreateDate { get; set; }
        public string ControlPlanName { get; set; }
        public object ControlPlanApproveData { get; set; }
        public string FgiserpGlobalPlanGuid { get; set; }
        public string ControlPlanYear { get; set; }
        public ICollection<ControlCard> ControlCardList { get; set; }
        public dicStatusPlan ControlPlanStatus { get; set; }
        public DicControlPlanType ControlPlanType { get; set; }
        public ICollection<Person> ControlPlanApprovers { get; set; }
        public Person ControlPlanAuthor { get; set; }
        public Materials Materials { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ExtendedAttribValue
    {
        public Guid? Id { get; set; }
        public string ExtAttributeContent { get; set; }
        public ControlObject ControlObject { get; set; }
        public ExtendedAttrib ExtendedAttrib { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class OrganizationUnit
    {
        public Guid? Id { get; set; }
        public string OrganizationUnitName { get; set; }
        public ControlOrganization ControlOrganization { get; set; }
        public ICollection<OrganizationUnit> OrganizationUnits { get; set; }
        public OrganizationUnit MainOrganizationUnit { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Mutation
    {
        public DicQuestionAnswers CreateDicQuestionAnswers { get; set; }
        public DicQuestionAnswers UpdateDicQuestionAnswers { get; set; }
        public bool? DeleteDicQuestionAnswers { get; set; }
        public dicProcStatmentStatus CreatedicProcStatmentStatus { get; set; }
        public dicProcStatmentStatus UpdatedicProcStatmentStatus { get; set; }
        public bool? DeletedicProcStatmentStatus { get; set; }
        public docProcStatement CreatedocProcStatement { get; set; }
        public docProcStatement UpdatedocProcStatement { get; set; }
        public bool? DeletedocProcStatement { get; set; }
        public docControlOrder CreatedocControlOrder { get; set; }
        public docControlOrder UpdatedocControlOrder { get; set; }
        public bool? DeletedocControlOrder { get; set; }
        public ControlCard CreateControlCard { get; set; }
        public ControlCard UpdateControlCard { get; set; }
        public bool? DeleteControlCard { get; set; }
        public Person CreatePerson { get; set; }
        public Person UpdatePerson { get; set; }
        public bool? DeletePerson { get; set; }
        public docControlPlan CreatedocControlPlan { get; set; }
        public docControlPlan UpdatedocControlPlan { get; set; }
        public bool? DeletedocControlPlan { get; set; }
        public dicStatusPlan CreatedicStatusPlan { get; set; }
        public dicStatusPlan UpdatedicStatusPlan { get; set; }
        public bool? DeletedicStatusPlan { get; set; }
        public docControlAct CreatedocControlAct { get; set; }
        public docControlAct UpdatedocControlAct { get; set; }
        public bool? DeletedocControlAct { get; set; }
        public AddressClass CreateAddress { get; set; }
        public AddressClass UpdateAddress { get; set; }
        public bool? DeleteAddress { get; set; }
        public docControlList CreatedocControlList { get; set; }
        public docControlList UpdatedocControlList { get; set; }
        public bool? DeletedocControlList { get; set; }
        public ControlListQuestions CreateControlListQuestions { get; set; }
        public ControlListQuestions UpdateControlListQuestions { get; set; }
        public bool? DeleteControlListQuestions { get; set; }
        public MandatoryReqs CreateMandatoryReqs { get; set; }
        public MandatoryReqs UpdateMandatoryReqs { get; set; }
        public bool? DeleteMandatoryReqs { get; set; }
        public NPA CreateNpa { get; set; }
        public NPA UpdateNpa { get; set; }
        public bool? DeleteNpa { get; set; }
        public dicControlTypes CreatedicControlTypes { get; set; }
        public dicControlTypes UpdatedicControlTypes { get; set; }
        public bool? DeletedicControlTypes { get; set; }
        public ControlItemResult CreateControlItemResult { get; set; }
        public ControlItemResult UpdateControlItemResult { get; set; }
        public bool? DeleteControlItemResult { get; set; }
        public Materials CreateMaterials { get; set; }
        public Materials UpdateMaterials { get; set; }
        public bool? DeleteMaterials { get; set; }
        public Violation CreateViolation { get; set; }
        public Violation UpdateViolation { get; set; }
        public bool? DeleteViolation { get; set; }
        public dicViolationTypes CreatedicViolationTypes { get; set; }
        public dicViolationTypes UpdatedicViolationTypes { get; set; }
        public bool? DeletedicViolationTypes { get; set; }
        public dicPunishmentType CreatedicPunishmentType { get; set; }
        public dicPunishmentType UpdatedicPunishmentType { get; set; }
        public bool? DeletedicPunishmentType { get; set; }
        public dicDamageType CreatedicDamageType { get; set; }
        public dicDamageType UpdatedicDamageType { get; set; }
        public bool? DeletedicDamageType { get; set; }
        public dicControlListStatus CreatedicControlListStatus { get; set; }
        public dicControlListStatus UpdatedicControlListStatus { get; set; }
        public bool? DeletedicControlListStatus { get; set; }
        public dicKNMForm CreatedicKnmForm { get; set; }
        public dicKNMForm UpdatedicKnmForm { get; set; }
        public bool? DeletedicKnmForm { get; set; }
        public ControlOrganization CreateControlOrganization { get; set; }
        public ControlOrganization UpdateControlOrganization { get; set; }
        public bool? DeleteControlOrganization { get; set; }
        public dicRole CreatedicRole { get; set; }
        public dicRole UpdatedicRole { get; set; }
        public bool? DeletedicRole { get; set; }
        public dicControlBase CreatedicControlBase { get; set; }
        public dicControlBase UpdatedicControlBase { get; set; }
        public bool? DeletedicControlBase { get; set; }
        public dicControlReasonDeny CreatedicControlReasonDeny { get; set; }
        public dicControlReasonDeny UpdatedicControlReasonDeny { get; set; }
        public bool? DeletedicControlReasonDeny { get; set; }
        public dicProsec CreatedicProsec { get; set; }
        public dicProsec UpdatedicProsec { get; set; }
        public bool? DeletedicProsec { get; set; }
        public docRegulation CreatedocRegulation { get; set; }
        public docRegulation UpdatedocRegulation { get; set; }
        public bool? DeletedocRegulation { get; set; }
        public RandEParameterValue CreateRandEParameterValue { get; set; }
        public RandEParameterValue UpdateRandEParameterValue { get; set; }
        public bool? DeleteRandEParameterValue { get; set; }
        public JournalAttributes CreateJournalAttributes { get; set; }
        public JournalAttributes UpdateJournalAttributes { get; set; }
        public bool? DeleteJournalAttributes { get; set; }
        public dicControlFileStatus CreatedicControlFileStatus { get; set; }
        public dicControlFileStatus UpdatedicControlFileStatus { get; set; }
        public bool? DeletedicControlFileStatus { get; set; }
        public ControlItem CreateControlItem { get; set; }
        public ControlItem UpdateControlItem { get; set; }
        public bool? DeleteControlItem { get; set; }
        public CitizenRequest CreateCitizenRequest { get; set; }
        public CitizenRequest UpdateCitizenRequest { get; set; }
        public bool? DeleteCitizenRequest { get; set; }
        public dicLicenceStatus CreatedicLicenceStatus { get; set; }
        public dicLicenceStatus UpdatedicLicenceStatus { get; set; }
        public bool? DeletedicLicenceStatus { get; set; }
        public RiskCat CreateRiskCat { get; set; }
        public RiskCat UpdateRiskCat { get; set; }
        public bool? DeleteRiskCat { get; set; }
        public dicOKOPF CreatedicOkopf { get; set; }
        public dicOKOPF UpdatedicOkopf { get; set; }
        public bool? DeletedicOkopf { get; set; }
        public CheckOutEGRUL CreateCheckOutEgrul { get; set; }
        public CheckOutEGRUL UpdateCheckOutEgrul { get; set; }
        public bool? DeleteCheckOutEgrul { get; set; }
        public dicOKSM CreatedicOksm { get; set; }
        public dicOKSM UpdatedicOksm { get; set; }
        public bool? DeletedicOksm { get; set; }
        public dicSubjectType CreatedicSubjectType { get; set; }
        public dicSubjectType UpdatedicSubjectType { get; set; }
        public bool? DeletedicSubjectType { get; set; }
        public ExtendedAttrib CreateExtendedAttrib { get; set; }
        public ExtendedAttrib UpdateExtendedAttrib { get; set; }
        public bool? DeleteExtendedAttrib { get; set; }
        public RandEParameter CreateRandEParameter { get; set; }
        public RandEParameter UpdateRandEParameter { get; set; }
        public bool? DeleteRandEParameter { get; set; }
        public dicStatusKNM CreatedicStatusKnm { get; set; }
        public dicStatusKNM UpdatedicStatusKnm { get; set; }
        public bool? DeletedicStatusKnm { get; set; }
        public PersonAppointment CreatePersonAppointment { get; set; }
        public PersonAppointment UpdatePersonAppointment { get; set; }
        public bool? DeletePersonAppointment { get; set; }
        public dicKNMTypes CreatedicKnmTypes { get; set; }
        public dicKNMTypes UpdatedicKnmTypes { get; set; }
        public bool? DeletedicKnmTypes { get; set; }
        public ControlJournals CreateControlJournals { get; set; }
        public ControlJournals UpdateControlJournals { get; set; }
        public bool? DeleteControlJournals { get; set; }
        public ControlProgram CreateControlProgram { get; set; }
        public ControlProgram UpdateControlProgram { get; set; }
        public bool? DeleteControlProgram { get; set; }
        public ControlFile CreateControlFile { get; set; }
        public ControlFile UpdateControlFile { get; set; }
        public bool? DeleteControlFile { get; set; }
        public ControlObject CreateControlObject { get; set; }
        public ControlObject UpdateControlObject { get; set; }
        public bool? DeleteControlObject { get; set; }
        public dicOKTMO CreatedicOktmo { get; set; }
        public dicOKTMO UpdatedicOktmo { get; set; }
        public bool? DeletedicOktmo { get; set; }
        public Activity CreateActivity { get; set; }
        public Activity UpdateActivity { get; set; }
        public bool? DeleteActivity { get; set; }
        public dicOKVED CreatedicOkved { get; set; }
        public dicOKVED UpdatedicOkved { get; set; }
        public bool? DeletedicOkved { get; set; }
        public Licence CreateLicence { get; set; }
        public Licence UpdateLicence { get; set; }
        public bool? DeleteLicence { get; set; }
        public ControlItemPassport CreateControlItemPassport { get; set; }
        public ControlItemPassport UpdateControlItemPassport { get; set; }
        public bool? DeleteControlItemPassport { get; set; }
        public Subject CreateSubject { get; set; }
        public Subject UpdateSubject { get; set; }
        public bool? DeleteSubject { get; set; }
        public CheckOutEGRIP CreateCheckOutEgrip { get; set; }
        public CheckOutEGRIP UpdateCheckOutEgrip { get; set; }
        public bool? DeleteCheckOutEgrip { get; set; }
        public CheckOutEGRSMSP CreateCheckOutEgrsmsp { get; set; }
        public CheckOutEGRSMSP UpdateCheckOutEgrsmsp { get; set; }
        public bool? DeleteCheckOutEgrsmsp { get; set; }
        public dicNPALevels CreatedicNpaLevels { get; set; }
        public dicNPALevels UpdatedicNpaLevels { get; set; }
        public bool? DeletedicNpaLevels { get; set; }
        public dicNPATypes CreatedicNpaTypes { get; set; }
        public dicNPATypes UpdatedicNpaTypes { get; set; }
        public bool? DeletedicNpaTypes { get; set; }
        public File CreateFile { get; set; }
        public File UpdateFile { get; set; }
        public bool? DeleteFile { get; set; }
        public DicMeasureUnitsType CreateDicMeasureUnitsType { get; set; }
        public DicMeasureUnitsType UpdateDicMeasureUnitsType { get; set; }
        public bool? DeleteDicMeasureUnitsType { get; set; }
        public DicControlItemBaseType CreateDicControlItemBaseType { get; set; }
        public DicControlItemBaseType UpdateDicControlItemBaseType { get; set; }
        public bool? DeleteDicControlItemBaseType { get; set; }
        public DicControlPlanType CreateDicControlPlanType { get; set; }
        public DicControlPlanType UpdateDicControlPlanType { get; set; }
        public bool? DeleteDicControlPlanType { get; set; }
        public OrganizationUnit CreateOrganizationUnit { get; set; }
        public OrganizationUnit UpdateOrganizationUnit { get; set; }
        public bool? DeleteOrganizationUnit { get; set; }
        public dicLicencedActivityTypes CreatedicLicencedActivityTypes { get; set; }
        public dicLicencedActivityTypes UpdatedicLicencedActivityTypes { get; set; }
        public bool? DeletedicLicencedActivityTypes { get; set; }
        public dicHazardClass CreatedicHazardClass { get; set; }
        public dicHazardClass UpdatedicHazardClass { get; set; }
        public bool? DeletedicHazardClass { get; set; }
        public dicRiskCategory CreatedicRiskCategory { get; set; }
        public dicRiskCategory UpdatedicRiskCategory { get; set; }
        public bool? DeletedicRiskCategory { get; set; }
        public ExtendedAttribValue CreateExtendedAttribValue { get; set; }
        public ExtendedAttribValue UpdateExtendedAttribValue { get; set; }
        public bool? DeleteExtendedAttribValue { get; set; }
        public ClassHazardClass CreateHazardClass { get; set; }
        public ClassHazardClass UpdateHazardClass { get; set; }
        public bool? DeleteHazardClass { get; set; }
    }

    public partial class File
    {
        public Guid? Id { get; set; }
        public string HashMd5 { get; set; }
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string FileSize { get; set; }
        public object TimeStamp { get; set; }
        public string DownloadLink { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class docRegulation
    {
        public Guid? Id { get; set; }
        public object RegulationCreateDate { get; set; }
        public object RegulationExecutionDate { get; set; }
        public string Result { get; set; }
        public ControlCard ControlCard { get; set; }
        public dicProcStatmentStatus ProcStatmentStatus { get; set; }
        public Materials Materials { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicControlFileStatus
    {
        public Guid? Id { get; set; }
        public string ControlFileStatusName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicControlListStatus
    {
        public Guid? Id { get; set; }
        public string ControlListStatusName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlItemResult
    {
        public Guid? Id { get; set; }
        public decimal? SumAdmFine { get; set; }
        public bool? SumAdmFineStatus { get; set; }
        public ICollection<Violation> Violations { get; set; }
        public ICollection<Materials> Materials { get; set; }
        public ICollection<dicDamageType> DamageType { get; set; }
        public ControlCard LinkedControlCard { get; set; }
        public ICollection<docControlList> ControlLists { get; set; }
        public dicPunishmentType PunishmentType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlItemPassport
    {
        public Guid? Id { get; set; }
        public object CreateDate { get; set; }
        public bool? IsInPlanYear { get; set; }
        public ControlCard ControlCard { get; set; }
        public ICollection<ControlObject> ControlObjects { get; set; }
        public dicControlTypes ControlType { get; set; }
        public dicKNMTypes KnmType { get; set; }
        public ControlOrganization ControlOrganization { get; set; }
        public Subject Subject { get; set; }
        public dicStatusKNM StatusKnmName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Meta
    {
        public string Creator { get; set; }
        public object CreateTime { get; set; }
        public string Editor { get; set; }
        public object EditTime { get; set; }
    }

    public partial class Activity
    {
        public Guid? Id { get; set; }
        public object ActivityStartDate { get; set; }
        public Subject Subject { get; set; }
        public dicOKVED Okved { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicViolationTypes
    {
        public Guid? Id { get; set; }
        public string Violation { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ClassHazardClass
    {
        public Guid? Id { get; set; }
        public string HazardClassLevel { get; set; }
        public object HazardClassStartDate { get; set; }
        public object HazardClassEndDate { get; set; }
        public ControlObject ControlObject { get; set; }
        public dicHazardClass HazardClass { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicControlTypes
    {
        public Guid? Id { get; set; }
        public string ControlTypeName { get; set; }
        public string ControlLevelName { get; set; }
        public ICollection<Subject> Subjects { get; set; }
        public ICollection<ControlOrganization> ControlOrganizations { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class PersonAppointment
    {
        public Guid? Id { get; set; }
        public object AppStartDate { get; set; }
        public object AppEndDate { get; set; }
        public Person Person { get; set; }
        public dicRole Role { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicControlReasonDeny
    {
        public Guid? Id { get; set; }
        public string ControlReasonDeny { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class CitizenRequest
    {
        public Guid? Id { get; set; }
        public string ApplicantName { get; set; }
        public string ApplicantSurname { get; set; }
        public string SecondName { get; set; }
        public string ApplicantEmail { get; set; }
        public string ApplicantPhone { get; set; }
        public string RequestContent { get; set; }
        public string ViolationCondition { get; set; }
        public object RequestGetDate { get; set; }
        public string TargetControlOrganization { get; set; }
        public string TargetControlOrganizationOgrn { get; set; }
        public AddressClass ApplicantAddress { get; set; }
        public ICollection<Materials> Materials { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicKNMTypes
    {
        public Guid? Id { get; set; }
        public string KnmTypeName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlListQuestions
    {
        public Guid? Id { get; set; }
        public string QuestionContent { get; set; }
        public string OuidBmQuestionInspection { get; set; }
        public string OuidBmInspectionListResult { get; set; }
        public ControlCard LinkedControlCard { get; set; }
        public DicQuestionAnswers QuestionAnswers { get; set; }
        public docControlList ControlList { get; set; }
        public MandatoryReqs MandatoryReq { get; set; }
        public NPA Npa { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicLicencedActivityTypes
    {
        public Guid? Id { get; set; }
        public string LicensedActivityCode { get; set; }
        public string LicensedActivityTypeName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlObject
    {
        public Guid? Id { get; set; }
        public string ControlObjectName { get; set; }
        public AddressClass Address { get; set; }
        public Subject Subject { get; set; }
        public dicOKTMO CodeOktmo { get; set; }
        public dicControlTypes ControlType { get; set; }
        public ICollection<RiskCat> RiskCategory { get; set; }
        public ICollection<ClassHazardClass> HazardClass { get; set; }
        public ICollection<ExtendedAttribValue> ExtendedAttribValues { get; set; }
        public ICollection<ControlItemPassport> ControlItemPassport { get; set; }
        public ControlFile ControlFile { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicPunishmentType
    {
        public Guid? Id { get; set; }
        public string PunishmentType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicOKOPF
    {
        public Guid? Id { get; set; }
        public string SubjectName { get; set; }
        public string CodeOkopf { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicProsec
    {
        public Guid? Id { get; set; }
        public string ProsecName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class NPA
    {
        public Guid? Id { get; set; }
        public string NpaName { get; set; }
        public object ApproveDate { get; set; }
        public string ApproveEntity { get; set; }
        public object NpaEndDate { get; set; }
        public string Number { get; set; }
        public string Body { get; set; }
        public dicControlTypes ControlType { get; set; }
        public ICollection<MandatoryReqs> MandatoryReqs { get; set; }
        public ICollection<ControlListQuestions> ControlListQuestions { get; set; }
        public dicNPATypes NpaType { get; set; }
        public dicNPALevels NpaLevel { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class DicStates
    {
        public Guid? Id { get; set; }
        public string State { get; set; }
        public string Step { get; set; }
        public string Infotxt { get; set; }
    }

    public partial class CheckOutEGRIP
    {
        public Guid? Id { get; set; }
        public object LastRenewData { get; set; }
        public string Ogrnip { get; set; }
        public string Innip { get; set; }
        public object OgrniPdate { get; set; }
        public object GrnEgripDate { get; set; }
        public string RegIpOld { get; set; }
        public string OrgRegLocationIp { get; set; }
        public string AddressOrgRegLocIp { get; set; }
        public string RegNumberPfrIp { get; set; }
        public object DateRegNumPfrIp { get; set; }
        public string BranchPfrip { get; set; }
        public string GrnRegPfrip { get; set; }
        public object DateGrnRegPfrIp { get; set; }
        public object DateUpdateEgrIp { get; set; }
        public Subject SubjectEgrip { get; set; }
        public AddressClass Address { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicDamageType
    {
        public Guid? Id { get; set; }
        public string DamageType { get; set; }
        public ICollection<ControlItemResult> ControlItemResults { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicNPALevels
    {
        public Guid? Id { get; set; }
        public string NpaLevelsName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Subject
    {
        public Guid? Id { get; set; }
        public string MainName { get; set; }
        public string Address { get; set; }
        public string Citizenship { get; set; }
        public string RepresentativeSurname { get; set; }
        public string RepresentativeName { get; set; }
        public string RepresentativeSecondName { get; set; }
        public string NameOrg { get; set; }
        public ICollection<ControlObject> ControlObjects { get; set; }
        public dicOKTMO CodeOktmo { get; set; }
        public ICollection<CheckOutEGRSMSP> CheckOutEgrsmsPs { get; set; }
        public ICollection<CheckOutEGRIP> CheckOutEgriPs { get; set; }
        public dicSubjectType SubjectTypes { get; set; }
        public ICollection<CheckOutEGRUL> CodeEgruLs { get; set; }
        public dicOKSM CodeOksm { get; set; }
        public ICollection<Materials> Materials { get; set; }
        public ICollection<Licence> Licences { get; set; }
        public ICollection<dicControlTypes> ControlType { get; set; }
        public ICollection<Activity> CodeOkveDs { get; set; }
        public ICollection<RiskCat> RiskCategory { get; set; }
        public Person AssignedOfficer { get; set; }
        public ICollection<ControlItemPassport> ControlItemPassports { get; set; }
        public ICollection<Subject> Filials { get; set; }
        public Subject HeadOrganization { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class RandEParameter
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public ICollection<ControlOrganization> ControlOrganizationParameter { get; set; }
        public DicMeasureUnitsType MeasureUnitType { get; set; }
        public ICollection<RandEParameterValue> RandEParameterValues { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicRiskCategory
    {
        public Guid? Id { get; set; }
        public string RiskCategoryName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class docControlList
    {
        public Guid? Id { get; set; }
        public object ControlListStartDate { get; set; }
        public object ControlListEndDate { get; set; }
        public ControlItemResult ControlItemResult { get; set; }
        public ICollection<ControlListQuestions> ControlListQuestions { get; set; }
        public Materials Materials { get; set; }
        public dicControlTypes ControlType { get; set; }
        public dicControlListStatus ControlListStatus { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class Licence
    {
        public Guid? Id { get; set; }
        public string LicenceName { get; set; }
        public string Series { get; set; }
        public string Number { get; set; }
        public object StartDate { get; set; }
        public object StopDate { get; set; }
        public object EndDate { get; set; }
        public string BlankNumber { get; set; }
        public string LicenseFinalDecision { get; set; }
        public object LicenseFinalStartDate { get; set; }
        public object LicenseFinalEndDate { get; set; }
        public dicLicenceStatus LicenceStatus { get; set; }
        public Subject Subject { get; set; }
        public ICollection<Materials> Materials { get; set; }
        public dicLicencedActivityTypes ActivityType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicHazardClass
    {
        public Guid? Id { get; set; }
        public string HazardClassName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlOrganization
    {
        public Guid? Id { get; set; }
        public string ControlOrganizationName { get; set; }
        public string OgrnKno { get; set; }
        public ICollection<OrganizationUnit> Divisions { get; set; }
        public ICollection<dicRole> OrganizationRoles { get; set; }
        public ICollection<Person> Employees { get; set; }
        public ICollection<dicControlTypes> ControlType { get; set; }
        public ICollection<ControlOrganization> SubOrganizations { get; set; }
        public ControlOrganization HeadOrganization { get; set; }
        public ICollection<RandEParameter> RandEParameters { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class docControlAct
    {
        public Guid? Id { get; set; }
        public object ControlActCreateDate { get; set; }
        public ControlCard ActLinkedtoControlCard { get; set; }
        public docControlList ControlList { get; set; }
        public Materials Materials { get; set; }
        public AddressClass ControlActCreatePlace { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class MandatoryReqs
    {
        public Guid? Id { get; set; }
        public string MandratoryReqContent { get; set; }
        public object StartDateMandatory { get; set; }
        public object EndDateMandatory { get; set; }
        public ICollection<NPA> Npa { get; set; }
        public ICollection<ControlListQuestions> ControlListQuestions { get; set; }
        public dicControlTypes ControlType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicNPATypes
    {
        public Guid? Id { get; set; }
        public string NpaTypeName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class dicControlBase
    {
        public Guid? Id { get; set; }
        public string ControlBaseName { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class DicQuestionAnswers
    {
        public Guid? Id { get; set; }
        public string QuestionAnswer { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class CheckOutEGRSMSP
    {
        public Guid? Id { get; set; }
        public object DateRmsp { get; set; }
        public string CaseNumber { get; set; }
        public bool? IsInRmsp { get; set; }
        public object LastRenewData { get; set; }
        public Subject SubjectEgrsmsp { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ControlJournals
    {
        public Guid? Id { get; set; }
        public string JournalCreationDate { get; set; }
        public string JournalName { get; set; }
        public ControlFile ControlFile { get; set; }
        public ICollection<JournalAttributes> JournalAttributes { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }

    public partial class ExtendedAttrib
    {
        public Guid? Id { get; set; }
        public string ExtAttributeName { get; set; }
        public string ExtAttributeContentUnit { get; set; }
        public string ExtAttributeTitle { get; set; }
        public ICollection<ExtendedAttribValue> ExtendedAttribValues { get; set; }
        public dicControlTypes ControlType { get; set; }
        public DicStates State { get; set; }
        public Meta Meta { get; set; }
    }
    #endregion

}