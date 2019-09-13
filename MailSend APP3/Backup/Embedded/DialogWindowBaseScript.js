function MetaBuilders_DialogWindow_OpenDialog(url,name,features,height,width) {
	if ( window.MetaBuilders_DialogWindow_CurrentDialog != null ) {
		window.MetaBuilders_DialogWindow_CurrentDialog.focus();
	}
	window.MetaBuilders_DialogWindow_CurrentDialogName = name;
	if ( height && width ) {
		features = MetaBuilders_DialogWindow_CenterPopup( features, width, height );
	}
	window.MetaBuilders_DialogWindow_CurrentDialog = window.open(url, name, features);
	if ( window.MetaBuilders_DialogWindow_CurrentDialog == null ) {
		return false;
	}
	window.setTimeout(MetaBuilders_DialogWindow_CatchClose,500);
	return true;
}
function MetaBuilders_DialogWindow_OpenOnLoad(url, name, features) {
	window.OpenOrInformUser = MetaBuilders_DialogWindow_OpenOrInformUser;
	MetaBuilders_DialogWindow_AddLoadHandler( "OpenOrInformUser" );
	
}
function MetaBuilders_DialogWindow_OpenOrInformUser() {
	if ( !MetaBuilders_DialogWindow_OpenOnPostBack() ) {
		if ( navigator.userAgent.indexOf( "Firefox" ) > 0 ) {
			// firefox has a bug that stops previously blocked popups from submiting forms.
			alert( "Your browser has blocked an important dialog window from this application. Please disable your popup blocker for this site. " );
		} else {
			var informContent = "<div id='MetaBuilders_DialogWindow_InformUserDiv' style='position:absolute; top:50px; left:50px; background:ButtonFace; color:ButtonText; padding:10px; border:1px solid black;'>";
			informContent += "Your browser has blocked an important dialog window from this application. ";
			informContent += "Please disable your popup blocker for this site. ";
			informContent += "Click the launch button to open the window now.<br/><input type='submit' value='Launch' onClick='MetaBuilders_DialogWindow_OpenOnPostBack(); document.getElementById(\"MetaBuilders_DialogWindow_InformUserDiv\").style.display=\"none\";return false;'>";
			informContent += "</div>";
			document.body.innerHTML += informContent; 
		}
	}
}
function MetaBuilders_DialogWindow_CenterPopup(features, width, height) { 
	if ( ( window.screen ) && ( width != "null" ) && ( height != "null" ) ) {
		var iWidth = parseInt(width);
		var iHeight = parseInt(height);
		var ah = screen.availHeight - 30;
		var aw = screen.availWidth - 10; 
		var centeredLeft = ( aw / 2 ) - ( iWidth / 2 );
		var centeredTop = ( ah / 2 ) - ( iHeight / 2 );

		features += ",left=" + centeredLeft + ",screenX=" + centeredLeft;
		features += ",top=" + centeredTop + ",screenY=" + centeredTop;
	}
	return features; 
}
function MetaBuilders_DialogWindow_CatchClose() {
	var dialog = MetaBuilders_DialogWindow_CurrentDialog;
	if ( dialog != null ) {
		if ( dialog.closed ) {
			MetaBuilders_DialogWindow_DoDialogPostBack(null);
		} else {
			if ( dialog.screenTop < 0 ) {
				MetaBuilders_DialogWindow_ModalFocus();
			}
			window.setTimeout(MetaBuilders_DialogWindow_CatchClose,100);
		}
	}
}
function MetaBuilders_DialogWindow_ModalFocus(e) {
	var dialog = MetaBuilders_DialogWindow_CurrentDialog;
	if ( dialog != null && !dialog.closed ) {
		dialog.focus();
		if ( window.event ) {
			window.event.cancelBubble = true;
		}
		if ( e && e.stopPropagation ) {
			e.stopPropagation();
		}
		if ( typeof ( dialog.MetaBuilders_DialogWindow_ModalFocus ) != "undefined" ) {
			dialog.MetaBuilders_DialogWindow_ModalFocus(e);
		}
		return false;
	}
	return true;
}
function MetaBuilders_DialogWindow_DoDialogPostBack(result) {
	if ( window.MetaBuilders_DialogWindow_PostingBack ) {
		return;
	}
	window.MetaBuilders_DialogWindow_CurrentDialog = null;
	var theScript = '';
	var doPost = true;
	for( var i = 0; i < MetaBuilders_DialogWindows.length; i++ ) {
		var info = MetaBuilders_DialogWindows[i];
		if ( info.ID == window.MetaBuilders_DialogWindow_CurrentDialogName ) {
			theScript = info.Script;
			doPost = ( info.PostBack == "True" );
			if ( result == null ) {
				result = info.DefaultValue;
				break;
			}
		}
	}
	if ( doPost ) {
		window.MetaBuilders_DialogWindow_PostingBack = true;
		if ( !result ) {
			result = "";
		}
		var safeResult = result.replace(/'/g, "\\'");
		var postBackScript = theScript.replace( '@dialogResult@', safeResult );
		eval( postBackScript );
	}
}
function MetaBuilders_DialogWindow_Init() {
	window.MetaBuilders_DialogWindow_CurrentDialog = null;
	window.MetaBuilders_DialogWindow_PostingBack = false;
	if ( typeof( window.addEventListener ) != "undefined" ) {
		window.addEventListener("focus",MetaBuilders_DialogWindow_ModalFocus,true);
		document.addEventListener("mouseover",MetaBuilders_DialogWindow_ModalFocus,true);
	} else if ( typeof( window.attachEvent ) != "undefined" ) {
		window.attachEvent("onfocus",MetaBuilders_DialogWindow_ModalFocus);
		document.attachEvent("onmouseover",MetaBuilders_DialogWindow_ModalFocus);
	} else {
		window.onfocus = MetaBuilders_DialogWindow_ModalFocus;
		document.onmouseover = MetaBuilders_DialogWindow_ModalFocus;
	}
}
function MetaBuilders_DialogWindow_AddLoadHandler(handlerName) { 
    var originalHandler = window.onload; 
    if ( originalHandler ) { 
      window.onload = function(e){originalHandler(e);window[handlerName](e);}; 
    } else { 
      window.onload = window[handlerName]; 
    } 
}
