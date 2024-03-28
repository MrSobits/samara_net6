//ToDo данный js перекрыт в связи с тем что понадобилось в ННовгород добавить для всех сонований поле Ликвидацию ЮЛ в котором
//ToDo при change поля Контрагент срабатывает получение ликвидации и вывода информации 
Ext.define('B4.view.baseinscheck.EditPanel', {
    extend: 'Ext.form.Panel',

    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    itemId: 'baseInsCheckEditPanel',
    title: 'Плановая испекционная проверка',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.store.dict.Inspector',
        'B4.store.dict.PlanInsCheckGji',
        'B4.store.Contragent',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.form.ComboBox',
        'B4.form.FileField',
        'B4.enums.TypeJurPerson',
        'B4.enums.TypeFactInspection',
        'B4.enums.TypeDocumentInsCheck',
        'B4.DisposalTextValues',
        'B4.view.GjiDocumentCreateButton'
    ],

    initComponent: function () {
        var me = this;
        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            defaults: {
                labelWidth: 150,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'panel',
                    padding: '0 0 5 0',
                    bodyStyle: Gkh.bodyStyle,
                    border: false,

                    defaults: {
                        labelWidth: 150
                    },
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    items: [
                        {
                            itemId: 'dfInsCheckDate',
                            xtype: 'datefield',
                            name: 'InsCheckDate',
                            fieldLabel: 'Дата',
                            labelAlign: 'right',
                            flex: 1,
                            format: 'd.m.Y',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'InspectionNumber',
                            fieldLabel: 'Номер',
                            labelAlign: 'right',
                            itemId: 'tfInspectionNumber',
                            flex: 1
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanInsCheckGji',
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
                    xtype: 'combobox', editable: false,
                    name: 'TypeJurPerson',
                    fieldLabel: 'Тип юридического лица',
                    displayField: 'Display',
                    store: B4.enums.TypeJurPerson.getStore(),
                    valueField: 'Value',
                    readOnly: true
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
                    xtype: 'gkhtriggerfield',
                    name: 'InspectorId',
                    itemId: 'insCheckInspectorsTrigerField',
                    fieldLabel: 'Инспектор'
                },
                {
                    xtype: 'gkhtriggerfield',
                    name: 'RealityObjectId',
                    itemId: 'insCheckRealityObjectsTrigerField',
                    fieldLabel: 'Дом',
                    allowBlank: false
                },
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
                            name: 'DateStart',
                            itemId: 'dfDateStart',
                            fieldLabel: 'Дата начала',
                            format: 'd.m.Y'
                        },
                        {
                            xtype: 'numberfield',
                            hideTrigger: true,
                            keyNavEnabled: false,
                            name: 'Area',
                            mouseWheelEnabled: false,
                            fieldLabel: 'Площадь (кв. м.)',
                            itemId: 'insCheckEditPanelAreaNumberField',
                            readOnly: true
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    defaults: {
                        labelWidth: 150
                    },
                    layout: {
                        pack: 'start',
                        type: 'hbox'
                    },
                    items: [
                        {
                            xtype: 'combobox', editable: false,
                            name: 'TypeFact',
                            fieldLabel: 'Факт проверки',
                            labelAlign: 'right',
                            displayField: 'Display',
                            flex: 1,
                            store: B4.enums.TypeFactInspection.getStore(),
                            valueField: 'Value',
                            itemId: 'cbTypeCheck'
                        },
                        {
                            xtype: 'textfield',
                            anchor: '100%',
                            name: 'Reason',
                            labelWidth: 90,
                            flex: 2,
                            fieldLabel: 'Причина',
                            labelAlign: 'right',
                            itemId: 'tfReason',
                            maxLength: 500
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    title: 'Документ',
                    items: [
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                flex: 1
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    itemId: 'dfDocumentDate',
                                    labelWidth: 140,
                                    fieldLabel: 'от',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    labelWidth: 150,
                                    fieldLabel: 'Номер',
                                    labelAlign: 'right',
                                    itemId: 'tfDocumentNumber',
                                    maxLength: 300
                                }
                            ]
                        },
                        {
                            xtype: 'container',
                            padding: '0 0 5 0',
                            defaults: {
                                labelAlign: 'right',
                                flex: 1
                            },
                            layout: 'hbox',
                            items: [
                                {
                                    xtype: 'combobox', editable: false,
                                    name: 'TypeDocument',
                                    labelWidth: 140,
                                    fieldLabel: 'Тип документа',
                                    displayField: 'Display',
                                    store: B4.enums.TypeDocumentInsCheck.getStore(),
                                    valueField: 'Value',
                                    itemId: 'cbTypeDocument'
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'DocFile',
                                    fieldLabel: 'Файл',
                                    anchor: '100%',
                                    labelWidth: 150
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