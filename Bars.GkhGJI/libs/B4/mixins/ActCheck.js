Ext.define('B4.mixins.ActCheck', {
    requires: ['B4'],

    // метод возвращает срок устранения нарушений в месяцах 
    getTimeline: function () {
        return 6;
    }
});