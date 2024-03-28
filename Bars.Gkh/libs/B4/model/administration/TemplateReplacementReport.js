Ext.define('B4.model.administration.TemplateReplacementReport', {
    extend: 'B4.base.Model',
    idProperty: 'Code',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TemplateReplacement'
    },
    fields: [
        { name: 'Code' },
        { name: 'Name' },
        { name: 'Description' }
    ]
});