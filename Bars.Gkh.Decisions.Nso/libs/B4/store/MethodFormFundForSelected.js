Ext.define('B4.store.MethodFormFundForSelected', {
    extend: 'B4.base.Store',
    requires: ['B4.model.MethodFormFund'],
    model: 'B4.model.MethodFormFund',
    loadRecords: function (records, options) {
        var me = this;

        me.clearData(true);

        var idProperty = me.model.prototype.idProperty;

        var methodFormFund = B4 && B4.enums && B4.enums.MethodFormFund;
        var value = options && options.params && options.params[idProperty];

        if (methodFormFund && value) {
            value = value.toString();
            var enumStore = methodFormFund.getStore();
            var splitted = value.split(',');

            Ext.each(splitted, function(val) {
                if (val && val != '' && typeof (val) == 'string') {
                    var found = enumStore.findRecord(idProperty, val.trim());

                    if (found) {
                        me.add(found);
                    }
                }
            });
        }
    }
});