Ext.define('B4.model.AppealCits', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCits',
        timeout: 60000
    },
    fields: [
        { name: 'AddressEdo' },
        { name: 'IsEdo' },
        { name: 'DateActual' },

        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Number' },
        { name: 'DocumentNumber' },
        { name: 'NumberGji' },
        { name: 'DateFrom' },
        { name: 'CheckTime' },
        { name: 'FastTrack', defaultValue: false },
        { name: 'IsIdentityVerified' },
        { name: 'FlatNum' },
        { name: 'ManagingOrganization' },
        { name: 'Contragent' },
        { name: 'Year' },
        { name: 'Status', defaultValue: 0 }, // старый статус из МОНЖФ
        { name: 'State', defaultValue: 0 },
        
        { name: 'Correspondent' },
        { name: 'CorrespondentAddress' },
        { name: 'StateCode' },
        { name: 'Email' },
        { name: 'Phone' },
        { name: 'QuestionsCount' },
        { name: 'Description' },
        { name: 'DescriptionLocationProblem' },
        
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
        { name: 'Executant', defaultValue: null },
        { name: 'Tester', defaultValue: null },
        { name: 'RealObjAddresses' },
        { name: 'CountSubject' },
        { name: 'AddressEdo' },

        { name: 'Subjects' },
        { name: 'SubSubjects' },
        { name: 'Features' },
        { name: 'Executants' },
        { name: 'RevenueSourceNames' },
        { name: 'RevenueSourceNumbers' },
        { name: 'RevenueSourceDates' },

        { name: 'IsPrelimentaryCheck' },
        { name: 'SubjectName' },
        { name: 'DocumentNumber' },
        { name: 'IsProcessedStateOver' },
        { name: 'IsNotProcessedStateOver' }
    ]
});