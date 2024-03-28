Ext.define('B4.model.courtpractice.CourtPractice', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'CourtPractice'
    },
    fields: [
        { name: 'Id' },
        { name: 'State'},
        { name: 'JurInstitution'},
        { name: 'DocumentNumber'},
        { name: 'ContragentPlaintiff' },
        { name: 'ContragentDefendant' },
        { name: 'PlaintiffFio' }, 
        { name: 'PlaintiffAddress' }, 
        { name: 'DefendantFio' }, 
        { name: 'DefendantAddress' }, 
        { name: 'DifferentFIo' },
        { name: 'DifferentAddress' },
        { name: 'DifferentContragent' },
        { name: 'FileInfo' },
        { name: 'CourtPracticeState', defaultValue: 20},
        { name: 'DisputeType', defaultValue: 10 },
        { name: 'DisputeCategory', defaultValue: 10 },
        { name: 'TypeFactViolation' },
        { name: 'DocumentGJINumber' },
        { name: 'Discription' },
        { name: 'TypeDocumentGji' }, 
        { name: 'InspId' },
        { name: 'TypeBase' }, 
        { name: 'Lawyer' },
        { name: 'DateCourtMeetingDate' },
        { name: 'Inspector' },
        { name: 'InstanceGji' },
        { name: 'InstanceGjiCode' },
        { name: 'Dispute' },
        { name: 'PausedComment' },
        { name: 'DateCourtMeeting' },
        { name: 'CourtMeetingTime' },
        { name: 'InterimMeasures', defaultValue:false },
        { name: 'InterimMeasuresDate' },
        { name: 'CourtMeetingResult', defaultValue: 0 },
        { name: 'InLaw', defaultValue:false },
        { name: 'InLawDate' },
        { name: 'Court�osts' },
        { name: 'Court�ostsPlan' },
        { name: 'Court�ostsFact' },
        { name: 'PerformanceList' },
        { name: 'DifferentChoice' },
        { name: 'FormatMinute' },
        { name: 'FormatHour' },
        { name: 'DefendantChoice' },
        { name: 'PlantiffChoice' },
        { name: 'DocumentGji' },
        { name: 'MKDLicRequest' },
        { name: 'PerformanceProceeding'}
    ]
});