Ext.define('B4.view.basejurperson.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseJurPersonEditPanel',
    title: 'Плановая проверка юр. лиц',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.form.ComboBox',
        'B4.store.dict.PlanJurPersonGji',
        'B4.store.dict.Inspector',
        'B4.store.dict.ZonalInspection',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.basejurperson.MainInfoTabPanel',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeJurPerson',
        'B4.enums.ReasonErpChecking',
        'B4.DisposalTextValues'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            margin: '10 0 5 0',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
                                    itemId: 'tfInspectionNumber',
                                    fieldLabel: 'Номер',
                                    labelAlign: 'right',
                                    labelWidth: 150
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
                        },
                        {
                            xtype: 'b4selectfield',
                            margin: '5 0',
                            store: 'B4.store.dict.PlanJurPersonGji',
                            name: 'Plan',
                            itemId: 'sflPlan',
                            fieldLabel: 'План',
                            allowBlank: false,
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            margin: '5 0',
                            name: 'TypeJurPerson',
                            fieldLabel: 'Тип контрагента',
                            displayField: 'Display',
                            valueField: 'Id',
                            itemId: 'cbTypeJurPerson',
                            editable: false,
                            storeAutoLoad: true,
                            url: '/Inspection/ListJurPersonTypes'
                        },
                        {
                            xtype: 'b4selectfield',
                            margin: '5 0 10 0',
                            name: 'Contragent',
                            textProperty: 'ShortName',
                            fieldLabel: 'Юридическое лицо',
                            editable: false,
                            readOnly: true
                        },
                        {
                            xtype: 'tabpanel',
                            flex: 1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            border: false,
                            items: [
                                {
                                    xtype: 'basejurpersonmaininfotabpanel',
                                    border: false
                                },
                                {
                                    xtype: 'baseJurPersonGjiRealObj',
                                    border: false,
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