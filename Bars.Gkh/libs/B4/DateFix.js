Ext.Date.add = function (date, interval, value) {
    var d = Ext.Date.clone(date),
        Date = Ext.Date,
        day;
    if (!interval || value === 0) {
        return d;
    }

    switch (interval.toLowerCase()) {
        case Ext.Date.MILLI:
            d.setMilliseconds(d.getMilliseconds() + value);
            break;
        case Ext.Date.SECOND:
            d.setSeconds(d.getSeconds() + value);
            break;
        case Ext.Date.MINUTE:
            d.setUTCMinutes(d.getMinutes() + value);
            break;
        case Ext.Date.HOUR:
            d.setHours(d.getHours() + value);
            break;
        case Ext.Date.DAY:
            d.setDate(d.getDate() + value);
            break;
        case Ext.Date.MONTH:
            day = date.getDate();
            if (day > 28) {
                day = Math.min(day, Ext.Date.getLastDateOfMonth(Ext.Date.add(Ext.Date.getFirstDateOfMonth(date), Ext.Date.MONTH, value)).getDate());
            }
            d.setDate(day);
            d.setMonth(date.getMonth() + value);
            break;
        case Ext.Date.YEAR:
            day = date.getDate();
            if (day > 28) {
                day = Math.min(day, Ext.Date.getLastDateOfMonth(Ext.Date.add(Ext.Date.getFirstDateOfMonth(date), Ext.Date.YEAR, value)).getDate());
            }
            d.setDate(day);
            d.setFullYear(date.getFullYear() + value);
            break;
    }
    return d;
};