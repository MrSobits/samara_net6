//ToDo данный js перекрыт в связи с тем что понадобилось в ННовгород добавить для всех сонований поле Ликвидацию ЮЛ в котором
//ToDo при change поля Контрагент срабатывает получение ликвидации и вывода информации 

Ext.define('B4.view.basedisphead.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseDispHeadEditPanel',
    title: 'Проверка по поручению руководства',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Inspector',
        'B4.store.DocumentGji',
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.ComboBox',
        'B4.store.Contragent',
        'B4.view.basedisphead.RealityObjectGrid',
        'B4.view.basedisphead.MainInfoTabPanel',
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton'
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
                                    name: 'TypeJurPerson',
                                    fieldLabel: 'Тип юридического лица',
                                    store: B4.enums.TypeJurPerson.getStore(),
                                    itemId: 'cbTypeJurPerson'
                                },
                                {
                                    name: 'PersonInspection',
                                    fieldLabel: 'Объект проверки',
                                    store: B4.enums.PersonInspection.getStore(),
                                    itemId: 'cbPersonInspection'
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
                                    xtype: 'b4selectfield',
                                    name: 'Contragent',
                                    textProperty: 'ShortName',
                                    fieldLabel: 'Юридическое лицо',
                                    editable: false,
                                    readOnly: true,
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
                                    name: 'ActivityInfo',
                                    fieldLabel: 'Ликвидация ЮЛ',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'PhysicalPerson',
                            fieldLabel: 'ФИО',
                            itemId: 'tfPhysicalPerson'
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