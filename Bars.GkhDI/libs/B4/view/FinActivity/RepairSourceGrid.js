Ext.define('B4.view.finactivity.RepairSourceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.finrepairsourcegrid',
    store: 'finactivity.RepairSource',
    itemId: 'finActivityRepairSourceGrid',
    title: 'Объем привлеченных средств на ремонт и благоустройство',

    requires: [
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeSourceFundsDi'
    ],

    initComponent: function() {
        var me = this;
        
        var renderer = function (val, meta, rec, index) {
            if (rec.get('IsInvalid').split(';')[index] == true) {
                meta.style = 'background: red;';
                meta.tdAttr = 'data-qtip="Введенное значение не совпадает с суммой по столбцу"';
            }
            return val;
        };

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeSourceFundsDi',
                    flex: 1,
                    text: 'Источник',
                    renderer: function (val) { return B4.enums.TypeSourceFundsDi.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма (тыс. руб.)',
                    editor: 'gkhdecimalfield',
                    renderer: function (val, meta, rec) {
                        return renderer(val, meta, rec, 0);
                    }
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing'
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});