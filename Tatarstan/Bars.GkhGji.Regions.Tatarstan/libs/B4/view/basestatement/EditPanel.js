Ext.define('B4.view.basestatement.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    itemId: 'baseStatementEditPanel',
    title: 'Проверка по обращению граждан',
    trackResetOnLoad: true,
    autoScroll: true,
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.store.dict.RevenueSourceGji',
        'B4.store.dict.RevenueFormGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhIntField',
        'B4.store.Contragent',
        'B4.enums.TatarstanInspectionFormType',
        'B4.enums.FormCheck',
        'B4.enums.TypeJurPerson',
        'B4.enums.ReasonErpChecking',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.basestatement.ContragentGrid'
    ],

    initComponent: function() {
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
                    bodyPadding: 5,
                    items: [
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
                                    itemId: 'tfInspectionNumber',
                                    fieldLabel: 'Номер',
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    width: 230,
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '5 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                xtype: 'combobox',
                                editable: false,
                                displayField: 'Display',
                                valueField: 'Value',
                                flex: 1
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'b4combobox',
                                    name: 'TypeJurPerson',
                                    fieldLabel: 'Тип юридического лица',
                                    displayField: 'Display',
                                    valueField: 'Id',
                                    itemId: 'cbTypeJurPerson',
                                    editable: false,
                                    storeAutoLoad: true,
                                    url: '/Inspection/ListJurPersonTypes'
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'PersonInspection',
                                    fieldLabel: 'Объект проверки',
                                    displayField: 'Display',
                                    itemId: 'cbPersonInspection',
                                    editable: false,
                                    storeAutoLoad: true,
                                    valueField: 'Id',
                                    url: '/Inspection/ListPersonInspection'
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
                            labelAlign: 'right',
                            labelWidth: 150,
                            name: 'Contragent',
                            textProperty: 'ShortName',
                            fieldLabel: 'Юридическое лицо',
                            editable: false,
                            itemId: 'sfContragent',
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
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                editable: false
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'textfield',
                                    labelAlign: 'right',
                                    labelWidth: 150,
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'ФИО',
                                    itemId: 'tfPhysicalPerson',
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    maxLength: 12
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1,
                                editable: false
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'appealCitizens',
                                    itemId: 'trigfAppealCitizens',
                                    fieldLabel: 'Обращение(я)'
                                },
                                {
                                    xtype: 'gkhtriggerfield',
                                    name: 'motivationConclusions',
                                    fieldLabel: 'Мотивировочное(ые) заключение(я)',
                                    disabled: true,
                                    hidden: true
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'TypeForm',
                                    fieldLabel: 'Форма проверки',
                                    itemId: 'cbFormCheck',
                                    storeAutoLoad: true,
                                    allowBlank: false,
                                    enumName: 'B4.enums.TatarstanInspectionFormType',
                                    readOnly: true
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
                                            allowBlank: true,
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
                                },
                                {
                                    xtype: 'container',
                                    defaults: {
                                        labelWidth: 140,
                                        labelAlign: 'right',
                                        margin: '10 0 0 50',
                                        cls: 'alert-hyperlink'
                                    },
                                    layout: {
                                        type: 'hbox',
                                        pack: 'end'
                                    },
                                    items: [
                                        {
                                            xtype: 'component',
                                            name: 'AllCategory',
                                            autoEl: {
                                                tag: 'span',
                                                html: 'Все категории юридического лица'
                                            }
                                        },
                                        {
                                            xtype: 'component',
                                            name: 'PrevCategory',
                                            autoEl: {
                                                tag: 'span',
                                                html: 'Предыдущие категории'
                                            }
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'CheckDate',
                                    fieldLabel: 'Дата проверки'
                                },
                                { xtype: 'component' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        border: false
                    },
                    items: [
                        {
                            xtype: 'baseStatementRealityObjGrid',
                            bodyStyle: 'backrgound-color:transparent;'
                        },
                        {
                            xtype: 'basestatementcontragentgrid'
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
                                    textAlign: 'right',
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