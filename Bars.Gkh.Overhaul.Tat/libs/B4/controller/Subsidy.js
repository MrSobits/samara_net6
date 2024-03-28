Ext.define('B4.controller.Subsidy', {
    extend: 'B4.base.Controller',
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    requires: [
        'B4.aspects.permission.GkhStatePermissionAspect'
    ],
    
    models: [
        'version.ProgramVersion',
        'subsidy.SubsidyMunicipalityRecord'
    ],
    
    views: [
        'subsidy.SubsidyMuPanel',
        'subsidy.SubsidyMuRecordGrid'
    ],
    
    stores: [
        'dict.MunicipalityByOperator',
        'subsidy.SubsidyMunicipalityRecord',
        'dict.MunicipalityByOperator',
        'subsidy.SubsidyMunicipalityRecord'
    ],

    mainView: 'subsidy.SubsidyMuPanel',
    
    refs: [
        { ref: 'mainPanel', selector: 'subsidymunicipalitypanel' },
        { ref: 'recordGrid', selector: 'subsidymunicipalityrecordgrid' }
    ],
    
    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'statepermissionaspect',
            permissionGrant: [],
            permissions: [
                {
                    name: 'Ovrhl.Subcidy.CalcValues',
                    applyTo: 'button[action=CalcValues]',
                    selector: 'subsidymunicipalitypanel'
                },
                {
                    name: 'Ovrhl.Subcidy.CorrectDpkr',
                    applyTo: 'button[action=CorrectDpkr]',
                    selector: 'subsidymunicipalitypanel'
                },
                {
                    name: 'Ovrhl.Subcidy.Edit',
                    applyTo: 'b4savebutton',
                    selector: 'subsidymunicipalitypanel'
                },
                {
                    name: 'Ovrhl.Subcidy.ColumnEditor.BudgetFcrEdit',
                    applyTo: '#gcBudgetFcr',
                    selector: 'subsidymunicipalityrecordgrid',
                    applyBy: function (component, allowed) {
                        if (!allowed) {
                            component.removeCls('b-editable-spec');
                            delete component.editor;
                        } else {
                            component.addCls('b-editable-spec');
                            component.editor = {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                allowDecimals: true,
                                decimalSeparator: ',',
                                decimalPrecision: 2,
                                minValue: 0
                            };
                        }
                    }
                },
                {
                    name: 'Ovrhl.Subcidy.ColumnEditor.BudgetRegionEdit',
                    applyTo: '#gcBudgetRegion',
                    selector: 'subsidymunicipalityrecordgrid',
                    applyBy: function (component, allowed) {
                        if (!allowed) {
                            component.removeCls('b-editable-spec');
                            delete component.editor;
                        } else {
                            component.addCls('b-editable-spec');
                            component.editor = {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                allowDecimals: true,
                                decimalSeparator: ',',
                                decimalPrecision: 2,
                                minValue: 0
                            };
                        }
                    }
                },
                {
                    name: 'Ovrhl.Subcidy.ColumnEditor.BudgetMunicipalityEdit',
                    applyTo: '#gcBudgetMu',
                    selector: 'subsidymunicipalityrecordgrid',
                    applyBy: function (component, allowed) {
                        if (!allowed) {
                            component.removeCls('b-editable-spec');
                            delete component.editor;
                        } else {
                            component.addCls('b-editable-spec');
                            component.editor = {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                allowDecimals: true,
                                decimalSeparator: ',',
                                decimalPrecision: 2,
                                minValue: 0
                            };
                        }
                    }
                },
                {
                    name: 'Ovrhl.Subcidy.ColumnEditor.OwnerSourcEdit',
                    applyTo: '#gcOwnerSource',
                    selector: 'subsidymunicipalityrecordgrid',
                    applyBy: function (component, allowed) {
                        if (!allowed) {
                            component.removeCls('b-editable-spec-allrows');
                            delete component.editor;
                        } else {
                            component.addCls('b-editable-spec-allrows');
                            component.editor = {
                                xtype: 'numberfield',
                                hideTrigger: true,
                                keyNavEnabled: false,
                                mouseWheelEnabled: false,
                                allowDecimals: true,
                                decimalSeparator: ',',
                                decimalPrecision: 2,
                                minValue: 0
                            };
                        }
                    }
                }
            ]
        }
    ],
    
    index: function () {
        var view = this.getMainPanel();
        if (!view) {
            view = Ext.widget('subsidymunicipalitypanel');
            this.bindContext(view);
            this.application.deployView(view);

            view.params = {};
            view.params.type = 0;
        }
    },
    
    init: function () {
        this.getStore('subsidy.SubsidyMunicipalityRecord').on('beforeload', this.onBeforeLoad, this);

        this.control({
            'subsidymunicipalitypanel b4savebutton': { click: { fn: this.onSaveRequestHandler, scope: this } },
            'subsidymunicipalitypanel button[action=CalcValues]': { click: { fn: this.onCalcValues, scope: this } },
            'subsidymunicipalitypanel button[action=Export]': { click: { fn: this.exportGrid, scope: this } },
            'subsidymunicipalitypanel button[action=CorrectResult]': { click: { fn: this.gotoCorrectionResult, scope: this } },
            'subsidymunicipalitypanel button[action=CorrectDpkr]': { click: { fn: this.correct, scope: this } },
            'subsidymunicipalityrecordgrid': { beforeedit: { fn: this.onBeforeEditGrid, scope: this } },
            'subsidymunicipalitypanel b4combobox[name="Municipality"]': {
                change: { fn: this.changeMunicipality, scope: this },
                render: { fn: this.renderMuField, scope: this }
            }
        });
        
        this.callParent(arguments);
    },
    
    correct: function(btn) {
        var me = this,
            panel = btn.up('subsidymunicipalitypanel'),
            recordGrid = panel.down('subsidymunicipalityrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [];

        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет корректировки', panel);

        B4.Ajax.request({
            url: B4.Url.action('CorrectDpkr', 'SubsidyMunicipality'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData),
                municipalityId: this.getMainPanel().down('b4combobox[name="Municipality"]').getValue()
            }
        }).next(function () {
            store.load();
            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Расчет корректировки произведен успешно', 'success');
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время расчета корректировки произошла ошибка', 'error');
        });
    },

    renderMuField: function (field) {
        var me = this,
            store = field.getStore();

        store.on('load', me.onLoadMunicipality, me);
        store.load();
    },
    
    onLoadMunicipality: function (store, records) {
        var me = this,
            panel = me.getMainPanel(),
            countRecords = store.getCount();
        
        if (countRecords > 0) {
            panel.down('b4combobox[name="Municipality"]').setValue(records[0].data);
        }
    },
    
    changeMunicipality: function (field, newValue) {
        var me = this,
            view = me.getMainPanel(),
            recordGrid = view.down('subsidymunicipalityrecordgrid'),
            store = recordGrid.getStore();

        store.removeAll();

        // если произошла ошибка то закрваем доступ ко всем кнопкам на форме
        view.down('b4savebutton').setDisabled(true);
        view.down('button[action=CalcValues]').setDisabled(true);
        view.down('button[action=Export]').setDisabled(true);
        view.down('button[action=CorrectResult]').setDisabled(true);
        view.down('button[action=CorrectDpkr]').setDisabled(true);

        me.mask('Получение субсидий...', view);

        B4.Ajax.request({
            url: B4.Url.action('GetSubsidy', 'SubsidyMunicipality'),
            params: { municipalityId: newValue },
            timeout: 9999999
        }).next(function (result) {
            var obj = Ext.decode(result.responseText),
                versionId = obj.data.versionId,
                model = me.getModel('version.ProgramVersion');
            
            me.unmask();

            store.load();

            view.down('button[action=Export]').setDisabled(false);
            view.down('button[action=CorrectResult]').setDisabled(false);

            me.getAspect('statepermissionaspect').setPermissionsByRecord(new model({ Id: versionId }));
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время получения субсидии произошла ошибка', 'error');
        });
    },
    
    onBeforeLoad: function (store, operation) {
        operation.params.municipalityId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();
    },
    
    getPanel: function () {
        return this.getMainPanel();
    },
    
    /**
    * Метод нажатия на кнопку 'Расчитать показатели'
    */
    onCalcValues: function (btn) {
        var me = this,
            panel = btn.up('subsidymunicipalitypanel'),
            recordGrid = panel.down('subsidymunicipalityrecordgrid'),
            store = recordGrid.getStore(),
            modifiedsData = [];

        Ext.each(store.getModifiedRecords(), function (rec) {
            modifiedsData.push(rec.data);
        });

        me.mask('Расчет показателей', panel);

        B4.Ajax.request({
            url: B4.Url.action('CalcValues', 'SubsidyMunicipality'),
            method: 'POST',
            timeout: 9999999,
            params: {
                records: Ext.JSON.encode(modifiedsData),
                municipalityId: me.getMainPanel().down('b4combobox[name="Municipality"]').getValue()
            }
        }).next(function () {
            store.load();
            me.unmask();
            B4.QuickMsg.msg('Успешно', 'Расчет показателей произведен успешно', 'success');
        }).error(function (e) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время расчета показателей произошла ошибка', 'error');
        });
    },

    /**
    * Метод нажатия на кнопку 'Сохранить'
    */
    onSaveRequestHandler: function (btn) {
        var panel = btn.up('subsidymunicipalitypanel'),
            recordGrid = panel.down('subsidymunicipalityrecordgrid'),
            store = recordGrid.getStore();
        
        store.sync({
            callback: function () {
                store.load();
                B4.QuickMsg.msg('Успешно', 'Сохранение прошло успешно', 'success');
            },
            // выводим сообщение при ошибке сохранения
            failure: function (e) {
                B4.QuickMsg.msg('Ошибка', e.message ? e.message : 'Во время сохранения произошла ошибка', 'error');
            }
        });
    },
    
    exportGrid: function () {
        var frame = Ext.get('downloadIframe');
            
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
            src: B4.Url.action('Export', 'SubsidyMunicipality', {})
        });
    },
    
    gotoCorrectionResult: function () {
        var muId = this.getMainPanel().down('b4combobox[name="Municipality"]').getValue();

        Ext.History.add('correctionresult/' + muId);
    },
    
    onBeforeEditGrid: function (editor, e) {
        if (e.record.get('IsShortTerm') || e.column.dataIndex === 'OwnerSource') {
            return true;
        }
       
        return false;
    }
});