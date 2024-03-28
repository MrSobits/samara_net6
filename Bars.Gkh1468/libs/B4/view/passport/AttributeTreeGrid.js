Ext.define('B4.view.passport.AttributeTreeGrid', {
    extend: 'Ext.tree.Panel',
    alias: 'widget.attrtreegrid',

    requires: ['Ext.tree.plugin.TreeViewDragDrop', 'B4.store.passport.AttrTreeStore'],
    controllerName: 'MetaAttribute',

    initComponent: function () {
        var me = this;

        me.addEvents({
            'attrtreenodesreordered': true
        });

        Ext.apply(me, {
            tbar: [
                    {
                        text: 'Добавить',
                        cmd: 'addattr',
                        iconCls: 'icon-add',
                        menu: {
                            items: [
                                {
                                    text: 'Простой',
                                    attrtype: 'simple'
                                },
                                {
                                    text: 'Групповой',
                                    attrtype: 'grouped'
                                },
                                {
                                    text: 'Групповой со значением',
                                    attrtype: 'groupedwithval'
                                },
                                {
                                    text: 'Групповой-множественный',
                                    attrtype: 'groupedcomplex'
                                }
                            ]
                        }
                    },
                    {
                        text: 'Обновить',
                        xtype:'b4updatebutton'
                    },
                    {
                        text: 'Удалить',
                        cmd: 'removeattr',
                        iconCls: 'icon-delete'
                    }
            ],
            store: Ext.create('B4.store.passport.AttrTreeStore'),
            viewConfig: {
                plugins: {
                    ptype: 'treeviewdragdrop',
                    ddGroup: 'AttrTreeDD',
                    allowContainerDrops: false
                },
                listeners: {
                    beforedrop: function (node, data, overModel, dropPos, dropFn) {
                        if (dropPos === 'append') {
                            return false;
                        }
                    },
                    drop: function (node, data, overModel, dropPosition) {
                        me.fireEvent('attrtreenodesreordered', me, data.records[0].parentNode);
                    }
                }
            },
            columns: [
               {
                   xtype: 'treecolumn',
                   dataIndex: 'Name',
                   editor: 'textfield',
                   text: 'Название',
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
                   draggable: false
               }
            ]
        });

        me.callParent(arguments);
    }
});