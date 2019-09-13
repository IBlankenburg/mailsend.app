function MetaBuilders_ExpandingPanel_Init() {
	if ( typeof( document.getElementById ) == "undefined" ) { return; }
	for( var i = 0; i < MetaBuilders_ExpandingPanels.length; i++ ) {
		var info = MetaBuilders_ExpandingPanels[i];
		ExpandingPanel_Load( info.ExpansionID, info.ContractionID, info.ExpandedContainerID, info.ContractedContainerID, info.TrackerID, info.AppPath, info.CookieLife, info.PanelID );
	}
}
function ExpandingPanel_Load( expanderId, contracterId, expanderContainerId, contracterContainerId, hiddenFieldId, cookiePath, cookieExpires, cookieKey ) {
	var expander = document.getElementById( expanderId );
	var expanderContainer = document.getElementById( expanderContainerId );

	var contracter = document.getElementById( contracterId );
	var contracterContainer = document.getElementById( contracterContainerId );
	
	var hiddenField = document.getElementById( hiddenFieldId );
	
	if ( expander == null || typeof( expander.container ) != 'undefined' ) {
		return;
	}
	
	if ( typeof( CookieJar ) != "undefined" ) {
		CookieJar.setPath( cookiePath );
	}

	expander.switchView = ExpandingPanel_Switch;
	expander.container = contracterContainer;
	expander.container.otherPanel = expanderContainer;
	expander.hiddenField = hiddenField;
	expander.cookieKey = cookieKey;
	expander.cookieExpires = cookieExpires;

	contracter.switchView = ExpandingPanel_Switch;
	contracter.container = expanderContainer;
	contracter.container.otherPanel = contracterContainer;
	contracter.hiddenField = hiddenField;
	contracter.cookieKey = cookieKey;
	contracter.cookieExpires = cookieExpires;

	expander.onclick = expander.switchView;
	contracter.onclick = contracter.switchView;
}
function ExpandingPanel_Switch() {

	this.container.otherPanel.style.display = "block";
	this.container.style.display = "none";
	if ( this.hiddenField.value == "True" ) {
		this.hiddenField.value = "False";
	} else {
		this.hiddenField.value = "True";
	}
	
	if ( typeof( CookieJar ) != "undefined" ) {
		var stateCookie = CookieJar.getCookie("ExpandingPanel");
		if (stateCookie == null) {
			stateCookie = new Cookie();
			stateCookie.name = "ExpandingPanel";
		}
		stateCookie.expires = this.cookieExpires;
		stateCookie.setValue( this.cookieKey, this.hiddenField.value );
		CookieJar.setCookie( stateCookie );
	}
	
	return false;
}