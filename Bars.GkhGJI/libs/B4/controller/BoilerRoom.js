Ext.define('B4.controller.BoilerRoom', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.mixins.Context',
        'B4.aspects.GridEditWindow',
        'B4.view.boilerroom.EditWindow',
        'B4.aspects.InlineGrid'
    ],

    views: ['boilerroom.Grid', 'boilerroom.EditWindow'],

    model: [
        'boilerroom.BoilerRoom',
        'boilerroom.HeatingPeriod',
        'boilerroom.UnactivePeriod'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    refs: [
        { ref: 'mainView', selector: 'boilerroomgrid' },
        { ref: 'heatingPeriodGrid', selector: 'boilerroomedit b4grid[name=HeatingPeriod]' },
        { ref: 'unactivePeriodGrid', selector: 'boilerroomedit b4grid[name=UnactivePeriod]' },
        { ref: 'boilerRoomIdField', selector: 'boilerroomedit hidden[name=Id]' },
        { ref: 'municipalityField', selector: 'boilerroomedit textfield[name=Municipality]' }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            gridSelector: 'boilerroomgrid',
            editFormSelector: 'boilerroomedit',
            modelName: 'boilerroom.BoilerRoom',
            editWindowView: 'boilerroom.EditWindow',
            listeners: {
                'aftersetformdata': function (asp, record) {
                    var ctrl = asp.controller,
                        unactPeriodGrid,
                        heatPeriodGrid;
                    if (record.getId() > 0) {
                        unactPeriodGrid = ctrl.getUnactivePeriodGrid();
                        heatPeriodGrid = ctrl.getHeatingPeriodGrid();

                        unactPeriodGrid.getStore().on('beforeload', ctrl.onBeforePeriodStoreLoad, ctrl);
                        heatPeriodGrid.getStore().on('beforeload', ctrl.onBeforePeriodStoreLoad, ctrl);
                        unactPeriodGrid.getStore().on('beforesync', ctrl.onBeforePeriodGridSync, ctrl);
                        heatPeriodGrid.getStore().on('beforesync', ctrl.onBeforePeriodGridSync, ctrl);
                        unactPeriodGrid.on('edit', ctrl.onUnactivePeriodGridEdit, ctrl);
                        heatPeriodGrid.on('edit', ctrl.onHeatingPeriodGridEdit, ctrl);

                        unactPeriodGrid.setDisabled(false);
                        heatPeriodGrid.setDisabled(false);

                        unactPeriodGrid.getStore().load();
                        heatPeriodGrid.getStore().load();

                    }
                },
                'beforesave': function (asp) {
                    var ctrl = asp.controller;
                    if (!ctrl.validatePeriods(ctrl.getUnactivePeriodGrid().getStore())) {
                        Ext.Msg.alert('Ошибка', 'Таблица "Период не активности" содержит ошибки');
                        return false;
                    } else if (!ctrl.validatePeriods(ctrl.getHeatingPeriodGrid().getStore())) {
                        Ext.Msg.alert('Ошибка', 'Таблица "Период подачи тепла" содержит ошибки');
                        return false;
                    }

                    ctrl.savePeriods();
                    return true;
                }
            }
        },
        {
            xtype: 'inlinegridaspect',
            modelName: 'boilerroom.HeatingPeriod',
            gridSelector: 'boilerroomedit b4grid[name=HeatingPeriod]'
        },
        {
            xtype: 'inlinegridaspect',
            modelName: 'boilerroom.UnactivePeriod',
            gridSelector: 'boilerroomedit b4grid[name=UnactivePeriod]'
        }
    ],

    init: function () {
        var me = this;
        me.control({
            'boilerroomedit b4fiasselectaddress': {
                'change': me.onFiasAddressChange
            }
        });
        me.callParent(arguments);
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('boilerroomgrid');
        this.bindContext(view);
        this.application.deployView(view);
    },

    onBeforePeriodStoreLoad: function (store, operation) {
        operation.params = operation.params || {};
        operation.params.boilerRoomId = this.getBoilerRoomIdField().getValue();
    },

    validatePeriods: function (periodsStore) {
        var invalidRecord;

        invalidRecord = periodsStore.findRecord('IsValid', false);
        if (invalidRecord) {
            return false;
        }

        return true;
    },

    onUnactivePeriodGridEdit: function (editor, e) {
        this.validatePeriodGridRow(e.row, e.record, e.field, e.value);
    },

    validatePeriodGridRow: function (row, record, field, value) {
        var me = this,
            startDate = null,
            endDate = null,
            unactivePeriodsStore,
            heatingPeriodsStore,
            isValid = true;


        record.set('IsValid', true);

        switch (field) {
            case 'Start':
                startDate = value;
                endDate = record.get('End');
                break;
            case 'End':
                startDate = record.get('Start');
                endDate = value;
        }

        if (!startDate && !endDate) { // Оба не заполнены
            record.set('IsValid', false);
            me.markRowInvalid(row);
        }

        if (startDate && endDate && startDate.getTime() > endDate.getTime()) { // Начало периода > конец периода
            record.set('IsValid', false);
            me.markRowInvalid(row);
            return;
        }

        unactivePeriodsStore = me.getUnactivePeriodGrid().getStore();

        unactivePeriodsStore.each(function (rec) { // Пересечение с другими периодами не активности
            if (rec !== record) {
                isValid = isValid && !me.intersection(startDate, endDate, rec.get('Start'), rec.get('End'));
            }
            return isValid;
        });

        if (!isValid) {
            record.set('IsValid', false);
            me.markRowInvalid(row);
        }

        heatingPeriodsStore = me.getHeatingPeriodGrid().getStore();

        heatingPeriodsStore.each(function (rec) { // Пересечение с другими периодами подачи тепла
            if (rec !== record) {
                isValid = isValid && !me.intersection(startDate, endDate, rec.get('Start'), rec.get('End'));
            }
            return isValid;
        });

        if (!isValid) {
            record.set('IsValid', false);
            me.markRowInvalid(row);
        }
    },

    onHeatingPeriodGridEdit: function (editor, e) {
        var me = this;

        me.validatePeriodGridRow(e.row, e.record, e.field, e.value);

        if (e.field === 'Start' && !e.value) {
            record.set('IsValid', false);
            me.markRowInvalid(e.row);
        }
    },

    markRowInvalid: function (rowEl) {
        rowEl.className = rowEl.className + ' back-coralred';
    },

    savePeriods: function () {
        var me = this;
        me.getUnactivePeriodGrid().getStore().sync();
        me.getHeatingPeriodGrid().getStore().sync();
    },

    intersection: function (start1, end1, start2, end2) {
        // if startN == null then startN = Date.MinValue
        // if endN == null then endN = Date.MaxValue
        var me = this,
            result = false;
        if (start1) {
            result = me.between(start1, start2, end2);
        }
        if (end1) {
            result = result || me.between(end1, start2, end2);
        }
        if (start2) {
            result = result || me.between(start2, start1, end1);
        }
        if (end2) {
            result = result || me.between(end2, start1, end1);
        }
        return result;
    },

    between: function (date, start, end) {
        var result = false;
        if (!date) {
            throw new Error('[' + Ext.getDisplayName(arguments.callee) + '] "date" argument can not be null or undefined');
        }
        if (start && end) {
            result = Ext.Date.between(date, start, end);
        } else if (start) {
            result = date.getTime() >= start.getTime();
        } else if (end) {
            result = date.getTime() < end.getTime();
        }
        return result;
    },

    onBeforePeriodGridSync: function (options) {
        var me = this,
            boilerRoomId = me.getBoilerRoomIdField().getValue(),
            createArr = options.create;


        if (createArr) {
            Ext.Array.each(createArr, function (rec) {
                rec.set('BoilerRoom', boilerRoomId);
            });
        }
    },

    onFiasAddressChange: function (field, newVal) {
        var me = this;
        if (newVal) {
            Ext.Ajax.request({
                method: 'GET',
                url: B4.Url.action('/Municipality/GetByPlaceGuid'),
                params: { placeGuidId: newVal.PlaceGuidId },
                success: function (response) {
                    try {
                        var resp = Ext.JSON.decode(response.responseText);
                        if (resp) {
                            me.getMunicipalityField().setValue(resp.data);
                        }
                    } catch (e) {
                        throw new Error('[' + Ext.getDisplayName(arguments.callee) + '] Ошибка при получении муниципального образования');
                    } 
                }
            });
        }
    }
});