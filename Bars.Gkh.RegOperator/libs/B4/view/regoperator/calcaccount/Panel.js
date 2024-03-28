Ext.define('B4.view.regoperator.calcaccount.Panel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.regopcalcaccountpanel',

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    title: 'Расчетные счета',
    requires: [
        'B4.view.regoperator.calcaccount.Grid',
        'B4.view.regoperator.persaccountmu.Grid',
        'B4.ux.button.Save'
    ],

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
            trackResetOnLoad: true
        }, me.initialConfig);

        Ext.applyIf(me, {
            dockedItems: [
                {
                    xtype: 'toolbar',
                    docked: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    handler: function() {
                                        me.fireEvent('updateme', me);
                                    }
                                }
                            ]
                        }
                    ]
                }
            ],
            items: [
                {
                    xtype: 'form',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    items: {
                        xtype: 'fieldset',
                        title: 'Общие данные по счетам',
                        items: [
                            {
                                xtype: 'panel',
                                bodyStyle: Gkh.bodyStyle,
                                border: false,
                                defaults: {
                                    xtype: 'numberfield',
                                    labelAlign: 'right',
                                    allowDecimals: true,
                                    hideTrigger: true,
                                    margin: '0 5 0 2',
                                    editable: false,
                                    decimalPrecision: 2,
                                    decimalSeparator: '.'
                                },
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                items: [
                                    {
                                        fieldLabel: 'Сальдо',
                                        labelWidth: 40,
                                        name: 'Saldo'
                                    },
                                    {
                                        fieldLabel: 'Итого по Дебету',
                                        labelWidth: 100,
                                        name: 'Debet'
                                    },
                                    {
                                        fieldLabel: 'Итого по Кредиту',
                                        labelWidth: 110,
                                        name: 'Credit'
                                    },
                                    {
                                        fieldLabel: 'Доля расходования средств, %',
                                        labelWidth: 190,
                                        name: 'ExpenditureShare'
                                    }
                                ]
                            }
                        ]
                    }
                },
                {
                    xtype: 'regopcalcaccountgrid',
                    flex: 1
                }
            ]
        });

        me.callParent(arguments);
    }
});