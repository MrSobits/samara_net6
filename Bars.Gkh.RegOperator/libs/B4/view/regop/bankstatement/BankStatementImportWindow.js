Ext.define('B4.view.regop.bankstatement.BankStatementImportWindow', {
    extend: 'B4.form.Window',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    height: 200,
    width: 400,
    bodyPadding: 5,
    itemId: 'bankStatementImportWindow',
    title: 'Импорт',
    trackResetOnLoad: true,
    resizable: false,

    requires: [
        'B4.form.FileField',
        'B4.ux.button.Close',
        'B4.ux.button.Save'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 50,
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'FileImport',
                            fieldLabel: 'Файл',
                            allowBlank: false,
                            itemId: 'fileImport'
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'center'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 200,
                    },
                    items: [
                        {
                            xtype: 'checkbox',
                            name: 'WithoutRegister',
                            itemId: 'cbWithoutRegister',
                            fieldLabel: 'Не создавать реестр платежных агентов'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'OutcomesOnly',
                            itemId: 'cbOutcomesOnly',
                            fieldLabel: 'Импортировать только расход'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'DistributeOnPenalty',
                            itemId: 'cbDistributeOnPenalty',
                            fieldLabel: 'Распределять на пеню'
                        }
                    ]
                },
                {
                    xtype: 'displayfield',
                    itemId: 'log'
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
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4closebutton'
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
