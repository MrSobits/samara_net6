Ext.define('B4.controller.MassCalcLongProgram', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.InlineGrid',
        'B4.view.params.Panel',
        'B4.Ajax',
        'B4.Url'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        { ref: 'mainPanel', selector: 'masscalctabpanel' },
        { ref: 'orderWin', selector: 'editorderwin' },
        { ref: 'copyGrid', selector: 'copydefaultcollectiongrid' },
        { ref: 'copyWin', selector: 'copydefaultcollectionwin' }
    ],

    models: [
        'subsidy.DefaultPlanCollectionInfo'
    ],

    stores: [
        'program.PriorityParam',
        'CurrentPrioirityParams',
        'subsidy.DefaultPlanCollectionInfo'
    ],

    views: [
        'program.CurrentPriorityGrid',
        'program.EditOrderWindow',
        'masscalc.TabPanel',
        'masscalc.CalcPanel',
        'masscalc.CopyDefaultCollectionGrid',
        'masscalc.CopyDefaultCollectionWindow'
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
            xtype: 'inlinegridaspect',
            name: 'defaultCollectionInfoAspect',
            storeName: 'subsidy.DefaultPlanCollectionInfo',
            modelName: 'subsidy.DefaultPlanCollectionInfo',
            gridSelector: 'copydefaultcollectiongrid'
        }
    ],

    init: function() {
        var me = this;

        me.control({
            'masscalclongprogpanel': { destroy: function() { var win = me.getOrderWin(); if (win) { win.close(); } } },
            'masscalclongprogpanel button[action="makelongprograms"]': { click: { fn: me.onClickMakeLongProgramAll, scope: me } },
            'masscalclongprogpanel button[action="setpriorityall"]': { click: { fn: me.onClickSetPriorityAll, scope: me } },
            'masscalclongprogpanel button[action="publishedprograms"]': { click: { fn: me.onClickPublishedPrograms, scope: me } },
            'masscalclongprogpanel button[action="makeversions"]': { click: { fn: me.onClickMakeNewVersions, scope: me } },
            'masscalclongprogpanel button[action="calcownercollection"]': { click: { fn: me.onClickCalcOwnerCollection, scope: me } },
            'masscalclongprogpanel button[action="calcvalues"]': { click: { fn: me.onClickCalcValues, scope: me } },
            'currprioritygrid button[cmd="priorityAll"]': { click: me.applyPriorityAll },
            'currprioritygrid b4closebutton': { click: me.closeOrderWin },
            'copydefaultcollectionwin button[action=Copy]': { click: { fn: me.onClickCopySubsidy, scope: me } },
            'copydefaultcollectionwin button[action=Close]': { click: function () { me.getCopyWin().close(); } },
            'copydefaultcollectiongrid button[action=UpdatePeriod]': { click: { fn: me.onClickUpdatePeriod, scope: me } },
            'copydefaultcollectiongrid': {
                destroy: function() {
                    var win = me.getCopyWin();
                    if (win) {
                        win.destroy();
                    }
                }
            },
            'copydefaultcollectiongrid button[action=Copy]': {
                click: function() {
                    var copyWin = me.getCopyWin();

                    if (!copyWin) {
                        copyWin = Ext.widget('copydefaultcollectionwin');
                        B4.getBody().getActiveTab().add(copyWin);
                    }
                    copyWin.show();
                }
            }
        });

        this.callParent(arguments);
    },

    index: function() {
        var view = this.getMainPanel() || Ext.widget('masscalctabpanel');

        this.bindContext(view);
        this.application.deployView(view);
    },

    onClickMakeLongProgramAll: function() {
        var me = this,
            mainPanel = me.getMainPanel();

        Ext.Msg.confirm('Внимание!', "Сейчас будет проведен расчет ДПКР для всех МО. Продолжить?", function(yesNo) {
            if (yesNo === 'yes') {
                me.mask('Расчет ДПКР', mainPanel);
                B4.Ajax.request({
                    url: B4.Url.action('MakeLongProgramAll', 'LongProgram'),
                    timeout: 9999999
                }).next(function() {
                    me.unmask();
                    Ext.Msg.alert('Успешно!', 'Долгосрочная программа успешно сформирована!');
                }).error(function(e) {
                    me.unmask();
                    Ext.Msg.alert('Ошибка!', e.message || 'При формировании плана произошла ошибка!');
                });
            }
        });
    },

    onClickSetPriorityAll: function() {
        var me = this,
            panel = me.getMainPanel(),
            priorityBtn,
            win = me.getOrderWin(),
            showPriorityWin = Gkh.config.Overhaul.OverhaulHmao.MethodOfCalculation != 20;

        Ext.Msg.confirm('Внимание!', "Сейчас будет сформирована очередь домов для всех МО. Продолжить?", function(yesNo) {
            if (yesNo === 'yes') {

                if (!showPriorityWin) {
                    me.setPriorityAll(null, panel);
                    return;
                }

                win = me.getOrderWin() || Ext.widget('editorderwin');

                priorityBtn = win.down('button[cmd="priority"]');
                if (priorityBtn) {
                    priorityBtn.cmd = 'priorityAll';
                }

                panel.add(win);
                me.bindContext(win);
                win.down('grid').getStore().load();
                win.show();
            }
        });
    },

    onClickPublishedPrograms: function() {
        var me = this,
            panel = me.getMainPanel();

        me.mask('Публикация программ', panel);
        B4.Ajax.request({
            url: B4.Url.action('CreateDpkrForPublish', 'RealityObjectStructuralElementInProgramm'),
            params: { isMass: true },
            timeout: 9999999
        }).next(function(resp) {
            me.unmask();
            Ext.Msg.alert("Сообщение!", "Программы успешно опубликованы!");
        }).error(function(e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    validatePriorityParam: function(rec) {
        return !Ext.isEmpty(rec.get('Code')) && !Ext.isEmpty(rec.get('Order'));
    },

    applyPriorityAll: function(btn) {
        var me = this,
            grid = btn.up('grid'),
            store = grid.getStore(),
            mainPanel = me.getMainPanel(),
            valid = true,
            data;

        store.each(function(r) {
            valid = me.validatePriorityParam(r);
            return valid;
        });

        if (!valid) {
            B4.QuickMsg.msg("Ошибка", "Заполните добавленные записи", "error");
        } else {
            btn.up('window').close();
            data = Ext.Array.map(store.data.items, function(r) {
                return { Code: r.get('Code'), Order: r.get('Order') };
            });

            me.setPriorityAll(data, mainPanel);
        }
    },
    setPriorityAll: function (data, mainPanel) {
        var me = this;

        me.mask('Расчет очередности...', mainPanel);

        B4.Ajax.request({
            url: B4.Url.action('SetPriorityAll', 'LongProgram'),
            params: {
                priorityParams: data ? Ext.encode(data) : []
            },
            timeout: 9999999
        }).next(function () {
            Ext.Msg.alert('Успешно!', 'Расчет очередности выполнен успешно!');
            me.unmask();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || 'При расчете очередности произошла ошибка!');
        });
    },
    onClickMakeNewVersions: function() {
        var me = this,
            panel = me.getMainPanel();

        me.mask('Создание новой версии программ...', panel);
        B4.Ajax.request({
            url: B4.Url.action('MakeNewVersionAll', 'ProgramVersion'),
            timeout: 9999999
        }).next(function() {
            me.unmask();
            Ext.Msg.alert("Успешно!", "Новые версии программ успешно созданы");
        }).error(function(e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || 'При создании версий программ произошла ошибка!');
        });
    },

    onClickCalcOwnerCollection: function() {
        var me = this,
            panel = me.getMainPanel();

        me.mask('Расчет собираемости', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcOwnerCollection', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                isMass: true
            }
        }).next(function() {
            me.unmask();
            Ext.Msg.alert('Успешно!', 'Расчет собираемости произведен успешно');
        }).error(function(e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || 'При расчете собираемости произошла ошибка');
        });
    },

    closeOrderWin: function(btn) {
        btn.up('editorderwin').close();
    },

    /*
    * Метод нажатия на кнопку 'Расчитать показатели'
    */
    onClickCalcValues: function(btn) {
        var me = this,
            panel = me.getMainPanel();

        me.mask('Расчет показателей', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcValues', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                isMass: true
            }
        }).next(function(res) {
            me.unmask();
            Ext.Msg.alert('Успешно!', 'Расчет показателей произведен успешно');
        }).error(function(e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', e.message || 'При расчете показателей произошла ошибка');
        });
    },
    
    onClickUpdatePeriod: function(btn) {
        var me = this,
            grid = btn.up('grid');
        
        if (grid.getStore().getModifiedRecords().length > 0) {
            B4.QuickMsg.msg('Ошибка', 'Сначала нужно сохранить измененные записи', 'error');
            return;
        }

        try {
            me.mask('Обновление периода...', grid);
            B4.Ajax.request({
                url: B4.Url.action('UpdatePeriod', 'DefaultPlanCollectionInfo'),
                timeout: 9999999
            }).next(function (res) {
                me.unmask();
                Ext.Msg.alert('Успешно', res.message || 'Период успешно обновлен');
                grid.getStore().load();
            }).error(function (res) {
                me.unmask();
                Ext.Msg.alert('Ошибка', res.message || 'Ошибка при обновлении периода');
            });
        } catch (e) {
            me.unmask();
        } 
    },

    onClickCopySubsidy: function() {
        var me = this,
            win = me.getCopyWin(),
            grid = win.down('grid[name=DestMunicipality]'),
            selectedMu,
            destinationIds = [];

        selectedMu = grid.getSelectionModel().getSelection();

        if (selectedMu.length == 0) {
            B4.QuickMsg.msg('Ошибка', 'Не выбраны муниципальные образования', 'error');
            return;
        }

        Ext.each(selectedMu, function(r) {
            destinationIds.push(r.getId());
        });
        
        try {
            me.mask('Копирование...', B4.getBody().getActiveTab());
            B4.Ajax.request({
                url: B4.Url.action('CopyCollectionInfo', 'DefaultPlanCollectionInfo'),
                method: 'POST',
                timeout: 9999999,
                params: {
                    destinationIds: Ext.encode(destinationIds)
                }
            }).next(function (res) {
                me.unmask();
                Ext.Msg.alert('Успешно!', res.message || 'Плановые показатели собираемости были скопированы');
                win.close();
            }).error(function (res) {
                me.unmask();
                Ext.Msg.alert('Ошибка!', res.message || 'Ошибка копирования плановых показателей собираемости');
            });
        } catch(e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Ошибка копирования плановых показателей собираемости');
        } 
        
    }
});