Ext.define('B4.model.decision.ControlSubjects', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'DecisionControlSubjects'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Decision', defaultValue: null },
        { name: 'ControlActivity', defaultValue: null },
        { name: 'PersonInspection', defaultValue: 20},
        { name: 'Contragent' },
        { name: 'PhysicalPerson' },
        { name: 'PhysicalPersonINN' },
        { name: 'PhysicalPersonPosition' }
    ]
});