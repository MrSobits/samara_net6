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
        'B4.view.protocolgji.RequirementGrid',
        'B4.view.GjiDocumentCreateButton',
        'B4.ux.form.field.TabularTextArea'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: 'form',
                    split: false,
                    collapsible: false,
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    defaults: {
                        labelWidth: 170,
                        border: false,
                        layout: 'hbox',
                        xtype: 'panel'
                    },
                    items: [
                        {
                            xtype: 'panel',
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
                                    labelWidth: 200,
                                    width: 320
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    labelWidth: 140,
                                    maxLength: 300,
                                    width: 295
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            padding: '10px 15px 5px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DateOfProceedings',
                                    fieldLabel: 'Дата и время расмотрения дела:',
                                    format: 'd.m.Y',
                                    labelWidth: 200,
                                    width: 320
                                },
                                {
                                    xtype: 'numberfield',
                                    itemId: 'HourOfProceedings',
                                    name: 'HourOfProceedings',
                                    margin: '0 0 0 33',
                                    fieldLabel: '',
                                    labelWidth: 25,
                                    width: 65,
                                    maxValue: 23,
                                    minValue: 0
                                },
                                {
                                    xtype: 'numberfield',
                                    itemId: 'MinuteOfProceedings',
                                    name: 'MinuteOfProceedings',
                                    fieldLabel: ' час ',
                                    labelSeparator: '',
                                    labelWidth: 25,
                                    width: 85,
                                    maxValue: 59,
                                    minValue: 0
                                },
                                {
                                    xtype: 'label',
                                    text: 'мин',
                                    margin: '5 0 0 5'
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            padding: '0 15px 20px 15px',
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
                                    labelWidth: 200,
                                    width: 320
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
                            xtype: 'panel',
                            title: 'Установил',
                            bodyStyle: Gkh.bodyStyle,
                            bodyPadding: '5',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                xtype: 'tabtextarea'
                            },
                            items: [
                                {
                                    name: 'DescriptionSet',
                                    fieldLabel: 'Установил',
                                    maxLength: 2000
                                },
                                {
                                    name: 'Description',
                                    fieldLabel: 'Подробнее',
                                    maxLength: 2000,
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'protocolgjiDefinitionGrid',
                            flex: 1
                        },
                        {
                            xtype: 'protocolgjiAnnexGrid',
                            flex: 1
                        },
                        {
                            xtype: 'protocolrequirementgrid',
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