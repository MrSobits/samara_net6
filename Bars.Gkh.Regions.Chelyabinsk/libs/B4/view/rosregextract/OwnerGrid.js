Ext.define('B4.view.rosregextract.OwnerGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox'
    ],

    title: 'Собственники',
    store: 'RosRegExtractBigOwner',
    alias: 'widget.rosregextractownergrid',
    closable: true,
    enableColumnHide: true,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Id',
                    flex: 0.3,
                    text: 'Id'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'OwnerName',
                    flex: 1,
                    text: 'Имя собственника',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaShareNum',
                    flex: 0.3,
                    text: 'Числитель доли',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AreaShareDen',
                    flex: 0.3,
                    text: 'Знаменатель доли',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RightNumber',
                    flex: 0.3,
                    text: 'Номер права',
                    filter: { xtype: 'textfield' },
                    hidden: false
                },

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