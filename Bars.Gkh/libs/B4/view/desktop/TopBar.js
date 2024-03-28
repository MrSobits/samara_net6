Ext.define('B4.view.desktop.TopBar', {
    extend: 'B4.view.desktop.Header',
    alias: 'widget.desktoptopbar',

    requires: ['Ext.tab.Bar'],

    ui: 'b4TopBar',

    initComponent: function () {
        var me = this;
        me.rightBar = Ext.create('Ext.Container', {
            layout: {
                type: 'anchor'
            },
            items: [
                {
                    itemId: 'permissionButton',
                    xtype: 'button',
                    ui: 'permission',
                    text: '<span class="icn-middle-state-permission"><span>',
                    tooltip: 'Настройка прав доступа',
                    styleHtmlContent: true,
                    styleHtmlCls: 'btn-b4',
                    width: 50,
                    padding: '8 0 0 0',
                    hidden: true
                }
            ]
        });
        me.leftBar = Ext.create('Ext.Container', {
            layout: {
                type: 'hbox'
            },
            width: 100
        });

        me.items = [me.leftBar, me.tabBar, me.rightBar];
        me.callParent(arguments);
    },

    /*
     * Добавление компоненты на правую панель кнопок
     */
    addToRightBar: function (component) {
        this.rightBar.add(component);
    },

    /*
    * Добавление компоненты на левую панель кнопок
    */
    addToLeftBar: function (component) {
        this.leftBar.add(component);
    }
});