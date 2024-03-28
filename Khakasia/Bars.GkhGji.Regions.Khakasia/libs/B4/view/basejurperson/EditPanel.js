//ToDo данный js перекрыт в связи с тем что понадобилось в ННовгород добавить для всех сонований поле Ликвидацию ЮЛ в котором
//ToDo при change поля Контрагент срабатывает получение ликвидации и вывода информации 
Ext.define('B4.view.basejurperson.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    width: 750,
    minWidth: 750,
    itemId: 'baseJurPersonEditPanel',
    title: 'Плановая проверка юр. лиц',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.store.dict.PlanJurPersonGji',
        'B4.store.dict.Inspector',
        'B4.view.Control.GkhTriggerField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.view.basejurperson.MainInfoTabPanel',
        'B4.view.GjiDocumentCreateButton',
        'B4.enums.TypeJurPerson',
        'B4.DisposalTextValues'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'panel',
                    flex: 1,
                    layout: { type: 'vbox', align: 'stretch' },
                    split: false,
                    collapsible: false,
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    bodyPadding: 5,
                    defaults: {
                        labelWidth: 150,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'InspectionNumber',
                                    itemId: 'tfInspectionNumber',
                                    fieldLabel: 'Номер',
                                    labelAlign: 'right',
                                    labelWidth: 150
                                }
                            ]
                        },
                        {
                            xtype: 'b4selectfield',
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
                                    itemId: 'sfContragent',
                                    fieldLabel: 'Юридическое лицо',
                                    editable: false,
                                    readOnly: true
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
                            xtype: 'tabpanel',
                            flex: 1,
                            layout: { type: 'vbox', align: 'stretch' },
                            border: false,
                            items: [
                                {
                                    xtype: 'basejurpersonmaininfotabpanel',
                                    border: false,
                                    frame: true
                                },
                                {
                                    xtype: 'baseJurPersonGjiRealObj',
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