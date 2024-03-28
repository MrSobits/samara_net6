Ext.define('B4.model.integrations.houseManagement.SupplyResourceContract', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'HouseManagement',
        listAction: 'GetSupplyResourceContractList'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address' },
        { name: 'ContractNumber' },
        { name: 'DateStart' },
        { name: 'DateEnd' }
    ]
});