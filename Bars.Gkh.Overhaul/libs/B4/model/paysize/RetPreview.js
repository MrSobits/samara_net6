Ext.define('B4.model.paysize.RetPreview', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    fields: [
        { name: 'Id', useNull: true },
        { name: 'Address', defaultValue: null },
        { name: 'ExistEstateTypes', defaultValue: null },
        { name: 'NewEstateTypes', defaultValue: null },
        { name: 'ExistRate', defaultValue: null },
        { name: 'NewRate', defaultValue: null }
    ],
    proxy: {
        type: 'pagingmemory',
        enablePaging: true
    }
});