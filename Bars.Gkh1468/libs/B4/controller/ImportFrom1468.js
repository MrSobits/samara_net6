Ext.define('B4.controller.ImportFrom1468', {
    extend: 'B4.base.Controller',
    
    views: ['importfrom1468rf.Form'],
    
    mixins: {
        context: 'B4.mixins.Context',
        mask: 'B4.mixins.MaskBody'
    },
    
    refs: [{
        ref: 'form',
        selector: 'importfrom1468form'
    }],
    
    index: function() {
        var view = this.getForm() || Ext.widget('importfrom1468form', {
            closable: true
        });

        this.bindContext(view);
        this.application.deployView(view);
    },
    
    init: function () {
        var me = this;

        me.control({
            'importfrom1468form [name="ImportFromService"]': {
                click: {
                    fn: me.onImportFromService,
                    scope: me
                }
            },
            'importfrom1468form [name="ImportFromFile"]': {
                click: {
                    fn: me.onImportFromFile,
                    scope: me
                }
            },
            'importfrom1468form [name="Municipality"]': {
                change: {
                    fn: me.enableImport,
                    scope: me
                }
            },
            'importfrom1468form [name="FileImport"]': {
                fileselected: {
                    fn: me.enableImport,
                    scope: me
                },
                fileclear: {
                    fn: me.enableImport,
                    scope: me
                }
            }
        });

        me.callParent(arguments);
    },
    
    enableImport: function (field, nv) {
        var buttonMap = {            
            'Municipality': 'ImportFromService',
            'FileImport': 'ImportFromFile'
        };

        var button = this.getForm().down(Ext.String.format('[name="{0}"]', buttonMap[field.getName()]));

        if (button) {
            if (nv && !buttonMap[nv]) {
                button.setDisabled(false);
            } else {
                button.setDisabled(true);
            }
        }
    },
    
    onImportFromService: function () {
        var me = this,
            form = me.getForm();

        if (form.getForm().isValid()) {
            var logItem = form.down("#log");
            logItem.hide();
            
            var params = {
                mo_id: form.down('[name="Municipality"]').getValue(),
                month: form.down('[name="Month"]').getValue(),
                year: form.down('[name="Year"]').getValue(),
                importId: '1468Import',
                import_type: 'ONLINE',
                not_from_file: 'true'
            };

            me.mask();
            B4.Ajax.request({
                url: B4.Url.action('/GkhImport/Import'),
                params: params,
                timeout: 9999999
            }).next(function (response) {
                me.unmask();
                var obj = Ext.JSON.decode(response.responseText);
                if (Ext.isEmpty(obj.title)) {
                    obj.title = 'Успешно!';
                }
                Ext.Msg.alert(obj.title, obj.message);

            }).error(function (response) {
                me.unmask();
                var obj = Ext.JSON.decode(response.responseText);
                Ext.Msg.alert("Ошибка!", obj.message);
            });
            
        }
    },
    
    createLogLink: function(logfileId) {
        var url = B4.Url.content(Ext.String.format('{0}/{1}?id={2}', 'FileUpload', 'Download', logfileId));
        var logRef = '<a href="' + url + '" target="_blank" style="color: #04468C !important; float: right;">Скачать лог импорта</a>';

        return logRef;
    },
    
    onImportFromFile: function(btn) {
        var me = this,
            panel = me.getForm(),
            form = panel.down('form'),
            logItem = form.down("#logArchive");

        logItem.hide();
        me.mask();
        btn.disable();
        form.getForm().submit({
            url: B4.Url.action('/GkhImport/Import'),
            params: {
                importId: '1468Import',
                import_type: 'ARCHIVE'
            },
            success: function (formPanel, action) {
                var obj = Ext.JSON.decode(action.response.responseText);
                me.unmask();
                Ext.Msg.show({
                    title: 'Успешная загрузка',
                    msg: obj.message,
                    width: 300,
                    buttons: Ext.Msg.OK,
                    icon: Ext.window.MessageBox.INFO
                });

                if (logItem && action.result.logFileId) {
                    logItem.setValue(me.createLogLink(action.logFileId));
                    logItem.show();
                }
            },
            failure: function (formPanel, action) {
                var obj = Ext.JSON.decode(action.response.responseText);
                me.unmask();
                Ext.Msg.alert('Ошибка загрузки', obj.message, function () {
                    if (logItem && action.result.logFileId) {
                        logItem.setValue(me.createLogLink(action.result.logFileId));
                        logItem.show();
                    }
                });
            }
        });
    }
});