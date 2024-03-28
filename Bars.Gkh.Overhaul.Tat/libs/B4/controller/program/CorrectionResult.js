Ext.define('B4.controller.program.CorrectionResult', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.view.program.CorrectionActualizeYearsWindow',
        'B4.view.program.CorrectionResultDetailsGrid',
        'B4.Ajax',
        'B4.Url',
        'B4.aspects.GridEditWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'version.ProgramVersion',
        'program.CorrectionResult'
    ],

    stores: [
        'program.CorrectionResult',
        'program.CorrectResultForMassChangeSelect',
        'program.CorrectResultForMassChangeSelected'
    ],

    views: [
        'program.CorrectionResultPanel',
        'program.CorrectionResultDetailsGrid',
        'program.EditCorrectionResultWindow',
        'program.MassCorrectYearChangeWindow',
        'SelectWindow.MultiSelectWindow'
    ],

    mainView: 'program.CorrectionResultPanel',
    mainViewSelector: 'correctionresultpanel', 
        
    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'correctionresultpanel correctionresultgrid',
            buttonSelector: 'correctionresultpanel correctionresultgrid #btnExport',
            controllerName: 'DpkrCorrectionStage2',
            actionName: 'Export'
        },
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'statepermissionaspect',
            permissions: [
                {
                    name: "Ovrhl.ProgramCorrection.CreateShortProgram",
                    applyTo: '[action=CreateShortProgram]',
                    selector: 'correctionresultpanel correctionresultgrid'
                },
                {
                    name: "Ovrhl.ProgramCorrection.ActualizeProgram",
                    applyTo: '[action=ActualizeProgram]',
                    selector: 'correctionresultpanel correctionresultgrid'
                },
                {
                    name: 'Ovrhl.ProgramCorrection.PublishDpkr',
                    applyTo: '[action=PublishDpkr]',
                    selector: 'correctionresultpanel correctionresultgrid'
                },
                {
                    name: 'Ovrhl.ProgramCorrection.MassYearChange',
                    applyTo: '[action=massyearchange]',
                    selector: 'correctionresultpanel correctionresultgrid'
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'correctionResultGridWindowAspect',
            storeName: 'program.CorrectionResult',
            modelName: 'program.CorrectionResult',
            gridSelector: 'correctionresultgrid',
            editFormSelector: 'editcorrectionresultwin',
            editWindowView: 'program.EditCorrectionResultWindow',
            otherActions: function (actions) {
                var me = this;

                actions['editcorrectionresultwin b4savebutton'] = { 'click': { fn: me.saveCorrectionResultNumber, scope: me } };
                actions['editcorrectionresultwin [name=PlanYear]'] = { 'change': { fn: me.onChangePlanYear, scope: me } };
            },
            listeners: {
                aftersetformdata: function (asp, record) {
                    var me = this,
                        window = me.getForm(),
                        storeDetails = window.down('correctionresultdetailsgrid').getStore(),
                        storeHistory = window.down('progcorrecthistorygrid').getStore();
                    
                    storeDetails.clearFilter(true);
                    storeDetails.filter([
                        { property: 'st3Id', value: record.get('St3Id') }
                    ]);

                    storeHistory.clearFilter(true);
                    storeHistory.filter([
                        { property: 'st3Id', value: record.get('St3Id') },
                        { property: 'id', value: record.get('Id') }
                    ]);
                }
            },
            onChangePlanYear: function (fld) {
                var window = fld.up('editcorrectionresultwin'),
                    chkBox = window.down('[name=FixedYear]');

                chkBox.setValue(true);
            },
            saveCorrectionResultNumber: function (btn) {
                var me = this,
                    form = me.getForm(),
                    win = btn.up('editcorrectionresultwin');
                
                if (me.fireEvent('beforesaverequest', me) !== false) {
                    form.getForm().updateRecord();
                    var record = form.getForm().getRecord(),
                        newNumber = form.getValues().NewIndexNumber,
                        newPlanYear = form.getValues().PlanYear,
                        fixedYear = form.getValues().FixedYear;
                    
                    me.controller.mask('Сохранение', me.controller.getMainComponent());
                    B4.Ajax.request({
                        url: B4.Url.action('ChangeIndexNumber', 'DpkrCorrectionStage2'),
                        params: {
                            id: record.get('Id'),
                            st3Id: record.get('St3Id'),
                            newNumber: newNumber,
                            newPlanYear: newPlanYear,
                            fixedYear: fixedYear
                        },
                        timeout: 9999999
                    }).next(function () {
                        me.getGrid().getStore().load();
                        me.controller.unmask();

                        win.close();
                        
                        B4.QuickMsg.msg('Сохранение!', 'Номер и плановый год успешно обновлены', 'success');
                    }).error(function (response) {
                        me.controller.unmask();

                        B4.QuickMsg.msg('Ошибка', response && response.message ? response.message : 'Ошибка при сохранении', 'error');
                    });
                    
                }
            }
        },
        {
            xtype: 'gkhtriggerfieldmultiselectwindowaspect',
            name: 'progmasscorrectyearchangewinMultiselectwindowaspect',
            fieldSelector: 'progmasscorrectyearchangewin [name=correctionRecForChange]',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#progmasscorrectyearchangewinMunicipalitySelectWindow',
            storeSelect: 'program.CorrectResultForMassChangeSelect',
            storeSelected: 'program.CorrectResultForMassChangeSelected',
            textProperty: 'IndexNumber',
            columnsGridSelect: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'IndexNumber',
                    text: 'Номер',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 50
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjectName',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstPlanYear',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanYear',
                    text: 'Скорректированный год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'Sum',
                    text: 'Сумма',
                    filter: {
                        xtype: 'numberfield',
                        allowDecimals: true,
                        hideTrigger: true,
                        minValue: 0,
                        operand: CondExpr.operands.eq
                    },
                    width: 75,
                    renderer: function (value) {
                        return Ext.util.Format.currency(value);
                    }
                }
            ],
            columnsGridSelected: [
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'RealityObject',
                    flex: 1,
                    text: 'Адрес',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'CommonEstateObjectName',
                    flex: 1,
                    text: 'Объекты общего имущества',
                    filter: {
                        xtype: 'textfield'
                    }
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'FirstPlanYear',
                    text: 'Плановый год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 80
                },
                {
                    xtype: 'gridcolumn',
                    dataIndex: 'PlanYear',
                    text: 'Скорректированный год',
                    filter: {
                        xtype: 'numberfield',
                        hideTrigger: true,
                        minValue: 1900,
                        maxValue: 2200,
                        operand: CondExpr.operands.eq
                    },
                    width: 80
                }
            ],
            titleSelectWindow: 'Выбор записи',
            titleGridSelect: 'Записи для отбора',
            titleGridSelected: 'Выбранная запись',
            onBeforeLoad: function (store) {
                var pnl = this.controller.getMainPanel(),
                    moId = pnl.down('b4combobox[name="Municipality"]').getValue(),
                    typeResult = pnl.params ? pnl.params.type : 0;

                Ext.apply(store.getProxy().extraParams, {
                    municipalityId: moId,
                    type: typeResult
                });
            }
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'correctionresultpanel' }
    ],
    
    init: function () {
        var me = this;
        me.control({
            'correctionresultpanel [action="PublishDpkr"]': { click: { fn: me.validationBeforePublish, scope: me } },
            'correctionresultpanel correctionresultgrid [action="CreateShortProgram"]': { click: { fn: me.onCreateShortProgram, scope: me } },
            'correctionresultpanel correctionresultgrid [action="ActualizeProgram"]': { click: { fn: me.onActualizeDpkr, scope: me } },
            'correctionresultpanel correctionresultgrid b4updatebutton': { click: { fn: me.onUpdate, scope: me } },
            'correctionresultpanel b4combobox[name="Municipality"]': {
                render: { fn: me.onRenderMunicipality, scope: me },
                change: { fn: me.onMoSelect, scope: me }
            },
            'correctionactualizeyearswindow b4savebutton': {
                click: {
                    fn: me.onActualizeYearsApply,
                    scope: me
                }
            },
            'correctionactualizeyearswindow b4closebutton': {
                click: {
                    fn: function (btn) {
                        var win = btn.up('correctionactualizeyearswindow');
                        win.destroy();
                    },
                    scope: me
                }
            },

            'progcorrecthistorygrid b4updatebutton': {
                click: function (btn) {
                    btn.up('progcorrecthistorygrid').getStore().load();
                }
            },
            'progcorrecthistorygrid': {
                itemdblclick: { fn: me.onItemDblClickHistoryGrid, scope: me }
            },
            'progcorrecthistorygrid b4editcolumn': {
                click: { fn: me.editHistoryGridBtnClick, scope: me }
            },
            'correctionresultgrid button[action=massyearchange]': {
                click: {
                    fn: me.onMassYearChangeItemClick,
                    scope: me
                }
            },
            'progmasscorrectyearchangewin button[action=changeyear]': {
                click: { fn: me.massChangeYear, scope: me }
            },
            'progmasscorrectyearchangewin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('window').close();
                    }, scope: me
                }
            }
        });

        me.control(me.createButtonListeners());
        
        me.callParent(arguments);
    },

    index: function(id) {
        var view = this.getMainPanel(),
            grid;

        if (!view) {
            view = Ext.widget('correctionresultpanel');

            this.bindContext(view);
            this.application.deployView(view);

            view.params = {};
            view.params.type = 0;

            grid = view.down('correctionresultgrid');
            grid.clearHeaderFilters();
            
            grid.getStore().on('beforeload', this.onStoreBeforeLoad, this);
        }

        view.down('hidden[name=MunicipalityId]').setValue(id);
    },

    validationBeforePublish: function (btn) {
        var me = this,
            moId = me.getMainPanel().down('b4combobox[name="Municipality"]').getValue();

        B4.Ajax.request({
            url: B4.Url.action('GetValidationForCreatePublishProgram', 'PublishedProgram'),
            timeout: 9999999,
            params: {
                mo_id: moId
            }
        }).next(function (resp) {
            var message = Ext.decode(resp.responseText);

            Ext.Msg.confirm('Внимание', message, function (result) {
                if (result == 'yes') {
                    me.onPublishDpkr(btn);
                }
            });

        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    onPublishDpkr: function (btn) {
        var me = this,
            panel = me.getMainPanel(),
            moId = panel.down('b4combobox[name="Municipality"]').getValue();

        me.mask('Публикация программы', panel);
        B4.Ajax.request({
            url: B4.Url.action('CreateDpkrForPublish', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999,
            params: {
                mo_id: moId
            }
        }).next(function (resp) {
            me.unmask();
            Ext.Msg.alert("Сообщение!", "Программа успешно опубликована!");
            Ext.History.add('publicationprogs/' + moId);
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    createButtonListeners: function() {
        var me = this,
            btns = this.getButtonSelectors(),
            control = {};
        
        Ext.each(btns, function(btn) {
            control['correctionresultpanel ' + btn] = {
                click: {
                    fn: me.filterByResultType,
                    scope: me
                }
            };
        });

        me.control(control);
    },
    
    onStoreBeforeLoad: function (store) {
        var pnl = this.getMainPanel(),
            moId = pnl.down('b4combobox[name="Municipality"]').getValue(),
            typeResult = pnl.params ? pnl.params.type : 0;

        Ext.apply(store.getProxy().extraParams, {
            municipalityId: moId,
            type: typeResult
        });
    },
    
    onActualizeDpkr: function() {
        var me = this,
            pnl = me.getMainPanel(),
            moId = pnl.down('b4combobox[name="Municipality"]').getValue();

        me.mask('Актуализация ДПКР', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('ActualizeVersion', 'ShortProgramRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                municipality_id: moId
            }
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg('Сообщение', 'Актуализация ДПКР выполнена успешно', 'success');
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert("Ошибка!", e.message ? e.message : 'Во время выполнения актуализации ДПКР произошла ошибка');
        });
    },

    onCreateShortProgram: function () {
        
        // показываем окно выбора параметров год 
        var winParams = Ext.widget('correctionactualizeyearswindow', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        winParams.show();
        
        /*
        var me = this,
            pnl = me.getMainPanel(),
            moId = pnl.down('b4combobox[name="Municipality"]').getValue(),
            message = 'После формирования краткосрочной программы записи, подсвеченные желтым, добавятся в программу, а записи, подсвеченные красным, удалятся из нее. Сформировать краткосрочную программу?';

        Ext.Msg.confirm('Предупреждение', message, function(result) {
            if (result == 'yes') {
                me.mask('Формирование программы', me.getMainPanel());
                B4.Ajax.request({
                    url: B4.Url.action('CreateShortProgram', 'ShortProgramRecord'),
                    method: 'POST',
                    timeout: 9999999,
                    params: {
                        municipality_id: moId
                    }
                }).next(function (resp) {
                    me.unmask();
                    var obj = Ext.JSON.decode(resp.responseText);
                    
                    me.fillFilters(function() {
                        Ext.History.add('shortprogram/' + moId);
                    });
                    if (obj.message) {
                        B4.QuickMsg.msg('Успешно', obj.message ? obj.message : 'Краткосрочная программа успешно сформирована', 'success');
                    }
                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert("Ошибка!", e.message ? e.message : 'Во время формирования программы произошла ошибка');
                });
            }
        });
        */
    },
    
    onMoSelect: function () {
        this.onUpdate();
    },
    
    onUpdate: function () {
        this.fillFilters();
    },
    
    fillFilters: function (callback) {
        var me = this,
            panel = me.getMainPanel(),
            muId = panel.down('b4combobox[name="Municipality"]').getValue(),
            grid = panel.down('correctionresultgrid'),
            store = grid.getStore();

        grid.down('button[action="CreateShortProgram"]').setDisabled(true);
        grid.down('button[action="PublishDpkr"]').setDisabled(true);
        grid.setDisabled(true);
        store.removeAll();

        me.mask();

        me.resetFilterButtons();

        Ext.each(me.getButtonSelectors(), function (btn) {
            panel.down(btn).setDisabled(true);
        });

        B4.Ajax.request(B4.Url.action('GetInfo', 'DpkrCorrectionStage2', {
            municipalityId: muId
        })).next(function(resp) {
            me.unmask();
            var obj = Ext.JSON.decode(resp.responseText),
                data = obj.data,
                versionId = obj.versionId,
                btn,
                cnt = 0,
                model = me.getModel('version.ProgramVersion');

            grid.setDisabled(false);
            store.load();

            Ext.each(me.getButtonSelectors(), function (selector) {
                panel.down(selector).setDisabled(false);
            });

            Ext.each(data, function (item) {
                btn = panel.down('[actionName="' + item.Type + '"]');
                cnt += item.Count;
                
                if (btn) {
                    btn.setText(btn.getText().replace(/\d+/, '') + item.Count);
                }
            });

            btn = panel.down('[actionName="AllRecords"]');
            if (btn) {
                btn.setText(btn.getText().replace(/\d+/, '') + cnt);
            }
            
            if (versionId) {
                me.getAspect('statepermissionaspect').setPermissionsByRecord(new model({ Id: versionId }));
            }

            if (callback) {
                callback();
            }

        }).error(function(e) {
            me.unmask();

            B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время получения программы произошла ошибка', 'error');
        });
    },
    
    filterByResultType: function(b) {
        var pnl = this.getMainPanel();
        
        if (pnl.params) {
            pnl.params.type = b.filterValue;
            pnl.down('correctionresultgrid').getStore().load();
        }
    },
    
    getButtonSelectors: function() {
        return [
            '[actionName="AllRecords"]',
            '[actionName="InShortTerm"]',
            '[actionName="RemoveFromShortTerm"]',
            '[actionName="AddInShortTerm"]'
        ];
    },
    
    resetFilterButtons: function() {
        var btns = this.getButtonSelectors(),
            panel = this.getMainPanel();

        Ext.each(btns, function(btn) {
            var btn = panel.down(btn);
            if (btn) {
                btn.setText(btn.getText().replace(/\d+/, '') + 0);
            }
        });
    },
    
    onRenderMunicipality: function(field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me);
        store.load();
    },
    
    onLoadMunicipality: function(store, records) {
        var me = this,
            panel = me.getMainPanel(),
            cmb = panel.down('b4combobox[name="Municipality"]'),
            muId = panel.down('hidden[name=MunicipalityId]').getValue(),
            record;

        var countRecords = store.getCount();
        if (countRecords > 0) {
            
            if (muId) {
                record = store.findRecord('Id', muId, false, true, true);
            }

            cmb.setValue(record ? record.getData() : records[0].getData());
        }
    },
    
    onActualizeYearsApply: function (btn) {
        var me = this,
            wnd = btn.up('correctionactualizeyearswindow'),
            year = wnd.down('[name=Year]').getValue(),
            pnl = me.getMainPanel(),
            moId = pnl.down('b4combobox[name="Municipality"]').getValue();
        
        if (!year) {
            Ext.Msg.alert("Ошибка!", 'Необходимо выбрать год');
            return;
        }

        me.mask('Формирование программы за '+year+' год', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('CreateShortProgram', 'ShortProgramRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                municipality_id: moId,
                year: year
            }
        }).next(function (resp) {
            wnd.destroy();
            me.unmask();
            var obj = Ext.JSON.decode(resp.responseText);

            me.fillFilters(function () {
                Ext.History.add('shortprogram/' + moId);
            });
            if (obj.message) {
                B4.QuickMsg.msg('Успешно', obj.message ? obj.message : 'Краткосрочная программа успешно сформирована', 'success');
            }
        }).error(function (e) {
            wnd.destroy();
            me.unmask();
            Ext.Msg.alert("Ошибка!", e.message ? e.message : 'Во время формирования программы произошла ошибка');
        });

    },

    onItemDblClickHistoryGrid: function (view, record) {
        var detailWindow = Ext.ComponentQuery.query('progcorrecthistorydetwindow')[0];

        if (detailWindow) {
            detailWindow.show();
        } else {
            detailWindow = Ext.create('B4.view.program.CorrectionHistoryDetailWindow',
            {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                closeAction: 'destroy',
                ctxKey: this.getCurrentContextKey()
            });
            detailWindow.show();
        }

        detailWindow.down('progcorrecthistorydetgrid').getStore().filter('logEntityId', record.get('Id'));
    },

    editHistoryGridBtnClick: function (gridView, rowIndex, colIndex, el, e, rec) {
        this.onItemDblClickHistoryGrid(gridView, rec);
    },

    onMassYearChangeItemClick: function () {
        var win = Ext.widget('progmasscorrectyearchangewin', {
            constrain: true,
            renderTo: B4.getBody().getActiveTab().getEl(),
            closeAction: 'destroy'
        });

        win.show();
    },

    massChangeYear: function (btn) {
        var me = this,
            win = btn.up('window'),
            panel = me.getMainPanel(),
            newYear = win.down('[name=NewYear]').getValue(),
            recIds = win.down('[name=correctionRecForChange]').getValue(),
            grid = panel.down('correctionresultgrid');

        if (!newYear || !recIds) {
            Ext.Msg.alert('Ошибка!', 'Значение полeй не может быть пустым');
            return;
        }

        if (!win.getForm().isValid()) {
            Ext.Msg.alert('Ошибка!', 'Выберите год входящий в диапозон');
            return;
        }

        me.mask('Пожалуйста, подождите...', panel);
        B4.Ajax.request({
            url: B4.Url.action('MassChangeYear', 'DpkrCorrectionStage2'),
            params: {
                newYear: newYear,
                recIds: recIds
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            var resp = Ext.decode(response.responseText);
            grid.getStore().load();
            Ext.Msg.alert('Предупреждение', resp.data);
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время изменения года произошла ошибка'));
        });

    }
});