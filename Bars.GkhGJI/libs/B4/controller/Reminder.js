Ext.define('B4.controller.Reminder', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.view.reminder.Panel',
        'B4.aspects.GkhCtxButtonDataExport',
        'B4.controller.AppealCits'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    models: ['Reminder', 'AppealCits', 'InspectionGji', 'dict.Inspector'],
    stores: ['reminder.InspectorGji', 'reminder.HeadGji'],

    views: ['reminder.InspectorGrid', 'reminder.HeadGrid'],
    
    currentParams: null,

    reminderPanelSelector: 'reminderPanel',
    
    refs: [
        {
            ref: 'InspectorPanel',
            selector: 'reminderPanel[typeReminder = "inspector"]'
        },
        {
            ref: 'HeadPanel',
            selector: 'reminderPanel[typeReminder = "head"][subType = "allTask"]'
        },
        {
            ref: 'TaskControlPanel',
            selector: 'reminderPanel[typeReminder = "head"][subType = "taskControl"]'
        },
        {
            ref: 'TaskStatePanel',
            selector: 'reminderPanel[typeReminder = "head"][subType = "taskState"]'
        }
    ],
    
    aspects: [
        {
            xtype: 'gkhctxbuttondataexportaspect',
            name: 'reminderInspectorButtonExportAspect',
            gridSelector: '#reminderInspectorGjiGrid',
            buttonSelector: '#reminderInspectorGjiGrid #btnExport',
            controllerName: 'Reminder',
            actionName: 'ExportReminderOfInspector'
        },
        {
            xtype: 'gkhctxbuttondataexportaspect',
            name: 'reminderHeadButtonExportAspect',
            gridSelector: '#reminderHeadGjiGrid',
            buttonSelector: '#reminderHeadGjiGrid #btnExport',
            controllerName: 'Reminder',
            actionName: 'ExportReminderOfHead'
        }
    ],
    
    init: function () {
        var me = this,
            actions = {};

        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #btnReminderRefresh'] = { 'click': { fn: me.onRefresh, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #dfDateStart'] = { 'change': { fn: me.onChangeDateStart, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #dfDateEnd'] = { 'change': { fn: me.onChangeDateEnd, scope: me } };


        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #cbStatement'] = { 'change': { fn: me.onChangeStatement, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #cbDisposal'] = { 'change': { fn: me.onChangeDisposal, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #cbPrescription'] = { 'change': { fn: me.onChangePrescription, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl #cbBaseInspection'] = { 'change': { fn: me.onChangeBaseInspection, scope: me } };

        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl button[actionName="AllTask"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl button[actionName="ComeUpToTerm"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl button[actionName="Expired"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "head"] reminderfilterpnl button[actionName="Unformed"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };

        actions['reminderPanel[typeReminder = "head"] #reminderHeadGjiGrid'] = { 'rowaction': { fn: me.rowAction, scope: me } };

        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #btnReminderRefresh'] = { 'click': { fn: me.onRefresh, scope: me } };

        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #dfDateStart'] = { 'change': { fn: me.onChangeDateStart, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #dfDateEnd'] = { 'change': { fn: me.onChangeDateEnd, scope: me } };

        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #cbStatement'] = { 'change': { fn: me.onChangeStatement, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #cbDisposal'] = { 'change': { fn: me.onChangeDisposal, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #cbPrescription'] = { 'change': { fn: me.onChangePrescription, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl #cbBaseInspection'] = { 'change': { fn: me.onChangeBaseInspection, scope: me } };

        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl button[actionName="AllTask"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl button[actionName="ComeUpToTerm"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl button[actionName="Expired"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] reminderfilterpnl button[actionName="Unformed"]'] = { 'click': { fn: me.onChangeDopFilter, scope: me } };
        actions['reminderPanel[typeReminder = "inspector"] #reminderInspectorGjiGrid'] = { 'rowaction': { fn: me.rowAction, scope: me } };

        me.control(actions);

        me.callParent(arguments);
    },
    
    /*
     * Метод для Панели инспектора - роуты
     */
    reminderInspector: function () {
        var me = this,
            pnl = me.getInspectorPanel(),
            panel,
            grid,
            store;

        if (!pnl) {
            pnl = Ext.widget('reminderPanel',
            {
                title: 'Доска задач Инспектора',
                typeReminder: 'inspector'
            });

            panel = pnl.down('#reminderCenterPanel');
            grid = Ext.widget('reminderInspectorGjiGrid');
            panel.removeAll();
            panel.add(grid);
            pnl.down('breadcrumbs').update({ text: 'Список всех задач' });
            
            me.bindContext(pnl);
            me.application.deployView(pnl);
            
            /*
             * Очищаю фильтры потому как если сначала отфильтровать по колонке,
             * а затем перейти в такуюже карточку то какбуто бы окно новое а фильтры у грида сохраняются 
             */
            grid.clearHeaderFilters();
            
            pnl.params = {};
            pnl.params.isStatement = true;
            pnl.params.isDisposal = true;
            pnl.params.isPrescription = true;
            pnl.params.isBaseInspection = true;
            pnl.params.dopFilter = 10;
            
            store = grid.getStore();
            
            store.on('beforeload', me.onBeforeLoad, me);

            me.fillDopFilter(pnl);
        }

        this.currentParams = pnl.params;
        
        if (!store) {
            grid = pnl.down('reminderInspectorGjiGrid');
            store = grid.getStore();
        }
        
        store.load();
    },
    
    /*
     * метод для панели Руководителя
     */ 
    reminderHead: function () {
        var me = this,
            pnl = me.getHeadPanel(),
            store,
            panel,
            grid;
        
        if (!pnl) {
            // создаем панель с типом head и подТипом = allTask
            pnl = Ext.widget('reminderPanel',
                    {
                        title: 'Панель руководителя',
                        typeReminder: 'head',
                        subType: 'allTask'
                    });
            
            panel = pnl.down('#reminderCenterPanel');
            panel.removeAll();
            pnl.down('breadcrumbs').update({ text: 'Список всех задач' });

            grid = Ext.widget('reminderHeadGjiGrid');
            panel.add(grid);
            
            me.bindContext(pnl);
            me.application.deployView(pnl);
            
            pnl.params = {};
            pnl.params.isStatement = true;
            pnl.params.isDisposal = true;
            pnl.params.isPrescription = true;
            pnl.params.isBaseInspection = true;

            pnl.params.dopFilter = 10;

            //Очищаю фильтры потому как если сначала отфильтроват ьпо колонке
            //а затем перейти в такуюже карточку то какбуто бы окно новое а фильтры у грида сохраняются 
            grid.clearHeaderFilters();
            
            store = grid.getStore();
            
            store.on('beforeload', me.onBeforeLoad, me);
            
            me.fillDopFilter(pnl);
        }

        me.currentParams = pnl.params;
        
        if (!store) {
            grid = pnl.down('reminderHeadGjiGrid');
            store = grid.getStore();
        }
        
        store.load();
    },
    
    // метод роута Контроль задач Руководителя (Суда попадаем из виджета Контроль задач)
    reminderTaskControl: function ( colorType, inspectorId) {
        var me = this,
            pnl = me.getTaskControlPanel(),
            store,
            panel,
            grid;

        if (!pnl) {
            pnl = Ext.widget('reminderPanel',
            {
                title: 'Контроль задач',
                typeReminder: 'head',
                subType: 'taskControl'
            });
            
            me.getModel('dict.Inspector').load(inspectorId, {
                success: function (rec) {
                    pnl.down('breadcrumbs').update({ text: 'Контроль задач по инспектору: ' + rec.get('Fio') });
                },
                scope: me
            });
            panel = pnl.down('#reminderCenterPanel');
            panel.removeAll();

            pnl.down('#reminderWestPanel').hide();

            grid = Ext.widget('reminderHeadGjiGrid');
            panel.add(grid);

            me.bindContext(pnl);
            me.application.deployView(pnl);
            
            pnl.params = {};
            pnl.params.isStatement = true;
            pnl.params.isDisposal = true;
            pnl.params.isPrescription = true;
            pnl.params.isBaseInspection = true;

            pnl.params.inspectorId = inspectorId;

            if (colorType !== 'all')
                pnl.params.colorType = colorType;

            pnl.params.dopFilter = 10;

            //Очищаю фильтры потому как если сначала отфильтроват ьпо колонке
            //а затем перейти в такуюже карточку то какбуто бы окно новое а фильтры у грида сохраняются 
            grid.clearHeaderFilters();
            
            store = grid.getStore();
            
            store.on('beforeload', me.onBeforeLoad, me);
            
            me.fillDopFilter(pnl);
        }
        
        me.currentParams = pnl.params;
        
        if (!store) {
            grid = pnl.down('reminderHeadGjiGrid');
            store = grid.getStore();
        }
        store.load();
    },
    
    // метод роута Состояние задач руководителя (суда попадаем из виджета состояние задач)
    reminderTaskState: function (colorType, typeReminder) {
        var me = this,
            pnl = me.getTaskStatePanel(),
            store,
            panel,
            grid,
            type = parseInt(typeReminder, 0),
            lableText;

        if (!pnl) {
            pnl = Ext.widget('reminderPanel',
               {
                   title: 'Состояние задач',
                   typeReminder: 'head',
                   subType: 'taskState'
               });
            
            lableText = 'Состояние по всем задачам';
            switch (type) {
                case 30: {
                    lableText = 'Состояние по задачам с типом \"Распоряжения\"';
                }
                    break;

                case 40: {
                    lableText = 'Состояние по задачам с типом \"Предписания\"';
                }
                    break;

                case 10: {
                    lableText = 'Состояние по задачам с типом \"Обращения граждан\"';
                }
                    break;
            }
            
            pnl.down('breadcrumbs').update({ text: lableText });

            panel = pnl.down('#reminderCenterPanel');
            panel.removeAll();

            pnl.down('#reminderWestPanel').hide();

            grid = Ext.widget('reminderHeadGjiGrid');
            panel.add(grid);

            me.bindContext(pnl);
            me.application.deployView(pnl);
            
            pnl.params = {};

            pnl.params.isStatement = true;
            pnl.params.isDisposal = true;
            pnl.params.isPrescription = true;
            pnl.params.isBaseInspection = true;
            pnl.params.colorType = colorType;

            if (type > 0)
                pnl.params.typeReminder = type;

            pnl.params.isTypeReminder = type > 0 ? true : false;
            pnl.params.dopFilter = 10;

            //Очищаю фильтры потому как если сначала отфильтроват ьпо колонке
            //а затем перейти в такуюже карточку то какбуто бы окно новое а фильтры у грида сохраняются 
            grid.clearHeaderFilters();
            
            store = grid.getStore();
            
            store.on('beforeload', me.onBeforeLoad, me);
            
            me.fillDopFilter(pnl);
        }

        me.currentParams = pnl.params;
        
        if (!store) {
            grid = pnl.down('reminderHeadGjiGrid');

            store = grid.getStore();
        }
        
        store.load();
    },

    fillDopFilter: function (pnl) {
        var me = this,
            btnAllTask = pnl.down('button[actionName="AllTask"]'),
            btnComeUpToTerm = pnl.down('button[actionName="ComeUpToTerm"]'),
            btnExpired = pnl.down('button[actionName="Expired"]'),
            btnUnformed = pnl.down('button[actionName="Unformed"]'),
            filterpnl = pnl.down('reminderfilterpnl');

        me.mask('Получение данных', filterpnl);
        
        B4.Ajax.request(B4.Url.action('GetInfo', 'Reminder', {
            
            dateStart: pnl.params.dateStart,
            dateEnd: pnl.params.dateEnd,
            isStatement: pnl.params.isStatement,
            isDisposal: pnl.params.isDisposal,
            isPrescription: pnl.params.isPrescription,
            isBaseInspection: pnl.params.isBaseInspection,
            isHead: pnl.typeReminder === 'head'
            
        })).next(function (response) {
            
            var obj = Ext.JSON.decode(response.responseText);
            btnAllTask.setText('Всего задач: ' + obj.AllTask);
            btnComeUpToTerm.setText('Подходящие к сроку: ' + obj.ComeUpToTerm);
            btnExpired.setText('Просроченные: '+ obj.Expired);
            btnUnformed.setText('Не сформировано: ' + obj.Unformed);
            me.unmask();
            
        }).error(function (msg) {
            
            btnAllTask.setText('Всего задач:');
            btnComeUpToTerm.setText('Подходящие к сроку:');
            btnExpired.setText('Просроченные:');
            btnUnformed.setText('Не сформировано:');
            me.unmask();
            
        });
    },
    
    onRefresh: function(btn) {
        var me = this,
            pnl = btn.up('reminderPanel'),
            grid = pnl.down(pnl.typeReminder === 'head' ? 'reminderHeadGjiGrid' : 'reminderInspectorGjiGrid'),
            store = grid.getStore();

        me.fillDopFilter(pnl);
        store.currentPage = 1;
        store.load();
    },
    
    onChangeDateStart: function(field, newValue) {
        field.up('reminderPanel').params.dateStart = newValue;
    },
    
    onChangeDateEnd: function(field, newValue) {
        field.up('reminderPanel').params.dateEnd = newValue;
    },
    
    onChangeStatement: function(field, newValue) {
        field.up('reminderPanel').params.isStatement = newValue;
    },
    
    onChangeDisposal: function(field, newValue) {
        field.up('reminderPanel').params.isDisposal = newValue;
    },
    
    onChangePrescription: function(field, newValue) {
        field.up('reminderPanel').params.isPrescription = newValue;
    },
    
    onChangeBaseInspection: function(field, newValue) {
        field.up('reminderPanel').params.isBaseInspection = newValue;
    },
    
    onChangeDopFilter: function(btn) {
        var pnl = btn.up('reminderPanel'),
            grid = pnl.down(pnl.typeReminder === 'head' ? 'reminderHeadGjiGrid' : 'reminderInspectorGjiGrid'),
            store = grid.getStore();

        pnl.params.dopFilter = btn.filterValue;
        store.currentPage = 1;
        store.load();
    },
    
    onBeforeLoad: function (store, operation) {
        var me = this,
            params = me.currentParams;
        
        if (params) {
            operation.params.dateStart = params.dateStart;
            operation.params.dateEnd = params.dateEnd;

            operation.params.isStatement = params.isStatement;
            operation.params.isDisposal = params.isDisposal;
            operation.params.isPrescription = params.isPrescription;
            operation.params.isBaseInspection = params.isBaseInspection;

            operation.params.inspectorId = params.inspectorId;
            operation.params.typeReminder = params.typeReminder;
            operation.params.isTypeReminder = params.isTypeReminder;
            operation.params.colorType = params.colorType;

            operation.params.dopFilter = params.dopFilter;
        }
    },
    
    rowAction: function(grid, action, record) {
        var me = this,
            typeReminder = record.get('TypeReminder'),
            portal = me.getController('PortalController'),
            controllerEditName,
            params = {},
            model,
            documentGji,
            inspection,
            appealCits,
            defaultParams;

        switch (typeReminder) {
            case 10:
                {
                    
                    controllerEditName = 'B4.controller.AppealCits';

                    params.appealId = record.get('AppealCits');
                    portal.loadController(controllerEditName, params);
                    
                    //ToDo Пока невозможно перевести реестр обращения на роуты
                    //Ext.History.add('appealcits/' + record.get('AppealCits'));
                }
                break;
            case 20:
                {
                    
                    inspection = record.get('InspectionGji');
                    model = me.getModel('InspectionGji');

                    controllerEditName = me.getControllerName(inspection.TypeBase);
                    params = new model({ Id: inspection.Id });

                    if (controllerEditName) {
                        portal.loadController(controllerEditName, params);
                    }
                }
                break;
            
            default:
                {
                    documentGji = record.get('DocumentGji');
                    inspection = record.get('InspectionGji');
                    model = me.getModel('InspectionGji');

                    controllerEditName = me.getControllerName(inspection.TypeBase);
                    params = new model({ Id: inspection.Id });

                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji.Id) {
                        defaultParams = me.getDefaultParams(documentGji.TypeDocumentGji);
                        params.defaultController = defaultParams.controllerName;
                        params.defaultParams = {
                            inspectionId: inspection.Id,
                            documentId: documentGji.Id,
                            title: defaultParams.docName
                        };
                    }

                    if (controllerEditName) {
                        portal.loadController(controllerEditName, params);
                    }
                }
                break;
                ;
        }

    },

    getControllerName: function(typeBase) {
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
            //Без основания                     
            case 150:
                return 'B4.controller.basedefault.Navigation';
        }

        return '';
    },

    getDefaultParams: function(typeDocument) {
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
        }

        return result;
    }
});