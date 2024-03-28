Ext.define('B4.view.suspenseaccount.SuspenseAccountAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.suspenceaccountaddwin',

    requires: [
        'B4.ux.button.Save',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.store.Contragent',
        'B4.model.Contragent',
        'B4.enums.SuspenseAccountDistributionParametersView',
        'B4.enums.SuspenseAccountDistributionParametersType'
    ],
    
    title: 'Параметры распределения',
    
    width: 600,
    height: 300,
    minHeight: 180,
    minWidth: 400,
    
    cbSuspenseAccountDistributionParametersTypes : [],
    
    initComponent: function () {
        var me = this;
        Ext.apply(me, {
            defaults: {
                margin: '5 5 5 5',
                labelAlign: 'right',
                labelWidth: 130
            },
            layout: {
                type: 'vbox',
                align:'stretch'
            },
            bodyStyle: Gkh.bodyStyle,
            border: 0,
            items: [
                 {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Тип',
                    store: me.cbSuspenseAccountDistributionParametersTypes,
                    displayField: 'Display',
                    valueField: 'Value',
                    name: 'SuspenseAccountDistributionParametersType',
                    value: 10
                },
                {
                    xtype: 'combobox',
                    editable: false,
                    fieldLabel: 'Вид распределения',
                    store: B4.enums.SuspenseAccountDistributionParametersView.getStore(),
                    displayField: 'Display',
                    valueField: 'Value',
                    itemId: 'SuspenseAccountDistributionParametersView',
                    name: 'SuspenseAccountDistributionParametersView',
                    value: 10
                },
                {
                    xtype: 'textarea',
                    fieldLabel: 'Комментарий к распределению',
                    flex: 1,
                    maxLength: 500
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
                                    action: 'continuecheckin',
                                    text: 'Продолжить'
                                }
                            ]
                        }, '->', {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [{
                                xtype: 'b4closebutton',
                                listeners: {
                                    click: function (btn) {
                                        btn.up('window').close();
                                    }
                                }
                            }]
                        }
                    ]
                }
            ]

        });
        me.callParent(arguments);
    }
});