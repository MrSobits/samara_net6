Ext.define('B4.controller.objectcr.FinanceSourceRes', {
    /*
    * Контроллер раздела средства источника финансирования
    */
    extend: 'B4.controller.MenuItemController',

    requires:
    [
        'B4.aspects.GridEditCtxWindow',
        'B4.aspects.permission.GkhStatePermissionAspect',
        'B4.aspects.GkhTriggerFieldMultiSelectWindow'
    ],

    models: ['objectcr.FinanceSourceRes'],
    stores: ['objectcr.FinanceSourceRes'],
    views: ['objectcr.FinanceSourceResGrid',
            'objectcr.FinanceSourceResEditWindow'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    parentCtrlCls: 'B4.controller.objectcr.Navi',

    mainView: 'objectcr.FinanceSourceResGrid',
    mainViewSelector: 'financesourceresgrid',

    aspects: [
        {
            xtype: 'gkhstatepermissionaspect',
            name: 'financeSourceResObjectPerm',
            permissions: [
                { name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Create', applyTo: 'b4addbutton', selector: 'financesourceresgrid' },
                { name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Edit', applyTo: 'b4savebutton', selector: 'financesourcereseditwin' },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Delete', applyTo: 'b4deletecolumn', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetMuIncome', applyTo: '[dataIndex=BudgetMuIncome]', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetMuPercent', applyTo: '[dataIndex=BudgetMuPercent]', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetSubjectIncome', applyTo: '[dataIndex=BudgetSubjectIncome]', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Column.BudgetSubjectPercent', applyTo: '[dataIndex=BudgetSubjectPercent]', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Column.FundResourceIncome', applyTo: '[dataIndex=FundResourceIncome]', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
                },
                {
                    name: 'GkhCr.ObjectCr.Register.FinanceSourceRes.Column.FundResourcePercent', applyTo: '[dataIndex=FundResourcePercent]', selector: 'financesourceresgrid',
                    applyBy: function (component, allowed) { if (allowed) { component.show(); } else { component.hide(); } }
                }
            ]
        },
        {
            /*
            * Аспект взаимодействия таблицы и формы редактирования средств источника финансирования
            */
            xtype: 'grideditctxwindowaspect',
            name: 'financeSourceResGridWindowAspect',
            gridSelector: 'financesourceresgrid',
            editFormSelector: 'financesourcereseditwin',
            modelName: 'objectcr.FinanceSourceRes',
            editWindowView: 'objectcr.FinanceSourceResEditWindow',
            otherActions: function (actions) {
                var me = this;

                actions['financesourcereseditwin [name=TypeWorkCr]'] = { 'beforeload': { fn: me.beforeLoadTypeWork, scope: me } };
            },
            listeners: {
                beforesave: function (asp, record) {
                    var form = asp.getForm();
                    asp.controller.mask('Сохранение', form);
                    if (record.phantom) {

                        B4.Ajax.request({
                            url: B4.Url.action('AddFinSources', 'FinanceSourceResource'),
                            params: {
                                years: Ext.encode(record.get('Year')),
                                record: Ext.JSON.encode(record.data),
                                typeWorkId: record.get('TypeWorkCr'),
                                objectCrId: record.get('ObjectCr')
                            }
                        }).next(function () {
                            form.close();
                            asp.controller.unmask();
                            asp.getGrid().getStore().load();
                        }).error(function (result) {
                            Ext.Msg.alert('Ошибка удаления!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            asp.controller.unmask();
                        });

                        return false;
                    } else {
                        return true;
                    }
                },
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.ObjectCr = this.controller.getContextValue(this.controller.getMainComponent(), 'objectCrId');
                    }
                },
                aftersetformdata: function (asp, record) {
                    var yearStart = 0,
                        shortPeriod = 0,
                        form = asp.getForm(),
                        grid = asp.getGrid(),
                        formFinanceSource = grid.formFinanceSource,
                        showOtherFinSource = grid.showOtherFinSource,
                        yearField = form.down('[name=Year]'),
                        typeWorkField = form.down('[name=TypeWorkCr]'),
                        yearsStore = yearField.getStore(),
                        finSourceField = form.down('[name=FinanceSource]'),
                        otherSourceField = form.down('[name=OtherResource]');

                    if (formFinanceSource == 0) {
                        finSourceField.hide();
                        finSourceField.allowBlank = true;
                        yearField.show();
                        typeWorkField.show();
                    } else {
                        finSourceField.show();
                        yearField.hide();
                        yearField.allowBlank = true;
                        typeWorkField.hide();
                        typeWorkField.allowBlank = true;
                    }

                    if (formFinanceSource == 0 && showOtherFinSource == 10) {
                        otherSourceField.show();
                    } else {
                        otherSourceField.hide();
                    }

                    yearField.setReadOnly(!record.phantom);
                    typeWorkField.setReadOnly(!record.phantom);

                    yearsStore.on('load', function (st) {
                        var config = {};

                        if (Gkh.config.Overhaul.OverhaulHmao) {
                            config = Gkh.config.Overhaul.OverhaulHmao;
                        }
                        else if (Gkh.config.Overhaul.OverhaulNso) {
                            config = Gkh.config.Overhaul.OverhaulNso;
                        }
                        else if (Gkh.config.Overhaul.OverhaulTat) {
                            config = Gkh.config.Overhaul.OverhaulTat;
                        }

                        yearStart = config.ProgrammPeriodStart;
                        shortPeriod = config.ShortTermProgPeriod;

                        for (var y = yearStart; y < yearStart + shortPeriod; y++) {
                            st.add({ Year: y });
                        }
                    });
                }
            },
            beforeLoadTypeWork: function (store, operation) {
                operation.params = operation.params || {};
                operation.params.objectCrId = this.controller.getContextValue(this.controller.getMainComponent(), 'objectCrId');
            }
        }
    ],

    index: function (id) {
        var me = this,
            view = me.getMainView() || Ext.widget('financesourceresgrid'),
            store,
            colTypeWorkCr = view.down('gridcolumn[dataIndex=TypeWorkCr]'),
            colOtherResources = view.down('gridcolumn[dataIndex=OtherResource]');

        view.formFinanceSource = Gkh.config.GkhCr.General.FormFinanceSource;
        view.showOtherFinSource = Gkh.config.GkhCr.General.TypeOtherFinSourceCalc;

        if (view.formFinanceSource === 0) {
            colTypeWorkCr.show();
        } else {
            colTypeWorkCr.hide();
        }

        if (view.formFinanceSource === 0 && view.showOtherFinSource === 10) {
            colOtherResources.show();
        } else {
            colOtherResources.hide();
        }

        me.getAspect('financeSourceResObjectPerm').setPermissionsByRecord({ getId: function () { return id; } });

        me.bindContext(view);
        me.setContextValue(view, 'objectCrId', id);
        me.application.deployView(view, 'objectcr_info');

        store = view.getStore();
        store.clearFilter(true);
        store.filter('objectCrId', id);
    }
});