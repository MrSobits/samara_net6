Ext.define('B4.controller.PersonalAccountBenefits', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhButtonImportAspect',
        'B4.aspects.GridEditCtxWindow'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    models: ['regop.personal_account.PersonalAccountBenefits'],
    stores: ['regop.personal_account.PersonalAccountBenefits'],
    views: [
        'persaccbenefits.Grid',
        'persaccbenefits.EditWindow'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'persaccbenefitsgrid'
        },
        {
            ref: 'importWindow',
            selector: 'persaccbenefitsimportwin'
        }
    ],

    aspects: [
        {
            xtype: 'gkhbuttonimportaspect',
            name: 'personalAccBenefitsImportAspect',
            buttonSelector: 'persaccbenefitsgrid gkhbuttonimport',
            codeImport: 'PersonalAccountBenefits',
            windowImportView: 'import.PersAccBenefitsImportWindow',
            windowImportSelector: 'persaccbenefitsimportwin',
            getUserParams: function () {
                var me = this;
                me.params = me.params || {};
                me.params.periodId = Ext.ComponentQuery.query('persaccbenefitsimportwin [name=Period]')[0].getValue();
            },
            closeWindow: function () {
                var me = this,
                    mainView = me.controller.getMainView();

                if (me.windowImport) {
                    me.windowImport.destroy();
                }

                mainView.getStore().load();
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Import.BenefitsCategoryPersAccSum', applyTo: 'gkhbuttonimport', selector: 'persaccbenefitsgrid' }
            ]
        },
        {
            xtype: 'grideditctxwindowaspect',
            name: 'personalAccountBenefitsGridEditCtxWindowAspect',
            gridSelector: 'persaccbenefitsgrid',
            editFormSelector: 'persaccbenefitseditwindow',
            historyGridSelector: 'persaccbenefitshistorygrid',
            modelName: 'regop.personal_account.PersonalAccountBenefits',
            storeName: 'regop.personal_account.PersonalAccountBenefits',
            editWindowView: 'persaccbenefits.EditWindow',
            listeners: {
                aftersetformdata: function(asp, record) {
                    var me = this,
                        id = record.get('Id'),
                        editWindow = me.componentQuery(me.editFormSelector);

                    var changeValBtn = Ext.ComponentQuery.query('changevalbtn[propertyName="Sum"]', editWindow)[0];
                    changeValBtn.setEntityId(id);
                    changeValBtn.setDisabled(id == 0);

                    var historyGrid = me.componentQuery(me.historyGridSelector);
                    historyGrid.entityId = id;
                    historyGrid.getStore().load();

                    var sumField = editWindow.down('[name=Sum]');
                    sumField.setReadOnly(id != 0);
                }
            },
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' changevalbtn[propertyName="Sum"]'] = {
                    beforevaluesave: { fn: me.onSumPropertyBeforeSave, scope: me },
                    beforeshowwindow: { fn: me.onSumPropertyBeforeShowWindow, scope: me }
                };
                actions[me.realityObjectFieldSelector] = { change: { fn: me.onRealityObjectChange, scope: me } };
            },
            onSumPropertyBeforeSave: function (button, params) {
                var form = button.editWindow.down('form');

                button.editWindow.body.mask('Сохранение', form);

                Ext.apply(params, {
                    className: button.className,
                    propertyName: button.propertyName,
                    entityId: button.entityId
                });

                form.submit({
                    url: B4.Url.action('UpdateSum', 'PersonalAccountBenefits'),
                    params: params,
                    success: function () {
                        button.editWindow.close();
                        button.onValueSaved.apply(button, [params.value]);
                        button.fireEvent('valuesaved', button, params.value);
                        B4.QuickMsg.msg('Изменение параметра', 'Значение параметра успешно изменено.', 'success');
                        button.editWindow.body.unmask();
                    },
                    failure: function () {
                        var message = 'Произошла ошибка при изменении значения параметра.',
                            typeMessage = 'error';

                        if (this.result && this.result.message) {
                            message = this.result.message;
                            typeMessage = 'warning';
                        }
                        button.editWindow.body.unmask();
                        B4.QuickMsg.msg('Изменение параметра', message, typeMessage);
                    }
                });

                return false;
            },
            onSumPropertyBeforeShowWindow: function(button) {
                var me = this,
                    win = button.editWindow,
                    winCfg,
                    renderTo = Ext.getBody(),
                    sumField = Ext.ComponentQuery.query('persaccbenefitseditwindow [name="Sum"]')[0];

                if (!win) {
                    winCfg = {
                        modal: true,
                        width: 400,
                        closeAction: 'hide',
                        title: 'Смена значения',
                        renderTo: renderTo,
                        items: {
                            xtype: 'form',
                            border: 0,
                            defaults: {
                                margin: '8 8 8 8',
                                labelWidth: 200
                            },
                            items: [
                                {
                                    fieldLabel: 'Текущая сумма начисленной льготы',
                                    xtype: 'numberfield',
                                    name: 'oldValue',
                                    hideTrigger: true,
                                    allowBlank: false,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowDecimals: true,
                                    decimalPrecision: button.decimalPrecision,
                                    decimalSeparator: button.decimalSeparator,
                                    disabled: true
                                },
                                {
                                    fieldLabel: 'Новая сумма начисленной льготы',
                                    xtype: button.valueFieldXtype,
                                    name: 'value',
                                    minValue: 0,
                                    hideTrigger: true,
                                    allowBlank: false,
                                    keyNavEnabled: false,
                                    mouseWheelEnabled: false,
                                    allowDecimals: true,
                                    decimalPrecision: button.decimalPrecision,
                                    decimalSeparator: button.decimalSeparator
                                },
                                {
                                    fieldLabel: 'Причина',
                                    xtype: 'textarea',
                                    name: 'reason'
                                },
                                {
                                    fieldLabel: 'Документ-основание',
                                    xtype: 'b4filefield',
                                    name: 'BaseDoc'
                                }
                            ],
                            dockedItems: [
                                {
                                    xtype: 'toolbar',
                                    dock: 'top',
                                    items: [
                                        {
                                            xtype: 'b4savebutton',
                                            listeners: {
                                                click: {
                                                    fn: button.changeValue,
                                                    scope: button
                                                }
                                            }
                                        },
                                        { xtype: 'tbfill' },
                                        {
                                            xtype: 'b4closebutton',
                                            listeners: {
                                                click: {
                                                    fn: button.closeWindow,
                                                    scope: button
                                                }
                                            }
                                        }
                                    ]
                                }
                            ]
                        }
                    };

                    if (button.windowConfig) {
                        Ext.apply(winCfg, button.windowConfig);
                    }
                    win = button.editWindow = Ext.widget('window', winCfg);
                }
                var oldValueField = Ext.ComponentQuery.query('[name="oldValue"]', win)[0];
                var valueField = Ext.ComponentQuery.query('[name="value"]', win)[0];
                var reasonField = Ext.ComponentQuery.query('[name="reason"]', win)[0];
                var BaseDocField = Ext.ComponentQuery.query('[name="BaseDoc"]', win)[0];
                
                oldValueField.setValue(sumField.value);
                valueField.setValue();
                reasonField.setValue();
                BaseDocField.setValue();
                

                win.show();

                return false;
            }
        }
    ],

    init: function () {
        var me = this;

        me.control({
            'persaccbenefitsgrid': {
                afterrender: function (grid) {
                    var periodField = grid.down('[name=ChargePeriod]');

                    grid.getStore().on('beforeload', function (s, operation) {
                        operation.params.periodIds = periodField.getValue();
                    });

                    me.getOpenPeriod();
                },
                'rowaction': {
                    fn: me.rowAction,
                    scope: me
                }
            },
            'persaccbenefitsimportwin b4selectfield[name=Period]': { change: { fn: me.onChangePeriod, scope: me } },
            'persaccbenefitsgrid b4selectfield[name=ChargePeriod]': { change: { fn: me.updateGrid, scope: me } },
            'persaccbenefitsgrid b4updatebutton': { click: { fn: me.updateGrid, scope: me } }
        });

        me.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('persaccbenefitsgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.down('b4selectfield[name=ChargePeriod]').getStore().load();

        me.getAspect('personalAccBenefitsImportAspect').loadImportStore();
    },

    updateGrid: function () {
        this.getMainView().getStore().load();
    },

    getOpenPeriod: function (callback) {
        var me = this,
            grid = me.getMainView();

        me.mask('Загрузка...', grid);
        B4.Ajax.request({
            url: B4.Url.action('GetOpenPeriod', 'ChargePeriod'),
            timeout: 9999999
        }).next(function (response) {
            if (response == null) {
                return;
            }
            var res = Ext.decode(response.responseText),
                sfPeriod;

            if (!res.data) {
                Ext.Msg.alert('Не создан период!', 'Необходимо создать период!');
                me.unmask();
                return;
            }

            me.openPeriod = res.data.Name;
            sfPeriod = grid.down('b4selectfield[name=ChargePeriod]');

            if (sfPeriod) {
                if (res.data)
                    sfPeriod.setValue(res.data);
                else
                    sfPeriod.setValue(null); // просто чтобы грид обновился 
            }
            me.unmask();

            if (callback && Ext.isFunction(callback)) {
                callback();
            }
        }).error(function (response) {
            me.updateGrid();
            var message = 'Ошибка загрузки данных. Попробуйте обновить старницу';
            if (response) {
                var res = Ext.decode(response.responseText);
                message = res.data.message;
            }
            me.unmask();
            Ext.Msg.alert('Результат', message);
        });
    },

    onChangePeriod: function (cmp, newValue) {
        var importForm = Ext.ComponentQuery.query('persaccbenefitsimportwin')[0],
            fileField = importForm.down('b4filefield');

        if (!newValue) {
            fileField.setValue(null);
            fileField.setDisabled(true);
        } else {
            fileField.setDisabled(false);
        }
    },

    rowAction: function (grid, action, record) {
        var me = this;
        if (!grid || grid.isDestroyed) return;
        if (me.fireEvent('beforerowaction', me, grid, action, record) !== false) {
            switch (action.toLowerCase()) {
                case 'delete':
                    me.deleteRecord(record);
                    break;
            }
        }
    },

    deleteRecord: function (record) {
        var me = this;

        Ext.Msg.confirm('Удаление записи!', 'Вы действительно хотите удалить запись?', function (result) {
            if (result == 'yes') {
                var model = me.getModel('regop.personal_account.PersonalAccountBenefits');

                var rec = new model({ Id: record.getId() });
                me.mask('Удаление', B4.getBody());
                rec.destroy()
                    .next(function () {
                        me.updateGrid();
                        me.unmask();
                    }, this)
                    .error(function (result) {
                        Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                        me.unmask();
                    }, this);
            }
        }, me);
    }
});