Ext.define('B4.model.builder.SroInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderSroInfo'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'Work', defaultValue: null },
        { name: 'DescriptionWork', defaultValue: null }
    ]
});