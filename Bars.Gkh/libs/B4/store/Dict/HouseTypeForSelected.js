Ext.define('B4.store.dict.HouseTypeForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.HouseType'],
    model: 'B4.model.dict.HouseType',
    loadRecords: function (records, options) {
        var me = this;
        
        me.clearData(true);

        var idProperty = me.model.prototype.idProperty;

        var typeHouse = B4 && B4.enums && B4.enums.TypeHouse;
        var value = options && options.params && options.params[idProperty];
        
        if (typeHouse && value) {
            if (value.split) {
                var enumStore = typeHouse.getStore();
                var splitted = value.split(',');

                Ext.each(splitted, function (val) {
                    if (val && val != '' && typeof(val) == 'string') {
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