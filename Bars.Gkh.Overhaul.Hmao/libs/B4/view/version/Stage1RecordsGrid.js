Ext.define('B4.view.version.Stage1RecordsGrid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.stage1recordsgrid',
    title: 'Конструктивные элементы',
    
    minHeight: 100,

    requires: [
        'B4.base.Store',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.selection.CheckboxModel'
    ],

    selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {
        mode: 'SINGLE'
    }),
  
    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                fields: [
                    'Id',
                    'StructuralElement',
                    'PlanYear'],
                    proxy:{
                        type: 'b4proxy',
                        controllerName: 'VersionRecordStage1'
                }
            });

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'StructuralElement',
                    text: 'Наименование',
                    flex: 3
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanYear',
                    flex: 1,
                    text: 'Плановый год',
                    editor: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        allowDecimals: false
                    }

                }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters'),
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