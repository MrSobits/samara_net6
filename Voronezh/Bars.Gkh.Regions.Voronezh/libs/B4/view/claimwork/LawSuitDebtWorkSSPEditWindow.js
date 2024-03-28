Ext.define('B4.view.claimwork.LawSuitDebtWorkSSPEditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.lawsuitdebtworksspeditwindow',
    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: 'form',
    width: 1000,
    minWidth: 800,
    height: 800,
    resizable: true,
    bodyPadding: 3,
    title: 'Взыскание долга с дольщика',
    closeAction: 'destroy',
    trackResetOnLoad: true,

    requires: [
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.enums.ClaimWork.TypeLawsuitDocument',
        'B4.enums.ClaimWork.CollectDebtFrom',
        'B4.enums.LawsuitCollectionDebtReasonStoppedType',
        'B4.enums.LawsuitCollectionDebtType',
        'B4.enums.LawsuitFactInitiationType',
        'B4.enums.LawsuitCollectionDebtDocumentType',
        'B4.store.dict.JurInstitution',
        'B4.form.EnumCombo',
        'B4.form.FileField',
        'B4.store.claimwork.LawsuitOwnerInfoByDocId',
        'B4.view.claimwork.LawSuitDebtWorkSSPDocumentGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelAlign: 'right',
                labelWidth: 150
            },
            items: [
                {
                    xtype: 'container',
                    padding: '0 0 5 0',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'b4selectfield',
                            store: 'B4.store.claimwork.LawsuitOwnerInfoByDocId',
                            textProperty: 'Name',
                            editable: false,
                            itemId: 'sfLawOI',
                            columns: [

                                {
                                    text: 'Собственник',
                                    dataIndex: 'Name',
                                    flex: 1,
                                    filter: { xtype: 'textfield' }
                                }
                            ],
                            name: 'LawsuitOwnerInfo',
                            fieldLabel: 'Собственник',
                            allowBlank: false
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Сумма, погашенная до исполнительного производства',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Размер погашения',
                                            enumName: 'B4.enums.LawsuitCollectionDebtType',
                                            name: 'CbSize'
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Тип заявления',
                                            enumName: 'B4.enums.LawSuitDebtWorkType',
                                            name: 'DebtWorkType',
                                            itemId: 'debtWorkType'
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
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'CbDebtSum',
                                            fieldLabel: 'Сумма долга (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'CbPenaltyDebt',
                                            fieldLabel: 'Сумма пени (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true
                                        }
                                    ]
                                }

                            ]
                        },
                        {
                            xtype: 'fieldset',
                            title: 'Исполнительное производство',
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            items: [
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Факт возбуждения',
                                            enumName: 'B4.enums.LawsuitFactInitiationType',
                                            name: 'CbFactInitiated'
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'CbDateInitiated',
                                            fieldLabel: 'Дата возбуждения/отказа',
                                            maxLength: 100,
                                            labelAlign: 'right'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'FactInitiatedNote',
                                    fieldLabel: 'Примечание',
                                    maxLength: 2000,
                                    labelAlign: 'right',
                                    labelWidth: 170
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4selectfield',
                                            store: 'B4.store.dict.JurInstitution',
                                            name: 'CbStationSsp',
                                            fieldLabel: 'Участок ССП',
                                            labelWidth: 170,
                                            labelAlign: 'right',
                                            textProperty: 'ShortName',
                                            columns: [
                                                { dataIndex: 'Municipality', text: 'Муниципальное образование', flex: 1, filter: { xtype: 'textfield' } },
                                                { dataIndex: 'ShortName', text: 'Краткое наименование', flex: 1, filter: { xtype: 'textfield' } }
                                            ],
                                            listeners: {
                                                'beforeload': function (store, operation) {
                                                    operation.params['type'] = 20;
                                                }
                                            },
                                            editable: false
                                        },
                                        {
                                            xtype: 'datefield',
                                            format: 'd.m.Y',
                                            name: 'CbDateSsp',
                                            fieldLabel: 'Дата направления ССП',
                                            maxLength: 100,
                                            labelAlign: 'right'
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
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Документ',
                                            enumName: 'B4.enums.LawsuitCollectionDebtDocumentType',
                                            name: 'CbDocumentType'
                                        },
                                        {
                                            xtype: 'numberfield',
                                            name: 'CbSumRepayment',
                                            fieldLabel: 'Сумма подлежащая взысканию (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true
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
                                        xtype: 'datefield',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'textfield',
                                            name: 'CbNumberDocument',
                                            fieldLabel: 'Номер документа',
                                            maxLength: 100,
                                            labelAlign: 'right'
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'CbDateDocument',
                                            format: 'd.m.Y',
                                            fieldLabel: 'Дата документа'
                                        }
                                    ]
                                },
                                {
                                    xtype: 'b4filefield',
                                    name: 'CbFile',
                                    fieldLabel: 'Файл',
                                    labelWidth: 170,
                                    labelAlign: 'right'
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'numberfield',
                                            name: 'CbSumStep',
                                            fieldLabel: 'Сумма взысканная в рамках производства (руб.)',
                                            hideTrigger: true,
                                            allowDecimals: true,
                                            flex:2
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'CbIsStopped',
                                            padding: '8 0 0 0',
                                            fieldLabel: 'Производство прекращено',
                                            flex:1                                            
                                        },
                                        {
                                            xtype: 'checkbox',
                                            name: 'CbDocReturned',
                                            padding: '8 0 0 0',
                                            fieldLabel: 'Исп. документ возвращен',
                                            flex:1,
                                            readOnly: true
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
                                        xtype: 'datefield',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'datefield',
                                            name: 'CbDateStopped',
                                            format: 'd.m.Y',
                                            fieldLabel: 'Дата прекращения',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'b4enumcombo',
                                            fieldLabel: 'Причина',
                                            enumName: 'B4.enums.LawsuitCollectionDebtReasonStoppedType',
                                            name: 'CbReasonStoppedType',
                                            readOnly: true
                                        }
                                    ]
                                },
                                {
                                    xtype: 'textfield',
                                    name: 'CbReasonStoppedDescription',
                                    fieldLabel: 'Примечание',
                                    maxLength: 2000,
                                    labelAlign: 'right',
                                    labelWidth: 170,
                                    readOnly: true
                                },
                                {
                                    xtype: 'container',
                                    padding: '0 0 5 0',
                                    layout: {
                                        type: 'hbox',
                                        align: 'stretch'
                                    },
                                    defaults: {
                                        xtype: 'datefield',
                                        labelWidth: 170,
                                        labelAlign: 'right',
                                        flex: 1
                                    },
                                    items: [
                                        {
                                            xtype: 'checkbox',
                                            name: 'LackOfPropertyAct',
                                            padding: '8 0 0 0',
                                            fieldLabel: 'Получен акт об отсутствии имущества',
                                            readOnly: true
                                        },
                                        {
                                            xtype: 'datefield',
                                            name: 'LackOfPropertyActDate',
                                            format: 'd.m.Y',
                                            fieldLabel: 'Дата акта об отсутствии имущества',
                                            readOnly: true
                                        }
                                    ]
                                }
                            ]
                        }

                    ]
                },
                {
                    xtype: 'tabpanel',
                    layout: {
                        align: 'stretch'
                    },
                    border: true,
                    items: [
                        { xtype: 'lawsuitsspdocgrid', flex: 1 },

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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            items: [
                                { xtype: 'b4closebutton' }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});