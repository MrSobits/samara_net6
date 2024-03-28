Ext.define('B4.view.realityobj.IntercomGrid', {
    extend: 'B4.ux.grid.Panel',
    mixins: ['B4.mixins.window.ModalMask'],
    layout: 'fit',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.YesNoNotSet',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    alias: 'widget.intercomgrid',
    title: 'Сведения о домофонах',
    closable: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.realityobj.Intercom');

        me.relayEvents(store, ['beforeload'], 'store.');
        me.relayEvents(store, ['load'], 'store.');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IntercomCount',
                    flex: 2,
                    text: 'Количество подъездных домофонов в доме'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Tariff',
                    flex: 1,
                    text: 'Минимальный тариф',
                    sortable: false
                },
                {
                    xtype: 'booleancolumn',
                    dataIndex: 'HasNotTariff',
                    text: 'Нет единого тарифа',
                    flex: 1,
                    trueText: 'Да',
                    falseText: 'Нет'
                },
                {
                    xtype: 'gridcolumn',
                    text: 'Аналоговый домофон',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'AnalogIntercomCount',
                            flex: 2,
                            text: 'Количество домофонов'
                        },
                        {
                            xtype: 'datecolumn',
                            dataIndex: 'InstallationDate',
                            flex: 2,
                            text: 'Планируемая дата установки IP-домофона',
                            format: 'F, Y'
                        },
                    ]
                },
                {
                    xtype: 'gridcolumn',
                    text: 'IP-домофон',
                    columns: [
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'IpIntercomCount',
                            flex: 2,
                            text: 'Количество домофонов'
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNoNotSet',
                            dataIndex: 'Recording',
                            flex: 1,
                            text: 'Наличие видеозаписи'
                        },
                        {
                            xtype: 'gridcolumn',
                            dataIndex: 'ArchiveDepth',
                            flex: 1,
                            text: 'Глубина архива видеозаписи'
                        },
                        {
                            xtype: 'b4enumcolumn',
                            enumName: 'B4.enums.YesNoNotSet',
                            dataIndex: 'ArchiveAccess',
                            flex: 2,
                            text: 'Постоянный удаленный доступ к архиву у МВД РТ'
                        }
                    ]
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'EntranceCount',
                    flex: 2,
                    text: 'Количество подъездов без домофона'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'IntercomInstallationDate',
                    flex: 2,
                    text: 'Планируемая дата установки IP-домофона',
                    format: 'F, Y'
                },
                {
                    xtype: 'b4deletecolumn'
                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
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
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
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