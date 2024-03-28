Ext.define('B4.model.appealcits.StatSubject', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsStatSubject'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'AppealCits', defaultValue: null },
        { name: 'Subject', defaultValue: null },
        { name: 'Subsubject' },
        { name: 'Feature' }
    ]
});