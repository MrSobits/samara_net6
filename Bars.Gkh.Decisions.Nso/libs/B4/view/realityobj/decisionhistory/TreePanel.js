Ext.define('B4.view.realityobj.decisionhistory.TreePanel', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.decisionhistorytreepanel',

    requires: [
        'B4.ux.button.Update',
        'B4.store.realityobj.decisionhistory.Tree'
    ],

    title: 'История решений',
    
    animate: false,
    autoScroll: true,
    useArrows: true,
    containerScroll: true,
    loadMask: true,
    rowLines: true,
    columnLines: true,
    rootVisible: false,
    
    /**
     * Переопределение чтобы не загружался если autoLoad==false
     */
    setRootNode: function () {
        if (this.getStore().autoLoad) {
            this.callParent(arguments);
        }
    },

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.realityobj.decisionhistory.Tree', {
                autoLoad: false
            });

        Ext.applyIf(me, {
            store: store,
            loader: {
                autoLoad: false
            },
            viewConfig: {
                loadMask: true
            },
            columns: [
                {
                    xtype: 'treecolumn',
                    text: 'Тип решения',
                    dataIndex: 'Type',
                    flex: 1,
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата начала',
                    dataIndex: 'DateStart',
                    format: 'd.m.Y',
                    width: 100,
                    sortable: false
                },
                {
                    xtype: 'datecolumn',
                    text: 'Дата окончания',
                    dataIndex: 'DateEnd',
                    format: 'd.m.Y',
                    width: 100,
                    sortable: false
                },
                {
                    text: 'Протокол',
                    dataIndex: 'Protocol',
                    width: 150,
                    sortable: false
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            items: [
                                {
                                    xtype: 'b4updatebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});