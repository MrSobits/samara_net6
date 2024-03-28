Ext.define('B4.aspects.permission.specialobjectcr.BuildContractType', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.specialobjectcrbuildcontracttypeperm',

    permissions: [
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.Smr',
            applyTo: 'combobox[name=TypeContractBuild]',
            selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'Smr');
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.Device',
            applyTo: 'combobox[name=TypeContractBuild]',
            selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'Device');
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.Lift',
            applyTo: 'combobox[name=TypeContractBuild]',
            selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'Lift');
            }
        },
        {
            name: 'GkhCr.SpecialObjectCr.Register.BuildContract.Field.BuildContractType.EnergySurvey',
            applyTo: 'combobox[name=TypeContractBuild]',
            selector: 'specialobjectcrbuildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'EnergySurvey');
            }
        }
    ]
});