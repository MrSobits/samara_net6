Ext.define('B4.controller.realityobj.TechnicalMonitoring',
    {
        extend: 'B4.controller.MenuItemController',

        requires: [
            'B4.aspects.GridEditWindow',
            'B4.enums.YesNo.Yes',
            'B4.aspects.permission.GkhStatePermissionAspect'
        ],

        models: [
            'realityobj.TechnicalMonitoring'
        ],

        stores: [
            'realityobj.TechnicalMonitoring'
        ],

        views: [
            'realityobj.technicalmonitoring.TechnicalMonitoringGrid',
            'realityobj.technicalmonitoring.TechnicalMonitoringEditWindow'
        ],

        mixins: {
            context: 'B4.mixins.Context'
        },

        refs: [
            {
                ref: 'mainView',
                selector: 'technicalmonitoringgrid'
            }
        ],

        parentCtrlCls: 'B4.controller.realityobj.Navi',

        aspects: [
            {
                xtype: 'gkhstatepermissionaspect',
                name: 'TechnicalMonitoringPermission',
                permissions: [
                    { name: 'Gkh.RealityObject.Register.TechnicalMonitoring.Create', applyTo: 'b4addbutton', selector: 'technicalmonitoringgrid' },
                    { name: 'Gkh.RealityObject.Register.TechnicalMonitoring.Edit', applyTo: 'b4savebutton', selector: 'technicalmonitoringeditwindow' },
                    {
                        name: 'Gkh.RealityObject.Register.TechnicalMonitoring.Delete', applyTo: 'b4deletecolumn', selector: 'technicalmonitoringgrid',
                        applyBy: function (component, allowed) {
                            if (allowed) component.show();
                            else component.hide();
                        }
                    }
                ]
            },
            {
                xtype: 'grideditwindowaspect',
                name: 'TechnicalMonitoringGridWindowAspect',
                gridSelector: 'technicalmonitoringgrid',
                editFormSelector: 'technicalmonitoringeditwindow',
                modelName: 'realityobj.TechnicalMonitoring',
                editWindowView: 'realityobj.technicalmonitoring.TechnicalMonitoringEditWindow',
                otherActions: function (actions) {
                    actions['#technicalmonitoringEditWindow #sfMonitoringTypeDict'] = { 'change': { fn: this.onChangeMonitoringTypeDict, scope: this } };
                },
                onChangeMonitoringTypeDict: function (field, newValue) {

                    var form = this.getForm(),
                        nfTotalBuildingVolume = form.down("#nfTotalBuildingVolume"),
                        nfAreaMkd = form.down("#nfAreaMkd"),
                        nfAreaLivingNotLivingMkd = form.down("#nfAreaLivingNotLivingMkd"),
                        nfAreaLiving = form.down("#nfAreaLiving"),
                        nfAreaNotLiving = form.down("#nfAreaNotLiving"),
                        nfAreaNotLivingFunctional = form.down("#nfAreaNotLivingFunctional"),
                        nfFloors = form.down("#nfFloors"),
                        nfNumberApartments = form.down("#nfNumberApartments"),
                        sfWallMaterial = form.down("#sfWallMaterial"),
                        nfPhysicalWear = form.down("#nfPhysicalWear"),
                        //nfTotalWear = form.down("#nfTotalWear"),
                        sfCapitalGroup = form.down("#sfCapitalGroup"),
                        nfWearFoundation = form.down("#nfWearFoundation"),
                        nfWearWalls = form.down("#nfWearWalls"),
                        nfWearRoof = form.down("#nfWearRoof"),
                        nfWearInnerSystems = form.down("#nfWearInnerSystems"),
                        nfWearHeating = form.down("#nfWearHeating"),
                        nfWearWater = form.down("#nfWearWater"),
                        nfWearWaterCold = form.down("#nfWearWaterCold"),
                        nfWearWaterHot = form.down("#nfWearWaterHot"),
                        nfWearSewere = form.down("#nfWearSewere"),
                        nfWearElectric = form.down("#nfWearElectric"),
                        nfWearLift = form.down("#nfWearLift"),
                        nfWearGas = form.down("#nfWearGas");

                    if (newValue)
                    {
                        if (newValue.Name == 'Разрешение на ввод объекта в эксплуатацию') {
                            nfTotalBuildingVolume.show();
                            nfTotalBuildingVolume.setDisabled(false);

                            nfAreaMkd.show();
                            nfAreaMkd.setDisabled(false);

                            nfAreaLivingNotLivingMkd.show();
                            nfAreaLivingNotLivingMkd.setDisabled(false);

                            nfAreaLiving.show();
                            nfAreaLiving.setDisabled(false);

                            nfAreaNotLiving.show();
                            nfAreaNotLiving.setDisabled(false);

                            nfAreaNotLivingFunctional.show();
                            nfAreaNotLivingFunctional.setDisabled(false);

                            nfFloors.show();
                            nfFloors.setDisabled(false);

                            nfNumberApartments.show();
                            nfNumberApartments.setDisabled(false);

                            sfWallMaterial.show();
                            sfWallMaterial.setDisabled(false);

                            nfPhysicalWear.hide();
                            nfPhysicalWear.setDisabled(true);

                            //nfTotalWear.hide();
                            //nfTotalWear.setDisabled(true);

                            sfCapitalGroup.hide();
                            sfCapitalGroup.setDisabled(true);

                            nfWearFoundation.hide();
                            nfWearFoundation.setDisabled(true);

                            nfWearWalls.hide();
                            nfWearWalls.setDisabled(true);

                            nfWearRoof.hide();
                            nfWearRoof.setDisabled(true);

                            nfWearInnerSystems.hide();
                            nfWearInnerSystems.setDisabled(true);

                            nfWearHeating.hide();
                            nfWearHeating.setDisabled(true);

                            nfWearWater.hide();
                            nfWearWater.setDisabled(true);

                            nfWearWaterCold.hide();
                            nfWearWaterCold.setDisabled(true);

                            nfWearWaterHot.hide();
                            nfWearWaterHot.setDisabled(true);

                            nfWearSewere.hide();
                            nfWearSewere.setDisabled(true);

                            nfWearElectric.hide();
                            nfWearElectric.setDisabled(true);

                            nfWearLift.hide();
                            nfWearLift.setDisabled(true);

                            nfWearGas.hide();
                            nfWearGas.setDisabled(true);
                        }
                        else if (newValue.Name == 'Справка БТИ') {
                            nfPhysicalWear.show();
                            nfPhysicalWear.setDisabled(false);

                            //nfTotalWear.show();
                            //nfTotalWear.setDisabled(false);

                            sfCapitalGroup.show();
                            sfCapitalGroup.setDisabled(false);

                            nfWearFoundation.show();
                            nfWearFoundation.setDisabled(false);

                            nfWearWalls.show();
                            nfWearWalls.setDisabled(false);

                            nfWearRoof.show();
                            nfWearRoof.setDisabled(false);

                            nfWearInnerSystems.show();
                            nfWearInnerSystems.setDisabled(false);

                            nfWearHeating.show();
                            nfWearHeating.setDisabled(false);

                            nfWearWater.show();
                            nfWearWater.setDisabled(false);

                            nfWearWaterCold.show();
                            nfWearWaterCold.setDisabled(false);

                            nfWearWaterHot.show();
                            nfWearWaterHot.setDisabled(false);

                            nfWearSewere.show();
                            nfWearSewere.setDisabled(false);

                            nfWearElectric.show();
                            nfWearElectric.setDisabled(false);

                            nfWearLift.show();
                            nfWearLift.setDisabled(false);

                            nfWearGas.show();
                            nfWearGas.setDisabled(false);

                            nfTotalBuildingVolume.hide();
                            nfTotalBuildingVolume.setDisabled(true);

                            nfAreaMkd.hide();
                            nfAreaMkd.setDisabled(true);

                            nfAreaLivingNotLivingMkd.hide();
                            nfAreaLivingNotLivingMkd.setDisabled(true);

                            nfAreaLiving.hide();
                            nfAreaLiving.setDisabled(true);

                            nfAreaNotLiving.hide();
                            nfAreaNotLiving.setDisabled(true);

                            nfAreaNotLivingFunctional.hide();
                            nfAreaNotLivingFunctional.setDisabled(true);

                            nfFloors.hide();
                            nfFloors.setDisabled(true);

                            nfNumberApartments.hide();
                            nfNumberApartments.setDisabled(true);

                            sfWallMaterial.hide();
                            sfWallMaterial.setDisabled(true);
                        }
                    }
                    else
                    {
                        nfTotalBuildingVolume.hide();
                        nfTotalBuildingVolume.setDisabled(true);

                        nfAreaMkd.hide();
                        nfAreaMkd.setDisabled(true);

                        nfAreaLivingNotLivingMkd.hide();
                        nfAreaLivingNotLivingMkd.setDisabled(true);

                        nfAreaLiving.hide();
                        nfAreaLiving.setDisabled(true);

                        nfAreaNotLiving.hide();
                        nfAreaNotLiving.setDisabled(true);

                        nfAreaNotLivingFunctional.hide();
                        nfAreaNotLivingFunctional.setDisabled(true);

                        nfFloors.hide();
                        nfFloors.setDisabled(true);

                        nfNumberApartments.hide();
                        nfNumberApartments.setDisabled(true);

                        sfWallMaterial.hide();
                        sfWallMaterial.setDisabled(true);

                        nfPhysicalWear.hide();
                        nfPhysicalWear.setDisabled(true);

                        //nfTotalWear.hide();
                        //nfTotalWear.setDisabled(true);

                        sfCapitalGroup.hide();
                        sfCapitalGroup.setDisabled(true);

                        nfWearFoundation.hide();
                        nfWearFoundation.setDisabled(true);

                        nfWearWalls.hide();
                        nfWearWalls.setDisabled(true);

                        nfWearRoof.hide();
                        nfWearRoof.setDisabled(true);

                        nfWearInnerSystems.hide();
                        nfWearInnerSystems.setDisabled(true);

                        nfWearHeating.hide();
                        nfWearHeating.setDisabled(true);

                        nfWearWater.hide();
                        nfWearWater.setDisabled(true);

                        nfWearWaterCold.hide();
                        nfWearWaterCold.setDisabled(true);

                        nfWearWaterHot.hide();
                        nfWearWaterHot.setDisabled(true);

                        nfWearSewere.hide();
                        nfWearSewere.setDisabled(true);

                        nfWearElectric.hide();
                        nfWearElectric.setDisabled(true);

                        nfWearLift.hide();
                        nfWearLift.setDisabled(true);

                        nfWearGas.hide();
                        nfWearGas.setDisabled(true);
                    }
                },
                listeners: {
                    getdata: function (me, record) {
                        if (!record.data.Id) {
                            record.set('RealityObject',
                                me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId'));
                        }
                    },
                    aftersetformdata: function (me) {
                        me.controller.getAspect('TechnicalMonitoringPermission').setPermissionsByRecord({ getId: function() {
                             return me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
                        } });
                    }
                }
            }
        ],

        init: function() {
            var me = this;

            me.callParent(arguments);
        },

        index: function(id) {
            var me = this,
                view = me.getMainView() || Ext.widget('technicalmonitoringgrid'),
                store = view.getStore();

            me.bindContext(view);
            me.setContextValue(view, 'realityObjectId', id);
            me.application.deployView(view, 'reality_object_info');

            store.clearFilter(true);
            store.filter('realityObjectId', id);

            this.getAspect('TechnicalMonitoringPermission').setPermissionsByRecord({ getId: function () { return id; } });
        }
    });