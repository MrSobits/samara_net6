/*
 * Данный аспект предназначен для массового выбора элементов по нажатию на какуюто кнопку на форме. Сохранение здесь пустой метод
 * поскольку обработку сохранения необходимо писать в тех контроллерах где этот аспект применяется.
 * Данный аспект вешается на нужную кнопку осуществляет масоовый отбор и затем в  навесившись на метод getdata идет получение выбранных записей
 * и пишется своя обработка и свои действия (например сохранение полученного массива в БД)
 */
Ext.define('B4.aspects.GkhButtonMultiSelectWindow', {
    extend: 'B4.aspects.GkhMultiSelectWindow',

    alias: 'widget.gkhbuttonmultiselectwindowaspect',

    buttonSelector: null,

    init: function (controller) {
        var me = this,
            actions = {};

        me.callParent(arguments);

        actions[me.buttonSelector] = { 'click': { fn: me.btnAction, scope: me } };
        
        controller.control(actions);
    },

    btnAction: function () {
        var me = this,
            grid = me.getSelectedGrid();

        me.getForm().show();
        
        if (grid) {
            grid.getStore().removeAll();
        }
        
        me.updateSelectGrid();
    }
});