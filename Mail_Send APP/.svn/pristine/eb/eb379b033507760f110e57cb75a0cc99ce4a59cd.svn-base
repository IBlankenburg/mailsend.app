function MetaBuilders_MultiViewItem_Init() {
	if( !MetaBuilders_MultiViewItem_Init ) {
		return;
	}
	for( var i=0; i < MetaBuilders_MultiViewItems.length; i++ ) {
		var scrollerID = MetaBuilders_MultiViewItems[i];
		var scroller = document.getElementById(scrollerID);
		if ( scroller == null ) {
			continue;
		}
		scroller.XTracker = MetaBuilders_MultiViewItem_FindTracker(scrollerID + "_ScrollX");
		scroller.YTracker = MetaBuilders_MultiViewItem_FindTracker(scrollerID + "_ScrollY");
		if ( scroller.XTracker == null || scroller.YTracker == null ) {
			continue;
		}
		if ( typeof( scroller.TrackScroll ) == "undefined" ) {
			scroller.TrackScroll = MetaBuilders_MultiViewItem_TrackScroll;
			MetaBuilders_MultiViewItem_XBrowserAddHandler(scroller,"scroll","TrackScroll");
		}
	}
	window.setTimeout(MetaBuilders_MultiViewItem_Load,50);
}
function MetaBuilders_MultiViewItem_FindTracker(name) {
	if ( document.getElementById(name) != null ) {
		return document.getElementById(name);
	}
	for( var i=0; i<document.forms.length; i++ ) {
		var theForm = document.forms[i];
		if ( theForm[name] != null ) {
			return theForm[name];
		}
	}
	return null;
}
function MetaBuilders_MultiViewItem_Load() {
	for( var i=0; i<MetaBuilders_MultiViewItems.length; i++ ) {
		var scroller = document.getElementById(MetaBuilders_MultiViewItems[i]);
		if ( scroller != null ) {
			scroller.scrollTop = scroller.YTracker.value;
			scroller.scrollLeft = scroller.XTracker.value;
		}
	}
}
function MetaBuilders_MultiViewItem_TrackScroll(e) {
	if ( this.YTracker && this.XTracker ) {
		this.YTracker.value = this.scrollTop;
		this.XTracker.value = this.scrollLeft;
	}
}
function MetaBuilders_MultiViewItem_BrowserCapable(){
	return (window.onscroll && document.getElementById);
}
function MetaBuilders_MultiViewItem_XBrowserAddHandler(target,eventName,handlerName) { 
	if ( target.addEventListener ) { 
		target.addEventListener(eventName, function(e){target[handlerName](e);}, false);
	} else if ( target.attachEvent ) { 
		target.attachEvent("on" + eventName, function(e){target[handlerName](e);});
	} else { 
		var originalHandler = target["on" + eventName]; 
		if ( originalHandler ) { 
			target["on" + eventName] = function(e){originalHandler(e);target[handlerName](e);}; 
		} else { 
			target["on" + eventName] = target[handlerName]; 
		} 
	} 
}