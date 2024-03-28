Ext.define('B4.view.multipleAnalysis.Grid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.multipleAnalysisGrid',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.button.Add',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.multipleAnalysis.Template',
        'B4.Enums.TypeCondition'
    ],

    title: 'Множественный анализ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.multipleAnalysis.Template');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                { xtype: 'b4editcolumn' },
                { xtype: 'gridcolumn', dataIndex: 'RealEstateTypeName', flex: 1, text: 'Тип дома' },
                {
                    xtype: 'gridcolumn', dataIndex: 'TypeCondition', flex: 1, text: 'Условие',
                    renderer: function(value) {
                        return B4.enums.TypeCondition.getStore().findRecord('Value', value).get('Display');
                    }
                },
                { xtype: 'gridcolumn', dataIndex: 'FormDay', flex: 1, text: 'День' },
                //{ xtype: 'gridcolumn', dataIndex: 'Email', flex: 1, text: 'Электронный адрес' },
                { xtype: 'datecolumn', dataIndex: 'LastFormDate', flex: 1, text: 'Дата последнего формирования', format: 'd.m.Y' },
                { xtype: 'b4deletecolumn' }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        { xtype: 'b4updatebutton' },
                        { xtype: 'b4addbutton' }
                    ]
                }]
            },
            {
                xtype: 'b4pagingtoolbar',
                displayInfo: true,
                store: store,
                dock: 'bottom'
            }]
        });

        me.callParent(arguments);
    }
});