Ext.define('B4.view.smevpremises.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevpremises.FileInfoGrid',
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    height: 750,
    bodyPadding: 10,
    itemId: 'smevpremisesEditWindow',
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
                items: [{
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
                        items: [{
                                xtype: 'fieldset',
                                defaults: {
                                    labelWidth: 250,
                                    anchor: '100%',
                                    labelAlign: 'right'
                                },
                                title: 'Реквизиты субъекта запроса',
                                items: [{
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            //labelWidth: 170,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'OKTMO',
                                                fieldLabel: 'OKTMO',
                                                itemId: 'dfOKTMO',
                                                allowBlank: false,
                                                disabled: false,
                                                flex: 1,
                                                editable: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'ActNumber',
                                                fieldLabel: 'Номер акта',
                                                allowBlank: false,
                                                flex: 1,
                                                disabled: false,
                                                editable: true,
                                                maxLength: 20,
                                                itemId: 'dfActNumber'
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'ActDate',
                                                fieldLabel: 'Дата принятия акта',
                                                allowBlank: false,
                                                flex: 1,
                                                disabled: false,
                                                editable: true,
                                                itemId: 'dfActDate'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            //labelWidth: 170,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ActName',
                                                fieldLabel: 'Наименование акта',
                                                itemId: 'dfActName',
                                                allowBlank: false,
                                                disabled: false,
                                                flex: 1,
                                                editable: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            //     margin: '10 0 5 0',
                                            //labelWidth: 170,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'ActDepartment',
                                                fieldLabel: 'Орган выдавший акт',
                                                itemId: 'dfActDepartment',
                                                allowBlank: false,
                                                disabled: false,
                                                flex: 1,
                                                editable: true
                                            }
                                        ]
                                    },
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
                                    xtype: 'smevpremisesfileinfogrid',
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
                                title: 'Информация о сотруднике обработавшего запрос',
                                items: [{
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //     margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'EmployeeName',
                                            fieldLabel: 'ФИО',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },

                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //    margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'EmployeePost',
                                            fieldLabel: 'Должность',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Department',
                                            fieldLabel: 'Орган',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        }
                                    ]
                                },

                                ]
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 10 0',
                                    labelWidth: 111,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'PremisesInfo',
                                        fieldLabel: 'Сведения о помещении',
                                        allowBlank: true,
                                        flex: 1,
                                        disabled: false,
                                        editable: false,
                                        readOnly: true
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
                                title: 'Адрес жилого помещения/многоквартирного дома',
                                items: [{
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Region',
                                            fieldLabel: 'Регион',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'District',
                                            fieldLabel: 'Район',
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
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        //    margin: '10 0 5 0',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'Locality',
                                            fieldLabel: 'Населенный пункт',
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
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'House',
                                            fieldLabel: 'Дом',
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
                                            //    margin: '10 0 5 0',
                                            labelWidth: 100,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'Housing',
                                                fieldLabel: 'Корпус',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Building',
                                                fieldLabel: 'Строение',
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
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Index',
                                                fieldLabel: 'Индекс',
                                                allowBlank: true,
                                                flex: 1,
                                                disabled: false,
                                                editable: false,
                                                readOnly: true
                                            }
                                        ]
                                    },
                                ]
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 10 0',
                                    labelWidth: 111,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'CadastralNumber',
                                        fieldLabel: 'Кадастровый номер',
                                        allowBlank: true,
                                        flex: 1,
                                        disabled: false,
                                        editable: false,
                                        readOnly: true
                                    },
                                    {
                                        xtype: 'datefield',
                                        name: 'PropertyRightsDate',
                                        fieldLabel: 'Дата права собственности',
                                        allowBlank: true,
                                        flex: 1,
                                        disabled: false,
                                        editable: false,
                                        readOnly: true
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
                                title: 'Реквизиты документов - оснований возникновения права собственности или иного вещного права',
                                items: [{
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocRightNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'DocRightDate',
                                            fieldLabel: 'Дата',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        }
                                    ]
                                },
                                ]
                            },
                            {
                                xtype: 'container',
                                layout: 'hbox',
                                defaults: {
                                    xtype: 'combobox',
                                    margin: '0 0 10 0',
                                    labelWidth: 111,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'RightholderInfo',
                                        fieldLabel: 'Cведения о правообладателе',
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
                                    margin: '0 0 10 0',
                                    labelWidth: 111,
                                    labelAlign: 'right',
                                    flex: 1
                                },
                                items: [
                                    {
                                        xtype: 'textfield',
                                        name: 'SupervisionDetails',
                                        fieldLabel: 'Реквизиты заключений',
                                        allowBlank: true,
                                        flex: 1,
                                        disabled: false,
                                        editable: false,
                                        readOnly: true
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
                                title: 'Реквизиты акта обследования помещения',
                                items: [{
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'InsNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'InsDate',
                                            fieldLabel: 'Дата',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        }
                                    ]
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
                                title: 'Реквизиты заключения о признании жилого помещения пригодным (непригодным) для постоянного проживания',
                                items: [{
                                    xtype: 'container',
                                    layout: 'hbox',
                                    defaults: {
                                        xtype: 'combobox',
                                        labelWidth: 100,
                                        labelAlign: 'right',
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'ConNumber',
                                            fieldLabel: 'Номер',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'ConDate',
                                            fieldLabel: 'Дата',
                                            allowBlank: true,
                                            flex: 1,
                                            disabled: false,
                                            editable: false,
                                            readOnly: true
                                        }
                                    ]
                                },
                                ]
                            },
                        ]
                    },
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