Ext.define('B4.view.constructionobject.participant.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.enums.ConstructionObjectParticipantType',
        'B4.enums.ConstructionObjectCustomerType'
    ],

    title: 'Участники строительства',
    alias: 'widget.constructionobjectparticipantgrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.constructionobject.Participant');

        me.relayEvents(store, ['beforeload'], 'store.');

        Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'ParticipantType',
                    text: 'Участник',
                    enumName: 'B4.enums.ConstructionObjectParticipantType',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'CustomerType',
                    text: 'Тип заказчика',
                    enumName: 'B4.enums.ConstructionObjectCustomerType',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Contragent',
                    text: 'Наименование',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentInn',
                    text: 'ИНН',
                    flex: 0.7
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentContactName',
                    text: 'ФИО',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentContactPhone',
                    text: 'Телефон',
                    flex: 0.7
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
                            columns: 2,
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