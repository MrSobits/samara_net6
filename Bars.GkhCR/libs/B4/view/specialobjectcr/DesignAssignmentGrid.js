Ext.define('B4.view.specialobjectcr.DesignAssignmentGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.specialobjectcrdesignassignmentgrid',

    requires: [
         'B4.ux.button.Add',
         'B4.ux.button.Update',
         'B4.ux.grid.column.Delete',
         'B4.ux.grid.column.Edit',
         'B4.ux.grid.plugin.HeaderFilters',
         'B4.ux.grid.toolbar.Paging',
         'B4.form.ComboBox',
         'B4.form.GridStateColumn'
    ],

    title: 'Задание на проектирование',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.specialobjectcr.DesignAssignment');

        Ext.applyIf(me, {
            store:store,
            columnLines: true,
            columns: [
                {
                    xtype: 'b4editcolumn',
                    scope: me
                },
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 300,
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeWorksCr',
                    flex: 1,
                    text: 'Виды работ'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Document',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    width: 100,
                    format: 'd.m.Y',
                    text: 'Дата размещения документа'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentFile',
                    width: 100,
                    text: 'Файл',
                    renderer: function (v) {
                        return v ? ('<a href="' + B4.Url.action('/FileUpload/Download?id=' + v.Id) + '" target="_blank" style="color: black">Скачать</a>') : '';
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
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 2,
                            items: [
                                { xtype: 'b4addbutton' },
                                { xtype: 'b4updatebutton' }
                            ]
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ]
        });

        me.callParent(arguments);
    }
});
