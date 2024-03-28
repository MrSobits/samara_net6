Ext.define('B4.view.preventiveaction.visit.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'visitEditPanel',
    title: 'Лист визита',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.enums.YesNoNotSet',
        'B4.enums.AcquaintState',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.preventiveaction.visit.AnnexGrid',
        'B4.view.preventiveaction.visit.InfoProvidedGrid',
        'B4.view.preventiveaction.visit.ViolationInfoGrid',
        'B4.store.dict.Inspector'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    layout: 'hbox',
                    defaults: {
                        border: false,
                        shrinkWrap: true,
                        margin: 6,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            name: 'DocumentDate',
                            fieldLabel: 'Дата',
                            format: 'd.m.Y',
                            allowBlank: false,
                            labelWidth: 30,
                            width: 200
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentYear',
                            fieldLabel: 'Год',
                            labelWidth: 30,
                            width: 200,
                            maxLength: 4,
                            regex: /^\d*$/,
                            regexText: 'Данное поле может содержать только цифры!'
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNumber',
                            fieldLabel: 'Номер документа',
                            maxLength: 50,
                            labelWidth: 110,
                            width: 280,
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentNum',
                            fieldLabel: 'Номер',
                            labelWidth: 40,
                            width: 210,
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'actActionIsolatedTabPanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false,
                        flex: 1,
                    },
                    items: [
                        {
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            title: 'Основная информация',
                            border: false,
                            bodyPadding: 10,
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        allowBlank: false,
                                        labelAlign: 'right',
                                        flex: 1,
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                flex: 1,
                                                labelAlign: 'right',
                                                labelWidth: 190,
                                                padding: '0 5 0 0'
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4selectfield',
                                                    store: 'B4.store.dict.Inspector',
                                                    name: 'ExecutingInspector',
                                                    fieldLabel: 'Визит проведен',
                                                    textProperty: 'Fio',

                                                    columns: [
                                                        {
                                                            header: 'ФИО',
                                                            xtype: 'gridcolumn',
                                                            dataIndex: 'Fio',
                                                            flex: 1,
                                                            filter: { xtype: 'textfield' }
                                                        },
                                                        {
                                                            header: 'Должность',
                                                            xtype: 'gridcolumn',
                                                            dataIndex: 'Position',
                                                            flex: 1,
                                                            filter: { xtype: 'textfield' }
                                                        }
                                                    ],
                                                    dockedItems: [
                                                        {
                                                            xtype: 'b4pagingtoolbar',
                                                            displayInfo: true,
                                                            store: 'B4.store.dict.Inspector',
                                                            dock: 'bottom'
                                                        }
                                                    ],
                                                    editable: false,
                                                    allowBlank: false
                                                },
                                                {
                                                    xtype: 'b4enumcombo',
                                                    name: 'HasCopy',
                                                    fieldLabel: 'Экземпляр листа визита получен',
                                                    enumName: 'B4.enums.YesNoNotSet'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                flex: 1,
                                                labelAlign: 'right',
                                                labelWidth: 210,
                                                padding: '0 0 0 5',
                                                allowBlank: false
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    name: 'VisitDateStart',
                                                    fieldLabel: 'Дата начала проведения визита',
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    name: 'VisitDateEnd',
                                                    fieldLabel: 'Дата окончания проведения визита',
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'timefield',
                                                        increment: 30,
                                                        format: 'H:i',
                                                        labelAlign: 'right',
                                                        allowBlank: false,
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            name: 'VisitTimeStart',
                                                            fieldLabel: 'Время проведения визита с',
                                                            labelWidth: 210,
                                                        },
                                                        {
                                                            name: 'VisitTimeEnd',
                                                            fieldLabel: 'по',
                                                            labelWidth: 30,
                                                            flex: 0.65
                                                        }
                                                    ]
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'Предоставленная информация',
                                    name: 'SuspensionInspection',
                                    items: [
                                        {
                                            xtype: 'infoprovidedgrid',
                                            height: 610
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'visitviolationinfogrid'
                        },
                        {
                            xtype: 'visitannexgrid'
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    itemId: 'documentGjiToolBar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            itemId: 'mainButtonGroup',
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-decline',
                                    text: 'Отмена',
                                    textAlign: 'left',
                                    itemId: 'btnCancel'
                                },
                                {
                                    xtype: 'gjidocumentcreatebutton'
                                },
                                {
                                    xtype: 'gkhbuttonprint'
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
                }
            ]
        });

        me.callParent(arguments);
    }
});