Ext.define('B4.controller.person.QualifySertificate', {
    extend: 'B4.base.Controller',

    requires: [
       
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.GkhGridEditForm',
        'B4.enums.YesNo'
    ],

    morgContract: null,
    ownerProtocol: null,

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    stores: [
        'person.QualificationCertificate',
    ],

    models: [
        'person.QualificationCertificate',
    ],

    views: [
        'person.QualificationRegistryGrid'
        //'person.QualificationEditWindow',
        //'person.QualificationDocumentGrid',
        //'person.QualificationDocumentEditWindow'
    ],

    mainView: 'person.QualificationRegistryGrid',
    mainViewSelector: 'personqualificationregistrygrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'personqualificationregistrygrid'
        }
    ],

    aspects: [
     
        //{
        //    xtype: 'grideditwindowaspect',
        //    name: 'personqualificationReestrGridWindowAspect',
        //    gridSelector: 'personqualificationgrid',
        //    editFormSelector: 'personqualificationeditwindow',
        //    modelName: 'person.QualificationCertificate',
        //    editWindowView: 'person.QualificationEditWindow',
        //   listeners: {
        //        aftersetformdata: function (asp, rec, form) {
        //            var me = this;
        //            morgContract = rec.getId();
        //            var grid = form.down('qualificationdocumentgrid'),
        //           store = grid.getStore();
        //            store.filter('qualificationId', rec.getId());
                  
        //        }
        //    }     



          
        //}      
      
        
    ],

    init: function () {
        this.getStore('person.QualificationCertificate').load();
        this.callParent(arguments);
    },

    index: function () {
        var me = this,
            view = me.getMainView() || Ext.widget('personqualificationregistrygrid');
        me.bindContext(view);
        me.application.deployView(view);
        view.getStore('person.QualificationCertificate').load();
    }
});