Ext.define('B4.view.smevchangepremisesstate.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevchangepremisesstate.FileInfoGrid',
        'B4.enums.ChangePremisesType',
        'B4.enums.OwnerType',
        'B4.store.RealityObject',
        'B4.store.dict.municipality.ListAllWithParent',
        'B4.store.cscalculation.RoomList',
        'B4.form.FileField',
        'B4.enums.realty.RoomType'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    height: 620,
    bodyPadding: 10,
    itemId: 'smevchangepremisesstateEditWindow',
    title: 'Запрос',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100
            },
            items: [{
                xtype: 'tabpanel',
                border: false,
                flex: 1,
                defaults: {
                    border: false
                },
                items: [
                    {
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 100,
                            margin: '5 0 5 0',
                            align: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Форма запроса',
                        border: false,
                        bodyPadding: 10,
                        items: [
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 100,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Cубъект запроса',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            margin: '0 0 5 0',
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'combobox',
                                                name: 'ChangePremisesType',
                                                fieldLabel: 'Тип запроса',
                                                displayField: 'Display',
                                                valueField: 'Value',
                                                itemId: 'dfChangePremisesType',
                                                labelWidth: 130,
                                                allowBlank: false,
                                                store: B4.enums.ChangePremisesType.getStore(),
                                                flex: 1,
                                                editable: false
                                            },
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        hidden: true,
                                        itemId: 'dfRealRoom',
                                        defaults: {
                                            margin: '0 0 5 0',
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'b4selectfield',
                                                name: 'RealityObject',
                                                fieldLabel: 'Жилой дом',
                                                textProperty: 'Address',
                                                store: 'B4.store.RealityObject',
                                                editable: false,
                                                flex: 1,
                                                itemId: 'sfRealityObject',
                                                allowBlank: false,
                                                columns: [
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
                                                    {
                                                        text: 'Адрес',
                                                        dataIndex: 'Address',
                                                        flex: 1,
                                                        filter: { xtype: 'textfield' }
                                                    }
                                                ]
                                            },
                                            {
                                                xtype: 'b4selectfield',
                                                name: 'Room',
                                                fieldLabel: 'Помещение',
                                                textProperty: 'RoomNum',
                                                store: 'B4.store.cscalculation.RoomList',
                                                editable: false,
                                                flex: 1,
                                                itemId: 'sfRoom',
                                                allowBlank: true,
                                                disabled: true,
                                                columns: [
                                                    {
                                                        text: 'Номер помещения',
                                                        dataIndex: 'RoomNum',
                                                        flex: 1,
                                                        filter: { xtype: 'textfield' }
                                                    },
                                                    {
                                                        text: 'Кадастровый номер',
                                                        dataIndex: 'CadastralNumber',
                                                        flex: 1,
                                                        filter: { xtype: 'textfield' }
                                                    }
                                                ]
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        itemId: 'dfCadastralNumber',
                                        hidden: true,
                                        defaults: {
                                            margin: '0 0 5 0',
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'CadastralNumber',
                                                fieldLabel: 'Кадастровый номер',
                                                allowBlank: false,
                                                disabled: false,
                                                flex: 1,
                                                editable: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'b4selectfield',
                                        name: 'Municipality',
                                        itemId: 'sfMunicipality',
                                        textProperty: 'Name',
                                        fieldLabel: 'Муниципальное образование',
                                        store: 'B4.store.dict.municipality.ListAllWithParent',
                                        flex: 1,
                                        editable: false,
                                        allowBlank: false,
                                        columns: [
                                            { text: 'Наименование', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                                            { text: 'Район', dataIndex: 'ParentMo', flex: 1, filter: { xtype: 'textfield' } },
                                            { text: 'ОКТМО', dataIndex: 'Oktmo', flex: 0.5, filter: { xtype: 'textfield' } },
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
                                items: [{
                                    xtype: 'smevchangepremisesstatefileinfogrid',
                                    flex: 1
                                }]
                            }
                        ]
                    },
                    {
                        layout: {
                            type: 'vbox',
                            align: 'stretch'
                        },
                        defaults: {
                            labelWidth: 170,
                            falign: 'stretch',
                            labelAlign: 'right'
                        },
                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                        title: 'Ответ',
                        border: false,
                        bodyPadding: 10,
                        items: [{
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 5 0',                                    
                                    labelAlign: 'right',
                                },
                                items: [{
                                        xtype: 'button',
                                        text: 'Получить сведения',
                                        tooltip: 'Получить сведения',
                                        iconCls: 'icon-accept',
                                        width: 200,
                                        //    action: 'romExecute',
                                        itemId: 'sendGetrequestButton'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'Answer',
                                        fieldLabel: 'Ответ на запрос',
                                        itemId: 'dfAnswerGet',
                                        allowBlank: true,
                                        disabled: false,
                                        flex: 1,
                                        editable: false,
                                        maxLength: 1000,
                                        labelWidth: 100,
                                        readOnly: true
                                    },
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Сведения о заявителе',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            enumName: 'B4.enums.OwnerType',
                                            name: 'DeclarantType',
                                            fieldLabel: 'Тип заявителя',
                                            allowBlank: true,
                                            flex: 0.5,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'DeclarantName',
                                            fieldLabel: 'Имя заявителя',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'DeclarantAddress',
                                                fieldLabel: 'Адрес заявителя',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'textfield',
                                name: 'Department',
                                fieldLabel: 'Орган',
                                labelWidth: 111,
                                allowBlank: true,
                                flex: 1,
                                disabled: false,
                                editable: false,
                                readOnly: true
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Сведения об объекте',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'Area',
                                                fieldLabel: 'Площадь',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'City',
                                                fieldLabel: 'Город',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Street',
                                                fieldLabel: 'Улица',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'House',
                                                fieldLabel: 'Дом',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Block',
                                                fieldLabel: 'Корпус',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Apartment',
                                                fieldLabel: 'Квартира',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'b4enumcombo',
                                                enumName: 'B4.enums.realty.RoomType',
                                                name: 'RoomType',
                                                fieldLabel: 'Тип помещения',
                                                allowBlank: true,
                                                flex: 0.5,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Appointment',
                                                fieldLabel: 'Цель использования',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Сведения об заключении',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ActName',
                                                fieldLabel: 'Акт',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ActNumber',
                                                fieldLabel: 'Дом',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'ActDate',
                                                fieldLabel: 'Дата акта',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'b4enumcombo',
                                                enumName: 'B4.enums.realty.RoomType',
                                                name: 'OldPremisesType',
                                                fieldLabel: 'Тип помещения (старый)',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'b4enumcombo',
                                                enumName: 'B4.enums.realty.RoomType',
                                                name: 'NewPremisesType',
                                                fieldLabel: 'Тип помещения (новый)',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ConditionTransfer',
                                                fieldLabel: 'Условия перевода',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Сведения об ответственном лице',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ResponsibleName',
                                                fieldLabel: 'Ответств. лицо',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ResponsiblePost',
                                                fieldLabel: 'Должность',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'ResponsibleDate',
                                                fieldLabel: 'Дата',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Файл',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'b4filefield',
                                                name: 'AnswerFile',
                                                fieldLabel: 'Файл',
                                                flex: 1
                                                //hideTrigger: true
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    }
                ]
            }],
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                        xtype: 'buttongroup',
                        columns: 2,
                        items: [{
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
                        items: [{
                            xtype: 'b4closebutton'
                        }]
                    }
                ]
            }]
        });

        me.callParent(arguments);
    }
});