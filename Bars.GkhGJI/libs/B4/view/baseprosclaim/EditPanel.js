Ext.define('B4.view.baseprosclaim.EditPanel', {
    extend: 'Ext.form.Panel',

    requires: [
        'B4.view.Control.GkhTriggerField',
        'B4.form.SelectField',
        'B4.store.ManagingOrganization',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.view.baseprosclaim.RealityObjectGrid',
        'B4.view.baseprosclaim.MainInfoTabPanel',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.enums.ReasonErpChecking',
        'B4.view.GjiDocumentCreateButton',
        'B4.DisposalTextValues'
    ],

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseProsClaimEditPanel',
    title: 'Проверка по требованию прокуратуры',
    trackResetOnLoad: true,
    autoScroll: true,

    initComponent: function () {
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
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
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
                                },
                                {
                                    xtype: 'tbfill'
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ControlType',
                                    width: 270,
                                    fieldLabel: 'Вид контроля',
                                    labelAlign: 'right',
                                    enumName: 'B4.enums.ControlType',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    editable: false,
                                    name: 'ReasonErpChecking',
                                    fieldLabel: 'Основание для включения проверки в ЕРП',
                                    labelAlign: 'right',
                                    enumName: 'B4.enums.ReasonErpChecking',
                                    allowBlank: false,
                                    labelWidth: 300
                                }
                            ]
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
                                xtype: 'combobox', editable: false,
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
                            xtype: 'textfield',
                            name: 'PhysicalPerson',
                            fieldLabel: 'ФИО',
                            readOnly: true,
                            itemId: 'tfPhysicalPerson'
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