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
        { name: 'FastTrack', defaultValue: false },
        { name: 'KindStatement', defaultValue: null },
        { name: 'DocumentNumber' },
        { name: 'NumberGji' },
        { name: 'DateFrom' },
        { name: 'CheckTime' },
        { name: 'CorrespondentAge' },
        { name: 'FlatNum' },
        { name: 'ManagingOrganization' },
        { name: 'Year', defaultValue: (new Date()).getFullYear() },
        { name: 'Status', defaultValue: 0 }, // старый статус из МОНЖФ
        { name: 'State', defaultValue: null },
        
{ name: 'AnswerDate' },
        { name: 'Correspondent' },
        { name: 'CorrespondentAddress' },
        { name: 'StateCode' },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'QuestionsCount' },
        { name: 'Description' },
        { name: 'DescriptionLocationProblem' },
        
        { name: 'RedtapeFlag', defaultValue: 0 },
        { name: 'PreviousAppealCits', defaultValue: null },
    
        { name: 'Municipality' },
        { name: 'CountRealtyObj' },
        { name: 'File'},
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
        { name: 'Subjects' },
        { name: 'SubSubjects' },
        { name: 'Features' },
        { name: 'Executants' },

        { name: 'SubSubjectsName' },
        { name: 'FeaturesName' },
        { name: 'ExecutantsFio' },
        { name: 'Executors' },
        { name: 'Testers' },
        { name: 'RevenueSourceNumbers' },
        { name: 'RevenueSourceNames' },
        { name: 'RevenueSourceDates' },

        { name: 'IsImported' },
        { name: 'MoSettlement' },
        { name: 'PlaceName' },
        { name: 'ContragentCorrespondent' },
        { name: 'IdentityConfirmed', defaultValue: false },
        { name: 'IsPrelimentaryCheck' }
    ]
});