Ext.define('B4.view.motivationconclusion.RelationsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.motivationconclusionrelationsgrid',
    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.button.Update',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Enum',

        'B4.store.motivationconclusion.Relations',

        'B4.enums.TypeDocumentGji'
    ],

    title: 'Предыдущие или последующие документы',
    store: 'motivationconclusion.Relations',

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
                    xtype: 'b4enumcolumn',
                    dataIndex: 'TypeDocumentGji',
                    text: 'Тип документа ',
                    enumName: 'B4.enums.TypeDocumentGji',
                    flex: 1
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNumber',
                    flex: 1,
                    text: 'Номер документа'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DocumentDate',
                    text: 'Дата',
                    format: 'd.m.Y',
                    width: 100
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
                            columns: 2,
                            items: [
                                { xtype: 'b4updatebutton', itemId: 'btnRelationsParentDocs', enableToggle: true, toggleGroup: 'clientsToggleGroup', text: 'Предыдущие документы', pressed: false },
                                { xtype: 'b4updatebutton', itemId: 'btnRelationsChildrenDocs', enableToggle: true, toggleGroup: 'clientsToggleGroup', text: 'Последующие документы', pressed: true }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});