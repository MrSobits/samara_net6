Ext.define('B4.store.dict.HeatTypeForSelect', {
    extend: 'Ext.data.Store',
    requires: ['B4.model.dict.HeatType', 'B4.enums.HeatingSystem'],
    model: 'B4.model.dict.HeatType',
    constructor: function (config) {
        config = config || {};
        config.data = B4.enums.HeatingSystem.getItemsMeta();
        this.callParent([config]);
    }
});
