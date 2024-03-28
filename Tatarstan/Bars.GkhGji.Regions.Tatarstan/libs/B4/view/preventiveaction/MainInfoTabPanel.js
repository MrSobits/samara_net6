Ext.define('B4.view.preventiveaction.MainInfoTabPanel', {
    extend: 'Ext.form.Panel',
    alias: 'widget.preventiveactionmaininfotabpanel',

    requires: [
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.form.FiasSelectAddress',
        'B4.form.FileField',

        'B4.view.Control.GkhTriggerField',

        'B4.store.dict.Inspector',
        'B4.store.dict.PlanActionGji',
        'B4.store.Contragent',
        'B4.store.dict.ControlType',

        'B4.enums.PreventiveActionType',
        'B4.enums.PreventiveActionVisitType',
        'B4.enums.TypeJurPerson'
    ],
    
    title: 'Общие сведения',
    bodyStyle: Gkh.bodyStyle,
    border: false,
    
    initComponent: function(){
        var me = this;
        
        Ext.apply(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        xtype: 'container',
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        labelAlign: 'right',
                        labelWidth: 170,
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'Head',
                            fieldLabel: 'Руководитель',
                            textProperty: 'Fio',
                            store: 'B4.store.dict.Inspector',
                            columns: [
                                { text: 'ФИО', dataIndex: 'Fio', flex: 1 },
                                { text: 'Код', dataIndex: 'Code', flex: 1 }
                            ],
                            editable: false,
                            padding: '10 10 0 15'
                        },
                        {
                            xtype: 'gkhtriggerfield',
                            name: 'Inspectors',
                            itemId: 'trigfInspectors',
                            fieldLabel: 'Инспекторы',
                            allowBlank: false,
                            padding: '0 10 0 15'
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.dict.PlanActionGji',
                            name: 'Plan',
                            fieldLabel: 'План',
                            editable: false,
                            columns: [
                                { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                            ],
                            padding: '0 10 0 15'
                        },
                        {
                            border: false,
                            defaults: {
                                style: {
                                    background: '#dfe9f6'
                                },
                                flex: 1,
                                labelAlign: 'right'
                            },
                            padding: '0 10 5 15',
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'ActionType',
                                    fieldLabel: 'Вид мероприятия',
                                    labelWidth: 170,
                                    enumName: 'B4.enums.PreventiveActionType',
                                    readOnly: true
                                },
                                {
                                    xtype: 'b4enumcombo',
                                    name: 'VisitType',
                                    hidden: true,
                                    fieldLabel: 'Тип визита',
                                    enumName: 'B4.enums.PreventiveActionVisitType',
                                    readOnly: true
                                }
                            ]
                        },
                        {
                            padding: '0 10 5 15',
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    name: 'ControlType',
                                    fieldLabel: 'Вид контроля',
                                    store: 'B4.store.dict.ControlType',
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'Name', flex: 1 }
                                    ],
                                    labelWidth: 170,
                                    labelAlign: 'right',
                                    allowBlank: false,
                                    flex: 1
                                },
                                {
                                    xtype: 'container',
                                    flex: 1
                                }
                            ]
                        },
                        {
                            padding: '0 10 10 15',
                            items: [
                                {
                                    xtype: 'container',
                                    itemId: 'erknmContainer',
                                    flex: 1,
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        readOnly: true,
                                        flex: 1,
                                        labelAlign: 'right',
                                        labelWidth: 170
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ErknmRegistrationNumber',
                                            fieldLabel: 'Учетный номер ПМ в ЕРКНМ'
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'ErknmGuid',
                                            fieldLabel: 'Идентификатор в ЕРКНМ'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ErknmRegistrationDate',
                                            fieldLabel: 'Дата присвоения учетного</br>номера / идентификатора</br>в ЕРКНМ',
                                        },
                                    ],
                                    hidden: true
                                },
                                {
                                    xtype: 'container',
                                    flex: 2
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Подконтрольное лицо, участвующее в мероприятии',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                    align: 'middle',
                                }
                            },
                            margin: '0 5 0 5',
                            items: [
                                {
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Тип подконтрольного</br>лица',
                                            enumName: 'B4.enums.TypeJurPerson',
                                            name: 'ControlledPersonType',
                                            enumItems: [
                                                'ManagingOrganization',
                                                'SupplyResourceOrg',
                                                'Tsj',
                                                'Owner',
                                                'ResourceCompany',
                                                'PublicServiceOrg',
                                            ],
                                            labelWidth: 170,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'FullName',
                                            fieldLabel: 'ФИО',
                                            maxLength: 255
                                        }
                                    ]
                                },
                                {
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            editable: false,
                                            name: 'ControlledOrganization',
                                            fieldLabel: 'Организация',
                                            store: 'B4.store.Contragent',
                                            columns: [
                                                {
                                                    header: 'Наименование',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'ShortName',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    header: 'Муниципальное район',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Municipality',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                },
                                                {
                                                    header: 'ИНН',
                                                    xtype: 'gridcolumn',
                                                    dataIndex: 'Inn',
                                                    flex: 1,
                                                    filter: { xtype: 'textfield' }
                                                }
                                            ],
                                            labelWidth: 170,
                                            allowBlank: false
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'PhoneNumber',
                                            fieldLabel: 'Телефон',
                                            maxLength: 50
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4fiasselectaddress',
                                    name: 'ControlledPersonAddress',
                                    fieldLabel: 'Адрес нахождения</br>контролируемого лица',
                                    fieldsRegex: {
                                        tfHousing: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        },
                                        tfBuilding: {
                                            regex: /^\d+$/,
                                            regexText: 'В это поле можно вводить только цифры'
                                        }
                                    },
                                    labelWidth: 170
                                },
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Документ',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                xtype: 'container',
                                layout: {
                                    type: 'hbox',
                                    align: 'stretch'
                                },
                                labelAlign: 'right'
                            },
                            margin: '0 5 0 5',
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'FileName',
                                    fieldLabel: 'Наименование',
                                    labelWidth: 170,
                                    maxLength: 255
                                },
                                {
                                    defaults: {
                                        flex: 1,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'FileNumber',
                                            fieldLabel: 'Номер',
                                            labelWidth: 170,
                                            maxLength: 255
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'FileDate',
                                            fieldLabel: 'Дата'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'File',
                                    fieldLabel: 'Файл',
                                    labelWidth: 170,
                                    margin: '5 0 0 0',
                                    editable: false,
                                    onClearFile: function () {
                                        var me = this,
                                            currentValue = me.getValue();

                                        me.setValue(null);

                                        me.fileIsDelete = true;
                                        me.fileIsLoad = false;

                                        me.fireEvent('fileclear', me, me.getName(), currentValue);
                                        B4.form.FileField.prototype.reset(me);
                                    },
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