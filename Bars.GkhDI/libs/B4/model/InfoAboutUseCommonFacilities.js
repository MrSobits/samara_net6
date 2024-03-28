Ext.define('B4.model.InfoAboutUseCommonFacilities', {
    extend: 'B4.base.Model',

    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InfoAboutUseCommonFacilities'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'DisclosureInfoRealityObj', defaultValue: null },
        { name: 'KindCommomFacilities' },
        { name: 'Number' },
        { name: 'CostContract' },
        { name: 'From' },
        { name: 'DateStart' },
        { name: 'DateEnd'},
        { name: 'Lessee' },
        { name: 'TypeContract', defaultValue: 10 },
        { name: 'AppointmentCommonFacilities' },
        { name: 'AreaOfCommonFacilities' },
        { name: 'ContractNumber' },
        { name: 'ContractDate' },
        { name: 'CostByContractInMonth' },
        { name: 'ProtocolFile' },
        { name: 'LesseeType', defaultValue: 10 },
        { name: 'Surname' },
        { name: 'Name' },
        { name: 'Gender', defaultValue: 0 },
        { name: 'BirthDate' },
        { name: 'BirthPlace' },
        { name: 'Snils' },
        { name: 'Ogrn' },
        { name: 'Inn' },
        { name: 'ContractFile' },
        { name: 'ContractSubject' },
        { name: 'Patronymic' },
        { name: 'Comment' },
        { name: 'SigningContractDate' },
        { name: 'DayMonthPeriodIn' },
        { name: 'DayMonthPeriodOut' },
        { name: 'IsLastDayMonthPeriodIn' },
        { name: 'IsLastDayMonthPeriodOut' },
        { name: 'IsNextMonthPeriodIn' },
        { name: 'IsNextMonthPeriodOut' }
    ]
});