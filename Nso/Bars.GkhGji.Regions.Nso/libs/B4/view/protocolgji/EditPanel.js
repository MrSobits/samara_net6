Ext.define('B4.view.protocolgji.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
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
        'B4.view.protocolgji.BaseDocumentGrid'
    ],
    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        layout: 'hbox',
                        xtype: 'panel',
                        shrinkWrap: true
                    },
                    shrinkWrap: true,
                    overflowY: 'hidden',
                    overflowX: 'hidden',
                    id: 'protocolTopPanel',
                    padding: 5,
                    items: [
                        {
                            padding: '5px 15px 5px 15px',
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
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FormatPlace',
                                    fieldLabel: 'Место и время составления протокола:',
                                    format: 'd.m.Y',
                                    labelWidth: 240,
                                    width: 600,
                                    maxLength: 500
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'FormatHour',
                                    margin: '0 0 0 10',
                                    fieldLabel: '',
                                    labelWidth: 25,
                                    width: 45,
                                    maxValue: 23,
                                    minValue: 0
                                },
                                {
                                    xtype: 'label',
                                    text: ':',
                                    margin: '5'
                                },
                                {
                                    xtype: 'numberfield',
                                    name: 'FormatMinute',
                                    width: 45,
                                    maxValue: 59,
                                    minValue: 0
                                }

                            ]
                        },
                        {
                            padding: '5px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                xtype: 'gkhintfield',
                                hideTrigger: true
                            },
                            items: [
                                {
                                    name: 'DocumentYear',
                                    fieldLabel: 'Год',
                                    itemId: 'nfDocumentYear',
                                    labelWidth: 50,
                                    width: 200
                                },
                                {
                                    name: 'DocumentNum',
                                    fieldLabel: 'Номер',
                                    itemId: 'nfDocumentNum',
                                    labelWidth: 140,
                                    width: 295,
                                    hideTrigger: true
                                },
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
                    autoScroll: true,
                    defaults: {
                        minWidth: 1400
                    },
                    listeners: {
                        render: function (p) {
                            p.body.on('scroll', function (e) {
                                var elementDisposalTopPanel = Ext.getCmp('protocolTopPanel').body.dom;
                                elementDisposalTopPanel.scrollLeft = e.target.scrollLeft;
                                elementDisposalTopPanel.scrollTop = e.target.scrollTop;
                            }, p);
                        }
                    },
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
                            xtype: 'protocolgjiArticleLawGrid',
                            flex: 1
                        },
                        {
                            xtype: 'protocolgjiDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'panel',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Деятельность',
                            border: false,
                            bodyPadding: 5,
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 170
                            },
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'protocolgjiDirections',
                                    itemId: 'protocolgjiDirectionsTrigerField',
                                    fieldLabel: 'Направление деятельности',
                                    allowBlank: true
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.dict.NormativeDoc',
                                    fieldLabel: 'Правовое основание',
                                    name: 'NormativeDoc',
                                    itemId: 'sfNormativeDoc',
                                    hidden: true
                                },
                                {
                                    xtype: 'protocolgjiBaseDocumentGrid',
                                    flex: 1,
                                    hidden: true
                                }
                            ]
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