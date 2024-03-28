Ext.define('B4.model.belaypolicy.Risk', {
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'BelayPolicyRisk'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'BelayPolicy', defaultValue: null }
    ]
});