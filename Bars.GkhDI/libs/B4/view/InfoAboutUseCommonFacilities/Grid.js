Ext.define('B4.view.infoaboutusecommonfacilities.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.infusecommonfacgrid',
    requires: [
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit'
    ],
    store: 'InfoAboutUseCommonFacilities',
    itemId: 'infoAboutUseCommonFacilitiesGrid',

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
                    dataIndex: 'KindCommomFacilities',
                    flex: 1,
                    text: 'Вид общего имущества'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Lessee',
                    flex: 1,
                    text: 'Наименование арендатора'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateStart',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateEnd',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 100
                },
                 {
                     xtype: 'gridcolumn',
                     dataIndex: 'CostContract',
                     text: 'Сумма договора',
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
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});