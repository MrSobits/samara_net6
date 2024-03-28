Ext.define('B4.aspects.permission.TypeWorkRealityObjectOutdoorEdit', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.typeworkrealityobjectoutdooreditstateperm',
    applyByPostfix: true,

    init: function () {
        var me = this;

        me.permissions = [
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Volume_Edit', applyTo: '[name=Volume]', selector: 'typeworkrealityobjectoutdooreditwindow' },    
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Sum_Edit', applyTo: '[name=Sum]', selector: 'typeworkrealityobjectoutdooreditwindow' },    
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Description_Edit', applyTo: '[name=Description]', selector: 'typeworkrealityobjectoutdooreditwindow' },    


            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.WorkRealityObjectOutdoor_View', applyTo: '[name=WorkRealityObjectOutdoor]', selector: 'typeworkrealityobjectoutdooreditwindow' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Volume_View', applyTo: '[name=Volume]', selector: 'typeworkrealityobjectoutdooreditwindow' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Sum_View', applyTo: '[name=Sum]', selector: 'typeworkrealityobjectoutdooreditwindow' },
            { name: 'GkhCr.ObjectOutdoorCr.Registries.ObjectOutdoorCrTypeWork.Fields.Description_View', applyTo: '[name=Description]', selector: 'typeworkrealityobjectoutdooreditwindow' }   
        ];

        me.callParent(arguments);
    }
});