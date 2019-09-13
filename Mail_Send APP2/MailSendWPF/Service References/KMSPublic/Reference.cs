﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.269
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MailSendWPF.KMSPublic {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", ConfigurationName="KMSPublic.KMSBackendPublicWebServicePortType")]
    public interface KMSBackendPublicWebServicePortType {
        
        // CODEGEN: Generating message contract since the operation getServerVersion is neither RPC nor document wrapped.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getServerVersion", ReplyAction="urn:getServerVersionResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        MailSendWPF.KMSPublic.getServerVersionResponse getServerVersion(MailSendWPF.KMSPublic.getServerVersionRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getX509CertificateBySerial", ReplyAction="urn:getX509CertificateBySerialResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        MailSendWPF.KMSPublic.getX509CertificateBySerialResponse getX509CertificateBySerial(MailSendWPF.KMSPublic.getX509CertificateBySerialRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:importX509Certificates", ReplyAction="urn:importX509CertificatesResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        MailSendWPF.KMSPublic.importX509CertificatesResponse importX509Certificates(MailSendWPF.KMSPublic.importX509CertificatesRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getAllX509CertificatesForEmail", ReplyAction="urn:getAllX509CertificatesForEmailResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        MailSendWPF.KMSPublic.getAllX509CertificatesForEmailResponse getAllX509CertificatesForEmail(MailSendWPF.KMSPublic.getAllX509CertificatesForEmailRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getX509CertificateForVerificationBySignedData", ReplyAction="urn:getX509CertificateForVerificationBySignedDataResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataResponse getX509CertificateForVerificationBySignedData(MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataRequest request);
        
        // CODEGEN: Parameter 'return' requires additional schema information that cannot be captured using the parameter mode. The specific attribute is 'System.Xml.Serialization.XmlElementAttribute'.
        [System.ServiceModel.OperationContractAttribute(Action="urn:getX509Certificate", ReplyAction="urn:getX509CertificateResponse")]
        [System.ServiceModel.XmlSerializerFormatAttribute(SupportFaults=true)]
        [return: System.ServiceModel.MessageParameterAttribute(Name="return")]
        MailSendWPF.KMSPublic.getX509CertificateResponse getX509Certificate(MailSendWPF.KMSPublic.getX509CertificateRequest request);
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://def.backend.ws.backend.kms.group.de/xsd")]
    public partial class KMSVersion : object, System.ComponentModel.INotifyPropertyChanged {
        
        private int buildField;
        
        private bool buildFieldSpecified;
        
        private int majorVersionField;
        
        private bool majorVersionFieldSpecified;
        
        private int minorVersionField;
        
        private bool minorVersionFieldSpecified;
        
        private string osField;
        
        private int revisionField;
        
        private bool revisionFieldSpecified;
        
        private int svnRevisionField;
        
        private bool svnRevisionFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public int build {
            get {
                return this.buildField;
            }
            set {
                this.buildField = value;
                this.RaisePropertyChanged("build");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool buildSpecified {
            get {
                return this.buildFieldSpecified;
            }
            set {
                this.buildFieldSpecified = value;
                this.RaisePropertyChanged("buildSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int majorVersion {
            get {
                return this.majorVersionField;
            }
            set {
                this.majorVersionField = value;
                this.RaisePropertyChanged("majorVersion");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool majorVersionSpecified {
            get {
                return this.majorVersionFieldSpecified;
            }
            set {
                this.majorVersionFieldSpecified = value;
                this.RaisePropertyChanged("majorVersionSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=2)]
        public int minorVersion {
            get {
                return this.minorVersionField;
            }
            set {
                this.minorVersionField = value;
                this.RaisePropertyChanged("minorVersion");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool minorVersionSpecified {
            get {
                return this.minorVersionFieldSpecified;
            }
            set {
                this.minorVersionFieldSpecified = value;
                this.RaisePropertyChanged("minorVersionSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public string os {
            get {
                return this.osField;
            }
            set {
                this.osField = value;
                this.RaisePropertyChanged("os");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=4)]
        public int revision {
            get {
                return this.revisionField;
            }
            set {
                this.revisionField = value;
                this.RaisePropertyChanged("revision");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool revisionSpecified {
            get {
                return this.revisionFieldSpecified;
            }
            set {
                this.revisionFieldSpecified = value;
                this.RaisePropertyChanged("revisionSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=5)]
        public int svnRevision {
            get {
                return this.svnRevisionField;
            }
            set {
                this.svnRevisionField = value;
                this.RaisePropertyChanged("svnRevision");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool svnRevisionSpecified {
            get {
                return this.svnRevisionFieldSpecified;
            }
            set {
                this.svnRevisionFieldSpecified = value;
                this.RaisePropertyChanged("svnRevisionSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://def.backend.ws.backend.kms.group.de/xsd")]
    public partial class ArrayOfKeys : object, System.ComponentModel.INotifyPropertyChanged {
        
        private long keysCountField;
        
        private bool keysCountFieldSpecified;
        
        private KeyResult[] listField;
        
        private ReturnStatus retField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=0)]
        public long keysCount {
            get {
                return this.keysCountField;
            }
            set {
                this.keysCountField = value;
                this.RaisePropertyChanged("keysCount");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool keysCountSpecified {
            get {
                return this.keysCountFieldSpecified;
            }
            set {
                this.keysCountFieldSpecified = value;
                this.RaisePropertyChanged("keysCountSpecified");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute("list", IsNullable=true, Order=1)]
        public KeyResult[] list {
            get {
                return this.listField;
            }
            set {
                this.listField = value;
                this.RaisePropertyChanged("list");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public ReturnStatus ret {
            get {
                return this.retField;
            }
            set {
                this.retField = value;
                this.RaisePropertyChanged("ret");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://def.backend.ws.backend.kms.group.de/xsd")]
    public partial class KeyResult : object, System.ComponentModel.INotifyPropertyChanged {
        
        private byte[] dataField;
        
        private string dataTypeField;
        
        private string keyPasswordField;
        
        private ReturnStatus retField;
        
        private string trustStatusField;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true, Order=0)]
        public byte[] data {
            get {
                return this.dataField;
            }
            set {
                this.dataField = value;
                this.RaisePropertyChanged("data");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=1)]
        public string dataType {
            get {
                return this.dataTypeField;
            }
            set {
                this.dataTypeField = value;
                this.RaisePropertyChanged("dataType");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=2)]
        public string keyPassword {
            get {
                return this.keyPasswordField;
            }
            set {
                this.keyPasswordField = value;
                this.RaisePropertyChanged("keyPassword");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=3)]
        public ReturnStatus ret {
            get {
                return this.retField;
            }
            set {
                this.retField = value;
                this.RaisePropertyChanged("ret");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=4)]
        public string trustStatus {
            get {
                return this.trustStatusField;
            }
            set {
                this.trustStatusField = value;
                this.RaisePropertyChanged("trustStatus");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    /// <remarks/>
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.Xml", "4.0.30319.233")]
    [System.SerializableAttribute()]
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.ComponentModel.DesignerCategoryAttribute("code")]
    [System.Xml.Serialization.XmlTypeAttribute(Namespace="http://def.backend.ws.backend.kms.group.de/xsd")]
    public partial class ReturnStatus : object, System.ComponentModel.INotifyPropertyChanged {
        
        private string descriptionField;
        
        private int statusField;
        
        private bool statusFieldSpecified;
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true, Order=0)]
        public string description {
            get {
                return this.descriptionField;
            }
            set {
                this.descriptionField = value;
                this.RaisePropertyChanged("description");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlElementAttribute(Order=1)]
        public int status {
            get {
                return this.statusField;
            }
            set {
                this.statusField = value;
                this.RaisePropertyChanged("status");
            }
        }
        
        /// <remarks/>
        [System.Xml.Serialization.XmlIgnoreAttribute()]
        public bool statusSpecified {
            get {
                return this.statusFieldSpecified;
            }
            set {
                this.statusFieldSpecified = value;
                this.RaisePropertyChanged("statusSpecified");
            }
        }
        
        public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;
        
        protected void RaisePropertyChanged(string propertyName) {
            System.ComponentModel.PropertyChangedEventHandler propertyChanged = this.PropertyChanged;
            if ((propertyChanged != null)) {
                propertyChanged(this, new System.ComponentModel.PropertyChangedEventArgs(propertyName));
            }
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class getServerVersionRequest {
        
        public getServerVersionRequest() {
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getServerVersionResponse", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getServerVersionResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MailSendWPF.KMSPublic.KMSVersion @return;
        
        public getServerVersionResponse() {
        }
        
        public getServerVersionResponse(MailSendWPF.KMSPublic.KMSVersion @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getX509CertificateBySerial", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getX509CertificateBySerialRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string guid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string issuerDN;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string serial;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=3)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string outputKeyType;
        
        public getX509CertificateBySerialRequest() {
        }
        
        public getX509CertificateBySerialRequest(string guid, string issuerDN, string serial, string outputKeyType) {
            this.guid = guid;
            this.issuerDN = issuerDN;
            this.serial = serial;
            this.outputKeyType = outputKeyType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getX509CertificateBySerialResponse", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getX509CertificateBySerialResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MailSendWPF.KMSPublic.KeyResult @return;
        
        public getX509CertificateBySerialResponse() {
        }
        
        public getX509CertificateBySerialResponse(MailSendWPF.KMSPublic.KeyResult @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="importX509Certificates", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class importX509CertificatesRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string guid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] certData;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string dataType;
        
        public importX509CertificatesRequest() {
        }
        
        public importX509CertificatesRequest(string guid, byte[] certData, string dataType) {
            this.guid = guid;
            this.certData = certData;
            this.dataType = dataType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="importX509CertificatesResponse", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class importX509CertificatesResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MailSendWPF.KMSPublic.KeyResult @return;
        
        public importX509CertificatesResponse() {
        }
        
        public importX509CertificatesResponse(MailSendWPF.KMSPublic.KeyResult @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllX509CertificatesForEmail", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getAllX509CertificatesForEmailRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string guid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string emailAddress;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string outputKeyType;
        
        public getAllX509CertificatesForEmailRequest() {
        }
        
        public getAllX509CertificatesForEmailRequest(string guid, string emailAddress, string outputKeyType) {
            this.guid = guid;
            this.emailAddress = emailAddress;
            this.outputKeyType = outputKeyType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getAllX509CertificatesForEmailResponse", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getAllX509CertificatesForEmailResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MailSendWPF.KMSPublic.ArrayOfKeys @return;
        
        public getAllX509CertificatesForEmailResponse() {
        }
        
        public getAllX509CertificatesForEmailResponse(MailSendWPF.KMSPublic.ArrayOfKeys @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getX509CertificateForVerificationBySignedData", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getX509CertificateForVerificationBySignedDataRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string guid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(DataType="base64Binary", IsNullable=true)]
        public byte[] signedData;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=2)]
        public bool importKeys;
        
        public getX509CertificateForVerificationBySignedDataRequest() {
        }
        
        public getX509CertificateForVerificationBySignedDataRequest(string guid, byte[] signedData, bool importKeys) {
            this.guid = guid;
            this.signedData = signedData;
            this.importKeys = importKeys;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getX509CertificateForVerificationBySignedDataResponse", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getX509CertificateForVerificationBySignedDataResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MailSendWPF.KMSPublic.KeyResult @return;
        
        public getX509CertificateForVerificationBySignedDataResponse() {
        }
        
        public getX509CertificateForVerificationBySignedDataResponse(MailSendWPF.KMSPublic.KeyResult @return) {
            this.@return = @return;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getX509Certificate", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getX509CertificateRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string guid;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=1)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string emailAddress;
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=2)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public string outputKeyType;
        
        public getX509CertificateRequest() {
        }
        
        public getX509CertificateRequest(string guid, string emailAddress, string outputKeyType) {
            this.guid = guid;
            this.emailAddress = emailAddress;
            this.outputKeyType = outputKeyType;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(WrapperName="getX509CertificateResponse", WrapperNamespace="http://def.backend.ws.backend.kms.group.de", IsWrapped=true)]
    public partial class getX509CertificateResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Namespace="http://def.backend.ws.backend.kms.group.de", Order=0)]
        [System.Xml.Serialization.XmlElementAttribute(IsNullable=true)]
        public MailSendWPF.KMSPublic.KeyResult @return;
        
        public getX509CertificateResponse() {
        }
        
        public getX509CertificateResponse(MailSendWPF.KMSPublic.KeyResult @return) {
            this.@return = @return;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface KMSBackendPublicWebServicePortTypeChannel : MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class KMSBackendPublicWebServicePortTypeClient : System.ServiceModel.ClientBase<MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType>, MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType {
        
        public KMSBackendPublicWebServicePortTypeClient() {
        }
        
        public KMSBackendPublicWebServicePortTypeClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public KMSBackendPublicWebServicePortTypeClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KMSBackendPublicWebServicePortTypeClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public KMSBackendPublicWebServicePortTypeClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MailSendWPF.KMSPublic.getServerVersionResponse MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType.getServerVersion(MailSendWPF.KMSPublic.getServerVersionRequest request) {
            return base.Channel.getServerVersion(request);
        }
        
        public MailSendWPF.KMSPublic.KMSVersion getServerVersion() {
            MailSendWPF.KMSPublic.getServerVersionRequest inValue = new MailSendWPF.KMSPublic.getServerVersionRequest();
            MailSendWPF.KMSPublic.getServerVersionResponse retVal = ((MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType)(this)).getServerVersion(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MailSendWPF.KMSPublic.getX509CertificateBySerialResponse MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType.getX509CertificateBySerial(MailSendWPF.KMSPublic.getX509CertificateBySerialRequest request) {
            return base.Channel.getX509CertificateBySerial(request);
        }
        
        public MailSendWPF.KMSPublic.KeyResult getX509CertificateBySerial(string guid, string issuerDN, string serial, string outputKeyType) {
            MailSendWPF.KMSPublic.getX509CertificateBySerialRequest inValue = new MailSendWPF.KMSPublic.getX509CertificateBySerialRequest();
            inValue.guid = guid;
            inValue.issuerDN = issuerDN;
            inValue.serial = serial;
            inValue.outputKeyType = outputKeyType;
            MailSendWPF.KMSPublic.getX509CertificateBySerialResponse retVal = ((MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType)(this)).getX509CertificateBySerial(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MailSendWPF.KMSPublic.importX509CertificatesResponse MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType.importX509Certificates(MailSendWPF.KMSPublic.importX509CertificatesRequest request) {
            return base.Channel.importX509Certificates(request);
        }
        
        public MailSendWPF.KMSPublic.KeyResult importX509Certificates(string guid, byte[] certData, string dataType) {
            MailSendWPF.KMSPublic.importX509CertificatesRequest inValue = new MailSendWPF.KMSPublic.importX509CertificatesRequest();
            inValue.guid = guid;
            inValue.certData = certData;
            inValue.dataType = dataType;
            MailSendWPF.KMSPublic.importX509CertificatesResponse retVal = ((MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType)(this)).importX509Certificates(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MailSendWPF.KMSPublic.getAllX509CertificatesForEmailResponse MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType.getAllX509CertificatesForEmail(MailSendWPF.KMSPublic.getAllX509CertificatesForEmailRequest request) {
            return base.Channel.getAllX509CertificatesForEmail(request);
        }
        
        public MailSendWPF.KMSPublic.ArrayOfKeys getAllX509CertificatesForEmail(string guid, string emailAddress, string outputKeyType) {
            MailSendWPF.KMSPublic.getAllX509CertificatesForEmailRequest inValue = new MailSendWPF.KMSPublic.getAllX509CertificatesForEmailRequest();
            inValue.guid = guid;
            inValue.emailAddress = emailAddress;
            inValue.outputKeyType = outputKeyType;
            MailSendWPF.KMSPublic.getAllX509CertificatesForEmailResponse retVal = ((MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType)(this)).getAllX509CertificatesForEmail(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataResponse MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType.getX509CertificateForVerificationBySignedData(MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataRequest request) {
            return base.Channel.getX509CertificateForVerificationBySignedData(request);
        }
        
        public MailSendWPF.KMSPublic.KeyResult getX509CertificateForVerificationBySignedData(string guid, byte[] signedData, bool importKeys) {
            MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataRequest inValue = new MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataRequest();
            inValue.guid = guid;
            inValue.signedData = signedData;
            inValue.importKeys = importKeys;
            MailSendWPF.KMSPublic.getX509CertificateForVerificationBySignedDataResponse retVal = ((MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType)(this)).getX509CertificateForVerificationBySignedData(inValue);
            return retVal.@return;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        MailSendWPF.KMSPublic.getX509CertificateResponse MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType.getX509Certificate(MailSendWPF.KMSPublic.getX509CertificateRequest request) {
            return base.Channel.getX509Certificate(request);
        }
        
        public MailSendWPF.KMSPublic.KeyResult getX509Certificate(string guid, string emailAddress, string outputKeyType) {
            MailSendWPF.KMSPublic.getX509CertificateRequest inValue = new MailSendWPF.KMSPublic.getX509CertificateRequest();
            inValue.guid = guid;
            inValue.emailAddress = emailAddress;
            inValue.outputKeyType = outputKeyType;
            MailSendWPF.KMSPublic.getX509CertificateResponse retVal = ((MailSendWPF.KMSPublic.KMSBackendPublicWebServicePortType)(this)).getX509Certificate(inValue);
            return retVal.@return;
        }
    }
}