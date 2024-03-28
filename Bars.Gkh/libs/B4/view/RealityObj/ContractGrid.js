Ext.define('B4.view.realityobj.ContractGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.realobjcontractgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',

        'B4.enums.TypeContractManOrgRealObj'
    ],

    title: 'Управление домом',
    store: 'realityobj.Contract',
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
                    dataIndex: 'ManagingOrganizationName',
                    flex: 1,
                    text: 'Управление домом'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContractManOrgRealObj',
                    flex: 1,
                    text: 'Вид управления',
                    renderer: function (val) {
                        return B4.enums.TypeContractManOrgRealObj.displayRenderer(val);
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeContractString',
                    flex: 1,
                    text: 'Тип договора'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentName',
                    flex: 1,
                    text: 'Документ'
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'StartDate',
                    text: 'Дата начала',
                    format: 'd.m.Y',
                    width: 100
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'EndDate',
                    text: 'Дата окончания',
                    format: 'd.m.Y',
                    width: 100
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
                                {
                                    xtype: 'b4addbutton',
                                    actionName: 'addDirectManag',
                                    text: 'Непосредственное управление'
                                },
                                { xtype: 'b4updatebutton'}
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});