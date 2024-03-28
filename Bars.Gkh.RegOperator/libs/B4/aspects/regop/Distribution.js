/*  Пример аспекта

          xtype: 'gkhregopdistributionaspect',
          name: 'trancferCrDistributionAspect',
          distribPanel: 'suspenseaccount.DistributionPanel',
          distribPanelSelector: 'suspaccdistribpanel',
          storeSelect: 'regop.personal_account.BasePersonalAccount',
          storeSelected: 'regop.personal_account.BasePersonalAccount',
          columnsGridSelect: [
                { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 1 },
                { header: 'Адрес', xtype: 'gridcolumn', dataIndex: 'RoomAddress', flex: 1},
                { header: 'Владелец', xtype: 'gridcolumn', dataIndex: 'AccountOwner', flex: 1 }
          ],
          columnsGridSelected: [
               { header: 'Номер', xtype: 'gridcolumn', dataIndex: 'PersonalAccountNum', flex: 1, sortable: false }
          ],
          distribObjEditWindowSelector: 'distributionobjectseditwindow',
          distribObjStore: 'fakeEntity.FakeStore',
          distribObjColumnsGrid: [
                { text: 'Номер ЛС', dataIndex: 'PersonalAccountNum',flex: 1 },
                { text: 'Распределенная сумма на взносы КР', dataIndex: 'Sum', flex: 1, editor: {  xtype: 'numberfield' } },
          ],
          getApplyUrlParams: function (win, store) {
              var params,
                  mapped = Ext.Array.map(store.getRange(), function (item) {
                  return {
                      Id: item.data.Id,
                      Sum: item.data.Sum,
                      SumPenalty: item.data.SumPenalty
                  };
              });

              params = {
                  code: this.distribTypeCode,
                  suspenseAccountId: win.suspenseAccountId,
                  records:  Ext.encode(mapped)
              };

              return params;
          }
 */

