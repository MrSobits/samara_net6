Ext.define('B4.view.preventiveaction.EditPanel', {
    extend: 'Ext.form.Panel',
    
    requires: [
        'B4.view.preventiveaction.MainInfoTabPanel',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.YesNo',
        'B4.ux.button.Save'
    ],
    
    title: 'Профилактическое мероприятие',
    itemId: 'preventiveActionEditPanel',
    autoscroll: true,
    trackResetOnLoad: true,
    
    initComponent: function(){
        var me = this;
        
        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            layout: {
                type: 'vbox',
                align: 'stretch'
            },
            defaults: {
                xtype: 'container',
                layout: {
                    type: 'hbox',
                    align: 'middle',
                }
            },
            items: [
                {
                    margin: '5 10 5 10',
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 130,
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            dateFormat: 'dd.MM.YYYY',
                            fieldLabel: 'Дата',
                            width: 180,
                            labelWidth: 50,
                            allowBlank: false,
                            editable: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер документа',
                            flex: 1,
                            maxLength: 50,
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'MunicipalityName',
                            fieldLabel: 'Муниципальное</br>образование',
                            flex: 2,
                            readOnly: true
                        },
                        {
                            xtype: 'b4enumcombo',
                            name: 'SentToErknm',
                            enumName: 'B4.enums.YesNo',
                            fieldLabel: 'Отправлено в ЕРКНМ',
                            labelWidth: 150,
                            flex: 0.8,
                            hidden: true,
                            readOnly: true
                        }
                    ]
                },
                {
                    defaults: {
                        labelAlign: 'right'
                    },
                    margin: '0 10 10 10',
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocumentYear',
                            fieldLabel: 'Год',
                            width: 180,
                            labelWidth: 50,
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            flex: 1,
                            labelWidth: 130,
                            maxLength: 50,
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'GjiContragentName',
                            fieldLabel: 'Орган ГЖИ',
                            labelWidth: 130,
                            flex: 2, 
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'preventiveActionTabPanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'preventiveactionmaininfotabpanel'
                        }
                    ]
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGJIToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-decline',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                }
                            ]
                        },
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            itemId: 'statusButtonGroup',
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус',
                                    menu: []
                                }
                            ]
                        }
                        ]
                }]
        });
        
        me.callParent(arguments);
    }
});