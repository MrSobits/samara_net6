Ext.define('B4.store.dict.ServiceTypeForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.dict.ServiceType'],
    model: 'B4.model.dict.ServiceType',
    loadRecords: function (records, options) {
        var me = this;
        
        me.clearData(true);

        var idProperty = me.model.prototype.idProperty;

        var typeService = B4 && B4.enums && B4.enums.TypeGroupServiceDi;
        var value = options && options.params && options.params[idProperty];
        
        if (typeService && value) {
            if (value.split) {
                var enumStore = typeService.getStore();
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