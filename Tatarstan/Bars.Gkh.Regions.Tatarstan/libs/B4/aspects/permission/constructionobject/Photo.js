Ext.define('B4.aspects.permission.constructionobject.Photo', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.constructionobjectphotopermission',

    permissions: [
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Create',
            applyTo: 'b4addbutton',
            selector: 'constructionobjectphotogrid'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Date_Edit',
            applyTo: '[name=Date]',
            selector: 'constructobjphotoeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Date_View',
            applyTo: '[name=Date]',
            selector: 'constructobjphotoeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Name_Edit',
            applyTo: '[name=Name]',
            selector: 'constructobjphotoeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Name_View',
            applyTo: '[name=Name]',
            selector: 'constructobjphotoeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Group_Edit',
            applyTo: '[name=Group]',
            selector: 'constructobjphotoeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Group_View',
            applyTo: '[name=Group]',
            selector: 'constructobjphotoeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.File_Edit',
            applyTo: '[name=File]',
            selector: 'constructobjphotoeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.File_View',
            applyTo: '[name=File]',
            selector: 'constructobjphotoeditwindow',
            applyBy: function(component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Description_Edit',
            applyTo: '[name=Description]',
            selector: 'constructobjphotoeditwindow'
        },
        {
            name: 'Gkh.EmergencyObject.Register.ConstructionObject.Register.Photos.Fields.Description_View',
            applyTo: '[name=Description]',
            selector: 'constructobjphotoeditwindow',
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