Ext.define('B4.view.suspenseaccount.SuspenseAccountPayoffDistributionWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.suspenceaccountpayoffdistributionwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.model.Contragent',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.enums.SuspenseAccountDistributionParametersType',
        'B4.view.suspenseaccount.SuspenseAccountPayoffDistribution',
        'B4.view.Control.GkhTriggerField'
    ],

    title: 'Распределение платежа',

    modal: true,

    width: 800,

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
                    xtype: 'gkhtriggerfield',
                    name: 'RealtyObjects',
                    fieldLabel: 'Дома',
                    emptyText: 'Выберите дома для зачисления'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'PerformedWorkActs',
                    fieldLabel: 'Акты',
                    emptyText: 'Выберите акты выполненных работ для зачисления'
                }                   
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Продолжить',
                                    action: 'NextStep'
                                }
                            ]
                        }
                    ]
                }
            ]
        });
        me.callParent(arguments);
    }
});