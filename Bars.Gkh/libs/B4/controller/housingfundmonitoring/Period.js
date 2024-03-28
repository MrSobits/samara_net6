Ext.define('B4.controller.housingfundmonitoring.Period', {

    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    views: [
        'housingfundmonitoring.PeriodGrid',
        'housingfundmonitoring.AddWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    refs: [
        {
            ref: 'mainView',
            selector: 'housingfundmonitoringperiodgrid'
        }
    ],

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'housingfundmonitoringperiodWindowAspect',
            gridSelector: 'housingfundmonitoringperiodgrid',
            editFormSelector: 'housingfundmonitoringaddwindow',
            editWindowView: 'B4.view.housingfundmonitoring.AddWindow',
            modelName: 'housingfundmonitoring.HousingFundMonitoringPeriod',
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
                Ext.History.add(Ext.String.format('housingfundmonitoringdetail/{0}/', record.getId()));
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
                                url: B4.Url.action('MassCreate', 'HousingFundMonitoringPeriod'),
                                params: {
                                    year: rec.get('Year'),
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
                { name: 'Gkh.HousingFundMonitoringPeriod.Create', applyTo: 'b4addbutton', selector: 'housingfundmonitoringperiodgrid' },
                { name: 'Gkh.HousingFundMonitoringPeriod.Delete', applyTo: 'b4deletecolumn', selector: 'housingfundmonitoringperiodgrid' }
            ]
        }
    ],

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('housingfundmonitoringperiodgrid');
        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});