Ext.define('B4.view.baseplanaction.EditPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.basePlanActionEditPanel',
    
    closable: true,
    layout: { type: 'vbox', align: 'stretch' },
    title: 'Проверка по плану мероприятий',
    trackResetOnLoad: true,
    autoScroll: true,

    requires: [
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.view.Control.GkhIntField',
        
        'B4.view.Control.GkhTriggerField',
        'B4.view.GjiDocumentCreateButton',
        
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        
        'B4.DisposalTextValues',
        
        'B4.store.dict.PlanActionGji',
        'B4.store.Contragent',
        
        'B4.enums.TypeJurPerson',
        'B4.enums.PersonInspection'
    ],

    initComponent: function () {
        var me = this,
            currTypeJurPerson = B4.enums.TypeJurPerson.getItems(),
            currPersonInspection = B4.enums.PersonInspection.getItems(),
            newPersonInspection = [],
            newTypeJurPerson = [];

        Ext.iterate(currTypeJurPerson, function (val) {
            if (val[0] != 30 && val[0] != 70)
                newTypeJurPerson.push(val);
        });

        Ext.iterate(currPersonInspection, function (val) {
            if (val[0] != 30)
                newPersonInspection.push(val);
        });
        
        Ext.applyIf(me, {
            bodyStyle: Gkh.bodyStyle,
            bodyPadding: 5,
            defaults: {
                anchor: '100%',
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.PlanActionGji',
                    name: 'Plan',
                    labelWidth: 175,
                    fieldLabel: 'План',
                    allowBlank: false,
                    editable: false,
                    columns: [
                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                    ]
                },
                {
                    xtype: 'fieldset',
                    padding: '5 5',
                    title: 'Субъект наблюдения',
                    defaults: {
                        labelWidth: 170,
                        anchor: '100%',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4combobox',
                            name: 'PersonInspection',
                            editable: false,
                            fieldLabel: 'Тип субъекта наблюдения',
                            displayField: 'Display',
                            items: newPersonInspection,
                            valueField: 'Value'
                        },
                        {
                            xtype: 'b4combobox',
                            name: 'TypeJurPerson',
                            editable: false,
                            fieldLabel: 'Тип организации',
                            displayField: 'Display',
                            items: newTypeJurPerson,
                            valueField: 'Value'
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Contragent',
                            textProperty: 'Name',
                            fieldLabel: 'Организация',
                            store: 'B4.store.Contragent',
                            allowBlank:false,
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                {
                                    text: 'Муниципальное образование', dataIndex: 'Municipality', flex: 1,
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
                            allowBlank: false,
                            fieldLabel: 'ФИО'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    padding: '5 5',
                    title: 'Реквизиты',
                    defaults: {
                        labelWidth: 170,
                        labelAlign: 'right',
                        anchor: '100%'
                    },
                    items: [
                        {
                            xtype: 'container',
                            padding: '5',
                            defaults: {
                                labelWidth: 165,
                                labelAlign: 'right',
                                flex: 1
                            },
                            layout: {
                                pack: 'start',
                                type: 'hbox'
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'ContragentOgrn',
                                    fieldLabel: 'ОГРН',
                                    readOnly: true
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'ContragentInn',
                                    fieldLabel: 'ИНН',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name:'ContragentAddress',
                            fieldLabel: 'Место нахождения',
                            readOnly: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'PersonAddress',
                            fieldLabel: 'Место нахождения'
                        }
                    ]
                },
                {
                    xtype: 'fieldset',
                    padding: '5 5',
                    title: 'Срок проведения мероприятия',
                    items: [
                        {
                            xtype: 'container',
                            defaults: {
                                labelWidth: 170,
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
                                    fieldLabel: 'Начало',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DateEnd',
                                    fieldLabel: 'Окончание',
                                    format: 'd.m.Y'
                                },
                                {
                                    xtype: 'gkhintfield',
                                    name: 'CountDays',
                                    fieldLabel: 'Рабочих дней',
                                    hideTrigger: true
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'textarea',
                    labelWidth: 175,
                    anchor: '100%',
                    name: 'Requirement',
                    height: 50,
                    fieldLabel: 'Требования, подлежащие наблюдению',
                    maxLength: 5000
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