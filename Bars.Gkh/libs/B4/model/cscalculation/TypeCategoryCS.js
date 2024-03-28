Ext.define('B4.model.cscalculation.TypeCategoryCS', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'TypeCategoryCS'
    },
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Name' },     
        { name: 'Code' }    
    ]
});