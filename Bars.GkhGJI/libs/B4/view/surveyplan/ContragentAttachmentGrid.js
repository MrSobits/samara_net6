Ext.define('B4.view.surveyplan.ContragentAttachmentGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',
        'B4.form.ComboBox',
        'B4.ux.grid.column.Enum',
        'B4.enums.Month',
        'B4.ux.grid.plugin.HeaderFilters'
    ],

    store: 'surveyplan.ContragentAttachment',
    alias: 'widget.surveyPlanContragentAttachmentGrid',
    enableColumnHide: true,

    initComponent: function() {
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
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'Наименование',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Num',
                    flex: 1,
                    text: 'Номер документа',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'Date',
                    width: 100,
                    text: 'Дата документа',
                    format: 'd.m.Y',
                    filter: { xtype: 'datefield', format: 'd.m.Y', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    flex: 1,
                    text: 'Описание',
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'File',
                    text: 'Файл',
                    flex: 1,
                    renderer: function (v) {
                        return Ext.String.format('<a href="{0}" target="_blank" style="color: blue;">{1}</a>', B4.Url.action('Download', 'FileUpload', { id: v.Id }), "Скачать");
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
                            xtype: 'b4addbutton'
                        },
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