Ext.define('B4.view.objectcr.BuildContractForMassBuildGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.buildcontractformassbuildgrid',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.ux.grid.column.Delete',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.toolbar.Paging',
        'B4.Ajax', 'B4.Url',
        'B4.form.ComboBox',
        'B4.aspects.StateButton',
        'B4.form.GridStateColumn',
        'B4.ux.grid.selection.CheckboxModel'
    ],

    title: 'Созданные договоры',

    initComponent: function () {
        var me = this,
            store = Ext.create('B4.store.objectcr.BuildContractForMassBuild');

        Ext.applyIf(me, {
            store: store,
            selModel: Ext.create('B4.ux.grid.selection.CheckboxModel', {}),
            columnLines: true,
            columns: [
                {
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 100
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    flex: 1,
                    text: 'МО'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectCr',
                    flex: 1,
                    text: 'Объект КР'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'DocumentNum',
                    flex: 1,
                    text: 'Номер'
                },
                {
                    xtype: 'datecolumn',
                    format: 'd.m.Y',
                    dataIndex: 'DocumentDateFrom',
                    flex: 1,
                    text: 'Дата'
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    flex: 1,
                    text: 'Сумма договора (руб.)'
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
                            xtype: 'b4combobox',
                            hideLabel: true,
                            editable: false,
                            valueField: 'Id',
                            displayField: 'Name',
                            itemId: 'cbChangeState',
                            url: '/BuildContract/ListAvailableStates'
                        },
                        {
                            xtype: 'button',
                            iconCls: 'icon-accept',
                            itemId: 'btnState',
                            text: 'Сменить статусы',
                            listeners: {
                                click: function () {
                                    var records = me.selModel.getSelection(),
                                        comboStateId = me.down('#cbChangeState').getValue();
                                        recordIds = [];
                                    
                                    if (comboStateId != null) {

                                        Ext.each(records, function (item) {
                                            recordIds.push(item.get('Id'));
                                        });

                                        if (recordIds[0] > 0) {
                                            B4.Ajax.request(B4.Url.action('ChangeBuildContractStateFromMassBuild', 'ObjectCr', {
                                                contractIds: recordIds,
                                                newStateId: comboStateId
                                            })).next(function (response) {
                                                return true;
                                            }).error(function () {
                                            });
                                        }

                                        store.load();
                                    }
                                    else {
                                        Ext.Msg.alert('Внимание!', 'Необходимо выбрать статус!');
                                    }
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