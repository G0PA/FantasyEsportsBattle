﻿//setInterval(() => {
//        var el = document; // This can be your element on which to trigger the event
//        var event = document.createEvent('HTMLEvents');
//        event.initEvent('resize', true, false);
//    el.dispatchEvent(event);
//},
//    500);


function ResizeWindow() {
    var el = document; // This can be your element on which to trigger the event
    var event = document.createEvent('HTMLEvents');
    event.initEvent('resize', true, false);
    el.dispatchEvent(event);
    console.log('resizing window');
}

