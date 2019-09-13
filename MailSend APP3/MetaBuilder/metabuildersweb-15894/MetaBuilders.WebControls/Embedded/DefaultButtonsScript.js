function DefaultButton_Init() {
	if ( !DefaultButton_BrowserCapable() ) return false;
	for( var i=0; i < MetaBuilders_WebControls_DefaultButtons.length; i++ ) {
		var item = MetaBuilders_WebControls_DefaultButtons[i];
		item.InputControl = document.getElementById( item.ID );
		item.CausesValidation = ( item.Validation == "True" );
		if ( item.InputControl != null ) {
		    var ctrl = item.InputControl;
			MetaBuilders_WebControls_DefaultButtons.Form = ctrl.form;
			ctrl.DefaultButton_PostBackScript = item.Script;
			ctrl.DefaultButton_CausesValidation = item.CausesValidation;
			ctrl.DefaultButton_ValidationGroup = item.Group;
			ctrl.DefaultButton_RegisterDefault = DefaultButton_RegisterDefault;
			ctrl.DefaultButton_UnRegisterDefault = DefaultButton_UnRegisterDefault;
			ctrl.DefaultButton_UnRegisterDefault();
			DefaultButton_AddHandler( ctrl, "focus", "DefaultButton_RegisterDefault" );
			DefaultButton_AddHandler( ctrl, "keypress", "DefaultButton_RegisterDefault" );
			DefaultButton_AddHandler( ctrl, "blur", "DefaultButton_UnRegisterDefault" );
		}
	}
}
function DefaultButton_BrowserCapable() {
	if ( typeof( document.getElementById ) == "undefined" ) {
		if( typeof( document.all ) != "undefined" ) {
			document.getElementById = function( elementId ) { return document.all[elementId]; };
		} else {
			return false;
		}
	}
	return true;
}
function DefaultButton_RegisterDefault(e) {
	this.form.DefaultButton_EnsureDefault = true;
	this.form.DefaultButton_PostBackScript = this.DefaultButton_PostBackScript;
	this.form.DefaultButton_CausesValidation = this.DefaultButton_CausesValidation;
	this.form.DefaultButton_ValidationGroup = this.DefaultButton_ValidationGroup;
}
function DefaultButton_UnRegisterDefault(e) {
	this.form.DefaultButton_EnsureDefault = false;
	this.form.DefaultButton_PostBackScript = "";
	this.form.DefaultButton_CausesValidation = false;
	this.form.DefaultButton_ValidationGroup = "";
}
function DefaultButton_RequireOwnPostback() {
    var form = MetaBuilders_WebControls_DefaultButtons.Form;
	if ( form.DefaultButton_EnsureDefault && form.DefaultButton_PostBackScript != "" ) {
		form.DefaultButton_EnsureDefault = false;
		window.setTimeout( 'DefaultButton_Postback( "' + form.DefaultButton_PostBackScript + '", ' + form.DefaultButton_CausesValidation + ', "' + form.DefaultButton_ValidationGroup + '" );', 10 );
		return true;
	} else {
		return false;
	}
}
function DefaultButton_Postback( postBackScript, causesValidation, validationGroup ) {
	if ( typeof( Page_ClientValidate ) == 'function' ) {
		if (  Page_ClientValidate( validationGroup ) ) {
			eval(postBackScript);
		}
	} else {
		eval(postBackScript);
	}
}
function DefaultButton_AddHandler(target,eventName,handlerName) { 
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