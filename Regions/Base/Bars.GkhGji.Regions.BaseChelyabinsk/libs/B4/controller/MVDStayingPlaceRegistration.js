Ext.define('B4.controller.MVDStayingPlaceRegistration', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GridEditWindow',
        'B4.Ajax',
        'B4.Url'
    ],

    smevNDFL: null,
  
    models: [
        'smev.MVDStayingPlaceRegistration',
        'smev.MVDStayingPlaceRegistrationFile'
    ],
    stores: [
        'smev.MVDStayingPlaceRegistration',
        'smev.MVDStayingPlaceRegistrationFile',
        'smev.MVDStayingPlaceRegistrationAnswer'
    ],
    views: [
        'mvdstayingplacereg.Grid',
        'mvdstayingplacereg.EditWindow',
        'mvdstayingplacereg.FileInfoGrid',
        'mvdstayingplacereg.AnswerGrid'
    ],

    mainView: 'mvdstayingplacereg.Grid',
    mainViewSelector: 'mvdstayingplacereggrid',

    refs: [
        {
            ref: 'mainView',
            selector: 'mvdstayingplacereggrid'
        }
    ],

    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },

    aspects: [
        {
            xtype: 'grideditwindowaspect',
            name: 'mvdstayingplacereggridAspect',
            gridSelector: 'mvdstayingplacereggrid',
            editFormSelector: '#mvdstayingplaceregEditWindow',
            storeName: 'smev.MVDStayingPlaceRegistration',
            modelName: 'smev.MVDStayingPlaceRegistration',
            editWindowView: 'mvdstayingplacereg.EditWindow',
            onSaveSuccess: function () {
                // перекрываем чтобы окно незакрывалось после сохранения
                B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
            },            
            listeners: {
                aftersetformdata: function (asp, record, form) {                 
                    var grid = form.down('mvdstayingplaceregfileinfogrid'),
                    store = grid.getStore();
                    store.filter('smevMVD', record.getId());
                }
            }          
        }   
    ],   

    init: function () {
        this.control({
            'mvdstayingplacereggrid actioncolumn[action="openpassport"]': { click: { fn: this.sendRequest, scope: this } },
        });

        this.callParent(arguments);
    },

    sendRequest: function (grid, rowIndex, colIndex, param, param2, rec, asp) {
        var me = this;
        debugger;
        smevCertInfo = rec.getId();
        B4.Ajax.request({
            url: B4.Url.action('Execute', 'MVDStayingPlaceRegistrationExecute'),
            params: {
                taskId: smevCertInfo
            },
            timeout: 9999999
        }).next(function (response) {
            me.unmask();
            me.getStore('smev.MVDStayingPlaceRegistration').load();
            return true;
        }).error(function () {
            me.unmask();
            me.getStore('smev.MVDStayingPlaceRegistration').load();
        });
    },

    /////////////////////
    onLaunch: function () {
        var grid = this.getMainView();
        if (this.params && this.params.get('Id') > 0) {
            this.getAspect('mvdstayingplacereggridAspect').editRecord(this.params);
            this.params = null;
        }
    },
    ////////////////////

    index: function () {
        var view = this.getMainView() || Ext.widget('mvdstayingplacereggrid');
        this.bindContext(view);
        this.application.deployView(view);
        this.getStore('smev.MVDStayingPlaceRegistration').load();
    }
});