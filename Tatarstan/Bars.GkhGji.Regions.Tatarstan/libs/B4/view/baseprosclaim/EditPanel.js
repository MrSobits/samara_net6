Ext.define('B4.view.baseprosclaim.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.enums.ReasonErpChecking',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.RiskCategory',
        'B4.store.ManagingOrganization',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.baseprosclaim.RealityObjectGrid',
        'B4.view.baseprosclaim.MainInfoTabPanel',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.baseprosclaim.ContragentGrid',
        'B4.DisposalTextValues'
    ],

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseProsClaimEditPanel',
    title: 'Проверка по требованию прокуратуры',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    bodyPadding: '5 5 0 5',
                    border: false,
                    autoScroll: true,
                    split: false,
                    collapsible: false,
                    layout: 'hbox',
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'InspectionNumber',
                            itemId: 'tfInspectionNumber',
                            fieldLabel: 'Номер',
                            width: 400
                        }
                    ]
                },
                {
                    xtype: 'panel',
                    bodyStyle: Gkh.bodyStyle,
                    bodyPadding: 5,
                    flex: 1,
                    border: false,
                    autoScroll: true,
                    split: false,
                    collapsible: false,
                    layout: { type: 'vbox', align: 'stretch' },
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
                            editable: false,
                            itemId: 'sfContragent',
                            store: 'B4.store.Contragent',
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
                                    readOnly: true,
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
                                },
                                { xtype: 'component' }
                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            flex: 1,
                            layout: { type: 'vbox', align: 'stretch' },
                            border: false,
                            items: [
                                {
                                    xtype: 'baseprosclaimmaininfotabpanel'
                                },
                                {
                                    xtype: 'baseProsClaimRealityObjectGrid',
                                    flex: 1
                                },
                                {
                                    xtype: 'baseprosclaimcontragentgrid'
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