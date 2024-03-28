Ext.define('B4.model.dict.ControlListTypicalQuestion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlListTypicalQuestion'
    },
    fields: [
        { name: 'Question' },
        { name: 'NormativeDoc' },
        { name: 'MandatoryRequirement' },
        { name: 'NpaName', defaultValue: null }
    ]
});