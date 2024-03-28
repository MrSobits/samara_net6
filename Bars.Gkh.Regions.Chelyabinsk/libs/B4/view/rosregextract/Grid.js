Ext.define('B4.view.rosregextract.Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
         'B4.enums.YesNoNotSet'
    ],

    title: 'Реестр выписок',
    store: 'RosRegExtractBig',
    alias: 'widget.rosregextractgrid',
    closable: true,
    enableColumnHide: true,

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
                    dataIndex: 'Id',
                    flex: 0.2,
                    text: 'Id'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CadastralNumber',
                    flex: 1,
                    text: 'Кадастровый номер',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 3.8,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExtractDate',
                    flex: 0.5,
                    text: 'Дата выписки',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ExtractNumber',
                    flex: 1,
                    text: 'Номер выписки',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RoomArea',
                    flex: 0.5,
                    text: 'Площадь',
                    filter: { xtype: 'textfield' },
                    hidden: false
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
                            xtype: 'b4updatebutton'
                        }
                    ]
                },
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