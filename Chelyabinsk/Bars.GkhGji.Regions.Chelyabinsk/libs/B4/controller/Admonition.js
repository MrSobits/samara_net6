Ext.define('B4.controller.Admonition', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.Ajax',
        'B4.aspects.GkhBlobText',
        'B4.aspects.GkhGridMultiSelectWindowTree',
        'B4.Url',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    appealCitsAdmonition: null,

    mixins: {
        mask: 'B4.mixins.MaskBody',       
        context: 'B4.mixins.Context'
    },

    stores: [
        'appealcits.Admonition',
        'appealcits.AppCitAdmonVoilation',
        'appealcits.AppCitAdmonAppeal',
        'appealcits.ForSelect',
        'appealcits.ForSelected',
        'appealcits.AppCitAdmonVoilation',
        'appealcits.AppCitAdmonAnnex'
    ],

    models: [
        'AppealCits',
        'appealcits.Admonition'
    ],

    views: [
        'appealcits.AdmonitionGrid',
        'appealcits.AdmonitionEditWindow',
        'appealcits.AdmonVoilationGrid',
        'appealcits.AppCitAdmonAppealGrid',
        'appealcits.AdmonVoilationEditWindow',
        'appealcits.AdmonAnnexGrid',
        'appealcits.AdmonAnnexEditWindow',
        'appealcits.MainPanel',       
        'SelectWindow.MultiSelectWindowTree',
        'appealcits.AdmonitionFilterPanel',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'appealcits.MainPanel',
    mainViewSelector: 'appealcitsMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'appealcitsMainPanel'
        },
        {
            ref: 'admonitionEditWindow',
            selector: 'admonitioneditwindow'
        }
    ],

    aspects: [
        {
            xtype: 'gkhbuttonprintaspect',
            name: 'admonitionPrintAspect',
            buttonSelector: '#admonitioneditwindow #btnPrint',
            codeForm: 'AppealCitsAdmonition',
            getUserParams: function () {
                var param = { Id: appealCitsAdmonition };
                this.params.userParams = Ext.JSON.encode(param);
            }
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'admonitionBlobViolationAspect',
            fieldSelector: '[name=Violations]',
            editPanelAspectName: 'admonitionGridWindowAspect',
            controllerName: 'AdmonitionOperations',
            valueFieldName: 'Violations',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'gkhblobtextaspect',
            name: 'admonitionBlobMeasuresAspect',
            fieldSelector: '[name=Measures]',
            editPanelAspectName: 'admonitionGridWindowAspect',
            controllerName: 'AdmonitionOperations',
            valueFieldName: 'Measures',
            previewLength: 2000,
            autoSavePreview: true,
            previewField: false
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'disposalGjiButtonExportAspect',
            gridSelector: '#admonitiongrid',
            buttonSelector: '#admonitiongrid #btnExport',
            controllerName: 'AdmonitionOperations',
            actionName: 'Export'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'admonitionGridWindowAspect',
            gridSelector: 'admonitiongrid',
            editFormSelector: 'admonitioneditwindow',
            modelName: 'appealcits.Admonition',
            storeName: 'appealcits.Admonition',
            editWindowView: 'appealcits.AdmonitionEditWindow',
            otherActions: function (actions) {
                actions['#appealcitsAdmonitionFilterPanel #dfDateStart'] = { 'change': { fn: this.onChangeDateStart, scope: this } };
                actions['#appealcitsAdmonitionFilterPanel #dfDateEnd'] = { 'change': { fn: this.onChangeDateEnd, scope: this } };
                actions['#appealcitsAdmonitionFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#admonitioneditwindow #dfPayerType'] = { 'change': { fn: this.onChangePayerType, scope: this } };
                actions['#admonitioneditwindow #sfInspectionReasonERKNM'] = { 'beforeload': { fn: this.onBeforeLoadReason, scope: this } };
                actions[this.editFormSelector + ' button[action=ERKNMRequest]'] = { 'click': { fn: this.ERKNMRequest, scope: this } };
            },
            onBeforeLoadReason: function (field, options, store) {
                debugger;
                options = options || {};
                options.params = options.params || {};
                options.params.ERKNMDocumentType = 20;
            },
            ERKNMRequest: function (btn) {
                var me = this,
                    panel = btn.up('#admonitioneditwindow'),
                    record = panel.getForm().getRecord();
                debugger;
                var recId = record.getId();
                Ext.Msg.confirm('Запрос в ЕРКНМ', 'Подтвердите размещение проверки в ЕРКНМ', function (result) {
                    if (result == 'yes') {
                        me.mask('Отправка запроса', B4.getBody());
                        B4.Ajax.request({
                            url: B4.Url.action('SendERKNMRequest', 'ERKNMExecute'),
                            method: 'POST',
                            timeout: 100 * 60 * 60 * 3,
                            params: {
                                docId: recId,
                                typeDoc: 20
                            }
                        }).next(function () {
                            B4.QuickMsg.msg('СМЭВ', 'Запрос на  размещение проверки в ЕРКНМ отправлен', 'success');
                            me.unmask();
                        }, me)
                            .error(function (result) {
                                if (result.responseData || result.message) {
                                    Ext.Msg.alert('Ошибка отправки запроса!', Ext.isString(result.responseData) ? result.responseData : result.message);
                                }
                                me.unmask();
                            }, me);

                    }
                }, this);
            },
            onUpdateGrid: function () {
                var str = this.controller.getStore('appealcits.Admonition');
                str.currentPage = 1;
                str.load();
            },
            onChangeDateStart: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateStart = newValue;
                }
            },
            onChangeDateEnd: function (field, newValue, oldValue) {
                if (this.controller.params) {
                    this.controller.params.dateEnd = newValue;
                }
            },
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },



            onChangePayerType: function (field, newValue) {
                var form = this.getForm(),
      
                    fsUrParams = form.down('#fsUrParams'),
                    fsFizParams = form.down('#fsFizParams'),
                    fsIpParams = form.down('#fsUrParams'),
                    contragent = form.down('#contragent');

                if (newValue == B4.enums.PayerType.Physical) {
                    fsFizParams.show();
                    fsFizParams.setDisabled(false);
                    fsUrParams.hide();
                    fsUrParams.setDisabled(true);
                    fsIpParams.hide();
                    fsIpParams.setDisabled(true);
                    contragent.hide();
                    contragent.setDisabled(true);
                }
                else if (newValue == B4.enums.PayerType.Juridical) {
                    fsFizParams.hide();
                    fsFizParams.setDisabled(true);
                    fsUrParams.show();
                    fsUrParams.setDisabled(false);                  
                    contragent.show();
                    contragent.setDisabled(false);
                }
                else {
                    fsFizParams.hide();
                    fsFizParams.setDisabled(true);
                    fsUrParams.hide();
                    fsUrParams.setDisabled(true);
                    fsIpParams.show();
                    fsIpParams.setDisabled(false);
                    contragent.show();
                    contragent.setDisabled(false);
                }
            },
             listeners: {
                 aftersetformdata: function(asp, rec, form) {
                     var me = this,
                         appealCitsAdmonition = rec.getId();
                     debugger;
                     me.controller.getAspect('admonitionPrintAspect').loadReportStore();
                     var grid = form.down('admonVoilationGrid'),
                         store = grid.getStore();
                     var grid_annex = form.down('admonAnnexGrid'),
                         store_annex = grid_annex.getStore();
                     var grid_appeal = form.down('appCitAdmonAppealGrid'),
                         store_appeal = grid_appeal.getStore();
                     store.filter('AppealCitsAdmonition', rec.getId());
                     store_annex.filter('AppealCitsAdmonition', rec.getId());
                     store_appeal.filter('AppealCitsAdmonition', rec.getId());
                     this.controller.getAspect('admonitionBlobViolationAspect').doInjection();
                     this.controller.getAspect('admonitionBlobMeasuresAspect').doInjection();
                     store.filter('AppealCitsAdmonition', appealCitsAdmonition);
                 }
             }
         },
        {
            /*
            аспект взаимодействия ВКЛАДКИ Связанные/Аналогичные обращения с массовой формой выбора 
            по нажатию на кнопку отбора показывается форма массового выбора после чего идет отбор
            По нажатию на кнопку Применить в методе getdata мы обрабатываем полученные значения
            и сохраняем обращения граждан через серверный метод /AppealCitizensGJI/AddAppealCitizens
            */
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'appCitAdmonAppealMultiSelectWindowAspect',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#appCitAdmonAppealSelectWindow',
            storeSelect: 'appealcits.ForSelect',
            storeSelected: 'appealcits.ForSelected',
            gridSelector: 'appCitAdmonAppealGrid',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    flex: 1,
                    header: '№ обращения',
                    filter: { xtype: 'textfield' } 
                },
                {
                    xtype: 'datecolumn',
                    dataIndex: 'DateFrom',
                    format: 'd.m.Y',
                    flex: 1,
                    header: 'Дата регистрации',
                    filter: { xtype: 'datefield', operand: CondExpr.operands.eq }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Correspondent',
                    flex: 1,
                    header: 'Заявитель',
                    filter: { xtype: 'textfield' } 
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'NumberGji',
                    flex: 1,
                    header: '№ обращения'
                }
            ],
            titleSelectWindow: 'Выбор обращений граждан',
            titleGridSelect: 'Обращения граждан для выбора',
            titleGridSelected: 'Выбранные обращения граждан',
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec, index) { recordIds.push(rec.get('Id')); });
                    debugger;
                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('SaveAppeal', 'AdmonitionOperations', {
                            objectIds: recordIds,
                            admonId: asp.controller.appealCitsAdmonition
                        })).next(function (response) {
                            Ext.ComponentQuery.query('appCitAdmonAppealGrid')[0].store.load();
                            asp.controller.unmask();
                            Ext.Msg.alert('Сохранение!', 'Обращения успешно сохранены');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать обращения граждан');
                        return false;
                    }
                    return true;
                }
            },
            deleteRecord: function (record) {
                var me = this;

                Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
                    if (result == 'yes') {
                        var controller = this.controller;
                        controller.mask('Удаление', controller.getMainComponent());
                        B4.Ajax.request(B4.Url.action('RemoveRelated', 'AdmonitionOperations', {
                            appealNumber: record.get('Id'),
                            admonId: controller.appealCitsAdmonition
                        })).next(function (response) {
                            controller.unmask();
                            Ext.ComponentQuery.query('appCitAdmonAppealGrid')[0].store.load();
                            return true;
                        }).error(function (response) {
                            Ext.Msg.alert('Ошибка удаления!', response.message);
                            controller.unmask();
                        });
                    }
                }, me);
            },
            //onBeforeLoad: function (store, operation) {
            //    operation = operation || {};
            //    operation.params = operation.params || {};

            //    operation.params.relatesToId = this.controller.appealCitizensId;
            //    operation.params.matchRelated = true;

            //    this.getSelectedGrid().getStore().load();
            //},

            //onSelectedBeforeLoad: function (store, operation) {
            //    operation = operation || {};
            //    operation.params = operation.params || {};

            //    operation.params.relatesToId = this.controller.appealCitizensId;
            //}
        },
        {
            xtype: 'gkhmultiselectwindowtreeaspect',
            name: 'admoVoilationTMSWIGAspect',
            gridSelector: '#admonVoilationGrid',
            saveButtonSelector: '#admonVoilationGrid #admonViolationSaveButton',
            storeName: 'appealcits.AppCitAdmonVoilation',
            modelName: 'appealcits.AppCitAdmonVoilation',
            multiSelectWindow: 'SelectWindow.MultiSelectWindowTree',
            multiSelectWindowSelector: '#multiSelectWindowTree',
            storeSelect: 'dict.ViolationGjiForTreeSelect',
            storeSelected: 'dict.ViolationGjiForSelected',
            titleSelectWindow: 'Выбор нарушений',
            titleGridSelect: 'Нарушения для отбора',
            titleGridSelected: 'Выбранные нарушения',
            isTbar: true,
            tbarCmp: [
                {
                    xtype: 'textfield',
                    ident: 'searchfield',
                    width: 350,
                    emptyText: 'Поиск',
                    enableKeyEvents: true
                },
                {
                    xtype: 'button',
                    text: 'Искать',
                    iconCls: 'icon-page-white-magnify',
                    ident: 'searchbtn'
                }
            ],
            otherActions: function (actions) {
                var me = this;

                actions[me.multiSelectWindowSelector + ' [ident=searchbtn]'] = { 'click': { fn: me.goFilter, scope: me } };
                actions[me.multiSelectWindowSelector + ' [ident=searchfield]'] = {
                    'keypress': {
                        fn: function (scope, e) {
                            if (e.getKey() == 13) {
                                me.goFilter(scope);
                            }
                        }, scope: me
                    }
                };
            },
            goFilter: function (btn) {
                var filterData = btn.up('#multiSelectWindowTree').down('[ident=searchfield]').getValue(),
                    treepanel = btn.up('#multiSelectWindowTree').down('treepanel');
                treepanel.getStore().reload({
                    params: { filter: filterData }
                });
            },
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'treecolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } },
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'Code', width: 140, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'ViolationGjiName', flex: 1, sortable: false },
                { header: 'Пункты НПД', xtype: 'gridcolumn', dataIndex: 'ViolationGjiPin', width: 80, sortable: false }
            ],
            listeners: {
                getdata: function (asp, records) {
                    var currentViolationStore = asp.controller.getStore(asp.storeName),
                        range = currentViolationStore.getRange(0, currentViolationStore.getCount());

                    asp.controller.mask('Выбор нарушений', asp.controller.getMainComponent());

                    currentViolationStore.removeAll();

                    Ext.Array.each(records.items, function (rec) {
                        currentViolationStore.add({
                            Id: 0,
                            Protocol197: asp.controller.params.documentId,
                            CodesPin: rec.get('ViolationGjiPin'),
                            ViolationGjiName: rec.get('ViolationGjiName'),
                            ViolationGjiId: rec.get('ViolationGjiId'),
                            Features: rec.get('FeatViol'),
                            DatePlanRemoval: null
                        });
                    });

                    Ext.Array.each(range, function (rec) {
                        currentViolationStore.add(rec);
                    });

                    asp.controller.unmask();

                    return true;
                }
            },
            onCheckRec: function (node, checked) {
                var me = this,
                    grid = me.getSelectedGrid(),
                    storeSelected = grid.getStore(),
                    model = me.controller.getModel(me.modelName);

                if (grid && node.get('leaf')) {
                    if (checked) {
                        if (storeSelected.find('Id', node.get('Id'), 0, false, false, true) == -1) {
                            storeSelected.add(new model({
                                Id: node.get('Id'),
                                ViolationGjiPin: node.get('Code'),
                                ViolationGjiName: node.get('Name'),
                                ViolationGjiId: node.get('ViolationGjiId')
                            }));
                        }
                    } else {
                        storeSelected.remove(storeSelected.getById(node.get('Id')));
                    }
                }
            },
            getSelectGrid: function () {
                var me = this;
                if (me.componentQuery) {
                    win = me.componentQuery(me.multiSelectWindowSelector);
                }

                if (!win) {
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0];
                }

                if (win) {
                    return win.down('#tpSelect');
                }
            },
            selectedGridRowActionHandler: function (action, record) {
                var me = this,
                    gridSelect = me.getSelectGrid(),
                    gridSelected = me.getSelectedGrid();

                if (gridSelect && gridSelected) {
                    gridSelected.fireEvent('rowaction', gridSelected, action, record);

                    var node = gridSelect.getRootNode().findChild('Id', record.getId(), true);
                    if (node) {
                        node.set('checked', false);
                    }
                }
            },
            getForm: function () {
                var me = this,
                    win = Ext.ComponentQuery.query(me.multiSelectWindowSelector)[0],
                    stSelected,
                    stSelect;

                if (win && !win.getBox().width) {
                    win = win.destroy();
                }

                if (!win) {
                    stSelected = me.storeSelected instanceof Ext.data.AbstractStore ? me.storeSelected : Ext.create('B4.store.' + me.storeSelected);
                    stSelected.on('beforeload', me.onSelectedBeforeLoad, me);

                    stSelect = me.storeSelect instanceof Ext.data.AbstractStore ? me.storeSelect : Ext.create('B4.store.' + me.storeSelect);
                    stSelect.on('beforeload', me.onBeforeLoad, me);
                    stSelect.on('load', me.onLoad, me);

                    win = me.controller.getView(me.multiSelectWindow).create({
                        itemId: me.multiSelectWindowSelector.replace('#', ''),
                        storeSelect: stSelect,
                        storeSelected: stSelected,
                        columnsGridSelect: me.columnsGridSelect,
                        columnsGridSelected: me.columnsGridSelected,
                        title: me.titleSelectWindow,
                        titleGridSelect: me.titleGridSelect,
                        titleGridSelected: me.titleGridSelected,
                        selModelMode: me.selModelMode,
                        isTbar: me.isTbar,
                        tbarCmp: me.tbarCmp,
                        constrain: true,
                        modal: false,
                        closeAction: 'destroy',
                        renderTo: B4.getBody().getActiveTab().getEl()
                    });

                    win.on('afterrender', me.formAfterrender, me);

                    if (Ext.isNumber(me.multiSelectWindowWidth) && win.setWidth) {
                        win.setWidth(me.multiSelectWindowWidth);
                    }

                    stSelected.sorters.clear();
                    stSelect.sorters.clear();
                }

                return win;
            }
        },
        {
            /*
            * Аспект взаимодействия Таблицы 'Дата и время проведения проверки' с формой редактирования
            */
            xtype: 'grideditwindowaspect',
            name: 'admonEditViolationAspect',
            gridSelector: '#admonVoilationGrid',
            editFormSelector: '#admonVoilationEditWindow',
            storeName: 'appealcits.AppCitAdmonVoilation',
            modelName: 'appealcits.AppCitAdmonVoilation',
            editWindowView: 'appealcits.AdmonVoilationEditWindow',
            otherActions: function (actions) {
                actions['#admonVoilationGrid #updateButton'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                actions['#admonVoilationGrid #admonViolationSaveButton'] = { 'click': { fn: this.onSaveViolations, scope: this } };
            },
            onUpdateGrid: function () {
                this.controller.getStore('appealcits.AppCitAdmonVoilation').load();
            },
            onSaveViolations: function () {
                var me = this,
                    grid = me.getGrid(),
                    storeViolation = grid.getStore(),
                    deferred = new Deferred();

                deferred.next(function () {
                    storeViolation.load();
                    Ext.Msg.alert('Сохранение!', 'Нарушения сохранены успешно');
                })
                    .error(function (e) {
                        if (e.message) {
                            Ext.Msg.alert('Ошибка сохранения!', e.message);
                        } else {
                            throw e;
                        }
                    });

                var panel = me.controller.getMainComponent();                

                var violations = [];
                var isCorrectDate = true;
                Ext.Array.each(storeViolation.getRange(0, storeViolation.getCount()),
                    function (item) {
                        var data = item.getData();

                        violations.push(
                            {
                                Id: data.Id || 0,
                                ViolationGjiId: data.ViolationGjiId,
                                PlanedDate: data.PlanedDate,
                                FactDate: data.FactDate
                            });
                    });      

                me.controller.mask('Сохранение', panel);
                B4.Ajax.request({
                    method: 'POST',
                    url: B4.Url.action('SaveViolations', 'AdmonitionOperations'),
                    params: {
                        admonId: me.controller.appealCitsAdmonition,
                        violations: Ext.encode(violations)
                    }
                }).next(function () {
                    me.controller.unmask();
                    deferred.call({ message: 'Сохранение нарушений прошло успешно' });
                }).error(function (e) {
                    me.controller.unmask();
                    deferred.fail(e);
                });
                return true;

            },
            editRecord: function (record) {
                var me = this,
                    id = record ? record.getId() : null,
                    model;

                if (!id) return;

                model = me.getModel(record);
                model.load(id, {
                    success: function (rec) {
                        me.setFormData(rec);
                    },
                    scope: me
                });

                me.getForm().getForm().isValid();

                me.controller.currentViolId = id;
            }
        },
         {
             xtype: 'grideditwindowaspect',
             name: 'admonAnnexGridWindowAspect',
             gridSelector: '#admonAnnexGrid',
             editFormSelector: '#admonAnnexEditWindow',
             storeName: 'appealcits.AppCitAdmonAnnex',
             modelName: 'appealcits.AppCitAdmonAnnex',
             editWindowView: 'appealcits.AdmonAnnexEditWindow',
             listeners: {
                 getdata: function (asp, record) {
                     if (!record.get('Id')) {
                         record.set('AppealCitsAdmonition', appealCitsAdmonition);
                     }
                 }
             }
         },

    ],

    index: function (operation) {
        var me = this,
            view = me.getMainView() || Ext.widget('appealcitsMainPanel');
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        me.bindContext(view);
        this.application.deployView(view);
        //me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);

        this.getStore('appealcits.Admonition').load();
        // this.getStore('appealcits.Admonition').filter()
    },

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        me.params.dateStart = new Date(new Date().getFullYear(), 0, 1);
        me.params.dateEnd = new Date(new Date().getFullYear(), 11, 31);
        this.getStore('appealcits.Admonition').on('beforeload', this.onBeforeLoadAdmonition, this);
        this.getStore('appealcits.Admonition').load();
        me.callParent(arguments);
    },

    onBeforeLoadAdmonition: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    },
});