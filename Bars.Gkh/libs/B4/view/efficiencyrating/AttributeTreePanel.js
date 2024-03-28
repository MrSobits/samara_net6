Ext.define('B4.view.efficiencyrating.AttributeTreePanel',{
    extend: 'Ext.tree.Panel',
    requires: [
        'B4.ux.button.Add',
        'B4.ux.button.Update',

        'B4.store.metavalueconstructor.DataMetaInfoTree'
    ],

    alias: 'widget.efAttributeTreePanel',
    style: 'border-top: 1px solid rgb(153, 188, 232)',
    border: false,
    closable: false,
    animate: true,
    autoScroll: true,
    useArrows: true,
    containerScroll: true,
    loadMask: true,
    rootVisible: false,

    initComponent: function() {
        var me = this,
            store = Ext.create('B4.store.metavalueconstructor.DataMetaInfoTree', { autoDestroy: true });

        me.addEvents('beforerowaction', 'rowaction');

        Ext.applyIf(me, {
            store: store,
            tbar: [
                {
                    xtype: 'b4addbutton',
                    menu: {
                        items: [
                            {
                                text: 'Коэффициент',
                                level: 1
                            },
                            {
                                text: 'Атрибут',
                                level: 2
                            }
                        ]
                    }
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
            ],

            columns: [
                {
                    xtype: 'treecolumn',
                    dataIndex: 'Name',
                    editor: 'textfield',
                    text: 'Наименование',
                    flex: 1,
                    menuDisabled: true,
                    sortable: false,
                    draggable: false
                },
                {
                    text: 'Код',
                    dataIndex: 'Code',
                    editor: 'textfield',
                    menuDisabled: true,
                    sortable: false,
                    draggable: false,
                    width: 100
                }
            ],
            listeners: {
                beforeitemexpand: function() {
                    Ext.suspendLayouts();
                },

                afteritemexpand: function() {
                    Ext.resumeLayouts(true);
                },

                beforeitemcollapse: function() {
                    Ext.suspendLayouts();
                },

                afteritemcollapse: function() {
                    Ext.resumeLayouts(true);
                }
            }
        });

        me.callParent(arguments);
    },

    fireRowAction: function (record, action) {
        var me = this;
        if (me.fireEvent('beforerowaction', me, action, record)) {
            me.fireEvent('rowaction', me, action, record);
        }
    },

    listeners: {
        'itemclick': {
            fn: function (view, record) {
                var me = this;
                me.fireRowAction(record, 'doubleclick');
            }
        }
    }
});