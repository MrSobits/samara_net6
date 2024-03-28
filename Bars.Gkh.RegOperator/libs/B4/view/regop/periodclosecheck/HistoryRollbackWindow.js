Ext.define('B4.view.regop.periodclosecheck.HistoryRollbackWindow', {
        extend: 'B4.form.Window',

        alias: 'widget.periodcloserollbackhistorywindow',

        requires: [
            'B4.ux.button.Save',
            'B4.ux.button.Close'
        ],

        modal: true,
        closeAction: 'destroy',
        layout: 'vbox',
        width: 600,
        height: 200,
        bodyPadding: 5,
        title: 'Откат периода',

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 250
                },
                items: [
                    {
                        xtype: 'checkbox',
                        name: 'IsDeleteSnapshot',
                        fieldLabel: 'Удалять слепки предыдущего периода'
                    },
                    {
                        xtype: 'textarea',
                        name: 'Reason',
                        fieldLabel: 'Причина',
                        allowBlank: false,
                        width: 500
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'b4savebutton',
                                fieldLabel: 'Применить'
                            },
                            {
                                xtype: 'tbfill'
                            },
                            {
                                xtype: 'b4closebutton',
                                handler: function (b) {
                                    b.up('window').close();
                                }
                            }
                        ]
                    }
                ]
            });

            me.callParent(arguments);
        }
    });