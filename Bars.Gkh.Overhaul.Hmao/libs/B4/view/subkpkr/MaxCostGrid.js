Ext.define('B4.view.subkpkr.MaxCostGrid',
    {
        extend: 'B4.ux.grid.Panel',
        requires: [
            'B4.ux.button.Update',
            'B4.ux.grid.toolbar.Paging',
            'B4.ux.grid.plugin.HeaderFilters',
            'B4.store.subkpkr.SubProgramKPKRCostByYear',
            'B4.store.subkpkr.SubProgramKPKRKE',
            'Ext.ux.RowExpander'
        ],
        alias: 'widget.subkpkrmaxcostgrid',
        title: 'Предельная стоимость по годам',
        initComponent: function () {
            var me = this,
                store = Ext.create('B4.store.subkpkr.SubProgramKPKRCostByYear');       
            store.on('beforeload', function(store, operation) {        
                var editfrm = me.up('versmakesubkpkrwin');
                var kegrid = editfrm.down('subkpkrkkegrid');

                operation = operation || {};
                operation.params = operation.params || {};
                operation.params.StartYear = editfrm.down('numberfield[name=StartYear]').getValue();
                operation.params.YearCount = editfrm.down('numberfield[name=YearCount]').getValue();
                operation.params.FirstYearPSD = editfrm.down('checkbox[name=FirstYearPSD]').getValue();
                operation.params.FirstYearWithoutWork = editfrm.versionId;
                operation.params.SelectedKE = Ext.Array.map(kegrid.getSelectionModel().getSelection(), function(el) { return el.get('Id'); });
            }, me);

            Ext.applyIf(me,
                {
                    store: store,
                    columnLines: true,
                    columns: [
                        {
                            xtype: 'b4editcolumn',
                            scope: me
                        },
                        {
                            header: 'Год',
                            dataIndex: 'Year',
                            flex: 0.5,
                            //filter:
                            //{
                            //    xtype: 'textfield'
                            //},
                        },
                        {
                            header: 'Сумма',
                            flex: 1,
                            dataIndex: 'Sum',
                            //filter:
                            //{
                            //    xtype: 'textfield'
                            //},
                        },
                        {
                            header: 'Максимальная сумма',
                            flex: 1,
                            dataIndex: 'GrantedSum',
                            //filter:
                            //{
                            //    xtype: 'textfield'
                            //},
                        },
                        ],
                    plugins:
                    [
                        Ext.create('B4.ux.grid.plugin.HeaderFilters')
                    ],
                    viewConfig:
                    {
                        loadMask: true
                    },
                    dockedItems: [
                        //{
                        //    xtype: 'toolbar',
                        //    dock: 'top',
                        //    name: 'buttons',
                        //    items: [
                        //        {
                        //            xtype: 'buttongroup',
                        //            columns: 2,
                        //            items: [
                        //                {
                        //                    xtype: 'b4updatebutton',
                        //                    handler: function () {
                        //                        var me = this;
                        //                        me.up(
                        //                            'grid'
                        //                        )
                        //                            .getStore()
                        //                            .load();
                        //                    }
                        //                },]
                        //        }]
                        //},
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