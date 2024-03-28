Ext.define('B4.view.warninginspection.EditPanel', {
    extend: 'Ext.form.Panel',

    alias: 'widget.warninginspectioneditpanel',
    itemId: 'warninginspectionEditPanel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },

    title: 'Основание предостережения',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.DisposalTextValues',
        'B4.enums.PersonInspection',
        'B4.enums.TypeJurPerson',
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.store.Contragent',
        'B4.store.DocumentGji',
        'B4.store.dict.Inspector',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.view.Control.GkhIntField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.GjiDocumentCreateButton',
        'B4.view.RealityObjectGjiGrid',
        'B4.view.warninginspection.MainInfoTabPanel',
        'B4.view.inspectiongji.ContragentGrid'
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
                                    name: 'Date',
                                    fieldLabel: 'Дата',
                                    format: 'd.m.Y',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
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
                                    emptyItem: {Id: 0, Display: ''},
                                    url: '/Inspection/ListJurPersonTypes'
                                },
                                {
                                    xtype: 'b4combobox',
                                    name: 'PersonInspection',
                                    fieldLabel: 'Объект',
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
                            margin: '0 0 10 0'
                        },
                        {
                            xtype: 'tabpanel',
                            border: false,
                            flex: 1,
                            name: 'inspectionTabPanel',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'warninginspectionmaininfotabpanel'
                                },
                                {
                                    xtype: 'realityobjectgjigrid',
                                    flex: 1
                                },
                                {
                                    xtype: 'inspectiongjicontragentgrid'
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
                                    iconCls: 'icon-arrow-undo',
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
                                    action: 'State',
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