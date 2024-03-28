Ext.define('B4.view.direalobjclaimwork.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    bodyPadding: 2,
    autoScroll: true,
    title: 'Претензионно-исковая работа',
    alias: 'widget.direalobjclaimworkeditpanel',
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
                    name: 'SentPretensionCount',
                    fieldLabel: 'Направлено претензий потребителям-должникам (ед.)',
                },
                {
                    xtype: 'numberfield',
                    allowDecimals: false,
                    name: 'SentPetitionCount',
                    fieldLabel: 'Направлено исковых заявлений (ед.)'
                },
                {
                    xtype: 'gkhdecimalfield',
                    name: 'ReceiveSumByClaimWork',
                    fieldLabel: 'Получено денежных средств по результатам претензионно-исковой работы (руб.)'
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
