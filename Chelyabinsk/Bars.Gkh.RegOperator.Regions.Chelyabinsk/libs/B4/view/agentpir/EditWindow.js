Ext.define('B4.view.agentpir.EditWindow', {
    extend: 'B4.form.Window',
    itemId: 'agentPIREditWindow',
    requires: [
        'B4.form.SelectField',
        'B4.ux.button.Close',
        'B4.ux.button.Save',
        'B4.store.regop.ChargePeriod',
        'B4.enums.YesNoNotSet',
        'B4.view.Control.GkhButtonPrint',
        'B4.form.FileField',
        'B4.store.Contragent'
    ],
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'form',
    width: 1000,
    height: 700,
    bodyPadding: 10,
    title: 'Форма редактирования Агент ПИР',
    closeAction: 'hide',
    trackResetOnLoad: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            defaults: {
                labelWidth: 100,
                labelAlign: 'right',
            },
            items: [
                {
                    xtype: 'b4selectfield',
                    fieldLabel: 'Контрагент',
                    name: 'Contragent',
                    itemId: 'ContragentEW',
                    store: 'B4.store.Contragent',
                    columns: [
                        { text: 'Наименование', dataIndex: 'ShortName', flex: 2, filter: { xtype: 'textfield' } },
                        { text: 'ИНН', dataIndex: 'Inn', flex: 1, filter: { xtype: 'textfield' } }
                    ],
                    editable: false
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        //margin: '5 0 5 0',
                        labelWidth: 100,
                        labelAlign: 'right'
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DateStart',
                            fieldLabel: 'Дата начала',
                            allowBlank: false
                        },
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'DateEnd',
                            fieldLabel: 'Дата окончания',
                            allowBlank: false
                        }
                    ]
                },
                {
                    xtype: 'container',
                    layout: 'hbox',
                    defaults: {
                        xtype: 'combobox',
                        //margin: '5 0 5 0',
                        labelWidth: 100,
                        labelAlign: 'right',
                    },
                    items: [
                        {
                            xtype: 'datefield',
                            format: 'd.m.Y',
                            name: 'ContractDate',
                            fieldLabel: 'Дата договора',
                            allowBlank: false
                        },
                        {
                            xtype: 'textfield',
                            name: 'ContractNumber',
                            fieldLabel: 'Номер договора',
                            allowBlank: false
                        }
                    ]
                },                
                {
                    xtype: 'b4filefield',
                    name: 'FileInfo',
                    fieldLabel: 'Договор'
                },
                {
                    xtype: 'tabpanel',
                    border: false,
                    flex: 1,
                    defaults: {
                        height: 480,
                        border: false
                    },
                    items: [
                        {
                            xtype: 'agentpirdebtorGrid',
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
                        },
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'gkhbuttonprint'
                                },
                                {
                                    xtype: 'b4selectfield',
                                    store: 'B4.store.regop.ChargePeriod',
                                    textProperty: 'Name',
                                    editable: false,
                                    allowBlank: true,
                                    //windowContainerSelector: '#' + me.getId(),
                                    //windowCfg: {
                                    //    modal: true
                                    //},
                                    //trigger2Cls: '',
                                    columns: [
                                        {
                                            text: 'Наименование',
                                            dataIndex: 'Name',
                                            flex: 1,
                                            filter: { xtype: 'textfield' }
                                        },
                                        {
                                            text: 'Дата открытия',
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'StartDate',
                                            flex: 1,
                                            filter: { xtype: 'datefield' }
                                        },
                                        {
                                            text: 'Дата закрытия',
                                            xtype: 'datecolumn',
                                            format: 'd.m.Y',
                                            dataIndex: 'EndDate',
                                            flex: 1,
                                            filter: { xtype: 'datefield' }
                                        },
                                        {
                                            text: 'Состояние',
                                            dataIndex: 'IsClosed',
                                            flex: 1,
                                            renderer: function (value) {
                                                return value ? 'Закрыт' : 'Открыт';
                                            }
                                        }
                                    ],
                                    name: 'ChargePeriod',
                                    labelAlign: 'right',
                                    fieldLabel: 'Период'
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