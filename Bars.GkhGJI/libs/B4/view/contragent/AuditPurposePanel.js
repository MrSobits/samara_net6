Ext.define('B4.view.contragent.AuditPurposePanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyStyle: Gkh.bodyStyle,
    bodyPadding: 5,
    alias: 'widget.contragentauditpurposepanel',
    title: 'Данные для плана проверок',
    requires: [
        'B4.ux.button.Save',
        'B4.view.contragent.AuditPurposeGrid'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    bodyStyle: Gkh.bodyStyle,
                    height: 50,
                    layout: {
                        type: 'hbox'
                    },
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 300,
                        labelAlign: 'right',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'LicenseDateReceipt',
                            fieldLabel: 'Дата получения лицензии'
                        },
                        {
                            name: 'RegDateInSocialUse',
                            fieldLabel: 'Дата постановки на учёт в реестре домов социального использования'
                        }
                    ]
                },
                {
                    xtype: 'auditpurposegrid',
                    height: 400
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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