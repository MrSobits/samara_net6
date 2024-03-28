Ext.define('B4.model.disposal.Expert', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DisposalExpert'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Disposal', defaultValue: null },
        { name: 'ExpertGji', defaultValue: null },
        { name: 'ExpertType' }
    ]
});