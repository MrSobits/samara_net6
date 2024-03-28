Ext.define('B4.view.efficiencyrating.manorg.EditPanel', {
    extend: 'Ext.form.Panel',
    requires: [
        'B4.ux.button.Update',

        'B4.view.efficiencyrating.manorg.FactorPanel'
    ],

    title: 'Рейтинг эффективности',
    alias: 'widget.efManorgEditPanel',
    closable: true,

    layout: { type: 'vbox', align: 'stretch' },
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,

    initComponent: function() {
        var me = this;

        me.initialConfig = Ext.apply({
                trackResetOnLoad: true
            },
            me.initialConfig);

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'form',
                    bodyPadding: 10,
                    margin: '0 5px 0 5px',
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        xtype: 'numberfield',
                        width: 500,
                        labelWidth: 200,
                        labelAlign: 'right',
                        decimals: true,
                        decimalPrecision: 2,
                        decimalSeparator: ','
                    },
                    items: [
                        {
                            fieldLabel: 'Показатель эффективности',
                            name: 'Rating',
                            readOnly: true
                        },
                        {
                            fieldLabel: 'Динамика',
                            name: 'Dynamics',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'efManorgFactorPanel',
                    flex: 1
                }
            ],

            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Рассчитать показатели',
                                    actionName: 'calcvalues'
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