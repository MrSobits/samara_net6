Ext.define('B4.view.complaints.StepGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.button.Save',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.enums.DOPetitionResult',
        'B4.enums.DOTypeStep',
        'B4.enums.YesNo',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.toolbar.Paging',
         'B4.form.ComboBox',
    ],
    alias: 'widget.complaintsstepgrid',
    title: 'Этапы рассмотрения',
    store: 'complaints.SMEVComplaintsStep',

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
                    enumName: 'B4.enums.DOTypeStep',
                    filter: true,
                    text: 'Этап рассмотрения',
                    dataIndex: 'DOTypeStep',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.DOPetitionResult',
                    filter: true,
                    text: 'Решение',
                    dataIndex: 'DOPetitionResult',
                    flex: 1
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.YesNo',
                    filter: true,
                    text: 'Отправлен в СМЭВ',
                    dataIndex: 'YesNo',
                    flex: 1
                },
                {
                    xtype: 'actioncolumn',
                    text: 'Операция',
                    action: 'openpassport',
                    width: 150,
                    items: [{
                        tooltip: 'Отправить в СМЭВ',
                        iconCls: 'icon-fill-button',
                        icon: B4.Url.content('content/img/btnSend.png')
                    }]
                },
                {
                    xtype: 'b4deletecolumn',
                    scope: me
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
            },
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 3,
                            items: [
                                {
                                    xtype: 'b4addbutton'
                                },
                                {
                                    xtype: 'b4updatebutton'
                                },
                                 {
                                     xtype: 'b4savebutton'
                                 }
                            ]
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