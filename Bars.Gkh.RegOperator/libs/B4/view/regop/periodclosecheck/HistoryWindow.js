Ext.define('B4.view.regop.periodclosecheck.HistoryWindow', {
        extend: 'B4.form.Window',

        alias: 'widget.periodclosecheckhistorywindow',

        requires: [
            'B4.ux.button.Close',
            'B4.view.regop.periodclosecheck.HistoryGrid'
        ],

        modal: true,
        closeAction: 'destroy',
        layout: 'vbox',
        width: 800,
        height: 600,
        bodyPadding: 5,
        title: 'История изменений',

        initComponent: function () {
            var me = this;

            Ext.applyIf(me, {
                defaults: {
                    labelAlign: 'right',
                    labelWidth: 100
                },
                items: [
                    {
                        xtype: 'textfield',
                        name: 'Code',
                        fieldLabel: 'Код',
                        readOnly: true,
                        width: '100%'
                    },
                    {
                        xtype: 'textfield',
                        name: 'Name',
                        fieldLabel: 'Наименование',
                        readOnly: true,
                        width: '100%'
                    },
                    {
                        xtype: 'periodclosecheckhistorygrid',
                        width: '100%',
                        flex: 1
                    }
                ],
                dockedItems: [
                    {
                        xtype: 'toolbar',
                        dock: 'top',
                        items: [
                            {
                                xtype: 'tbfill'
                            },
                            {
                                xtype: 'buttongroup',
                                columns: 2,
                                items: [
                                    {
                                        xtype: 'b4closebutton',
                                        handler: function(b) {
                                            b.up('window').close();
                                        }
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