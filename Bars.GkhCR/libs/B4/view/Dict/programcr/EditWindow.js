Ext.define('B4.view.dict.programcr.EditWindow', {
    extend: 'B4.form.Window',
    alias: 'widget.programCrEditWindow',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: {
        type: 'vbox',
        align: 'stretch',
        pack: 'start'
    },
    width: 650,
    height: 600,
    maxWidth: 700,
    minWidth: 500,
    minHeight: 550,
    autoScroll: true,
    bodyPadding: 5,
    itemId: 'programCrEditWindow',
    title: 'Программа капитального ремонта',
    closeAction: 'hide',
    trackResetOnLoad: true,
    requires: [
        'B4.view.dict.programcr.FinSourceGrid',
        'B4.view.dict.programcr.ChangeJournalGrid',
        'B4.form.SelectField',
        'B4.store.dict.Period',
        'B4.store.dict.NormativeDoc',
        'B4.form.FileField',

        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.ux.config.ContragentSelectField',

        'B4.enums.TypeVisibilityProgramCr',
        'B4.enums.TypeProgramCr',
        'B4.enums.TypeProgramStateCr',
        'B4.enums.AddWorkFromLongProgram'
    ],

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    defaults: {
                        labelAlign: 'right',
                        labelWidth: 120
                    },
                    items: [
                        {
                            xtype: 'textfield',
                            name: 'Name',
                            fieldLabel: 'Наименование',
                            allowBlank: false,
                            maxLength: 300
                        },
                        {
                            xtype: 'textfield',
                            name: 'Code',
                            fieldLabel: 'Код',
                            maxLength: 200
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'Period',
                            fieldLabel: 'Период',
                            store: 'B4.store.dict.Period',
                            editable: false,
                            allowBlank: false
                        },
                        {
                            xtype: 'checkbox',
                            boxLabel: 'Сформировать на основании Региональной программы КР (только при создании)',
                            actionType: 'CreateDpkr',
                            itemId: 'cbCreateByDpkr',
                            margin: '0 0 5 125'
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Видимость',
                            store: B4.enums.TypeVisibilityProgramCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeVisibilityProgramCr'
                        },
                        {
                            xtype: 'contragentselectfield',
                            name: 'GovCustomer',
                            fieldLabel: 'Государственный заказчик',
                        },
                        {
                            xtype: 'combobox', editable: false,
                            fieldLabel: 'Состояние',
                            store: B4.enums.TypeProgramStateCr.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'TypeProgramStateCr'
                        },
                        {
                            xtype: 'container',
                            layout: 'hbox',
                            padding: '0 0 5 0',
                            defaults: {
                                labelWidth: 120,
                                labelAlign: 'right',
                                flex: 1
                            },
                            items: [
                                {
                                    xtype: 'textfield',
                                    name: 'DocumentNumber',
                                    fieldLabel: 'Номер документа',
                                    allowBlank: false
                                },
                                {
                                    xtype: 'datefield',
                                    name: 'DocumentDate',
                                    fieldLabel: 'Дата документа',
                                    format: 'd.m.Y'
                                }
                            ]
                        },
                        {
                            xtype: 'textfield',
                            name: 'DocumentDepartment',
                            fieldLabel: 'Орган, принявший документ',
                            allowBlank: false
                        },
                        {
                            xtype: 'combobox',
                            fieldLabel: 'Добавление видов работ из ДПКР',
                            store: B4.enums.AddWorkFromLongProgram.getStore(),
                            displayField: 'Display',
                            valueField: 'Value',
                            name: 'AddWorkFromLongProgram',
                            listeners: {
                                beforerender: function (c) {
                                    c.autoEl = Ext.apply({ 'data-qtip': 'Возможность добавления видов работ из ДПКР для Краткосрочной программы, созданной не на основе Региональной программы' }, c.autoEl);
                                }
                            }
                        },
                        {
                            xtype: 'b4selectfield',
                            name: 'NormativeDoc',
                            fieldLabel: 'Постановление об утверждении КП',
                            store: 'B4.store.dict.NormativeDoc',
                            editable: false
                        },
                        {
                            xtype: 'b4filefield',
                            name: 'File',
                            fieldLabel: 'Файл'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'UsedInExport',
                            boxLabel: 'Используется при экспорте',
                            margin: '0 0 2 125'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ImportContract',
                            boxLabel: 'Используется при экспорте договоров/актов КПР',
                            margins: '0 0 2 95'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'NotAddHome',
                            boxLabel: 'Не доступно добавление домов',
                            margin: '0 0 2 125'
                        },
                        
                        {
                            xtype: 'checkbox',
                            name: 'MatchFl',
                            boxLabel: 'Соответствует ФЗ',
                            margin: '0 0 2 125'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'ForSpecialAccount',
                            boxLabel: 'Для специальных счетов',
                            margin: '0 0 2 125'
                        },
                        {
                            xtype: 'checkbox',
                            name: 'UseForReformaAndGisGkhReports',
                            boxLabel: 'Использовать для отчетов Реформы и ГИС ЖКХ',
                            margin: '0 0 2 125'
                        },
                        {
                            xtype: 'textarea',
                            name: 'Description',
                            fieldLabel: 'Примечание',
                            margin: '8 0',
                            height: 60,
                            maxLength: 2000
                        }
                    ]
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    margins: -1,
                    minHeight: 200,
                    flex: 1,
                    layout: {
                        type: 'vbox',
                        align: 'stretch'
                    },
                    items: [
                        {
                            xtype: 'programcrfinsourcegrid',
                            flex: 1
                        },
                        {
                            xtype: 'panel',
                            title: 'Журнал изменений',
                            panelName: 'ChangeJournal',
                            margins: -1,
                            layout: {
                                type: 'vbox',
                                align: 'stretch'
                            },
                            bodyStyle: Gkh.bodyStyle,
                            items: [
                                {
                                    xtype: 'programcrchangejournalgrid',
                                    flex: 1
                                }
                            ]
                        }]
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