Ext.define('B4.view.manorg.CommunalServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorgcommunalservicegrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.column.Enum',
        'B4.enums.CommunalServiceName',
        'B4.enums.CommunalServiceResource',
        'B4.store.manorg.ManOrgBilCommunalService'
    ],

    title: 'Коммунальные услуги',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.manorg.ManOrgBilCommunalService');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    enumName: 'B4.enums.CommunalServiceName',
                    filter: true
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ServiceName',
                    flex: 1,
                    text: 'Главная коммунальная услуга',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'b4enumcolumn',
                    dataIndex: 'Resource',
                    flex: 1,
                    text: 'Коммунальный ресурс',
                    enumName: 'B4.enums.CommunalServiceResource',
                    filter: true
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        },
                        {
                            xtype: 'label',
                            text: 'В данном разделе должна быть представлена информация о газо-, водо-, электроснабжении и иных коммунальных услугах',
                            padding: '0 0 0 20'
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