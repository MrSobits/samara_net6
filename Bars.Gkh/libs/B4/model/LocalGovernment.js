Ext.define('B4.model.LocalGovernment', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [ 'B4.enums.OrgStateRole',
                'B4.enums.GroundsTermination'],
    proxy: {
        type: 'b4proxy',
        controllerName: 'LocalGovernment'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Contragent', defaultValue: null },
        { name: 'OrgStateRole', defaultValue: 10 },
        { name: 'ContragentName' },
        { name: 'Municipality' },
        { name: 'Inn' },
        { name: 'Description' },
        { name: 'Email' },
        { name: 'NameDepartamentGkh' },
        { name: 'OfficialSite' },
        { name: 'Phone' }
    ]
});