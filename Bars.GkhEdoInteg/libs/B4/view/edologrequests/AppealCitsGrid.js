Ext.define('B4.view.edologrequests.AppealCitsGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.edologReqsAppCitsGrid',
    requires: [
        'B4.form.ComboBox',
        
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        
        'B4.enums.ActionIntegrationRow'
    ],

    store: 'edolog.RequestsAppealCits',
    itemId: 'requestsAppCitsGrid',
    closable: false,

    initComponent: function () {
        var me = this;

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    width: 170,
                    flex: 1,
                    text: 'Номер ГЖИ',
                    filter: { xtype: 'textfield' }
                },
                 {
                     xtype: 'datecolumn',
                     dataIndex: 'DateActual',
                     text: 'Дата актуальности',
                     format: 'd.m.Y H:i:s',
                     width: 140,
                     filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                 },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ActionIntegrationRow',
                    flex: 1,
                    text: 'Действие',
                    renderer: function (val) {
                        return B4.enums.ActionIntegrationRow.displayRenderer(val);
                    },
                    filter: {
                        xtype: 'b4combobox',
                        items: B4.enums.ActionIntegrationRow.getItemsWithEmpty([null, '-']),
                        editable: false,
                        operand: CondExpr.operands.eq,
                        valueField: 'Value',
                        displayField: 'Display'
                    }
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [
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