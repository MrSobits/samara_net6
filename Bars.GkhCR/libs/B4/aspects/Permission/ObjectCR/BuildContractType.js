Ext.define('B4.aspects.permission.objectcr.BuildContractType', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.buildcontracttypeperm',

    permissions: [
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.Smr',
            applyTo: '#cbbxTypeContractBuild',
            selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'Smr');
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.Device',
            applyTo: '#cbbxTypeContractBuild',
            selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'Device');
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.Lift',
            applyTo: '#cbbxTypeContractBuild',
            selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'Lift');
            }
        },
        {
            name: 'GkhCr.ObjectCr.Register.BuildContract.Field.BuildContractType.EnergySurvey',
            applyTo: '#cbbxTypeContractBuild',
            selector: 'buildcontracteditwindow',
            applyBy: function (component, allowed) {
                var arr = component.notAllowed || (component.notAllowed = []);
                (allowed ? Ext.Array.remove : Ext.Array.include)(arr, 'EnergySurvey');
            }
        }
    ]
});