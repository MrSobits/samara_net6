Ext.define('B4.view.config.PaymentSourceTreeGrid', {
    extend: 'Ext.Container',

    alias: 'widget.paymentsourcetreegrid',

    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',
        'B4.store.config.PaymentSourceConfig'
    ],

    title: 'Источники для документов на оплату',

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.config.PaymentSourceConfig');

        Ext.apply(me,
            {
            items: [
                {
                    autoScroll: true,
                    useArrows: true,
                    containerScroll: true,
                    flex: 1,
                    rootVisible: false,
                    height: 800,
                    multiSelect: true,
                    xtype: 'treepanel',
                    itemId: 'sourcesConfig',
                    store: store,
                    loadMask: true,
                    viewConfig: {
                        loadMask: true
                    },

                    columns: [
                        {
                            xtype: 'treecolumn',
                            flex: 1,
                            dataIndex: 'Name',
                            sortable: false,
                            draggable: false,
                            text: 'Название источника'
                        },
                        {
                            dataIndex: 'Description',
                            flex: 3,
                            text: 'Описание источника'
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});