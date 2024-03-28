Ext.define('B4.controller.transitaccount.ControlTransitAccount', {
    extend: 'B4.base.Controller',

    requires: [
        'B4.Ajax',
        'B4.Url'
    ],

    views: [
        'transitaccount.Panel',
        'transitaccount.CreditGrid',
        'transitaccount.DebetGrid'
    ],

    refs: [
        {
            ref: 'mainView',
            selector: 'transitaccountpanel'
        },
        {
            ref: 'creditGrid',
            selector: 'transitaccountcreditgrid'
        },
        {
            ref: 'debetGrid',
            selector: 'transitaccountdebetgrid'
        }
    ],

    mixins: {
        mask: 'B4.mixins.MaskBody',
        context: 'B4.mixins.Context'
    },

    index: function() {
        var me = this,
            view = me.getMainView();
        
        if (!view) {
            view = Ext.widget('transitaccountpanel');
            
            me.bindContext(view);
            me.application.deployView(view);

            view.down('transitaccountcreditgrid').getStore().load();
            view.down('transitaccountdebetgrid').getStore().load();

            me.getInfo();
        }
    },
    
    init: function () {
        var me = this;

        me.control({
            'transitaccountcreditgrid b4updatebutton': { click: me.onCreditGridUpdate, scope: me },
            'transitaccountdebetgrid b4updatebutton': { click: me.onDebetGridUpdate, scope: me },
            'transitaccountcreditgrid button[action=MakeCredit]': { click: me.makeCredit, scope: me },
            'transitaccountcreditgrid button[action=Export]': { click: me.exportToTxt, scope: me },
            'transitaccountdebetgrid button[action=MakeDebet]': { click: me.makeDebet, scope: me }
        });

        this.callParent(arguments);
    },

    exportToTxt: function(btn) {
        var me = this,
            grid = btn.up('grid'),
            record = grid.getSelectionModel().getSelection()[0];
        if (record) {
            me.mask("Обработка...", me.getMainView());
            B4.Ajax.request({
                url: B4.Url.action('ExportToTxt', 'transitAccount', { recId: record.get('Id') }),
                timeout: 9999999
            }).next(function(resp) {
                me.unmask();
                var tryDecoded;
                try {
                    tryDecoded = Ext.JSON.decode(resp.responseText);
                } catch (e) {
                    tryDecoded = {};
                }
                var data = resp.data ? resp.data : tryDecoded,
                    message = resp.message ? resp.message : tryDecoded.message;
                if (data && data.Id) {
                    window.open(B4.Url.action('Download', 'FileUpload', { Id: data.Id }));
                } else {
                    me.unmask();
                    Ext.Msg.alert('Внимание', message);
                }
            }).error(function(err) {
                me.unmask();
                Ext.Msg.alert('Ошибка', err.message || err.message || err);
            });
        } else {
            Ext.Msg.alert('Ошибка', "Необходимо выбрать запись!");
        }
    },

    onCreditGridUpdate: function (btn) {
        var grid = btn.up('grid');

        grid.getStore().load();
    },
    
    onDebetGridUpdate: function (btn) {
        var grid = btn.up('grid');

        grid.getStore().load();
    },

    makeDebet: function (btn) {
        var me = this,
            grid = btn.up('grid');

        me.mask('Формирование...');
        
        B4.Ajax.request({
            url: B4.Url.action('MakeDebetList', 'TransitAccount'),
            timeout: 999999
        }).next(function (response) {
            var resp = Ext.decode(response.responseText);
            me.unmask();
            if (resp.success) {
                me.getInfo();
                grid.getStore().load();
            } else {
                Ext.Msg.alert('Ошибка!', 'Ошибка при формировании!');
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Ошибка при формировании!');
        });
    },

    makeCredit: function (btn) {
        var me = this,
            grid = btn.up('grid');

        me.mask('Формирование...');

        B4.Ajax.request({
            url: B4.Url.action('MakeCreditList', 'TransitAccount'),
            timeout: 999999
        }).next(function (response) {
            var resp = Ext.decode(response.responseText);
            me.unmask();
            if (resp.success) {
                me.getInfo();
                grid.getStore().load();
            } else {
                Ext.Msg.alert('Ошибка!', 'Ошибка при формировании!');
            }
        }).error(function () {
            me.unmask();
            Ext.Msg.alert('Ошибка!', 'Ошибка при формировании!');
        });
    },
    
    getInfo: function() {
        var me = this,
            cmpBalance = me.getDebetGrid().down('[name=UnallocatedBalance]');
        
        B4.Ajax.request({
            url: B4.Url.action('GetInfo', 'TransitAccount')
        }).next(function (response) {
            var data = Ext.JSON.decode(response.responseText);

            cmpBalance.setValue(data.unallocatedBalance);

        }).error(function () {
            
        });
    }
});