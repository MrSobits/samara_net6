Ext.define('B4.controller.cscalculation.CSFormulaEdit', {
    extend: 'B4.base.Controller',

    views: [
        'cscalculation.EditPanel',
        'cscalculation.FormulaPanel',
        'cscalculation.FormulaSelectWindow'
    ],
    
    stores: [
        'cscalculation.CSFormula'
    ],
    
    models: [
        'cscalculation.CSFormula',
        'FormulaParameter'
    ],

    requires: [
        'B4.mixins.MaskBody',
        'B4.mixins.Context',
        'B4.Ajax', 'B4.Url',
        'B4.ux.grid.Panel'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainGrid: 'cscalculationformulagrid',

    refs: [
        { ref: 'mainView', selector: 'cscalculationeditpanel' },
        { ref: 'paramGrid', selector: 'cscalculationformulapanel b4grid' }
    ],

    init: function () {
        var me = this;

        me.control({
            'cscalculationeditpanel b4savebutton': {
                'click': function() {
                    me.onSave();
                }
            },
            'cscalculationeditpanel cscalculationformulapanel b4addbutton': {
                'click': function() {
                    me.onEditParameter(null);
                }
            },
            'cscalculationeditpanel cscalculationformulapanel b4grid': {
                'rowaction': function (g, action, record) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            me.onEditParameter(record);
                            break;
                        case 'delete':
                            g.getStore().remove(record);
                            break;
                    }
                }
            },
            'cscalculationeditpanel button[action="checkformula"]': {
                click: function () {
                    me.onCheckFormula();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            mainView = me.getMainView() || Ext.widget('cscalculationeditpanel');

        me.setContextValue('dutyId', id);
        me.bindContext(mainView);
        me.application.deployView(mainView);

        me.loadRecord(id, mainView);
    },

    loadRecord: function (id, panel) {
        var me = this,
            model = me.getModel('cscalculation.CSFormula');
        if (id == 0) {
            panel.down('cscalculationformulapanel').setDisabled(true);
        }
        if (+id) {
            model.load(id, {
                success: function (rec) {
                    var paramStore = panel.down('cscalculationformulapanel b4grid').getStore(),
                        parameters;
                    
                    panel.getForm().loadRecord(rec);

                    paramStore.removeAll();

                    parameters = rec.get('FormulaParameters');
                    
                    if (parameters) {
                        Ext.each(parameters, function (p) {
                            paramStore.add(p);
                        });
                    }
                }
            });
        } else {
            panel.getForm().loadRecord(new model());
        }
    },

    onEditParameter: function(record) {
        var me = this,
            win = me.getFormulaEditWin(),
            model = me.getModel('FormulaParameter');

        if (record) {
            win.loadRecord(record);
        } else {
            win.loadRecord(new model());
        }

        win.show();
    },

    onCheckFormula: function() {
        var me = this,
            panel = me.getMainView(),
            formula = panel.down('[name="Formula"]').getValue(),
            msgFld = panel.down('[name="FormulaMsg"]');

        B4.Ajax.request({
            url: B4.Url.action('CheckFormula', 'Formula'),
            params: { formula: formula }
        }).next(function (resp) {
            var json = Ext.JSON.decode(resp.responseText);
            msgFld.setValue(json.message);
        }).error(function (resp) {
            var json = Ext.JSON.decode(resp.responseText);
            msgFld.setValue(json.message);
        });
    },

    getFormulaEditWin: function () {
        var me = this,
            win = Ext.widget('cscalculationformulaselectwindow', {
                constrain: true,
                renderTo: B4.getBody().getActiveTab().getEl(),
                ctxKey: me.getCurrentContextKey(),
                listeners: {
                    saverequest: function (w, record) {
                        var store = me.getParamGrid().getStore(),
                            existRec = store.findRecord('Code', record.get('Code')),
                            existingId;

                        if (existRec && existRec.get('Name') != record.get('Name')) {
                            B4.QuickMsg.msg('Ошибка', 'Параметр с указанной характеристикой уже добавлен', 'error');
                            return;
                        }

                        existingId = store.find('Name', record.get('Name'));

                        if (existingId > -1) {
                            store.removeAt(existingId);
                        }

                        store.add(record);
                    }
                }
            });

        return win;
    },    
    
    onSave: function() {
        var me = this,
            panel = me.getMainView(),
            form = panel.getForm(),
            record, id;

        form.updateRecord();
        record = form.getRecord();

        record.set('FormulaParameters', me.getFormulaParameters());

        id = record.get('Id');

        me.mask('Сохранение', panel);

        record.save({ id: id })
            .next(function (result) {
                me.unmask();
                B4.QuickMsg.msg('Успешно', 'Формула успешно сохранена', 'success');
                
                if (!id) {
                    B4.getBody().getActiveTab().close();
                    Ext.History.add('cscalculationformula_edit/' + result.record.get('Id'));
                }
            })
            .error(function (result) {
                me.unmask();
                Ext.Msg.alert('Ошибка сохранения!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
            });
    },
    
    getFormulaParameters: function () {
        var paramGrid = this.getParamGrid();

        return Ext.Array.map(paramGrid.getStore().getRange(),
            function(rec) {
                return {
                    Name: rec.get('Name'),
                    DisplayName: rec.get('DisplayName'),
                    Code: rec.get('Code')
                };
            });
    }
});