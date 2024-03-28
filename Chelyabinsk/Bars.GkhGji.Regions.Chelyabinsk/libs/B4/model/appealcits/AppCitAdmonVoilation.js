Ext.define('B4.model.appealcits.AppCitAdmonVoilation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppCitAdmonVoilation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCitsAdmonition', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'PlanedDate' },
        { name: 'ViolationGjiPin', defaultValue: null },
        { name: 'CodesPin' },
        { name: 'ViolationGjiName' },
        { name: 'ViolationGjiId' },
        { name: 'FactDate' }
    ]
});