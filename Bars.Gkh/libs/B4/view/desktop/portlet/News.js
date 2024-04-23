Ext.define('B4.view.desktop.portlet.News', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.newsportlet',
    ui: 'b4portlet',
    cls: 'x-portlet purple',
    id: 'newsportlet',
    title: 'Новости',
    iconCls: 'wic-list',
    layout: { type: 'vbox', align: 'stretch' },
    header: true,
    draggable: false,
    collapsible: false,
    closable: false,
    isBuilt: false,
    store: 'News',
    column: 3,
    position: 2,
    
    actions: {
        '#allNewsBtn': {
            click: function(){
                b4app.getController('B4.controller.PortalController')
                    .loadController('B4.controller.Public.News');
            }
        },
    },
    
    permissions: [
        {
            name: 'Widget.News',
            applyTo: 'newsportlet',
            selector: 'portalpanel',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
    ],

    dockedItems: [{
        xtype: 'toolbar',
        dock: 'bottom',
        ui: 'b4portlet-footer',
        items: [
            '->',
            {
                xtype: 'button',
                id: 'allNewsBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Все новости'
            }
        ]
    }],

    initComponent: function () {
        var me = this,
            store = me.store;
        if (Ext.isString(store)) {
            store = me.store = Ext.create(store);
        }
        if (store) {
            me.relayEvents(store, ['load'], 'store');
        }

        me.callParent();
    },

    afterRender: function () {
        this.callParent(arguments);
        if (this.store.isStore) {
            if (this.store.getCount() == 0) {
                this.store.load({ limit: 5 });
            } else {
                this.build(this.store);
            }
        }
    },

    listeners: {
        storeload: function (store, records, successful) {
            if (successful) {
                this.build(store);
            }
        }
    },

    build: function (store) {
        if (!this.isBuilt) {
            this.add({
                xtype: 'dataview',
                ui: 'newsportletItem',
                itemSelector: 'div.news-block',
                store: store,
                tpl: Ext.create('Ext.XTemplate',
                    '<div class="news-list">',
                    '<tpl for=".">',
                    '<div class="news-block">',
                    '<div class="news">',
                    '<div class="date">',
                    '<div class="date-inner">',
                    '<div class="day">{NewsDate:date("j")}</div>',
                    '<div class="month">{NewsDate:date("F")}</div>',
                    '</div>',
                    '</div>',
                    '<div class="header">{Header}</div>',
                    '<div class="text">{Contents:this.formatter}</div>',
                    '</div>',
                    '</div>',
                    '</tpl>',
                    '</div>',
                    {
                        formatter: function(val) {
                            return Ext.String.ellipsis(Ext.util.Format.stripTags(val), 250, true);
                        }
                    }
                )
            });
            this.isBuilt = true;
        }
    }

});