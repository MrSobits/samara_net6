Ext.define('B4.model.networkoperator.NetworkOperatorRealityObject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'NetworkOperatorRealityObject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'OperatorName' },
        { name: 'NetworkOperator', defaultValue: null },
        { name: 'Bandwidth' },
        { name: 'RealityObject', defaultValue: null },
        { name: 'TechDecisionsTitle'},
        { name: 'TechDecisions', defaultValue: null }
    ]
});