Ext.define('B4.view.finactivity.RealityObjCommunalServiceGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.fincommunalservicerogrid',
    store: 'finactivity.RealityObjCommunalService',
    itemId: 'finActivityRealityObjCommunalServiceGrid',
    title: 'Финансовые показатели по коммунальным услугам',
    padding: 5,
    
    requires: [
        'B4.view.Control.GkhDecimalField',
        
        'B4.enums.TypeServiceDi'
    ],

    initComponent: function() {
        var me = this;
        Ext.applyIf(me, {
            columnLines: true,
            minHeight: 270,
            columns: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'TypeServiceDi',
                    flex: 1,
                    text: 'Услуга',
                    renderer: function (val) { return B4.enums.TypeServiceDi.displayRenderer(val); }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidOwner',
                    flex: 1,
                    text: 'Оплачено собственниками (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DebtOwner',
                    flex: 1,
                    text: 'Задолженность собственников (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidByIndicator',
                    flex: 1,
                    text: 'Оплачено по показаниям общедомовых ПУ (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PaidByAccount',
                    flex: 1,
                    text: 'Оплачено по счетам на общедомовые нужды (тыс.руб.)',
                    editor: 'gkhdecimalfield'
                }
            ],
            plugins: [
                Ext.create('Ext.grid.plugin.CellEditing', {
                    clicksToEdit: 1,
                    pluginId: 'cellEditing',
                    listeners: {
                        edit: function(editor, eventArgs, eventOpts) {
                            var store = editor.grid.getStore(),
                                summaryRecord = store.findRecord('TypeServiceDi', B4.enums.TypeServiceDi.Summury),
                                sum = 0;

                            if (eventArgs.record.get('TypeServiceDi') != B4.enums.TypeServiceDi.Summury && summaryRecord) {
                                summaryRecord.set(eventArgs.field, 0);
                                store.each(function(record) { sum += record.get(eventArgs.field); });
                                summaryRecord.set(eventArgs.field, sum);
                            }
                        }
                    }
                })
            ],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});