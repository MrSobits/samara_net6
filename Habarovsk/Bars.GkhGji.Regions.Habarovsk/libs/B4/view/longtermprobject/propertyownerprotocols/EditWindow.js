Ext.define('B4.view.longtermprobject.propertyownerprotocols.EditWindow', {
    extend: 'B4.form.Window',

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.VoteForm',
        'B4.form.EnumCombo',
        'B4.form.SelectField',
        'B4.view.Control.GkhTriggerField',
        'B4.view.longtermprobject.propertyownerprotocols.FileGrid',
        'B4.form.FileField'
    ],

    modal: true,
    layout: 'fit',
    width: 600,
    bodyPadding: 5,
    itemId: 'propertyownerprotocolsEditWindow',
    title: 'Редактирование',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            name: 'ProtocolMKDState',
                            fieldLabel: 'Статус протокола',
                            store: 'B4.store.dict.ProtocolMKDState',
                            allowBlank: false,
                            editable: false
                        },
                        //{
                        //    xtype: 'b4selectfield',
                        //    name: 'ProtocolTypeId',
                        //    fieldLabel: 'Повестка собрания',
                        //    store: 'B4.store.dict.OwnerProtocolType',
                        //    allowBlank: true,
                        //    editable: false
                        //},
                        {
                            xtype: 'b4selectfield',
                            name: 'ProtocolMKDIniciator',
                            fieldLabel: 'Инициатор ОСС',
                            store: 'B4.store.dict.ProtocolMKDIniciator',
                            allowBlank: false,
                            editable: false
                        },        
                        {
                            xtype: 'container',
                            height: 27,
                            layout: {
                                type: 'hbox'
                            },
                            defaults: {
                                labelAlign: 'right',
                                flex: 1,
                                allowBlank: false
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер протокола'
                                },
                                {
                                    xtype: 'datefield',
                                    format: 'd.m.Y',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата протокола'
                                }
                            ]
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'DocumentFile',
                            fieldLabel: 'Файл',
                            itemId: 'ffDocumentFile',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            fieldLabel: 'Примечание',
                            name: 'Description'
                        },
                        {
                            xtype: 'fieldset',
                            padding: '5',
                            title: 'Регистрационные данные',
                            items: [                              
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            name: 'ProtocolMKDSource',
                                            fieldLabel: 'Источник поступления',
                                            flex: 1,
                                            store: 'B4.store.dict.ProtocolMKDSource',
                                            allowBlank: false,
                                            editable: false
                                        }, 
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'gkhtriggerfield',
                                            name: 'disposalInspectors',
                                            itemId: 'trigFInspectors',
                                            fieldLabel: 'Сотрудники',
                                            allowBlank: true,
                                            editable: false
                                        }
                                        //{
                                        //    xtype: 'b4selectfield',
                                        //    name: 'Inspector',
                                        //    fieldLabel: 'Сотрудник',
                                        //    flex: 1,
                                        //    textProperty: 'Fio',
                                        //    store: 'B4.store.dict.Inspector',
                                        //    columns: [
                                        //        { header: 'ФИО', xtype: 'gridcolumn', dataIndex: 'Fio', flex: 1, filter: { xtype: 'textfield' } },
                                        //        { header: 'Должность', xtype: 'gridcolumn', dataIndex: 'Position', flex: 1, filter: { xtype: 'textfield' } }
                                        //    ],
                                        //    dockedItems: [
                                        //        {
                                        //            xtype: 'b4pagingtoolbar',
                                        //            displayInfo: true,
                                        //            store: 'B4.store.dict.Inspector',
                                        //            dock: 'bottom'
                                        //        }
                                        //    ],
                                        //    allowBlank: false,
                                        //    editable: false
                                        //},
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    height: 35,
                                    layout: {
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'RegistrationNumber',
                                            fieldLabel: 'Регистрационный номер'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'RegistrationDate',
                                            fieldLabel: 'Дата регистрации'
                                        }
                                    ]
                                }     
                            ]
                        },
                        {
                            xtype: 'fieldset',                    
                            title: 'Количественные характеристики',
                            items: [                   
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            name: 'VoteForm',
                                            labelWidth: 100,
                                            fieldLabel: 'Форма голосования',
                                            enumName: 'B4.enums.VoteForm',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            name: 'PercentOfParticipating',
                                            fieldLabel: 'Доля принявших участие (%)',
                                            anchor: '50%',
                                            allowBlank: true,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            decimalSeparator: ',',
                                            minValue: 0
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            labelWidth: 100,
                                            name: 'NumberOfVotes',
                                            fieldLabel: 'Количество голосов (кв.м.)',
                                            keyNavEnabled: false,
                                            allowBlank: true,
                                            mouseWheelEnabled: false,
                                            decimalSeparator: ',',
                                            minValue: 0
                                        },
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            labelWidth: 100,
                                            name: 'TotalNumberOfVotes',
                                            fieldLabel: 'Общее количество голосов (кв.м.)',
                                            keyNavEnabled: false,
                                            allowBlank: true,
                                            mouseWheelEnabled: false,
                                            decimalSeparator: ',',
                                            minValue: 0
                                        }
                                    ]
                                }

                            ]
                        },
                        {
                            xtype: 'tabpanel',
                            border: false,
                            flex: 1,
                            defaults: {
                                border: false
                            },
                            items: [
                                {
                                    xtype: 'propertyownerprotocolsfilegrid',
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
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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