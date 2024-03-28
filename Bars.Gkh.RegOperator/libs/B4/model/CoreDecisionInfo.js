Ext.define('B4.model.CoreDecisionInfo', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CoreDecision'
    },
    fields: [
        { name: 'Id'},
        { name: 'DocumentNum' },
        { name: 'DocumentType' },
        { name: 'ProtocolDate' },
        { name: 'State' }
    ]
});