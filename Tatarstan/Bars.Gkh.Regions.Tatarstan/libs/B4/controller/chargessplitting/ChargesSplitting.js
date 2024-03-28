Ext.define('B4.controller.chargessplitting.ChargesSplitting', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.GkhInlineGrid',
        'B4.aspects.permission.GkhInlineGridPermissionAspect'
    ],

    views: [
        'chargessplitting.MainPanel',
        'chargessplitting.contrpersumm.Grid',
        'chargessplitting.budgetorg.Grid'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    aspects:[
        {
            xtype: 'gkhpermissionaspect',
            name: 'chargesSplittingPermissionAspect',
            applyOn: {
                event: 'beforerender',
                selector: 'chargessplittingmainpanel'
            },
            permissions: [
                {
                    name: 'Gkh.ChargesSplitting.Period_View',
                    applyTo: 'button[action=GoToPeriods]',
                    selector: 'contractperiodsummarygrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ChargesSplitting.ContractPeriodSumm.ImportExport_View',
                    applyTo: 'button[action=Operations]',
                    selector: 'contractperiodsummarygrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ChargesSplitting.ContractPeriodSumm.Register.Uo_View',
                    applyTo: 'gridcolumn[name=ManOrgGroup]',
                    selector: 'contractperiodsummarygrid',
                    applyBy: function (column, allowed) {
                        column.allowShow = allowed;
                    }
                },
                {
                    name: 'Gkh.ChargesSplitting.ContractPeriodSumm.Register.Rso_View',
                    applyTo: 'gridcolumn[name=PubServOrgGroup]',
                    selector: 'contractperiodsummarygrid',
                    applyBy: function (column, allowed) {
                        column.allowShow = allowed;
                    }
                },

                /* 
                * Договор РСО - Бюджет
                */
                {
                    name: 'Gkh.ChargesSplitting.Period_View',
                    applyTo: 'button[action=GoToPeriods]',
                    selector: 'budgetorggrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ChargesSplitting.BudgetOrg.ImportExport_View',
                    applyTo: 'button[action=Operations]',
                    selector: 'budgetorggrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ChargesSplitting.Period_View',
                    applyTo: 'button[action=GoToPeriods]',
                    selector: 'fuelenergyresourcecontractgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gkh.ChargesSplitting.FuelEnergyResource.Import_View',
                    applyTo: 'button[action=Operations]',
                    selector: 'fuelenergyresourcecontractgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                }
            ]
        },
        {
            /*
            * Договор РСО - Бюджет
            */
            xtype: 'inlinegridpermissionaspect',
            gridSelector: 'budgetorggrid',
            permissionPrefix: 'Gkh.ChargesSplitting.BudgetOrg'
        },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'contractperiodsummaryButtonExportExcelAspect',
            gridSelector: 'contractperiodsummarygrid',
            buttonSelector: 'contractperiodsummarygrid menuitem[action=Export]',
            controllerName: 'PubServContractPeriodSumm',
            actionName: 'Export',
            usePost: true
        },
         {
             /*
             *аспект для импорта
             */
             xtype: 'gkhbuttonimportaspect',
             name: 'importAspect',
             buttonSelector: 'menuitem[action=Import]',
             loadImportList: false
         },
        {
            xtype: 'b4buttondataexportaspect',
            name: 'budgetorggridButtonExportExcelAspect',
            gridSelector: 'budgetorggrid',
            buttonSelector: 'budgetorggrid menuitem[action=Export]',
            controllerName: 'BudgetOrgContractPeriodSumm',
            actionName: 'Export',
            usePost: true
        },
        {
            xtype: 'gkhinlinegridaspect',
            name: 'budgetorgGridAspect',
            modelName: 'chargessplitting.budgetorg.BudgetOrgPeriodSumm',
            storeName: 'chargessplitting.budgetorg.BudgetOrgPeriodSumm',
            gridSelector: 'budgetorggrid'
        }
    ],

    mainView: 'chargessplitting.MainPanel',
    mainViewSelector: 'chargessplittingmainpanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'chargessplittingmainpanel'
        },
        {
            ref: 'contrPerSummView',
            selector: 'contractperiodsummarygrid'
        },
        {
            ref: 'budgetorgView',
            selector: 'budgetorggrid'
        },
        {
            ref: 'fuelEnergyContrView',
            selector: 'fuelenergyresourcecontractgrid'
        }
    ],

    init: function () {
        var me = this,
            actions = {};

        actions['button[action=GoToPeriods]'] = { 'click': { fn: me.goToPeriods, scope: me } };
        actions['contractperiodsummarygrid b4updatebutton'] = { 'click': { fn: function () { me.loadStore(me.getContrPerSummView()); }, scope: me } };
        actions['contractperiodsummarygrid'] = {
            'rowaction': { fn: me.onUoRowAction, scope: me },
            'store.beforeload': { fn: me.onUkStoreBeforeLoad, scope: me },
            'store.load': { fn: me.applyColumnPermission, scope: me },
            'beforerender': { fn: me.disableHeader, scope: me }
        };
        actions['contrpersummfilterpanel b4selectfield'] = { change: { fn: function () { me.loadStore(me.getContrPerSummView()); }, scope: me } };


        actions['chargessplittingmainpanel tabpanel'] = { tabchange: { fn: me.onTabChange, scope: me } };
        actions['budgetorggrid budgetorgfilterpanel b4selectfield'] = { change: { fn: function () { me.loadStore(me.getBudgetorgView()); }, scope: me } };

        actions['budgetorggrid'] = {
            'store.beforeload': { fn: me.onBudgetStoreBeforeLoad, scope: me }
        };

        actions['fuelenergyresourcecontractgrid button[action=Actualize]'] = { 'click': { fn: me.actualizeFuelEnergySumm, scope: me } };
        actions['fuelenergyresourcecontractgrid b4updatebutton'] = { 'click': { fn: function () { me.loadStore(me.getFuelEnergyContrView()); }, scope: me } };
        actions['fuelenergyresourcecontractfilterpanel b4selectfield'] = { change: { fn: function () { me.loadStore(me.getFuelEnergyContrView()); }, scope: me } };

        actions['fuelenergyresourcecontractgrid'] = {
            'rowaction': { fn: me.onTerRowAction, scope: me },
            'store.beforeload': { fn: me.onTerStoreBeforeLoad, scope: me }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('chargessplittingmainpanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

    onTabChange: function (tabPanel, newCard, oldCard, eOpts) {
        var me = this;

        me.loadStore(newCard);

        switch (newCard.xtype) {
            case 'contractperiodsummarygrid': break;
            case 'budgetorggrid': break;
        }
    },

    loadStore: function(view) {
        var store = view.getStore ? view.getStore() : null;

        if (store) {
            store.load();
        }
    },

    onBudgetStoreBeforeLoad: function(store, operation) {
        var params = this.getBudgetorgView().down('budgetorgfilterpanel').getFilterParams();

        Ext.apply(operation.params, params);
    },

    onUkStoreBeforeLoad: function (store, operation) {
        var params = this.getUkFilterParams();

        operation.params.period = params.period;
        operation.params.service = params.service,
        operation.params.municipality = params.municipality;
        operation.params.manOrg = params.manOrg;
        operation.params.pubServOrg = params.pubServOrg;
    },

    onTerStoreBeforeLoad: function (store, operation) {
        var params = this.getTerFilterParams();

        operation.params.period = params.period;
        operation.params.municipality = params.municipality;
        operation.params.pubServOrg = params.pubServOrg;
    },

    getUkFilterParams: function () {
        var me = this,
            filterPanel = me.getContrPerSummView().down('contrpersummfilterpanel');

        var getPeriodFilter = function() {
            var filterValue = filterPanel.down('b4selectfield[name=Period]').getValue();
            return filterValue || 0;
        };

        var getServiceFilter = function () {
            var ids = [],
                filterValue = filterPanel.down('b4selectfield[name=Service]').getValue();

            if (filterValue && filterValue !== 'All') {
                ids = Ext.JSON.encode(filterValue);
            }

            return ids;
        };

        var getMunicipalityFilter = function () {
            var ids = [],
                filterValue = filterPanel.down('b4selectfield[name=Municipality]').getValue();

            if (filterValue && filterValue !== 'All') {
                ids = Ext.JSON.encode(filterValue);
            }

            return ids;
        };

        var getManOrgFilter = function () {
            var ids = [],
                filterValue = filterPanel.down('b4selectfield[name=ManagingOrganization]').getValue();

            if (filterValue && filterValue !== 'All') {
                ids = Ext.JSON.encode(filterValue);
            }

            return ids;
        };

        var getPubServOrgFilter = function () {
            var ids = [],
                filterValue = filterPanel.down('b4selectfield[name=PublicServiceOrg]').getValue();

            if (filterValue && filterValue !== 'All') {
                ids = Ext.JSON.encode(filterValue);
            }

            return ids;
        };

        return {
            period: getPeriodFilter(),
            service: getServiceFilter(),
            municipality: getMunicipalityFilter(),
            manOrg: getManOrgFilter(),
            pubServOrg: getPubServOrgFilter()
        };
    },

    getTerFilterParams: function () {
        var me = this,
            filterPanel = me.getFuelEnergyContrView().down('fuelenergyresourcecontractfilterpanel');

        var getPeriodFilter = function () {
            var filterValue = filterPanel.down('b4selectfield[name=Period]').getValue();
            return filterValue || 0;
        };

        var getMunicipalityFilter = function () {
            var ids = [],
                filterValue = filterPanel.down('b4selectfield[name=Municipality]').getValue();

            if (filterValue && filterValue !== 'All') {
                ids = Ext.JSON.encode(filterValue);
            }

            return ids;
        };

        var getPubServOrgFilter = function () {
            var ids = [],
                filterValue = filterPanel.down('b4selectfield[name=PublicServiceOrg]').getValue();

            if (filterValue && filterValue !== 'All') {
                ids = Ext.JSON.encode(filterValue);
            }

            return ids;
        };

        return {
            period: getPeriodFilter(),
            municipality: getMunicipalityFilter(),
            pubServOrg: getPubServOrgFilter()
        };
    },

    goToPeriods: function() {
        Ext.History.add('contractperiods');
    },

    onUoRowAction: function (grid, action, record) {
        if (!grid || grid.isDestroyed) return;

        switch (action.toLowerCase()) {
            case 'doubleclick':
            case 'edit':
                Ext.History.add(Ext.String.format('contractperiodsumm_detail/{0}/', record.getId()));
                break;
        }
    },

    onTerRowAction: function (grid, action, record) {
        if (!grid || grid.isDestroyed) return;

        var periodSummId = record.get('PeriodSummId');

        if (periodSummId > 0) {
            switch (action.toLowerCase()) {
                case 'doubleclick':
                case 'edit':
                    Ext.History.add(Ext.String.format('fuelenergyresource_detail/{0}/', periodSummId));
                    break;
            }
        }
    },

    applyColumnPermission: function () {
        var grid = this.getContrPerSummView(),
            rsoGroup = grid.down('gridcolumn[name=PubServOrgGroup]'),
            uoGroup = grid.down('gridcolumn[name=ManOrgGroup]'),
            rsoState = grid.down('gridcolumn[dataIndex=RsoState]'),
            uoState = grid.down('gridcolumn[dataIndex=UoState]'),
            headercontainer = grid.down('headercontainer');

        rsoGroup.setVisible(rsoGroup.allowShow);
        rsoState.setVisible(rsoGroup.allowShow);
        uoGroup.setVisible(uoGroup.allowShow);
        uoState.setVisible(uoGroup.allowShow);

        if (grid.getStore().getCount() > 0) {
            headercontainer.show();
        } else {
            headercontainer.hide();
        }
    },

    disableHeader: function(grid) {
        grid.down('headercontainer').hide();
    },

    actualizeFuelEnergySumm: function (button) {
        var me = this,
            grid = button.up('fuelenergyresourcecontractgrid'),
            filterPanel = grid.down('fuelenergyresourcecontractfilterpanel'),
            periodField = filterPanel.down('b4selectfield[name=Period]'),
            periodId = periodField.getValue();

        if (periodId) {
            me.mask();

            B4.Ajax.request({
                url: B4.Url.action('ActualizeFuelEnergyValues', 'FuelEnergyOrgContractDetail'),
                params: {
                    periodId: periodId
                },
                timeout: 5 * 60 * 1000 // 5 минут
            }).next(function () {
                grid.getStore().load();
                me.unmask();

                Ext.Msg.alert('Успешно!', 'Сведения успешно обновлен');
            }).error(function (response) {
                me.unmask();

                Ext.Msg.alert('Ошибка!', response.message);
            });

        } else {
            Ext.Msg.alert('Внимание!', 'Необходимо выбрать период');
        }
    }
});