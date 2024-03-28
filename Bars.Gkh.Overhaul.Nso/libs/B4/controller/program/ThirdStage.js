Ext.define('B4.controller.program.ThirdStage', {
    extend: 'B4.base.Controller',

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    requires: [
        'B4.aspects.ButtonDataExport',
        'B4.aspects.InlineGrid',
        'B4.view.program.thirddetails.CommonEstateGrid',
        'B4.view.program.thirddetails.WorkTypeGrid'
    ],

    stores: [
        'CurrentPrioirityParams',
        'program.PriorityParam'
    ],

    views: [ 'program.NewVersionWindow', 'program.ThirdStagePanel', 'program.ThirdStageGrid', 'program.EditOrderWindow', 'program.CurrentPriorityGrid', 'program.thirddetails.ThirdStageDetails'],

    refs: [
        { ref: 'mainPanel', selector: 'programthirdstagepanel' },
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
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'Ovrhl.LongProgram.Delete',
                    applyTo: 'button[action=DeleteDpkr]',
                    selector: 'programthirdstagegrid',
                    applyBy: function (component, allowed) {
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
            'programthirdstagegrid button[cmd="order"]': {
                click: {
                    fn: me.validationSetPriority,
                    scope: me
                }
            },
            
            'currprioritygrid button[cmd="priority"]': { click: me.applyPriority },
            
            'currprioritygrid b4closebutton': { click: me.closeOrderWin },
            
            'programthirdstagegrid': { rowaction: me.showDetails, scope: this },
            
            'programthirdstagepanel b4selectfield[name=Mu]': { change: me.onMuChange, scope: this },
            
            'programthirdstagegrid button[action=makelongprogram]': { click: me.makeLongProgram, scope: me },
            
            'programthirdstagegrid button[action=NewVersion]': { click: me.showNewVersionWindow, scope: this },
            
            'programthirdstagegrid button[action=DeleteDpkr]': { click: me.deleteDpkr, scope: this },
            
            'programversionwin b4savebutton': { click: me.saveVersion, scope: this },
            
            'programversionwin b4closebutton': { click: me.closeVersionWin, scope: this }
        });

        me.callParent(arguments);
    },

    // Action Index - shows LongProgram grid
    index: function () {
        var view = this.getMainPanel() || Ext.widget('programthirdstagepanel');

        this.bindContext(view);
        this.application.deployView(view);

        //view.getStore().load();
    },
    
    onMuChange: function (field, newVal, oldVal) {
        var val = newVal && newVal.Id ? newVal.Id : newVal,
            store = field.up('programthirdstagepanel').down('programthirdstagegrid').getStore();
        
        store.clearFilter(true);
        if (val) {
            store.filter('muId', val);
        } else {
            store.removeAll();
        }
    },

    update: function (btn) {
        var store = btn.up('programthirdstagegrid').getStore();
        store.load();
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
        
        me.mask('Создание новой версии программы...', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('MakeNewVersion', 'RealityObjectStructuralElementInProgrammStage3'),
            params: form.getValues(),
            timeout: 9999999
        }).next(function () {
            me.unmask();
            B4.QuickMsg.msg("Сохранение", "Новая версия программы успешно создана.");
            win.close();
        }).error(function () {
            me.unmask();
            B4.QuickMsg.msg("Ошибка", "Ошибка при сохранении", "error");
            win.close();
        });
    },
    
    showNewVersionWindow: function (btn) {
        var win = Ext.widget('programversionwin');

        win.show();
    },

    validationSetPriority: function (btn) {
        var me = this;

        // Если просят расчитать только по Баллам, то пожалуста
        if (Gkh.config.Overhaul.OverhaulNso.MethodOfCalculation == 20) {
            me.setPriority();
        } else {
            // иначе расчитываем по критериям, но перед этим показываем форму критериев
            me.editOrder(btn);
        }
    },

    editOrder: function(btn) {
        var me = this,
            store;

        if (!me.editOrderWin) {
            me.editOrderWin = Ext.widget('editorderwin');
            this.bindContext(me.editOrderWin);
        }
        
        store = me.editOrderWin.down('grid').getStore();

        store.load();
        
        me.editOrderWin.show();
    },

    // методотправляющий запрос на очередность серверу
    setPriority: function (data) {
        var me = this;
        
        me.mask('Формирование очередности программы...', me.getMainPanel());
        B4.Ajax.request({
            url: B4.Url.action('SetPriority', 'RealityObjectStructuralElementInProgrammStage3'),
            params: {
                records: data ? Ext.encode(data) : []
            },
            timeout: 9999999
        }).next(function() {
            me.unmask();
            B4.QuickMsg.msg("Сообщение", "Очередность успешно сформирована.");
            me.getMainPanel().down('programthirdstagegrid').getStore().load();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Не удалось сформировать очередность: ' + (e.message || e));
        });
    },

    // метод применения очередности с формы
    applyPriority: function(btn) {
        var me = this,
            grid = btn.up('grid'),
            store = grid.getStore(),
            valid = true,
            data = [];
        
        store.each(function(r) {
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
        var me = this;
        B4.Ajax.request({
            url: B4.Url.action('GetInfo', 'RealityObjectStructuralElementInProgrammStage3'),
            params: { st3Id: id },
            timeout: 9999999
        }).next(function(response) {
            var data = Ext.JSON.decode(response.responseText);
            me.getDetailsPanel().getForm().setValues(data);
            me.getDetailsPanel().down('#thirddetailsinfolabel').update({ text: data.Address });
        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка', 'Произошла ошибка при получении данных по объекту', 'error');
        });
    },
    
    makeLongProgram: function(btn) {
        var me = this,
            grid = btn.up('programthirdstagegrid'),
            store = grid.getStore();

        me.mask('Формирование программы...', grid);

        B4.Ajax.request({
            url: B4.Url.action('MakeLongProgram', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999
        }).next(function () {
            store.load();
            Ext.Msg.alert('Успешно!', 'Долгосрочная программа успешно сформирована!');
            me.unmask();
        }).error(function (response) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', response.message ? response.message : 'При формировании плана произошла ошибка!');
        });
    },
    
    deleteDpkr: function (btn) {
        var me = this,
        grid = btn.up('programthirdstagegrid'),
        store = grid.getStore();
        
        B4.Ajax.request({
            url: B4.Url.action('ValidationDeleteDpkr', 'RealityObjectStructuralElementInProgrammStage3'),
            timeout: 9999999
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText);
            if (data.success == true) {
                Ext.MessageBox.show({
                    title: 'Внимание!',
                    msg: 'Вы действительно хотите удалить текущую программу?',
                    buttons: Ext.MessageBox.YESNO,
                    icon: Ext.MessageBox.WARNING,
                    width: 300,
                    closable: false,
                    fn: function(btn) {
                        if (btn == 'yes') {
                            me.mask('Удаление программы...', grid);
                            B4.Ajax.request({
                                url: B4.Url.action('DeleteDpkr', 'RealityObjectStructuralElementInProgrammStage3'),
                                timeout: 9999999
                            }).next(function() {
                                store.load();
                                Ext.Msg.alert('Успешно!', 'Долгосрочная программа успешно удалена!');
                                me.unmask();
                            }).error(function (e) {
                                me.unmask();
                                B4.QuickMsg.msg('Ошибка!', 'Произошла ошибка при удалении программы', 'error');
                            });
                        }
                    }
                });
            } else {
                B4.QuickMsg.msg('Ошибка', 'Отсутствуют данные для удаления', 'error');
            }
        }).error(function (e) {
            B4.QuickMsg.msg('Ошибка', 'Произошла ошибка', 'error');
        });
    }
});