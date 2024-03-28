Ext.define('B4.controller.emergencyobj.ResettlementProgram', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.aspects.permission.emergencyobj.ResettlementProgram'
    ],

    models: [
        'emergencyobj.ResettlementProgram'
    ],
    stores: [
        'emergencyobj.ResettlementProgram'
    ],

    views: [
        'emergencyobj.ResettlementProgramGrid',
        'emergencyobj.ResettlementProgramEditWindow'
    ],

    mainView: 'emergencyobj.ResettlementProgramGrid',
    mainViewSelector: '#emergencyObjResettlementProgramGrid',

    aspects: [
        {
            xtype: 'emergencyobjresettlementprogperm'
        },
        {
            xtype: 'grideditwindowaspect',
            name: 'emergencyObjResettlementProgramGridWindowAspect',
            gridSelector: '#emergencyObjResettlementProgramGrid',
            editFormSelector: '#emergencyObjResetProgEditWindow',
            storeName: 'emergencyobj.ResettlementProgram',
            modelName: 'emergencyobj.ResettlementProgram',
            editWindowView: 'emergencyobj.ResettlementProgramEditWindow',
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
        this.getStore('emergencyobj.ResettlementProgram').on('beforeload', this.onBeforeLoad, this);

        this.callParent(arguments);
    },

    onLaunch: function () {
        this.getStore('emergencyobj.ResettlementProgram').load();
    },

    onBeforeLoad: function (store, operation) {
        if (this.params)
            operation.params.emergencyObjId = this.params.getId();
    }
});