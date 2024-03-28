Ext.define('B4.aspects.permission.constructionobject.smr.Progress', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectsmrprogresspermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.WorkName_View',
            applyTo: '[dataIndex=WorkName]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.UnitMeasureName_View',
            applyTo: '[dataIndex=UnitMeasureName]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.VolumeOfCompletion_View',
            applyTo: '[dataIndex=VolumeOfCompletion]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.PercentOfCompletion_View',
            applyTo: '[dataIndex=PercentOfCompletion]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.CostSum_View',
            applyTo: '[dataIndex=CostSum]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.Volume_View',
            applyTo: '[dataIndex=Volume]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Columns.Sum_View',
            applyTo: '[dataIndex=Sum]',
            selector: 'constructionobjsmrprogressgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.VolumeOfCompletion_Edit',
            applyTo: '[name=VolumeOfCompletion]',
            selector: 'constructionobjsmrprogresseditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.VolumeOfCompletion_View',
            applyTo: '[name=VolumeOfCompletion]',
            selector: 'constructionobjsmrprogresseditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.PercentOfCompletion_Edit',
            applyTo: '[name=PercentOfCompletion]',
            selector: 'constructionobjsmrprogresseditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.PercentOfCompletion_View',
            applyTo: '[name=PercentOfCompletion]',
            selector: 'constructionobjsmrprogresseditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.CostSum_Edit',
            applyTo: '[name=CostSum]',
            selector: 'constructionobjsmrprogresseditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Progress.Fields.CostSum_View',
            applyTo: '[name=CostSum]',
            selector: 'constructionobjsmrprogresseditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});