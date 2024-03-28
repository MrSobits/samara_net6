Ext.define('B4.model.controllist.ControlListQuestion', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ControlListQuestion'
    },
    fields: [
        { name: 'QuestionContent' },
        { name: 'TypicalQuestion' },
        { name: 'TypicalAnswer' },
        { name: 'ControlList' }
    ]
});