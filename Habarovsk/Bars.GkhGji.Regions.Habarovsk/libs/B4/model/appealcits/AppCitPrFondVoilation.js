Ext.define('B4.model.appealcits.AppCitPrFondVoilation', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppCitPrFondVoilation'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCitsPrescriptionFond', defaultValue: null },
        { name: 'ViolationGji', defaultValue: null },
        { name: 'PlanedDate' },
        { name: 'FactDate' }
    ]
});