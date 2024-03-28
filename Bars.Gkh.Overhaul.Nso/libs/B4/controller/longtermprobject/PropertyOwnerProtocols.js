Ext.define('B4.controller.longtermprobject.PropertyOwnerProtocols', {
    extend: 'B4.base.Controller',
    
    params: null,

    models: ['PropertyOwnerProtocols'],
    stores: ['PropertyOwnerProtocols'],
    views: [
        'longtermprobject.propertyownerprotocols.Grid',
        'longtermprobject.propertyownerprotocols.EditWindow'
    ],

    mainView: 'longtermprobject.propertyownerprotocols.Grid',
    mainViewSelector: '#propertyownerprotocolsGrid',
    
    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'propertyOwnerProtocolsGridWindowAspect',
            gridSelector: '#propertyownerprotocolsGrid',
            editFormSelector: '#propertyownerprotocolsEditWindow',
            storeName: 'PropertyOwnerProtocols',
            modelName: 'PropertyOwnerProtocols',
            editWindowView: 'longtermprobject.propertyownerprotocols.EditWindow',
            listeners: {
                beforesave: function (asp, obj) {
                    obj.data.LongTermPrObject = asp.controller.params.longTermObjId;
                    return true;
                }
            }
        },
        {
            xtype: 'gkhpermissionaspect',
            permissions: [
                { name: 'Ovrhl.LongTermProgramObject.PropertyOwnerProtocols.Create', applyTo: 'b4addbutton', selector: '#propertyownerprotocolsGrid' },
                { name: 'Ovrhl.LongTermProgramObject.PropertyOwnerProtocols.Edit', applyTo: 'b4savebutton', selector: '#propertyownerprotocolsEditWindow' },
                { name: 'Ovrhl.LongTermProgramObject.PropertyOwnerProtocols.Delete', applyTo: 'b4deletecolumn', selector: '#propertyownerprotocolsGrid',
                    applyBy: function (component, allowed) {
                        if (allowed) component.show();
                        else component.hide();
                    }
                }
            ]
        }
    ],

    init: function () {
        this.getStore('PropertyOwnerProtocols').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('PropertyOwnerProtocols').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params && this.params.longTermObjId) {
            operation.params.ltpObjectId = this.params.longTermObjId;
        }
    }
});