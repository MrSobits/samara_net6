Ext.define('B4.model.manorg.contract.ManOrgAgrContractService', {
    extend: 'B4.model.manorg.contract.ManOrgContractService',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgAgrContractService'
    },
    fields: [
        { name: 'Type' },
        { name: 'PaymentAmount' }
    ]
});