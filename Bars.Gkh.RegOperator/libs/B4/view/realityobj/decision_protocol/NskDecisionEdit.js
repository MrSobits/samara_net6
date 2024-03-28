Ext.define('B4.view.realityobj.decision_protocol.NskDecisionEdit', {
    extend: 'B4.form.Window',
    alias: 'widget.nskdecisionedit',

    requires: [
        'B4.form.ComboBox',
        'B4.form.SelectField',
        'B4.form.EnumCombo',
        'B4.ux.button.Close',
        'B4.form.FileField',
        'B4.ux.button.Save',
        'B4.ux.button.Add',
        'B4.ux.form.field.GkhTimeField',
        'B4.store.ManagingOrganization',
        'B4.enums.MkdManagementDecisionType',
        'B4.enums.CrFundFormationDecisionType',
        'B4.store.ManagingOrganization',
        'Ext.grid.plugin.CellEditing',
        'B4.enums.AccountOwnerDecisionType',
        'B4.enums.AccountManagementType',
        'B4.enums.FormVoting',
        'B4.enums.LegalityMeeting',
        'B4.enums.VotingStatus',
        'B4.enums.YesNo',
        'B4.view.realityobj.ProtocolNpaPanel',

        // decisions forms
        'B4.view.realityobj.decision.MonthlyFee',
        'B4.view.realityobj.decision.JobYear',
        'B4.view.realityobj.decision.Protocol',
        'B4.view.realityobj.decision.PenaltyDelay',
        'B4.enums.CoreDecisionType'
    ],

    modal: true,
    title: 'Протокол решения',
    border: false,
    width: 768,
    height: 600,
    bodyPadding: 5,
    closeAction: 'destroy',
    autoScroll: true,
    protocolId: 0,

    __OWNER: 0,
    __GOV: 1,
    __OTHER: 2,

    initComponent: function() {
        var me = this,
            contragentStore = Ext.create('B4.store.Contragent'),
            contragentNpaStore = Ext.create('B4.store.Contragent', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'Contragent',
                    listAction: 'GetAllActiveContragent'
                }
            });;

        contragentStore.on('beforeload', function(store, operation) {
            operation.params.decisionType = me.down('form[entity=AccountOwnerDecision] b4enumcombo').getValue();
        });

        Ext.apply(me, {
            items:
                {
                    xtype: 'tabpanel',
                    flex: 1,
                    border: 0,
                    items: [
                        {
                            xtype: 'panel',
                            title: 'Сведения о протоколе',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            border: 0,
                            defaults: {
                                defaults: {
                                    margin: '5 5 5 5'
                                },
                                border: '0 0 0 0'
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    padding: '5 5',
                                    fieldLabel: 'Тип протокола',
                                    enumName: 'B4.enums.CoreDecisionType',
                                    anchor: '100%',
                                    includeEmpty: false,
                                    enumItems: [],
                                    allowBlank: false,
                                    name: 'ProtocolType',
                                    labelAlign: 'right',
                                    listeners: {
                                        change: me.onProtocolTypeChange,
                                        scope: me
                                    },
                                    listConfig: {
                                        listeners: {
                                            refresh: function(picker) {
                                                var cb = picker.pickerField;
                                                cb.fireEvent('refresh', cb);
                                            }
                                        }
                                    },
                                    value: 10
                                },
                                {
                                    xtype: 'form',
                                    bodyStyle: Gkh.bodyStyle,
                                    ownerType: me.__OWNER,
                                    border: 0,
                                    items: [
                                        {
                                            xtype: 'hiddenfield',
                                            name: 'Id',
                                            fname: 'Protocol'
                                        },
                                        {
                                            xtype: 'fieldset',
                                            title: 'Решение собрания собственников жилых помещений',
                                            defaults: {
                                                anchor: '100%',
                                                bodyStyle: Gkh.bodyStyle
                                            },
                                            items: [
                                                {
                                                    xtype: 'protocoldecision'
                                                },
                                                {
                                                    xtype: 'form',
                                                    entity: 'CrFundFormationDecision',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'hidden',
                                                            name: 'Id'
                                                        },
                                                        {
                                                            xtype: 'checkbox',
                                                            name: 'IsChecked',
                                                            //checked: true,
                                                            boxLabel: 'Способ формирования фонда КР',
                                                            margin: '0 0 0 10',
                                                            flex: 1
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            col: 'right',
                                                            width: 350,
                                                            layout: 'anchor',
                                                            items: {
                                                                anchor: '100%',
                                                                xtype: 'b4enumcombo',
                                                                enumName: 'B4.enums.CrFundFormationDecisionType',
                                                                includeEmpty: false,
                                                                enumItems: [],
                                                                value: 1,
                                                                name: 'Decision',
                                                                margin: '5 5 5 5'
                                                            }
                                                        }
                                                    ],
                                                    setValues: function(v) {
                                                        this.getForm().setValues(v);
                                                    }
                                                },
                                                {
                                                    xtype: 'form',
                                                    entity: 'AccountOwnerDecision',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'hidden',
                                                            name: 'Id'
                                                        },
                                                        {
                                                            xtype: 'checkbox',
                                                            specialacc: true,
                                                            disabled: true,
                                                            name: 'IsChecked',
                                                            boxLabel: 'Владелец специального счета',
                                                            flex: 1,
                                                            forceSelect: true,
                                                            margin: '0 0 0 10'
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            width: 350,
                                                            col: 'right',
                                                            layout: 'anchor',
                                                            items: [
                                                                {
                                                                    xtype: 'b4enumcombo',
                                                                    anchor: '100%',
                                                                    enumName: 'B4.enums.AccountOwnerDecisionType',
                                                                    includeEmpty: false,
                                                                    enumItems: [],
                                                                    margin: '5 5 5 5',
                                                                    name: 'DecisionType'
                                                                }
                                                            ]
                                                        }
                                                    ],
                                                    setValues: function(v) {
                                                        this.getForm().setValues(v);
                                                    }
                                                },
                                                {
                                                    xtype: 'form',
                                                    entity: 'CreditOrgDecision',
                                                    layout: 'anchor',
                                                    items: [
                                                        {
                                                            xtype: 'hidden',
                                                            name: 'Id'
                                                        },
                                                        {
                                                            margin: '0 0 0 10',
                                                            xtype: 'checkbox',
                                                            disabled: true,
                                                            name: 'IsChecked',
                                                            boxLabel: 'Кредитная организация',
                                                            flex: 1
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            flex: 1,
                                                            col: 'right',
                                                            layout: {
                                                                type: 'vbox',
                                                                align: 'stretch'
                                                            },
                                                            defaults: {
                                                                flex: 1,
                                                                labelWidth: 140,
                                                                allowBlank: true
                                                            },
                                                            items: [
                                                                {
                                                                    xtype: 'b4selectfield',
                                                                    fieldLabel: 'Наименование',
                                                                    name: 'Decision',
                                                                    labelAlign: 'right',
                                                                    store: 'B4.store.CreditOrg',
                                                                    isGetOnlyIdProperty: false,
                                                                    allowBlank: false,
                                                                    margin: '5 5 5 5'
                                                                }
                                                            ]
                                                        }
                                                    ],
                                                    setValues: function(v) {
                                                        this.getForm().setValues(v);
                                                    }
                                                },
                                                {
                                                    xtype: 'monthlyfeedecision'
                                                },
                                                {
                                                    xtype: 'form',
                                                    entity: 'MinFundAmountDecision',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stertch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'hidden',
                                                            name: 'Id'
                                                        },
                                                        {
                                                            xtype: 'checkbox',
                                                            specialacc: true,
                                                            disabled: true,
                                                            name: 'IsChecked',
                                                            boxLabel: 'Минимальный размер фонда КР',
                                                            flex: 1,
                                                            margin: '0 0 0 10'
                                                        },
                                                        {
                                                            col: 'right',
                                                            labelWidth: 120,
                                                            xtype: 'numberfield',
                                                            hideTrigger: true,
                                                            disabled: true,
                                                            name: 'Decision',
                                                            minValue: 0,
                                                            maxValue: 100,
                                                            margin: '5 5 5 5'
                                                        }
                                                    ],
                                                    setValues: function(v) {
                                                        var decField = this.down('numberfield[name=Decision]');
                                                        decField.setMinValue(v.Default);
                                                        v.Decision = v.Decision || v.Default;
                                                        this.getForm().setValues(v);
                                                        decField.validate();
                                                    }
                                                },
                                                {
                                                    xtype: 'form',
                                                    entity: 'AccumulationTransferDecision',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'hidden',
                                                            name: 'Id',
                                                            flex: 0
                                                        },
                                                        {
                                                            xtype: 'checkbox',
                                                            specialacc: false,
                                                            //checked: true,
                                                            name: 'IsChecked',
                                                            boxLabel:
                                                                'Сумма ранее накопленных средств, перечисляемая на спецсчет',
                                                            flex: 2,
                                                            margin: '0 0 0 10'
                                                        },
                                                        {
                                                            labelWidth: 120,
                                                            col: 'right',
                                                            xtype: 'numberfield',
                                                            hideTrigger: true,
                                                            name: 'Decision',
                                                            minValue: 0,
                                                            allowDecimals: true,
                                                            margin: '5 5 5 5'
                                                        }
                                                    ],
                                                    setValues: function(v) {
                                                        this.getForm().setValues(v);
                                                    }
                                                },
                                                {
                                                    xtype: 'form',
                                                    entity: 'AccountManagementDecision',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'hidden',
                                                            name: 'Id',
                                                            flex: 0
                                                        },
                                                        {
                                                            xtype: 'checkbox',
                                                            specialacc: false,
                                                            name: 'IsChecked',
                                                            boxLabel: 'Ведение лицевых счетов',
                                                            flex: 1,
                                                            margin: '0 0 0 10'
                                                        },
                                                        {
                                                            xtype: 'container',
                                                            width: 350,
                                                            col: 'right',
                                                            layout: 'anchor',
                                                            items: [
                                                                {
                                                                    xtype: 'b4enumcombo',
                                                                    anchor: '100%',
                                                                    enumName: 'B4.enums.AccountManagementType',
                                                                    includeEmpty: false,
                                                                    enumItems: [],
                                                                    margin: '5 5 5 5',
                                                                    name: 'Decision'
                                                                }
                                                            ]
                                                        }
                                                    ],
                                                    setValues: function(v) {
                                                        this.getForm().setValues(v);
                                                    }
                                                },
                                                {
                                                    xtype: 'penaltydelaydecision',
                                                    height: 150
                                                },
                                                {
                                                    xtype: 'jobyeardecision',
                                                    height: 200
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'form',
                                    bodyStyle: Gkh.bodyStyle,
                                    ownerType: me.__GOV,
                                    layout: { type: 'vbox', align: 'stretch' },
                                    hidden: true,
                                    disabled: true,
                                    border: 0,
                                    items: [
                                        {
                                            xtype: 'hiddenfield',
                                            name: 'Id',
                                            fname: 'Protocol'
                                        },
                                        {
                                            xtype: 'fieldset',
                                            title: 'Решение органов государственной власти',
                                            layout: { type: 'vbox', align: 'stretch' },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Номер',
                                                    name: 'ProtocolNumber',
                                                    allowBlank: false,
                                                    labelWidth: 140
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: {
                                                        type: 'hbox'
                                                    },
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        allowBlank: false,
                                                        format: 'd.m.Y',
                                                        flex: 1,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            fieldLabel: 'Дата протокола',
                                                            name: 'ProtocolDate',
                                                            labelWidth: 140
                                                        },
                                                        {
                                                            fieldLabel: 'Дата вступления в силу',
                                                            name: 'DateStart',
                                                            labelWidth: 150
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Управление домом',
                                                    name: 'RealtyManagement',
                                                    readOnly: true,
                                                    labelWidth: 140
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Уполномоченное лицо',
                                                    name: 'AuthorizedPerson',
                                                    labelWidth: 140
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Телефон уполномоченного лица',
                                                    name: 'AuthorizedPersonPhone',
                                                    labelWidth: 200
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    fieldLabel: 'Протокол',
                                                    itemId: 'decisProtocol',
                                                    name: 'ProtocolFile',
                                                    allowBlank: false,
                                                    labelWidth: 142
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: {
                                                        type: 'hbox'
                                                    },
                                                    defaults: {
                                                        labelWidth: 150,
                                                        labelAlign: 'right',
                                                        flex: 1
                                                    },
                                                    items: [
                                                        {
                                                            xtype: 'textfield',
                                                            fieldLabel: 'Номер входящего письма',
                                                            name: 'LetterNumber'
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата входящего письма',
                                                            name: 'LetterDate',
                                                            format: 'd.m.Y'
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    margin: '10 0 15 5',
                                                    boxLabel: 'Способ формирования фонда на счету регионального оператора',
                                                    name: 'FundFormationByRegop'
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right'
                                                    },
                                                    margin: '8 0 4 5',
                                                    items: [
                                                        {
                                                            xtype: 'checkbox',
                                                            boxLabel: 'Снос МКД',
                                                            width: 135,
                                                            chbcontrol: 'Destroy',
                                                            name: 'Destroy',
                                                            listeners: {
                                                                'change': function(cmp, newValue) {
                                                                    me.down('datefield[name=DestroyDate]')
                                                                        .setDisabled(!newValue);
                                                                }
                                                            }
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата',
                                                            chbgroup: 'Destroy',
                                                            name: 'DestroyDate',
                                                            disabled: true,
                                                            flex: 1,
                                                            labelWidth: 95,
                                                            allowBlank: false
                                                        },
                                                        {
                                                            xtype: 'tbfill'
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
                                                        labelAlign: 'right'
                                                    },
                                                    margin: '4 0 10 5',
                                                    items: [
                                                        {
                                                            xtype: 'checkbox',
                                                            chbcontrol: 'Reconstruction',
                                                            width: 135,
                                                            boxLabel: 'Реконструкция МКД',
                                                            name: 'Reconstruction',
                                                            listeners: {
                                                                'change': function(cmp, newValue) {
                                                                    me.down('datefield[name=ReconstructionStart]')
                                                                        .setDisabled(!newValue);
                                                                    me.down('datefield[name=ReconstructionEnd]')
                                                                        .setDisabled(!newValue);
                                                                }
                                                            }
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата с',
                                                            chbgroup: 'Reconstruction',
                                                            disabled: true,
                                                            name: 'ReconstructionStart',
                                                            flex: 1,
                                                            labelWidth: 95,
                                                            allowBlank: false
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            fieldLabel: 'Дата по',
                                                            disabled: true,
                                                            chbgroup: 'Reconstruction',
                                                            name: 'ReconstructionEnd',
                                                            flex: 1,
                                                            labelWidth: 70,
                                                            allowBlank: false
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'checkbox',
                                                    chbcontrol: 'TakeLandForGov',
                                                    margin: '12 0 5 5',
                                                    boxLabel:
                                                        'Изъятие для государственных или муниципальных нужд земельного участка, на котором расположен МКД',
                                                    name: 'TakeLandForGov',
                                                    listeners: {
                                                        'change': function(cmp, newValue) {
                                                            me.down('datefield[name=TakeLandForGovDate]')
                                                                .setDisabled(!newValue);
                                                        }
                                                    }
                                                },
                                                {
                                                    xtype: 'datefield',
                                                    chbgroup: 'TakeLandForGov',
                                                    fieldLabel: 'Дата',
                                                    disabled: true,
                                                    width: '200px !important',
                                                    name: 'TakeLandForGovDate',
                                                    labelWidth: 70,
                                                    labelAlign: 'right',
                                                    allowBlank: false
                                                },
                                                {
                                                    xtype: 'container',
                                                    layout: {
                                                        type: 'hbox',
                                                        align: 'stretch'
                                                    },
                                                    defaults: {
                                                        labelAlign: 'right'
                                                    },
                                                    margin: '15 0 10 5',
                                                    items: [
                                                        {
                                                            xtype: 'checkbox',
                                                            chbcontrol: 'TakeApartsForGov',
                                                            boxLabel: 'Изъятие каждого жилого помещения в доме',
                                                            name: 'TakeApartsForGov',
                                                            listeners: {
                                                                'change': function(cmp, newValue) {
                                                                    me.down('datefield[name=TakeApartsForGovDate]')
                                                                        .setDisabled(!newValue);
                                                                }
                                                            }
                                                        },
                                                        {
                                                            xtype: 'datefield',
                                                            chbgroup: 'TakeApartsForGov',
                                                            fieldLabel: 'Дата',
                                                            name: 'TakeApartsForGovDate',
                                                            disabled: true,
                                                            width: 300,
                                                            labelWidth: 70,
                                                            allowBlank: false
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    fieldLabel: 'Минимальный размер взноса на КР',
                                                    ediatable: false,
                                                    name: 'MinFundPaymentSize',
                                                    labelWidth: 235,
                                                    allowDecimals: true,
                                                    decimalSeparator: ','

                                                },
                                                {
                                                    xtype: 'numberfield',
                                                    fieldLabel: 'Минимальный размер фонда',
                                                    ediatable: false,
                                                    name: 'MinFundSize',
                                                    labelWidth: 235,
                                                    allowDecimals: true,
                                                    decimalSeparator: ','
                                                }
                                            ]
                                        }
                                    ]
                                },
                                {
                                    xtype: 'form',
                                    bodyStyle: Gkh.bodyStyle,
                                    ownerType: me.__OTHER,
                                    layout: { type: 'vbox', align: 'stretch' },
                                    hidden: true,
                                    disabled: true,
                                    border: 0,
                                    items: [
                                        {
                                            xtype: 'hiddenfield',
                                            name: 'Id',
                                            fname: 'Protocol'
                                        },
                                        {
                                            xtype: 'fieldset',
                                            title: 'Решение',
                                            layout: { type: 'vbox', align: 'stretch' },
                                            defaults: {
                                                labelAlign: 'right'
                                            },
                                            items: [
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Номер',
                                                    name: 'ProtocolNumber',
                                                    allowBlank: false,
                                                    labelWidth: 140
                                                },
                                                {
                                                    xtype: 'container',
                                                    padding: '0 0 5 0',
                                                    layout: {
                                                        type: 'hbox'
                                                    },
                                                    defaults: {
                                                        xtype: 'datefield',
                                                        allowBlank: false,
                                                        format: 'd.m.Y',
                                                        flex: 1,
                                                        labelAlign: 'right'
                                                    },
                                                    items: [
                                                        {
                                                            fieldLabel: 'Дата протокола',
                                                            name: 'ProtocolDate',
                                                            labelWidth: 140
                                                        },
                                                        {
                                                            fieldLabel: 'Дата вступления в силу',
                                                            name: 'DateStart',
                                                            labelWidth: 150
                                                        }
                                                    ]
                                                },
                                                {
                                                    xtype: 'textfield',
                                                    fieldLabel: 'Описание',
                                                    name: 'Description',
                                                    labelWidth: 140
                                                },
                                                {
                                                    xtype: 'b4filefield',
                                                    fieldLabel: 'Протокол',
                                                    name: 'ProtocolFile',
                                                    allowBlank: false,
                                                    labelWidth: 140
                                                }
                                            ]
                                        }
                                    ]
                                }
                            ]
                        },
                        {
                            xtype: 'form',
                            title: 'Сведения о голосовании',
                            bodyStyle: Gkh.bodyStyle,
                            border: 0,
                            entity: 'Protocol',
                            name: 'VotingDetails',
                            ownerType: me.__OWNER,
                            border: 0,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            defaults: {
                                labelAlign: 'right',
                                labelWidth: 150
                            },
                            items: [
                                {
                                    xtype: 'b4enumcombo',
                                    fieldLabel: 'Форма проведения голосования',
                                    name: 'FormVoting',
                                    enumName: 'B4.enums.FormVoting',
                                    allowBlank: false,
                                    editable: false,
                                    hideTrigger: false,
                                    listeners: {
                                        change: me.onFormVotingChange,
                                        scope: me
                                    }
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
                            ],
                            setValues: function(v) {
                                this.getForm().setValues(v);
                            }
                        },
                        {
                            ownerType: me.__GOV,
                            hidden: true,
                            xtype: 'protocolnpapanel'
                        }
                    ]
                },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    ownerType: me.__OWNER,
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
                                    text: 'Сформировать уведомление',
                                    itemId: 'formconfirmbtn'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Скачать договор',
                                    itemId: 'downloadContract'
                                }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    itemId: 'protocolStateBtn',
                                    text: 'Статус',
                                    menu: [],
                                    owner: me.__OWNER
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    ownerType: me.__GOV,
                    hidden: true,
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }, '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    type: 'stateBtn',
                                    itemId: 'stateDecBtn',
                                    text: 'Статус',
                                    menu: [],
                                    owner: me.__GOV
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    ownerType: me.__OTHER,
                    hidden: true,
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'b4savebutton'
                                }
                            ]
                        }, '->',
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-accept',
                                    type: 'stateBtn',
                                    itemId: 'stateDecBtn',
                                    text: 'Статус',
                                    menu: [],
                                    owner: me.__OTHER
                                },
                                {
                                    xtype: 'b4closebutton',
                                    listeners: {
                                        click: function (btn) {
                                            btn.up('window').close();
                                        }
                                    }
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
        me.__initDisable();
    },

    __initDisable: function() {
        var win = this,
            mainForm,
            subforms,
            subformsHash = {};

        mainForm = win.down('form');
        subforms = Ext.ComponentQuery.query('form', mainForm);
        Ext.each(subforms, function(sfr) {
            subformsHash[sfr.entity] = sfr;
        });


        Ext.Object.each(subformsHash, function(key, frm) {
            var fields,
                chb = frm.down('checkbox'),
                generateNotif = win.down('#formconfirmbtn'),
                downloadContr = win.down('#donwloadContract'),
                decisionEnum = win.down('b4enumcombo[name=Decision]'),
                decisionTypeEnum = win.down('b4enumcombo[name=DecisionType]'),
                val;

            if (decisionEnum.value != B4.enums.CrFundFormationDecisionType.SpecialAccount) {
                generateNotif.disable();
            }

            if (decisionEnum.value != B4.enums.CrFundFormationDecisionType.RegOpAccount) {
                if (decisionEnum.value != B4.enums.CrFundFormationDecisionType.SpecialAccount
                    && decisionTypeEnum.value != B4.enums.AccountOwnerDecisionType.RegOp) {
                    downloadContr.disable();
                }
            }

            if (key === 'Protocol' || key === 'MonthlyFeeAmountDecision') {

            } else if (key === 'JobYearDecision') { // 
                val = chb.getValue();
                frm.down('grid').setDisabled(!val);
            } else {
                val = chb.getValue();
                fields = Ext.ComponentQuery.query('[col=right] field', frm);
                Ext.each(fields, function(f) {
                    f.setDisabled(!val);
                });
            }
        });
    },

    loadValues: function(values) {
        var win = this,
            mainForm,
            subforms,
            subformsHash = {};

        mainForm = win.down('form');
        subforms = Ext.ComponentQuery.query('form', win);
        Ext.each(subforms, function(sfr) {
            if (Ext.isDefined(subformsHash[sfr.entity])) {
                subformsHash[sfr.entity] = [subformsHash[sfr.entity], sfr];
            } else {
                subformsHash[sfr.entity] = sfr;
            }
        });

        win.__initDisable();
        if (values) {
            Ext.Object.each(values, function(key, value) {
                var frms = subformsHash[key];

                Ext.each(frms, function(frm) {
                    if (frm) {
                        if (value != null) {
                            frm.setValues(value);
                        } else {
                            frm.down('[name=Id]').setValue(null);
                        }
                    }
                });
            });
        }
        if (values.Protocol && values.Protocol.ManOrgName) {
            mainForm.down('[name=ManOrgName]').setValue(values.Protocol.ManOrgName);
        }
        if (values.PaymentAndFundDecisions && values.PaymentAndFundDecisions.MinFundPaymentSize) {
            win.down('[name=MinFundPaymentSize]').setValue(values.PaymentAndFundDecisions.MinFundPaymentSize);
        }
        if (values.PaymentAndFundDecisions && values.PaymentAndFundDecisions.MinFundSize) {
            win.down('[name=MinFundSize]').setValue(values.PaymentAndFundDecisions.MinFundSize);
        }
    },

    onProtocolTypeChange: function (combo, newVal, oldVal) {
        if (oldVal == newVal) {
            return;
        }

        var me = this,
            form = combo.up('window').getForm(),
            owners = Ext.ComponentQuery.query(Ext.String.format('[ownerType={0}]', me.__OWNER), combo.up('nskdecisionedit')),
            govs = Ext.ComponentQuery.query(Ext.String.format('[ownerType={0}]', me.__GOV), combo.up('nskdecisionedit')),
            others = Ext.ComponentQuery.query(Ext.String.format('[ownerType={0}]', me.__OTHER), combo.up('nskdecisionedit')),
            isOwnersType, isGovernmentType, isOtherType;

        isOwnersType = newVal == B4.enums.CoreDecisionType.Owners;
        isGovernmentType = newVal == B4.enums.CoreDecisionType.Government;
        isOtherType = !(isOwnersType || isGovernmentType);

        Ext.each(owners, function (item) {
            if (item.tab) {
                item.tab.setVisible(isOwnersType);
                item.tab.setDisabled(!isOwnersType);
            } else {
                item.setVisible(isOwnersType);
                item.setDisabled(!isOwnersType);
            }
            item.setDisabled(!isOwnersType);
        }, me);

        Ext.each(govs, function(item) {
            if (item.tab) {
                item.tab.setVisible(isGovernmentType);
                item.tab.setDisabled(!isGovernmentType);
            } else {
                item.setVisible(isGovernmentType);
                item.setDisabled(!isGovernmentType);
            }
            item.setDisabled(!isGovernmentType);
        }, me);

        Ext.each(others, function (item) {
            item.setVisible(isOtherType);
            item.setDisabled(!isOtherType);
        }, me);
        form.isValid();
    },

    onFormVotingChange: function (combo, newVal, oldVal) {
        if (oldVal == newVal) {
            return;
        }

        var form = combo.up('form'),
            notUseSystemFieldSet = form.down('fieldset[votingUseSystem=false]'),
            useSystemFieldSet = form.down('fieldset[votingUseSystem=true]'),
            notUse = newVal === B4.enums.FormVoting.Extramural ||
                newVal === B4.enums.FormVoting.Intramural ||
                newVal === B4.enums.FormVoting.FullTime;

        if (notUse) {
            notUseSystemFieldSet.setVisible(true);
            notUseSystemFieldSet.setDisabled(false);

            useSystemFieldSet.setVisible(false);
            useSystemFieldSet.setDisabled(true);
        } else {
            notUseSystemFieldSet.setVisible(false);
            notUseSystemFieldSet.setDisabled(true);

            useSystemFieldSet.setVisible(true);
            useSystemFieldSet.setDisabled(false);
        }
    }
});