Ext.define('B4.controller.RosRegExtract', {
    extend: 'B4.base.Controller',
    requires: ['B4.aspects.RosRegExtractAspect'],

    mixins: { context: 'B4.mixins.Context' },

    views: ['RosRegExtract'],

    mainView: 'RosRegExtract',
    mainViewSelector: 'RosRegExtract',

    aspects: [
        {
            xtype: 'rosregextractaspect',
            viewSelector: 'RosRegExtract',
            importId: 'Bars.Gkh.Regions.Voronezh.Imports.RosRegExtractImport',
            initComponent: function () {
                var me = this;
                Ext.apply(me,
                    {
                        maxFileSize: Gkh.config.General.MaxUploadFileSize * 1048576
                    });

                me.callParent(arguments);
            }
        }
    ],
    /* aspects: [
         {
             xtype: 'gkhimportaspect',
             viewSelector: 'rosregextract',
             importId: 'Bars.Gkh.RegOperator.Imports.DebtorClaimWork.DebtorClaimWorkImport',
             initComponent: function () {
                 var me = this;
                 Ext.apply(me,
                     {
                         maxFileSize: Gkh.config.General.MaxUploadFileSize * 1048576
                     });
 
                 me.callParent(arguments);
             }
         }
     ],*/
    /*init: function () {
        var me = this;

        me.control({
            'button[action="Import"]': {
                click: me.loadButtonClick
            },
        });

        m*/
    index: function () {
        var me = this;
        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
        me.bindContext(view);
        me.application.deployView(view);
    },
    //getUserParams: function () {
    //    this.params = this.params || {};
    //},
    //loadButtonClick: function () {
    //    var me = this;

    //    me.importPanel = me.getMainComponent();

    //    var fileImport = me.importPanel.down('#fileImport');

    //    if (!fileImport.isValid()) {
    //        B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл для импорта!', 'warning');
    //        return;
    //    }

    //    if (!fileImport.isFileExtensionOK()) {
    //        B4.QuickMsg.msg('Внимание!', 'Необходимо выбрать файл с допустимым расширением: ' + fileImport.possibleFileExtensions, 'warning');
    //        return;
    //    }

    //    me.params = me.getUserParams();
    //    me.params.importId = me.importId;

    //    var formImport = me.importPanel.down('#importForm');

    //    me.mask('Загрузка данных', me.getMainComponent());

    //    formImport.submit({
    //        url: B4.Url.action('/RosRegExtract/ImportToDB'),
    //        params: me.params,
    //        success: function (form, action) {
    //            me.unmask();
    //            var message;
    //            if (!Ext.isEmpty(action.result.message)) {
    //                message = action.result.title + ' <br/>' + action.result.message;
    //            } else {
    //                message = action.result.title;
    //            }

    //            Ext.Msg.show({
    //                title: 'Успешная загрузка',
    //                msg: message,
    //                width: 300,
    //                buttons: Ext.Msg.OK,
    //                icon: Ext.window.MessageBox.INFO
    //            });

    //            var log = me.importPanel.down('#log');
    //            if (log) {
    //                log.setValue(me.createLink(action.result.logFileId));
    //            }
    //        },
    //        failure: function (form, action) {
    //            me.unmask();
    //            var message;
    //            if (!Ext.isEmpty(action.result.message)) {
    //                message = action.result.title + ' <br/>' + action.result.message;
    //            } else {
    //                message = action.result.title;
    //            }

    //            Ext.Msg.alert('Ошибка загрузки', message, function () {
    //                if (action.result.logFileId > 0) {
    //                    var log = me.importPanel.down('#log');
    //                    if (log) {
    //                        log.setValue(me.createLink(action.result.logFileId));
    //                    }
    //                }
    //            });
    //        }
    //    }, this);
    //},
});

//);/*
//Ext.define('B4.controller.RosRegExtract', {
//    extend: 'B4.base.Controller',
//    requires: ['B4.aspects.RosRegExtractAspect'],

//    mixins: { context: 'B4.mixins.Context' },

//    views: ['RosRegExtract'],

//    mainView: 'RosRegExtract',
//    mainViewSelector: 'rosregextract',

//    aspects: [
//    {
//        xtype: 'rosregextractaspect',
//        viewSelector: 'rosregestract',
//        importId: 'Bars.Gkh.RosRegExtract'
//    }],

//    index: function () {
//        var me = this;
//        var view = me.getMainView() || Ext.widget(me.mainViewSelector);
//        me.bindContext(view);
//        me.application.deployView(view);
//    }
//});*/


//Ext.define('B4.controller.RosRegExtract', {
//    extend: 'B4.base.Controller',
//    requires: ['B4.aspects.RosRegExtractAspect'],

//    mixins: {
//        context: 'B4.mixins.Context'
//    },

//    views: [
//        'RosRegExtract'
//    ],

//    mainView: 'RosRegExtract',
//    mainViewSelector: 'RosRegExtract',
//    /*
//    aspects: [
//        {
//            xtype: 'rosregextractaspect',
//            viewSelector: 'RosRegExtract',
//            importId: 'Bars.Gkh.Imports.RosRegExtractImport',
//            initComponent: function () {
//                var me = this;
//                Ext.apply(me,
//                    {
//                        maxFileSize: Gkh.config.General.MaxUploadFileSize * 1048576
//                    });

//                me.callParent(arguments);
//            }
//        }
//    ],*/

//    index: function () {
//        var me = this,
//            view = me.getMainView() || Ext.widget(me.mainViewSelector);
//        me.bindContext(view);
//        me.application.deployView(view);
//    }
//});