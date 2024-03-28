Ext.define('B4.view.smevemergencyhouse.EditWindow', {
    extend: 'B4.form.Window',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.view.smevemergencyhouse.FileInfoGrid',
        'B4.store.dict.municipality.ListAllWithParent',
        'B4.store.RealityObject',
        'B4.store.cscalculation.RoomList',
        'B4.enums.realty.RoomType',
        'B4.enums.EmergencyTypeSGIO',
        'B4.form.FileField'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1100,
    height: 350,
    bodyPadding: 10,
    itemId: 'smevemergencyhouseEditWindow',
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
                                        xtype: 'combobox',
                                        name: 'EmergencyTypeSGIO',
                                        fieldLabel: 'Тип запроса',
                                        displayField: 'Display',
                                        itemId: 'cbEmergencyTypeSGIO',
                                        flex: 1,
                                        store: B4.enums.EmergencyTypeSGIO.getStore(),
                                        valueField: 'Value',
                                        allowBlank: false,
                                        editable: false
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
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
                                    xtype: 'smevemergencyhousefileinfogrid',
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