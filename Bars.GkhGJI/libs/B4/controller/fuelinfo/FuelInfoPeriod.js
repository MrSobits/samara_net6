Ext.define('B4.controller.fuelinfo.FuelInfoPeriod', {

    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    views: [
        'fuelinfo.FuelInfoPeriodGrid',
        'fuelinfo.AddWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'fuelinfoperiodgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'fuelinfoperiodGridWindowAspect',
            gridSelector: 'fuelinfoperiodgrid',
            editFormSelector: 'fuelinfoaddwindow',
            editWindowView: 'B4.view.fuelinfo.AddWindow',
            modelName: 'fuelinfo.FuelInfoPeriod',
            gridAction: function (grid, action) {
                if (!grid || grid.isDestroyed) return;
                if (this.fireEvent('beforegridaction', this, grid, action) !== false) {
                    switch (action.toLowerCase()) {
                        case 'add':
                            this.addRecord();
                            break;
                        case 'update':
                            this.updateGrid();
                            break;
                    }
                }
            },
            editRecord: function(record) {
                Ext.History.add(Ext.String.format('fuelinfodetail/{0}/', record.getId()));
            },
            addRecord: function () {
                var me = this,
                    model = me.getModel();

                me.setFormData(new model({ Id: 0 }));
                me.getForm().getForm().isValid();
            },
            saveRequestHandler: function () {
                var me = this,
                    form = this.getForm(),
                    rec;

                if (this.fireEvent('beforesaverequest', this) !== false) {
                    form.getForm().updateRecord();
                    rec = this.getRecordBeforeSave(form.getRecord());

                    this.fireEvent('getdata', this, rec);

                    if (form.getForm().isValid()) {
                        if (this.fireEvent('validate', this)) {

                            me.controller.mask('Сохранение', me.controller.getMainView());

                            B4.Ajax.request({
                                url: B4.Url.action('MassCreate', 'FuelInfoPeriod'),
                                params: {
                                    year: rec.get('Year'),
                                    month: rec.get('Month'),
                                    municipality: Ext.JSON.encode(rec.get('Municipality'))
                                },
                                timeout: 5 * 60 * 1000 // 5 минут
                            }).next(function (response) {
                                me.controller.getMainView().getStore().load();
                                me.controller.unmask();
                                form.close();
                                Ext.Msg.alert('Сохранено!', 'Периоды успешно созданы');
                                return true;
                            }).error(function (response) {
                                me.controller.unmask();
                                form.close();
                                Ext.Msg.alert('Ошибка!', response.message);
                            });

                        }
                    } else {
                        var errorMessage = this.getFormErrorMessage(form);
                        Ext.Msg.alert('Ошибка сохранения!', errorMessage);
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'GkhGji.HeatSeason.FuelInfoPeriod.Create', applyTo: 'b4addbutton', selector: 'fuelinfoperiodgrid' },
                { name: 'GkhGji.HeatSeason.FuelInfoPeriod.Delete', applyTo: 'b4deletecolumn', selector: 'fuelinfoperiodgrid' }
            ]
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('fuelinfoperiodgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});