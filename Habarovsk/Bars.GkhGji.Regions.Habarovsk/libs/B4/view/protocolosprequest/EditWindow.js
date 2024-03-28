Ext.define('B4.view.protocolosprequest.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.protocolosprequesteditwindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: { type: 'vbox', align: 'stretch' },
    width: 800,
    minWidth: 520,
    //minHeight: 485,
    //height: 485,
    bodyPadding: 5,
    itemId: 'protocolOSPRequestEditWindow',
    title: 'Форма редактирования заявки на доступ к протоколам ОСС',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.store.dict.OwnerProtocolTypeDecisionForSelect',
        'B4.ux.button.Close',
        'B4.store.RealityObject',
        'B4.store.dict.Inspector',
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.enums.FuckingOSSState'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 120,
                labelAlign: 'right'
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 120,
                            name: 'Date',
                            fieldLabel: 'Дата заявления',
                            flex: 0.8,
                            readOnly: true,
                            format: 'd.m.Y',
                            itemId: 'dfDateStart',
                        },
                        {
                            xtype: 'textfield',
                            name: 'RequestNumber',
                            readOnly: true,
                            flex: 0.5,
                            labelWidth: 60,
                            fieldLabel: 'Номер',
                            allowBlank: true
                        },
                        {
                            xtype: 'b4enumcombo',
                            name: 'Approved',
                            flex: 1,
                            labelWidth: 100,
                            fieldLabel: 'Результат',
                            itemId: 'cbFuckingOSSState',
                            enumName: 'B4.enums.FuckingOSSState',
                        },
                    ]
                },
                {
                    xtype: 'textarea',
                    height: 50,
                    anchor: '100%',
                    itemId: 'taResolutionContent',
                    name: 'ResolutionContent',
                    fieldLabel: 'Причина отказа',
                    labelAlign: 'right',
                    maxLength: 500
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'FIO',
                            labelWidth: 120,
                            flex: 1,
                            readOnly: true,
                            fieldLabel: 'Заявитель',
                            allowBlank: false
                        },
                        {
                            xtype: 'b4enumcombo',
                            name: 'ApplicantType',
                            flex: 1,
                            readOnly: true,
                            labelWidth: 130,
                            fieldLabel: 'Статус заявителя',
                            enumName: 'B4.enums.OSSApplicantType',
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [

                        {
                            xtype: 'textfield',
                            name: 'AttorneyFio',
                            fieldLabel: 'ФИО Поручителя',
                            labelWidth: 120,
                            readOnly: true,
                            flex: 2,
                            allowBlank: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'AttorneyNumber',
                            labelWidth: 100,
                            readOnly: true,
                            flex: 1,
                            fieldLabel: 'Доверенность',
                            allowBlank: true
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 50,
                            name: 'AttorneyDate',
                            readOnly: true,
                            fieldLabel: 'от',
                            flex: 1,
                            format: 'd.m.Y',
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'DocNumber',
                            readOnly: true,
                            labelWidth: 120,
                            fieldLabel: '№ Свидетельства',
                            flex: 1,
                            allowBlank: true
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 120,
                            name: 'DocDate',
                            readOnly: true,
                            fieldLabel: 'Дата свидет-ва',
                            flex: 1,
                            format: 'd.m.Y'
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'AttorneyFile',
                            labelWidth: 120,
                            flex: 1,
                            fieldLabel: 'Доверенность',
                            editable: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'FileInfo',
                            labelWidth: 120,
                            flex: 1,
                            fieldLabel: 'Собственность',
                            editable: false
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Email',
                            flex: 1,
                            readOnly: true,
                            labelWidth: 120,
                            fieldLabel: 'Электронная почта',
                            allowBlank: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'PhoneNumber',
                            flex: 1,
                            readOnly: true,
                            labelWidth: 120,
                            fieldLabel: 'Телефон',
                            allowBlank: true
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Address',
                            labelWidth: 120,
                            readOnly: true,
                            fieldLabel: 'Адрес дома',
                            allowBlank: false,
                            flex: 1,
                            allowBlank: true
                        },
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.RealityObject',
                            textProperty: 'Address',
                            editable: false,
                            flex: 1,
                            name: 'RealityObject',
                            fieldLabel: 'МКД',
                            columns: [
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
                                {
                                    text: 'Адрес',
                                    dataIndex: 'Address',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Room',
                            labelWidth: 120,
                            readOnly: true,
                            fieldLabel: 'Помещение',
                            allowBlank: false,
                            flex: 1,
                            allowBlank: true
                        },
                        {
                            xtype: 'textfield',
                            name: 'CadastralNumber',
                            fieldLabel: 'Кадастровый №',
                            labelWidth: 120,
                            readOnly: true,
                            flex: 1,
                            allowBlank: true
                        }
                    ]
                },                
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.OwnerProtocolTypeDecisionForSelect',
                    textProperty: 'Name',
                    editable: false,
                    readOnly: true,
                    name: 'OwnerProtocolType',
                    fieldLabel: 'Повестка',
                    columns: [
                        {
                            text: 'Наименование',
                            dataIndex: 'Name',
                            flex: 1,
                            filter: { xtype: 'textfield' }
                        }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'ProtocolNum',
                            labelWidth: 120,
                            fieldLabel: '№ Протокола',
                            flex: 1,
                            readOnly: true,
                            allowBlank: true
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 120,
                            name: 'ProtocolDate',
                            fieldLabel: 'Дата протокола',
                            flex: 1,
                            format: 'd.m.Y',
                            readOnly: true,
                            itemId: 'dfProtocolDate',
                        },
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            labelWidth: 220,
                            name: 'DateFrom',
                            fieldLabel: 'Период принятия решения и составления протокола ОСС с',
                            readOnly: true,
                            flex: 1,
                            format: 'd.m.Y',
                            itemId: 'dfDateStart',
                        },
                        {
                            xtype: 'datefield',
                            labelWidth: 120,
                            name: 'DateTo',
                            fieldLabel: 'Период по',
                            flex: 1,
                            readOnly: true,
                            format: 'd.m.Y',
                            itemId: 'dfDateEnd'
                        }
                    ]
                },
                {
                    xtype: 'b4selectfield',
                    store: 'B4.store.dict.Inspector',
                    name: 'Inspector',
                    editable: true,
                    fieldLabel: 'Исполнитель',
                    textProperty: 'Fio',
                    isGetOnlyIdProperty: true,
                    columns: [
                        { text: 'ФИО', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                        { text: 'Описание', dataIndex: 'Description', flex: 1, filter: { xtype: 'textfield' } }
                    ]
                },
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    border: false,
                    layout: 'hbox',
                    defaults: {
                        format: 'd.m.Y',
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4filefield',
                            name: 'ProtocolFile',
                            labelWidth: 120,
                            flex: 1,
                            fieldLabel: 'Проткол ОСС',
                            editable: false
                        },
                        {
                            xtype: 'button',
                            name: 'CopyButtonFactAddress',
                            itemId: 'btnCopyButtonFactAddress',
                            text: 'Получить протокол',
                            width: 120,
                            margin: '0 0 0 10'
                        }
                    ]
                },
              
                {
                    xtype: 'textarea',
                    height: 50,
                    anchor: '100%',
                    name: 'Note',
                    readOnly: true,
                    fieldLabel: 'Примечание',
                    labelAlign: 'right',
                    maxLength: 500
                }
 
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Запрос в ЕГРН',
                                    tooltip: 'Отправить запрос',
                                    iconCls: 'icon_prepareData',
                                    itemId: 'sendCalculateButton'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Отправить протокол',
                                    tooltip: 'Отправить протокол',
                                    iconCls: 'icon_arrow',
                                    itemId: 'sendEmailButton'
                                }
                            ]
                        },
       
                        {
                            xtype: 'tbfill'
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'btnState',
                                    text: 'Статус заявления',
                                    menu: []
                                },
                                {
                                    xtype: 'b4closebutton'
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