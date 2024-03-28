Ext.define('B4.view.manorglicense.RequestInspectionGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.manorglicenserequestinspgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.store.baselicenseapplicants.ForLicenseRequest'
    ],

    title: 'Проверки',

    closable: false,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.baselicenseapplicants.ForLicenseRequest');

        Ext.applyIf(me, {
            store: store,
            columnLines: true,
            columns: [
                //{
                //    xtype: 'b4editcolumn',
                //    scope: me
                //},
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionNumber',
                    flex: 2,
                    text: 'Номер проверки',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DisposalNumber',
                    flex: 1,
                    text: 'Номер распоряжения',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DisposalDate',
                    flex: 1,
                    text: 'Дата распоряжения',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'AppealNumber',
                    flex: 2,
                    text: 'Номер обращения',
                    filter: { xtype: 'textfield' }
                },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'AppealDate',
                     flex: 1,
                     text: 'Дата обращения',
                     format: 'd.m.Y',
                     filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'InspectionResult',
                    flex: 2,
                    text: 'Результат проверки',
                    filter: { xtype: 'textfield' }
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
                                    xtype: 'b4updatebutton'
                                }
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