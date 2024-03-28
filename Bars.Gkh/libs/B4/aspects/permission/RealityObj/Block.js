Ext.define('B4.aspects.permission.realityobj.Block', {
    extend: 'B4.aspects.permission.GkhStatePermissionAspect',
    alias: 'widget.realityobjblockperm',

    permissions: [
    /*
    * name - имя пермишена в дереве, 
    * applyTo - селектор контрола, к которому применяется пермишен, 
    * selector - селектор формы, на которой находится контрол
    */
    //основной грид блоков дома
        { name: 'Gkh.RealityObject.Register.Block.Create', applyTo: 'b4addbutton', selector: 'realityobjblockgrid' },
        { name: 'Gkh.RealityObject.Register.Block.Edit', applyTo: 'b4savebutton', selector: 'realityobjblockgrid' },
        {
            name: 'Gkh.RealityObject.Register.Block.Delete', applyTo: 'b4deletecolumn', selector: 'realityobjblockgrid',
            applyBy: function (component, allowed) {
                if (allowed) {
                    component.show();
                } else {
                    component.hide();
                }
            }
        },

        {
            name: 'Gkh.RealityObject.Register.Block.Fields.Number_Edit',
            applyTo: '[name=Number]',
            selector: 'blockeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.Block.Fields.Number_View',
            applyTo: '[name=Number]',
            selector: 'blockeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.RealityObject.Register.Block.Fields.AreaLiving_Edit',
            applyTo: '[name=AreaLiving]',
            selector: 'blockeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.Block.Fields.AreaLiving_View',
            applyTo: '[name=AreaLiving]',
            selector: 'blockeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.RealityObject.Register.Block.Fields.AreaTotal_Edit',
            applyTo: '[name=AreaTotal]',
            selector: 'blockeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.Block.Fields.AreaTotal_View',
            applyTo: '[name=AreaTotal]',
            selector: 'blockeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        },

        {
            name: 'Gkh.RealityObject.Register.Block.Fields.CadastralNumber_Edit',
            applyTo: '[name=CadastralNumber]',
            selector: 'blockeditwindow'
        },
        {
            name: 'Gkh.RealityObject.Register.Block.Fields.CadastralNumber_View',
            applyTo: '[name=CadastralNumber]',
            selector: 'blockeditwindow',
            applyBy: function (component, allowed) {
                if (allowed) component.show();
                else component.hide();
            }
        }
    ]
});