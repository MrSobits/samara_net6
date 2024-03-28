Ext.define('B4.aspects.permission.constructionobject.smr.Schedule', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectsmrschedulepermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.WorkName_View',
            applyTo: '[dataIndex=WorkName]',
            selector: 'constructionobjsmrschedulegrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.UnitMeasureName_View',
            applyTo: '[dataIndex=UnitMeasureName]',
            selector: 'constructionobjsmrschedulegrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateStartWork_Edit',
            applyTo: '[dataIndex=DateStartWork]',
            selector: 'constructionobjsmrschedulegrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateStartWork_View',
            applyTo: '[dataIndex=DateStartWork]',
            selector: 'constructionobjsmrschedulegrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateEndWork_Edit',
            applyTo: '[dataIndex=DateEndWork]',
            selector: 'constructionobjsmrschedulegrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Schedule.Columns.DateEndWork_View',
            applyTo: '[dataIndex=DateEndWork]',
            selector: 'constructionobjsmrschedulegrid',
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