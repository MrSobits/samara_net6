Ext.define('B4.model.RealEstateTypeStructElement', {
    extend: 'B4.base.Model',
    idProperty: 'Id',
    proxy: {
        type: 'b4proxy',
        controllerName: 'RealEstateTypeStructElement'
    },
    fields: [
        { name: 'Id', type: 'number', defaultValue: 0 },
        { name: 'RealEstateType' },
        { name: 'StructuralElement' },
        { name: 'Exists', type: 'boolean', defaultValue: true, allowBlank: false }
    ]
});