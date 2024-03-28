Ext.define('B4.view.protocolgji.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: {type: 'vbox', align: 'stretch'},
    itemId: 'protocolgjiEditPanel',
    title: 'Протокол',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.protocolgji.RequisitePanel',
        'B4.view.protocolgji.RealityObjListPanel',
        'B4.view.protocolgji.ArticleLawGrid',
        'B4.view.protocolgji.DefinitionGrid',
        'B4.view.protocolgji.AnnexGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.ux.form.field.TabularTextArea'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: 'hbox',
                    autoScroll: true,
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        layout: 'vbox',
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    items: [
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false,
                                    labelWidth: 50,
                                    width: 200
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    maxLength: 4,
                                    labelWidth: 50,
                                    width: 200
                                }
                            ]
                        },
                        {
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'LiteralNum',
                                    itemId: 'nfLiteralNum',
                                    fieldLabel: 'Буквенный подномер',
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    name: 'DocumentSubNum',
                                    itemId: 'nfDocumentSubNum',
                                    fieldLabel: 'Подномер',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    itemId: 'protocolTabPanel',
                    items: [
                        {
                            xtype: 'protocolgjiRequisitePanel',
                            flex: 1
                        },
                        {
                            xtype: 'protocolgjiRealityObjListPanel',
                            flex: 1
                        },
                        {
                            xtype: 'panel',
                            autoScroll: true,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            flex: 1,
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Описание нарушений',
                            border: false,
                            bodyPadding: 5,
                            defaults: {
                                labelAlign: 'top',
                                minHeight: 250,
                                minWidth: 600
                            },
                            shrinkWrap: true,
                            items: [
                                {
                                    xtype: 'textarea',
                                    name: 'ViolationDescription',
                                    fieldLabel: 'Описание нарушений'
                                },
                                {
                                    xtype: 'textarea',
                                    name: 'ExplanationsComments',
                                    fieldLabel: 'Объяснения, заявления, замечания'
                                }
                            ]
                        },
                        {
                            xtype: 'protocolgjiArticleLawGrid',
                            flex: 1
                        },
                        {
                            xtype: 'protocolgjiDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'protocolgjiAnnexGrid',
                            flex: 1
                        }
                    ]
                }
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
                                    iconCls: 'icon-accept',
                                    text: 'Отменить',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                //ToDo ГЖИ после перехода на правила необходимо удалить
                                /*
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    text: 'Сформировать',
                                    itemId: 'btnCreateDocument',
                                    menu: [
                                        {
                                            text: 'Постановление',
                                            textAlign: 'left',
                                            itemId: 'btnCreateProtocolToResolution',
                                            actionName: 'createProtocolToResolution'
                                        }
                                    ]
                                },
                                */
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-delete',
                                    text: 'Удалить',
                                    textAlign: 'left',
                                    itemId: 'btnDelete'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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
                }
            ]
        });

        me.callParent(arguments);
    }
});