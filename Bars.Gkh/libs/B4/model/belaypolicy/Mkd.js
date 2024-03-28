Ext.define('B4.model.belaypolicy.Mkd', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayPolicyMkd'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'IsExcluded', defaultValue: false },
        { name: 'Address' },
        { name: 'BelayPolicy', defaultValue: null },
        { name: 'RealityObject', defaultValue: null }
    ]
});