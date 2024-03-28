Ext.define('B4.model.manorg.contract.Owners', {
    extend: 'B4.model.manorg.contract.Base',
    requires: ['B4.enums.ManOrgContractOwnersFoundation'],
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ManOrgContractOwners'
    },
    fields: [
        { name: 'ContractFoundation', defaultValue: 20 },
        { name: 'DocumentNumberOnRegistry' },
        { name: 'DocumentDateOnRegistry' },
        { name: 'DocumentNumberOffRegistry' },
        { name: 'DocumentDateOffRegistry' }  
    ]
});