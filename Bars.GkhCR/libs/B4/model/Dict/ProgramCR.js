Ext.define('B4.model.dict.ProgramCr', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProgramCr'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name'}, 
        { name: 'Description'},
        { name: 'Code' },
        { name: 'UsedInExport', defaultValue: false },
        { name: 'NotAddHome', defaultValue: false },
        { name: 'MatchFl', defaultValue: false },
        { name: 'ForSpecialAccount', defaultValue: false },
        { name: 'UseForReformaAndGisGkhReports', defaultValue: false },
        { name: 'FinanceSource' },
        { name: 'Contragent' },
        { name: 'ContragentName' },
        { name: 'Period' },
        { name: 'PeriodName' },
        { name: 'IsCreateByDpkr', defaultValue: false },
        { name: 'TypeVisibilityProgramCr', defaultValue: 10 },
        { name: 'TypeProgramCr', defaultValue: 10 },
        { name: 'TypeProgramStateCr', defaultValue: 10 },
        { name: 'AddWorkFromLongProgram', defaultValue: 0 },
        { name: 'NormativeDoc' },
        { name: 'File', defaultValue: null },
        { name: 'DocumentNumber' },
        { name: 'DocumentDate' },
        { name: 'DocumentDepartment' },
        { name: 'ImportContract', defaultValue: false },
        { name: 'GovCustomer' }
    ]
});