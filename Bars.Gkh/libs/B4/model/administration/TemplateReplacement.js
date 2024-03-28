Ext.define('B4.model.administration.TemplateReplacement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TemplateReplacement'
    },
    fields: [
        { name: 'Id', useNull: true},
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Description' },
        { name: 'HasReplace', defaultValue: false },
        { name: 'File', defaultValue: null },
        { name: 'Extension' }
    ]
});