Ext.define('B4.controller.fssp.AddressMatching', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    refs: [
        {
            ref: 'ImportedAddressRegistry',
            selector: 'importedaddressmatchingregistry'
        },
        {
            ref: 'PgmuAddressGrid',
            selector: 'fsspaddressmatchingpanel pgmuaddressgrid'
        },
    ],

    views: [
        'fssp.courtordergku.ImportedAddressMatchingRegistry',
        'fssp.courtordergku.PgmuAddressGrid'
    ],
    
    models: ['fssp.addressmatching.ImportedAddress'],

    mainView: 'fssp.courtordergku.AddressMatchingPanel',
    mainViewSelector: 'fsspaddressmatchingpanel',
    showAll: false,

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'fsspAddressMatchGridWindowAspect',
            gridSelector: 'importedaddressmatchingregistry',
            modelName: 'fssp.addressmatching.ImportedAddress',
            rowAction: function(grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'match':
                            this.controller.matchAddress(record);
                            break;
                        case 'mismatch':
                            this.controller.mismatchAddress(record);
                            break;
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                {
                    name: 'Clw.Fssp.CourtOrderGku.AddressMatchingRegistry.ShowAll',
                    selector: 'importedaddressmatchingregistry',
                    applyBy: function (component, allowed) {
                        this.controller.showAll = allowed;
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.AddressMatchingRegistry.Match', 
                    applyTo: '[name=match]', 
                    selector: 'importedaddressmatchingregistry',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                },
                {
                    name: 'Clw.Fssp.CourtOrderGku.AddressMatchingRegistry.Mismatch',
                    applyTo: '[name=mismatch]',
                    selector: 'importedaddressmatchingregistry',
                    applyBy: function (component, allowed) {
                        if (allowed) {
                            component.show();
                        } else {
                            component.hide();
                        }
                    }
                }
            ]
        }
    ],

    init: function() {
        var me = this,
            actions = {
                'importedaddressmatchingregistry': {
                    render: { fn: me.onRenderImportedAddressMatchingRegistry, scope: me }
                },
                'fsspaddressmatchingpanel pgmuaddressgrid': {
                    render: { fn: me.onRenderPgmuAddressGrid, scope: me }
                }
            };

        me.control(actions);
        me.callParent(arguments);
    },

    onRenderPgmuAddressGrid: function(grid) {
        var store = grid.getStore();
        
        store.load();
    },

    onRenderImportedAddressMatchingRegistry: function(grid) {
        var me = this,
            matchingRegistryStore = grid.getStore();

        matchingRegistryStore.on('beforeload', function(store, operation){
            operation.params = operation.params || {};
            operation.params.showAll = me.showAll;
        });

        matchingRegistryStore.load();
    },

    // Вызов функции сопоставления адресов
    matchAddress: function(record) {
        var me = this,
            fiasGrid = me.getPgmuAddressGrid(),
            fiasSelectRecord = fiasGrid.getSelectionModel().getSelection()[0],
            id = record ? record.getId() : null;

        if (!fiasSelectRecord) {
            B4.QuickMsg.msg('Внимание', 'Выберите адрес ПГМУ', 'warning');
            return;
        }
        
        Ext.MessageBox.show({
            title: 'Сопоставление адреса',
            modal: true,
            msg: 'Вы уверены что хотите сопоставить выбранные адреса?',
            buttonText: { yes: "Да", no: "Нет" },
            fn: function(btn) {
                switch (btn) {
                    case 'yes':
                        me.matchSelectedAddresses(id, fiasSelectRecord.getId());
                        break;
                }
            }
        });
    },

    // Вызов функции удаления связи
    mismatchAddress: function (record) {
        var me = this,
            id = record ? record.getId() : null;

        if (!record.data.IsMatched) {
            B4.QuickMsg.msg('Внимание', 'Выбранная запись не имеет связи с адресом ПГМУ', 'warning');
            return;
        }
        Ext.MessageBox.show({
            title: 'Удаление',
            modal: true,
            msg: 'Разорвать связь с адресом ПГМУ?',
            buttonText: { yes: "Да", no: "Нет" },
            fn: function (btn) {
                if (btn == 'yes') {
                    me.mismatchSelectedAddresses(id);
                }
            }
        });
    },

    // Сопоставить выбранные адреса
    matchSelectedAddresses: function(fsspAddressId, pgmuAddressId) {
        var me = this,
            store = me.getImportedAddressRegistry().getStore();

        me.mask('Сохранение', me.getMainComponent());

        B4.Ajax.request(
            {
                method: 'POST',
                url: B4.Url.action('AddressMatch', 'FsspAddress'),
                params: {
                    fsspAddressId: fsspAddressId,
                    pgmuAddressId: pgmuAddressId
                },
                timeout: 999999
            }).next(function (response) {
            var responseObj = Ext.decode(response.responseText);
            me.unmask();
            B4.QuickMsg.msg(
                responseObj.success ? 'Успешно' : 'Внимание!',
                responseObj.success ? 'Адреса сопоставлены' : 'Не удалось сопоставить выбранные адреса',
                responseObj.success ? 'success' : 'warning');
            store.load();
        }).error(function (response) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка!', 'Не удалось сохранить новый адрес', 'error');
        });
    },

    // Удалить связь адреса ФССП с адресом ПГМУ
    mismatchSelectedAddresses: function (id) {
        var me = this,
            store = me.getImportedAddressRegistry().getStore();

        B4.Ajax.request(
            {
                method: 'POST',
                url: B4.Url.action('AddressMismatch', 'FsspAddress'),
                params: {
                    id: id
                },
                timeout: 999999
            }).next(function (response) {
            var responseObj = Ext.decode(response.responseText);
            me.unmask();
            B4.QuickMsg.msg(
                responseObj.success ? 'Успешно' : 'Внимание!',
                responseObj.success ? 'Связь удалена' : 'Не удалось удалить связь с адресом ПГМУ',
                responseObj.success ? 'success' : 'warning');
            store.load();
        }).error(function (response) {
            me.unmask();
            B4.QuickMsg.msg('Ошибка!', 'Не удалось удалить связь с адресом ПГМУ', 'error');
        });
    }
});