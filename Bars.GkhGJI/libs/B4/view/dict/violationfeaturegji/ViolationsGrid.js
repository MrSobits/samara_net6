Ext.define('B4.view.dict.violationfeaturegji.ViolationsGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.form.TreeSelectField',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.store.dict.ViolationFeatureGji'
    ],
    alias: 'widget.violationsgrid',
    title: 'Нарушения',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.dict.ViolationFeatureGji', {
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'ViolationFeatureGji',
                    listAction: 'ListViols'
                }
            });

       Ext.applyIf(me, {
            columnLines: true,
            store: store,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NormDocNum',
                    filter: { xtype: 'textfield' },
                    flex: 2,
                    text: 'Пункт НПД'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    filter: { xtype: 'textfield' },
                    flex: 3,
                    text: 'Текст нарушения'
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
                            columns: 3,
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
                    dock: 'bottom',
                    store: store
                }
            ]
        });

        me.callParent(arguments);
    }
});