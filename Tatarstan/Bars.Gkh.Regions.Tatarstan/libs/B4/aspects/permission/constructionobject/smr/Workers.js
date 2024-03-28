Ext.define('B4.aspects.permission.constructionobject.smr.Workers', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectsmrworkerspermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.WorkName_View',
            applyTo: '[dataIndex=WorkName]',
            selector: 'constructionobjsmrworkersgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.UnitMeasureName_View',
            applyTo: '[dataIndex=UnitMeasureName]',
            selector: 'constructionobjsmrworkersgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.Volume_View',
            applyTo: '[dataIndex=Volume]',
            selector: 'constructionobjsmrworkersgrid',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.CountWorker_Edit',
            applyTo: '[dataIndex=CountWorker]',
            selector: 'constructionobjsmrworkersgrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Smr.Workers.Columns.CountWorker_View',
            applyTo: '[dataIndex=CountWorker]',
            selector: 'constructionobjsmrworkersgrid',
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