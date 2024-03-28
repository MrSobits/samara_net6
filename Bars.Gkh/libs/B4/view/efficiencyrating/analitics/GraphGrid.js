Ext.define('B4.view.efficiencyrating.analitics.GraphGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',

        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    alias: 'widget.efanaliticsgraphgrid',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.efficiencyrating.EfficiencyRatingAnaliticsGraph');

        // отключаем paging
        Ext.apply(store.getProxy(),
        {
            pageParam: undefined,
            startParam: undefined,
            limitParam: undefined
        });

        Ext.applyIf(me, {
            store: store,
            columns: [
                {
                    xtype: 'actioncolumn',
                    width: 20,
                    tooltip: 'Переход к графику',
                    icon: B4.Url.content('content/img/icons/arrow_right.png'),
                    handler: function(gridView, rowIndex, colIndex, el, e, rec) {
                        var scope = this.origScope;

                        // Если scope не задан в конфиге, то берем грид которому принадлежит наша колонка
                        if (!scope) {
                            scope = this.up('grid');
                        }

                        scope.fireRowAction(rec, 'showgraph');
                    },
                    sortable: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
                }
            ]
        });

        me.callParent(arguments);
    }
});