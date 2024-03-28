Ext.define('B4.controller.Subsidy', {
    extend: 'B4.base.Controller',
    
    requires: [
        'B4.mixins.Context',
        'B4.aspects.ButtonDataExport'
    ],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    models: ['subsidy.SubsidyRecord'],
    views: [
        'subsidy.SubsidyTabPanel',
        'subsidy.SubsidyPanel',
        'subsidy.SubsidyRecordGrid',
        'program.FourthStageGrid',
        'program.FourthStageEditWindow'],
    
    stores: ['subsidy.SubsidyRecord'],

    mainView: 'subsidy.SubsidyPanel',
    
    aspects: [
        {
            xtype: 'b4buttondataexportaspect',
            name: 'buttonExportAspect',
            gridSelector: 'programfourthstagegrid',
            buttonSelector: 'programfourthstagegrid #btnExport',
            controllerName: 'DpkrCorrectionStage2',
            actionName: 'Export'
        }
    ],
    
    refs: [
        { ref: 'mainPanel', selector: 'subsidypanel' },
        { ref: 'recordGrid', selector: 'subsidyrecordgrid' },
        { ref: 'version', selector: 'subsidypanel b4selectfield[itemId=sfVersion]' },
        { ref: 'subsidyTabPanel', selector: 'subsidytabpanel' },
        { ref: 'programFourStageGrid', selector: 'programfourthstagegrid' },
        { ref: 'editWindow', selector: 'programfourthstageeditwin' }
    ],
        
    index: function () {
        var me = this,
            view = this.getMainPanel(),
            recordGrid,
            store;
        
        if (!view) {
            view = Ext.widget('subsidytabpanel');
            
            this.bindContext(view);
            this.application.deployView(view);
            
            recordGrid = view.down('subsidyrecordgrid');
            store = recordGrid.getStore();
            
            store.removeAll();
            
            // если произошла ошибка то закрваем доступ ко всем кнопкам на форме
            view.down('b4savebutton').setDisabled(true);
            view.down('button[action=CalcOwnerCollection]').setDisabled(true);
            view.down('button[action=CalcValues]').setDisabled(true);
            view.down('button[action=Export]').setDisabled(true);

            me.mask('Получение субсидий...', view);

            B4.Ajax.request({
                url: B4.Url.action('GetSubsidy', 'SubsidyRecord'),
                timeout: 9999999
            }).next(function (resp) {

                var respData = Ext.decode(resp.responseText);

                store.load();

                // если Все нормально то открываем кнопки для возможности нажатия
                view.down('b4savebutton').setDisabled(false);
                view.down('button[action=CalcOwnerCollection]').setDisabled(false);
                view.down('button[action=CalcValues]').setDisabled(false);
                view.down('button[action=Export]').setDisabled(false);

                me.unmask();

            }).error(function (e) {
                me.unmask();

                Ext.Msg.alert('Ошибка!', e.message);
            });
        }
        
    },
    
    init: function () {
        var me = this;
        
        me.control({
            //'subsidypanel b4selectfield[itemId=sfVersion]': {
            //    change: { fn: this.onChangeVersion, scope: this }
            //},
            'subsidypanel b4savebutton': { click: { fn: me.onSaveRequestHandler, scope: me } },
            'subsidypanel button[action=CalcOwnerCollection]': { click: { fn: me.onCalcOwnerCollection, scope: me } },
            'subsidypanel button[action=CalcValues]': { click: { fn: me.onCalcValues, scope: me } },
            'subsidypanel button[action=Export]': { click: { fn: me.exportGrid, scope: me } },
            'subsidyrecordgrid': { beforeedit: { fn: me.onBeforeEditGrid, scope: me } },
            
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
            }
        });
        
        this.callParent(arguments);
    },
   
    getPanel: function () {
        return this.getMainPanel();
    },
    
    /*
      Метод нажатия на кнопку 'Расчитать собираемость'
    */
    onCalcOwnerCollection: function (btn) {
        var me = this,
            panel = btn.up('subsidypanel'),
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [];

        Ext.each( store.getModifiedRecords(), function(rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет собираемости', panel);
            
        B4.Ajax.request({
            url: B4.Url.action('CalcOwnerCollection', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData)
            }
        }).next(function (res) {
            me.unmask();
            store.load();
            Ext.Msg.alert('Расчет собираемости!', 'Расчет собираемости произведен успешно');

        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка расчета собираемости!', e.message);
        });
    },
    
    /*
      Метод нажатия на кнопку 'Расчитать показатели'
    */
    onCalcValues: function (btn) {
        var me = this,
            panel = btn.up('subsidypanel'),
            recordGrid = panel.down('subsidyrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [];

        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет показателей', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcValues', 'SubsidyRecord'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData)
            }
        }).next(function (res) {
            store.load();
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
            frame = Ext.get('downloadIframe');
            
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
            src: B4.Url.action('Export', 'SubsidyRecord', {
            })
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
            window = me.getEditWindow() || Ext.widget('programfourthstageeditwin');

        this.bindContext(window);

        window.down('[name=CurrentIndex]').setValue(record.get('IndexNumber'));


        window.show();
    },
    
    validationBeforePublish: function (btn) {
        var me = this;

       B4.Ajax.request({
            url: B4.Url.action('GetValidationForCreatePublishProgram', 'PublishedProgram'),
            timeout: 9999999
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
            panel = btn.up('subsidypanel');
        
        me.mask('Публикация программы', panel);
        B4.Ajax.request({
            url: B4.Url.action('CreateDpkrForPublish', 'RealityObjectStructuralElementInProgramm'),
            timeout: 9999999
        }).next(function (resp) {
            me.unmask();
            Ext.Msg.alert("Сообщение!", "Программа успешно опубликована!");
            Ext.History.add('publicationprogs');
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || e));
        });
    },

    onUpdate: function () {
        this.getProgramFourStageGrid().getStore().load();
    },

    onClickSave: function (btn) {
        var me = this,
            mainPanel = me.getMainPanel(),
            store = mainPanel.getStore(),
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

        me.mask('Пожалуйста, подождите...', mainPanel);
        win.close();

        B4.Ajax.request({
            url: B4.Url.action('ChangeNumber', 'DpkrCorrectionStage2'),
            timeout: 9999999,
            params: {
                currIndex: currIndex,
                destIndex: destIndex
            }
        }).next(function () {
            me.unmask();
            store.load();
            B4.QuickMsg.msg('Успешно', 'Номер изменен успешно!', 'success');
        }).error(function (e) {
            me.unmask();
            Ext.Msg.alert('Ошибка!', (e.message || 'Во время изменения номера произошла ошибка'));
        });
    }
});