Ext.define('B4.view.Fias.EditWindow', {
    extend: 'B4.form.Window',

    modal: true,
    layout: 'fit',
    width: 600,
    height: 700,
    maximizable: true,
    resizable: true,
    itemId: 'fiasEditWindow',
    title: 'Запись ФИАС',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        
        'B4.enums.FiasLevelEnum',
        'B4.enums.FiasCenterStatusEnum',
        'B4.enums.FiasActualStatusEnum'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    items: [
                        {
                            xtype: 'panel',
                            margins: -1,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right',
                                margin: '0 0 10px 0'
                            },
                            layout: {
                                type: 'anchor'
                            },
                            title: 'Основная информация',
                            items: [
                                {
                                    xtype: 'combobox',
                                    anchor: '100%',
                                    floating: false,
                                    itemId: 'levelCombobox',
                                    name: 'AOLevel',
                                    fieldLabel: 'Уровень',
                                    labelAlign: 'right',
                                    displayField: 'Display',
                                    store: B4.enums.FiasLevelEnum.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'textfield',
                                    maxLength:120,
                                    anchor: '100%',
                                    name: 'OffName',
                                    fieldLabel: 'Официальное наименование',
                                    labelAlign: 'right',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'textfield',
                                    maxLength: 120,
                                    anchor: '100%',
                                    name: 'FormalName',
                                    fieldLabel: 'Формализованное наименование',
                                    labelAlign: 'right',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    anchor: '100%',
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            maxLength: 10,
                                            anchor: '100%',
                                            name: 'ShortName',
                                            labelWidth: 200,
                                            fieldLabel: 'Краткое наименование',
                                            flex: 1,
                                            labelAlign: 'right'
                                        },
                                        {
                                            xtype: 'textfield',
                                            maxLength: 6,
                                            anchor: '100%',
                                            name: 'PostalCode',
                                            labelWidth: 200,
                                            flex: 1,
                                            fieldLabel: 'Почтовый индекс',
                                            labelAlign: 'right'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    itemId:'codeRegionTextField',
                                    anchor: '100%',
                                    name: 'CodeRegion',
                                    maxLength: 5,
                                    labelWidth: 350,
                                    fieldLabel: 'Код региона',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeAutoTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeAuto',
                                    labelWidth: 350,
                                    fieldLabel: 'Код автономии',
                                    labelAlign: 'right'
                                }, 
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeAreaTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeArea',
                                    labelWidth: 350,
                                    fieldLabel: 'Код района',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeCityTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeCity',
                                    labelWidth: 350,
                                    fieldLabel: 'Код города',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeCtarTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeCtar',
                                    labelWidth: 350,
                                    fieldLabel: 'Код внутригородского района',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codePlaceTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodePlace',
                                    labelWidth: 350,
                                    fieldLabel: 'Код населенного пункта',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeStreetTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeStreet',
                                    labelWidth: 350,
                                    fieldLabel: 'Код улицы',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeExtrTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeExtr',
                                    labelWidth: 350,
                                    fieldLabel: 'Код доп. адресообразующего элемента',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'codeSextTextField',
                                    anchor: '100%',
                                    maxLength: 5,
                                    name: 'CodeSext',
                                    labelWidth: 350,
                                    fieldLabel: 'Код  подчиненного доп. адресообразующего элемента',
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    anchor: '100%',
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        labelWidth: 200,
                                        labelAlign: 'right'
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            anchor: '100%',
                                            name: 'StartDate',
                                            fieldLabel: 'Дата начала',
                                            flex: 1,
                                            format: 'd.m.Y'
                                        },
                                        {
                                            xtype: 'datefield',
                                            anchor: '100%',
                                            name: 'EndDate',
                                            fieldLabel: 'Дата окончания',
                                            flex: 1,
                                            format: 'd.m.Y'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'combobox',
                                    anchor: '100%',
                                    floating: false,
                                    name: 'CentStatus',
                                    fieldLabel: 'Статус центра',
                                    labelAlign: 'right',
                                    displayField: 'Display',
                                    store: B4.enums.FiasCenterStatusEnum.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'combobox',
                                    anchor: '100%',
                                    floating: false,
                                    name: 'ActStatus',
                                    fieldLabel: 'Статус актуальности',
                                    labelAlign: 'right',
                                    displayField: 'Display',
                                    store: B4.enums.FiasActualStatusEnum.getStore(),
                                    valueField: 'Value'
                                },
                                {
                                    xtype: 'textfield',
                                    itemId: 'sdAOGuid',
                                    anchor: '100%',
                                    maxLength: 50,
                                    name: 'AOGuid',
                                    fieldLabel: 'Код ФИАС',
                                    labelAlign: 'right'
                                },
                            ]
                        },
                        {
                            xtype: 'panel',
                            margins: -1,
                            border: false,
                            frame: true,
                            defaults: {
                                labelWidth: 200,
                                labelAlign: 'right'
                            },
                            layout: {
                                type: 'anchor'
                            },
                            title: 'Дополнительная информация',
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    anchor: '100%',
                                    layout: {
                                        pack: 'start',
                                        type: 'hbox'
                                    },
                                    defaults: {
                                        //labelWidth: 60,
                                        labelAlign: 'right',
                                        //labelAlign: 'top',
                                        margin: '5px 10px 0 12px'
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            anchor: '100%',
                                            name: 'OKATO',
                                            fieldLabel: 'ОКАТО',
                                            flex: 1
                                        },
                                        {
                                            xtype: 'textfield',
                                            anchor: '100%',
                                            name: 'OKTMO',
                                            flex: 1,
                                            fieldLabel: 'ОКТМО'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'ИФНС ФЛ',
                                    anchor: '100%',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                //labelWidth: 50,
                                                labelAlign: 'right'
                                                //,labelAlign: 'top'
                                                
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    anchor: '100%',
                                                    name: 'IFNSFL',
                                                    fieldLabel: 'Код',
                                                    flex: 1,
                                                    margin: '0 12px 0 0'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    anchor: '100%',
                                                    name: 'TerrIFNSFL',
                                                    fieldLabel: 'Код терр. участка',
                                                    flex: 1,
                                                    margin: '0 0 0 10px'
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    title: 'ИФНС ЮЛ',
                                    anchor: '100%',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            anchor: '100%',
                                            layout: {
                                                pack: 'start',
                                                type: 'hbox'
                                            },
                                            defaults: {
                                                //labelWidth: 100,
                                                labelAlign: 'right'
                                                //,labelAlign: 'top'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    anchor: '100%',
                                                    name: 'IFNSUL',
                                                    fieldLabel: 'Код',
                                                    flex: 1,
                                                    margin: '0 12px 0 0'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    anchor: '100%',
                                                    name: 'TerrIFNSUL',
                                                    fieldLabel: 'Код терр. участка',
                                                    flex: 1,
                                                    margin: '0 0 0 10px'
                                                }
                                            ]
                                        }
                                    ]
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
                                    xtype: 'b4savebutton',
                                    itemId:'btnSave'
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
                                    xtype: 'b4closebutton',
                                    itemId: 'btnClose'
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