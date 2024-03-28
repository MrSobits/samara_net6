Ext.define('B4.view.longtermprobject.propertyownerprotocols.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.propertyownerprotocolseditwin',
    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.form.SelectField',
        'B4.form.FileField',
        'B4.form.ComboBox',
        'B4.form.EnumCombo',
        'B4.ux.form.field.GkhTimeField',
        'B4.enums.PropertyOwnerProtocolType',
        'B4.enums.FormVoting',
        'B4.enums.LegalityMeeting',
        'B4.enums.VotingStatus',
        'B4.enums.YesNo',
        'B4.view.realityobj.ProtocolNpaPanel'
    ],

    width: 600,
    minHeight: 500,
    maxHeight: 700,
    bodyPadding: 5,
    itemId: 'propertyownerprotocolsEditWindow',
    title: 'Редактирование',
    border: false,

    initComponent: function() {
        var me = this;

        var contragentStore = Ext.create('B4.store.Contragent', {
            proxy: {
                type: 'b4proxy',
                controllerName: 'Contragent',
                listAction: 'GetAllActiveContragent'
            }
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: false,
                    items: [
                        {
                            xtype: 'panel',
                            title: 'Общие сведения',
                            bodyStyle: Gkh.bodyStyle,
                            border: 0,

                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right'
                            },

                            items:[
                                {
                                    xtype: 'b4combobox',
                                    name: 'TypeProtocol',
                                    items: B4.enums.PropertyOwnerProtocolType.getItems(),
                                    displayField: 'Display',
                                    valueField: 'Value',
                                    fieldLabel: 'Тип протокола',
                                    padding: '10 0 0 0',
                                    editable: false
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: 'hbox',
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1,
                                        allowBlank: false
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'DocumentNumber',
                                            fieldLabel: 'Номер'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'DocumentDate',
                                            fieldLabel: 'Дата',
                                            maxValue: new Date(),
                                            validator: function (v) {
                                                if (this.parseDate(v) > this.maxValue) {
                                                    return 'Выбранная дата больше текущей даты';
                                                }

                                                return true;
                                            }
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
                                    fieldLabel: 'Описание',
                                    name: 'Description'
                                },
                                {
                                    xtype: 'numberfield',
                                    fieldLabel: 'Сумма займа',
                                    name: 'LoanAmount',
                                    hideTrigger: true,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    decimalSeparator: ','
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Borrower',
                                    fieldLabel: 'Заемщик',
                                    store: contragentStore,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальный район',
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
                                                url: '/Municipality/ListMoAreaWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'b4selectfield',
                                    name: 'Lender',
                                    fieldLabel: 'Кредитор',
                                    store: contragentStore,
                                    columns: [
                                        { text: 'Наименование', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } },
                                        {
                                            text: 'Муниципальный район',
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
                                                url: '/Municipality/ListMoAreaWithoutPaging'
                                            }
                                        },
                                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                                    ]
                                },
                                {
                                    xtype: 'fieldset',
                                    padding: '5',
                                    title: 'Количественные характеристики',
                                    name: 'Parameters',
                                    items: [
                                        {
                                            xtype: 'container',
                                            padding: '0 0 5 0',
                                            layout: 'hbox',
                                            defaults: {
                                                xtype: 'numberfield',
                                                hideTrigger: true,
                                                keyNavEnabled: false,
                                                mouseWheelEnabled: false,
                                                allowBlank: false,
                                                decimalSeparator: ',',
                                                minValue: 0,
                                                labelAlign: 'right',
                                                flex: 1
                                            },
                                            items: [
                                                {
                                                    labelWidth: 180,
                                                    name: 'NumberOfVotes',
                                                    fieldLabel: 'Количество голосов (кв.м.)'
                                                },
                                                {
                                                    labelWidth: 220,
                                                    name: 'TotalNumberOfVotes',
                                                    fieldLabel: 'Общее количество голосов (кв.м.)'
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'numberfield',
                                            hideTrigger: true,
                                            labelWidth: 180,
                                            labelAlign: 'right',
                                            name: 'PercentOfParticipating',
                                            fieldLabel: 'Доля принявших участие (%)',
                                            anchor: '50%',
                                            allowBlank: false,
                                            keyNavEnabled: false,
                                            mouseWheelEnabled: false,
                                            decimalSeparator: ',',
                                            minValue: 0
                                        }
                                    ]
                                },
                                {
                                    xtype: 'container',
                                    name: 'VotingDetails',
                                    hidden: true,
                                    disabled: true,
                                    defaults: {
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    layout: {
                                        type: 'vbox',
                                        align: 'stretch'
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Форма проведения голосования',
                                            name: 'FormVoting',
                                            enumName: 'B4.enums.FormVoting',
                                            allowBlank: false,
                                            editable: false,
                                            hideTrigger: false
                                        },
                                        {
                                            xtype: 'fieldset',
                                            title: 'Сведения о дате и месте проведения голосования',
                                            votingUseSystem: false,
                                            hidden: true,
                                            disabled: true,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 140,
                                                flex: 1,
                                                allowBlank: false
                                            },
                                            items: [
                                                {
                                                    xtype: 'datefield',
                                                    fieldLabel: 'Дата окончания приема решений',
                                                    name: 'EndDateDecision'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PlaceDecision',
                                                    fieldLabel: 'Место приема решений',
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'PlaceMeeting',
                                                    fieldLabel: 'Место проведения собрания',
                                                    maxLength: 100
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        labelWidth: 140,
                                                        flex: 1,
                                                        allowBlank: false
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата проведения собрания',
                                                            name: 'DateMeeting'
                                                        },
                                                        {
                                                            xtype: 'gkhtimefield',
                                                            fieldLabel: 'Время проведения собрания',
                                                            increment: 30,
                                                            name: 'TimeMeeting'
                                                        }
                                                    ]
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'fieldset',
                                            title: 'Сведения о дате и месте проведения голосования',
                                            votingUseSystem: true,
                                            hidden: true,
                                            disabled: true,
                                            layout: {
                                                type: 'vbox',
                                                align: 'stretch'
                                            },
                                            defaults: {
                                                labelAlign: 'right',
                                                labelWidth: 140,
                                                flex: 1,
                                                allowBlank: false
                                            },
                                            items: [
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        labelWidth: 140,
                                                        flex: 1,
                                                        allowBlank: false
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата начала проведения голосования',
                                                            name: 'VotingStartDate'
                                                        },
                                                        {
                                                            xtype: 'gkhtimefield',
                                                            fieldLabel: 'Время начала проведения голосования',
                                                            increment: 30,
                                                            name: 'VotingStartTime'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right',
                                                        labelWidth: 140,
                                                        flex: 1,
                                                        allowBlank: false
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата окончания проведения голосования',
                                                            name: 'VotingEndDate'
                                                        },
                                                        {
                                                            xtype: 'gkhtimefield',
                                                            fieldLabel: 'Время окончания проведения голосования',
                                                            increment: 30,
                                                            name: 'VotingEndTime'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Порядок приема решений собственников',
                                                    name: 'OrderTakingDecisionOwners'
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    name: 'OrderAcquaintanceInfo',
                                                    fieldLabel: 'Порядок ознакомления с информацией',
                                                    maxLength: 100
                                                }
                                            ]
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Ежегодное собрание',
                                            name: 'AnnuaLMeeting',
                                            enumName: 'B4.enums.YesNo',
                                            allowBlank: false,
                                            editable: false,
                                            hideTrigger: false
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Правомерность собрания',
                                            name: 'LegalityMeeting',
                                            enumName: 'B4.enums.LegalityMeeting',
                                            allowBlank: false,
                                            editable: false,
                                            hideTrigger: false
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Статус',
                                            name: 'VotingStatus',
                                            enumName: 'B4.enums.VotingStatus',
                                            allowBlank: false,
                                            editable: false,
                                            hideTrigger: false
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'protocolnpapanel'
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