Ext.define('B4.view.suspenseaccount.SuspenseAccountPayoffDistributionPersonalAccountWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.suspenceaccountpayoffdistributionpersonalaccountwindow',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.model.Contragent',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.enums.SuspenseAccountDistributionParametersType',
        'B4.view.suspenseaccount.SuspenseAccountPayoffDistributionPersonalAccount',
        'B4.view.Control.GkhTriggerField'
    ],

    title: 'Распределение платежа',

    modal: true,

    width: 800,

    initComponent: function() {
        var me = this;
        Ext.apply(me, {
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            bodyStyle: Gkh.bodyStyle,
            border: 0,
            items: [
                {
                    xtype: 'gkhtriggerfield',
                    name: 'PersonalAccounts',
                    fieldLabel: 'Лицевые счета',
                    labelAlign: 'right',
                    editable: false,
                    padding: '5 5 0 5',
                    emptyText: 'Выберите лицевые счета для зачисления'
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