Ext.define('B4.controller.dict.stateduty.Edit', {
    extend: 'B4.base.Controller',

    views: [
        'dict.stateduty.EditPanel',
        'dict.stateduty.PetitionGrid',
        'dict.stateduty.FormulaPanel',
        'dict.stateduty.FormulaSelectWindow'
    ],
    
    stores: [
        'dict.StateDuty',
        'dict.StateDutyPetition'
    ],
    
    models: [
        'StateDuty',
        'dict.StateDutyPetition',
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

    mainGrid: 'statedutygrid',

    refs: [
        { ref: 'mainView', selector: 'statedutyeditpanel' },
        { ref: 'paramGrid', selector: 'statedutyformulapanel b4grid' }
    ],

    init: function () {
        var me = this;

        me.control({
            'statedutyeditpanel b4savebutton': {
                'click': function() {
                    me.onSave();
                }
            },
            'statedutyeditpanel statedutypetitiongrid': {
                'rowaction': function(g, action, record) {
                    switch(action.toLowerCase()) {
                        case 'delete':
                            g.getStore().remove(record);
                            break;
                    }
                }
            },
            'statedutyeditpanel statedutypetitiongrid b4addbutton': {
                'click': function() {
                    me.onAddPetition();
                }
            },
            'statedutyeditpanel statedutyformulapanel b4addbutton': {
                'click': function() {
                    me.onEditParameter(null);
                }
            },
            'statedutyeditpanel statedutyformulapanel b4grid': {
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
            'statedutyeditpanel button[action="checkformula"]': {
                click: function () {
                    me.onCheckFormula();
                }
            }
        });

        me.callParent(arguments);
    },

    index: function (id) {
        var me = this,
            mainView = me.getMainView() || Ext.widget('statedutyeditpanel');

        me.setContextValue('dutyId', id);
        me.bindContext(mainView);
        me.application.deployView(mainView);

        me.loadRecord(id, mainView);
    },

    loadRecord: function (id, panel) {
        var me = this,
            model = me.getModel('StateDuty');
        if (id == 0) {
            panel.down('statedutyformulapanel').setDisabled(true);
            panel.down('statedutypetitiongrid').setDisabled(true);
        }
        if (+id) {
            model.load(id, {
                success: function (rec) {
                    var petitionStore = panel.down('statedutypetitiongrid').getStore(),                        
                        paramStore = panel.down('statedutyformulapanel b4grid').getStore(),
                        parameters;
                    
                    panel.getForm().loadRecord(rec);

                    petitionStore.on('beforeload', function (s, operation) {
                        operation.params['dutyId'] = rec.get('Id');
                    });

                    petitionStore.load();

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

    onAddPetition: function () {
        this.getPetitionAddWin().show();
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
            win = Ext.widget('formulaselectwindow', {
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

    getPetitionAddWin: function () {
        var me = this,
            editPanel = me.getMainView(),
            grid = editPanel.down('statedutypetitiongrid'),
            editForm = editPanel.getForm(),
            win = Ext.create('B4.form.SelectWindow', {
                selectionMode: 'MULTI',
                textProperty: 'ShortName',
                store: Ext.create('B4.store.dict.PetitionToCourt', {
                    listeners: {
                        beforeload: function (store, opts) {
                            opts.params.dutyId = editForm.getRecord().get('Id');
                        }
                    }
                }),
                modal: true,
                selectWindowCallback: function (data) {
                    var id, records;
                    if (!data || data.length === 0) {
                        return;
                    }

                    id = editForm.getRecord().get('Id');
                    records = Ext.Array.map(data, function (rec) {
                        return {
                            StateDuty: id,
                            PetitionToCourtType: rec.Id
                        };
                    });

                    me.mask();
                    B4.Ajax.request({
                        url: B4.Url.action('Create', 'StateDutyPetition'),
                        params: { records: Ext.JSON.encode(records) }
                    }).next(function () {
                        me.unmask();
                        grid.getStore().load();
                        B4.QuickMsg.msg('Успешно', 'Типы заявлений успешно добавлены', 'success');
                    }).error(function () {
                        me.unmask();
                        B4.QuickMsg.msg('Ошибка', 'При добавлении типов заявлений произошла ошибка', 'error');
                    });
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
                B4.QuickMsg.msg('Успешно', 'Госпошлина успешно сохранена', 'success');
                
                if (!id) {
                    B4.getBody().getActiveTab().close();
                    Ext.History.add('stateduty_edit/' + result.record.get('Id'));
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