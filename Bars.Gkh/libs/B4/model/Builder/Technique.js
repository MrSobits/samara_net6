Ext.define('B4.model.builder.Technique', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'BuilderTechnique'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Builder', defaultValue: null },
        { name: 'Name' },
        { name: 'File', defaultValue: null }
    ]
});