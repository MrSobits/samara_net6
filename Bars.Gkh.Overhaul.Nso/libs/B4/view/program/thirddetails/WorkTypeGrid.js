Ext.define('B4.view.program.thirddetails.WorkTypeGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'Ext.grid.feature.Grouping',
        'B4.store.program.thirddetails.WorkType',
        'B4.enums.TypeWork'
    ],

    alias: 'widget.thirddetailsworktypegrid',
    
    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.program.thirddetails.WorkType');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { text: 'Тип работы', dataIndex: 'WorkType', renderer: function (val) { return B4.enums.TypeWork.displayRenderer(val); } },
                { flex: 1, text: 'Вид работы', dataIndex: 'WorkKind' },
                { text: 'Объем', dataIndex: 'Volume' },
                {
                    text: 'Сумма (руб.)', dataIndex: 'Sum',
                    renderer: function (value) {
                        return Ext.util.Format.currency(value, null, 2);
                    }
                }
            ],
            viewConfig: {
                loadMask: true
            },
            features: [
                {
                    ftype: 'grouping',
                    groupHeaderTpl: '{name}'
                }]
        });

        me.callParent(arguments);
    }
});