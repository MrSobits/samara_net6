// Этот "псевдо"-аспект сделан для того чтобы не переопределять в регионе контроллер чисто из за этого аспекта

Ext.define('B4.aspects.regop.PersAccImportAspect', {
    extend: 'B4.aspects.GkhButtonImportAspect',

    alias: 'widget.persaccimportaspect',

    requires: [
        'B4.view.regop.personal_account.PersonalAccountGrid',
        'B4.view.import.PersAccImportWindow',
        'B4.view.import.PersonalAccountMultiImportWindow'
    ],

    name: 'personalAccImportAspect',
    buttonSelector: 'paccountgrid #btnImport',
    codeImport: 'PersonalAccountImport',
    windowImportView: 'import.PersAccImportWindow',
    windowImportSelector: 'persaccimportwin',
    defaultImportId: 'Bars.Gkh.RegOperator.Imports.PersonalAccountImport',
    
    listeners: {
        aftercreatewindow: function (window, importId) {
            if (importId === this.defaultImportId) {
                window.supportMultipleImport = true;
                
                var btn = this.componentQuery('[itemId=fileImport]');
                btn.maxFileSize = Gkh.config.RegOperator.GeneralConfig.ImportPersonalAccountFileSize * Math.pow(1024, 2);
                btn.setMultiple(true, 45);
            }
        }
    },
    onSuccessSaveHandler: function (asp, response) {
        var me = this;
        
        if (asp.importId !== this.defaultImportId){
            var resp = {
                result: {
                    message: response.message,
                    title: response.title
                }
            };

            me.callSuper([asp, resp]);
            return;
        }
        
        if (response.success) {
            Ext.Msg.show({
                title: 'Импорт ЛС',
                msg: '<div style="text-align: center">Уважаемый пользователь!<br/> Начался импорт файлов лицевых счетов, для просмотра результата загрузки необходимо перейти в Логи загрузок',
                width: 300,
                buttons: Ext.Msg.OK,
                buttonText: {
                    ok: 'Перейти в Логи загрузок',
                },
                fn: function(buttonValue){
                    if (buttonValue === 'ok') {
                        me.controller.application.redirectTo('importlog');
                    }
                }
            });
        }
        else {
            Ext.Msg.alert('Ошибка', response.message || 'Произошла ошибка при запуске импорта');
        }
    }
});