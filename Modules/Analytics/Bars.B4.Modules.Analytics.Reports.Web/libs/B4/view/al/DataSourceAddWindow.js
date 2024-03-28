Ext.define('B4.view.al.DataSourceAddWindow', {
    extend: 'Ext.window.Window',
    alias: 'widget.datasourceaddwindow',
    closeAction: 'destroy',
    width: 600,
    height: 350,
    layout: 'fit',
    requires: [
        'B4.ux.button.Close',
        'B4.model.al.DataSourceNode'
    ],

    title: 'Источник данных',

    initComponent: function () {
        var me = this,
            store = Ext.create('Ext.data.TreeStore', {
                fields: ['text', 'id', 'type'],
                proxy: {
                    type: 'memory'
                },
                root: {
                    text: 'Root',
                    expanded: true,
                    leaf: false,
                    children: []
                }
            });

        me.addEvents('beforesetlist');

        Ext.apply(me, {
            items: [
                {
                    xtype: 'treepanel',
                    store: store,
                    border: false,
                    rootVisible: false,
                    useArrows: true,
                    columns: [
                        {
                            xtype: 'treecolumn',
                            dataIndex: 'text',
                            text: 'Наименование',
                            flex: 2,
                            sortable: true
                        },
                        {
                            dataIndex: 'type',
                            text: 'Описание',
                            flex: 1,
                            sortable: true
                        }
                    ]
                }
            ],
            dockedItems: [
                {
                    xtype: 'toolbar',
                    dock: 'top',
                    items: [
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'button',
                                    text: 'Выбрать',
                                    name: 'pickdata',
                                    iconCls: 'icon-add'
                                }
                            ]
                        },
                        '->',
                        {
                            xtype: 'buttongroup',
                            columns: 1,
                            items: [
                                {
                                    xtype: 'b4closebutton'
                                }
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);

        B4.Ajax.request(B4.Url.action('GetTree', 'DataSourceTree'))
            .next(function (response) {
                var json = Ext.JSON.decode(response.responseText);
                me.fireEvent('beforesetlist', me, json.data);
                me.down('treepanel').getRootNode().appendChild(json.data);
        });
    }
});