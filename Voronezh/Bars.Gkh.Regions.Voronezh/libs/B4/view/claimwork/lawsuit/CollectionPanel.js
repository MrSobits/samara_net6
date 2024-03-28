Ext.define('B4.view.claimwork.lawsuit.CollectionPanel', {
    extend: 'Ext.form.Panel',

    closable: false,
    layout: {
        type: 'vbox',
        align: 'stretch'
    },
    bodyPadding: 5,
    bodyStyle: Gkh.bodyStyle,
    autoScroll: true,
    hidden:true,
    alias: 'widget.clwlawsuitcollectionpanel',
    title: 'Взыскание долга',
    requires: [
        'B4.form.EnumCombo',
        'B4.ux.button.Save',
        'B4.form.FileField',
        'B4.enums.LawsuitCollectionDebtType',
        'B4.enums.LawsuitFactInitiationType',
        'B4.enums.LawsuitCollectionDebtReasonStoppedType',
        'B4.enums.LawsuitCollectionDebtDocumentType',
        'B4.store.dict.JurInstitution',
        'B4.view.claimwork.lawsuit.DocumentationGrid'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
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
                                        xtype: 'component'
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
                                        allowDecimals: true
                                    },
                                    {
                                        xtype: 'checkbox',
                                        name: 'CbIsStopped',
                                        padding: '8 0 0 0',
                                        fieldLabel: 'Производство прекращено'
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
                            }
                        ]
                    },
                    {
                        xtype: 'claimworklawsuitdocumentationgrid',
                        flex: 1
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
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4savebutton'
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