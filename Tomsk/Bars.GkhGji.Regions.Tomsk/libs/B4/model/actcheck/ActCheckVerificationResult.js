Ext.define('B4.model.actcheck.ActCheckVerificationResult', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckVerificationResult'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name', defaultValue: null },
        { name: 'Active', defaultValue: null }
    ]
});