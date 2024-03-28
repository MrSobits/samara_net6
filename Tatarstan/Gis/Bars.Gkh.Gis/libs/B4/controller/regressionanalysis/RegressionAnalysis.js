Ext.define('B4.controller.regressionanalysis.RegressionAnalysis', {
    extend: 'B4.base.Controller',
    mixins: { context: 'B4.mixins.Context' },

    views: [
        'regressionanalysis.Panel',
        'regressionanalysis.Chart'
    ],
    mainView: 'regressionanalysis.Panel',
    mainViewSelector: 'regressionanalysispanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'regressionanalysispanel'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'regressionanalysispanel': {
                'afterrender': {
                    fn: me.onRender,
                    scope: me
                }
            },

            'regressionanalysispanel button[name=MakeChart]': {
                'click': function(button) {
                    me.loadOrClearChart();
                }
            },

            'regressionanalysispanel b4monthpicker': {
                'change': {
                    fn: me.onMonthpickerChange,
                    scope: me
                }
            },
            'regressionanalysispanel gistreeselectfield[name=IndicatorSelectField]': {
                'change': {
                    fn: me.onIndicatorSelectFieldChange,
                    scope: me
                }
            },
            'regressionanalysispanel gistreeselectfield[name=HouseType]': {
                'change': {
                    fn: me.onHouseTypeChange,
                    scope: me
                }
            },
            'regressionanalysispanel b4selectfield[name=MunicipalArea]': {
                change: {
                    fn: me.onMunicipalChange,
                    scope: me
                }
            },
            //'regressionanalysispanel b4selectfield[name=Settlement]': {
            //    change: {
            //        fn: me.onSettlementChange,
            //        scope: me
            //    }
            //},
            //'regressionanalysispanel b4selectfield[name=Street]': {
            //    change: {
            //        fn: me.onStreetChange,
            //        scope: me
            //    },
            //    //custom event - см. view
            //    selectAll: {
            //        fn: me.onStreetButtonSelectAll,
            //        scope: me
            //    }
            //}
        });

        me.callParent(arguments);
    },

    index: function() {
        var me = this,
            view = me.getMainView() || Ext.widget('regressionanalysispanel');

        me.bindContext(view);
        me.application.deployView(view);
    },

    onRender: function(mainPanel) {
        var me = this,
            chartStore = mainPanel.down('chart[name=RegressionAnalazysChart]').getStore();
            //streetStore = mainPanel.down('b4selectfield[name=Street]').getStore();

        chartStore.on('beforeload', me.onChartStoreBefroeLoad, me);
        //streetStore.on('beforeload', me.onStreetStoreBeforeLoad, me);

    },
    
    //добавляем параметры в стор графика
    onChartStoreBefroeLoad: function(store, operation) {
        var me = this;

        if (!operation.params) {
            operation.params = {};
        }

        operation.params.houseType = me.getContextValue(me.getMainView(), 'houseType');
        operation.params.indicators = Ext.encode(me.getContextValue(me.getMainView(), 'indicators'));
        operation.params.dateBegin = me.getContextValue(me.getMainView(), 'dateBegin');
        operation.params.dateEnd = me.getContextValue(me.getMainView(), 'dateEnd');
        operation.params.areaGuid = me.getContextValue(me.getMainView(), 'areaGuid');
        //operation.params.placeGuid = me.getContextValue(me.getMainView(), 'placeGuid');
        //var streetGuid = me.getContextValue(me.getMainView(), 'streetGuid');
        //if (streetGuid) {
        //    operation.params.streetGuid = Ext.encode(streetGuid);
        //}
    },

    //onStreetStoreBeforeLoad: function(store,operation) {
    //    var me = this;
    //    me.setContextValue(me.getMainView(), 'streetFilter', operation.params.complexFilter);
    //    me.setContextValue(me.getMainView(), 'streetStoreParams', operation.params);
    //},
    
    //изменение даты
    onMonthpickerChange: function(monthPicker, newValue, oldValue) {
        var me = this,
            view = me.getMainView(),
            monthPickerDateBegin = monthPicker.up('container').down('b4monthpicker[name=dateBegin]'),
            monthPickerDateEnd = monthPicker.up('container').down('b4monthpicker[name=dateEnd]');

        if (!monthPickerDateBegin.getValue() || !monthPickerDateEnd.getValue()) {
            return;
        }

        if (monthPickerDateBegin > monthPickerDateEnd) {
            Ext.Msg.alert(
                'Внимание!',
                'Дата начала не может превышать дату окончания периода',
                'warning'
            );
            monthPicker.reset();
            return;
        }

        me.setContextValue(view, 'dateBegin', monthPickerDateBegin.getValue());
        me.setContextValue(view, 'dateEnd', monthPickerDateEnd.getValue());        
    },
    
    //выбор индикаторов
    onIndicatorSelectFieldChange: function(indicatorSelectField, newValue, oldValue, eOpts) {
        var me = this,
            chart = me.getMainView().down('chart'),
            store = chart.getStore(),
            model = store.getProxy().getModel(),
            indicators = indicatorSelectField.getValue() || [],
            onlyLeaf = newValue ?
                Ext.Array.map(newValue, function(i) {
                    if (i.leaf) {
                        return i[indicatorSelectField.idProperty];
                    }
                })
                : [];

        //отфильтровать индикаторы
        indicators = Ext.Array.intersect(indicators, onlyLeaf);

        //сохранить выбранные индикаторы
        me.setContextValue(me.getMainView(), 'indicators', newValue ? indicators : null);

        //изменить модель и линии
        var fields = ['month'];
        var series = [];
        Ext.each(indicators, function(i) {

            var indicatorNode = indicatorSelectField.store.getNodeById(i),
                indicatorTitle = indicatorNode ?
                    Ext.String.format('{0}({1})', indicatorNode.get('text'), indicatorNode.parentNode.get('text')) : '';

            fields.push({
                name: i
            });

            series.push(
                {
                    type: 'line',
                    axis: 'left',
                    highlight: {
                        size: 7,
                        radius: 7
                    },
                    xField: 'month',
                    yField: i,
                    title: indicatorTitle,
                    markerConfig: {
                        type: 'circle',
                        size: 4,
                        radius: 4,
                        'stroke-width': 0
                    },
                    tips: {
                        trackMouse: true,
                        width: 350,
                        height: 60,
                        renderer: function(storeItem, item) {
                            this.setTitle(
                                Ext.String.format('<b>{0}<br>{1}<br>{2}</b>',
                                    indicatorTitle, storeItem.get('month'), item.value[1])
                            );
                        }
                    }
                }
            );

        });

        //обновить модель
        model.setFields(fields);

        //заменить график
        me.replaceChart(me.createChart(series, indicators, store));        
    },
    
    //смена типа дома
    onHouseTypeChange: function(houseTypeSelectField, newValue, oldValue) {
        var me = this;
        me.setContextValue(me.getMainView(), 'houseType', houseTypeSelectField.getValue());        
    },
    

    //замена графика
    replaceChart: function(chart) {
        var me = this,
            view = me.getMainView(),
            oldChart = view.down('chart'),
            viewChartContainer = view.down('regressionanalysischart'),
            oldIndex = view.items.indexOf(oldChart);

        viewChartContainer.remove(oldChart);
        viewChartContainer.insert(oldIndex, chart);

    },
    
    //создание графика
    createChart: function(series, yAxesFields, store) {
        return Ext.widget('regressionanalysischartunit', {
            store: store,
            axes: [
                {
                    type: 'Numeric',
                    position: 'left',
                    minorTickSteps: 1,
                    fields: yAxesFields,
                    title: 'Значение индикатора',
                    label: {
                        rotate: {
                            degrees: 315
                        }
                    },
                    minimum: 0,
                    grid: {
                        odd: {
                            opacity: 1,
                            fill: '#ddd',
                            stroke: '#bbb',
                            'stroke-width': 0.5
                        }
                    }
                },
                {
                    type: 'Category',
                    position: 'bottom',
                    fields: ['month'],
                    title: 'Месяцы из выбранного периода'
                }
            ],
            series: series
        });
    },
    
    //загрузка стора графика
    loadOrClearChart: function() {
        var me = this,
            store = me.getMainView().down('chart').getStore();
        
        if (!me.getMainView().getForm().isValid()) {
            B4.QuickMsg.msg('Внимание!', 'Не заполнены обязательные поля', 'warning');
            return;
        }

        me.getMainView().setLoading(true);

        if (me.getContextValue(me.getMainView(), 'houseType')
            &&
            me.getContextValue(me.getMainView(), 'indicators')
            &&
            me.getContextValue(me.getMainView(), 'dateBegin')
            &&
            me.getContextValue(me.getMainView(), 'dateEnd')) {
            store.load(function() {
                me.getMainView().setLoading(false);
            });
        } else {
            store.removeAll();
            me.getMainView().setLoading(false);
        }
    },
    
    //смена МО
    onMunicipalChange: function(control, opts) {
        var me = this;
        //me.loadSettlement(control, opts);
        //me.loadStreet(control, opts);
        me.setContextValue(me.getMainView(), 'areaGuid', control.value != null ? control.getValue() : undefined);
    },
    
    ////смена нас. пункта
    //onSettlementChange: function(control, opts) {
    //    var me = this;
    //    me.loadStreet(control, opts);
    //    me.setContextValue(me.getMainView(), 'placeGuid', control.value != null ? control.getValue() : undefined);
    //},
    
    ////смена улицы
    //onStreetChange: function(control, opts) {
    //    var me = this;
    //    me.setContextValue(me.getMainView(), 'streetGuid', control.getValue() != null ? control.getValue() : undefined);
    //},
    
    ////Загрузка населенного пункта
    //loadSettlement: function(control, opts) {
    //    var view = control.up('regressionanalysispanel'),
    //        streetField = view.down('b4selectfield[name=Street]'),
    //        settlementField = view.down('b4selectfield[name=Settlement]');

    //    settlementField.setValue();
    //    streetField.setValue();

    //    streetField.disable();

    //    //поле очищено
    //    if (!opts) {
    //        settlementField.disable();
    //        return;
    //    }

    //    settlementField.getStore().on({
    //        beforeload: function(store, operation) {
    //            operation.params.ParentGuid = control.getValue();
    //        },
    //        load: function(store) {
    //            if (store.totalCount == 0) {
    //                settlementField.disable();
    //            } else {
    //                settlementField.enable();
    //            }
    //        }
    //    });

    //    settlementField.getStore().load();
    //},

    ////Загрузка улицы
    //loadStreet: function(control, opts) {
    //    var view = control.up('regressionanalysispanel'),
    //        municipalAreaField = view.down('b4selectfield[name=MunicipalArea]'),
    //        streetField = view.down('b4selectfield[name=Street]');

    //    streetField.setValue();

    //    //поле очищено
    //    if (!opts) {
    //        streetField.disable();
    //        return;
    //    }

    //    streetField.getStore().on({
    //        beforeload: function(store, operation) {
    //            operation.params.PlaceGuid = control.getValue()
    //                ? control.getValue()
    //                : municipalAreaField.getValue();
    //        },
    //        load: function(store) {
    //            if (store.totalCount == 0) {
    //                streetField.disable();
    //            } else {
    //                streetField.enable();
    //            }
    //        }
    //    });

    //    streetField.getStore().load();
    //},

    ////кнопка выбора Выбрать все в улицах
    //onStreetButtonSelectAll: function() {
    //    var me = this.getMainView().down('b4selectfield[name=Street]'),
    //        oldValue = me.getValue(),
    //        isValid = me.getErrors() != '';

    //    me.updateDisplayedText('Выбраны все');

    //    var params = this.getContextValue(this.getMainView(), 'streetStoreParams');
    //    params.Limit = 0;
    //    me.store.load({
    //        params: params,
    //        callback: function (records, operation, success) {
    //            if (success) {
    //                me.value = Ext.Array.map(records, function (i) { return i.data; });
                    
    //                me.fireEvent('validitychange', me, isValid);
    //                me.fireEvent('change', me, me.getValue(), oldValue);
    //                me.validate();

    //                me.selectWindow.hide();
    //            }
    //        }
    //    });        
    //}
});