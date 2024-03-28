/**
* При наличия модуля Интеграция с ЭДО перекрывается  этим модулем
* Перекрывается в модуле GkhGji.Regions.Nso
*/
Ext.define('B4.model.AppealCits', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCits',
        timeout: 60000 * 5
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Number' },
        { name: 'SoprDate' },
        { name: 'DocumentNumber' },
        { name: 'NumberGji' },
        { name: 'DateFrom', defaultValue: new Date()},
        { name: 'CheckTime' },
        { name: 'OrderContragent' },
        { name: 'IncomingSourcesName' },
        { name: 'CorrespondentAge' },
        { name: 'FlatNum' },
        { name: 'ManagingOrganization' },
        { name: 'Year', defaultValue: (new Date()).getFullYear() },
        { name: 'Status', defaultValue: 0 }, // старый статус из МОНЖФ
        { name: 'State', defaultValue: null },
        { name: 'FastTrack', defaultValue: false },
        { name: 'Correspondent' },
        { name: 'CorrespondentAddress' },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'QuestionsCount' },
        { name: 'Description' },
        { name: 'DescriptionLocationProblem' },

        { name: 'KindStatement', defaultValue: null },
        { name: 'RedtapeFlag', defaultValue: 0 },
        { name: 'PreviousAppealCits', defaultValue: null },

        { name: 'Municipality' },
        { name: 'MunicipalityId' },
        { name: 'CountRealtyObj' },
        { name: 'File', defaultValue: null },
        { name: 'TypeCorrespondent', defaultValue: 10 },
        { name: 'AnswerDate' },

        { name: 'RealityAddresses' },
        { name: 'IncomingSources' },

        { name: 'Executors', defaultValue: null },
        { name: 'Testers', defaultValue: null },

        { name: 'SuretyDate' },
        { name: 'ExecuteDate' },
        { name: 'Surety', defaultValue: null },
        { name: 'SuretyResolve', defaultValue: null },
        { name: 'ZonalInspection', defaultValue: null },
        { name: 'ZoneName' },
        { name: 'Executant', defaultValue: null },
        { name: 'Tester', defaultValue: null },
        { name: 'Correspondent' },
        { name: 'RealObjAddresses' },
        { name: 'Accepting', defaultValue: 0 },
        { name: 'ArchiveNumber' },
        { name: 'CaseNumber', defaultValue: null },
        { name: 'CaseDate', defaultValue: null },
        { name: 'SocialStatus', defaultValue: null },
        { name: 'ApprovalContragent', defaultValue: null },

        { name: 'SpecialControl', defaultValue: false },
        { name: 'Subjects', defaultValue: null },
        { name: 'SubSubjects' },
        { name: 'Features' },
        { name: 'Executants' },
        { name: 'RevenueSourceNames' },
        { name: 'RevenueSourceNumbers' },
        { name: 'IsImported' },
        { name: 'ExecutantNames', defaultValue: null },

        { name: 'SSTUTransferOrg' },
        { name: 'SOPR' },
        { name: 'IsSopr' },
        { name: 'StateCode' },
        { name: 'AnswerDocNum' },
        { name: 'AnswerSugFile' },
        { name: 'SSTUExportState', defaultValue: 10 },
        { name: 'QuestionStatus', defaultValue: 0 },

        { name: 'RevenueSourceDates' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ExtensTime' },
        { name: 'IdentityConfirmed', defaultValue: false }
    ]
});