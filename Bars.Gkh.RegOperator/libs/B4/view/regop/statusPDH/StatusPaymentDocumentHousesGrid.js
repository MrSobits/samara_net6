Ext.define('B4.view.regop.statusPDH.StatusPaymentDocumentHousesGrid', {
    extend: 'B4.ux.grid.Panel',
    alias: 'widget.statuspaymentdocumenthousesgrid',

    requires: [
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.button.Update',
        'B4.ux.button.Delete',
        'B4.ux.grid.column.Enum',
        'B4.ux.grid.column.Edit',
        'B4.ux.grid.toolbar.Paging',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.form.SelectField',
        'B4.store.regop.statusPDH.StatusPaymentDocumentHouses',
        'B4.enums.StatusPaymentDocumentHousesType',
    ],

    title: 'Реестр документов на оплату',
    closable: true,
    enableColumnHide: true,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.regop.statusPDH.StatusPaymentDocumentHouses');

        //store.on('beforeload', function(st, opts) {
        //    return this.fireEvent('beforeload', st, opts);
        //}, me);

        Ext.apply(me, {
            //selModel: Ext.create('Ext.selection.CheckboxModel', {
            //    mode: 'MULTI'
            //}),
            store: store,
            columns: [

                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Name',
                    flex: 1,
                    text: 'МО',
                    filter: { xtype: 'textfield' },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 200,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Address',
                    flex: 1,
                    text: 'Адрес',
                    filter: { xtype: 'textfield' },
                    editor: {
                        xtype: 'textfield',
                        maxLength: 200,
                        allowBlank: false
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Account',
                    flex: 1,
                    text: 'ЛС'
                },
                {
                    xtype: 'b4enumcolumn',
                    enumName: 'B4.enums.StatusPaymentDocumentHousesType',
                    dataIndex: 'State',
                    filter:true,
                    flex: 1,
                    text: 'Статус'
                },
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [           
                        {
                            itemId: 'periodSelect',
                            xtype: 'b4selectfield',
                            store: 'B4.store.regop.ChargePeriod',
                            textProperty: 'Name',
                            editable: false,
                            allowBlank: false,
                            windowContainerSelector: '#' + me.getId(),
                            windowCfg: {
                                modal: true
                            },
                            name: 'ChargePeriod',
                            labelAlign: 'right',
                            fieldLabel: 'Период',
                            labelWidth: 175
                        }
                    ]
                },
                {
                    xtype: 'b4pagingtoolbar',
                    displayInfo: true,
                    store: store,
                    dock: 'bottom'
                }
            ],
            plugins: [Ext.create('B4.ux.grid.plugin.HeaderFilters')],
            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    }
});