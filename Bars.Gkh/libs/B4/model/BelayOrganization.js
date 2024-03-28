Ext.define('B4.model.BelayOrganization', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [ 'B4.enums.OrgStateRole',
                'B4.enums.GroundsTermination'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayOrganization'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'OrgStateRole', defaultValue: 10 },
        { name: 'Municipality' },
        { name: 'ContragentName' },
        { name: 'Description' },
        { name: 'ActivityDescription' },
        { name: 'ActivityGroundsTermination', defaultValue: 10 },
        { name: 'DateTermination' },
        { name: 'Inn' }
    ]
});