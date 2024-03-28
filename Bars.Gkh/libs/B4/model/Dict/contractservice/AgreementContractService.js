Ext.define('B4.model.dict.contractservice.AgreementContractService', {
    extend: 'B4.model.dict.contractservice.ManagementContractService',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'agreementContractService'
    },
    fields: [
        { name: 'WorkAssignment' },
        { name: 'TypeWork' }
    ]
});