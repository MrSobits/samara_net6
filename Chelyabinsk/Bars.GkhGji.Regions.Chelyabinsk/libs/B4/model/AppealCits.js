﻿/*
* При наличия модуля Интеграция с ЭДО перекрывается  этим модулем
*/
Ext.define('B4.model.AppealCits', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCits',
        timeout: 5 * 60 * 1000 // 5 минут
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Number' },
        { name: 'DocumentNumber' },
        { name: 'NumberGji' },
        { name: 'DateFrom' },
        { name: 'CheckTime' },
        { name: 'OrderContragent' },
        { name: 'FlatNum' },
        { name: 'ManagingOrganization' },
        { name: 'Year', defaultValue: (new Date()).getFullYear() },
        { name: 'Status', defaultValue: 0 }, // старый статус из МОНЖФ
        { name: 'State', defaultValue: null },
        { name: 'CorrespondentAge' },
        { name: 'Correspondent' },
        { name: 'CorrespondentAddress' },
        { name: 'StateCode' },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'QuestionsCount' },
        { name: 'Description' },
        { name: 'DescriptionLocationProblem' },
        { name: 'FastTrack', defaultValue: false },
        { name: 'KindStatement', defaultValue: null },
        { name: 'RedtapeFlag', defaultValue: 0 },
        { name: 'PreviousAppealCits', defaultValue: null },

        { name: 'Municipality' },
        { name: 'CountRealtyObj' },
        { name: 'File', defaultValue: null },
        { name: 'TypeCorrespondent', defaultValue: 10 },

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
        { name: 'HasExecutant' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'IncomingSources' },
        { name: 'RevenueSourceNames' }, 
        { name: 'RealityAddresses' }, 
        { name: 'RevenueSourceNumbers' }, 
        { name: 'RevenueSourceDates' },
        { name: 'ExtensTime' },
        { name: 'Comment' },
        { name: 'Executors' },
        { name: 'Testers' },
        { name: 'AmountPages' },
        { name: 'Citizenship' },
        { name: 'DeclarantMailingAddress' },
        { name: 'DeclarantWorkPlace' },
        { name: 'DeclarantPhone' },
        { name: 'DeclarantSex', defaultValue: 0 },
        { name: 'AppealStatus' },
        { name: 'PlannedExecDate' },
        { name: 'AppealRegistrator' },
        { name: 'ExecutantTakeDate' },
        { name: 'Executants' },
        { name: 'Controllers' },
        { name: 'IncomingSourcesName' },
        { name: 'ContragentCorrespondent' },
        { name: 'SSTUTransferOrg' },
        { name: 'SSTUExportState', defaultValue: 10 },
        { name: 'QuestionStatus', defaultValue: 0 },
        { name: 'IdentityConfirmed', defaultValue: false },

        { name: 'ControlDateGisGkh' },
        { name: 'FavoriteFilter' },
        { name: 'FavoriteFilterCondition' }
    ]
});