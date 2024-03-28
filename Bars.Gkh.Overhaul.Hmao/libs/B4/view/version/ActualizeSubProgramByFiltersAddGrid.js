Ext.define('B4.view.version.ActualizeSubProgramByFiltersAddGrid', {
    extend: 'B4.ux.grid.Panel',

    alias: 'widget.actualizesubprogrambyfilteraddgrid',

    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.version.ActualizeSubProgramByFiltersAdd',
    ],

    closable: false,
    title: 'Дома на добавление',
    store: 'version.ActualizeSubProgramByFiltersAdd',
    initComponent: function ()
     {
        var me = this;

        Ext.applyIf(me, {
            store: this.store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                {
                    xtype: 'actioncolumn',
                    action: 'remove',
                    align: 'center',
                    icon: B4.Url.content('content/images/btnRemove.png'),
                    tooltip: 'Исключить',
                    width: 18,
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Адрес',
                    dataIndex: 'Address',
                    text: 'Address',
                    filter: {
                        xtype: 'textfield',
                    },
                    width: 200,
                },
                {
                    xtype: 'gridcolumn',
                    header: 'Причина',
                    dataIndex: 'Reasons',
                    text: 'Reasons',
                    filter: {
                        xtype: 'textfield',
                    },
                    width: 200,
                },
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            //features: [
            //    Ext.create('Ext.grid.feature.Grouping', { groupHeaderTpl: 'Адрес: {name}' })
            //],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
                //{
                //    xtype: 'button',
                //    iconCls: 'icon-arrow-in',
                //    text: 'Свернуть все',
                //    handler: function () {
                //        me.features[0].collapseAll();
                //    }
                //},
                //{
                //    xtype: 'button',
                //    iconCls: 'icon-arrow-out',
                //    text: 'Развернуть все',
                //    handler: function () {
                //        me.features[0].expandAll();
                //    }
                //},
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: this.store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});