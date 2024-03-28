Ext.define('B4.view.bankstatement.PaymentOrderFilterPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.paymentorderfilterpanel',

    closable: false,
    header: false,
    layout: 'anchor',
    bodyPadding: 5,
    itemId: 'paymentOrderFilterPanel',
    trackResetOnLoad: true,
    autoScroll: true,
    requires: [
        'B4.form.SelectField',
        'B4.store.dict.Period'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {

            items: [
                {
                    xtype: 'b4selectfield',
                    editable: false,
                   

                    store: 'B4.store.dict.Period',
                    itemId: 'sfPeriod',
                    fieldLabel: 'Период',
                    width: 500,
                    labelAlign: 'right',
                    emptyText: 'Выберите период...',
                    labelWidth: 70
                }
            ]
        });

        me.callParent(arguments);
    }
});