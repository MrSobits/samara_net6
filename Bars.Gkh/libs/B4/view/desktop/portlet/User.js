Ext.define('B4.view.desktop.portlet.User', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.userportlet',
    ui: 'b4portlet',
    cls: 'x-portlet green',
    id: 'userportlet',
    title: 'Пользователь',
    iconCls: 'wic-user',
    layout: { type: 'vbox', align: 'stretch' },
    collapsible: false,
    closable: false,
    header: true,
    draggable: false,
    //maxWidth: 350,
    //minWidth: 270,
    isBuilt: false,
    column: 3,
    position: 1,
    store: 'desktop.ActiveOperator',

    actions: {
        '#profileBtn': {
            click: function() {
                Ext.History.add('profilesettingadministration');
            }
        },

        '#logoutBtn': {
            click: function() {
                Gkh.signalR.stop();
                window.location = B4.Url.action('LogOut', 'Login');
            }
        },
    },

    permissions: [
        {
            name: 'Widget.ActiveOperator',
            applyTo: 'userportlet',
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
                id: 'profileBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Профиль'
            },
            {
                xtype: 'button',
                id: 'logoutBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Выйти'
            }
        ]
    }],

    initComponent: function () {
        var me = this,
            store = me.store;
        if (Ext.isString(store)) {
            store = me.store = Ext.create(store);
        }
        me.relayEvents(store, ['load'], 'store');
        me.callParent();
    },

    afterRender: function () {
        this.callParent(arguments);
        if (this.store && this.store.isStore) {
            if (this.store.getCount() == 0) {
                this.store.load();
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
            var record = store.findRecord('IsActive', true);
            this.add({
                xtype: 'component',
                ui: 'userportletItem',
                renderTpl: new Ext.XTemplate(
                    '<div class="widget-item">',
                        '<div class="widget-item-inner">',
                            '<div class="user-name">&nbsp;{Name}</div>',
                            '<div class="user-role">&nbsp;{Role.Name}</div>',
                        '</div>',
                        '<div class="date-wrap">',
                            '<div class="pull-left">',
                                '<div class="img-note"></div>',
                            '</div>',
                            '<div class="pull-left">',
                                '<div class="text">Сегодня:</div>',
                                '<div class="text date" id="userportlet-dateEl">&nbsp;</div>',
                            '</div>',
                            '<div class="pull-right clock" id="userportlet-clockEl">&nbsp;</div>',
                        '</div>',
                    '</div>'
                ),
                renderData: record ? record.getData() : {
                    Name: 'Неизвестно',
                    Role: { Name: 'Неизвестно' }
                }
            });

            Ext.TaskManager.start({
                run: this.updateClock,
                interval: 1000
            });
        }
        this.isBuilt = true;
    },

    updateClock: function () {
        
        var month = [   'Января',
                        'Февраля',
                        'Марта',
                        'Апреля',
                        'Мая',
                        'Июня',
                        'Июля',
                        'Августа',
                        'Сентября',
                        'Октября',
                        'Ноября',
                        'Декабря'];

        var date = new Date();

        var dateEl = Ext.fly('userportlet-dateEl');
        if (dateEl) {
            dateEl.update(
                Ext.String.format("{0} {1}",
                    date.getDate(),
                    month[date.getMonth()])
            );
        }

        var clockEl = Ext.fly('userportlet-clockEl');
        if(clockEl) {
            clockEl.update(
            Ext.String.format(
                "{0}:{1}",
                date.getHours(),
                date.getMinutes() > 9 ? date.getMinutes() : "0" + date.getMinutes()
            ));
        }
    }
});