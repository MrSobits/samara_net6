Ext.define('B4.view.pretensionqualitywork.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    autoScroll: true,
    title: 'Претензии по качеству работ',
    alias: 'widget.pretensionqualityworkeditpanel',
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 300,
                labelAlign: 'right',
                hideTrigger: true,
                minValue: 0
            },
            items: [
                {
                    xtype: 'numberfield',
                    allowDecimals: false,
                    name: 'ReceivedPretensionCount',
                    fieldLabel: 'Количество поступивших претензий (ед.)'
                },
                {
                    xtype: 'numberfield',
                    allowDecimals: false,
                    name: 'ApprovedPretensionCount',
                    fieldLabel: 'Количество удовлетворенных претензий (ед.)'
                },
                {
                    xtype: 'numberfield',
                    allowDecimals: false,
                    name: 'NoApprovedPretensionCount',
                    fieldLabel: 'Количество претензий, в удовлетворении которых отказано (ед.)'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'PretensionRecalcSum',
                    fieldLabel: 'Сумма произведенного перерасчета (руб.)'
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
                                    xtype: 'b4savebutton'
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
