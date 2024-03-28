Ext.define('B4.model.actcheck.InstrExamActionNormativeDoc', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InstrExamActionNormativeDoc'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'InstrExamAction' },
        { name: 'NormativeDoc' }
    ]
});