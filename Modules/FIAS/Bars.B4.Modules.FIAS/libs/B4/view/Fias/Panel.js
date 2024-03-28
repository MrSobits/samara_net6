Ext.define('B4.view.Fias.Panel', {
    extend: 'Ext.panel.Panel',
    closable: true,
    layout: 'fit',
    title: 'ФИАС',
    itemId: 'fiasPanel',

    requires: ['Ext.data.TreeStore'],

    initComponent: function () {
        var me = this;

        var storePlaces = Ext.create('Ext.data.TreeStore', {
            root: {
                expanded: true
            },
            proxy: {
                type: 'ajax',
                url: B4.Url.action('/Fias/GetObjectsWithoutStreet')
            },
            fields: [
                //{ name: 'id' },
                { name: 'text' },
                { name: 'expanded' },
                { name: 'children' },
                { name: 'leaf' },
                { name: 'level' },
                { name: 'fiasId' },
                { name: 'fiasParentId' },
                { name: 'fiasGuidId' },
                { name: 'fiasParentGuidId' },
                { name: 'fiasCode' },
                { name: 'mirrorGuid' }
            ]
        });

        Ext.applyIf(me, {
            items: [
                {
                    xtype: 'container',
                    layout: 'border',
                    items:
                    [
                        {
                            xtype: 'panel',
                            region: 'west',
                            layout: 'fit',
                            width: 270,
                            split: true,
                            collapsible: true,
                            margins: '0 2 0 0',
                            items: [
                                {
                                    xtype: 'treepanel',
                                    itemId: 'fiasTreePanel',
                                    title: 'Адресный объект',
                                    store: storePlaces,
                                    rootVisible: false,
                                    useArrows: true,
                                    tbar: {
                                        xtype: 'toolbar',
                                        itemId: 'navigationToolbar',
                                        disabled: true,
                                        dock: 'top',
                                        items: [
                                            {
                                                xtype: 'buttongroup',
                                                columns: 1,
                                                items: [
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'icon-add',
                                                        text: 'Добавить в корень',
                                                        textAlign: 'left',
                                                        itemId: 'btnAddRoot'
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'icon-add',
                                                        text: 'Добавить дочерний',
                                                        textAlign: 'left',
                                                        itemId: 'btnAddChildren'
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'icon-add',
                                                        text: 'Заменить',
                                                        textAlign: 'left',
                                                        itemId: 'btnReplace'
                                                    }
                                                ]
                                            },
                                            {
                                                xtype: 'buttongroup',
                                                columns: 1,
                                                items: [
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'icon-pencil',
                                                        text: 'Редактировать',
                                                        textAlign: 'left',
                                                        itemId: 'btnEdit'
                                                    },
                                                    {
                                                        xtype: 'button',
                                                        iconCls: 'icon-delete',
                                                        text: 'Удалить',
                                                        textAlign: 'left',
                                                        itemId: 'btnDelete'
                                                    }
                                                ]
                                            }
                                        ]
                                    }
                                }
                            ]
                        },
                        {
                            xtype: 'panel',
                            region: 'center',
                            layout: 'fit',
                            title: 'Улицы',
                            items: [
                                Ext.create('B4.view.Fias.StreetGrid', { bodyStyle: 'backrgound-color:transparent;'})
                            ]
                        }
                    ]
                }
            ]
        });

        me.callParent(arguments);
    }
});