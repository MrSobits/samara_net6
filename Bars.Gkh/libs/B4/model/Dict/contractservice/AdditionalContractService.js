Ext.define('B4.model.dict.contractservice.AdditionalContractService', {
    extend: 'B4.model.dict.contractservice.ManagementContractService',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'additionalContractService'
    }
});