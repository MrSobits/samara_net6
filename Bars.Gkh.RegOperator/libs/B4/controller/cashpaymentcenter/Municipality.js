Ext.define('B4.controller.cashpaymentcenter.Municipality', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'cashpaymentcenter.Municipality'
    ],

    stores: [
        'cashpaymentcenter.Municipality',
        'dict.MunicipalityForSelect',
        'dict.MunicipalityForSelected'
    ],

    views: [
        'cashpaymentcenter.MunicipalityGrid',
        'SelectWindow.MultiSelectWindow'
    ],

    refs: [
      {
            ref: 'mainView',
            selector: 'cashpaymentcentermugrid'
      }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Gkh.Orgs.CashPaymentCenter.Municipality.Create', applyTo: 'b4addbutton', selector: 'cashpaymentcentermugrid' },
                { name: 'Gkh.Orgs.CashPaymentCenter.Municipality.Delete', applyTo: 'b4deletecolumn', selector: 'cashpaymentcentermugrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        },
        {
            xtype: 'gkhgridmultiselectwindowaspect',
            name: 'cashpaymentcenterMuAspect',
            gridSelector: 'cashpaymentcentermugrid',
            storeName: 'cashpaymentcenter.Municipality',
            modelName: 'cashpaymentcenter.Municipality',
            multiSelectWindow: 'SelectWindow.MultiSelectWindow',
            multiSelectWindowSelector: '#cashpaymentcenterMuMultiSelectWindow',
            storeSelect: 'dict.MunicipalityForSelect',
            storeSelected: 'dict.MunicipalityForSelected',
            titleSelectWindow: 'Выбор муниципальных образований',
            titleGridSelect: 'Муниципальные образования для отбора',
            titleGridSelected: 'Выбранные муниципальные образования',
            columnsGridSelect: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, filter: { xtype: 'textfield' } }
            ],
            columnsGridSelected: [
                { header: 'Наименование', xtype: 'gridcolumn', dataIndex: 'Name', flex: 1, sortable: false }
            ],
            updateGrid: function () {
                this.getGrid().getStore().load();
            },
            listeners: {
                getdata: function (asp, records) {
                    var recordIds = [];

                    records.each(function (rec) {
                        recordIds.push(rec.get('Id'));
                    });

                    if (recordIds[0] > 0) {
                        asp.controller.mask('Сохранение', asp.controller.getMainComponent());
                        B4.Ajax.request({
                            url: B4.Url.action('AddMunicipalities', 'CashPaymentCenter'),
                            method: 'POST',
                            params: {
                                muIds: Ext.encode(recordIds),
                                cashPaymentCenterId: asp.controller.getContextValue(asp.controller.getMainView(), 'cashPaymentCenterId')
                            }
                        }).next(function () {
                            asp.controller.unmask();
                            asp.controller.getMainView().getStore().load();
                            Ext.Msg.alert('Сохранение!', 'Муниципальные образования сохранены успешно');
                            return true;
                        }).error(function () {
                            asp.controller.unmask();
                        });
                    }
                    else {
                        Ext.Msg.alert('Ошибка!', 'Необходимо выбрать муниципальные образования');
                        return false;
                    }
                    return true;
                }
            }
        }
    ],


    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('cashpaymentcentermugrid');
        me.bindContext(view);
        me.setContextValue(view, 'cashPaymentCenterId', id);
        me.application.deployView(view, 'cashpayment_center');
        
        view.getStore().filter('cashPaymentCenterId', id);
    }
});