Ext.define('B4.view.desktop.portlet.TaskControl', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.taskcontrol',
    ui: 'b4portlet',
    iconCls: 'wic-search',

    title: 'Контроль задач',
    layout: { type: 'vbox', align: 'stretch' },
    collapsible: false,
    closable: false,
    header: true,
    footer: true,
    isBuilt: false,

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
                    click: function(btn) {
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
            me.relayEvents(store, ['beforeload', 'load'], 'store');
        }

        me.mask = Ext.create('Ext.LoadMask', me, {
            msg: "Загрузка.."
        });

        me.callParent();
    },

    afterRender: function () {
        this.callParent(arguments);
        if (this.store.isStore) {
            if (this.store.getCount() == 0) {
                this.store.load();
            } else {
                this.build(this.store);
            }
        }
    },

    listeners: {
        storebeforeload: function () {
            this.mask.show();
        },
        storeload: function (store, records, successful) {
            var me = this;
            if (successful) {
                if (me.isBuilt) {
                    me.refresh();
                }
                me.build(store, records);
            }
            me.mask.hide();
        }
    },

    refresh: function () {
        var me = this;
        me.removeAll();
        me.isBuilt = false;
    },

    build: function (store, records) {
        var me = this;
        
        if (!this.isBuilt && records) {

            if (records.length > 0) {

                var stringTpl = '';

                Ext.each(records, function(record, i) {
                    stringTpl += '<div class="task-list-item">' +
                        '<div class="task-list-item-inner">' +
                        '<div class="pull-left">' +
                            '<span class="title" inspectorId=' + record.get("InspectorId") + '>' + record.get("InspectorFio") + '</span>' +
                        '</div>' +
                        '<div class="pull-right">' +
                        '<ul class="squares" typeWidget="control">' +
                        '<li class="red" inspectorId=' + record.get("InspectorId") + '>' + record.get("CountRed") + '</li>' +
                        '<li class="yellow" inspectorId=' + record.get("InspectorId") + '>' + record.get("CountYellow") + '</li>' +
                        '<li class="green" inspectorId=' + record.get("InspectorId") + '>' + record.get("CountGreen") + '</li>' +
                        '<li class="white" inspectorId=' + record.get("InspectorId") + '>' + record.get("CountWhite") + '</li>' +
                        '</ul>' +
                        '</div>' +
                        '<div class="clearfix"></div>' +
                        '</div>' +
                        '</div>';

                });
            }
            
            me.add({
                xtype: 'component',
                ui: 'tasksportlet',
                renderTpl: new Ext.XTemplate(
                    '<div class="task-list">',
                            '<div class="legend-item">',
                                '<ul class="squares max">',
                                    '<li class="red" data-qtip="Задачи, контрольный срок которых уже прошел">Просроченных</li>',
                                    '<li class="yellow" data-qtip="Задачи, до наступления контрольного срока которых осталось менее 5 дней">Подх. к сроку</li>',
                                    '<li class="green" data-qtip="Задачи, до наступления контрольного срока которых осталось более 5 дней">В пределах срока</li>',
                                    '<li class="white" data-qtip="Основания проверок, по которым не сформированы приказы">Нет приказа</li>',
                                '</ul>',
                            '</div>',
                            '<div style="overflow-x: auto; max-height: 280px;">',
                                stringTpl,
                            '</div>',
                    '</div>'
                )
            });

            Ext.select('ul[typeWidget="control"] li').on('click', function () {
                var inspectorId = this.getAttribute('inspectorId'),
                    colorType = this.getAttribute('class');

                me.fireEvent('openregistrtaskcontrol', colorType, inspectorId);
            });
            
            Ext.select('span.title').on('click', function () {
                var inspectorId = this.getAttribute('inspectorId'),
                    colorType = 'all';

                me.fireEvent('openregistrtaskcontrol', colorType, inspectorId);
            });
            
            this.isBuilt = true;
        }
    }
});