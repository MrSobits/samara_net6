Ext.define('B4.model.integrations.houseManagement.PublicPropertyContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseManagement',
        listAction: 'GetPublicPropertyContractList'
    },
    fields: [
        { name: 'Id'},
        { name: 'Address' },
        { name: 'ContractNumber' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});
