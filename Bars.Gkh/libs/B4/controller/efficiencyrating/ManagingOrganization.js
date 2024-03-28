/* 
* Контроллер рейтинга эффективности УО
*/
Ext.define('B4.controller.efficiencyrating.ManagingOrganization', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditCtxWindow',
        'B4.enums.efficiencyrating.DataMetaObjectType',
        'B4.aspects.permission.GkhPermissionAspect'
    ],

    models: [
        'B4.model.efficiencyrating.ManagingOrganization',
        'B4.model.efficiencyrating.ManagingOrganizationGrid'
    ],
    stores: ['B4.store.efficiencyrating.ManagingOrganizationGrid'],
    views:  ['efficiencyrating.manorg.Grid'],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'efficiencyrating.manorg.Grid',
    mainViewSelector: 'efficiencyratingManorgGrid',

    aspects: [
        {
            xtype: 'grideditformaspect',
            name: 'ManagingOrganizationEditFormAspect',
            gridSelector: 'efficiencyratingManorgGrid',
            modelName: 'efficiencyrating.ManagingOrganization',
            editRecord: function (record) {
                var me = this,
                    maonrgId = record.getId(),
                    objectId = record.get('ObjectId'),
                    model = B4.model.efficiencyrating.ManagingOrganization,
                    rec,
                    periodId = me.controller.getFilterParams().periodId,
                    grid = me.getGrid();

                if (!objectId) {
                    me.mask('Загрузка...', grid);
                    rec = new model({ ManagingOrganization: maonrgId, Period: periodId });
                    rec.save()
                        .next(function(result) {
                                me.unmask();

                                if (result.responseData && result.responseData.data) {
                                    var data = result.responseData.data;
                                    if (data.length > 0) {
                                        var id = data[0] instanceof Object ? data[0].Id : data[0];

                                        if (id) {
                                            me.redirectToEdit(id);
                                            return;
                                        }
                                    }

                                    Ext.Msg.alert('Ошибка', 'Ошибка во время сохранения записи!');
                                }
                            }, me)
                        .error(function(result) {
                                me.unmask();
                                me.fireEvent('savefailure', result.record, result.responseData);

                                Ext.Msg.alert('Ошибка во время создания записи!', Ext.isString(result.responseData) ? result.responseData : result.responseData.message);
                            }, me);
                } else {
                    me.redirectToEdit(objectId);
                }
            },

            redirectToEdit: function(objectId) {
                Ext.History.add('efmanorg_rating/' + objectId);
            },

            otherActions: function(actions) {
                var me = this;

                actions[me.gridSelector + ' b4updatebutton'] = {
                    click: {
                        fn: function (btn) {
                            var me = this;

                            if (!me.controller.isValidParams()) {
                                return false;
                            }

                            this.getGrid().getSelectionModel().deselectAll();
                            this.getGrid().getStore().load();
                            return true;
                        },
                        scope: me
                    }
                };

                actions[me.gridSelector + ' b4pagingtoolbar'] = {
                    beforechange: {
                        fn: function () { return me.controller.isValidParams(); },
                        scope: me
                    }
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
               { name: 'Gkh.Orgs.EfficiencyRating.ManagingOrganization.MassCalcValues', applyTo: 'button[actionName=calcvalues]', selector: 'efficiencyratingManorgGrid' },
               { name: 'Gkh.Orgs.EfficiencyRating.ManagingOrganization.View', applyTo: 'b4selectfield[name=EfficiencyRatingPeriod]', selector: 'efficiencyratingManorgGrid' }
            ]
        }
    ],

    getFilterParams: function() {
        var me = this,
            view = me.getMainView(),
            sfMunicipalityValue = view.down('b4selectfield[name=Municipality]').getValue(),
            sfEfficiencyRatingPeriodValue = view.down('b4selectfield[name=EfficiencyRatingPeriod]').getValue(),
            headerFilters = view.getHeaderFilters(),
            selectedIds = view.getSelectionModel().getSelection(),
            objectIds = selectedIds
                ? Ext.Array.map(selectedIds, function(elem) {
                        return {
                            Id: elem.get('ObjectId'),
                            ManorgId: elem.get('Id')
                        }
                    })
                : [];

        return {
            periodId: sfEfficiencyRatingPeriodValue,
            municipalityIds: Ext.encode(sfMunicipalityValue),
            complexFilter: Ext.encode(headerFilters),
            objectIds: Ext.encode(objectIds)
        }
    },

    isValidParams: function () {
        var me = this;

        if (!me.getFilterParams().periodId) {
            Ext.Msg.alert('Ошибка', 'Выберите период рейтинга эффективности!');
            return false;
        }

        return true;
    },

    onBeforeLoad: function(store, operation) {
        operation.params = Ext.apply(operation.params || {}, this.getFilterParams());
    },

    onCalcBtnClick: function() {
        var me = this,
           filterParams = me.getFilterParams(),
           view = me.getMainView(),
           params = {
               dataMetaObjectType: B4.enums.efficiencyrating.DataMetaObjectType.EfficientcyRating
           };

        Ext.apply(params, filterParams);

        if (!me.isValidParams()) {
            return;
        }

        Ext.Msg.confirm('Массовый расчет', 'Вы действительно хотите запустить массовый расчет показателей по выбранным управляющим организациям?',
            function(result) {
                if (result == 'yes') {
                    me.mask('Массовый расчет показателей', view);
                    B4.Ajax.request({
                        url: B4.Url.action('CalcMass', 'BaseDataValue'),
                        method: 'POST',
                        timeout: 2 * 60 * 60 * 1000, // 2 часа
                        params: params
                    })
                    .next(function (response) {
                        me.unmask();
                        Ext.Msg.alert('Массовый расчет показателей', 'Расчет показателей эффективности согласно выбранным фильтрам успешно выполнен');
                        me.getMainView().getStore().load();
                    })
                    .error(function (response) {
                        me.unmask();
                        Ext.Msg.alert('Ошибка!', response.message || 'При выполнении запроса произошла ошибка!');
                    });
                }
            });
    },

    init: function () {
        this.callParent(arguments);

        this.control({
            'efficiencyratingManorgGrid': {
                'afterrender': {
                    fn: function (grid) {
                        var me = this;
                        grid.getStore().on('beforeload', me.onBeforeLoad, me);
                    },
                    scope: this
                }
            },

            'efficiencyratingManorgGrid toolbar b4selectfield': {
                'change': {
                    fn: function (sf, newVal, oldVal) {
                        var me = this,
                            view = me.getMainView(),
                            sfEfficiencyRatingPeriod = view.down('b4selectfield[name=EfficiencyRatingPeriod]');

                        if (sfEfficiencyRatingPeriod.getValue()) {
                            view.getSelectionModel().deselectAll();
                            view.getStore().load();
                        }

                        if (sf.name === 'EfficiencyRatingPeriod' && !newVal) {
                            view.getStore().removeAll();
                        }
                    },
                    scope: this
                }
            },

            'efficiencyratingManorgGrid button[actionName=calcvalues]': { 'click': { fn: this.onCalcBtnClick, scope: this } },
            'efficiencyratingManorgGrid toolbar b4selectfield[name=EfficiencyRatingPeriod]': {
                'afterrender': {
                    fn: function(sf) {
                        sf.getStore().on('beforeload', function(st, operation) {
                                    operation.params.fromEfRating = true; /* Скрываем периоды без реализованных конструкторов */
                                });
                    },
                    scope: this
                }
            }
        });
    },

    index: function() {
        var me = this,
           view = me.getMainView() || Ext.widget(me.mainViewSelector);

        me.bindContext(view);
        me.application.deployView(view);

        if (me.getFilterParams().periodId) {
            view.getStore().load();
        }
    }
});