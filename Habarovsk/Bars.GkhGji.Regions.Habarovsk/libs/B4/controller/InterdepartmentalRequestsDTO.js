Ext.define('B4.controller.InterdepartmentalRequestsDTO', {
    extend: 'B4.base.Controller',
    requires: [
        'B4.aspects.GkhGjiDigitalSignatureGridAspect',
        'B4.aspects.GkhInlineGrid',
        'B4.enums.NameOfInterdepartmentalDepartment'
    ],

    mixins: {
        context: 'B4.mixins.Context'
    },

    models: [
        'InterdepartmentalRequestsDTO',
        'complaints.SMEVComplaintsRequest',
        'smev.SMEVNDFL',
        'smev.GASU',
        'smev.GISERP',
        'smev.ERKNM',
        'smev.MVDPassport',
        'smev.MVDLivingPlaceRegistration',
        'smev.MVDStayingPlaceRegistration',
        'smev.SMEVEGRN',
        'smevpremises.SMEVPremises',
        'smev.SMEVDISKVLIC',
        'smev.SMEVSNILS',
        'smev.SMEVExploitResolution',
        'smev.SMEVChangePremisesState',
        'smev2.SMEVValidPassport',
        'smev2.SMEVStayingPlace',
        'smev.SMEVSocialHire',
        'smevemergencyhouse.SMEVEmergencyHouse',
        'smevredevelopment.SMEVRedevelopment',
        'smevownershipproperty.SMEVOwnershipProperty',
        'smev.SMEVFNSLicRequest'
    ],
    stores: ['InterdepartmentalRequestsDTO'],

    views: ['InterdepartmentalRequestsDTO.Grid'],

    mainView: 'InterdepartmentalRequestsDTO.Grid',
    mainViewSelector: 'interdepartmentalrequestsdtoGrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'interdepartmentalrequestsdtoGrid'
        }
    ],

    init: function () {
        this.control({
            'interdepartmentalrequestsdtoGrid #dfDateStart': { change: { fn: this.onChangeDateStart, scope: this } },
            'interdepartmentalrequestsdtoGrid #dfDateEnd': { change: { fn: this.onChangeDateEnd, scope: this } },
            'interdepartmentalrequestsdtoGrid b4updatebutton': { click: { fn: this.updategrid, scope: this } },
            'interdepartmentalrequestsdtoGrid': { rowaction: { fn: this.gotoRequest, scope: this } }
        });
        var me = this;
        me.params = {};
        this.getStore('InterdepartmentalRequestsDTO').on('beforeload', this.onBeforeLoadDoc, this);
        me.callParent(arguments);
    },
    onChangeDateStart: function (field, newValue, oldValue) {
        debugger;
        if (this.params) {
            this.params.dateStart = newValue;
        }
    },
    onChangeDateEnd: function (field, newValue, oldValue) {
        debugger;
        if (this.params) {
            this.params.dateEnd = newValue;
        }
    },
    gotoRequest: function (grid, action, rec) {
        debugger;
        var me = this,
        params = {},
        portal = me.getController('PortalController');
        if (rec.get('Id')) {
            var recId = rec.get('Id');
            var typerequest = rec.get('NameOfInterdepartmentalDepartment');
            var controllername = rec.get('FrontControllerName');
            var modelname = rec.get('FrontModelName');
            var model = me.getModel(modelname);
            params = new model({ Id : rec.get('Id') });
            portal.loadController(controllername, params);
        }
        
    },
    updategrid: function (btn) {
        debugger;
        var grid = btn.up('interdepartmentalrequestsdtoGrid');
        grid.getStore().load();
    },

    index: function () {
        this.params = {};
        var view = this.getMainView() || Ext.widget('interdepartmentalrequestsdtoGrid');
        this.params.dateStart = view.down('#dfDateStart').getValue();
        this.params.dateEnd = view.down('#dfDateEnd').getValue();
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('InterdepartmentalRequestsDTO').load();
    },

    onBeforeLoadDoc: function (store, operation) {
        debugger;
        if (this.params) { 
            operation.params.dateStart = this.params.dateStart;
            operation.params.dateEnd = this.params.dateEnd;
        }
    }
});