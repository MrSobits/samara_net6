Ext.define('B4.aspects.realityobj.RealityObjectLiftEditPanelAspect', {
    extend: 'B4.aspects.GridEditCtxWindow',

    alias: 'widget.realityobjlifteditpanelaspect',
        
    gridSelector: 'realityobjectliftgrid',
    editFormSelector: 'realityobjectliftwindow',
    modelName: 'realityobj.Lift',
    editWindowView: 'realityobj.LiftWindow',

    listeners: {
        getdata: function (asp, record) {
            var me = this;
            if (!record.data.id) {
                record.data.RealityObject = me.controller.getContextValue(me.controller.getMainComponent(), 'realityObjectId');
            }
        }
    }
});