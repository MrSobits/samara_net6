Ext.define('B4.model.SupplyResourceOrg', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [ 'B4.enums.OrgStateRole',
                'B4.enums.GroundsTermination'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'SupplyResourceOrg'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'OrgStateRole', defaultValue: 10 },
        { name: 'ContragentId' },
        { name: 'ContragentName' },
        { name: 'Description' },
        { name: 'ActivityGroundsTermination', defaultValue: 10 },
        { name: 'Municipality' },
        { name: 'Settlement' },
        { name: 'DateTermination' },
        { name: 'Inn' }
    ]
});