Ext.define('B4.aspects.regop.Distribution', {
    extend: 'B4.base.Aspect',
    required: [
        'B4.Ajax',
        'B4.Url',
        'B4.ux.grid.plugin.HeaderFilters',
        'B4.ux.grid.column.Delete',
        'B4.enums.SuspenseAccountDistributionParametersView'
    ],
    alias: 'widget.gkhregopdistributionaspect',

    distribPanelSelector: null,

    storeSelect: null,
    storeSelected: null,

    columnsGridSelect: null,
    columnsGridSelected: null,

    distribObjEditWindowSelector: null,
    distribObjStore: null,
    distribObjColumnsGrid: null,

    idProperty: 'Id',

    selModelMode: 'MULTI',

    controller: null,

    initialSum: 0,

    constructor: function (config) {
        var me = this;
        Ext.apply(this, config);
        me.callParent(arguments);

        me.addEvents(
            'beforeaccept'
        );
    },

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        //костыль, аспект используется в разных контроллерах, но вьюха одна
        //из-за этого на одни и те же селекторы повторно вешаются обработчики
        //ниже вьюха пересоздается, но привязка событий все равно остается,
        //возможно есть другое решение
        var ctxKey = controller.getCurrentContextKey(); //метод переопределен в контроллерах, использующих этот аспект
        //т.к. иначе привязка событий идет к элементам с тем id, который был на момент создания контроллера, и другие уже не работают

        actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']'] = { 'selectedgridrowactionhandler': { fn: me.selectedGridRowActionHandler, scope: me } };
        actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' [type=multiSelectGrid]'] = { 'select': { fn: me.onRowSelect, scope: me } };
        actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' [type=multiSelectedGrid]'] = { 'rowaction': { fn: me.selectedGridRowAction, scope: me } };
        actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' b4updatebutton'] = { 'click': { fn: me.updateSelectGrid, scope: me } };
        actions[me.distribPanelSelector + '[ctxKey=' + ctxKey + ']' + ' button[action=NextStep]'] = { 'click': { fn: me.validateDistributionObjects, scope: me } };
        actions[me.distribObjEditWindowSelector + '[ctxKey=' + ctxKey + ']' + ' button[action=Accept]'] = { 'click': { fn: me.acceptDistribution, scope: me } };
        actions[me.distribObjEditWindowSelector + '[ctxKey=' + ctxKey + ']' + ' [name=DistrSum]'] = { 'change': { fn: me.changeDistrSum, scope: me } };
        actions[me.distribObjEditWindowSelector + '[ctxKey=' + ctxKey + ']' + ' button[action=Distribute]'] = { 'click': { fn: me.distribute, scope: me } };

        me.otherActions(actions);

        me.on('validate', me.validate, me);

        controller.control(actions);
    },

    otherActions: function (actions) { },

    onBeforeLoad: function (store, operation) { },

    onSelectedBeforeLoad: function (store, operation) { },

    getForm: function () {
        var me = this,
            panel = me.componentQuery(me.distribPanelSelector);

        return panel;
    },

    getSelectGrid: function () {
        var me = this,
            win = me.componentQuery(me.distribPanelSelector);

        if (win) {
            return win.down('[type=multiSelectGrid]');
        }
    },

    getSelectedGrid: function () {
        var me = this,
            win = me.componentQuery(me.distribPanelSelector);

        if (win) {
            return win.down('[type=multiSelectedGrid]');
        }
    },

    getDistribGrid: function () {
        var me = this,
            win = me.componentQuery(me.distribObjEditWindowSelector);

        if (win) {
            return win.down('b4grid');
        }
    },

    selectedGridRowActionHandler: function (action, record) {
        var gridSelected = this.getSelectedGrid();
        if (gridSelected) {
            gridSelected.fireEvent('rowaction', gridSelected, action, record);
        }
    },

    selectedGridRowAction: function (grid, action, record) {
        switch (action.toLowerCase()) {
            case 'delete':
                grid.getStore().remove(record);
                break;
        }
    },

    updateSelectedGrid: function () {
        //Здесь событие beforeload вешается именно так с параметром single: true
        //Для того чтобы параметр передавался только 1 раз, поскольку есть места где необходимо
        //чтобы один и тот же стор принимал разные параметры, если не сделать single: true то получится каша
        //при первом вызове передастся один параметр при следующем вызове передастся другой параметр.
        //при такой схеме сейчас проблема возникает только при сортировке в гриде на колонке

        var grid = this.getSelectedGrid();
        if (grid) {
            grid.getStore().load();
        }
    },

    updateSelectGrid: function () {
        debugger;
        var grid = this.getSelectGrid();
        if (grid) {
            grid.getStore().load();
        }
    },

    onRowSelect: function (rowModel, record) {
        var me = this,
            grid = me.getSelectedGrid();
        if (grid && !grid.disabled) {
            var storeSelected = grid.getStore();

            if (storeSelected.find(me.idProperty, record.get(me.idProperty), 0, false, false, true) == -1) {
                storeSelected.add(record);
            }
        }
    },

    reconfigure: function (sum) {
        var me = this,
            stSelected,
            stSelect,
            headerplugin,
            panel = me.getForm(),
            selectGrid = panel.down('[type=multiSelectGrid]'),
            selectedGrid = panel.down('[type=multiSelectedGrid]'),
            selectedGridColumns,
            distrView = panel.down('[name=DistributionView]'),
            distrViewStore = distrView.getStore();

        if (panel.code != 'TransferCrDistribution') { //Платеж КР
            distrView.queryMode = 'local';

            var item = distrViewStore.findRecord('Value', B4.enums.SuspenseAccountDistributionParametersView.ByPaymentDocument);
            distrViewStore.remove(item);
        }

        if (panel.code === 'PerformedWorkActsDistribution' || //оплата акта
            panel.code === 'TransferContractorDistribution' || //распределение средств подрядчику
            panel.code === 'BuildControlPaymentDistribution' || //оплата стройконтроль
            panel.code === 'DEDPaymentDistribution' || //оплата ПСД
            panel.code === 'RestructAmicableAgreementDistribution') { //оплата по мировому соглашению

            distrView.queryMode = 'local';

            var record = distrViewStore.findRecord('Value', B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions);
            distrViewStore.remove(record);
        }

        me.initialSum = +sum;
        stSelected = me.storeSelected instanceof Ext.data.AbstractStore ? me.storeSelected : Ext.create('B4.store.' + me.storeSelected);

        //сортировка на клиенте
        Ext.apply(stSelected, {
            remoteSort: false
        });

        stSelected.on('beforeload', me.onSelectedBeforeLoad, me);

        stSelect = me.storeSelect instanceof Ext.data.AbstractStore ? me.storeSelect : Ext.create('B4.store.' + me.storeSelect);
        stSelect.on('beforeload', me.onBeforeLoad, me);

        stSelected.sorters.clear();
        stSelect.sorters.clear();

        //копируем колонки
        selectedGridColumns = Ext.Array.slice(me.columnsGridSelected, 0, me.columnsGridSelected.length);
        //добавляем колонку удаления
        selectedGridColumns.push({ xtype: 'b4deletecolumn' });

        selectGrid.plugins = [];
        selectGrid.plugins.push({ ptype: 'b4gridheaderfilters', pluginId: 'headerFilter' });
        selectGrid.constructPlugins();

        selectGrid.reconfigure(stSelect, me.columnsGridSelect);

        selectedGrid.reconfigure(stSelected, selectedGridColumns);

        headerplugin = Ext.create('B4.ux.grid.plugin.HeaderFilters', { pluginId: 'headerfilter' });
        headerplugin.init(selectGrid);
        selectGrid.plugins.push(headerplugin);
        selectGrid.getPlugin('headerfilter').renderFilters();
        selectGrid.down('b4pagingtoolbar').bindStore(stSelect);

        selectGrid.doLayout();
        selectGrid.getStore().load();

        B4.Ajax.request({
                url: B4.Url.action('GetOriginatorName', 'Distribution'),
                params: {
                    code: panel.code,
                    distributionId: panel.distributionId,
                    distributionIds: panel.distributionIds,
                    distributionSource: panel.src
                }
            })
            .next(function(response) {
                var resp = Ext.decode(response.responseText);
                me.setOriginatorName(resp.data);
            });
    },

    validateDistributionObjects: function (btn) {
        var me = this,
            win = btn.up(me.distribPanelSelector),
            selectGrid = me.getSelectGrid(),
            selectedGrid = me.getSelectedGrid(),
            selectStore = selectGrid.getStore(),
            selectedStore = selectedGrid.getStore(),
            distrField = win.down('[name="DistributionView"]'),
            distributionType = distrField.getValue(),
            distributeUsingFiltersField = win.down('[name="distributeUsingFilters"]'),
            distributeUsingFilters = distributeUsingFiltersField ? distributeUsingFiltersField.getValue() : false;
        
        if (distributionType === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions) {

            var periodStartDateField = win.down('[name=PeriodStartDate]'),
                periodEndDateField = win.down('[name=PeriodEndDate]');

                if (!periodStartDateField || !periodEndDateField) {
                    Ext.Msg.alert('Внимание', 'Для данного типа распределения данный вид распределения недоступен');
                    return;
                }

            var periodStartDate = periodStartDateField.getValue(),
                periodEndDate = periodEndDateField.getValue();

            if (!periodStartDate || !periodEndDate) {
                Ext.Msg.alert('Внимание', 'Необходимо выбрать период для расчета накопленных взносов');
                return;
            }
            else if (periodStartDate >= periodEndDate) {
                Ext.Msg.alert('Внимание', 'Дата начала периода не может быть больше даты окончания периода. Необходимо скорректировать даты');
                return;
            }
        }

        if ((!distributeUsingFilters && selectedStore.count() === 0) || (distributeUsingFilters && selectStore.count() === 0)) {
            Ext.Msg.alert('Внимание', 'Необходимо выбрать хотя бы 1 запись для распределения');
            return;
        }

        me.controller.mask('Распределение', win);
        B4.Ajax.request({
            url: B4.Url.action('Validate', 'Distribution'),
            method: 'POST',
            params: me.getObjectsUrlParams(),
            timeout: 2 * 60 * 60 * 1000 // 2 часа
        }).next(function () {
            me.continueSoftValidating(win);
        }).error(function (res) {
            var msg = 'Ошибка при распределении!';
            try {
                msg = res.message;
            } catch (e) {

            }
            me.controller.unmask();
            Ext.Msg.alert('Ошибка', msg);
        });
    },

    continueSoftValidating: function (win) {
        var me = this,
            params = me.getObjectsUrlParams();

        B4.Ajax.request({
            url: B4.Url.action('SoftValidate', 'Distribution'),
            method: 'POST',
            params: params,
            timeout: 2 * 60 * 60 * 1000 // 2 часа
        }).next(function () {
            me.controller.unmask();
            me.showDistributionObjects(win);
        }).error(function (res) {
            me.controller.unmask();
            Ext.Msg.confirm('Внимание', (res.message || 'Ошибка при распределении!') + ' Применить распределение?', function (result) { if (result === 'yes') { me.showDistributionObjects(win); } });
        });
    },

    showDistributionObjects: function (panel) {
        var me = this,
            manuallyDistrib = true,
            distributeOn = panel.down('[name=DistributeOn]'),
            tab = B4.getBody().getActiveTab(),
            win = me.controller.getCmpInContext(me.distribObjEditWindowSelector),
            grid,
            distrSumFld;

        if (win) {
            win.close();
        }

        win = Ext.widget(me.distribObjEditWindowSelector, {
            constrain: true,
            closeAction: 'destroy',
            ctxKey: me.controller.getCurrentContextKey ? me.controller.getCurrentContextKey() : '',
            distributionId: panel.distributionId,
            distributionIds: panel.distributionIds,
            src: panel.src,
            gridStore: me.distribObjStore,
            gridColumns: me.distribObjColumnsGrid,
            code: panel.code,
            enableCellEdit: true,
            isAutoChangeDistrSum: false,
            distributeOn: distributeOn ? distributeOn.getValue() : null,
            renderTo: tab.getEl()
        });

        tab.add(win);
        win.show();

        grid = win.down('b4grid[type=distribObjects]');
        distrSumFld = grid.down('[name=DistrSum]');
        distrSumFld.setValue(me.initialSum);

        me.controller.mask('Распределение', win);

        var params = me.getObjectsUrlParams();

        var accountNumField = me.controller.getMainView().down('b4selectfield[name="AccountNum"]');
        if (accountNumField) {
            if (accountNumField.getValue()) {
                params.bankAccNum = true;
            }
        }

        B4.Ajax.request({
            url: B4.Url.action('ListDistributionObjs', 'Distribution'),
            method: 'POST',
            params: params,
            timeout: 1000 * 60 * 60
        }).next(function (response) {
            var decoded = Ext.decode(response.responseText),
                store,
                balanceFld,
                sum = 0,
                sumPenalty = 0,
                cellEdit;

            if (!decoded.success) {
                me.controller.unmask();
                Ext.Msg.alert('Ошибка', decoded.message);
                win.close();
                return;
            }

            tab.add(win);

            balanceFld = grid.down('[name=Balance]');
            balanceFld.setValue(manuallyDistrib ? me.initialSum : 0);

            cellEdit = grid.getPlugin('cellEdit');

            cellEdit.on('beforeedit', function (e) {
                return !e.disabled;
            });

            cellEdit.on('edit', function (editor, args) {
                var rest = parseFloat(balanceFld.getValue()),
                    orig = parseFloat(args.originalValue),
                    value = parseFloat(args.value);

                rest = !isNaN(rest) ? rest : 0;
                orig = !isNaN(orig) ? orig : 0;
                value = !isNaN(value) ? value : 0;

                if (orig === value) {
                    return;
                }

                rest = rest + orig - value;

                win.isAutoChangeDistrSum = true;

                balanceFld.setValue(rest.toFixed(2));
                distrSumFld.setValue(me.initialSum - rest.toFixed(2));

                switch (args.field) {
                    case 'Sum':
                        editor.view.store.proxy.data[args.record.get('Index')].Sum = value;
                        break;
                    case 'SumPenalty':
                        editor.view.store.proxy.data[args.record.get('Index')].SumPenalty = value;
                        break;
                }

                // не работает, нужно только для того чтобы пометка о изменении записи пропала
                editor.view.store.sync();
            });

            store = grid.getStore();

            Ext.Array.each(decoded.data, function (rec, index) {
                rec.Index = index;
                sum += +rec.Sum;
                sumPenalty += +rec.SumPenalty;
            });

            balanceFld.setValue(me.initialSum
                - (!isNaN(sum) ? sum : 0)
                - (!isNaN(sumPenalty) ? sumPenalty : 0));

            distrSumFld.lastValue = (!isNaN(sum) ? sum : 0)
                + (!isNaN(sumPenalty) ? sumPenalty : 0);
            distrSumFld.setValue(distrSumFld.lastValue);

            store.removeAll();

            store.setProxy(
                Ext.create('Ext.ux.data.PagingMemoryProxy', {
                    enablePaging: true,
                    data: decoded.data
                }));

            store.load();
            me.controller.unmask();
        }).error(function (e) {
            me.controller.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при распределении!');
            win.close();
        });
    },

    changeDistrSum: function (fld, newValue, oldValue) {
        var me = this,
            editWindow = me.controller.getCmpInContext(me.distribObjEditWindowSelector),
            balanceFld = editWindow.down('[name=Balance]'),
            grid,
            store,
            forFirstCheck = oldValue == undefined ? undefined : Number(oldValue.toFixed(2));
      
        if (editWindow.isAutoChangeDistrSum) 
        {
            editWindow.isAutoChangeDistrSum = false;
            return;
        }
        else if (forFirstCheck == newValue)
        {
            balanceFld.setValue(me.initialSum - newValue);

            grid = editWindow.down('b4grid[type=distribObjects]');

            store = grid.getStore();
            store.proxy.data = [];
            store.removeAll();
        }
        else if (oldValue !== undefined)
        {
            balanceFld.setValue(me.initialSum - newValue);

            grid = editWindow.down('b4grid[type=distribObjects]');

            store = grid.getStore();
            store.proxy.data = [];
            store.removeAll();

            editWindow.down('button[action=Accept]').disable();
        }
    },

    getObjectsUrlParams: function () {
        var me = this,
            win = me.getForm(),
            storeSelected = me.getSelectedGrid().getStore(),
            distrFld,
            val = [],
            params = {},
            distrWindow = me.controller.getCmpInContext(me.distribObjEditWindowSelector),
            distrSumFld = distrWindow ? distrWindow.down('[name=DistrSum]') : null,
            distrSum = distrSumFld ? distrSumFld.getValue() : this.initialSum,
            distributeUsingFiltersField = win.down('[name="distributeUsingFilters"]'),
            distributeUsingFilters = distributeUsingFiltersField ? distributeUsingFiltersField.getValue() : false,
            crFundTypesField = me.getSelectGrid().down('b4selectfield[name=crFundTypes]'),
            roIdsField = me.getSelectGrid().down('b4selectfield[name=accountNum]'),
            programCrField = win.down('[name=ProgramCr]');

        distrFld = win.down('[name=DistributionView]');

        if (storeSelected) {
            storeSelected.each(function (rec) {
                val.push(rec.get('Id'));
            });
        };

        if (distrFld.getValue() === B4.enums.SuspenseAccountDistributionParametersView.ProportionallyToOwnersContributions) {
            params.periodStartDate = win.down('[name=PeriodStartDate]').getValue();
            params.periodEndDate = win.down('[name=PeriodEndDate]').getValue();
        }

        if (crFundTypesField && crFundTypesField.getValue()) {
            params.crFundTypes = Ext.JSON.encode(crFundTypesField.getValue());
        }

        if (roIdsField && roIdsField.getValue()) {
            params.roIds = roIdsField.getValue();
        }

        if (programCrField && programCrField.getValue()) {
            params.programCr = programCrField.getValue();
        }

        params.ids = val;
        params.code = win.code;
        params.distributeUsingFilters = distributeUsingFilters;
        params.complexFilter = Ext.encode(me.getSelectGrid().getHeaderFilters());
        params.distributionType = distrFld ? distrFld.getValue() : null;
        params.distributionId = win.distributionId;
        params.distributionIds = win.distributionIds;
        params.distribSum = distrSum;
        params.distributionSource = win.src;

        return params;
    },

    getSuspenseAccountSum: function (win, id) {
        var record,
            fld = win.down('[name=Balance]');

        B4.Ajax.request({
            url: B4.Url.action('Get', 'SuspenseAccount'),
            method: 'GET',
            params: { id: id }
        }).next(function (response) {
            record = Ext.decode(response.responseText);
            fld.setValue(record.data.Sum);
        }).error(function () {
        });
    },

    acceptDistribution: function (btn) {
        var me = this,
            grid = btn.up('b4grid[type=distribObjects]'),
            form = grid.up(me.distribObjEditWindowSelector),
            store = grid.getStore(),
            balanceField = grid.down('[name=Balance]'),
            distrSumField = grid.down('[name=DistrSum]'),
            params = me.getApplyUrlParams(form, store);

        if (distrSumField.getValue() <= 0) {
            Ext.Msg.alert('Ошибка', 'Распределяемая сумма должна быть больше нуля');
            return;
        }

        if (balanceField.getValue() < 0) {
            Ext.Msg.alert('Распределение!', 'В поле остаток - неверное значение!');
            return;
        }

        if (!grid.store.proxy.data || grid.store.proxy.data.length == 0) {
            Ext.Msg.alert('Ошибка', 'После изменения распределяемой суммы необходимо повторить распределение');
            return;
        }

        if (me.fireEvent('beforeaccept', me) !== false) {
            me.controller.mask('Распределение', form);

            B4.Ajax.request({
                url: B4.Url.action('SoftApplyValidate', 'Distribution'),
                method: 'POST',
                params: params,
                timeout: 1 * 60 * 60 * 1000 // 1 час
            }).next(function () {
                me.apply(grid);
            }).error(function (res) {
                me.controller.unmask();
                Ext.Msg.confirm('Внимание', (res.message || 'Ошибка при распределении!') + ' Применить распределение?', function (result) { if (result === 'yes') { me.apply(grid); } });
            });
        }
    },

    apply: function (grid) {
        var me = this,
            form = grid.up(me.distribObjEditWindowSelector),
            store = grid.getStore(),
            distrWindow = grid.up('window');

        form.submit({
            url: B4.Url.action('Apply', 'Distribution'),
            params: me.getApplyUrlParams(form, store),
            timeout: 2 * 60 * 60 * 1000, // 2 часа,
            success: function (f, action) {
                var resp = Ext.decode(action.response.responseText);
                me.controller.unmask();
                distrWindow.close();
                me.getForm().close();
                Ext.Msg.alert('Результат', resp.message || 'Распределение выполнено успешно!');
            },
            failure: function (f, action) {
                var json = Ext.JSON.decode(action.response.responseText);
                Ext.Msg.alert('Результат', json.message || 'Во время распределения произошла ошибка');
                me.controller.unmask();
            }
        });
    },

    getApplyUrlParams: function (win, store) {
        var records = [],
            distrSumFld = win.down('[name=DistrSum]'),
            distrSum = distrSumFld ? distrSumFld.getValue() : this.initialSum;

        Ext.each(store.proxy.data, function (item) {
            records.push(item);
        });

        return {
            code: win.code,
            distributionId: win.distributionId,
            distributionIds: win.distributionIds,
            distributionSource: win.src,
            distribSum: distrSum,
            distributeOn: win.distributeOn,
            records: Ext.encode(records)
        };
    },

    distribute: function () {
        var me = this,
            editWindow = me.controller.getCmpInContext(me.distribObjEditWindowSelector),
            grid = editWindow.down('b4grid[type=distribObjects]'),
            balanceField = grid.down('[name=Balance]');

        if (balanceField.getValue() < 0) {
            Ext.Msg.alert('Ошибка', 'Распределяемая сумма не может быть больше остатка');
            return;
        }

        me.controller.mask('Обновление', editWindow);

        var params = me.getObjectsUrlParams();
        var accountNumField = me.controller.getMainView().down('b4selectfield[name="AccountNum"]');
        if (accountNumField) {
            if (accountNumField.getValue()) {
                params.bankAccNum = true;
            }
        }

        B4.Ajax.request({
            url: B4.Url.action('ListDistributionObjs', 'Distribution'),
            method: 'POST',
            params: params,
            timeout: 1000 * 60 * 60
        }).next(function (response) {
            var decoded = Ext.decode(response.responseText),
                store,
                grid;

            if (!decoded.success) {
                me.controller.unmask();
                Ext.Msg.alert('Ошибка', decoded.message);
                return;
            }

            grid = editWindow.down('b4grid[type=distribObjects]');

            store = grid.getStore();

            Ext.Array.each(decoded.data, function (rec, index) {
                rec.Index = index;
            });

            store.proxy.data = decoded.data;
            store.load();

            editWindow.down('button[action=Accept]').enable();

            me.controller.unmask();

        }).error(function (e) {
            me.controller.unmask();
            Ext.Msg.alert('Ошибка', e.message || 'Ошибка при распределении!');
        });
    },

    setOriginatorName: function(data) {
        
    }
});