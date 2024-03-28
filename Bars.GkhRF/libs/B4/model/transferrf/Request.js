Ext.define('B4.model.transferrf.Request', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: ['B4.enums.TypeProgramRequest'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RequestTransferRf'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ContractRf', defaultValue: null },
        { name: 'ContractRfName' },
        { name: 'ProgramCr', defaultValue: null },
        { name: 'ProgramCrName' },
        { name: 'ContragentBank', defaultValue: null },
        { name: 'ContragentBankName' },
        { name: 'ManagingOrganization', defaultValue: null },
        { name: 'ManagingOrganizationName' },
        { name: 'DocumentNum' },
        { name: 'DateFrom', defaultValue: null },
        { name: 'Perfomer' },
        { name: 'TypeProgramRequest', defaultValue: 10 },
        { name: 'State', defaultValue: null },
        { name: 'File', defaultValue: null },
        { name: 'MunicipalityName' },
        { name: 'TransferFundsCount', defaultValue: 0 },
        { name: 'TransferFundsSum',defaultValue: 0 }
    ]
});