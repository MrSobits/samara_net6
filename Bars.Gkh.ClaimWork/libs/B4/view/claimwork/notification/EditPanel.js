Ext.define('B4.view.claimwork.notification.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    width: 500,
    bodyPadding: 5,
    alias: 'widget.clwnotifeditpanel',
    title: 'Уведомление',
    autoScroll: true,
    frame: true,
    requires: [
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.ux.button.AcceptMenuButton',
        'B4.view.Control.GkhButtonPrint'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'hbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'datefield',
                        labelWidth: 120,
                        labelAlign: 'right',
                        format: 'd.m.Y',
                        flex: 1
                    },
                    items: [
                        {
                            name: 'DocumentDate',
                            fieldLabel: 'Дата формирования'
                        },
                        {
                            name: 'SendDate',
                            fieldLabel: 'Дата отправки'
                        }
                    ]
                },
                {
                    xtype: 'b4filefield',
                    name: 'File',
                    fieldLabel: 'Файл',
                    labelWidth: 120,
                    labelAlign: 'right'
                },
                {
                    xtype: 'fieldset',
                    title: 'Устранение нарушений',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'anchor',
                            padding: '0 0 5 0',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateElimination',
                                    fieldLabel: 'Срок устранения',
                                    format: 'd.m.Y',
                                    labelAlign: 'right',
                                    labelWidth: 120,
                                    anchor: '50%'
                                }
                            ]
                        },
                        {
                            xtype: 'textarea',
                            name: 'EliminationMethod',
                            fieldLabel: 'Способ устранения',
                            maxLength: 1000,
                            labelWidth: 120,
                            labelAlign: 'right'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 4,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'acceptmenubutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    action: 'delete',
                                    text: 'Удалить',
                                    textAlign: 'left'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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