Ext.define('B4.view.actvisual.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Акт визуального осмотра',
    trackResetOnLoad: true,
    autoScroll: true,
    
    itemId: 'actvisualpanel',

    alias: 'widget.actvisualpanel',

    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhButtonPrint',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.FrameVerification',
        'B4.view.GjiDocumentCreateButton'
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
                        xtype: 'container'
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: 'vbox',
                            padding: '10px 15px 10px 15px',
                            bodyStyle: Gkh.bodyStyle,
                            defaults: {
                                labelAlign: 'right'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    itemId: 'tfDocumentNumber',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    maxLength: 300,
                                    labelWidth: 140,
                                    width: 295
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '0 0 5 0',
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'DocumentDate',
                                            itemId: 'dfDocumentDate',
                                            fieldLabel: 'Дата документа',
                                            format: 'd.m.Y',
                                            allowBlank: false,
                                            labelWidth: 140,
                                            width: 295
                                        },
                                        {
                                            xtype: 'timefield',
                                            name: 'Time',
                                            valueField: 'time',        
                                            format: 'H:i',
                                            fieldLabel: 'Время составления документа',
                                            labelWidth: 200,
                                            width: 280
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'form',
                            title: 'Реквизиты',
                            border: false,
                            padding: 10,
                            //margins: -1,
                            frame: true,
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'Inspectors',
                                    fieldLabel: 'Инспекторы',
                                    labelWidth: 150,
                                    labelAlign: 'right',
                                    width: 500
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'RealityObject',
                                            fieldLabel: 'Адрес',
                                            labelWidth: 150,
                                            flex: 3,
                                            editable: false,
                                            isGetOnlyIdProperty: true,
                                            textProperty: 'Address',
                                            store: 'B4.store.RealityObject',
                                            columns: [
                                                {
                                                    dataIndex: 'Municipality',
                                                    flex: 1,
                                                    text: 'Муниципальное образование',
                                                    filter: {
                                                        xtype: 'b4combobox',
                                                        operand: CondExpr.operands.eq,
                                                        storeAutoLoad: false,
                                                        hideLabel: true,
                                                        editable: false,
                                                        valueField: 'Name',
                                                        emptyItem: { Name: '-' },
                                                        url: '/Municipality/ListWithoutPaging'
                                                    }
                                                },
                                                { flex: 1, dataIndex: 'Address', filter: { xtype: 'textfield' } }
                                            ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Flat',
                                            fieldLabel: 'Квартира',
                                            maxLength: 10,
                                            labelWidth: 100,
                                            flex: 1,
                                            regex: /^\d+(\s*([А-Яа-я]{0,1}))?$/,
                                            regexText: 'В это поле можно вводить только цифры и одну букву(кириллица)! (Пример: 45А)'
                                        }                               
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'FrameVerification',
                                    fieldLabel: 'Основание проверки',
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    flex: 3,
                                    editable: false,
                                    isGetOnlyIdProperty: true,
                                    textProperty: 'Name',
                                    store: 'B4.store.dict.FrameVerification',
                                    columns: [
                                        { dataIndex: 'Name', flex: 1, text: 'Рамки проверки', filter: { xtype: 'textfield' } },
                                        { dataIndex: 'Code', flex: 1, text: 'Код', filter: { xtype: 'textfield' } }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Результат проверки',
                            padding: 10,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    fieldLabel: 'Результат проверки',
                                    name: 'InspectionResult',
                                    maxLength: 2000,
                                    labelAlign: 'right',
                                    labelWidth: 120
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            title: 'Вывод',
                            padding: 10,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'textarea',
                                    fieldLabel: 'Вывод',
                                    name: 'Conclusion',
                                    maxLength: 2000,
                                    labelAlign: 'right',
                                    labelWidth: 120
                                }
                            ]
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