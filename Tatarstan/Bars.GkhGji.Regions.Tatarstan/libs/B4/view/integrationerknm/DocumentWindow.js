Ext.define('B4.view.integrationerknm.DocumentWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.form.EnumCombo',
        'B4.enums.TypeDocumentGji',
        'B4.ux.button.Close',
        'B4.ux.button.Update',
        'B4.view.baseintegration.RisTaskGrid'
    ],

    alias: 'widget.integrationerknmdocumentwindow',

    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 1300,
    height: 720,
    bodyPadding: 5,

    title: 'Документ',
    closeAction: 'destroy',
    trackResetOnLoad: true,
    maximizable: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'hbox',
                    align: 'stretch',
                    padding: 2,
                    items: [
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            fieldLabel: 'Номер документа',
                            name: 'DocumentNumber',
                            labelWidth: 140,
                            flex: 1.5,
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            fieldLabel: 'Дата документа',
                            name: 'DocumentDate',
                            format: 'd.m.Y',
                            labelWidth: 160,
                            flex: 1,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    align: 'stretch',
                    padding: 2,
                    items: [
                        {
                            xtype: 'b4enumcombo',
                            name: 'DocumentType',
                            fieldLabel: 'Тип документа',
                            enumName: 'B4.enums.TypeDocumentGji',
                            labelAlign: 'right',
                            labelWidth: 140,
                            flex: 1.5,
                            readOnly: true
                        },
                        {
                            xtype: 'datefield',
                            labelAlign: 'right',
                            fieldLabel: 'Дата присвоения идентификатора в ЕРКНМ',
                            name: 'ErknmRegistrationDate',
                            format: 'd.m.Y',
                            labelWidth: 160,
                            flex: 1,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    align: 'stretch',
                    padding: 2,
                    items: [
                        {
                            xtype: 'textfield',
                            labelAlign: 'right',
                            fieldLabel: 'Идентификатор в ЕРКНМ',
                            name: 'ErknmGuid',
                            labelWidth: 140,
                            flex: 1.5,
                            readOnly: true
                        },
                        {
                            xtype: 'container',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'integrationristaskgrid',
                    padding: 2,
                    align: 'stretch',
                    flex: 1
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'button',
                            text: 'Перейти к документу',
                            name: 'goToDocumentButton'
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton',
                                    listeners: {
                                        click: function() {
                                            var store = me.down('integrationristaskgrid').getStore();
                                            if (store) {
                                                store.load();
                                            }
                                        }
                                    }
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function() {
                                            me.close();
                                        }
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