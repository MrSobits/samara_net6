Ext.define('B4.model.realityobj.RealityObjectHousekeeper', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.YesNoNotSet'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealityObjectHousekeeper'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'RealityObject' },
        { name: 'IsActive', defaultValue: 10 },
        { name: 'FIO' },
        { name: 'Login' },
        { name: 'NewPassword' },
        { name: 'NewConfirmPassword' },
        { name: 'Password' },
        { name: 'PhoneNumber' }
    ]
});