Ext.define('B4.model.administration.Operator', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.TypeWorkplace',
        'B4.enums.ContragentType'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'operator'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'Login' },
        { name: 'Password' },
        { name: 'NewPassword' },
        { name: 'NewPasswordCommit' },
        { name: 'Role', defaultValue: null },
        { name: 'Email' },
        { name: 'TypeWorkplace', defaultValue: 10 },
        { name: 'Phone' },
        { name: 'IsActive', defaultValue: true },
        { name: 'Inspector' },
        { name: 'Contragent' },
        { name: 'GisGkhContragent' },
        { name: 'Municipality' },
        { name: 'Inn' },
        { name: 'ContragentName' },
        { name: 'RisToken' },
        { name: 'ContragentType', defaultValue: 10 },
        { name: 'ExportFormat', defaultValue: 0 },
        { name: 'MobileApplicationAccessEnabled' },
        { name: 'UserPhoto'}
    ]
});