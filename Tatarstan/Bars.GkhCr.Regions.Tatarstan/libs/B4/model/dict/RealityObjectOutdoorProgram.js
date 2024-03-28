Ext.define('B4.model.dict.RealityObjectOutdoorProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectOutdoorProgram'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ExportId' },
        { name: 'ExternalId' },
        { name: 'Name' },
        { name: 'Code' },
        { name: 'TypeVisibilityProgram', defaultValue: 10 },
        { name: 'TypeProgramState', defaultValue: 10 },
        { name: 'TypeProgram', defaultValue: 10 },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentDepartment' },
        { name: 'IsNotAddOutdoor', defaultValue: false },
        { name: 'Description' },
        { name: 'GovernmentCustomer' },
        { name: 'Period' },
        { name: 'PeriodName' },
        { name: 'NormativeDoc' },
        { name: 'File', defaultValue: null }
    ]
});