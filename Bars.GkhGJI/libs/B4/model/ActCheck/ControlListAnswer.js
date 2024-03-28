Ext.define('B4.model.actcheck.ControlListAnswer', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ActCheckControlListAnswer'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'ActCheck', defaultValue: null },
        { name: 'YesNoNotApplicable', defaultValue: 0},
        { name: 'Description' },
        { name: 'NpdName' },
        { name: 'ControlListQuestion' },
        { name: 'Question' }
    ]
});