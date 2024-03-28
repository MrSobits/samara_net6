Ext.define('B4.controller.riskorientedmethod.ROMCategory', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.aspects.GkhButtonPrintAspect',
        'B4.aspects.GridEditWindow',
        'B4.aspects.BackForward',
        'B4.aspects.GkhGridMultiSelectWindow',
        'B4.aspects.permission.ROMCategory',
        'B4.aspects.GkhGridEditForm',
        'B4.aspects.ButtonDataExport',
         'B4.aspects.permission.ROMCategoryState',
        'B4.aspects.StateContextMenu',
         'B4.Ajax', 'B4.Url',
         'B4.aspects.ButtonDataExport',
    ],

    rOMCategory: null,
    isChanges: false,
    kknd: null,

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    stores: [
        'riskorientedmethod.ROMCategory',
        'riskorientedmethod.VpResolution',
        'riskorientedmethod.VnResolution',
        'riskorientedmethod.VprResolution',
        'riskorientedmethod.VprPrescription',
        'riskorientedmethod.ROMCategoryMKD'
    ],

    models: [
        'riskorientedmethod.ROMCategory'
    ],

    views: [
        'riskorientedmethod.ROMCategoryGrid',
        'riskorientedmethod.ROMCategoryEditWindow',
        'riskorientedmethod.ROMCategoryMKDGrid',
        'riskorientedmethod.VpResolutionGrid',
        'riskorientedmethod.VnResolutionGrid',
        'riskorientedmethod.VprResolutionGrid',
        'riskorientedmethod.VprPrescriptionGrid',
     //   'riskorientedmethod.AdmonVoilationEditWindow',
        'riskorientedmethod.MainPanel',
        'riskorientedmethod.ROMCategoryFilterPanel'
    ],

    mainView: 'riskorientedmethod.MainPanel',
    mainViewSelector: 'riskorientedmethodMainPanel',

    refs: [
        {
            ref: 'mainView',
            selector: 'riskorientedmethodMainPanel'
        },
        {
            ref: 'romcategoryeditwindow',
            selector: 'romcategoryeditwindow'
        }
    ],

    aspects: [
         {
             /*
             * аспект прав доступа
             */
             xtype: 'romcategoryperm'
         },
            //{
            //    xtype: 'romcategorystateperm',
            //    editFormAspectName: 'romcategorystatepermAspect'
            //},
          {
              xtype: 'b4_state_contextmenu',
              name: 'rOMCategoryStateTransferAspect',
              gridSelector: 'romcategorygrid',
              stateType: 'gji_romcategory',
              menuSelector: 'rOMCategoryGridStateMenu'
          },
         //{
         //    xtype: 'gkhbuttonprintaspect',
         //    name: 'admonitionPrintAspect',
         //    buttonSelector: '#admonitioneditwindow #btnPrint',
         //    codeForm: 'AppealCitsAdmonition',
         //    getUserParams: function () {
         //        var param = { Id: appealCitsAdmonition };
         //        this.params.userParams = Ext.JSON.encode(param);
         //    }
         //},
        {
            xtype: 'b4buttondataexportaspect',
            name: 'romcategoryButtonExportAspect',
            gridSelector: 'romcategorygrid',
            buttonSelector: 'romcategorygrid #btnExport',
            controllerName: 'ROMCalcTaskManOrg',
            actionName: 'Export'
        },
         {
             xtype: 'grideditwindowaspect',
             name: 'riskorientedmethodGridWindowAspect',
             gridSelector: 'romcategorygrid',
             editFormSelector: 'romcategoryeditwindow',
             modelName: 'riskorientedmethod.ROMCategory',
             storeName: 'riskorientedmethod.ROMCategory',
             editWindowView: 'riskorientedmethod.ROMCategoryEditWindow',
             otherActions: function (actions) {
                 actions['#rOMCategoryFilterPanel #dfYearEnums'] = { 'change': { fn: this.onChangeYearEnums, scope: this } };
                 actions['#romcategoryeditwindow #dfVn'] = { 'change': { fn: this.onChangeAnyVal, scope: this } };
                 actions['#romcategoryeditwindow #dfVp'] = { 'change': { fn: this.onChangeAnyVal, scope: this } };
                 actions['#romcategoryeditwindow #dfVpr'] = { 'change': { fn: this.onChangeAnyVal, scope: this } };
                 actions['#romcategoryeditwindow #dfS'] = { 'change': { fn: this.onChangeAnyVal, scope: this } };
                 actions['#romcategoryeditwindow #dfR'] = { 'change': { fn: this.onChangeAnyVal, scope: this } };
                 actions['#rOMCategoryFilterPanel #updateGrid'] = { 'click': { fn: this.onUpdateGrid, scope: this } };
                 actions['#romcategoryeditwindow #romExecuteButton'] = { 'click': { fn: this.recalculate, scope: this } };
                 actions['#romcategoryeditwindow #sfKindKND'] = { 'change': { fn: this.onChangeType, scope: this } };
                 actions['#romcategoryeditwindow #sfContragent'] = { 'beforeload': { fn: this.onBeforeLoadContragent, scope: this } };
             },
             recalculate: function (record) {
                
                 var romCategory1 = rOMCategory;
                 var form = this.getForm();
                 var dfResult = form.down('#dfResult');
                 var dfRiskCategory = form.down('#dfRiskCategory');
                 if (isChanges == true) {
                     Ext.Msg.alert('Внимание!', 'Перед пересчетом категории необходимо сохранить запись');
                 }
                 else {
                     var result = B4.Ajax.request(B4.Url.action('Calculate', 'ROMCalculate', {
                         romCategory: rOMCategory
                     }
                     )).next(function (response) {
                         var data = Ext.decode(response.responseText);
                         dfResult.setValue(data.data.result);
                         dfRiskCategory.setValue(data.data.category);
                         Ext.Msg.alert('Внимание!', 'Расчет проведен. Запись сохранена');
                         return true;
                     }).error(function () {
                         
                     });
               
                 }
             },
             onChangeAnyVal: function () {
                 isChanges = true;
             },

             onUpdateGrid: function () {
                 var str = this.controller.getStore('riskorientedmethod.ROMCategory');
                 str.currentPage = 1;
                 str.load();
             },
             onChangeYearEnums: function (field, newValue, oldValue) {
                 if (this.controller.params) {
                     this.controller.params.yearEnum = newValue;
                 }
             },
             onSaveSuccess: function () {
                 isChanges = false;
                 // перекрываем чтобы окно незакрывалось после сохранения
                 B4.QuickMsg.msg('Сохранение', 'Данные успешно сохранены', 'success');
             },

             onBeforeLoadContragent: function (store, operation) {
                 operation = operation || {};
                 operation.params = operation.params || {};

                 operation.params.kindKND = kknd;
             },

             onChangeType: function (field, newValue) {
                 var form = this.getForm();
                 kknd = newValue;
                 form.down('#sfContragent').setValue(null);
             },
             listeners: {
                 aftersetformdata: function (asp, rec, form) {
                     var me = this;
                     isChanges = false;
                     rOMCategory = rec.getId();
                  //   me.controller.getAspect('admonitionPrintAspect').loadReportStore();
                     var grid = form.down('romcategorymkdgrid'),
                     store = grid.getStore();
                     store.filter('parentId', rec.getId());

                     var grid2 = form.down('vpresolutiongrid'),
                     store2 = grid2.getStore();
                     store2.filter('parentId', rec.getId());

                     var grid3 = form.down('vnresolutiongrid'),
                     store3 = grid3.getStore();
                     store3.filter('parentId', rec.getId());

                     var grid4 = form.down('vprresolutiongrid'),
                     store4 = grid4.getStore();
                     store4.filter('parentId', rec.getId());

                     var grid5 = form.down('vprprescriptiongrid'),
                         store5 = grid5.getStore();
                     store5.filter('parentId', rec.getId());



                 }
             }
         },

         //{
         //    xtype: 'grideditwindowaspect',
         //    name: 'admoVoilationGridWindowAspect',
         //    gridSelector: '#admonVoilationGrid',
         //    editFormSelector: '#admonVoilationEditWindow',
         //    storeName: 'appealcits.AppCitAdmonVoilation',
         //    modelName: 'appealcits.AppCitAdmonVoilation',
         //    editWindowView: 'appealcits.AdmonVoilationEditWindow',
         //    listeners: {
         //        getdata: function (asp, record) {
         //            if (!record.get('Id')) {
         //                record.set('AppealCitsAdmonition', appealCitsAdmonition);
         //            }
         //        }               
         //    }
         //},
    ],

    //index: function (operation) {
    //    debugger;
    //    var me = this,
    //        view = me.getMainView() || Ext.widget('riskorientedmethodMainPanel');
    //    me.params = {};
    //    me.params.yearEnum = new Date().getFullYear();
    //    me.bindContext(view);
    //    this.application.deployView(view);
    //    //me.getAspect('manOrgLicenseNotificationGisEditPanelAspect').setData(id);
        
    //    this.getStore('riskorientedmethod.ROMCategory').load();
    //   // this.getStore('appealcits.Admonition').filter()
    //},

    init: function () {
        var me = this,
            actions = {};
        me.params = {};
        me.params.yearEnum = new Date().getFullYear();
        this.getStore('riskorientedmethod.ROMCategory').on('beforeload', this.onBeforeLoadROMCategory, this);
        this.getStore('riskorientedmethod.ROMCategory').load();
        me.callParent(arguments);
    },

   

    onBeforeLoadROMCategory: function (store, operation) {
        operation.params.isFiltered = true;
        if (this.params) {
            operation.params.yearEnum = this.params.yearEnum;
        }
    },
});