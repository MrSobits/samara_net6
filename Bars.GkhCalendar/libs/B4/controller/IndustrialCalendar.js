Ext.define('B4.controller.IndustrialCalendar', {
    extend: 'B4.base.Controller',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: ['Day'],
    stores: ['B4.store.Month'],
    views: [
        'IndustrialCalendar',
        'calendarday.EditWindow'
    ],

    mainView: 'IndustrialCalendar',
    mainViewSelector: 'industrialcalendar',

    refs: [
        {
            ref: 'mainView',
            selector: 'industrialcalendar'
        },
        {
            ref: 'monthLabel',
            selector: 'industrialcalendar #monthLabel'
        },
        {
            ref: 'yearLabel',
            selector: 'industrialcalendar #yearLabel'
        },
        {
            ref: 'daysPanel',
            selector: 'industrialcalendar #vboxpanel'
        },
        {
            ref: 'monthField',
            selector: 'industrialcalendar #monthField'
        },
        {
            ref: 'yearField',
            selector: 'industrialcalendar #yearField'
        },
        {
            ref: 'headerPanel',
            selector: 'industrialcalendar #headerPanel'
        }
    ],

    aspects: [],

    init: function() {
        var me = this;

        var actions = {};

        actions[this.mainViewSelector + ' #prevBtn'] = { 'click': { fn: this.onChangeMonthPrev, scope: this } };
        actions[this.mainViewSelector + ' #nextBtn'] = { 'click': { fn: this.onChangeMonthNext, scope: this } };
        actions['dayeditwindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
        actions['dayeditwindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
        actions[this.mainViewSelector + ' [action="SelectDate"]'] = { 'click': { fn: this.onSelectDate, scope: this } };
        actions[this.mainViewSelector + ' [action="AcceptDate"]'] = { 'click': { fn: this.onAcceptNewDate, scope: this } };
        actions[this.mainViewSelector + ' [action="CancelDateChange"]'] = { 'click': { fn: this.onCancelDateChange, scope: this } };

        me.control(actions);

        this.date = new Date();

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('industrialcalendar');
        me.bindContext(view);
        me.application.deployView(view);

        me.editWindow = Ext.create('widget.dayeditwindow');

        me.fillData();
    },

    fillData: function() {
        var me = this;
        me.mask('Загрузка...');
        B4.Ajax.request(B4.Url.action('GetDaysList', 'Day', { date: this.date }
        )).next(function(response) {
            var data = Ext.JSON.decode(response.responseText);
            me.createElements(data.days);
            me.unmask();
        }).error(function() {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'При загрузке данных произошла ошибка');
        });
    },

    createElements: function(daysList) {
        var days = this.getDaysPanel(),
            month = this.getMonthLabel(),
            year = this.getYearLabel(),
            date = new Date(this.date);

        if (daysList.length == 0) {
            return;
        }

        days.removeAll();

        month.setText(Ext.util.Format.date(date, 'F'));
        year.setText(date.getFullYear());

        var hbox = null;
        var hboxIndex = 1;
        var index = 1;

        var firstDayWeekDay = daysList[0].dayOfWeek;
        var lastDayWeekDay = daysList[daysList.length - 1].dayOfWeek;

        // добавление 'пустых' элементов в начало массива дней
        while (firstDayWeekDay > 1) {
            --firstDayWeekDay;
            daysList.unshift({ id: -1, dayOfWeek: firstDayWeekDay, number: 0, type: 10 });
        }

        // добавление 'пустых' элементов в конец массива дней
        while (lastDayWeekDay < 7) {
            ++lastDayWeekDay;
            daysList.push({ id: -1, dayOfWeek: lastDayWeekDay, number: 0, type: 10 });
        }

        for (var i = 0; i < daysList.length; i++) {

            var day = daysList[i];

            var btnBackground = '#ffffff';

            if (day != null) {
                if (day.dayOfWeek == 1) {
                    hbox = Ext.create('Ext.panel.Panel', {
                        bodyStyle: Gkh.bodyStyle,
                        flex: 1,
                        border: false,
                        layout: {
                            type: 'hbox',
                            align: 'stretch'
                        },
                        items: []
                    });
                    index = 1;
                }

                switch (day.type) {
                    case 20:
                        btnBackground = '#efd334';
                        break;
                    case 30:
                        btnBackground = '#e72717';
                        break;
                }

                var element = Ext.create('Ext.panel.Panel', {
                    flex: 1,
                    itemId: day.id < 0 ? day.id * i : day.id,
                    bodyStyle: Gkh.bodyStyle,
                    border: false,
                    padding: '10px',
                    layout: 'fit',
                    items: []
                });

                if (day.id > 0) {
                    debugger;
                    element.insert(1, Ext.create('Ext.button.Button', {
                        style: {
                            background: btnBackground
                        },
                        dayId: day.id,
                        listeners: {
                            click: function(sender) {
                                if (sender.dayId) {
                                    this.onDayButtonClick(sender.dayId);
                                }
                            },
                            scope: this
                        },
                        html: '<span style="position: relative; left: 35%;"><span style="position: relative; top: -60%;"><span style="font-weight:bold;font-size:15px;border: 1px solid blue;padding: 5px;">' + day.id + '</span>'+
                            '<span style="position: relative; left: -45%;"><span style="position: relative; top: 60%;"><span style="font-weight:bold;font-size:20px;" > ' + day.number + '</span>',
                    }));
                }

                if (hbox != null && element != null) {
                    hbox.insert(index, element);
                }

                if (day.dayOfWeek == 7 && hbox != null) {
                    days.insert(hboxIndex, hbox);
                    hboxIndex++;
                }
            }

            index++;
        }
    },

    onChangeMonthPrev: function() {
        this.date.setMonth(this.date.getMonth() - 1);
        this.fillData();
    },

    onChangeMonthNext: function() {
        this.date.setMonth(this.date.getMonth() + 1);
        this.fillData();
    },

    onDayButtonClick: function(id) {
        var me = this;
        if (me.editWindow) {
            var model = me.getModel('Day');
            model.load(id, {
                success: function(rec) {
                    me.editWindow.getForm().loadRecord(rec);
                    me.editWindow.show();
                },
                scope: me
            });
        }
    },

    saveRequestHandler: function() {
        var rec, form = this.editWindow.getForm();
        form.updateRecord();
        rec = form.getRecord();
        if (form.isValid()) {
            this.saveRecord(rec);
        } else {
            var fields = form.getFields();
            var invalidFields = '';

            Ext.each(fields.items, function(field) {
                if (!field.isValid()) {
                    invalidFields += '<br>' + field.fieldLabel;
                }
            });

            Ext.Msg.alert('Ошибка сохранения!', 'Не заполнены обязательные поля: ' + invalidFields);
        }
    },

    saveRecord: function(rec) {
        var me = this;
        var window = me.editWindow;
        var frm = window.getForm();
        window.mask('Сохранение', frm);
        rec.save({ id: rec.getId() })
            .next(function() {
                window.unmask();
                window.close();
                me.fillData();
            }, this)
            .error(function(result) {
                window.unmask();
                window.close();
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            }, this);
    },

    closeWindowHandler: function() {
        this.editWindow.close();
    },

    onSelectDate: function() {
        var view = this.getMainView(),
            panel = view.down("#headerPanel"),
            layout = panel.getLayout(),
            monthField = this.getMonthField(),
            yearField = this.getYearField();

        monthField.setValue(this.date.getMonth() + 1);
        yearField.setValue(this.date.getFullYear());

        layout.setActiveItem(1);
    },

    onAcceptNewDate: function() {
        var view = this.getMainView(),
            panel = view.down("#headerPanel"),
            layout = panel.getLayout(),
            monthField = this.getMonthField(),
            yearField = this.getYearField(),
            month = monthField.getValue() - 1,
            year = yearField.getValue();

        if (month && month >= 0 && month < 12 && year && year > 0) {
            layout.setActiveItem(0);

            this.date = new Date(year, month, 1);

            this.fillData();
        } else {
            Ext.Msg.alert('Ошибка!', 'Выбран неверный месяц и/или год');
        }
    },

    onCancelDateChange: function() {
        var view = this.getMainView(),
            panel = view.down("#headerPanel"),
            layout = panel.getLayout();

        layout.setActiveItem(0);
    }
});