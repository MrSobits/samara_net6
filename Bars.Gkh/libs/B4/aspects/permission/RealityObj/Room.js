Ext.define('B4.aspects.permission.realityobj.Room', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.roomperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид и панель редактирования жилого дома
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Create',
            applyTo: 'b4addbutton',
            selector: 'realobjroomgrid'
        },
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Edit',
            applyTo: 'b4savebutton',
            selector: 'roomeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Edit',
            applyTo: '[name=IsRoomHasNoNumber]',
            selector: 'roomeditwindow'
        },

        // Помещение составляет общее имущество в МКД
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Edit',
            applyTo: '[name=IsRoomCommonPropertyInMcd]',
            selector: 'roomeditwindow'
        },

        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Edit',
            applyTo: 'changevalbtn[propertyName=RoomNum]',
            selector: 'roomeditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Edit',
            applyTo: 'changevalbtn[propertyName=OwnershipType]',
            selector: 'roomeditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Edit',
            applyTo: 'changevalbtn[propertyName=Area]',
            selector: 'roomeditwindow',
            applyBy: function (component, allowed) {
                if (component) {
                    if (allowed) {
                        component.show();
                    } else {
                        component.hide();
                    }
                }
            }
        },
        {
            name: 'Gkh.RealityObject.Register.HouseInfo.Delete',
            applyTo: 'b4deletecolumn',
            selector: 'realobjroomgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        }
    ]
});