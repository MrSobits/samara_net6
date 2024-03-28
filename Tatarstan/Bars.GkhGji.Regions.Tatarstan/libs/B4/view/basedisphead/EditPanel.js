Ext.define('B4.view.basedisphead.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseDispHeadEditPanel',
    title: 'Проверка по поручению руководства',
    autoScroll: true,

    requires: [
        'B4.DisposalTextValues',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.enums.ReasonErpChecking',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.view.basedisphead.RealityObjectGrid',
        'B4.view.basedisphead.MainInfoTabPanel',
        'B4.view.Control.GkhTriggerField',
        'B4.view.Control.GkhIntField',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.basedisphead.ContragentGrid',
        'B4.store.Contragent',
        'B4.store.dict.Inspector',
        'B4.store.dict.RiskCategory',
        'B4.store.DocumentGji',
        'B4.ux.button.Save',
        'B4.ux.button.Add'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyPadding: 5,
                    autoScroll: true,
                    bodyStyle: Gkh.bodyStyle,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    border: false,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 150,
                                labelAlign: 'right',
                                flex: 1
                            },
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DispHeadDate',
                                    itemId: 'dfDispHeadDate',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
                                    itemId: 'tfInspectionNumber',
                                    fieldLabel: 'Номер'
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
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
                            name: 'Contragent',
                            textProperty: 'ShortName',
                            fieldLabel: 'Юридическое лицо',
                            store: 'B4.store.Contragent',
                            editable: false,
                            itemId: 'sfContragent',
                            columns: [
                                { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
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
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150,
                                flex: 1
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'PhysicalPerson',
                                    fieldLabel: 'ФИО',
                                    itemId: 'tfPhysicalPerson'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'Inn',
                                    fieldLabel: 'ИНН',
                                    maxLength: 12
                                }
                            ],
                            margin: '0 0 5 0'
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
                                }
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            border: false,
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'basedispheadmaininfotabpanel'
                                },
                                {
                                    xtype: 'baseDispHeadRealityObject',
                                    flex: 1
                                },
                                {
                                    xtype: 'basedispheadcontragentgrid'
                                }
                            ]
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