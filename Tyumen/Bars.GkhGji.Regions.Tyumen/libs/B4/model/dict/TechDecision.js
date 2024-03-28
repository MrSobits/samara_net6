Ext.define('B4.model.dict.TechDecision', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TechDecision'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' }
    ]
});