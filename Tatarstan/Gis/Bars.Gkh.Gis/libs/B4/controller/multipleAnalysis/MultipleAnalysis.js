Ext.define('B4.controller.multipleAnalysis.MultipleAnalysis', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    views: [
        'multipleAnalysis.Grid',
        'multipleAnalysis.Window'
    ],
    mainView: 'multipleAnalysis.Grid',
    mainViewSelector: 'multipleAnalysisGrid',

    refs: [
        {
            ref: 'Win',
            selector: 'multipleAnalysisGrid multipleAnalysisWindow'
        }
    ],

    init: function () {
        var me = this;

        this.control({
            'multipleAnalysisGrid b4updatebutton': {
                'click': function () {
                    me.getMainView().getStore().reload();
                },
                scope: me
            },
            'multipleAnalysisGrid b4addbutton': {
                'click': me.onAddTemplateClick,
                scope: me
            },
            'multipleAnalysisGrid': {
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                },
                'itemdblclick': {
                    fn: me.rowDblClick,
                    scope: me
                }
            },
            'multipleAnalysisWindow': {
                'afterrender': {
                    fn: me.windowRender,
                    scope: me
                }
            },
            'multipleAnalysisWindow treepanel': {
                'cellclick': {
                    fn: me.treeCellClick,
                    scope: me
                },
                'changeConfirmed': {
                    fn: me.changeConfirmed,
                    scope: me
                }
            },
            'multipleAnalysisWindow button[name=Form]': {
                'click': {
                    fn: me.onFormClick,
                    scope: me
                }
            },
            'multipleAnalysisWindow b4savebutton': {
                'click': {
                    fn: me.onSaveClick,
                    scope: me
                }
            },
            //'multipleAnalysisWindow b4selectfield[name=MunicipalArea]': {
            //    change: {
            //        fn: function (control, opts) {
            //            me.loadSettlement(control, opts);
            //            me.loadStreet(control, opts);
            //        }
            //    }
            //},
            //'multipleAnalysisWindow b4selectfield[name=Settlement]': {
            //    change: {
            //        fn: me.loadStreet
            //    }
            //}
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('multipleAnalysisGrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    },

    rowAction: function (grid, action, record) {
        var me = this;

        if (!grid || grid.isDestroyed) return;
        if (action.toLowerCase() == 'edit' && me.fireEvent('beforerowaction', me, grid, action, record) !== false) {
            me.editRecord(record);
        } else if (action.toLowerCase() == 'delete') {
            me.deleteRecord(record);
        }
    },

    rowDblClick: function (view, record) {
        if (!view || view.isDestroyed) return;
        this.editRecord(record);
    },

    onAddTemplateClick: function () {
        this.editRecord(undefined);
    },

    editRecord: function (record) {
        var me = this,
            view = me.getMainView(),
            win = me.getWin();

        if (!win) {
            win = Ext.widget('multipleAnalysisWindow', {
                editedRecord: record,
                width: view.getWidth() * 0.9,
                height: view.getHeight() * 0.9
            });
        }
        win.show();
    },

    deleteRecord: function (record) {
        var me = this,
            view = me.getMainView();

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result !== 'yes') return;

            me.mask('Удаление', B4.getBody());

            B4.Ajax.request({
                url: B4.Url.action('Delete', 'MultipleAnalysisTemplate'),
                method: 'GET',
                timeout: 999999,
                params: {
                    id: record.get('Id')
                }
            })
            .next(function (response) {
                var resp = Ext.decode(response.responseText);
                if (resp.success) {
                    view.getStore().reload();
                } else {
                    B4.QuickMsg.msg('Ошибка', resp.message, 'warning');
                }
                me.unmask();
            })
            .error(function () {
                B4.QuickMsg.msg('Ошибка при удалении', 'Не удалось удалить шаблон!', 'error');
                me.unmask();
            });
        }, me);
    },

    windowRender: function (win) {
        var //combo = win.down('combobox[name=FormDay]'),
            //numberStore = Ext.create('Ext.data.Store', {
            //    fields: ['Day']
            //}),
            array = [],
            record = win.editedRecord;

        win.down('treepanel').getStore().load();
        for (var i = 1; i <= 28; i++) {
            array.push({ Day: i });
        }
        //numberStore.loadData(array);
        //combo.bindStore(numberStore);

        if (record) {
            win.down('gistreeselectfield[name=HouseType]').setValue([{
                EntityId: record.get('RealEstateTypeId'),
                Id: record.get('RealEstateTypeId'),
                Name: record.get('RealEstateTypeName'),
                text: record.get('RealEstateTypeName')
            }]);
            win.down('b4monthpicker[name=MonthYear]').setValue(record.get('MonthYear'));
            win.down('combobox[name=TypeCondition]').setValue(record.get('TypeCondition'));
            //combo.setValue(record.get('FormDay'));
            //win.down('textfield[name=EMail]').setValue(record.get('Email'));
            win.down('textfield[name=EMail]').hide();

            if (record.get('MunicipalAreaGuid') && record.get('MunicipalAreaName')) {
                win.down('b4selectfield[name=MunicipalArea]').setValue({
                    Name: record.get('MunicipalAreaName'),
                    Id: record.get('MunicipalAreaGuid')
                });
            }

            //if (record.get('SettlementGuid') && record.get('SettlementName')) {
            //    win.down('b4selectfield[name=Settlement]').setValue({
            //        Name: record.get('SettlementName'),
            //        Id: record.get('SettlementGuid')
            //    });
            //}

            //if (record.get('StreetGuid') && record.get('StreetName')) {
            //    win.down('b4selectfield[name=Street]').setValue({
            //        Name: record.get('StreetName'),
            //        Id: record.get('StreetGuid')
            //    });
            //}
        }
    },

    treeCellClick: function () {
        var me = this,
            view = me.getMainView();

        me.setContextValue(view, 'lastCellClickArguments', arguments);
    },

    changeConfirmed: function (record, column) {
        var me = this,
            view = me.getMainView(),
            args = me.getContextValue(view, 'lastCellClickArguments'),
            tree = column.up('treepanel');

        if (column.dataIndex == 'DeviationPercent') {
            record.set('MinValue', null);
            record.set('MaxValue', null);
            record.set('ExactValue', null);
        } else if (column.dataIndex == 'ExactValue') {
            record.set('MinValue', null);
            record.set('MaxValue', null);
            record.set('DeviationPercent', null);
        } else {
            record.set('ExactValue', null);
            record.set('DeviationPercent', null);
        }

        tree.getView().fireEvent('cellclick', args[0], args[1], args[2], args[3], args[4], args[5], args[6]);
    },

    onFormClick: function (button) {
        var me = this,
            win = button.up('window'),
            typeHouse = win.down('gistreeselectfield[name=HouseType]').getValue(),
            typeCondition = win.down('combobox[name=TypeCondition]').getValue(),
            date = win.down('b4monthpicker').getValue(),
            indicatorArray = me.getSelectedIndicators(win.down('treepanel').getStore()),
            municipalArea = win.down('b4selectfield[name=MunicipalArea]').getValue();
            //settlement = win.down('b4selectfield[name=Settlement]').getValue(),
            //street = win.down('b4selectfield[name=Street]').getValue();

        if (indicatorArray.length === 0) {
            B4.QuickMsg.msg('Формирование', 'Выберите хотя бы один индикатор', 'warning');
            return;
        }
        if (!typeHouse) {
            B4.QuickMsg.msg('Формирование', 'Выберите тип дома', 'warning');
            return;
        }
        if (!typeCondition) {
            B4.QuickMsg.msg('Формирование', 'Выберите условие', 'warning');
            return;
        }
        if (!date) {
            B4.QuickMsg.msg('Формирование', 'Выберите месяц', 'warning');
            return;
        }

        win.setLoading('Формирование...');

        B4.Ajax.request({
            url: B4.Url.action('GetReport', 'MultipleAnalysisTemplate'),
            method: 'POST',
            timeout: 999999,
            params: {
                typeHouse: typeHouse,
                typeCondition: typeCondition,
                date: date,
                indicators: Ext.encode(indicatorArray),
                municipalArea: municipalArea
                //settlement: settlement,
                //street: street
            }
        })
        .next(function (response) {
            var resp = Ext.decode(response.responseText);
            if (resp.success) {
                me.updateGrid(resp, win);
            } else {
                B4.QuickMsg.msg('Внимание', 'Множественный анализ не дал результата!', 'warning');
            }
            win.setLoading(false);
        })
        .error(function () {
            win.setLoading(false);
            B4.QuickMsg.msg('Ошибка при формировании', 'Не удалось провести множественный анализ!', 'error');
        });
    },

    getSelectedIndicators: function (store) {
        var array = [],
            root = store.getRootNode();

        Ext.iterate(root.childNodes, function (indiGroup) {
            Ext.iterate(indiGroup.childNodes, function (indi) {
                if (indi.get('MinValue') != undefined || indi.get('MaxValue') != undefined
                    || indi.get('DeviationPercent') != undefined || indi.get('ExactValue') != undefined) {
                    array.push(indi.data);
                }
            });
        });
        return array;
    },

    onSaveClick: function (button) {
        var me = this,
            view = me.getMainView(),
            win = button.up('window'),
            id = win.editedRecord ? win.editedRecord.get('Id') : 0,
            typeHouse = win.down('gistreeselectfield[name=HouseType]').getValue(),
            typeCondition = win.down('combobox[name=TypeCondition]').getValue(),
            //formDay = win.down('combobox[name=FormDay]').getValue(),
            email = win.down('textfield[name=EMail]').getValue(),
            indicatorArray = me.getSelectedIndicators(win.down('treepanel').getStore()),
            municipalArea = win.down('b4selectfield[name=MunicipalArea]').getValue(),
            monthYear = win.down('b4monthpicker[name=MonthYear]').getValue();
            //settlement = win.down('b4selectfield[name=Settlement]').getValue(),
            //street = win.down('b4selectfield[name=Street]').getValue();

        if (!win.down('form').getForm().isValid()) {
            B4.QuickMsg.msg('Сохранение', 'Заполните правильно все обязательные поля!', 'warning');
            return;
        }
        if (indicatorArray.length === 0) {
            B4.QuickMsg.msg('Формирование', 'Выберите хотя бы один индикатор', 'warning');
            return;
        }

        win.setLoading('Сохранение...');

        B4.Ajax.request({
            url: B4.Url.action('SaveTemplate', 'MultipleAnalysisTemplate'),
            method: 'POST',
            timeout: 999999,
            params: {
                record: Ext.encode({
                    id: id,
                    typeHouse: typeHouse[0],
                    typeCondition: typeCondition,
                    //formDay: formDay,
                    email: email,
                    indicators: indicatorArray,
                    municipalArea: municipalArea,
                    monthYear: monthYear
                    //settlement: settlement,
                    //street: street
                })
            }
        })
        .next(function (response) {
            var resp = Ext.decode(response.responseText);
            if (resp.success) {
                win.close();
                view.getStore().reload();
            } else {
                B4.QuickMsg.msg('Сохранение', resp.message, 'error');
            }
            win.setLoading(false);
        })
        .error(function () {
            win.setLoading(false);
            B4.QuickMsg.msg('Ошибка при cохранении', 'Не удалось сохранить шаблон!', 'error');
        });
    },

    updateGrid: function (resp, win) {
        var me = this,
            grid = win.down('grid[name=AnalysisResult]'),
            columns = resp.columns,
            data = resp.data,
            fields = new Array(),
            totalHouseCount = resp.totalHouseCount;

        fields.push(new Ext.data.Field({ name: 'Id' }));
        fields.push(new Ext.data.Field({ name: 'Address' }));

        grid.columns = new Array();
        grid.columns.push(Ext.create('Ext.grid.column.Column', {
            text: 'Адрес',
            dataIndex: 'Address',
            width: 300,
            minWidth: 100,
            locked: true,
            menuDisabled: true,
            renderer: function (value) {
                return Ext.util.Format.trim(value);
            }
        }));

        for (var i = 0; i < columns.length; i++) {
            grid.columns.push(Ext.create('Ext.grid.column.Number', {
                text: columns[i].text,
                format: '0.00',
                dataIndex: columns[i].dataIndex,
                flex: 1,
                minWidth: 80,
                menuDisabled: true,
                locked: false,
                renderer: function (value) {
                    var newValue = parseFloat(value);
                    if (isNaN(newValue)) return value;
                    return parseFloat(value).toFixed(2);
                }
            }));
            fields.push(new Ext.data.Field({ name: columns[i].dataIndex, type: 'string' }));
        }

        var store = Ext.create('Ext.data.Store', { fields: [] });
        store.model.setFields(fields);
        store.add(data);
        grid.reconfigure(store, grid.columns);
        grid.setTitle(Ext.String.format('Результат {0}/{1} (Найдено домов/Всего домов данного типа)', store.getCount(), totalHouseCount));
        grid.setDisabled(false);
        me.unmask();
    },

    ////Загрузка населенного пункта
    //loadSettlement: function (control, opts) {
    //    var view = control.up('multipleAnalysisWindow'),
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
    //        beforeload: function (store, operation) {
    //            operation.params.ParentGuid = control.getValue();
    //        },
    //        load: function (store) {
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
    //loadStreet: function (control, opts) {
    //    var view = control.up('multipleAnalysisWindow'),
    //        municipalAreaField = view.down('b4selectfield[name=MunicipalArea]'),
    //        streetField = view.down('b4selectfield[name=Street]');

    //    streetField.setValue();

    //    //поле очищено
    //    if (!opts) {
    //        streetField.disable();
    //        return;
    //    }

    //    streetField.getStore().on({
    //        beforeload: function (store, operation) {
    //            operation.params.PlaceGuid = control.getValue()
    //                ? control.getValue()
    //                : municipalAreaField.getValue();
    //        },
    //        load: function (store) {
    //            if (store.totalCount == 0) {
    //                streetField.disable();
    //            } else {
    //                streetField.enable();
    //            }
    //        }
    //    });

    //    streetField.getStore().load();
    //}
});