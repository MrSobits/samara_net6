Ext.define('B4.view.claimwork.lawsuit.LawsuitOwnerInfoGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.lawsuitownerinfogrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.field.AreaShareField',
        'B4.store.claimwork.lawsuit.LawsuitOwnerInfo',
        'B4.view.Control.GkhButtonPrint'
    ],

    title: 'Сведения о собственниках',
    cls: 'x-large-head',
    closable: false,

    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.claimwork.lawsuit.LawsuitOwnerInfo');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('Ext.selection.CheckboxModel'),
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Name',
                    dataIndex: 'Name',
                    text: 'Cобственник',
                    flex: 2
                },
                {
                    xtype: 'gridcolumn',
                    name: 'Address',
                    dataIndex: 'Address',
                    text: 'Адрес',
                    flex: 3
                },
                {
                    xtype: 'gridcolumn',
                    name: 'LivePlace',
                    dataIndex: 'LivePlace',
                    text: 'Адрес проживания',
                    flex: 3
                },
                {
                    xtype: 'gridcolumn',
                    name: 'ClaimNumber',
                    dataIndex: 'ClaimNumber',
                    text: 'Номер заявления',
                    flex: 0.5
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PersonalAccount',
                    dataIndex: 'PersonalAccount',
                    text: 'Номер ЛС',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    name: 'AreaShare',
                    dataIndex: 'AreaShare',
                    text: 'Доля собственности',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    name: 'BirthDate',
                    dataIndex: 'BirthDate',
                    text: 'Дата рождения',
                    flex: 0.5
                },                
                {
                    xtype: 'gridcolumn',
                    name: 'DebtBaseTariffSum',
                    dataIndex: 'DebtBaseTariffSum',
                    text: 'Новая задолженность по базовому тарифу',
                    hideable: false,
                    flex: 1,
                    renderer: function(value) {
                        return value > 0 ? value : 0;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'DebtDecisionTariffSum',
                    dataIndex: 'DebtDecisionTariffSum',
                    text: 'Новая задолженность по тарифу решения',
                    hideable: false,
                    flex: 1,
                    renderer: function (value) {
                        return value > 0 ? value : 0;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'PenaltyDebt',
                    dataIndex: 'PenaltyDebt',
                    text: 'Новая задолженность по пени',
                    hideable: false,
                    flex: 1,
                    renderer: function (value) {
                        return value > 0 ? value : 0;
                    }
                },
                {
                    xtype: 'gridcolumn',
                    name: 'CalcPeriod',
                    dataIndex: 'CalcPeriod',
                    text: 'Периоды расчета задолженности',
                    hideable: false,
                    flex: 1.8
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' },
                                {
                                    xtype: 'button',
                                    action: 'DebtCalculate',
                                    text: 'Расчитать долг',
                                    icon: B4.Url.content('content/img/icons/calculator.png')
                                },
                                {
                                    xtype: 'button',
                                    action: 'GetRosRegOwners',
                                    text: 'Заполнить из выписки',
                                    icon: B4.Url.content('content/img/icons/page_copy.png')
                                },
                                {
                                    xtype: 'button',
                                    iconCls: 'icon-page',
                                    action: 'PrintExtract',
                                    //href: '/ExtractPrinter/PrintExtractForClaimWork/?id=',
                                    text: 'Выписка'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    action: 'PrintOwner',
                                    name:'PrintOwner',
                                    itemId: 'btnPrint'
                                },
                                {
                                    xtype: 'gkhbuttonprint',
                                    action: 'LawsuitPrintOwner',
                                    name: 'LawsuitPrintOwner',
                                    itemId: 'btnLawsuitPrintOwner'
                                },
                                {
                                    xtype: 'button',
                                    action: 'CreateArchive',
                                    icon: B4.Url.content('content/img/icons/book_go.png'),
                                    text: 'Выгрузить в реестр долевых ПИР'
                                }
                            ],
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
                Ext.create('Ext.grid.plugin.CellEditing',
                {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true,
                getRowClass: function (record, index) {
                    var underage = record.get('Underage');
                    if (underage) {
                        return 'back-coralyellow';
                    }
                    return;
                }
            }

        });

        me.callParent(arguments);
    }
});

