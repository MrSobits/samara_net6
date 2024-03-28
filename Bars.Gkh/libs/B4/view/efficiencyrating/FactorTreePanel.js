Ext.define('B4.view.efficiencyrating.FactorTreePanel',{
    extend: 'Ext.tree.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update'
    ],

    title: 'Факторы оценки конкурентоспособности',
    alias: 'widget.efFactorTreePanel',
    order: false,
    closable: false,
    animate: true,
    autoScroll: true,
    useArrows: true,
    containerScroll: true,
    loadMask: true,
    rootVisible: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.metavalueconstructor.DataMetaInfoTree');

        Ext.applyIf(me,
        {
            store: store,
            columns: [
                {
                    xtype: 'treecolumn',
                    dataIndex: 'Name',
                    text: 'Наименование',
                    flex: 1,
                    menuDisabled: true,
                    sortable: false,
                    draggable: false
                }
            ],
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
                                    xtype: 'button',
                                    text: 'Удалить',
                                    iconCls: 'icon-delete',
                                    action: 'delete'
                                }
                            ]
                        }
                    ]
                }
            ],

            viewConfig: {
                loadMask: true
            }
        });

        me.callParent(arguments);
    },

    fireRowAction: function(record, action) {
        var me = this;
        if (me.fireEvent('beforerowaction', me, action, record)) {
            me.fireEvent('rowaction', me, action, record);
        }
    },

    listeners: {
        'itemclick': {
            fn: function(view, record) {
                var me = this;
                me.fireRowAction(record, 'doubleclick');
            }
        }
    }
});