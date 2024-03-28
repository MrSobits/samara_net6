Ext.define('B4.view.objectcr.ProtocolEditWindow', {
    extend: 'B4.form.Window',

    mixins: [ 'B4.mixins.window.ModalMask' ],
    layout: { type: 'vbox', align: 'stretch' },
    minHeight: 450,
    width: 700,
    minWidth: 600,
    maxWidth: 800,
    bodyPadding: 5,
    
    alias: 'widget.objectcrprotocolwin',
    title: 'Протоколы, акты',
    closeAction: 'hide',
    trackResetOnLoad: true,

    requires: [
        'B4.view.Control.GkhDecimalField',
        'B4.form.FileField',
        'B4.form.SelectField',
        'B4.form.ComboBox',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.Contragent',
        'B4.view.Control.GkhTriggerField',
        'B4.view.objectcr.ProtocolTypeWorkCrGrid',
        'B4.enums.YesNo'
    ],

    initComponent: function() {
        var me = this;

        Ext.applyIf(me, {
            items: [
            {
                xtype: 'tabpanel',
                layout: 'vbox',
                flex: 1,
                border: false,
                margins: -1,
                items: [
                    {
                        layout: 'anchor',
                        title: 'Общие сведения',
                        border: false,
                        bodyPadding: 5,
                        margins: -1,
                        frame: true,
                        defaults: {
                            labelWidth: 150
                        },
                        items: [
                            {
                                xtype: 'fieldset',
                                flex: 1,
                                layout: { type: 'vbox', align: 'stretch' },
                                defaults: {
                                    labelWidth: 120,
                                    labelAlign: 'right'
                                },
                                title: 'Общие сведения',
                                items: [
                                    {
                                        xtype: 'b4combobox',
                                        fieldLabel: 'Тип документа',
                                        store: Ext.create('B4.base.Store', {
                                            autoLoad: false,
                                            fields: ['Id', 'Key', 'Name'],
                                            proxy: {
                                                type: 'b4proxy',
                                                controllerName: 'ProtocolCr',
                                                listAction: 'GetTypeDocumentCr'
                                            }
                                        }),
                                        editable: false,
                                        valueField: 'Id',
                                        displayField: 'Name',
                                        itemId: 'cbTypeDocumentCr',
                                        name: 'TypeDocumentCr',
                                        queryMode: 'local',
                                        allowBlank: false
                                    },
                                    {
                                        xtype: 'checkbox',
                                        name: 'DecisionOms',
                                        fieldLabel: 'Решение ОМС'
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'DocumentName',
                                        fieldLabel: 'Документ',
                                        allowBlank: false,
                                        maxLength: 300
                                    },
                                    {
                                        xtype: 'container',
                                        layout: {
                                            type: 'hbox'
                                        },
                                        padding: '0 0 5 0',
                                        defaults: {
                                            labelWidth: 120,
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                xtype: 'textfield',
                                                name: 'DocumentNum',
                                                fieldLabel: 'Номер',
                                                maxLength: 300
                                            },
                                            {
                                                xtype: 'datefield',
                                                name: 'DateFrom',
                                                itemId: 'dfDateFrom',
                                                fieldLabel: 'от',
                                                format: 'd.m.Y'
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'b4filefield',
                                        name: 'File',
                                        fieldLabel: 'Файл',
                                        editable: false
                                    },
                                    {
                                        xtype: 'gkhdecimalfield',
                                        name: 'SumActVerificationOfCosts',
                                        itemId: 'nfSumActVerificationOfCosts',
                                        fieldLabel: 'Сумма Акта сверки данных о расходах',
                                        allowBlank: false
                                    },
                                    {
                                        xtype: 'b4selectfield',
                                        editable: false,
                                        name: 'Contragent',
                                        fieldLabel: 'Участник процесса',
                                        store: 'B4.store.Contragent',
                                        itemId: 'sfContragent',
                                        columns: [
                                            { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ShortName', flex: 1, filter: { xtype: 'textfield' } }
                                        ]
                                    },
                                    {
                                        xtype: 'textfield',
                                        name: 'OwnerName',
                                        fieldLabel: 'Собственник, участвующий в приемке работ',
                                        maxLength: 300
                                    },
                                    {
                                        xtype: 'textarea',
                                        name: 'Description',
                                        fieldLabel: 'Описание',
                                        maxLength: 2000,
                                        flex: 1
                                    },
                                    {
                                        xtype: 'combobox',
                                        editable: false,
                                        fieldLabel: 'Выводить документ на портал',
                                        name: 'UsedInExport',
                                        store: B4.enums.YesNo.getStore(),
                                        displayField: 'Display',
                                        valueField: 'Value'
                                    }
                                ]
                            },
                            {
                                xtype: 'fieldset',
                                title: 'Количественные характеристики',
                                fieldSetType: 'CountProperties',
                                items: [
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'gkhdecimalfield',
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                name: 'CountVote',
                                                itemId: 'nfCountVote',
                                                fieldLabel: 'Количество голосов (кв. м.)',
                                                labelWidth: 180
                                            },
                                            {
                                                name: 'CountVoteGeneral',
                                                itemId: 'nfCountVoteGeneral',
                                                fieldLabel: 'Общее количество голосов (кв. м.)',
                                                labelWidth: 215
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        padding: '5 0 5 0',
                                        layout: 'hbox',
                                        anchor: '50%',
                                        items: [
                                            {
                                                xtype: 'gkhdecimalfield',
                                                name: 'CountAccept',
                                                itemId: 'nfCountAccept',
                                                fieldLabel: 'Доля принявших участие (%)',
                                                labelWidth: 180,
                                                labelAlign: 'right',
                                                flex: 0.5
                                            }
                                        ]
                                    },
                                    {
                                        xtype: 'container',
                                        layout: 'hbox',
                                        defaults: {
                                            xtype: 'gkhdecimalfield',
                                            labelAlign: 'right',
                                            flex: 1
                                        },
                                        items: [
                                            {
                                                name: 'GradeClient',
                                                itemId: 'nfGradeClient',
                                                fieldLabel: 'Оценка заказчика',
                                                labelWidth: 180
                                            },
                                            {
                                                name: 'GradeOccupant',
                                                itemId: 'nfGradeOccupant',
                                                fieldLabel: 'Оценка жителей',
                                                labelWidth: 215
                                            }
                                        ]
                                    }
                                ]
                            }
                        ]
                    },
                    {
                        title: 'Виды работ',
                        layout: 'fit',
                        flex: 1,
                        border: false,
                        itemId: 'tpTypeWork',
                        defaults: {
                            labelWidth: 150
                        },
                        items: [
                        {
                            xtype: 'protocoltypeworkcrgrid'
                        }]
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
                                { xtype: 'b4savebutton' }
                            ]
                        },
                        { xtype: 'tbfill' },
                        {
                            xtype: 'buttongroup',
                            columns: 2,
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