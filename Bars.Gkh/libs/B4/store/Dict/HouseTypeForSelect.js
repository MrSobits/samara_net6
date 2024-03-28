Ext.define('B4.store.dict.HouseTypeForSelect', {
    extend: 'Ext.data.Store',
    requires: ['B4.model.dict.HouseType', 'B4.enums.TypeHouse'],
    model: 'B4.model.dict.HouseType',
    constructor: function (config) {
        config = config || { };
        config.data = B4.enums.TypeHouse.getItemsMeta();
        this.callParent([config]);
    }
});
