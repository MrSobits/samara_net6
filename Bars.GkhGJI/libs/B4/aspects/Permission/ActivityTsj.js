Ext.define('B4.aspects.permission.ActivityTsj', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.activitytsjperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
        { name: 'GkhGji.ActivityTsj.Create', applyTo: 'b4addbutton', selector: '#activityTsjGrid' },
        { name: 'GkhGji.ActivityTsj.Edit', applyTo: 'b4savebutton', selector: '#activityTsjEditPanel' },
        { name: 'GkhGji.ActivityTsj.Delete', applyTo: 'b4deletecolumn', selector: '#activityTsjGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.ActivityTsj.Create', applyTo: 'b4addbutton', selector: '#activityTsjStatuteGrid' },
        { name: 'GkhGji.ActivityTsj.Edit', applyTo: 'b4savebutton', selector: '#activityTsjStatuteEditWindow' },
        { name: 'GkhGji.ActivityTsj.Delete', applyTo: 'b4deletecolumn', selector: '#activityTsjStatuteGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.ActivityTsj.Edit', applyTo: '#buttonSave', selector: '#activityTsjArticleGrid' },

        { name: 'GkhGji.ActivityTsj.Create', applyTo: 'b4addbutton', selector: '#activityTsjRealObjGrid' },
        { name: 'GkhGji.ActivityTsj.Delete', applyTo: 'b4deletecolumn', selector: '#activityTsjRealObjGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.ActivityTsj.Create', applyTo: 'b4addbutton', selector: '#activityTsjProtocolGrid' },
        { name: 'GkhGji.ActivityTsj.Edit', applyTo: 'b4savebutton', selector: '#activityTsjProtocolEditWindow' },
        { name: 'GkhGji.ActivityTsj.Delete', applyTo: 'b4deletecolumn', selector: '#activityTsjProtocolGrid',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.ActivityTsj.Create', applyTo: 'b4addbutton', selector: '#activityTsjProtocolRealObjGrid' },
        { name: 'GkhGji.ActivityTsj.Delete', applyTo: 'b4deletecolumn', selector: '#activityTsjProtocolRealObjGrid',
            applyBy: function(component, allowed) {
                if (component) {
                    if (allowed) component.show();
                    else component.hide();
                }
            }
        },

        { name: 'GkhGji.ActivityTsj.Create', applyTo: 'b4addbutton', selector: '#activityTsjInspectionGrid' }
    ]
});