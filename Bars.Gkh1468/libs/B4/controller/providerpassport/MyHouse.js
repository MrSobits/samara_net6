Ext.define('B4.controller.providerpassport.MyHouse', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.GkhDigitalSignatureGridAspect',
        'B4.Ajax', 'B4.Url',
        'B4.aspects.StateContextMenu'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        maks: 'B4.mixins.MaskBody'
    },

    views: [
        'providerpassport.myhouse.Grid',
        'providerpassport.myhouse.EditWindow'
    ],

    aspects: [
        {
            xtype: 'b4_state_contextmenu',
            name: 'myhousestatetransfermenu',
            gridSelector: 'myhouseprovpaspgrid',
            menuSelector: 'myhouseprovpaspgridstatemenu',
            stateType: 'houseproviderpassport'
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh1468.Passport.MyHouse.Create', applyTo: 'b4addbutton', selector: 'myhouseprovpaspgrid' },
                { name: 'Gkh1468.Passport.MyHouse.Delete', applyTo: 'b4deletecolumn', selector: 'myhouseprovpaspgrid',
                    applyBy: function(component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhdigitalsignaturegridaspect',
            gridSelector: 'myhouseprovpaspgrid',
            controllerName: 'HouseProviderPassport'
        }
    ],

    refs: [
        { ref: 'mainPanel', selector: 'myhouseprovpaspgrid' }
    ],

    init: function() {
        this.control({
            'myhouseprovpaspgrid combobox[index=Year]': { change: { fn: this.onComboBoxChange, scope: this } },

            'myhouseprovpaspgrid combobox[index=Month]': { change: { fn: this.onComboBoxChange, scope: this } },

            'myhouseprovpaspgrid b4addbutton': { click: { fn: this.showAddWindow, scope: this } },

            'myhouseprovpaspgrid b4updatebutton': { click: { fn: this.updateGrid, scope: this } },

            'houseprovpassportwin b4closebutton': { click: { fn: this.closeWindow, scope: this } },

            'houseprovpassportwin b4savebutton': { click: { fn: this.saveRec, scope: this } },

            'houseprovpassportwin b4selectfield[name=RealityObject]': {
                change: { fn: this.loadInfo, scope: this },
                beforeload: { fn: this.onBeforeRoLoad, scope: this }
            },

            'myhouseprovpaspgrid actioncolumn[action="openpassport"]': { click: { fn: this.editpassport, scope: this } },

            'houseprovpassportwin b4selectfield[name=Struct]': { beforeload: { fn: this.onStructBeforeLoad, scope: this } },

            'myhouseprovpaspgrid': {
                'rowaction': { fn: this.onRowAction, scope: this },
                'itemdblclick': { fn: this.rowDblClick, scope: this }
            },
            
            'houseprovpassportwin combobox[index="Month"]': { change: { fn: this.onCreatePeriodChange, scope: this } },
            'houseprovpassportwin combobox[index="Year"]': { change: { fn: this.onCreatePeriodChange, scope: this } }
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

    updateGrid: function(btn) {
        btn.up('myhouseprovpaspgrid').getStore().load();
    },

    rowDblClick: function(view, record) {
        this.editpassport(view, 0, 0, null, null, record);
    },

    editpassport: function(grid, rowIndex, colIndex, param, param2, rec) {
        Ext.History.add('housepasspeditor/' + rec.getId() + '/');
    },

    index: function() {
        var view = this.getMainPanel() || Ext.widget('myhouseprovpaspgrid');

        this.bindContext(view);
        this.application.deployView(view);
        view.getStore().load();
    },
    
    onCreatePeriodChange: function(field) {
        var win = field.up('houseprovpassportwin'),
            roSelectField = win.down('b4selectfield[name=RealityObject]');
        
        if (roSelectField) {
            roSelectField.setValue(null);
        }
    },

    showAddWindow: function(btn) {
        var grid = btn.up('myhouseprovpaspgrid'),
            win = Ext.widget('houseprovpassportwin');

        grid.add(win);
        win.show();
    },

    closeWindow: function(btn) {
        btn.up('houseprovpassportwin').close();
    },

    saveRec: function(btn) {
        var me = this,
            win = btn.up('houseprovpassportwin');

        if (!win.getForm().isValid()) {
            B4.QuickMsg.msg('Ошибка', 'Проверьте правильность заполнения формы', 'error');
            return;
        }

        var oldPaspId = win.down('combobox[name=OldPaspPeriods]').getValue(),
            roId = win.down('b4selectfield[name=RealityObject]').getValue(),
            year = win.down('combobox[index=Year]').getValue(),
            month = win.down('combobox[index=Month]').getValue(),
            paspStructId = win.paspStructId;

        if (!oldPaspId && !paspStructId) {
            B4.QuickMsg.msg('Ошибка', 'Невозможно сохранить паспорт без структуры', 'error');
            return;
        }

        me.mask('Сохранение...', win);

        B4.Ajax.request({
            url: B4.Url.action('MakeNewPassport', 'HouseProviderPassport'),
            params: {
                oldPaspId: oldPaspId,
                roId: roId,
                year: year,
                month: month,
                paspStructId: paspStructId
            }
        }).next(function() {
            B4.QuickMsg.msg('Паспорт создан', 'Паспорт был успешно создан', 'success');
            win.up('myhouseprovpaspgrid').getStore().load();
            me.unmask();
            win.close();
        }).error(function (response) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка', response && response.message ? response.message : 'Ошибка при сохранении паспорта', 'error');
        });
    },
    
    loadInfo: function (field, val) {
        var me = this,
            id = val && val.Id ? val.Id : 0,
            win = field.up('houseprovpassportwin'),
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

        this.roId = id;
        
        B4.Ajax.request({
            url: B4.Url.action('GetActivePassportStruct', 'HouseProviderPassport'),
            params: {
                roId: id,
                year: year,
                month: month
            }
        }).next(function(resp) {
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
        }).error(function() {
            B4.QuickMsg.msg('Ошибка', 'Ошибка при получении данных', 'error');
            win.paspStructId = null;
        });
    },

    onStructBeforeLoad: function(f, opts, store) {
        Ext.apply(store.getProxy().extraParams, {
            roId: this.roId
        });
    },

    onBeforeRoLoad: function(f, opts, store) {
        var win = f.up('houseprovpassportwin'),
            year = win.down('combobox[index=Year]').getValue(),
            month = win.down('combobox[index=Month]').getValue(),
            period = new Date(year, month > 0 ? month - 1 : month, 1, 0, 0, 0, 0);

        Ext.apply(store.getProxy().extraParams, {
            selectedPeriod: period
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

    deletePasp: function (grid, action, rec) {
        var permissions = ['Gkh1468.Passport.MyHouse.Delete'];
        
        grid.mask();
        B4.Ajax.request({
            url: B4.Url.action('GetObjectSpecificPermissions', 'Permission'),
            method: 'POST',
            params: {
                ids: Ext.encode([rec.getId()]),
                permissions: Ext.encode(permissions)
            }
        }).next(function (response) {
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
                        }).error(function() {
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