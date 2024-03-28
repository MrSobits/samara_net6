Ext.define('B4.view.overhaulpropose.Grid', {
    extend: 'B4.ux.grid.Panel',
    
    alias: 'widget.overhaulproposegrid',

    requires: [
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.plugin.HeaderFilters',      
        'B4.form.ComboBox',
        'B4.form.GridStateColumn',
        'B4.ux.grid.toolbar.Paging',
        'B4.form.GridStateColumn',       
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.store.dict.Municipality',
        'B4.model.dict.ProgramCr',
        'B4.Url',
        'B4.store.dict.ProgramCrObj'
       
    ],
    closable: true,
    //title: 'Реестр предложений по капремонту',
    store: 'OverhaulProposal',

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
                    xtype: 'b4gridstatecolumn',
                    dataIndex: 'State',
                    text: 'Статус',
                    width: 160,
                    filter: {
                        xtype: 'b4combobox',
                        url: '/State/GetListByType',
                        storeAutoLoad: false,
                        operand: CondExpr.operands.eq,
                        listeners: {
                            storebeforeload: function (field, store, options) {
                                options.params.typeId = 'ovrhl_proposal';
                            },
                            storeloaded: {
                                fn: function (me) {
                                    me.getStore().insert(0, { Id: null, Name: '-' });
                                    me.select(me.getStore().data.items[0]);
                                }
                            }
                        }
                    },
                    processEvent: function (type, view, cell, recordIndex, cellIndex, e) {
                        if (type == 'click' && e.target.localName == 'img') {
                            var record = view.getStore().getAt(recordIndex);
                            view.ownerCt.fireEvent('cellclickaction', view.ownerCt, e, 'statechange', record);
                        }
                    },
                    scope: this
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ProgramCr',
                    text: 'Программа',
                    flex: 3,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/ProgramCr/ListWithoutPaging?forObjCr=true'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Municipality',
                    text: 'Муниципальный район',
                    flex: 1,
                    filter: {
                        xtype: 'b4combobox',
                        operand: CondExpr.operands.eq,
                        storeAutoLoad: false,
                        hideLabel: true,
                        editable: false,
                        valueField: 'Name',
                        emptyItem: { Name: '-' },
                        url: '/Municipality/ListMoAreaWithoutPaging'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'ObjectCr',
                    text: 'Адрес',
                    flex: 2,
                    filter: { xtype: 'textfield' }
                },   
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Description',
                    text: 'Работы',
                    flex: 3,
                    filter: { xtype: 'textfield' }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Apartments',
                    text: 'Кол-во помещений',
                    flex: 0.5
                },               
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Entryes',
                    text: 'Кол-во подъездов',
                    flex: 0.5
                },    
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Index',
                    text: 'Почтовый индекс',
                    flex: 1
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
                            },
                            {
                                xtype: 'button',
                                iconCls: 'icon-table-go',
                                text: 'Экспорт',
                                textAlign: 'left',
                                itemId: 'btnExport'
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