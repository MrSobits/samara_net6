Ext.define('B4.model.dict.WorkTypeFinSource', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'WorkTypeFinSource'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Work', defaultValue: null },
        { name: 'TypeFinSource', defaultValue: 10 },
        { name: 'checked', defaultValue: false }
    ]
});