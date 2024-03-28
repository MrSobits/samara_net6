// исползутеся model BasePersonalAccount
Ext.define('B4.view.regop.owner.PersonalAccountAddWindow', {
    extend: 'B4.form.Window',

    alias: 'widget.paowneraccountaddwin',

    modal: true,

    width: 700,
    height: 550,
    bodyPadding: 5,
    closeAction: 'destroy',
    requires: [
        'B4.store.RealityObject',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.ux.button.ChangeValue',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.button.Close',
        'B4.store.realityobj.Room',
        'B4.form.field.AreaShareField',
        'B4.view.regop.owner.HistoryGrid',
        'B4.view.regop.owner.RoomAreaShareAddWindow',
        'B4.view.regop.owner.RoomAreaShareGrid'
    ],

    layout: {
        type: 'vbox',
        align: 'stretch'
    },

    title: 'Сведения о помещении',

    initComponent: function() {
        var me = this;

        Ext.apply(me, {
                items: [
                {
                    xtype: 'hiddenfield',
                    name: 'Id'
                },
                {
                    xtype: 'hiddenfield',
                    name: 'AccountOwner'
                },
                {
                    xtype: 'panel',
                    section: 'add',
                    border: false,
                    bodyStyle: Gkh.bodyStyle,
                    items: [
                        {
                            xtype: 'fieldset',
                            title: 'Лицевой счет',
                            items: [
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '5 0 0 0',
                                    defaults: {
                                        labelWidth: 180
                                    },
                                    items: [
                                        {
                                            fieldLabel: 'Дата открытия ЛС',
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'OpenDate',
                                            itemId: 'dfOpenDate',
                                            flex: 2,
                                            endDateField: 'DateEnd'
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Информация о помещении',
                            defaults: {
                                anchor: '100%',
                                labelWidth: 180
                            },
                            items: [
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.RealityObject',
                                    textProperty: 'Address',
                                    editable: false,
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
                                    ],
                                    name: 'RealityObject',
                                    fieldLabel: 'Жилой дом',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'container',
                                    layout: 'hbox',
                                    padding: '5 0 0 0',
                                    defaults: {
                                        labelWidth: 180
                                    },
                                    items: [
                                        {
                                            fieldLabel: '№ квартиры/помещения',
                                            xtype: 'textfield',
                                            name: 'RoomInfo',
                                            editable: false,
                                            allowBlank: false,
                                            flex: 6,
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'hiddenfield',
                                            name: 'Rooms'
                                        },
                                        {
                                            xtype: 'button',
                                            text: 'Добавить',
                                            itemId: 'roomAddBtn',
                                            flex: 1
                                        }
                                    ]
                                },
                                {
                                    margin: '5 0 0 0',
                                    xtype: 'numberfield',
                                    fieldLabel: 'Размер взноса на КР',
                                    name: 'Tariff',
                                    editable: false,
                                    hideTrigger: true,
                                    allowDecimals: true,
                                    labelWidth: 180
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    section: 'edit',
                    flex: 1,
                    border: false,
                    items: [
                    {
                        xtype: 'panel',
                        border: false,
                        title: 'Сведения о помещении',
                        bodyStyle: Gkh.bodyStyle,
                        items: [
                            {
                                xtype: 'fieldset',
                                title: 'Лицевой счет',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            flex: 1,
                                            labelWidth: 180
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                fieldLabel: 'Лицевой счет',
                                                disabled: true,
                                                name: 'PersonalAccountNum'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        padding: '5 0 0 0',
                                        defaults: {
                                            // hideTrigger: true,
                                            labelWidth: 180
                                        },
                                        items: [
                                            {
                                                fieldLabel: 'Дата открытия ЛС',
                                                xtype: 'datefield',
                                                format: 'd.m.Y',
                                                name: 'OpenDate',
                                                itemId: 'dfOpenDate',
                                                //readOnly: true,
                                                flex: 2,
                                                //vtype: 'daterange',
                                                endDateField: 'DateEnd' // id of the end date field
                                            },
                                            {
                                                xtype: 'changevalbtn',
                                                margins: '0 0 0 5',
                                                flex: 1,
                                                className: 'BasePersonalAccount',
                                                valueFieldXtype: 'datefield',
                                                propertyName: 'OpenDate',
                                                onValueSaved: function(val) {
                                                    var numfield = this.up('container').down('datefield[name=OpenDate]');
                                                    numfield.setValue(Ext.Date.parse(val, 'd.m.Y'));
                                                }
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        name: 'CloseDateContainer',
                                        padding: '5 0 0 0',
                                        defaults: {
                                            hideTrigger: true,
                                            labelWidth: 180
                                        },
                                        items: [
                                            {
                                                fieldLabel: 'Дата закрытия ЛС',
                                                xtype: 'datefield',
                                                name: 'CloseDate',
                                                format: 'd.m.Y',
                                                readOnly: true,
                                                flex: 2,
                                                //vtype: 'daterange',
                                                startDateField: 'OpenDate' // id of the start date field
                                            },
                                            {
                                                xtype: 'changevalbtn',
                                                margins: '0 0 0 5',
                                                flex: 1,
                                                className: 'BasePersonalAccount',
                                                valueFieldXtype: 'datefield',
                                                propertyName: 'CloseDate',
                                                onValueSaved: function(val) {
                                                    var numfield = this.up('container').down('datefield[name=CloseDate]');
                                                    numfield.setValue(Ext.Date.parse(val, 'd.m.Y'));
                                                }
                                            }
                                        ]
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                title: 'Информация о помещении',
                                defaults: {
                                    anchor: '100%',
                                    labelWidth: 180
                                },
                                items: [
                                    {
                                        xtype: 'b4selectfield',
                                        store: 'B4.store.RealityObject',
                                        textProperty: 'Address',
                                        editable: false,
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
                                        ],
                                        name: 'RealityObject',
                                        fieldLabel: 'Жилой дом',
                                        allowBlank: false,
                                        disabled: true
                                    },
                                    {
                                        xtype: 'b4selectfield',
                                        store: 'B4.store.realityobj.Room',
                                        textProperty: 'RoomNum',
                                        editable: false,
                                        columns: [
                                            {
                                                text: 'Номер квартиры',
                                                dataIndex: 'RoomNum',
                                                flex: 1
                                            }
                                        ],
                                        name: 'Room',
                                        fieldLabel: '№ квартиры/помещения',
                                        allowBlank: false,
                                        disabled: true
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'ChamberNum',
                                        fieldLabel: 'Номер комнаты',
                                        disabled: true
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            hideTrigger: true,
                                            flex: 1,
                                            allowDecimals: true
                                        },
                                        items: [
                                            {
                                                xtype: 'areasharefield',
                                                fieldLabel: 'Доля собственности',
                                                name: 'AreaShare',
                                                //editable: false,
                                                labelWidth: 180
                                            },
                                            {
                                                xtype: 'changevalbtn',
                                                margins: '0 0 0 10',
                                                decimalPrecision: Gkh.config.RegOperator.GeneralConfig.AreaShareConfig.DecimalsAreaShare,
                                                className: 'BasePersonalAccount',
                                                valueFieldXtype: 'areasharefield',
                                                propertyName: 'AreaShare',
                                                onValueSaved: function(val) {
                                                    var numfield = this.up('container').down('areasharefield[name=AreaShare]');
                                                    numfield.setValue(val);
                                                }
                                            }
                                        ]
                                    },
                                    {
                                        margin: '5 0 0 0',
                                        xtype: 'numberfield',
                                        fieldLabel: 'Размер взноса на КР',
                                        name: 'Tariff',
                                        editable: false,
                                        hideTrigger: true,
                                        allowDecimals: true,
                                        labelWidth: 180
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                title: 'Договор',
                                defaults: {
                                    anchor: '100%',
                                    labelWidth: 180
                                },
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            flex: 1,
                                            labelWidth: 180,
                                            allowBlank: false
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                fieldLabel: 'Номер договора',
                                                itemId: 'tfContractNumber',
                                                name: 'ContractNumber',
                                                maxLength: 255
                                            },
                                            {
                                                fieldLabel: 'Дата заключения договора',
                                                labelAlign: 'right',
                                                xtype: 'datefield',
                                                itemId: 'dfContractDate',
                                                name: 'ContractDate'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        margin: '5 0 0 0',
                                        layout: 'hbox',
                                        defaults: {
                                            flex: 1,
                                            labelWidth: 180
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                fieldLabel: 'Код/номер документа',
                                                name: 'DocumentNumber',
                                                maxLength: 255
                                            },
                                            {
                                                fieldLabel: 'Дата выдачи документа',
                                                xtype: 'datefield',
                                                name: 'DocumentRegistrationDate',
                                                labelAlign: 'right'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        margin: '5 0 0 0',
                                        layout: 'hbox',
                                        defaults: {
                                            flex: 1,
                                            labelWidth: 180
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                fieldLabel: 'Тип документа о праве собственности',
                                                name: 'OwnershipDocumentType',
                                                maxLength: 255
                                            },
                                            {
                                                fieldLabel: 'Договор отправлен',
                                                xtype: 'datefield',
                                                name: 'ContractSendDate',
                                                labelAlign: 'right'
                                            }
                                        ]
                                    },
                                    {
                                        margin: '5 0 0 0',
                                        xtype: 'b4filefield',
                                        fieldLabel: 'Файл',
                                        itemId: 'ffContractDocument',
                                        name: 'ContractDocument',
                                        allowBlank: false
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        xtype: 'paowneraccounthistorygrid',
                        border: false
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
                                    text: 'Переходы',
                                    iconCls: 'icon-arrow-out',
                                    menu: {
                                        xtype: 'menu',
                                        items: [
                                            {
                                                text: 'Перейти к ЛС',
                                                action: 'redirecttopersonalaccount',
                                                iconCls: 'icon-arrow-out'
                                            },
                                            {
                                                text: 'Перейти к помещению',
                                                action: 'redirecttoroom',
                                                iconCls: 'icon-arrow-out'
                                            }
                                        ]
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4closebutton',
                                    text: 'Закрыть'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    },

    setAddLayout: function (isAdd) {
        var section = this.query(isAdd ? '[section="edit"]' : '[section="add"]')[0];

        if (section) {
            section.destroy();
        }

        this.setHeight(isAdd ? 300 : 550);
        this.center();
    }
});