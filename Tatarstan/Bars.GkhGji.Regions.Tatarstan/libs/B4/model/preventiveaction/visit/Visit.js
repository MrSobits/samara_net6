Ext.define('B4.model.preventiveaction.visit.Visit', {
    extend: 'B4.model.DocumentGji',
    proxy: {
        type: 'b4proxy',
        controllerName: 'VisitSheet'
    },
    fields: [
        { name: 'ExecutingInspector' },
        { name: 'HasCopy' },
        { name: 'VisitDateStart' },
        { name: 'VisitDateEnd' },
        { name: 'VisitTimeStart' },
        { name: 'VisitTimeEnd' },
        { name: 'HasViolThreats' }
    ]
});