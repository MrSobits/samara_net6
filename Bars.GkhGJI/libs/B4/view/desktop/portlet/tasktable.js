Ext.define('B4.view.desktop.portlet.TaskTable', {
    extend: 'B4.view.desktop.portal.Portlet',
    alias: 'widget.tasktable',
    ui: 'b4portlet',
    iconCls: 'wic-eye',
    requires: [
        'B4.enums.TypeReminder',
        'B4.enums.CategoryReminder',
        'B4.enums.YesNoNotSet',
        'B4.GjiTextValuesOverride'
    ],
    layout: { type: 'vbox', align: 'stretch' },
    collapsible: false,
    closable: false,
    header: true,
    footer: true,
    isBuilt: false,
    title: 'Доска задач',
    store: 'desktop.ReminderWidget',
    column: 1,

    permissions: [
        {
            name: 'Widget.TaskTable',
            applyTo: 'tasktable',
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

    actions: {
        'tasktable #allTasksBtn': {
            click: function(){
                Ext.History.add('reminderInspector');
            }
        },

        'tasktable #refreshBtn': {
            click: function(btn) {
                var widget = btn.up('tasktable');

                if (widget) {
                    widget.store.load({ limit: 4 });
                }
            }
        },
    },
    
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
            store = me.store = Ext.create(store);
        }
        if (store) {
            me.relayEvents(store, ['load'], 'store');
        }
        
        Ext.apply(me.actions, {
            'tasktable #taskTableDataView': {
                itemclick: {
                    fn: me.openReminderEditCard
                }
            },
        })
        
        me.callParent();
    },

    afterRender: function () {
        this.callParent(arguments);
        if (this.store && this.store.isStore) {
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
                                    '<div><span class="date {Color}">{CheckDate:date("d.m.Y")}</span><span class="type {ColorTypeReminder}">{[B4.GjiTextValuesOverride.getText(B4.enums.TypeReminder.getMeta(values.TypeReminder).Display)]}</span></div>',
                                    '<div><span class="title">{NumText}: </span><span class="text">{Num}</span></div>',
                                    '<div><span class="title">Категория: </span><span class="text">{[B4.enums.CategoryReminder.getMeta(values.CategoryReminder).Display]}</span></div>',
                                    
                                    '<tpl if="values.ContragentName">',
                                         '<div><span class="title">ЮЛ: </span><span class="text">{ContragentName}</span></div>',
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
    },

    openReminderEditCard: function(dw, rec) {
        var me = this,
            widget = dw.up('tasktable'),
            id = rec.get('Id'),
            model = this.getModel('Reminder'),
            typeReminder,
            controllerEditName,
            params = {},
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
                    modelInspectionGji = me.getModel('InspectionGji');

                    controllerEditName = widget.getControllerName(inspection.TypeBase);
                    params = new modelInspectionGji({ Id: record.get('InspectionGji').Id });

                    // Получаем тип документа, в зависимости от типа задаем имя дефолтного контроллера (откроется вкладка по умолчанию) и дефолтных параметров
                    if (documentGji) {
                        defaultParams = widget.getDefaultParams(documentGji.TypeDocumentGji);
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