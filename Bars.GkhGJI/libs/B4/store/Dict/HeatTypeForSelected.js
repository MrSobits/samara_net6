Ext.define('B4.store.dict.HeatTypeForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.HeatType'],
    model: 'B4.model.dict.HeatType',
    loadRecords: function (records, options) {
        var me = this;
        me.clearData(true);

        var idProperty = me.model.prototype.idProperty;

        var heatType = B4 && B4.enums && B4.enums.HeatingSystem;
        var value = options && options.params && options.params[idProperty];

        if (heatType && value) {
            if (value.split) {
                var enumStore = heatType.getStore();
                var splitted = value.split(',');

                Ext.each(splitted, function (val) {
                    if (val && val != '' && typeof (val) == 'string') {
                        var found = enumStore.findRecord(idProperty, val.trim());

                        if (found) {
                            me.add(found);
                        }
                    }
                });
            }
        }
    }
});