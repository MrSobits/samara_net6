Ext.define('B4.view.MassCalcReport731Grid', {
    extend: 'B4.ux.grid.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',

        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.ComboBox',
        'B4.base.Store'
    ],

    alias: 'widget.masscalcreport731grid',
    title: 'Массовая генерация отчета по 731 (988) ПП РФ',
    closable: true,

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.base.Store', {
                autoLoad: false,
                fields: [
                    { name: 'ManOrgId' },
                    { name: 'ManOrgName' },
                    {name: 'Inn'},
                    { name: 'CountRo' }
                ],
                proxy: {
                    type: 'b4proxy',
                    controllerName: 'DiReport',
                    listAction: 'ListManOrgsNotExistsReport'
                },
                listeners: {
                    beforeload: function(s, operation) {
                        operation.params.periodId = me.down('b4combobox[name=PeriodDi]').getValue();
                    }
                }
            }),
            selModel = Ext.create('Ext.selection.CheckboxModel', {
                mode: 'MULTI'
            });

        Ext.applyIf(me, {
            selModel: selModel,
            store: store,
            columnLines: true,
            columns: [
                {
                    dataIndex: 'ManOrgName',
                    flex: 1,
                    text: 'Наименование УО',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'Inn',
                    flex: 1,
                    text: 'ИНН',
                    filter: { xtype: 'textfield' }
                },
                {
                    dataIndex: 'CountRo',
                    flex: 1,
                    text: 'Количество домов',
                    filter: { xtype: 'numberfield', operand: CondExpr.operands.eq }
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
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Сгенерировать',
                                    action: 'Generate'
                                },
                                {
                                    xtype: 'button',
                                    text: 'Обновить',
                                    listeners: {
                                        click: function() {
                                            store.load();
                                        }
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'b4combobox',
                            pageSize: 25,
                            url: '/PeriodDi/List',
                            displayValue: 'Name',
                            name: 'PeriodDi',
                            fieldLabel: 'Период',
                            labelAlign: 'right',
                            width: 400,
                            editable: false,
                            readOnly: true,
                            listeners: {
                                storeloaded: function (cmp) {
                                    var s = cmp.getStore(),
                                        recIndex,
                                        record;

                                    recIndex = s.findBy(function (r) {
                                        var name = r.get('Name');
                                        
                                        if (!Ext.isEmpty(name) && name.indexOf('2013') > -1) {
                                            return true;
                                        }

                                        return false;
                                    });
                                    
                                    if (recIndex > -1) {
                                        record = s.data.items[recIndex];
                                        cmp.setValue(record.get('Id'));
                                    }

                                    store.load();
                                }
                            }
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