Ext.define('B4.controller.ShortProgramDeficit', {
    extend: 'B4.base.Controller',
    requires: [],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [],
    stores: ['ShortProgramDeficit'],
    views: ['shortprogram.DeficitGrid'],

    mainView: 'shortprogram.DeficitGrid',
    mainViewSelector: 'shortprogramdeficitgrid',

    aspects: [],

    refs: [
        { ref: 'mainPanel', selector: 'shortprogramdeficitgrid' }
    ],
    
    init: function() {
        var me = this;

        var actions = {
            'shortprogramdeficitgrid button[action=CalcShortProgram]': { click: { fn: this.onCalcShortProgram, scope: this } },
            'shortprogramdeficitgrid button[action=OpenShortProgram]': { click: { fn: this.gotoShortProgram, scope: this } },
            'shortprogramdeficitgrid b4savebutton': {
                click: {
                    fn: this.save,
                    scope: me
                }
                
            },
            'shortprogramdeficitgrid b4updatebutton': {
                click: {
                    fn: this.refresh,
                    scope: me
                }
            }
        };

        me.control(actions);

        me.callParent(arguments);
    },

    refresh: function () {
        var view = this.getMainPanel();
        var me = this;
        window.vi = view;

        if (view) {
            var mainStore = view.getStore();

            mainStore.removeAll();

            me.mask();

            //TODO выделить в отдельный метод
            B4.Ajax.request(B4.Url.action('List', 'ShortProgramDeficit'))
                .next(function(response) {
                    me.unmask();

                    var result = Ext.decode(response.responseText),
                        data = result.data,
                        items = data.data;

                    Ext.each(items, function(item) {
                        var newRec = {
                            Id: item.Id,
                            Municipality: item.Municipality,
                            VersionId: item.VersionId
                        };

                        Ext.each(item.Data, function(rec) {
                            var year = rec.Year;
                            newRec['Def' + year] = rec.Deficit;
                            newRec['Sha' + year] = rec.Share;
                        });
                        mainStore.add(newRec);
                    });
                })
                .error(function(response) {
                });
        }
    },

    save: function(btn) {
        var me = this,
            grid = btn.up('shortprogramdeficitgrid'),
            startYear = grid.startYear,
            shortProgramYear = grid.shortProgramYear,
            items = grid.getStore().data.items,
            records = [];

        if (!me.validateGrid(grid)) {
            B4.QuickMsg.msg("Ошибка", "Проверьте правильность введенных данных!", "error");
            return;
        }

        Ext.each(items, function(item) {
            for (var i = startYear; i < parseInt(startYear) + parseInt(shortProgramYear); i++) {
                var newRec = {
                    MuId: item.get('Id'),
                    VersionId: item.get('VersionId')
                };

                newRec['Def'] = item.get('Def' + i);
                newRec['Sha'] = item.get('Sha' + i);
                newRec['Year'] = i;

                records.push(newRec);
            }
        });

        me.mask();
        B4.Ajax.request({
            url: B4.Url.action('SaveDeficit', 'ShortProgramDeficit'),
            params: {
                records: Ext.encode(records)
            }
        }).next(function(response) {
            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Изменения успешно сохранены', 'success');
        }).error(function(response) {
            me.unmask();
            B4.QuickMsg.msg("Ошибка", response.message, "error");
        });
    },
    
    onPromptKey: function (textField, e) {
        var me = this,
            blur;

        if (e.keyCode === Ext.EventObject.RETURN || e.keyCode === 10) {
            if (me.msgButtons.ok.isVisible()) {
                blur = true;
                me.msgButtons.ok.handler.call(me, me.msgButtons.ok);
            } else if (me.msgButtons.yes.isVisible()) {
                me.msgButtons.yes.handler.call(me, me.msgButtons.yes);
                blur = true;
            }

            if (blur) {
                me.textField.blur();
            }
        }
    },
    
    index: function() {
        var me = this,
            view = this.getMainPanel();

        if (!view) {
            B4.Ajax.request(B4.Url.action('List', 'ShortProgramDeficit'))
                .next(function(response) {
                    var result = Ext.decode(response.responseText),
                        data = result.data,
                        items = data.data,
                        startYear = data.startYear,
                        shortProgramYear = data.shortProgramYear,
                        fields = [{ name: 'Id' }, { name: 'Municipality' }, { name: 'VersionId' }],
                        store,
                        columns = [{ xtype: 'gridcolumn', dataIndex: 'Municipality', text: 'Муниципальное образование', flex: 1 }],
                        defColumns = [],
                        shaColumns = [],
                        renderer = function(value) {
                            return Ext.util.Format.currency(value);
                        },
                        editor = {
                            xtype: 'numberfield',
                            name: 'tt',
                            allowDecimals: true,
                            decimalPrecision: 2,
                            decimalSeparator: ',',
                            minValue: 0,
                            maxValue: 100,
                            hideTrigger: true,
                            enableKeyEvents: true,
                            listeners: {
                                keydown: function(field, e) {
                                    if (e.keyCode > 47 && e.keyCode < 57) { // цифры
                                        var decimalCount = (field.value.toString().split('.')[1] || []).length;

                                        if (decimalCount > 1) {
                                            e.preventDefault();
                                        }
                                    }
                                }
                            }
                        };

                    for (var i = startYear; i < parseInt(startYear) + parseInt(shortProgramYear); i++) {
                        fields.push({ name: 'Def' + i, defaultValue: 0 });
                        fields.push({ name: 'Sha' + i, defaultValue: 0 });

                        defColumns.push({ width: 150, flex: 1, dataIndex: 'Def' + i, text: i.toString(), renderer: renderer });
                        shaColumns.push({
                            width: 150,
                            flex: 1,
                            dataIndex: 'Sha' + i,
                            text: i.toString(),
                            renderer: renderer,
                            editor: editor,
                            tdCls: 'b-editable'
                        });
                    }

                    columns.push({ text: 'Дефицит финансирования капитального ремонта, руб.', flex: 1, columns: defColumns });
                    columns.push({ text: 'Доля финансирования дефицита из регионального бюджета, %', flex: 1, columns: shaColumns });

                    store = Ext.create('B4.store.ShortProgramDeficit', {
                        fields: fields,
                        remoteSort: false
                    });

                    Ext.each(items, function(item) {
                        var newRec = {
                            Id: item.Id,
                            Municipality: item.Municipality,
                            VersionId: item.VersionId
                        };

                        Ext.each(item.Data, function(rec) {
                            var year = rec.Year;
                            newRec['Def' + year] = rec.Deficit;
                            newRec['Sha' + year] = rec.Share;
                        });

                        store.add(newRec);
                    });

                    view = Ext.widget('shortprogramdeficitgrid', {
                        store: store,
                        columns: columns,
                        startYear: startYear,
                        shortProgramYear: shortProgramYear
                    });

                    me.bindContext(view);
                    me.application.deployView(view);
                })
                .error(function(response) {
                });
        }
    },
    
    /*
      Метод нажатия на кнопку 'Сформировать краткосрочную программу'
    */
    onCalcShortProgram: function (btn) {
        var me = this,
            grid = btn.up('shortprogramdeficitgrid');

        me.mask('Формирование краткосрочной программы', grid);
            
        B4.Ajax.request({
            url: B4.Url.action('CreateShortProgram', 'ShortProgramDeficit'),
            method: 'POST',
            timeout: 9999999
        }).next(function (res) {
            me.unmask();
            Ext.Msg.alert('Формирвоание краткосрочной программы!', 'Расчет краткосрочной программы произведен успешно');
            
            Ext.History.add('shortprogram');
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка формирование краткосрочной программы!', e.message);
            });
    },
    
    validateGrid: function (grid) {
        /*
        *   Проходим по строкам.
        *   Далее, используя editor каждой колонки, валидируем данные в ячейках
        */
        var me = this, view = grid.getView(), isValid = true;
        var columnLength = grid.columns.length,
            totalColumnLength = 0;
        grid.getStore().each(function (record, idx) {
            for (var j = 0; j < columnLength; j++) {
                var subColumns = grid.columns[j].getGridColumns();
                for (var c = 0, len = subColumns.length; c < len; ++c) {
                    ++totalColumnLength;
                    var cell = view.getCellByPosition({ row: idx, column: totalColumnLength });
                    cell.removeCls("x-form-invalid-field");

                    var dataIndex = subColumns[c].dataIndex;
                    if (dataIndex) {
                        var value = record.get(dataIndex);

                        var errors = me.validateCellValue(subColumns[c], value);
                        if (!Ext.isEmpty(errors)) {
                            isValid = false;
                            cell.addCls("x-form-invalid-field");
                            cell.set({ 'data-errorqtip': errors.join('<br>') });
                        }
                    }
                }
            }
        });
        return isValid;
    },
    
    validateCellValue: function (column, value) {
        var errors = [];
        if (!column) {
            return [];
        }

        var editor = column.getEditor();
        if (!editor) {
            return [];
        }
        
        if (editor.allowBlank && !Ext.isDefined(value)) {
            errors.push('Значение не может быть пустым!');
        }
        
        if (Ext.isDefined(editor.minValue) && Ext.isDefined(editor.maxValue) && (value < editor.minValue || value > editor.maxValue)) {
            errors.push(Ext.String.format('Значение должно быть в промежутке от {0} до {1}', editor.minValue, editor.maxValue));
        }

        return errors;
    }
    
    //gotoShortProgram: function () {
    //    Ext.History.add('shortprogram');
    //}
});