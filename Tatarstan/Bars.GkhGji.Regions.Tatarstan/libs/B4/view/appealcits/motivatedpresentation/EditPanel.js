Ext.define('B4.view.appealcits.motivatedpresentation.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.dict.Inspector',
        'B4.enums.MotivatedPresentationType',
        'B4.enums.MotivatedPresentationResultType',
        'B4.view.Control.GkhTriggerField',
        'B4.view.appealcits.motivatedpresentation.AnnexGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'motivatedPresentationAppealCitsEditPanel',
    title: 'Мотивированное представление',
    trackResetOnLoad: true,
    closable: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    padding: '5 0 0 0',
                    defaults: {
                        padding: '0 0 5 0'
                    },
                    border: false,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
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
                                        width: 200
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            fieldLabel: 'Дата',
                                            format: 'd.m.Y'
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
                                        labelAlign: 'right',
                                        labelWidth: 140,
                                        width: 295
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер документа'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    itemId: 'motivatedPresentationTabPanel',
                    border: false,
                    flex: 1,
                    items: [
                        {
                            xtype: 'panel',
                            autoScroll: true,
                            title: 'Реквизиты',
                            bodyStyle: Gkh.bodyStyle,
                            bodyPadding: 8,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Basement',
                                                    fieldLabel: 'Основание',
                                                    value: 'Обращение гражданина',
                                                    labelWidth: 140,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'AppealCitsFormatted',
                                                    fieldLabel: 'Обращение гражданина',
                                                    labelWidth: 150,
                                                    allowBlank: false,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 140,
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    xtype: 'b4enumcombo',
                                                    fieldLabel: 'Вид мотивированного представления',
                                                    enumName: 'B4.enums.MotivatedPresentationType',
                                                    name: 'PresentationType'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Кем вынесено',
                                    margin: '10 0 0 0',
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            itemId: 'tfInspectors',
                                            fieldLabel: 'Инспектор',
                                            modalWindow: true,
                                            editable: false,
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Кому направлено (адресовано)',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            store: 'B4.store.dict.Inspector',
                                            textProperty: 'Fio',
                                            name: 'Official',
                                            fieldLabel: 'Должностное лицо',
                                            labelAlign: 'right',
                                            labelWidth: 130,
                                            columns: [
                                                { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                                { header: 'Код', xtype: 'gridcolumn', dataIndex: 'Code', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            onStoreBeforeLoad: function (store, operation) {
                                                operation.params = operation.params || {};
                                                operation.params.headOnly = true;
                                            }
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    title: 'Результат',
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Результат',
                                            enumName: 'B4.enums.MotivatedPresentationResultType',
                                            name: 'ResultType',
                                            labelAlign: 'right',
                                            labelWidth: 130
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'motivatedpresentationappealcitsannexgrid'
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