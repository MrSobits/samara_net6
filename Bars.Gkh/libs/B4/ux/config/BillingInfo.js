Ext.define('B4.ux.config.BillingInfo', {
    extend: 'Ext.panel.Panel',
    alias: 'widget.billinginfo',
    layout: 'anchor',
    requires: [
        'B4.enums.SymbolsLocation',
        'B4.form.ComboBox'
    ],

    mixins: {
        field: 'Ext.form.field.Field'
    },

    bodyStyle: Gkh.bodyStyle,
    border: 0,
    
    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'label',
                    html: 'Примечание! <br> Данные настройки действуют для следующих файлов: Account, Calc, Recalc, SaldoChange, CalcProt.',
                    height: 35

                }
            ]
        });

        me.callParent(arguments);
    }
});