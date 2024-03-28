Ext.define('B4.controller.dict.Tariff', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.EntityChangeLog',
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.GkhPermissionAspect',
        'B4.enums.TypeServiceGis',
        'B4.Ajax',
        'B4.Url'
    ],

    models: [
        'dict.GisTariff'
    ],

    stores: [
        'dict.GisTariff'
    ],

    views: [
        'dict.tariff.Grid',
        'dict.tariff.EditWindow'
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    mainView: 'dict.tariff.Grid',
    mainViewSelector: 'tariffdictgrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'tariffdictgrid'
        }
    ],

    aspects: [
        {
            xtype: 'gkhpermissionaspect',
            name: 'gisTariffPermissionAspect',
            applyBy: function (component, allowed) {
                if (component) {
                    component.isAllowed = allowed;
                    component.setDisabled(!allowed);
                    component.setVisible(allowed);
                }
            },
            permissions: [
                { name: 'Gis.Dict.Tariff.Create', applyTo: 'b4addbutton', selector: 'tariffdictgrid' },
                { name: 'Gis.Dict.Tariff.Edit', applyTo: 'b4savebutton', selector: 'paymentdocinfoeditwin' },
                {
                    name: 'Gis.Dict.Tariff.Delete', applyTo: 'b4deletecolumn', selector: 'tariffdictgrid',
                    applyBy: function (component, allowed) {
                        component.setVisible(allowed);
                    }
                },
                {
                    name: 'Gis.Dict.Tariff.Field.Contragent_Edit', applyTo: 'field[name=Contragent]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.ActivityKind_Edit', applyTo: 'field[name=ActivityKind]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.ContragentName_Edit', applyTo: 'field[name=ContragentName]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.EaisUploadDate_Edit', applyTo: 'field[name=EaisUploadDate]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.EaisEditDate_Edit', applyTo: 'field[name=EaisEditDate]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.Municipality_Edit', applyTo: 'field[name=Municipality]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.Service_Edit', applyTo: 'field[name=Service]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.UnitMeasure_Edit', applyTo: 'field[name=UnitMeasure]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.StartDate_Edit', applyTo: 'field[name=StartDate]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.EndDate_Edit', applyTo: 'field[name=EndDate]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.TariffKind_Edit', applyTo: 'combobox[name=TariffKind]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.ZoneCount_Edit', applyTo: 'combobox[name=ZoneCount]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.TariffValue_Edit', applyTo: 'field[name=TariffValue]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.TariffValue1_Edit', applyTo: 'field[name=TariffValue1]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.TariffValue2_Edit', applyTo: 'field[name=TariffValue2]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.TariffValue3_Edit', applyTo: 'field[name=TariffValue3]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.IsNdsInclude_Edit', applyTo: 'checkbox[name=IsNdsInclude]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.IsSocialNorm_Edit', applyTo: 'checkbox[name=IsSocialNorm]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.IsMeterExists_Edit', applyTo: 'checkbox[name=IsMeterExists]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.IsElectricStoveExists_Edit', applyTo: 'checkbox[name=IsElectricStoveExists]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.Floor_Edit', applyTo: 'field[name=Floor]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.ConsumerType_Edit', applyTo: 'combobox[name=ConsumerType]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.SettelmentType_Edit', applyTo: 'combobox[name=SettelmentType]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.ConsumerByElectricEnergyType_Edit', applyTo: 'combobox[name=ConsumerByElectricEnergyType]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.RegulatedPeriodAttribute_Edit', applyTo: 'textarea[name=RegulatedPeriodAttribute]', selector: 'tariffdicteditpanel'
                },
                {
                    name: 'Gis.Dict.Tariff.Field.BasePeriodAttribute_Edit', applyTo: 'textarea[name=BasePeriodAttribute]', selector: 'tariffdicteditpanel'
                }
            ]
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'tariffDictGridEditWindowAspect',
            gridSelector: 'tariffdictgrid',
            editFormSelector: 'tariffdicteditwin',
            modelName: 'dict.GisTariff',
            editWindowView: 'dict.tariff.EditWindow',
            otherActions: function (actions) {
                var me = this;
                actions[me.editFormSelector + ' b4selectfield[name=Service]'] = {
                     change: { fn: me.onServiceChange, scope: me }
                };
                actions[me.editFormSelector + ' b4combobox[name=ZoneCount]'] = {
                    change: { fn: me.onZoneCountChange, scope: me }
                };
            },
            listeners: {
                beforesetformdata: function (asp, record) {
                    var me = this,
                        id = record.getId() || 0,
                        editWin = me.getForm(),
                        uploadDate = record.get('EiasUploadDate'),
                        editDate = record.get('EiasEditDate'),
                        dateFields = editWin.down('[name=EaisDateFields]'),
                        entityChangeLogAspect = asp.controller.getAspect('entityChangeLogAspect');

                    if (uploadDate || editDate) {
                        dateFields.setVisible(true);
                        dateFields.setDisabled(false);
                    }

                    if (id > 0) {
                        editWin.setTitleAction('Редактирование');
                        editWin.down('b4selectfield[name=Municipality]').setReadOnly(true);
                        editWin.down('b4selectfield[name=Service]').setReadOnly(true);
                    }

                    entityChangeLogAspect.getEntityId = function () {
                        return id;
                    }

                    return true;
                }
            },
            onServiceChange: function(component, newValue) {
                var me = this,
                    isElectricTarif = newValue && newValue.Code === 25 || false, // электроснабжение
                    isHeatingTarif = newValue && newValue.Code === 8 || false, // отопление
                    editWin = me.getForm(),
                    unitMeasure = editWin.down('[name=UnitMeasure]'),
                    unitMeasureValue = '';

                if (newValue && newValue.UnitMeasure) {
                    unitMeasureValue = typeof newValue.UnitMeasure === 'object'
                                       ? newValue.UnitMeasure.Name
                                       : newValue.UnitMeasure;
                }

                unitMeasure.setValue(unitMeasureValue);
                me.visibleContainer(editWin.down('[name=Floor]'), isHeatingTarif);
                me.visibleContainer(editWin.down('[name=OtherTariffBlock]'), !isElectricTarif);
                me.visibleContainer(editWin.down('[name=ElectricTariffBlock]'), isElectricTarif);
                me.visibleContainer(editWin.down('[name=IsElectricStoveExists]'), isElectricTarif);
                me.visibleContainer(editWin.down('[name=ConsumerByElectricEnergyType]'), isElectricTarif);

                editWin.getForm().isValid();
            },
            onZoneCountChange: function (component, newValue) {
                var me = this,
                    editWin = me.getForm(),
                    tariffValue = {};
                tariffValue[2] = editWin.down('[name=TariffValue2]');
                tariffValue[3] = editWin.down('[name=TariffValue3]');

                Ext.iterate(tariffValue, function (key, field, array) {
                    var zoneValue = newValue,
                        isHidden = zoneValue < key;
                    
                    field.allowBlank = isHidden;
                    field.setDisabled(isHidden);
                    field.isValid();
                }, me);
            },
            visibleContainer: function(component, isVisible) {
                if (component && component.isAllowed) {
                    component.setVisible(isVisible);
                    component.setDisabled(!isVisible);
                }
            }
        },
        {
            xtype: 'entitychangelogaspect',
            name: 'entityChangeLogAspect',
            gridSelector: 'tariffdicteditwin entitychangeloggrid',
            entityType: 'Bars.Gkh.Gis.Entities.Dict.GisTariffDict',
            getEntityId: function () {
            }
        }
    ],

    init: function () {
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('tariffdictgrid');

        me.bindContext(view);
        me.application.deployView(view);

        view.getStore().load();
    }
});