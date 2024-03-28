Ext.define('B4.view.desktop.portlet.TaskState', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.taskstate',
    ui: 'b4portlet',
    iconCls: 'wic-search',

    title: 'Состояние задач',
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
            me.relayEvents(store, ['beforeload','load'], 'store');
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
        storebeforeload: function() {
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
        var me = this,
            recordsDict,
            sumRed = 0,
            sumYellow = 0,
            sumGreen = 0,
            red,
            yellow,
            green;
            
        if (!this.isBuilt) {
            if (records.length > 0) {

                recordsDict = new Ext.util.MixedCollection();

                Ext.each(records, function (record, i) {

                    red = record.get('CountRed');
                    yellow = record.get('CountYellow');
                    green = record.get('CountGreen');
                    white = record.get('CountWhite');

                    recordsDict.add(
                        record.get('TypeReminder'),
                        {
                            red: red,
                            yellow: yellow,
                            green: green,
                            white: white
                        });

                    if (record.get('TypeReminder') != 20) {
                        sumRed += red;
                        sumYellow += yellow;
                        sumGreen += green;
                    }
                });
                
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
                             '<div class="head-item">',
                                 '<div class="pull-left"><span class="title">Всего</span></div>',
                                 '<div class="pull-right">',
                                     '<ul class="squares" typeWidget="total">',
                                         '<li class="red">' + sumRed + '</li>',
                                         '<li class="yellow">' + sumYellow + '</li>',
                                         '<li class="green">' + sumGreen + '</li>',
                                         '<li class="white">' + (recordsDict.containsKey('20') ? recordsDict.get('20').white : 0) + '</li>',
                                     '</ul>',
                                 '</div>',
                                 '<div class="clearfix"></div>',
                             '</div>',
                             '<div class="task-list-item">',
                                 '<div class="task-list-item-inner">',
                                     '<div class="pull-left">',
                                         '<span class="title">Приказ</span>',
                                     '</div>',
                                     '<div class="pull-right">',
                                         '<ul class="squares" typeWidget="disposal">',
                                             '<li class="red">' + (recordsDict.containsKey('30') ? recordsDict.get('30').red : 0) + '</li>',
                                             '<li class="yellow">' + (recordsDict.containsKey('30') ? recordsDict.get('30').yellow : 0) + '</li>',
                                             '<li class="green">' + (recordsDict.containsKey('30') ? recordsDict.get('30').green : 0) + '</li>',
                                             '<li class="white">0</li>',
                                         '</ul>',
                                     '</div>',
                                     '<div class="clearfix"></div>',
                                 '</div>',
                             '</div>',
                             '<div class="task-list-item">',
                                 '<div class="task-list-item-inner">',
                                     '<div class="pull-left">',
                                         '<span class="title">Предписание</span>',
                                     '</div>',
                                     '<div class="pull-right">',
                                         '<ul class="squares" typeWidget="prescription">',
                                             '<li class="red">' + (recordsDict.containsKey('40') ? recordsDict.get('40').red : 0) + '</li>',
                                             '<li class="yellow">' + (recordsDict.containsKey('40') ? recordsDict.get('40').yellow : 0) + '</li>',
                                             '<li class="green">' + (recordsDict.containsKey('40') ? recordsDict.get('40').green : 0) + '</li>',
                                             '<li class="white">0</li>',
                                         '</ul>',
                                     '</div>',
                                     '<div class="clearfix"></div>',
                                 '</div>',
                             '</div>',
                             '<div class="task-list-item">',
                                 '<div class="task-list-item-inner">',
                                     '<div class="pull-left">',
                                         '<span class="title">Акт проверки</span>',
                                     '</div>',
                                     '<div class="pull-right">',
                                         '<ul class="squares" typeWidget="actcheck">',
                                             '<li class="red">' + (recordsDict.containsKey('60') ? recordsDict.get('60').red : 0) + '</li>',
                                             '<li class="yellow">' + (recordsDict.containsKey('60') ? recordsDict.get('60').yellow : 0) + '</li>',
                                             '<li class="green">' + (recordsDict.containsKey('60') ? recordsDict.get('60').green : 0) + '</li>',
                                             '<li class="white">0</li>',
                                         '</ul>',
                                     '</div>',
                                     '<div class="clearfix"></div>',
                                 '</div>',
                             '</div>',
                             '<div class="task-list-item">',
                                 '<div class="task-list-item-inner">',
                                     '<div class="pull-left">',
                                         '<span class="title">Уведомление о проверке</span>',
                                     '</div>',
                                     '<div class="pull-right">',
                                         '<ul class="squares" typeWidget="noticeofinspection">',
                                             '<li class="red">' + (recordsDict.containsKey('70') ? recordsDict.get('70').red : 0) + '</li>',
                                             '<li class="yellow">' + (recordsDict.containsKey('70') ? recordsDict.get('70').yellow : 0) + '</li>',
                                             '<li class="green">' + (recordsDict.containsKey('70') ? recordsDict.get('70').green : 0) + '</li>',
                                             '<li class="white">0</li>',
                                         '</ul>',
                                     '</div>',
                                     '<div class="clearfix"></div>',
                                 '</div>',
                             '</div>',
                             '<div class="task-list-item">',
                                 '<div class="task-list-item-inner">',
                                     '<div class="pull-left">',
                                         '<span class="title">Обращение граждан</span>',
                                     '</div>',
                                     '<div class="pull-right">',
                                         '<ul class="squares" typeWidget="appeal">',
                                             '<li class="red">' + (recordsDict.containsKey('10') ? recordsDict.get('10').red : 0) + '</li>',
                                             '<li class="yellow">' + (recordsDict.containsKey('10') ? recordsDict.get('10').yellow : 0) + '</li>',
                                             '<li class="green">' + (recordsDict.containsKey('10') ? recordsDict.get('10').green : 0) + '</li>',
                                             '<li class="white">0</li>',
                                         '</ul>',
                                     '</div>',
                                     '<div class="clearfix"></div>',
                                 '</div>',
                             '</div>',
                        '</div>'
                    )
                });
                
                Ext.select('ul[typeWidget="appeal"] li').on('click', function () {
                    var typeReminder = 10,
                        colorType = this.getAttribute('class');
                    
                    me.fireEvent('openregistrtaskstate', colorType, typeReminder);
                });
                
                Ext.select('ul[typeWidget="prescription"] li').on('click', function () {
                    var typeReminder = 40,
                        colorType = this.getAttribute('class');
                    
                    me.fireEvent('openregistrtaskstate', colorType, typeReminder);
                });
                
                Ext.select('ul[typeWidget="disposal"] li').on('click', function () {
                    var typeReminder = 30,
                        colorType = this.getAttribute('class');
                    
                    me.fireEvent('openregistrtaskstate', colorType, typeReminder);
                });
                
                Ext.select('ul[typeWidget="actcheck"] li').on('click', function () {
                    var typeReminder = 60,
                        colorType = this.getAttribute('class');

                    me.fireEvent('openregistrtaskstate', colorType, typeReminder);
                });
                
                Ext.select('ul[typeWidget="noticeofinspection"] li').on('click', function () {
                    var typeReminder = 70,
                        colorType = this.getAttribute('class');

                    me.fireEvent('openregistrtaskstate', colorType, typeReminder);
                });
                
                Ext.select('ul[typeWidget="total"] li').on('click', function () {
                    var colorType = this.getAttribute('class');
                    
                    me.fireEvent('openregistrtaskstate', colorType, 0);
                });

            }
        }
        this.isBuilt = true;
    }
});