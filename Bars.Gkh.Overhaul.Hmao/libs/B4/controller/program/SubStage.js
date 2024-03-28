Ext.define('B4.controller.program.SubStage', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.InlineGrid',
        'B4.view.program.thirddetails.CommonEstateGrid',
        'B4.view.program.thirddetails.WorkTypeGrid',
        'B4.aspects.GkhButtonImportAspect'
    ],

    models: ['dict.Municipality'],

    stores: [
        'CurrentPrioirityParams',
        'program.PriorityParam',
        'dict.municipality.ByParam'
    ],

    views: [
        'program.NewVersionWindow',
        'program.SubStagePanel',
        'program.SubStageGrid',
        'program.EditOrderWindow',
        'program.CurrentPriorityGrid',
        'program.thirddetails.ThirdStageDetails'
    ],

    refs: [
        { ref: 'mainPanel', selector: 'programsubstagepanel' },
        { ref: 'detailsPanel', selector: 'substagepanel' },
        { ref: 'commonestateTree', selector: 'substagepanel thirddetailscommonestatetree' },
        { ref: 'workTypeGrid', selector: 'substagepanel thirddetailsworktypegrid' },
        { ref: 'orderWin', selector: 'editorderwin' }
    ],

    aspects: [
        {
            xtype: 'inlinegridaspect',
            name: 'currPriorityAspect',
            storeName: 'CurrentPrioirityParams',
            modelName: 'CurrentPrioirityParams',
            gridSelector: 'currprioritygrid'
        },
        {
            xtype: 'gkhbuttonimportaspect',
            name: 'dpkrImportAspect',
            buttonSelector: 'programthirdstagegrid #btnImport',
            codeImport: 'DpkrImport',
            windowImportView: 'program.ImportWindow',
            windowImportSelector: 'programimportwin',
            maxFileSize: 120971520,
            listeners: {
                aftercreatewindow: function (window, importId) {
                    var chkBox = window.down('[name=AddStructEl]');
                    chkBox.setVisible(importId == 'Dpkr1cImport');
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Ovrhl.LongProgram.PriorParams.Create',
                    applyTo: 'b4addbutton',
                    selector: 'currprioritygrid'
                },
                {
                    name: 'Ovrhl.LongProgram.PriorParams.Edit',
                    applyTo: 'button[cmd="priority"]',
                    selector: 'currprioritygrid'
                },
                {
                    name: 'Ovrhl.LongProgram.PriorParams.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'currprioritygrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.LongProgram.Import_View',
                    applyTo: 'gkhbuttonimport',
                    selector: 'programsubstagegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.LongProgram.SaveVersion',
                    applyTo: 'button[action=NewVersion]',
                    selector: 'programsubstagegrid'
                },
                {
                    name: 'Ovrhl.LongProgram.SaveVersion_View',
                    applyTo: 'button[action=NewVersion]',
                    selector: 'programsubstagegrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    // Init
    init: function () {
        var me = this;
        me.control({
            'programsubstagegrid b4updatebutton': { click: { fn: me.update, scope: me } },

            'programsubstagegrid button[cmd="order"]': { click: { fn: me.validationSetPriority, scope: me } },

            'currprioritygrid button[cmd="priority"]': { click: me.applyPriority },

            'currprioritygrid b4closebutton': { click: me.closeOrderWin },

            'programsubstagegrid': {
                rowaction: { fn: me.showDetails, scope: me },
                destroy: function () { var win = me.getOrderWin(); if (win) { win.close(); } }
            },

            'programsubstagegrid button[action=NewVersion]': { click: me.showNewVersionWindow, scope: me },

            'programversionwin b4savebutton': { click: me.saveVersion, scope: me },

            'programsubstagegrid actioncolumn[action="hide"]': { click: { fn: this.hideRecord, scope: this } },

            'programversionwin b4closebutton': { click: me.closeVersionWin, scope: me },

            'programsubstagepanel combobox[name=Municipality]': {
                render: { fn: me.onRenderMunicipality, scope: me },
                select: { fn: me.onSelectMunicipality, scope: me }
            }
        });

        me.callParent(arguments);
    },

    // Action Index - shows LongProgram grid
    index: function () {
        var view = this.getMainPanel() || Ext.widget('programsubstagepanel');
        this.getAspect('dpkrImportAspect').loadImportStore();
        //this.getAspect('dpkr1cImportAspect').loadImportStore();

        this.bindContext(view);

        this.application.deployView(view);
    },

    onSelectMunicipality: function (field, records) {
        var me = this,
            value = records[0].getId(),
            label = field.up("programsubstagepanel").down("label[name=DateCalc]");

        field.store.clearFilter();

        field.up("programsubstagepanel").down('programsubstagegrid').getStore().load();

        me.updateDateCalcDpkr(label, value);
    },

    updateDateCalcDpkr: function (cmp, municipalityId) {
        B4.Ajax.request({
            url: B4.Url.action('GetDateCalcDpkr', 'ProgramVersion'),
            params: {
                muId: municipalityId
            },
            timeout: 9999999
        }).next(function (resp) {
            var data = Ext.decode(resp.responseText);

            var str = 'Дата расчета ' + (data ? data.dateStr : '');
            cmp.update(str);
        }).error(function () {

        });
    },

    onRenderMunicipality: function (field) {
        var store = field.getStore(),
            storeStage3 = this.getMainPanel().down('programsubstagegrid').getStore();

        store.on('load', this.onLoadMunicipality, this, { single: true });
        store.load();
        storeStage3.on('beforeload', this.onBeforeLoadStage3, this);
    },

    onLoadMunicipality: function (store, records) {
        var me = this,
            cmb;

        if (!Ext.isEmpty(records[0])) {
            cmb = me.getMainPanel().down('combobox[name="Municipality"]');
            cmb.setValue(records[0]);
            me.onSelectMunicipality(cmb, [records[0]]);
        }
    },

    onBeforeLoadStage3: function (store, operation) {
        var me = this,
            muField = me.getMainPanel().down('combobox[name=Municipality]');
        operation = operation || {};
        operation.params = operation.params || {};

        operation.params['muId'] = muField.getValue();
    },

    update: function (btn) {
        btn.up('programsubstagegrid').getStore().load();
    },

    saveChanges: function (btn) {
        var me = this,
            store = btn.up('programsubstagegrid').getStore(),
            modifiedCount = store.getModifiedRecords().length;

        if (modifiedCount > 0) {
            me.mask('Сохранение изменений...', me.getMainPanel());
            store.sync({
                callback: function () {
                    me.unmask();
                }
            });
        }
    },

    closeVersionWin: function (btn) {
        btn.up('programversionwin').close();
    },

    saveVersion: function (btn) {
        var me = this,
            mainPanel = me.getMainPanel(),
            muField = mainPanel.down('combobox[name=Municipality]'),
            win = btn.up('programversionwin'),
            form = win.getForm(),
            valid = form.isValid(),
            values;

        if (!valid) {
            return;
        }

        values = form.getValues();

        me.mask('Создание новой версии программы...', mainPanel);
        B4.Ajax.request({
            url: B4.Url.action('MakeNewVersion', 'ProgramVersion'),
            params: {
                Date: values.Date,
                Name: values.Name,
                IsMain: values.IsMain,
                muId: muField.getValue()
            },
            timeout: 9999999
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg("Сохранение", "Новая версия программы успешно создана.");
            win.close();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || "Ошибка при сохранении");
            //B4.QuickMsg.msg("Ошибка", e.message ? e.message : "Ошибка при сохранении", "error");
            win.close();
        });
    },

    showNewVersionWindow: function () {
        var panel = this.getMainPanel(),
            versionWin = Ext.widget('programversionwin');

        panel.add(versionWin);

        panel.on('destroy', function () {
            if (versionWin) {
                versionWin.destroy();
            }
        }, this, { single: true });

        versionWin.show();
    },

    validationSetPriority: function (btn) {
        var me = this;

        // Если просят расчитать только по Баллам, то пожалуста
        if (Gkh.config.Overhaul.OverhaulHmao.MethodOfCalculation == 20) {
            me.setPriority();
        } else {
            // иначе расчитываем по критериям, но перед этим показываем форму критериев
            me.editOrder(btn);
        }
    },

    editOrder: function () {
        var me = this,
            priorityBtn,
            win = me.getOrderWin() || Ext.widget('editorderwin');

        // если вдруг поменяют closeAction на hide
        priorityBtn = win.down('button[cmd="priorityAll"]');
        if (priorityBtn) {
            priorityBtn.cmd = 'priority';
        }

        me.bindContext(win);
        win.down('grid').getStore().load();
        win.show();
    },

    // метод отправляющий запрос на очередность серверу
    setPriority: function (data) {
        var me = this,
            mainPanel = me.getMainPanel(),
            muField = mainPanel.down('combobox[name=Municipality]');

        me.mask('Формирование очередности программы...', mainPanel);
        B4.Ajax.request({
            url: B4.Url.action('SetPriority', 'LongProgram'),
            params: {
                muId: muField.getValue(),
                records: data ? Ext.encode(data) : []
            },
            timeout: 9999999
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg("Сообщение", "Очередность успешно сформирована.");
            mainPanel.down('programsubstagegrid').getStore().load();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Не удалось сформировать очередность: ' + (e.message || e));
        });
    },

    // метод применения очередности с формы
    applyPriority: function (btn) {
        var me = this,
            grid = btn.up('grid'),
            store = grid.getStore(),
            valid = true,
            data;

        store.each(function (r) {
            valid = me.validatePriorityParam(r);
            return valid;
        });

        if (!valid) {
            B4.QuickMsg.msg("Ошибка", "Заполните добавленные записи", "error");
        } else {
            btn.up('window').close();
            data = Ext.Array.map(store.data.items, function (r) {
                return { Code: r.get('Code'), Order: r.get('Order') };
            });
            me.setPriority(data);
        }
    },

    validatePriorityParam: function (rec) {
        return !Ext.isEmpty(rec.get('Code')) && !Ext.isEmpty(rec.get('Order'));
    },

    closeOrderWin: function (btn) {
        btn.up('editorderwin').close();
    },

    showDetails: function (grid, action, record) {
        if (action === 'edit') {
            Ext.History.add(Ext.String.format("viewdetails/{0}", record.get('Id')));
        }
    },

    viewdetails: function (id) {
        var panel = this.getDetailsPanel() || Ext.widget('substagepanel', {
            objectId: id
        });

        this.bindContext(panel);
        this.application.deployView(panel);

        this.loadDetails(id);
        this.loadDetailsTree(id);
        this.getWorkTypeGrid().getStore().load({
            params: {
                st3Id: id
            }
        });
    },

    loadDetailsTree: function (id) {
        var tree = this.getCommonestateTree();
        B4.Ajax.request({
            url: B4.Url.action('ListDetails', 'RealityObjectStructuralElementInProgrammStage3'),
            params: { st3Id: id },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText);
            tree.setRootNode(data);
        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка', 'Произошла ошибка при получении списка КЭ', 'error');
        });
    },

    hideRecord: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        debugger;
        B4.Ajax.request({
            url: B4.Url.action('RemoveFromSubProgramm', 'ProgramVersion'),
            params: {
                stage3Id: rec.getId()
            },
        }).next(function (response) {
            grid.getStore().load();
        }).error(function (response) {
            Ext.Msg.alert('Ошибка', response.message);
        });
    },

    loadDetails: function (id) {
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetInfo', 'RealityObjectStructuralElementInProgrammStage3'),
            params: { st3Id: id },
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText);

            if (!data.ShowWorks) {
                me.getWorkTypeGrid().disable();
            }

            me.getDetailsPanel().getForm().setValues(data);
            me.getDetailsPanel().down('#thirddetailsinfolabel').update({ text: data.Address });
        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка', 'Произошла ошибка при получении данных по объекту', 'error');
        });
    },
});