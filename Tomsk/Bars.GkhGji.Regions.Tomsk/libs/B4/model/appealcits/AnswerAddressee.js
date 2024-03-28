Ext.define('B4.model.appealcits.AnswerAddressee', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'AppealCitsAnswerAddressee'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Addressee', defaultValue: null },
        { name: 'Answer', defaultValue: null }
    ]
});