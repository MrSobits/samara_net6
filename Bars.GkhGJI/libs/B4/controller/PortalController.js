Ext.define('B4.controller.PortalController', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GjiDocumentRegister',
        'B4.aspects.permission.GkhFormPermissionAspect',
        'B4.mixins.LayoutControllerLoader',
        'B4.mixins.MaskBody',
        'B4.mixins.Context'
    ],

    refs: [
        {
            ref: 'mainMenu',
            selector: '#mainMenu'
        },
        {
            ref: 'b4TabPanel',
            selector: '#contentPanel'
        }
    ],

    views: [
        'Portal',
        'instructions.Window'
    ],

    stores: [
        'MenuItemStore',
        'SearchIndex',
        'desktop.ActiveOperator',
        'desktop.TaskState',
        'desktop.TaskControl',
        'desktop.ReminderWidget',
        'News'
    ],

    mixins: {
        controllerLoader: 'B4.mixins.LayoutControllerLoader',
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    models: [
        'InspectionGji',
        'Reminder',
        'AppealCits'
    ],

    route: 'portal',

    containerSelector: '#contentPanel',

    allowLoadMask: false,
    deployViewKeys: {
        'default': 'defaultDeploy'
    },

    /**
     * @private
     * Метод по умолчанию для добавления компонент на рабочий стол.
     * Добавляет компоненту в виде новой вкладки.
     */
    defaultDeploy: function(controller, view, data) {
        var container = this.getB4TabPanel(),
            viewSelector = '#' + view.getId();

        if (!container.down(viewSelector)) {
            container.add(view);
        }
        container.setActiveTab(view);
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Widget.Faq',
                    applyTo: '[wtype=faq]',
                    selector: 'portalpanel',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.News',
                    applyTo: '[wtype=newsportlet]',
                    selector: 'portalpanel',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.ActiveOperator',
                    applyTo: '[wtype=activeOperator]',
                    selector: 'portalpanel',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.TaskControl',
                    applyTo: '[wtype=taskControl]',
                    selector: 'portalpanel',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.TaskState',
                    applyTo: '[wtype=taskState]',
                    selector: 'portalpanel',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Widget.TaskTable',
                    applyTo: '[wtype=taskTable]',
                    selector: 'portalpanel',
                    applyBy: function(component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'B4.Security.AccessRights', applyTo: '#permissionButton', selector: 'desktoptopbar',
                    applyBy: function (component, allowed) {
                        if (component) {
                            if (allowed) {
                                component.show();
                            } else {
                                component.hide();
                            }
                        }
                    }
                }
            ]
        },
        {
            xtype: 'formpermissionaspect',
            buttonSelector: '#permissionButton'
        }
    ],

    init: function () {
        debugger;
        var actions = {
            // Временное решение для работы старой версии routing
            'b4portal': {
                afterrender: function() {
                    var token = Ext.History.getToken();
                    if (token && token.indexOf("B4.controller") == 0) {
                        Ext.History.add('');
                    }
                }
            },
            '#contentPanel': {
                tabchange: function(tabPanel, newPanel, oldPanel, eOpts) {
                    if (!newPanel.ctxKey) {
                        Ext.History.add(newPanel.token || '');
                    }
                }
            },
            // Конец - Временное решение для работы старой версии routing

            'menufirstlevelitem': {
                tap: this.onMenuItemClick
            },

            'iconablemenu': {
                itemtap: this.onMenuItemClick
            },

            '#searchList': {
                itemtap: this.onMenuItemClick
            },

            '#profileBtn': {
                click: this.onProfile
            },

            '#logoutBtn': {
                click: this.onLogout
            },

            '#logoutBt': {
                click: this.onLogout
            },

            '#allNewsBtn': {
                click: this.onAllNews
            },

            '#col-2 #allInspectionsBtn': {
                click: this.onAllDisposal
            },

            '#col-3 #allInspectionsBtn': {
                click: this.onAllPrescription
            },

            '#helpBtn': {
                click: this.onHelpBtnClick
            },

            '#goToInstructionsBtn': {
                click: this.goToInstructionsBtnClick
            },

            '#taskTable #taskTableDataView': {
                itemclick: this.openReminderEditCard
            },

            '#taskTable #allTasksBtn': {
                click: this.onAllRemindersForInspector
            },

            '#taskTable #refreshBtn': {
                click: this.onRefreshTableTask
            },

            '#taskControl #allTasksBtn': {
                click: this.onAllRemindersForHead
            },

            '#taskControl #refreshBtn': {
                click: this.onRefreshControlTasks
            },

            '#taskState #allTasksBtn': {
                click: this.onAllRemindersForInspector
            },

            '#taskState #refreshBtn': {
                click: this.onRefreshStateTasks
            },

            '#taskControl': {
                openregistrtaskcontrol: this.openRegistrTaskControl
            },
            '#taskState': {
                openregistrtaskstate: this.openRegistrTaskState
            }
        };

        this.control(actions);

        this.callParent(arguments);
    },

    onLaunch: function() {
        // jQuery
        debugger;
        Ext.Loader.loadScript(rootUrl + 'libs/jQuery/jquery-1.9.1.min.js');
      //  Gkh.signalR.start();
        var viewPortal = this.getView('Portal');
        if (viewPortal)
            viewPortal.create();
    },

    onMenuItemClick: function(record, icmenu) {
        var moduleScript = record.get('moduleScript');

        if (moduleScript.indexOf('B4.controller') > -1) {
            if (Ext.History.getToken() == moduleScript) {
                return;
            }
            this.loadController(record.get('moduleScript'), null, null, function() { icmenu.enableItem(record.id); });
        } else {
            Ext.History.add(moduleScript);
        }
    },

    selectHomeView: function() {
        var container = Ext.ComponentQuery.query(this.containerSelector)[0];
        container.setActiveTab(0);
    },

    onRefreshTableTask: function(btn) {
        var widget = Ext.ComponentQuery.query('#taskTableDataView')[0];
        if (widget) {
            widget.getStore().load({ limit: 4 });
        }
    },

    onRefreshStateTasks: function(btn) {
        var widget = Ext.ComponentQuery.query('#taskState')[0];
        if (widget) {
            widget.store.load();
        }
    },

    onRefreshControlTasks: function(btn) {
        var widget = Ext.ComponentQuery.query('#taskControl')[0];
        if (widget) {
            widget.store.load();
        }
    },

    onLogout: function() {
     //   Gkh.signalR.stop();
        window.location = B4.Url.action('LogOut', 'Login');
    },

    onProfile: function() {
        Ext.History.add('profilesettingadministration');
        //this.loadController('B4.controller.administration.ProfileSetting');
    },

    onAllNews: function() {
        this.loadController('B4.controller.Public.News');
    },

    onAllRemindersForInspector: function() {
        Ext.History.add('reminderInspector');
    },

    onAllRemindersForHead: function() {
        Ext.History.add('reminderHead');
    },

    onHelpBtnClick: function() {
        var url = B4.Url.action('/Instruction/GetMainInstruction');
        window.open(url, '_blank');
    },

    goToInstructionsBtnClick: function() {
        var me = this;
        me.getB4TabPanel();
        if (!me.helpWindow) {
            me.helpWindow = Ext.create('B4.view.instructions.Window', {
                renderTo: B4.getBody().getEl()
            });
        }
        this.helpWindow.show();
    },

    openReminderEditCard: function(dw, rec) {
        var me = this,
            id = rec.get('Id'),
            model = this.getModel('Reminder'),
            typeReminder,
            controllerEditName,
            params = {},
            modelAppealCits,
            appealCits,
            documentGji,
            inspection,
            modelInspectionGji,
            defaultParams;

        model.load(id, {
            success: function(record) {
                typeReminder = record.get('TypeReminder');

                if (typeReminder === 10) {

                    controllerEditName = 'B4.controller.AppealCits';

                    params.appealId = record.get('AppealCits').Id;
                    me.loadController(controllerEditName, params);

                    //ToDo Пока невозможно перевести реестр обращения на роуты
                    //Ext.History.add('appealcits/' + record.get('AppealCits').Id);
                } else {
                    documentGji = record.get('DocumentGji');
                    inspection = record.get('InspectionGji');
                    modelInspectionGji = this.getModel('InspectionGji');

                    controllerEditName = this.getControllerName(inspection.TypeBase);
                    params = new modelInspectionGji({ Id: record.get('InspectionGji').Id });

                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji) {
                        defaultParams = this.getDefaultParams(documentGji.TypeDocumentGji);
                        params.defaultController = defaultParams.controllerName;
                        params.defaultParams = {
                            inspectionId: inspection.Id,
                            documentId: documentGji.Id,
                            title: defaultParams.docName
                        };
                    }

                    if (controllerEditName) {
                        me.loadController(controllerEditName, params);
                    }
                }
            },
            scope: this
        });
    },

    openRegistrTaskControl: function(colorType, inspectorId) {
        Ext.History.add('reminderTaskControl/' + colorType + '/' + inspectorId);
    },

    openRegistrTaskState: function(colorType, typeReminder) {
        Ext.History.add('reminderTaskState/' + colorType + '/' + typeReminder);
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