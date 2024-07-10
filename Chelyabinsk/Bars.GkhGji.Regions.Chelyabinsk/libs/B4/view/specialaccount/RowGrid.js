Ext.define('B4.view.specialaccount.RowGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.button.Save',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.specialaccount.SpecialAccountRow'
    ],

    title: 'Информация по спец счетам',
    alias: 'widget.specialaccountrowgrid',
    store: 'specialaccount.SpecialAccountRow',
    closable:false,

    initComponent: function () {
        var me = this;     

        Ext.applyIf(me,
        {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1.3,
                    editable: false,
                    height: 230,
                    text: 'Полное <br />наименование <br />владельца <br />специального <br />счета '
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentInn',
                    flex: 0.7,
                    height: 230,
                    text: 'ИНН'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1.2,
                    height: 230,
                    text: 'Адрес МКД'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuracyArea',
                    flex: 0.5,
                    allowBlank: false,
                    height: 230,
                    text: 'Общая <br />площадь <br />МКД (кв.м.)',
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'SpecialAccountNum',
                    flex: 1,
                    allowBlank: false,
                    height: 230,
                    text: 'Номер <br />специального <br />счета',
                    editor: {
                        xtype: 'textfield'
                    },                    
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'StartDate',
                    flex: 0.7,
                    allowBlank: false,
                    height: 230,
                    text: 'Дата начала <br />формирования <br />ФКР',
                    editor: {
                        xtype: 'datefield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tariff',
                    flex: 0.7,
                    text: 'Размер взноса <br /> на КР',
                    height: 230,
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AccuredTotal',
                    flex: 0.7,
                    height: 230,
                    allowBlank: false,
                    text: 'Начислено <br />взносов <br />на КР по <br />состоянию на <br />начало отчетного <br />периода',
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Accured',
                    flex: 0.7,
                    text: 'Начислено <br />взносов <br />на КР за <br />отчетный <br />период',
                    height: 230,
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IncomingTotal',
                    flex: 0.7,
                    height: 230,
                    text: 'Оплачено <br />взносов <br />на КР по <br />состоянию на <br />начало отчетного <br />периода',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Incoming',
                    flex: 0.7,
                    allowBlank: false,
                    height: 230,
                    text: 'Оплачено <br />взносов <br />на КР за <br />отчетный <br />период',
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'AmountDebtForPeriod',
                //    flex: 0.7,
                //    height: 230,
                //    text: 'Задолженность <br />по оплате за <br />отчетный <br />период',
                //    allowBlank: false,
                //    editor: {
                //        xtype: 'numberfield'
                //    },
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'AmmountDebt',
                //    flex: 0.7,
                //    height: 230,
                //    text: 'Задолженность <br />по оплате по <br />состоянию на <br />начало отчетного <br />периода',
                //    allowBlank: false,
                //    editor: {
                //        xtype: 'numberfield'
                //    },
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TransferTotal',
                    flex: 0.7,
                    height: 230,
                    text: 'Израсходовано <br />по состоянию на <br />начало отчетного <br />периода',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Transfer',
                    flex: 0.7,
                    height: 230,
                    text: 'Израсходовано <br />за отчетный <br />период',
                    allowBlank: false,
                    editor: {
                        xtype: 'numberfield'
                    },
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'Ballance',
                    flex: 0.7,
                    height: 230,
                    format: '0.00',
                    text: 'Остаток <br />средств на <br />конец отчетного <br />периода'
                },
                {
                    xtype: 'numbercolumn',
                    dataIndex: 'TotalDebt',
                    flex: 0.7,
                    height: 230,
                    format: '0.00',
                    text: 'Задолженность <br />по состоянию на <br />конец отчетного <br />периода'
                },
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'Contracts',
                //    flex: 0.7,
                //    hheight: 230,
                //    text: 'Сведения <br />о заключении <br />договоров <br />займа <br />на проведение <br />КР',
                //    editor: {
                //        xtype: 'textfield'
                //    },
                //},
                //{
                //    xtype: 'gridcolumn',
                //    dataIndex: 'AmountDebtCredit',
                //    flex: 0.7,
                //    hheight: 230,
                //    text: 'Сумма <br />задолженности <br />по договорам <br />займа',
                //    editor: {
                //        xtype: 'textfield'
                //    },
                //},
                
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Perscent',
                    flex: 0.7,
                    height: 230,
                    text: '%',
                    hidden: true,
                },

                
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        afteredit: function (record) {
                            
                            var balanceValue = record.context.record.get('Incoming') + record.context.record.get('IncomingTotal') - record.context.record.get('Transfer') - record.context.record.get('TransferTotal'),
                                totalDebtValue = record.context.record.get('Accured') + record.context.record.get('AccuredTotal') - record.context.record.get('Incoming') - record.context.record.get('IncomingTotal');
                            record.context.record.set('Ballance', balanceValue);
                            record.context.record.set('TotalDebt', totalDebtValue);
                        }
                    }
                })
            ],
            viewConfig: {
                loadMask: true
            },
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
                        //{
                        //    xtype: 'buttongroup',
                        //    columns: 3,
                        //    items: [                             
                        //        {
                        //            xtype: 'b4addbutton'
                        //        }                              
                        //    ]
                        //},
                        // {
                        //    xtype: 'buttongroup',
                        //    columns: 3,
                        //    items: [                             
                        //        {
                        //            xtype: 'b4updatebutton'
                        //        }                              
                        //    ]
                        //}
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    },

    onStoreAfterLoad: function (store, records) {
        
        for (var i = 0; i < records.length; i++) {
            var balanceValue = records[i].data.Incoming + records[i].data.IncomingTotal - records[i].data.Transfer - records[i].data.TransferTotal,
                totalDebtValue = records[i].data.Accured + records[i].data.AccuredTotal - records[i].data.Incoming - records[i].data.IncomingTotal;
            records[i].set('Ballance', balanceValue);
            records[i].set('TotalDebt', totalDebtValue);
        }
    }
});

