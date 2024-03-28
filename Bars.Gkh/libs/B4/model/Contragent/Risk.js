Ext.define('B4.model.contragent.Risk', {
    extend: 'B4.base.Model',
    proxy: {
        type: 'b4proxy',
        controllerName: 'InspectionGji',
        listAction: 'ListContragentRisk'
    },
    fields: [
        { name: 'Id', type: 'int', useNull: true },
        { name: 'Number', type: 'string' },
        { name: 'RiskCategory', type: 'string' },
        { name: 'StartDate', type: 'date' },
        { name: 'EndDate', type: 'date' }
    ]
});