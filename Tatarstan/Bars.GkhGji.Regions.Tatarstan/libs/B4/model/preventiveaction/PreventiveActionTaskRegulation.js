Ext.define('B4.model.preventiveaction.PreventiveActionTaskRegulation',{
    extend: 'B4.base.Model',
    idProperty: 'Id',

    proxy: {
        type: 'b4proxy',
        controllerName: 'PreventiveActionTaskRegulation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'NormativeDoc'}
    ]
});