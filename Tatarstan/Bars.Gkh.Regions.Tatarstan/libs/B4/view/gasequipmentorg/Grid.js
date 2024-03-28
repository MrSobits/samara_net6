Ext.define('B4.view.gasequipmentorg.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.gasequipmentorggrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.store.dict.Municipality',
        'B4.store.GasEquipmentOrg'
    ],

    title: 'Реестр ВДГО',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.GasEquipmentOrg');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    width: 160,
                    text: 'Муниципальный район',
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
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    text: 'Контрагент',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Inn',
                    width: 160,
                    text: 'ИНН',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Kpp',
                    width: 160,
                    text: 'КПП',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Ogrn',
                    width: 160,
                    text: 'ОГРН',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'JuridicalAddress',
                    width: 160,
                    text: 'Юридический адрес',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    name: 'ActivityDateStart',
                    dataIndex: 'DateRegistration',
                    text: 'Дата начала деятельности',
                    format: 'd.m.Y',
                    allowBlank: false,
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'datecolumn',
                    name: 'TerminationDate',
                    dataIndex: 'DateTermination',
                    text: 'Дата прекращения деятельности',
                    format: 'd.m.Y',
                    allowBlank: false,
                    flex: 1,
                    filter: { xtype: 'datefield' }
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ],

            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
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
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});