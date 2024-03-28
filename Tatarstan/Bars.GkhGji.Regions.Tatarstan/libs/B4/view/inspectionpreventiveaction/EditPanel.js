Ext.define('B4.view.inspectionpreventiveaction.EditPanel', {
    extend: 'Ext.form.Panel',
    closable: true,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    itemId: 'inspectionPreventiveActionEditPanel',
    title: 'Проверка по профилактическому мероприятию',
    trackResetOnLoad: true,
    autoScroll: true,
    bodyStyle: Gkh.bodyStyle,
    requires: [
        'B4.form.EnumCombo',
        'B4.form.SelectField',

        'B4.ux.button.Save',
        'B4.ux.button.Add',

        'B4.enums.TypeJurPerson',
        'B4.enums.TypeObjectAction',
        'B4.enums.TatarstanInspectionFormType',

        'B4.view.GjiDocumentCreateButton',
        'B4.view.inspectionpreventiveaction.RealityObjectGrid',
        'B4.view.inspectionpreventiveaction.JointInspectionGrid',

        'B4.store.dict.RiskCategory'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        labelWidth: 150,
                        flex: 1,
                        margin: 2
                    },
                    items: [
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch',
                                padding: '5 0 0 0'
                            },
                            defaults: {
                                labelAlign: 'right',
                                allowBlank: false,
                                labelWidth: 150,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
                                    fieldLabel: 'Номер',
                                    maxLength: 25
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'CheckDate',
                                    fieldLabel: 'Дата проверки'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                flex: 1,
                                allowBlank: false,
                                labelWidth: 150,
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeJurPerson',
                                    fieldLabel: 'Тип контрагента',
                                    storeAutoLoad: true,
                                    readOnly: true,
                                    enumName: 'B4.enums.TypeJurPerson'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeObject',
                                    fieldLabel: 'Объект проверки',
                                    readOnly: true,
                                    storeAutoLoad: true,
                                    enumName: 'B4.enums.TypeObjectAction',
                                    allowBlank: false
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            labelAlign: 'right',
                            name: 'JurPerson',
                            textProperty: 'JurPerson',
                            fieldLabel: 'Юридическое лицо',
                            readOnly: true,
                            editable: false,
                            allowBlank: false,
                            store: 'B4.store.Contragent',
                            columns: [
                                {
                                    text: 'Наименование',
                                    dataIndex: 'ShortName',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                },
                                {
                                    text: 'Муниципальное образование',
                                    dataIndex: 'Municipality',
                                    flex: 1,
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
                                { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                            ]
                        },
                        {
                            xtype: 'container',
                            layout: {
                                type: 'hbox',
                                align: 'stretch', 
                                padding: '0 0 5 0'
                            },
                            defaults: {
                                labelAlign: 'right',
                                allowBlank: false,
                                labelWidth: 150,
                                readOnly: true,
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PreventiveAction',
                                    labelAlign: 'right',
                                    fieldLabel: 'Мероприятие',
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeForm',
                                    fieldLabel: 'Форма проверки',
                                    itemId: 'cbFormCheck',
                                    storeAutoLoad: true,
                                    enumName: 'B4.enums.TatarstanInspectionFormType'
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Категории риска',
                            name: 'RiskSet',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    defaults: {
                                        labelWidth: 140,
                                        labelAlign: 'right'
                                    },
                                    layout: 'hbox',
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'RiskCategory',
                                            fieldLabel: 'Категория',
                                            store: 'B4.store.dict.RiskCategory',
                                            editable: false,
                                            flex: 1,
                                            columns: [
                                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                            ]
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'RiskCategoryStartDate',
                                            minWidth: 275,
                                            fieldLabel: 'Дата начала'
                                        }
                                    ]
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: false,
                    defaults: {
                        border: false,
                        bodyStyle: 'backrgound-color:transparent;'
                    },
                    items: [
                        {
                            xtype: 'inspectionpreventiveactionrealityobjgrid',
                        },
                        {
                            xtype: 'inspectionpreventiveactionjointinspectiongrid'
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