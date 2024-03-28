Ext.define('B4.view.desktop.portlet.TaskTable', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.tasktable',
    ui: 'b4portlet',
    iconCls: 'wic-eye',
    requires: [
        'B4.enums.TypeReminder',
        'B4.enums.CategoryReminder'
    ],
    layout: { type: 'vbox', align: 'stretch' },
    collapsible: false,
    closable: false,
    header: true,
    footer: true,
    isBuilt: false,
    title: 'Доска задач',
    
    dockedItems: [{
        xtype: 'toolbar',
        dock: 'bottom',
        ui: 'b4portlet-footer',
        items: [
            '->',
            {
                xtype: 'button',
                itemId: 'refreshBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Обновить'
            },
            {
                xtype: 'button',
                itemId: 'appealCitTasksBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Обращения',
                listeners: {
                    click: function (btn) {
                        Ext.History.add('reminderappealcits');
                    }
                }
            },
            {
                xtype: 'button',
                itemId: 'allTasksBtn',
                ui: 'searchportlet-footer-btn-rt',
                height: 30,
                text: 'Все задачи'
            }
        ]
    }],
    
    initComponent: function () {
        var me = this,
            store = me.store;
        if (Ext.isString(store)) {
            store = me.store = Ext.getStore(store);
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
                this.store.load({ limit: 4 });
            } else {
                this.build(this.store);
            }
        }
    },
    
    listeners: {
        storeload: function (store, records, successful) {
            if (successful) {
                this.build(store, records);
            }
        }
    },
    
    build: function (store) {
        
        if (!this.isBuilt) {

            this.add({
                xtype: 'dataview',
                ui: 'inspectorportletItem',
                itemSelector: 'a.link-link',
                itemId: 'taskTableDataView',
                store: store,
                tpl: Ext.create('Ext.XTemplate',
                    '<ul class="check-list">',
                        '<tpl for=".">',
                        '<li>',
                            '<a href="javascript:void(0);" class="check-list-item link-link">',
                                '<div class="check-list-item-inner pull-left">',
                                    '<input type="hidden" class="reminderItem" value="{Id}">',
                                    '<div><span class="date {Color}">{CheckDate:date("d.m.Y")}</span><span class="type {ColorTypeReminder}">{[B4.enums.TypeReminder.getMeta(values.TypeReminder).Display]}</span></div>',
                                    '<div><span class="title">{NumText}: </span><span class="text">{Num}</span></div>',
                                    '<div><span class="title">Категория: </span><span class="text">{[B4.enums.CategoryReminder.getMeta(values.CategoryReminder).Display]}</span></div>',
                    
                                    '<tpl if="values.ContragentName">',
                                         '<div><span class="title">ЮЛ: </span><span class="text">{ContragentName}</span></div>',
                                    '</tpl>',
                    
                                    '<tpl if="values.AppealCorr">',
                                         '<div><span class="title">ФИО: </span><span class="text">{AppealCorr}</span></div>',
                                    '</tpl>',
                    
                                    '<tpl if="values.AppealCorrAddress">',
                                         '<div><span class="title">Адрес: </span><span class="text">{AppealCorrAddress}</span></div>',
                                    '</tpl>',
                    
                                    '<tpl if="values.AppealDescription">',
                                         '<div><span class="title">Описание: </span><span class="text">{AppealDescription}</span></div>',
                                    '</tpl>',
                                    
                                '</div>',
                                '<div class="pull-right">',
                                    '<div class="arrow-right"></div>',
                                '</div>',
                                '<div class="clear"></div>',
                            '</a>',
                         '</li>',
                         '</tpl>',
                    '</ul>'
                )
            });
            
            
            
            this.isBuilt = true;
        }
        
    }
});