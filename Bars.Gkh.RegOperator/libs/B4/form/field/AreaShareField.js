Ext.define('B4.form.field.AreaShareField', {
    extend: 'Ext.form.field.Number',

    alias: 'widget.areasharefield',

    allowDecimals: true,
    decimalSeparator: ',',
    maxValue: 1,
    hideTrigger: true,
    initComponent: function() {
        var me = this;
        Ext.apply(me,
            {
                decimalPrecision: Gkh.config.RegOperator.GeneralConfig.AreaShareConfig.DecimalsAreaShare
            });

        me.callParent(arguments);
    }
});