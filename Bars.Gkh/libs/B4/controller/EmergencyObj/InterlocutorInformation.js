Ext.define('B4.controller.emergencyobj.InterlocutorInformation', {
    extend: 'B4.base.Controller',

    requires: ['B4.aspects.GridEditWindow', 'B4.aspects.permission.emergencyobj.InterlocutorInformation'],

    models: ['emergencyobj.InterlocutorInformation'],
    stores: ['emergencyobj.InterlocutorInformation'],

    views: ['emergencyobj.InterlocutorInformationGrid', 'emergencyobj.InterlocutorInformationEditWindow'],

    mainView: 'emergencyobj.InterlocutorInformationGrid',
    mainViewSelector: '#emergencyObjInterlocutorInformationGrid',


    aspects: [
        {
            xtype: 'emergencyobjinterlocutorinformation'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'emergencyObjInterlocutorInformationGridWindowAspect',
            gridSelector: '#emergencyObjInterlocutorInformationGrid',
            editFormSelector: '#emergencyObjInterlocutorInformationEditWindow',
            storeName: 'emergencyobj.InterlocutorInformation',
            modelName: 'emergencyobj.InterlocutorInformation',
            editWindowView: 'emergencyobj.InterlocutorInformationEditWindow',
            listeners: {
                getdata: function (asp, record) {
                    if (!record.data.Id) {
                        record.data.EmergencyObject = this.controller.params.getId();
                    }
                }
            }
        }
    ],

    init: function () {
        this.getStore('emergencyobj.InterlocutorInformation').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('emergencyobj.InterlocutorInformation').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.emergencyObjId = this.params.getId();
    }
});