Ext.define('B4.view.smevownershipproperty.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevownershipproperty.FileInfoGrid',
        'B4.store.dict.municipality.ListAllWithParent',
        'B4.store.RealityObject',
        'B4.store.cscalculation.RoomList',
        'B4.enums.realty.RoomType',
        'B4.enums.YesNo',
        'B4.form.ComboBox',
        'B4.enums.PublicPropertyLevel',
        'B4.enums.QueryTypeType',
        'B4.form.FileField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1100,
    height: 600,
    bodyPadding: 10,
    itemId: 'smevownershippropertyEditWindow',
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
                                        //itemId: 'dfQueryType',
                                        defaults: {
                                            margin: '0 0 5 0',
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'combobox',
                                                name: 'QueryType',
                                                fieldLabel: 'Тип запроса',
                                                displayField: 'Display',
                                                itemId: 'dfQueryType',
                                                flex: 1,
                                                store: B4.enums.QueryTypeType.getStore(),
                                                valueField: 'Value',
                                                allowBlank: false,
                                                editable: false
                                                //hidden: true
                                            },
                                            {
                                                xtype: 'combobox',
                                                name: 'PublicPropertyLevel',
                                                fieldLabel: 'Тип собственности',
                                                displayField: 'Display',
                                                itemId: 'dfPublicPropertyLevel',
                                                flex: 1,
                                                store: B4.enums.PublicPropertyLevel.getStore(),
                                                valueField: 'Value',
                                                allowBlank: false,
                                                editable: false
                                                //hidden: true
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        itemId: 'dfRealRoom',
                                        hidden: true,
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
                                                allowBlank: false,
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
                                        itemId: 'dfCadastral',
                                        hidden: true,
                                        defaults: {
                                            //     margin: '10 0 5 0',
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'CadasterNumber',
                                                fieldLabel: 'Кадастровый номер',
                                                allowBlank: false,
                                                flex: 1,
                                                disabled: false
                                                
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        itemId: 'dfRegNumber',
                                        hidden: true,
                                        defaults: {
                                            //     margin: '10 0 5 0',
                                            labelWidth: 130,
                                            labelAlign: 'right',
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'RegisterNumber',
                                                fieldLabel: 'Номер реестра',
                                                allowBlank: false,
                                                flex: 1,
                                                disabled: false
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
                                    //,
                                    //{
                                    //    xtype: 'b4filefield',
                                    //    name: 'AnswerFile',
                                    //    fieldLabel: 'Файл',
                                    //    flex: 1
                                    //}
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
                                    xtype: 'smevownershippropertyfileinfogrid',
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
                                    margin: '0 0 20 0',                                    
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
                                xtype: 'b4filefield',
                                name: 'AttachmentFile',
                                fieldLabel: 'Файл',
                                flex: 1,
                                hideTrigger: false
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
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 130,
                                        falign: 'stretch',
                                        labelAlign: 'right'
                                    },
                                    bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                    title: 'Информация',
                                    border: false,
                                    bodyPadding: 10,
                                    items: [{
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'combobox',
                                            margin: '0 0 20 0',
                                            labelAlign: 'right',
                                            labelWidth: 130
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'Number',
                                                fieldLabel: 'Номер выписки',
                                                allowBlank: true,
                                                flex: 1,
                                                readOnly: true
                                            },
                                            {
                                                xtype: 'textfield',
                                                name: 'Date',
                                                fieldLabel: 'Дата выписки',
                                                allowBlank: true,
                                                flex: 1,
                                                readOnly: true
                                            }
                                        ]
                                        },
                                        {
                                            xtype: 'textfield',
                                            name: 'Issuer',
                                            fieldLabel: 'Орган выдачи',
                                            allowBlank: true,
                                            flex: 1,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 130
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'SignerFullName',
                                                    fieldLabel: 'ФИО подписавшего',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'SignerPosition',
                                                    fieldLabel: 'Должность',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 130
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Type',
                                                    fieldLabel: 'Вид имущества',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RegNum',
                                                    fieldLabel: 'Реестровый номер имущества',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RegisterNumberDate',
                                                    fieldLabel: 'Дата реестрового номера имущества',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'InitialCost',
                                                    fieldLabel: 'Первоначальная стоимость',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 130
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OtherTypeOfCostName',
                                                    fieldLabel: 'Наименование иного вида стоимости',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'CostOfOtherTypeOfLand',
                                                    fieldLabel: 'Стоимость иного вида',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                            ]
                                        }
                                        ],
                                    },
                                    {
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        defaults: {
                                            labelWidth: 130,
                                            falign: 'stretch',
                                            labelAlign: 'right'
                                        },
                                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                        title: 'Документы',
                                        border: false,
                                        bodyPadding: 10,
                                        items: [{
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 120
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RegistrationNumber',
                                                    fieldLabel: 'Номер регистрации',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'Name',
                                                    fieldLabel: 'Документ',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                labelWidth: 100,
                                                anchor: '100%',
                                                labelAlign: 'right'
                                            },
                                            title: 'Документы, подтверждающие право',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'combobox',
                                                        margin: '0 0 20 0',
                                                        labelAlign: 'right',
                                                        labelWidth: 110
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ORDocNum',
                                                            fieldLabel: 'Номер',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'ORDocDate',
                                                            fieldLabel: 'Дата',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        }
                                                    ]
                                                },
                                            ]
                                        }
                                        ,
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                labelWidth: 100,
                                                anchor: '100%',
                                                labelAlign: 'right'
                                            },
                                            title: 'Документы - основания изъятия из оборота',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'combobox',
                                                        margin: '0 0 20 0',
                                                        labelAlign: 'right',
                                                        labelWidth: 110
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'TDName',
                                                            fieldLabel: 'Наименование',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'TDNum',
                                                            fieldLabel: 'Номер',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'TDDate',
                                                            fieldLabel: 'Дата',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        }
                                                    ]
                                                },
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                labelWidth: 100,
                                                anchor: '100%',
                                                labelAlign: 'right'
                                            },
                                            title: 'Документы - основания ограничения оборота',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'combobox',
                                                        margin: '0 0 20 0',
                                                        labelAlign: 'right',
                                                        labelWidth: 110
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RDName',
                                                            fieldLabel: 'Наименование',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RDNum',
                                                            fieldLabel: 'Номер',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RDDate',
                                                            fieldLabel: 'Дата',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        }
                                                    ]
                                                },
                                            ]
                                        },
                                        ],
                                    },
                                    {
                                        layout: {
                                            type: 'vbox',
                                            align: 'stretch'
                                        },
                                        defaults: {
                                            labelWidth: 130,
                                            falign: 'stretch',
                                            labelAlign: 'right'
                                        },
                                        bodyStyle: 'background: none repeat scroll 0 0 #DFE9F6',
                                        title: 'Правообладатель',
                                        border: false,
                                        bodyPadding: 10,
                                        items: [{
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 110
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhType',
                                                    fieldLabel: 'Тип лица',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhName',
                                                    fieldLabel: 'Наименование',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 110
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhCNum',
                                                    fieldLabel: 'Номер карты',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhOGRN',
                                                    fieldLabel: 'ОГРН',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhOGRNDate',
                                                    fieldLabel: 'Дата ОГРН',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 110
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhLastName',
                                                    fieldLabel: 'Фамилия',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhFirstName',
                                                    fieldLabel: 'Имя',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhMiddleName',
                                                    fieldLabel: 'Отчество',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'container',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'combobox',
                                                margin: '0 0 20 0',
                                                labelAlign: 'right',
                                                labelWidth: 110
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhINN',
                                                    fieldLabel: 'ИНН',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhSNILS',
                                                    fieldLabel: 'СНИЛС',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            defaults: {
                                                labelWidth: 100,
                                                anchor: '100%',
                                                labelAlign: 'right'
                                            },
                                            title: 'Паспорт',
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: 'hbox',
                                                    defaults: {
                                                        xtype: 'combobox',
                                                        margin: '0 0 20 0',
                                                        labelAlign: 'right',
                                                        labelWidth: 110
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RhPassSeries',
                                                            fieldLabel: 'Серия',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RhPassNumber',
                                                            fieldLabel: 'Номер',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        },
                                                        {
                                                            xtype: 'textfield',
                                                            name: 'RhPassIsOn',
                                                            fieldLabel: 'Дата выдачи',
                                                            allowBlank: true,
                                                            flex: 1,
                                                            readOnly: true
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'RhPassIsBy',
                                                    fieldLabel: 'Кем выдан',
                                                    allowBlank: true,
                                                    flex: 1,
                                                    readOnly: true
                                                }
                                            ]
                                        }
                                        ],
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
                        xtype: 'button',
                        iconCls: 'icon-book-go',
                        name: 'btnGetExtract',
                        itemId: 'btnGetExtract',
                        action: 'getExtract',
                        text: 'Результат'
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