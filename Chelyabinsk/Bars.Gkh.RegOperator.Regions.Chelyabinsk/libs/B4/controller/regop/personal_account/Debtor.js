Ext.define('B4.controller.regop.personal_account.Debtor', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url',
        'B4.view.regop.personal_account.DebtorGrid',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.mixins.Context',
        'Ext.ux.data.PagingMemoryProxy',
        'Ext.ux.IFrame',
        'Ext.tree.Panel',
        'B4.form.Window'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'debtorgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.ClaimWorkForm',
                    applyTo: 'button[action="CreateClaimWork"]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        var grid = component.up('debtorgrid');
                        if (allowed) {
                            component.show();
                            grid.headerCt.child('gridcolumn[isCheckerHd]').show();
                        } else {
                            component.hide();
                            grid.headerCt.child('gridcolumn[isCheckerHd]').hide();
                        }
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Columns.BaseTariffDebtSum',
                    applyTo: '[dataIndex=DebtBaseTariffSum]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Columns.DecisionTariffDebtSum',
                    applyTo: '[dataIndex=DebtDecisionTariffSum]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Columns.DebtSum',
                    applyTo: '[dataIndex=DebtSum]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (component) component.setVisible(allowed);
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Column.MonthCount',
                    applyTo: '[dataIndex=ExpirationMonthCount]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Column.HasClaimWork',
                    applyTo: '[dataIndex=HasClaimWork]',
                    selector: 'debtorclaimworkgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Column.CourtType',
                    applyTo: '[dataIndex=CourtType]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.Column.JurInstitution',
                    applyTo: '[dataIndex=JurInstitution]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhRegOp.PersonalAccountOwner.Debtor.UpdateJurInstitution',
                    applyTo: 'button[action=UpdateJurInstitution]',
                    selector: 'debtorgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'debtorButtonExportAspect',
            gridSelector: 'debtorgrid',
            buttonSelector: 'debtorgrid #btnExport',
            controllerName: 'Debtor',
            actionName: 'Export'
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'debtorgrid b4selectfield[name=Municipality]': {change: {fn: me.updateGrid, scope: me}},
            'debtorgrid b4selectfield[name=Owner]': {change: {fn: me.updateGrid, scope: me}},
            'debtorgrid b4selectfield[name=State]': {change: {fn: me.updateGrid, scope: me}},
            'debtorgrid b4selectfield[name=ProgramCr]': {change: {fn: me.updateGrid, scope: me}},
            'debtorgrid': {'render': {fn: me.onMainViewRender, scope: me}},
            'debtorgrid button[action="Create"]': {'click': {fn: me.create}},
            'debtorgrid button[action="Clear"]': {'click': {fn: me.clear}},
            'debtorgrid button[action="CreateClaimWork"]': {'click': {fn: me.validateClaimWork}},
            'debtorgrid button[action="UpdateJurInstitution"]': {'click': {fn: me.updateJurInstitution}},
            'debtorgrid button[action="UpdateExtractInfo"]': {'click': {fn: me.updateExtractInfo}},
            'debtorgrid button[action="CreateClaimWorksByAccNum"]': {'click': {fn: me.createClaimWorksByAccNum}}
        });

        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('debtorgrid');
        this.bindContext(view);
        this.application.deployView(view);

        view.on('itemmouseenter', function (view, record, item, index, e, options) {
            item.classList.add('x-grid-row-focused');
            item.classList.add('x-grid-row-over');
            console.log('enter: ' + index);
        });
        view.on('itemmouseleave', function (view, record, item, index, e, options) {
            if (item.classList.contains('x-grid-row-focused')) item.classList.remove('x-grid-row-focused');
            if (item.classList.contains('x-grid-row-over')) item.classList.remove('x-grid-row-over');
            console.log('leave: ' + index);
        });


        view.getStore().load();

        B4.Ajax.request({
            url: B4.Url.action('GetParams', 'GkhParams')
        }).next(function (response) {
            var settlementCol = view.down('[dataIndex=Settlement]'),
                json = Ext.JSON.decode(response.responseText);

            if (settlementCol) {
                if (json.ShowStlDebtorGrid) {
                    settlementCol.show();
                } else {
                    settlementCol.hide();
                }
            }

        }).error(function () {
            Ext.Msg.alert('Ошибка!', 'Ошибка получения параметров приложения');
        });
    },

    clear: function () {
        var me = this;

        Ext.Msg.confirm('Внимание', 'Удалить все записи реестра? ', function (result) {
            if (result == 'yes') {
                me.mask('Удаление...');
                B4.Ajax.request({
                    url: B4.Url.action('Clear', 'Debtor')
                }).next(function (response) {
                    me.unmask();
                    var json = Ext.JSON.decode(response.responseText);
                    if (json && json.success) {
                        Ext.Msg.alert('Внимание!', 'Записи успешно удалены');
                        me.getMainView().getStore().load();
                    } else {
                        Ext.Msg.alert('Ошибка!', ' Ошибка при удалении!');
                    }
                }).error(function () {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', ' Ошибка при удалении!');
                });
            }
        });
    },

    validateClaimWork: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            totalCount = grid.getStore().totalCount,
            sm = grid.getSelectionModel(),
            selected = Ext.Array.map(sm.getSelection(), function (el) {
                return el.get('Id');
            }),
            filters = {
                dataFilter: Ext.encode(grid.filterBar.toDataFilter(grid.filterBar.filterArray))
            }

        var postParams = Ext.applyIf(filters, {
            ids: selected
        });

        Ext.apply(postParams, me.getFilterParams());

        Ext.Msg.prompt({
            title: 'Начать претензионную работу',
            msg: 'Начать претензионную работы по ' + (selected.length ? selected.length : totalCount) + ' лицевым счетам? ',
            buttons: Ext.Msg.OKCANCEL,

            fn: function (btnId) {
                if (btnId === 'ok') {
                    me.createClaimWork(postParams);
                }
            }
        });
    },

    updateJurInstitution: function () {
        var me = this,
            mainView = me.getMainView();

        me.mask('Обновление...', mainView);
        B4.Ajax.request({
            url: B4.Url.action('UpdateJurInstitution', 'Debtor'),
            timeout: 9999999,
            method: 'POST'
        }).next(function () {
            mainView.store.load();
            me.unmask();
            Ext.Msg.alert('Успешно!', ' Судебные учреждения успешно обновлены!');
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', ' Ошибка при обновлении!');
        });
    },

    updateExtractInfo: function () {
        var me = this,
            mainView = me.getMainView();

        me.mask('Обновление данных о выписках...', mainView);
        B4.Ajax.request({
            url: B4.Url.action('UpdateExtractInfo', 'RosRegExtractOperations'),
            timeout: 9999999,
            method: 'POST'
        }).next(function () {
            mainView.store.load();
            me.unmask();
            Ext.Msg.alert('Успешно!', 'Данные обновлены');
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Ошибка при обновлении');
        });
    },

    createClaimWork: function (postParams) {
        var me = this,
            mainView = me.getMainView();
        postParams = postParams || {};

        me.mask('Формирование...', mainView);
        B4.Ajax.request({
            url: B4.Url.action('CreateClaimWorks', 'Debtor'),
            params: postParams,
            timeout: 100 * 60 * 60,
            method: 'POST'
        }).next(function () {
            mainView.store.load();
            me.unmask();
            Ext.Msg.alert('Успешно!', 'Задача поставлена в очередь на обработку. Статус ее выполнения можно отследить в разделе задачи');
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Ошибка при формировании!');
        });
    },

    createClaimWorksByAccNum: function (btn) {
        {
            var me = this,
                grid = btn.up('grid'),
                totalCount = grid.getStore().totalCount,
                sm = grid.getSelectionModel(),
                selected = Ext.Array.map(sm.getSelection(), function (el) {
                    return el.get('Id');
                }),
                filters = {
                    dataFilter: Ext.encode(grid.filterBar.toDataFilter(grid.filterBar.filterArray))
                }

            var postParams = Ext.applyIf(filters, {
                ids: selected
            });

            Ext.apply(postParams, me.getFilterParams());

            Ext.MessageBox.prompt({
                title: 'Создание ПИР по номерам ЛС',
                msg: 'Список номеров ЛС для создания ПИР:',
                width: 300,
                height: 700,
                buttons: Ext.MessageBox.OKCANCEL,
                multiline: true,
                scope: this,
                fn: function (btn, reason, cfg) {
                    if (btn == 'ok' && Ext.isEmpty(reason)) {
                        //if you want to mark the text as mandatory

                        Ext.MessageBox.show(Ext.apply({}, {msg: cfg.msg}, cfg));
                    } else if (btn == 'ok') {

                        postParams.AccNum = reason;
                        me.createClaimWork(postParams);
                        //alert(reason);                 
                    }
                }
            })
        }
    },

    create: function () {
        var me = this;

        Ext.Msg.confirm('Внимание', 'Сформировать реестр неплательщиков? ', function (result) {
            if (result === 'yes') {
                me.mask('Формирование...');
                B4.Ajax.request({
                    url: B4.Url.action('MakeNew', 'Debtor'),
                    timeout: 100 * 60 * 60
                }).next(function (response) {
                    me.unmask();
                    var json = Ext.JSON.decode(response.responseText);
                    if (json && json.success) {
                        Ext.Msg.alert('Внимание!', 'Задача поставлена в очередь на обработку. Статус ее выполнения можно отследить в разделе задачи');
                        me.getMainView().getStore().load();
                    } else {
                        Ext.Msg.alert('Ошибка!', 'Ошибка при формировании!');
                    }
                }).error(function (e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', e.message);
                });
            }
        });
    },

    onMainViewRender: function (grid) {
        var me = this;

        grid.getStore().on('beforeLoad', me.onStoreBeforeLoad, me);
    },

    onStoreBeforeLoad: function (store, operation) {
        var params = this.getFilterParams(),
            mainView = this.getMainView(),
            selectionModel;
        Ext.apply(operation.params, params);

        if (mainView) {
            selectionModel = mainView.getSelectionModel();
            if (selectionModel) {
                selectionModel.clearSelections();
            }
        }
    },

    getFilterParams: function () {
        var me = this,
            grid = me.getMainView(),
            getMunicipalityFilter = function () {
                var filterValue = grid.down('b4selectfield[name=Municipality]').value,
                    returnValue = [];
                if (filterValue) {
                    if (Array.isArray(filterValue)) {
                        returnValue = Ext.Array.map(filterValue, function (el) {
                            return el.Id;
                        });
                    }
                }
                return Ext.JSON.encode(returnValue);
            },
            getStateFilter = function () {
                var filterValue = grid.down('b4selectfield[name=State]').value,
                    returnValue = [];
                if (filterValue) {
                    if (Array.isArray(filterValue)) {
                        returnValue = Ext.Array.map(filterValue, function (el) {
                            return el.Id;
                        });
                    }
                }
                return Ext.JSON.encode(returnValue);
            },
            getOwnerFilter = function () {
                var filterValue = grid.down('b4selectfield[name=Owner]').value,
                    returnValue = [];
                if (filterValue) {
                    if (Array.isArray(filterValue)) {
                        returnValue = Ext.Array.map(filterValue, function (el) {
                            return el.Id;
                        });
                    }
                }
                return Ext.JSON.encode(returnValue);
            },
            getPogramCrFilter = function () {
                var filterValue = grid.down('b4selectfield[name=ProgramCr]').value,
                    returnValue = [];
                if (filterValue) {
                    if (Array.isArray(filterValue)) {
                        returnValue = Ext.Array.map(filterValue, function (el) {
                            return el.Id;
                        });
                    }
                }
                return Ext.JSON.encode(returnValue);
            };

        return {
            municipalityIds: getMunicipalityFilter(),
            stateIds: getStateFilter(),
            ownerIds: getOwnerFilter(),
            programCrIds: getPogramCrFilter()
        };
    },

    updateGrid: function () {
        this.getMainView().getStore().load();
    }
});