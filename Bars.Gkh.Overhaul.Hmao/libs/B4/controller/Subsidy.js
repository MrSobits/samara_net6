Ext.define('B4.controller.Subsidy', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.mixins.Context',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.InlineGrid',
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: [
        'subsidy.SubsidyRecord',
        'subsidy.DefaultPlanCollectionInfo'
    ],
    
    views: [
        'subsidy.SubsidyTabPanel',
        'subsidy.SubsidyPanel',
        'subsidy.SubsidyRecordGrid',
        'program.FourthStageGrid',
        'program.FourthStageEditWindow',
        'subsidy.DefaultPlanCollectionInfoGrid'
    ],

    stores: [
        'subsidy.SubsidyRecord',
        'subsidy.DefaultPlanCollectionInfo'
    ],

    mainView: 'subsidy.SubsidyPanel',

    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'programfourthstagegrid',
            buttonSelector: 'programfourthstagegrid #btnExport',
            controllerName: 'DpkrCorrectionStage2',
            actionName: 'Export'
        },
        {
            xtype: 'inlinegridaspect',
            name: 'defaultPlanCollectionInfoAspect',
            storeName: 'subsidy.DefaultPlanCollectionInfo',
            modelName: 'subsidy.DefaultPlanCollectionInfo',
            gridSelector: 'defaultplancollectioninfogrid'
        },
         {
             xtype: 'gkhstatepermissionaspect',
             name: 'statepermissionaspect',
             permissionGrant: [],
             permissions: [
                 {
                     name: 'Ovrhl.Subcidy.CalcValues',
                     applyTo: 'button[action=CalcValues]',
                     selector: 'subsidypanel'
                 },
                 {
                     name: 'Ovrhl.Subcidy.CalcOwnerCollection',
                     applyTo: 'button[action=CalcOwnerCollection]',
                     selector: 'subsidypanel'
                 },
                 {
                     name: 'Ovrhl.Subcidy.Edit',
                     applyTo: 'b4savebutton',
                     selector: 'subsidypanel'
                 }
             ]
         }

    ],

    refs: [
        { ref: 'mainPanel', selector: 'subsidypanel' },
        { ref: 'recordGrid', selector: 'subsidyrecordgrid' },
        { ref: 'version', selector: 'subsidypanel b4selectfield[itemId=sfVersion]' },
        { ref: 'subsidyTabPanel', selector: 'subsidytabpanel' },
        { ref: 'programFourStageGrid', selector: 'programfourthstagegrid' },
        { ref: 'editWindow', selector: 'programfourthstageeditwin' },
        { ref: 'planCollectionInfoGrid', selector: 'defaultplancollectioninfogrid' }
    ],

    index: function () {
        var me = this,
            view = me.getSubsidyTabPanel();

        if (!view) {
            view = Ext.widget('subsidytabpanel');

            me.bindContext(view);
            me.application.deployView(view);
        }

        me.getPlanCollectionInfoGrid().getStore().load();
    },

    init: function () {
        var me = this;

        me.control({
            'subsidypanel b4savebutton': { click: { fn: me.onSaveRequestHandler, scope: me } },
            'subsidypanel button[action=CalcOwnerCollection]': { click: { fn: me.onCalcOwnerCollection, scope: me } },
            'subsidypanel button[action=CalcBalance]': { click: { fn: me.onCalcBalance, scope: me } },
            'subsidypanel button[action=CalcValues]': { click: { fn: me.onCalcValues, scope: me } },
            'subsidypanel button[action=Export]': { click: { fn: me.exportGrid, scope: me } },
            'subsidytabpanel': { tabchange: { fn: me.onTabChange, scope: me } },
            'subsidytabpanel combobox[name="Municipality"]': {
                select: { fn: this.onSelectMunicipality, scope: this },
                render: { fn: this.renderMuField, scope: this }
            },
            
            'subsidyrecordgrid': {
                render: {
                    fn: this.onSubsidyRecordRender,
                    scope: this
                }  
            },

            'programfourthstagegrid [action="PublishDpkr"]': { click: { fn: me.validationBeforePublish, scope: me } },
            'programfourthstagegrid b4updatebutton': { click: { fn: me.onUpdate, scope: me } },
            'programfourthstagegrid': {
                itemdblclick: {
                    fn: function (view, record) {
                        me.editRecord(record);
                    },
                    scope: me
                },
                rowaction: {
                    fn: function (view, action, record) {
                        if (action.toLowerCase() === 'edit') {
                            me.editRecord(record);
                        }
                    },
                    scope: me
                },
                render: {
                    fn: this.onFourthStageGridRender,
                    scope: this
                }
            },
            'programfourthstageeditwin b4closebutton': {
                click: {
                    fn: function (btn) {
                        btn.up('programfourthstageeditwin').close();
                    },
                    scope: me
                }
            },
            'programfourthstageeditwin b4savebutton': {
                click: {
                    fn: me.onClickSave,
                    scope: me
                }
            },
            'defaultplancollectioninfogrid [action=UpdatePeriod]': { click: { fn: me.onClickUpdatePeriod, scope: me } }
        });

        this.callParent(arguments);
    },

    renderMuField: function (field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me, { single: true });
        store.load();
    },

    onLoadMunicipality: function (store, records) {
        var me = this,
            cmb;

        if (!Ext.isEmpty(records[0])) {
            cmb = me.getSubsidyTabPanel().down('combobox[name="Municipality"]');
            cmb.setValue(records[0]);
            me.onSelectMunicipality(cmb, [records[0]]);
        }
    },

    onTabChange: function (tabPanel, newCard) {
        var municipalityFld = tabPanel.down('combobox[name=Municipality]');

        if (newCard.xtype == 'defaultplancollectioninfogrid') {
            municipalityFld.hide();
        } else {
            municipalityFld.show();
        }
    },
    
    onSelectMunicipality: function (field, records) {
        var me = this,
            view = me.getSubsidyTabPanel(),
            recordGrid = view.down('subsidyrecordgrid'),
            store = recordGrid.getStore(),
            storeFs = me.getProgramFourStageGrid().getStore(),
            labelOwner = view.down('label[name=DateCalcOwner]'),
            labelCorrection = view.down('label[name=DateCalcCorrection]'),
            value = records[0].getId(),
            model = me.getModel('version.ProgramVersion'),
            versionId;

        field.store.clearFilter();
        storeFs.load();
        store.removeAll();

        // если произошла ошибка то закрваем доступ ко всем кнопкам на форме
        view.down('b4savebutton').setDisabled(true);
        view.down('button[action=CalcOwnerCollection]').setDisabled(true);
        view.down('button[action=CalcValues]').setDisabled(true);
        view.down('button[action=Export]').setDisabled(true);

        me.mask('Получение субсидий...', view);

        B4.Ajax.request({
            url: B4.Url.action('GetSubsidy', 'SubsidyRecord'),
            params: {
                mo_id: value
            },
            timeout: 9999999
        }).next(function (result) {
            var obj = Ext.decode(result.responseText);
            versionId = obj.data.data;

            store.load();

            // обновляем дату расчета потребности
            me.updateDateCalcOwnerCollection(labelOwner, value);

            //обновляем дату расета корреткировки
          //  me.updateDateCalcCorrection(labelCorrection, value);
            
            // если Все нормально то открываем кнопки для возможности нажатия
            view.down('b4savebutton').setDisabled(false);
            view.down('button[action=CalcOwnerCollection]').setDisabled(false);
            view.down('button[action=CalcValues]').setDisabled(false);
            view.down('button[action=Export]').setDisabled(false);
            me.getAspect('statepermissionaspect').setPermissionsByRecord(new model({ Id: versionId }));

            me.unmask();
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка!', e.message, 'error');
        });

    },



    updateDateCalcOwnerCollection: function(cmp, municipalityId) {
        B4.Ajax.request({
            url: B4.Url.action('GetDateCalcOwnerCollection', 'ProgramVersion'),
            params: {
                muId: municipalityId
            },
            timeout: 9999999
        }).next(function (resp) {
            var data = Ext.decode(resp.responseText);
            var str = 'Дата расчета собираемости ' + (data ? data.dateStr : '');
            cmp.update(str);
        }).error(function () {

        });
    },

    updateDateCalcCorrection: function (cmp, municipalityId) {
        B4.Ajax.request({
            url: B4.Url.action('GetDateCalcCorrection', 'ProgramVersion'),
            params: {
                muId: municipalityId
            },
            timeout: 9999999
        }).next(function (resp) {
            var data = Ext.decode(resp.responseText);
            var str = 'Дата расчета корректировки ' + (data ? data.dateStr : '');
            cmp.update(str);
        }).error(function () {

        });
    },

    getPanel: function () {
        return this.getMainPanel();
    },

    updateSaldoBallance: function (municipalityId, panel) {
        var me = this,
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore();
        me.mask('Расчет итогов', panel);
        B4.Ajax.request({
            url: B4.Url.action('UpdateSaldoBallance', 'SubsidyRecord'),
            params: {
                muId: municipalityId
            },
            timeout: 9999999
        }).next(function (resp) {
            me.unmask();
            store.load();
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка расчета собираемости!', e.message);
        });
    },

    /*
      Метод нажатия на кнопку 'Расчитать собираемость'
    */
    onCalcOwnerCollection: function (btn) {
        var me = this,
            panel = btn.up('subsidypanel'),
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [],
            labelOwner = panel.down("label[name=DateCalcOwner]"),
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();

        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет собираемости', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcOwnerCollection', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData),
                mo_id: moId,
                recalcOwnerSumForCr: true
            }
        }).next(function (res) {
            me.unmask();
            store.load();
            
            //обновляем дату расета собираемости
            me.updateSaldoBallance(moId, panel);
            me.updateDateCalcOwnerCollection(labelOwner, moId);
            
            Ext.Msg.alert('Расчет собираемости!', 'Расчет собираемости произведен успешно');

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка расчета собираемости!', e.message);
        });
    },

    /*
      Метод нажатия на кнопку 'Расчитать баланс'
    */
    onCalcBalance: function (btn) {
        var me = this,
            panel = btn.up('subsidypanel'),
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [],
            labelOwner = panel.down("label[name=DateCalcOwner]"),
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();

        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет собираемости', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcOwnerCollection', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData),
                mo_id: moId,
                recalcOwnerSumForCr: false
            }
        }).next(function (res) {
            me.unmask();
            store.load();

            //обновляем дату расета собираемости
            me.updateSaldoBallance(moId, panel);

            Ext.Msg.alert('Расчет баланса!', 'Расчет баланса произведен успешно');

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка расчета баланса!', e.message);
        });
    },

    /*
      Метод нажатия на кнопку 'Расчитать показатели'
    */
    onCalcValues: function (btn) {
        var me = this,
            panel = btn.up('subsidytabpanel'),
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [],
            labelCorrection = panel.down("label[name=DateCalcCorrection]"),
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();

        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет показателей', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcValues', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData),
                mo_id: moId
            }
        }).next(function (res) {
            store.load();
            
            //обновляем дату расчета корректировки
         //   me.updateDateCalcCorrection(labelCorrection, moId);
            
            me.unmask();
            Ext.Msg.alert('Расчет показателей!', 'Расчет показателей произведен успешно');

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка расчета показателей!', e.message);
        });
    },

    /*
      Метод нажатия на кнопку 'Сохранить'
    */
    onSaveRequestHandler: function (btn) {
        var me = this,
            panel = btn.up('subsidypanel'),
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore();

        store.sync({
            callback: function () {
                Ext.Msg.alert('Сохранение', 'Сохранение прошло успешно!');
            },
            // выводим сообщение при ошибке сохранения
            failure: function (e) {
                Ext.Msg.alert('Ошибка сохранения!', e.message);
            }
        });
    },

    exportGrid: function () {
        var me = this,
            frame = Ext.get('downloadIframe'),
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();

        if (frame != null) {
            Ext.destroy(frame);
        }

        Ext.DomHelper.append(document.body, {
            tag: 'iframe',
            id: 'downloadIframe',
            frameBorder: 0,
            width: 0,
            height: 0,
            css: 'display:none;visibility:hidden;height:0px;',
            src: B4.Url.action('Export', 'SubsidyRecord', { municipalityId: moId })
        });
    },

    onBeforeEditGrid: function (editor, e) {
        if (e.record.get('IsShortTerm')) {
            return true;
        }

        return false;
    },

    editRecord: function (record) {
        var me = this,
            window = me.getEditWindow() || Ext.widget('programfourthstageeditwin'),
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();;

        this.bindContext(window);

        window.down('[name=CurrentIndex]').setValue(record.get('IndexNumber'));
        window.params = {};
        window.params.moId = moId;
        
        window.show();
    },

    validationBeforePublish: function (btn) {
        var me = this,
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();

        B4.Ajax.request({
            url: B4.Url.action('GetValidationForCreatePublishProgram', 'PublishedProgram'),
            timeout: 9999999,
            params: {
                mo_id: moId
            }
        }).next(function (resp) {
            var message = Ext.decode(resp.responseText);

            Ext.Msg.confirm('Внимание', message, function (result) {
                if (result == 'yes') {
                    me.onPublishDpkr(btn);
                }
            });

        }).error(function (e) {
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    onPublishDpkr: function (btn) {
        var me = this,
            panel = btn.up('subsidytabpanel'),
            moId = btn.up('subsidytabpanel').down('combobox[name="Municipality"]').getValue();

        me.mask('Публикация программы', panel);
        B4.Ajax.request({
            url: B4.Url.action('CreateDpkrForPublish', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999,
            params: {
                mo_id: moId
            }
        }).next(function (resp) {
            me.unmask();
            Ext.Msg.alert("Сообщение!", "Программа успешно опубликована!");
            Ext.History.add('publicationprogs/' + moId);
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },
    
    onFourthStageGridRender: function(grid) {
        grid.getStore().on('beforeload', this.onDispatchMunicipalityBeforeLoad, this);
    },
    
    onDispatchMunicipalityBeforeLoad: function(store) {
        var me = this,
            moId = me.getSubsidyTabPanel().down('combobox[name="Municipality"]').getValue();
        
        Ext.apply(store.getProxy().extraParams, { mo_id: moId });
    },
    
    onSubsidyRecordRender: function(grid) {
        grid.getStore().on('beforeload', this.onDispatchMunicipalityBeforeLoad, this);
    },

    onUpdate: function () {
        this.getProgramFourStageGrid().getStore().load();
    },

    onClickSave: function (btn) {
        var me = this,
            grid = me.getProgramFourStageGrid(),
            store = grid.getStore(),
            win = btn.up('programfourthstageeditwin'),
            currIndex = win.down('[name=CurrentIndex]').getValue(),
            destIndex = win.down('[name=DestIndex]').getValue();

        if (currIndex == destIndex) {
            B4.QuickMsg.msg('Предупреждение', 'Номера совпадают', 'warning');
            return;
        }

        if (destIndex < 1) {
            B4.QuickMsg.msg('Предупреждение', 'Необходимо заполнить все поля', 'warning');
            return;
        }

        me.mask('Пожалуйста, подождите...', grid);
        win.close();

        B4.Ajax.request({
            url: B4.Url.action('ChangeNumber', 'DpkrCorrectionStage2'),
            timeout: 9999999,
            params: {
                currIndex: currIndex,
                destIndex: destIndex,
                moId: win.params.moId
            }
        }).next(function () {
            me.unmask();
            store.load();
            B4.QuickMsg.msg('Успешно', 'Номер изменен успешно!', 'success');
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время изменения номера произошла ошибка'));
        });
    },
    
    onClickUpdatePeriod: function (btn) {
        var me = this,
        grid = btn.up('grid');
        
        me.mask('Обновление периода...', grid);
        B4.Ajax.request({
            url: B4.Url.action('UpdatePeriod', 'DefaultPlanCollectionInfo'),
            timeout: 9999999
        }).next(function () {
            me.unmask();
            grid.getStore().load();
        }).error(function () {
            me.unmask();

        });
    }
});