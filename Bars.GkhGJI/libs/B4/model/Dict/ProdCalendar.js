Ext.define('B4.model.dict.ProdCalendar', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'ProdCalendar'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
		{ name: 'ProdDate' }
    ]
});