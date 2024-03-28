Ext.define('B4.view.nonresidentialplacement.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.nonresidplacegrid',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        
        'B4.enums.TypeContragentDi'
    ],
    store: 'NonResidentialPlacement',
    itemId: 'nonResidentialPlacementGrid',

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContragentDi',
                    flex: 1,
                    text: 'Тип контрагента',
                    renderer: function (val) { return B4.enums.TypeContragentDi.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ContragentName',
                    flex: 1,
                    text: 'Наименование контрагента'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Area',
                    text: 'Площадь помещения',
                    flex: 1,
                    renderer: function (val) {
                        if (!Ext.isEmpty(val) && val.toString().indexOf('.') != -1) {
                            return val.toString().replace('.', ',');
                        }
                        return val;
                    }
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