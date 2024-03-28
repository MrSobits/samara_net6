Ext.define('B4.model.complaints.DecisionLS', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'SMEVComplaintsDecisionLifeSituation'
    },
    fields: [
        { name: 'Code'},
        { name: 'Name' },
        { name: 'FullName' },
        { name: 'SMEVComplaintsDecision'}       
    ]
});