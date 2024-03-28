Ext.define('B4.controller.program.ThirdStage', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    requires: [
        'B4.Ajax', 'B4.Url',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.InlineGrid',
        'B4.view.program.thirddetails.CommonEstateGrid',
        'B4.view.program.thirddetails.WorkTypeGrid',
        'B4.view.program.CopyPricesWindow'
    ],

    stores: [
        'CurrentPrioirityParams',
        'program.PriorityParam',
        'dict.MunicipalityByOperator',
        'B4.store.program.ThirdStage'
    ],

    views: [
        'program.NewVersionWindow',
        'program.ThirdStagePanel',
        'program.ThirdStageGrid',
        'program.EditOrderWindow',
        'program.CurrentPriorityGrid',
        'program.thirddetails.ThirdStageDetails',
        'program.CopyPricesWindow'
    ],

    refs: [
        { ref: 'mainPanel', selector: 'programthirdstagepanel' },
        { ref: 'thirdStageGrid', selector: 'programthirdstagegrid' },
        { ref: 'detailsPanel', selector: 'thirdstagepanel' },
        { ref: 'commonestateTree', selector: 'thirdstagepanel thirddetailscommonestatetree' },
        { ref: 'workTypeGrid', selector: 'thirdstagepanel thirddetailsworktypegrid' }
    ],

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            gridSelector: 'programthirdstagegrid',
            buttonSelector: 'programthirdstagegrid button[action=Export]',
            controllerName: 'RealityObjectStructuralElementInProgrammStage3',
            actionName: 'Export'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'currPriorityAspect',
            storeName: 'CurrentPrioirityParams',
            modelName: 'CurrentPrioirityParams',
            gridSelector: 'currprioritygrid'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.LongProgram.Edit', applyTo: 'b4savebutton', selector: 'thirdstagepanel' },
                { name: 'Ovrhl.LongProgram.PriorParams.Create', applyTo: 'b4addbutton', selector: 'currprioritygrid' },
                { name: 'Ovrhl.LongProgram.ProgramVersion.Create', applyTo: 'button[action="NewVersion"]', selector: 'programthirdstagegrid' },
                { name: 'Ovrhl.LongProgram.PriorParams.Create', applyTo: 'b4addbutton', selector: 'currprioritygrid' },
                { name: 'Ovrhl.LongProgram.PriorParams.Edit', applyTo: 'button[cmd="priority"]', selector: 'currprioritygrid' },
                { name: 'Ovrhl.LongProgram.PriorParams.Delete',
                    applyTo: 'b4deletecolumn',
                    selector: 'currprioritygrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    // Init
    init: function() {
        var me = this;
        me.control({
            'programthirdstagegrid b4updatebutton': { click: { fn: me.update, scope: me } },
            'programthirdstagegrid b4savebutton': { click: { fn: me.saveChanges, scope: me } },
            'currprioritygrid': {
                render: me.afterPriorityParamGridRender
            },
            'currprioritygrid button[cmd="priority"]': { click: me.applyPriority, scope: me },
            'currprioritygrid b4closebutton': { click: me.closeOrderWin, scope: me },
            
            'programthirdstagegrid': { rowaction: { fn: this.showDetails, scope: this } },
            
            'programthirdstagegrid button[action=NewVersion]': { click: me.showNewVersionWindow, scope: this },
            
            'programversionwin b4savebutton': { click: me.saveVersion, scope: this },
            
            'programversionwin b4closebutton': { click: me.closeVersionWin, scope: this },
            
            'thirdstagepanel b4savebutton': { click: { fn: this.onSaveRequestHandler, scope: this } },
            
            'thirdstagepanel thirddetailscommonestatetree': { beforeedit: me.onBeforeEditOoiGrid, scope: this },
            
            'programthirdstagegrid b4combobox[name="Municipality"]': {
                change: { fn: this.changeMunicipality, scope: this },
                render: { fn: this.renderMuField, scope: this }
            },
            
            'programthirdstagegrid button[name="CopyPrices"]': {
                click: { fn: this.clickCopyPricesBtn, scope: this }
            },
            
            'copypriceswin b4savebutton': {
                click: { fn: this.clickSaveBtn, scope: this }
            }
        });
        
        me.callParent(arguments);
    },

    // Action Index - shows LongProgram grid
    index: function (id) {
        var view = this.getMainPanel();

        if (!view) {
            view = Ext.widget('programthirdstagepanel');

            view.down('programthirdstagegrid').getStore().on('beforeload', this.onBeforeLoadThirdStage, this);
            
            this.bindContext(view);
            this.application.deployView(view);
        }

        view.down('hidden[name=MunicipalityId]').setValue(id);
    },

    onLoadMunicipality: function (store, records) {
        var me = this,
            panel = me.getMainPanel(),
            cmb = panel.down('b4combobox[name="Municipality"]'),
            muId = panel.down('hidden[name=MunicipalityId]').getValue(),
            countRecords = store.getCount(),
            record;

        if (countRecords > 0) {
            if (muId) {
                record = store.findRecord('Id', muId, false, true, true);
            }

            cmb.setValue(record ? record.getData() : records[0].data);
        }
    },

    renderThirdStageGrid: function (grid) {
        grid.getStore().on('beforeload', this.onBeforeLoadThirdStage, this);
    },

    renderMuField: function (field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me);
        store.load();
    },
    
    changeMunicipality: function () {
        var me = this,
            panel = me.getThirdStageGrid();

        panel.getStore().load();
    },
    
    onBeforeLoadThirdStage: function (store, operation) {
        if (!operation.params) {
            operation.params = {};
        }

        operation.params.municipalityId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
    },
    
    update: function (btn) {
        btn.up('programthirdstagegrid').getStore().load();
    },
    
    saveChanges: function (btn) {
        var me = this,
            store = btn.up('programthirdstagegrid').getStore(),
            modifiedCount = store.getModifiedRecords().length;

        if (modifiedCount > 0) {
            me.mask('Сохранение изменений...', me.getMainPanel());
            store.sync({
                callback: function() {
                    me.unmask();
                }
            });
        }
    },
    
    closeVersionWin: function(btn) {
        btn.up('programversionwin').close();
    },
    
    saveVersion: function (btn) {
        var me = this,
            win = btn.up('programversionwin'),
            form = win.getForm(),
            valid = form.isValid();

        if (!valid) {
            return;
        }

        var values = form.getValues();
        values["Municipality"] = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
        
        me.mask('Создание новой версии программы...', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('MakeNewVersion', 'RealityObjectStructuralElementInProgrammStage3'),
            params: values,
            timeout: 9999999
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg("Сохранение", "Новая версия программы успешно создана.", 'success');
            win.close();
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg("Ошибка", e.message, "error");
            win.close();
        });
    },
    
    showNewVersionWindow: function (btn) {
        var win = Ext.widget('programversionwin');
        win.title += ' для МО: ' + this.getMainPanel().down('b4combobox[name="Municipality"]').getRawValue();
        
        win.show();
    },

    afterPriorityParamGridRender: function(grid) {
        grid.getStore().load();
    },

    applyPriority: function(btn) {
        var me = this,
            grid = btn.up('grid'),
            store = grid.getStore(),
            valid = true,
            data;
        
        store.each(function(r) {
            valid = me.validatePriorityParam(r);
            return valid;
        });

        if (!valid) {
            B4.QuickMsg.msg("Ошибка", "Заполните добавленные записи", "error");
        } else {
            data = Ext.Array.map(store.data.items, function(r) {
                return { Code: r.get('Code'), Order: r.get('Order') };
            });
            btn.up('window').close();

            me.mask('Формирование очередности программы...', me.getMainPanel());
            B4.Ajax.request({
                url: B4.Url.action('SetPriority', 'RealityObjectStructuralElementInProgrammStage3'),
                params: {
                    records: Ext.encode(data)
                },
                timeout: 9999999
            }).next(function() {
                me.unmask();
                B4.QuickMsg.msg("Сохранение", "Приоритет успешно сохранен.");
                me.getMainPanel().down('programthirdstagegrid').getStore().load();
            }).error(function (result) {
                me.unmask();
                var message = "Ошибка при сохранении";
                if (!Ext.isEmpty(result) && !Ext.isEmpty(result.message)) {
                    message = result.message;
                }
                B4.QuickMsg.msg("Ошибка", message, "error");
            });
        }
    },

    validatePriorityParam: function(rec) {
        return !Ext.isEmpty(rec.get('Code')) && !Ext.isEmpty(rec.get('Order'));
    },

    closeOrderWin: function(btn) {
        btn.up('editorderwin').close();
    },
    
    showDetails: function(grid, action, record) {
        if (action === 'edit') {
            Ext.History.add(Ext.String.format("viewdetails/{0}", record.get('Id')));
        }
    },
    
    viewdetails: function(id) {
        var panel = this.getDetailsPanel() || Ext.widget('thirdstagepanel', {            
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
    
    loadDetailsTree: function(id) {
        var tree = this.getCommonestateTree();
        B4.Ajax.request({
            url: B4.Url.action('ListDetails', 'RealityObjectStructuralElementInProgrammStage3'),
            params: { st3Id: id },
            timeout: 9999999
        }).next(function(response) {
            var data = Ext.JSON.decode(response.responseText);
            tree.setRootNode(data);
        }).error(function(e) {
            B4.QuickMsg.msg('Ошибка', 'Произошла ошибка при получении списка КЭ', 'error');
        });
    },
    
    loadDetails: function (id) {
        var me = this,
            panel = me.getDetailsPanel();
        
        me.mask('Загрузка значений...', panel);
        
        B4.Ajax.request({
            url: B4.Url.action('GetInfo', 'RealityObjectStructuralElementInProgrammStage3'),
            params: { st3Id: id },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            var data = Ext.JSON.decode(response.responseText);
            me.getDetailsPanel().getForm().setValues(data);
            me.getDetailsPanel().down('#thirddetailsinfolabel').update({ text: data.Address });
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', 'Произошла ошибка при получении данных по объекту', 'error');
        });
    },
    
    onBeforeEditOoiGrid: function (editor, e) {
        if (e.record.get('leaf')) {
            return true;
        }
        
        return false;
    },
    
    /*
      Метод нажатия на кнопку 'Сохранить изменения'
    */
    onSaveRequestHandler: function (btn) {
        var me = this,
            panel = btn.up('thirdstagepanel'),
            recordGrid = panel.down('thirddetailscommonestatetree'),
            store = recordGrid.getStore(),
            modifiedsData = [];

        if (store.getModifiedRecords().length == 0) {
            return;
        }
        
        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push({
                Id: rec.getId(),
                WorkSum: rec.get('WorkSum'),
                ServiceSum: rec.get('ServiceSum')
            });
        });
        
        me.mask('Сохранение изменений...', panel);
        
        B4.Ajax.request({
            url: B4.Url.action('UpdateWorkSum', 'RealityObjectStructuralElementInProgrammStage3'),
            method: 'POST',
            timeout: 9999999,
            params: {
                objectId: panel.objectId,
                records: Ext.JSON.encode(modifiedsData)
            }
        }).next(function () {
            me.unmask();
            
            me.loadDetailsTree(panel.objectId);
            me.loadDetails(panel.objectId);

            B4.QuickMsg.msg('Сообщение', 'Сохранение изменений прошло успешно', 'success');
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', 'При сохранении изменений произошла ошибка', 'error');
        });
        
    },
    
    clickCopyPricesBtn: function () {
        var win = Ext.widget('copypriceswin');

        if (win) {
        win.show();
            win.down('b4selectfield').getStore().on('beforeload', this.onBeforeLoadVersion, this);
        }
    },
    
    onBeforeLoadVersion: function (store, operation) {
        operation.params.municipalityId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
    },
    
    clickSaveBtn: function(btn) {
        var me = this,
            win = btn.up('copypriceswin'),
            form = win.getForm(),
            valid = form.isValid(),
            store = me.getMainPanel().down('programthirdstagegrid').getStore();

        if (!valid) {
            B4.QuickMsg.msg("Предупреждение", "Не заполнены обязательные поля", "warning");
            return;
        }

        var values = form.getValues();
        values["Municipality"] = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();

        me.mask('Копирование стоимостей из версии...', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('CopyFromVersion', 'RealityObjectStructuralElementInProgrammStage3'),
            params: values,
            timeout: 9999999
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            me.unmask();
            if (response.success) {
                store.load();
                B4.QuickMsg.msg("Сохранение", "Данные из версии успешно скопированы.", 'success');
            }
            
            win.close();
        }).error(function (resp) {
            me.unmask();
            B4.QuickMsg.msg("Ошибка", "Ошибка при копировании из версии", "error");
            win.close();
        });
    }
});