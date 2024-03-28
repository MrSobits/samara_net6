Ext.define('B4.controller.providerpassport.MyOki', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhDigitalSignatureGridAspect',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        maks: 'B4.mixins.MaskBody'
    },

    views: [
        'providerpassport.myoki.Grid',
        'providerpassport.myoki.EditWindow'
    ],

    refs: [
        { ref: 'mainPanel', selector: 'myokiprovpaspgrid' }
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'myokistatetransfermenu',
            gridSelector: 'myokiprovpaspgrid',
            menuSelector: 'myokiprovpaspgridstatemenu',
            stateType: 'okiproviderpassport'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh1468.Passport.MyOki.Create', applyTo: 'b4addbutton', selector: 'myokiprovpaspgrid' },
                { name: 'Gkh1468.Passport.MyOki.Delete', applyTo: 'b4deletecolumn', selector: 'myokiprovpaspgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhdigitalsignaturegridaspect',
            gridSelector: 'myokiprovpaspgrid',
            controllerName: 'OkiProviderPassport'
        }
    ],

    init: function() {
        this.control({
            'myokiprovpaspgrid combobox[index=Year]': { change: { fn: this.onComboBoxChange, scope: this } },

            'myokiprovpaspgrid combobox[index=Month]': { change: { fn: this.onComboBoxChange, scope: this } },

            'myokiprovpaspgrid b4addbutton': { click: { fn: this.showAddWindow, scope: this } },

            'myokiprovpaspgrid b4updatebutton': { click: { fn: this.updateGrid, scope: this } },

            'okiprovpassportwin b4closebutton': { click: { fn: this.closeWindow, scope: this } },

            'okiprovpassportwin b4savebutton': { click: { fn: this.saveRec, scope: this } },

            'okiprovpassportwin b4selectfield[name=Municipality]': { change: { fn: this.loadInfo, scope: this } },

            'myokiprovpaspgrid actioncolumn[action="openpassport"]': { click: { fn: this.editpassport, scope: this } },

            'myokiprovpaspgrid': {
                rowaction: {
                    fn: this.onRowAction,
                    scope: this
                }
            },

            'okiprovpassportwin combobox[index="Month"]': { change: { fn: this.loadInfo, scope: this } },
            'okiprovpassportwin combobox[index="Year"]': { change: { fn: this.loadInfo, scope: this } }
        });

        this.callParent(arguments);
    },

    months: {
        1: 'Январь',
        2: 'Февраль',
        3: 'Март',
        4: 'Апрель',
        5: 'Май',
        6: 'Июнь',
        7: 'Июль',
        8: 'Август',
        9: 'Сентябрь',
        10: 'Октябрь',
        11: 'Ноябрь',
        12: 'Декабрь'
    },

    getGrid: function() {
        return this.getMainPanel();
    },

    editpassport: function(grid, rowIndex, colIndex, param, param2, rec) {
        Ext.History.add('okipasspeditor/' + rec.getId() + '/');
    },

    index: function() {
        var year = new Date().getFullYear(),
            month = new Date().getMonth() + 1,
            view = this.getMainPanel() || Ext.widget('myokiprovpaspgrid', {
                year: year,
                month: month
            });

        this.bindContext(view);
        this.application.deployView(view);

        this.onComboBoxChange();
    },

    updateGrid: function(btn) {
        btn.up('myokiprovpaspgrid').getStore().load();
    },

    onComboBoxChange: function() {
        var panel = this.getMainPanel(),
            cbYear = panel.down('combobox[index=Year]'),
            cbMonth = panel.down('combobox[index=Month]'),
            store = panel.getStore();

        store.clearFilter(true);
        store.filter([
            { property: "year", value: cbYear.getValue() },
            { property: "month", value: cbMonth.getValue() }
        ]);
    },
    
    showAddWindow: function (btn) {
        var grid = btn.up('myokiprovpaspgrid'),
            win = Ext.widget('okiprovpassportwin', {
                renderTo: grid.getEl()
            });

        grid.add(win);
        win.show();
    },

    closeWindow: function(btn) {
        btn.up('okiprovpassportwin').close();
    },

    saveRec: function(btn) {
        var me = this,
            win = btn.up('okiprovpassportwin'),
            oldPaspId = win.down('combobox[name=OldPaspPeriods]').getValue(),
            muId = win.down('b4selectfield[name=Municipality]').getValue(),
            year = win.down('combobox[index=Year]').getValue(),
            month = win.down('combobox[index=Month]').getValue(),
            paspStructId = win.paspStructId;

        if (!win.getForm().isValid()) {
            B4.QuickMsg.msg('Ошибка', 'Проверьте правильность заполнения формы', 'error');
            return;
        }

        if (!oldPaspId && !paspStructId) {
            B4.QuickMsg.msg('Ошибка', 'Невозможно сохранить паспорт без структуры', 'error');
            return;
        }

        me.mask('Сохранение...', win);
        B4.Ajax.request({
            url: B4.Url.action('MakeNewPassport', 'OkiProviderPassport'),
            params: {
                oldPaspId: oldPaspId,
                muId: muId,
                year: year,
                month: month,
                paspStructId: paspStructId
            }
        }).next(function(resp) {
            me.unmask();
            var response = Ext.decode(resp.responseText);
            if (!response.success) {
                B4.QuickMsg.msg('Ошибка', response.message.length > 0 ? response.message : 'Ошибка при сохранении паспорта!', 'error');
            } else {
                B4.QuickMsg.msg('Паспорт создан', 'Паспорт был успешно создан!', 'success');
            }
            win.up('myokiprovpaspgrid').getStore().load();
            win.close();
        }).error(function(response) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', response && response.message ? response.message : 'Ошибка при сохранении паспорта', 'error');
        });
    },

    loadInfo: function(field) {
        var me = this,
            win = field.up('okiprovpassportwin'),
            id = win.down('b4selectfield[name=Municipality]').getValue(),
            year = win.down('combobox[index=Year]').getValue(),
            month = win.down('combobox[index=Month]').getValue();

        win.down('displayfield[index=info]').hide();
        
        var combo = win.down('combobox[name=OldPaspPeriods]');

        if (combo) {
            combo.hide();
            combo.store.removeAll();
            combo.setValue(null);
        }

        if (year == null || month == null || id == 0) {
            return;
        }

        B4.Ajax.request({
            url: B4.Url.action('GetActivePassportStruct', 'OkiProviderPassport'),
            timeout: 60000,
            params: {
                muId: id,
                year: year,
                month: month
            }
        }).next(function (resp) {
            var response = Ext.decode(resp.responseText);
            win.paspStructId = null;
            
            if (response.Id) {
                if (response.existingPassports) {
                    Ext.Array.each(response.existingPassports, function (item) { combo.store.add(item); });
                    combo.setValue(response.existingPassports[0].Id);
                    combo.show();
                    win.down('displayfield[index=info]').setValue('Вы создаете паспорт на основе паспорта:');
                } else {
                    win.paspStructId = response.Id;
                    win.down('displayfield[index=info]').setValue('Вы создаете паспорт со структурой: ' + response.Name);
                }
            } else {
                var msg = "Нет ни одной структуры паспорта, действующей на данный период. Для создания паспорта необходимо создать структуру";
                win.down('displayfield[index=info]').setValue(msg);
                B4.QuickMsg.msg('Ошибка', msg, 'error');
            }
            win.down('displayfield[index=info]').show();
        }).error(function () {
            B4.QuickMsg.msg('Ошибка', 'Ошибка при получении данных', 'error');
            win.paspStructId = null;
        });
    },

    onRowAction: function(grid, action, rec) {
        switch (action.toLowerCase()) {
        case 'edit':
            this.editpassport(rec);
            break;
        case 'delete':
            this.deletePasp(grid, action, rec);
            break;
        }
    },

    deletePasp: function(grid, action, rec) {
        var permissions = ['Gkh1468.Passport.MyOki.Delete'];

        grid.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
            method: 'POST',
            params: {
                ids: Ext.encode([rec.getId()]),
                permissions: Ext.encode(permissions)
            }
        }).next(function(response) {
            var perm = Ext.decode(response.responseText)[0];
            grid.unmask();
            if (perm[0]) {
                Ext.Msg.confirm('Удаление записи!', 'При удалении паспорта теряются ранее заполненные вами данные. Продолжить?', function(result) {
                    if (result === 'yes') {
                        grid.mask();
                        rec.destroy().next(function(response) {
                            grid.unmask();
                            if (response.responseData.success) {
                                B4.QuickMsg.msg('Сообщение', 'Удаление прошло успешно', 'info');
                                grid.getStore().reload();
                            } else {
                                B4.QuickMsg.msg('Ошибка', 'Во время удаления произошла ошибка', 'error');
                            }
                        }).error(function(e) {
                            grid.unmask();
                            B4.QuickMsg.msg('Ошибка', 'Во время удаления произошла ошибка', 'error');
                        });
                    }
                });
            } else {
                B4.QuickMsg.msg('Ошибка', 'Удаление на данном статусе запрещено', 'error');
            }
        }).error(function () {
            grid.unmask();
            B4.QuickMsg.msg('Ошибка', 'Удаление на данном статусе запрещено', 'error');
        });
    }
});