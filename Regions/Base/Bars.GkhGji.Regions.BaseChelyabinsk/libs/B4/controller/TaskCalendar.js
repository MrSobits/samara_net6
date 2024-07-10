Ext.define('B4.controller.TaskCalendar', {
    extend: 'B4.base.Controller',
    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'TaskCalendar',
        'DocumentGji',
        'ProtocolGji',
        'courtpractice.CourtPractice',
        'Disposal',
        'ResolPros'
    ],
    stores: [
        'B4.store.Month',
        'taskcalendar.ListDisposals',
        'taskcalendar.ListProtocols',
        'taskcalendar.ListCourt',
        'taskcalendar.ListResolPros'
    ],
    views: [
        'TaskCalendar',
        'calendarday.EditWindow',
        'taskcalendar.TaskWindow',
        'taskcalendar.ProtocolGrid',
        'taskcalendar.CourtPracticeGrid',
        'taskcalendar.ResolProsGrid'
    ],

    mainView: 'TaskCalendar',
    mainViewSelector: 'taskcalendar',

    refs: [
        {
            ref: 'mainView',
            selector: 'taskcalendar'
        },
        {
            ref: 'monthLabel',
            selector: 'taskcalendar #monthLabel'
        },
        {
            ref: 'yearLabel',
            selector: 'taskcalendar #yearLabel'
        },
        {
            ref: 'daysPanel',
            selector: 'taskcalendar #vboxpanel'
        },
        {
            ref: 'monthField',
            selector: 'taskcalendar #monthField'
        },
        {
            ref: 'yearField',
            selector: 'taskcalendar #yearField'
        },
        {
            ref: 'headerPanel',
            selector: 'taskcalendar #headerPanel'
        }
    ],

    aspects: [],

    init: function() {
        var me = this;

        var actions = {};

        actions[this.mainViewSelector + ' #prevBtn'] = { 'click': { fn: this.onChangeMonthPrev, scope: this } };
        actions[this.mainViewSelector + ' #nextBtn'] = { 'click': { fn: this.onChangeMonthNext, scope: this } };
      //  actions['dayeditwindow b4savebutton'] = { 'click': { fn: this.saveRequestHandler, scope: this } };
        actions['taskcalendarEditWindow b4closebutton'] = { 'click': { fn: this.closeWindowHandler, scope: this } };
        actions[this.mainViewSelector + ' [action="SelectDate"]'] = { 'click': { fn: this.onSelectDate, scope: this } };
        actions[this.mainViewSelector + ' [action="AcceptDate"]'] = { 'click': { fn: this.onAcceptNewDate, scope: this } };
        actions[this.mainViewSelector + ' [action="CancelDateChange"]'] = { 'click': { fn: this.onCancelDateChange, scope: this } };
        actions['taskcalendardisposalgrid'] = { 'rowaction': { fn: this.rowAction, scope: this } };
        actions['taskcalendarprotocolgrid'] = { 'rowaction': { fn: this.rowActionProt, scope: this } };
        actions['taskcalendarcourtpracticeGrid'] = { 'rowaction': { fn: this.goToCourtPractice, scope: this } };
        actions['taskcalendarresolprosGrid'] = { 'rowaction': { fn: this.goToResolPros, scope: this } };
        actions['taskcalendarresolprosGrid b4updatebutton'] = { 'click': { fn: this.onReload, scope: this } };
        actions['taskcalendardisposalgrid b4updatebutton'] = { 'click': { fn: this.onReload, scope: this } };
        actions['taskcalendarprotocolgrid b4updatebutton'] = { 'click': { fn: this.onReload, scope: this } };
        actions['taskcalendarcourtpracticeGrid b4updatebutton'] = { 'click': { fn: this.onReload, scope: this } };

        me.control(actions);

        this.date = new Date();

        me.callParent(arguments);
    },

    onReload: function (btn) {
        
        var grid = btn.up('grid');
        var gridstore = grid.getStore();
        gridstore.load();
    },

    goToResolPros: function (grid, action, record) {
        
        var me = this,
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {};
        controllerEditName = 'B4.controller.resolpros.Edit';
        params.documentId = record.get('Id');
        if (controllerEditName) {
            portal.loadController(controllerEditName, params);
        }
    },

    goToCourtPractice: function (grid, action, record) {
        
        var me = this,
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {};
        controllerEditName = 'B4.controller.CourtPractice';
        params.recId = record.get('Id');
        if (controllerEditName) {
            portal.loadController(controllerEditName, params);
        }
    },

    rowActionProt: function (grid, action, record) {
        
        var me = this,
            typeReminder = 10,
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {},
            model,
            documentGji,
            inspection,
            defaultParams;

        switch (typeReminder) {

            default:
                {
                    documentGji = record.get('Id');
                    inspection = record.get('InspectionId');
                    model = me.getModel('InspectionGji');
                    var typeBase = record.get('TypeBase');
                    var TypeDocumentGji = record.get('TypeDocumentGji');
                    controllerEditName = me.getControllerName(typeBase);
                    params = new model({ Id: inspection });

                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji) {
                        defaultParams = me.getDefaultParams(TypeDocumentGji);
                        params.defaultController = defaultParams.controllerName;
                        params.defaultParams = {
                            inspectionId: inspection,
                            documentId: documentGji,
                            title: defaultParams.docName
                        };
                    }
                    if (controllerEditName) {
                        portal.loadController(controllerEditName, params);
                    }
                }
                break;
        }

    },

    rowAction: function (grid, action, record) {
        var me = this,
            typeReminder = 10,
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {},
            model,
            documentGji,
            inspection,
            defaultParams;

        switch (typeReminder) {

            default:
                {
                    documentGji = record.get('Id');
                    inspection = record.get('InspectionId');
                    model = me.getModel('InspectionGji');
                    var typeBase = record.get('TypeBase');
                    controllerEditName = me.getControllerName(typeBase);
                    params = new model({ Id: inspection});

                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji) {
                        defaultParams = me.getDefaultParams(10);
                        params.defaultController = defaultParams.controllerName;
                        params.defaultParams = {
                            inspectionId: inspection,
                            documentId: documentGji,
                            title: defaultParams.docName
                        };
                    }
                    if (controllerEditName) {
                        portal.loadController(controllerEditName, params);
                    }
                }
                break;
        }

    },

    getControllerName: function (typeBase) {
        switch (typeBase) {
            //Инспекционная проверка
            case 10:
                return 'B4.controller.baseinscheck.Navigation';
            //Обращение граждан                  
            case 20:
                return 'B4.controller.basestatement.Navigation';
            //Плановая проверка юр лиц                
            case 30:
                return 'B4.controller.basejurperson.Navigation';
            //Распоряжение руководства               
            case 40:
                return 'B4.controller.basedisphead.Navigation';
            //Требование прокуратуры                 
            case 50:
                return 'B4.controller.baseprosclaim.Navigation';
            //Постановление прокуратуры                  
            case 60:
                return 'B4.controller.resolpros.Navigation';
            //Проверка деятельности ТСЖ                   
            case 70:
                return 'B4.controller.baseactivitytsj.Navigation';
            //Отопительный сезон                    
            case 80:
                return 'B4.controller.baseheatseason.Navigation';
            //Административное дело
            case 90:
                return '';
            //Протокол МВД
            case 100:
                return 'B4.controller.protocolmvd.Navigation';
            //Проверка по плану мероприятий
            case 110:
                return 'B4.controller.baseplanaction.Navigation';
            //Протокол МЖК
            case 120:
                return 'B4.controller.protocolmhc.Navigation';
            case 140:
                return 'B4.controller.protocol197.Navigation';
            //Без основания                     
            case 150:
                return 'B4.controller.basedefault.Navigation';
        }

        return '';
    },

    getDefaultParams: function (typeDocument) {
        var result = {};
        switch (typeDocument) {
            //Распоряжение
            case 10:
                {
                    result.controllerName = 'B4.controller.Disposal';
                    result.docName = B4.DisposalTextValues.getSubjectiveCase();
                }
                break;
            //Предписание
            case 50:
                {
                    result.controllerName = 'B4.controller.Prescription';
                    result.docName = 'Предписание';
                }
                break;
            //Постановление
            case 70:
                {
                    result.controllerName = 'B4.controller.Resolution';
                    result.docName = 'Постановление';
                }
                break;

            //Постановление
            case 140:
                {
                    result.controllerName = 'B4.controller.protocol197.Edit';
                    result.docName = 'Протокол 19.7';
                }
                break;
            case 60:
                {
                    result.controllerName = 'B4.controller.ProtocolGji';
                    result.docName = 'Протокол';
                }
                break;
        }

        return result;
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('taskcalendar');
        me.bindContext(view);
        me.application.deployView(view);

        me.editWindow = Ext.create('widget.taskcalendarEditWindow');

        me.fillData();
    },

    fillData: function() {
        var me = this;
        me.mask('Загрузка...');
        B4.Ajax.request(B4.Url.action('GetDaysList', 'TaskCalendar', { date: this.date }
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
                        btnBackground = '#b13dff';
                        break;
                    case 30:
                        btnBackground = '#e35252';
                        break;
                    case 40:
                        btnBackground = '#8cff8e';
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
                    element.insert(1, Ext.create('Ext.button.Button', {
                        style: {
                            background: btnBackground
                        },
                        layout: {
                            align: 'stretch'
                        },
                        dayId: day.id,
                        daynumber: day.number,
                        daytasks: day.taskCount,
                        listeners: {
                            mouseover: function (btn) {
                                btn.btnEl.dom.innerHTML = '<span class="spanCalendar3">' + btn.daytasks + '</span>' +
                                    '<span style = "position: absolute; left: 5px; bottom: 4px" ><span style="font-family: serif;font-weight:bold;font-size:25px;font-colour:blue" >' + btn.daynumber + '</span>';
                         //       btn.btnEl.dom.innerHTML = '<span class="spanCalendar3">' + 'Нажали' + '</span>';
                            },
                            mouseout: function (btn) {
                                btn.btnEl.dom.innerHTML = '<span class="spanCalendar4">' + btn.daytasks + '</span>' +
                                    '<span style = "position: absolute; left: 5px; bottom: 4px" ><span style="font-family: serif;font-weight:bold;font-size:25px;font-colour:blue" >' + btn.daynumber + '</span>';
                                //       btn.btnEl.dom.innerHTML = '<span class="spanCalendar3">' + 'Нажали' + '</span>';
                            },
                            click: function(sender) {
                                if (sender.dayId) {
                                    this.onDayButtonClick(sender.dayId);
                                }
                            },
                            scope: this
                        },
                        html: '<span class="spanCalendar2">' + day.taskCount + '</span>'+
                        '<span style = "position: absolute; left: 5px; bottom: 1px" ><span style="font-family: serif;font-weight:bold;font-size:25px;font-colour:blue" >' + day.number + '</span>',
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
            
            me.editWindow.show();
            var width = Ext.getBody().getViewSize().width;
            me.editWindow.setSize('80%', '60%');
            width = (width / 100) * 80;
            width = Math.round(width);
            me.editWindow.setWidth(width);
            me.editWindow.center();
            
         
            var dispgrid = me.editWindow.down('taskcalendardisposalgrid');
            var dispstore = dispgrid.getStore();
            dispstore.removeAll();
            dispstore.on('beforeload',
                function (store, operation) {
                    operation.params.dayId = id;
                },
                me);
            dispstore.load();

            var protgrid = me.editWindow.down('taskcalendarprotocolgrid');
            var protstore = protgrid.getStore();
            protstore.removeAll();
            protstore.on('beforeload',
                function (store, operation) {
                    operation.params.dayId = id;
                },
                me);
            protstore.load();

            var cpgrid = me.editWindow.down('taskcalendarcourtpracticeGrid');
            var cpstore = cpgrid.getStore();
            cpstore.removeAll();
            cpstore.on('beforeload',
                function (store, operation) {
                    operation.params.dayId = id;
                },
                me);
            cpstore.load();

            var rpgrid = me.editWindow.down('taskcalendarresolprosGrid');
            var rpstore = rpgrid.getStore();
            rpstore.removeAll();
            rpstore.on('beforeload',
                function (store, operation) {
                    operation.params.dayId = id;
                },
                me);
            rpstore.load();
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

    closeWindowHandler: function (btn) {
        var window = btn.up('taskcalendarEditWindow');
        window.close();
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

    onAcceptNewDate: function () {
        
        var view = this.getMainView(),
            panel = view.down("#headerPanel"),
            layout = panel.getLayout(),
            monthField = this.getMonthField(),
            yearField = this.getYearField(),
            month = monthField.getValue() - 1,
            year = yearField.getValue();

        if (month +1 > 0 && month < 12 && year && year > 0) {
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