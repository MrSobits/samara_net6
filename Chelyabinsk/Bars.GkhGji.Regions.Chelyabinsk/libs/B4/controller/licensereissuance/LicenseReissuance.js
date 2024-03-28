Ext.define('B4.controller.licensereissuance.LicenseReissuance', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.StateContextMenu',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhGridEditForm'
    ],

    models: ['licensereissuance.LicenseReissuance'],
    stores: ['licensereissuance.LicenseReissuance'],
    views: [
        'licensereissuance.AddWindow',
        'licensereissuance.Grid',
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'specialaccountgridAspect',
        //    gridSelector: 'specialaccountgrid',
        //    editFormSelector: '#specialAccountAddWindow',
        //    storeName: 'specialaccount.SpecialAccountReport',
        //    modelName: 'specialaccount.SpecialAccountReport',
        //    editWindowView: 'specialaccount.AddWindow'
        //},
        {
            xtype: 'b4_state_contextmenu',
            name: 'licenseReissuanceStateTransferAspect',
            gridSelector: 'licensereissuancegrid',
            menuSelector: 'rmanorglicenserequestgridStateMenu',
            stateType: 'gkh_manorg_license_reissuance'
        },
        {
            xtype: 'gkhgrideditformaspect',
            name: 'licenseReissuanceGridWindowAspect',
            gridSelector: 'licensereissuancegrid',
            editFormSelector: '#licenseReissuanceAddWindow',
            storeName: 'licensereissuance.LicenseReissuance',
            modelName: 'licensereissuance.LicenseReissuance',
            editWindowView: 'licensereissuance.AddWindow',
            controllerEditName: 'B4.controller.licensereissuance.Edit',
            //   controllerEditName: 'B4.controller.specialaccount.Navigation',
            onSaveSuccess: function (aspect, rec) {
                //Закрываем окно после добавления новой записи
                debugger;
                aspect.getForm().close();
                debugger;
                //загружаем добавленный объект
                var model = this.controller.getModel(this.modelName);
                debugger;
                model.load(rec.getId(), {
                    success: function (record) {
                        //После загрузки объекта подменяем параметр и открываем вкладку
                        this.editRecord(record);
                        debugger;
                    },
                    scope: this
                });
            },
            rowAction: function (grid, action, record) {
                if (this.fireEvent('beforerowaction', this, grid, action, record) !== false) {
                    switch (action.toLowerCase()) {
                        case 'edit':
                            this.editRecord(record);
                            break;
                        case 'delete':
                            this.deleteRecord(grid, action, record);
                            break;
                    }
                }
            },
            editRecord: function (record) {
                debugger;
                var me = this,

                    id = record ? record.get('Id') : null,
                    model;
                debugger;
                model = me.controller.getModel(me.modelName);
                var p = me.controller.getModel(me.modelName);
                var v = me.controllerEditName;
                if (id) {
                    if (me.controllerEditName) {
                        me.controller.application.redirectTo(Ext.String.format('licensereissuanceeditor/{0}', id));
                    } else {
                        model.load(id, {
                            success: function (rec) {
                                me.setFormData(rec);
                            },
                            scope: me
                        });
                    }
                } else {
                    me.setFormData(new model({ Id: 0 }));
                }
            },
            deleteRecord: function (grid, action, rec) {
                debugger;
                Ext.Msg.confirm('Удаление записи!',
                    'При удалении отчета теряются ранее заполненные вами данные. Продолжить?',
                    function (result) {
                        if (result === 'yes') {
                            grid.mask();
                            rec.destroy().next(function (response) {
                                grid.unmask();
                                if (response.responseData.success) {
                                    B4.QuickMsg.msg('Сообщение', 'Удаление прошло успешно', 'info');
                                    grid.getStore().reload();
                                } else {
                                    B4.QuickMsg.msg('Ошибка', 'Во время удаления произошла ошибка', 'error');
                                }
                            }).error(function () {
                                grid.unmask();
                                B4.QuickMsg.msg('Ошибка', 'Во время удаления произошла ошибка', 'error');
                            });
                        }
                    })
            }
        }
    ],

    mainView: 'licensereissuance.Grid',
    mainViewSelector: 'licensereissuancegrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'licensereissuancegrid'
        }
    ],

    init: function () {
        this.control({
            'licensereissuancegrid b4updatebutton': { click: { fn: this.updateGrid, scope: this } },
        });

        this.callParent(arguments);
    },

    updateGrid: function (btn) {
        btn.up('licensereissuancegrid').getStore().load();
    },

    getGrid: function () {
        return this.getMainView();
    },

    index: function () {
        var view = this.getMainView() || Ext.widget('licensereissuancegrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('licensereissuance.LicenseReissuance').load();
    }
});
