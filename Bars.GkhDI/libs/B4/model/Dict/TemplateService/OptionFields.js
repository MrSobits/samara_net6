Ext.define('B4.model.dict.templateservice.OptionFields', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TemplateServiceOptionFields'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'IsHidden', defaultValue: true },
        { name: 'Name' },
        { name: 'FieldName' },
        { name: 'TemplateService', defaultValue: false }
    ]
});