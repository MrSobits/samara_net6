Ext.define('B4.view.suspenseaccount.SuspenseAccountPayoffDistributionWindowWithGrid', {
    extend: 'B4.form.Window',

    alias: 'widget.suspenceaccountpayoffdistributionwindowwithgrid',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.model.Contragent',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.enums.SuspenseAccountDistributionParametersType',
        'B4.view.suspenseaccount.SuspenseAccountPayoffDistribution'
    ],

    title: 'Распределение платежа',

    modal: true,

    width: 800,
    height: 500,

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5'
            },
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            border: 0,
            items: [
                {
                    xtype: 'suspenseaccounpayoffdistribution',
                    enableCellEdit: me.enableCellEdit,
                    sum: me.sum,
                    flex: 1
                }
            ]

        });
        me.callParent(arguments);
    }
});