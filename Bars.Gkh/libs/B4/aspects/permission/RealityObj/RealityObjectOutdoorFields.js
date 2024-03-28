Ext.define('B4.aspects.permission.realityobj.RealityObjectOutdoorFields', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjectoutdoorfieldsperm',
    applyByPostfix: true,

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'Gkh.RealityObjectOutdoor.Field.View.Municipality_View', applyTo: 'b4selectfield[name=MunicipalityFiasOktmo]', selector: 'realityobjectoutdooreditpanel'},
            { name: 'Gkh.RealityObjectOutdoor.Field.View.Name_View', applyTo: 'textfield[name=Name]', selector: 'realityobjectoutdooreditpanel'},
            { name: 'Gkh.RealityObjectOutdoor.Field.View.Code_View', applyTo: 'textfield[name=Code]', selector: 'realityobjectoutdooreditpanel'},
            { name: 'Gkh.RealityObjectOutdoor.Field.View.Area_View', applyTo: 'numberfield[name=Area]', selector: 'realityobjectoutdooreditpanel'},
            { name: 'Gkh.RealityObjectOutdoor.Field.View.AsphaltArea_View', applyTo: 'numberfield[name=AsphaltArea]', selector: 'realityobjectoutdooreditpanel'},
            { name: 'Gkh.RealityObjectOutdoor.Field.View.Description_View', applyTo: 'textfield[name=Description]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.View.RepairPlanYear_View', applyTo: 'textfield[name=RepairPlanYear]', selector: 'realityobjectoutdooreditpanel' },

            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.Municipality_Edit', applyTo: 'b4selectfield[name=MunicipalityFiasOktmo]', selector: 'realityobjectoutdooreditpanel'},
            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.Name_Edit', applyTo: 'textfield[name=Name]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.Code_Edit', applyTo: 'textfield[name=Code]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.Area_Edit', applyTo: 'numberfield[name=Area]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.AsphaltArea_Edit', applyTo: 'numberfield[name=AsphaltArea]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.Description_Edit', applyTo: 'textfield[name=Description]', selector: 'realityobjectoutdooreditpanel' },
            { name: 'Gkh.RealityObjectOutdoor.Field.Edit.RepairPlanYear_Edit', applyTo: 'textfield[name=RepairPlanYear]', selector: 'realityobjectoutdooreditpanel' }
        ];

        me.callParent(arguments);
    }
});