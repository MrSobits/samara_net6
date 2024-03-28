Ext.define('B4.model.cscalculation.CategoryCSMKD', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'CategoryCSMKD'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },
        { name: 'TypeCategoryCS' },
        { name: 'Code' }
    ]
});