Ext.define('B4.view.StateTransfer.RuleGrid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.form.ComboBox',
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.toolbar.Paging'
    ],

    title: 'Правила перехода статусов',
    store: 'StateTransferRule',
    itemId: 'stateTransferRuleGrid',
    closable: true,

    initComponent: function () {
        var me = this;

        var types = Ext.create('Ext.data.Store', {
            autoLoad: true,
            fields: ['TypeId', 'Name', 'FilterName'],
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/State/StatefulEntityList'),
                reader: { type: 'json', root: 'data' }
            },
            listeners: {
                'load': {
                    fn: function (store, records) {
                        if (records && records.length > 0) {
                            store.insert(0, { TypeId: null, Name: '-', FilterName: '' });
                            Ext.each(records, function (rec) {
                                rec.set('FilterName', rec.get('Name'));
                            });
                        } else {
                            store.add({ TypeId: null, Name: '-', FilterName: '' });
                        }

                        this.typeIdFilter.select(store.data.items[0]);
                    },
                    scope: this
                }
            }
        });

        this.typeIdFilter = Ext.create('Ext.form.ComboBox', {
            operand: CondExpr.operands.eq,
            hideLabel: true,
            editable: false,
            store: types,
            valueField: 'FilterName',
            displayField: 'Name',
            queryMode: 'local',
            triggerAction: 'all'
        });

        Ext.applyIf(me, {
            columnLines: true,
            columns: [
                { xtype: 'b4editcolumn', scope: me },
                { xtype: 'gridcolumn', dataIndex: 'RuleName', flex: 1, text: 'Правило', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'RuleDescription', flex: 1, text: 'Описание', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'TypeName', flex: 1, text: 'Тип объекта', filter: this.typeIdFilter },
                { xtype: 'gridcolumn', dataIndex: 'RoleName', flex: 1, text: 'Роль', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'CurrentState', flex: 1, text: 'Текущий статус', filter: { xtype: 'textfield' } },
                { xtype: 'gridcolumn', dataIndex: 'NewState', flex: 1, text: 'Новый статус', filter: { xtype: 'textfield' } },
                { xtype: 'b4deletecolumn', scope: me }
            ],
            plugins: [
                Ext.create('B4.ux.grid.plugin.HeaderFilters')
            ],
            viewConfig: {
                loadMask: true
            },
            dockedItems: [{
                xtype: 'toolbar',
                dock: 'top',
                items: [{
                    xtype: 'buttongroup',
                    columns: 2,
                    items: [
                        { xtype: 'b4addbutton' },
                        { xtype: 'b4updatebutton' }
                    ]
                }]
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