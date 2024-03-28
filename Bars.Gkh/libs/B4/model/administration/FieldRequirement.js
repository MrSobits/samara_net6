Ext.define('B4.model.administration.FieldRequirement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'FieldRequirement'
    },
    fields: [
        { name: 'RequirementId', useNull: false },
        { name: 'RecId', useNull: true },
        { name: 'ObjectName' },
        { name: 'FieldName' },
        { name: 'Required', defaultValue: false }
    ]
});