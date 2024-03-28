Ext.define('B4.model.dict.ResettlementProgram', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    requires: [
        'B4.enums.StateResettlementProgram',
        'B4.enums.TypeResettlementProgram',
        'B4.enums.VisibilityResettlementProgram'
    ],
    proxy: {
        type: 'b4proxy',
        controllerName: 'ResettlementProgram'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'MatchFederalLaw', defaultValue: false },
        { name: 'PeriodName' },
        { name: 'Period', defaultValue: null },
        { name: 'StateProgram', defaultValue: 10 },
        { name: 'TypeProgram', defaultValue: 10 },
        { name: 'UseInExport', defaultValue: false },
        { name: 'Visibility', defaultValue: 10 }
    ]
});