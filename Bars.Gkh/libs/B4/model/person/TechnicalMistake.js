Ext.define('B4.model.person.TechnicalMistake', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TechnicalMistake'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'QualificationCertificate', defaultValue: null },
        { name: 'StatementNumber' },
        { name: 'FixInfo' },
        { name: 'FixDate' },
        { name: 'IssuedDate' },
        { name: 'Inspector' },
        { name: 'DecisionNumber' },
        { name: 'DecisionDate' }
    ]
});