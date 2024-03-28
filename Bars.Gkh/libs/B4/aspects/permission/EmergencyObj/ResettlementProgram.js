Ext.define('B4.aspects.permission.emergencyobj.ResettlementProgram', {
    extend: 'B4.aspects.permission.GkhPermissionAspect',
    alias: 'widget.emergencyobjresettlementprogperm',

    permissions: [
        { name: 'Gkh.EmergencyObject.Register.ResettlementProgram.Create', applyTo: 'b4addbutton', selector: '#emergencyObjResettlementProgramGrid' },
        { name: 'Gkh.EmergencyObject.Register.ResettlementProgram.Edit', applyTo: 'b4savebutton', selector: '#emergencyObjResetProgEditWindow' },
        {
            name: 'Gkh.EmergencyObject.Register.ResettlementProgram.Delete', applyTo: 'b4deletecolumn', selector: '#emergencyObjResettlementProgramGrid',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        { name: 'Gkh.EmergencyObject.Register.EmerObjResettlementProgram.Cost_Edit', applyTo: 'gkhdecimalfield[name=Cost]', selector: '#emergencyObjResetProgEditWindow' },
        { name: 'Gkh.EmergencyObject.Register.EmerObjResettlementProgram.ActualCost_Edit', applyTo: 'gkhdecimalfield[name=ActualCost]', selector: '#emergencyObjResetProgEditWindow' },

        {
            name: 'Gkh.EmergencyObject.Register.EmerObjResettlementProgram.Cost_View', applyTo: 'gkhdecimalfield[name=Cost]', selector: '#emergencyObjResetProgEditWindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.EmerObjResettlementProgram.ActualCost_View', applyTo: 'gkhdecimalfield[name=ActualCost]', selector: '#emergencyObjResetProgEditWindow',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.EmerObjResettlementProgram.Cost_View', applyTo: 'gridcolumn[dataIndex=Cost]', selector: '#emergencyObjResettlementProgramGrid',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        },
        {
            name: 'Gkh.EmergencyObject.Register.EmerObjResettlementProgram.ActualCost_View', applyTo: 'gridcolumn[dataIndex=ActualCost]', selector: '#emergencyObjResettlementProgramGrid',
            applyBy: function (component, allowed) {
                component.setVisible(allowed);
            }
        }
    ]
});