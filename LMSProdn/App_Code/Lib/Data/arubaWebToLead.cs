using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace com.arubanetworks.Licensing.LeadSalesforce
{
 /// <summary>
 /// This class represents the base Web Lead object in Salesforce.com.
 /// </summary>
 public class WebToLead
 {
  private bool isWebToLeadUrlBuilt;
  #region Events/Event Handlers
  /// <summary>
  /// Occurs immediately following the HttpRequest.GetResponse() method being called.
  /// </summary>
  public event SubmitWebToLeadEventHandler SubmitWebToLeadEvent;
  #endregion
  #region Accessors/Properties
  private Dictionary<string, string> keyValueFields;
  /// <summary>
  /// Dictionary <string,string> containing key/value pairs of single field values
  /// </summary>
  public Dictionary<string,string> KeyValueFields
  {
   set
   {
    keyValueFields = value;
   }
   get
   {
    return keyValueFields;
   }
  }
  private Dictionary<string, List<string>> keyPicklistFields;
  /// <summary>
  /// Dictionary <string,List<string,string>> containing key/value pairs of multi-select values
  /// </summary>
  public Dictionary<string, List<string>> KeyPicklistFields
  {
   get
   {
    return keyPicklistFields;
   }
   set
   {
    keyPicklistFields = value;
   }
  }
  private string webRequestMethod;
  /// <summary>
  /// Method to submit HttpRequest to Salesforce.com. Default is POST
  /// </summary>
  public string WebRequestMethod
  {
   set
   {
    webRequestMethod = value;
   }
   get
   {
    return webRequestMethod;
   }
  }
  private string webContentType;
  /// <summary>
  /// ContentType when submitting to Salesforce.com. Default is application/x-www-form-urlencoded
  /// </summary>
  public string WebContentType
  {
   set
   {
    webContentType = value;
   }
   get
   {
    return webContentType;
   }
  }
  private string webToLeadUrl;
  /// <summary>
  /// The Web-to-lead form action URL as outputed in the Web-to-lead HTML generation wizard.
  /// </summary>
  public string WebToLeadUrl
  {
   set
   {
    webToLeadUrl = value;
   }
   get
   {
    return this.webToLeadUrl;
   }
  }
  private string webToLeadData;
  /// <summary>
  /// Form data that will be submitted to Salesforce.com. Viewable only after the BuildWebToLeadUrl() method is called.
  /// </summary>
  public string WebToLeadData
  {
   get
   {
    return this.webToLeadData;
   }
}

#region Default SFDC Lead Fields (strong typed)

private bool enableDebug;
  /// <summary>
  /// Enable debug for this request (requires debug email)
  /// </summary>
  public bool EnableDebug
  {
   set
   {
    enableDebug = value;
    this.keyValueFields["debug"] = value ? "1" : "0";
   }
   get
   {
    return enableDebug;
   }
  }
  
     private string debugEmail;
  /// <summary>
  /// If debug is enabled, the email address to send the submitted lead confirmation.
  /// </summary>
  public string DebugEmail
  {
   set
   {
    debugEmail = value;
    this.keyValueFields["debugEmail"] = value;
   }
   get
   {
    return debugEmail;
   }
  }

  private string organizationID;
  /// <summary>
  /// The Organization ID for the account to which leads should be posted
  /// </summary>
  public string OrganizationID
  {
   set
   {
    this.organizationID = value;
    this.keyValueFields["oid"] = value;
   }
   get { return this.organizationID; }
  }
  
     private string firstName;
  public virtual string FirstName
  {
   set
   {
    this.firstName = value;
    this.keyValueFields["first_name"] = value;
   }
   get { return this.firstName; }
  }
  
     private string lastName;
  public virtual string LastName
  {
   set
   {
    this.lastName = value;
    this.keyValueFields["last_name"] = value;
   }
   get { return this.lastName; }
  }
  
     private string email;
  public virtual string Email
  {
   set
   {
    this.email = value;
    this.keyValueFields["email"] = value;
   }
   get { return this.email; }
  }
 
  private string company;
  public virtual string Company
  {
   set
   {
    this.company = value;
    this.keyValueFields["company"] = value;
   }
   get { return this.company; }
  }

  private string city;
  public virtual string City
  {
   set
   {
    this.city = value;
    this.keyValueFields["city"] = value;
   }
   get { return this.city; }
  }
 
   private string country;
  public virtual string Country
  {
   set
   {
    this.country = value;
    this.keyValueFields["country"] = value;
   }
   get { return this.country; }
  }

     private string state;
     public virtual string State
     {
         set
         {
             this.state = value;
             this.KeyValueFields["state"] = value;
         }
         get { return this.state; }

     }

  private string webSite;
  /// <summary>
  /// Web site for the lead being submitted
  /// </summary>
  public virtual string WebSite
  {
   set
   {
    this.webSite = value;
    this.keyValueFields["00N70000002qZD9"] = value;
   }
   get { return this.webSite; }
  }
  
  private string leadSource;
  public virtual string LeadSource
  {
   set
   {
    this.leadSource = value;
    this.keyValueFields["lead_source"] = value;
   }
   get { return this.leadSource; }
  }

  private string partnerName;
  public virtual string PartnerName
  {
      set
      {
          this.partnerName = value;
          this.keyValueFields["00N70000002MjAM"] = value;
      }
      get { return this.partnerName; }
  }

  private string estimatedCloseDate;
  public virtual string EstimatedCloseDate
  {
      set
      {
          this.estimatedCloseDate = value;
          this.keyValueFields["00N70000002pxei"] = value;
      }
      get { return this.estimatedCloseDate; }
  }


  private string users;
  public virtual string Users
  {
      set
      {
          this.users = value;
          this.keyValueFields["00N70000002qXZc"] = value;
      }
      get { return this.users; }
  }

  private string partnerContactName;
  public virtual string PartnerContactName
  {
      set
      {
          this.partnerContactName = value;
          this.keyValueFields["00N70000002pxes"] = value;
      }
      get { return this.partnerContactName; }
  }

  private string partnerContactEmail;
     public virtual string PartnerContactEmail
  {
      set
      {
          this.partnerContactEmail = value;
          this.keyValueFields["00N70000002pxf2"] = value;
      }
      get { return this.partnerContactEmail; }
  }

  private string partnerContactPhone;
     public virtual string PartnerContactPhone
  {
      set
      {
          this.partnerContactPhone = value;
          this.keyValueFields["00N70000002pxf7"] = value;
      }
      get { return this.partnerContactPhone; }
  }

  private string partnerContactType;
  public virtual string PartnerContactType
  {
      set
      {
          this.partnerContactType = value;
          this.keyValueFields["00N700000029xwJ"] = value;
      }
      get { return this.partnerContactType ; }
  }

  private string owner;
  public virtual string Owner
  {
      set
      {
          this.owner = value;
          this.keyValueFields["Owner"] = value;
      }
      get { return this.owner; }
  }


  private string salesName;
  public virtual string SalesName
  {
      set
      {
          this.salesName = value;
          this.keyValueFields["00N70000002qZDT"] = value;
      }
      get { return this.salesName; }
  }

  private string salesEmail;
  public virtual string SalesEmail
  {
      set
      {
          this.salesEmail = value;
          this.keyValueFields["00N70000002qZDF"] = value;
      }
      get { return this.salesEmail; }
  }

  private string accountType;
  public virtual string AccountType
  {
      set
      {
          this.accountType = value;
          this.keyValueFields["00N70000002qojH"] = value;
      }
      get { return this.accountType; }
  }
 
  #endregion
  #endregion
  #region Constructors
  /// <summary>
  /// Provides properties, methods, and events that pertain to submitting a Web-to-lead request in a Salesforce.com instance.
  /// </summary>
  /// <param name="oid">Organization ID of the Salesforce.com account to which the lead submission will be sent.</param>
  public WebToLead(string oid)
  {
   // initialize default salesforce fields
      this.keyValueFields = new Dictionary<string, string>();
      this.keyPicklistFields = new Dictionary<string, List<string>>();

      //zscaler details
   this.OrganizationID = oid;
   this.DebugEmail = String.Empty;
   this.EnableDebug = false;
   this.Owner = string.Empty;
   this.LeadSource = String.Empty;

  //customer details    
   this.FirstName = String.Empty;
   this.LastName = String.Empty;
   this.Email = String.Empty;
   this.Company = String.Empty;
   this.City = String.Empty;
   this.Country = String.Empty;
   this.WebSite = String.Empty;
   this.Users = string.Empty;
   this.EstimatedCloseDate = string.Empty;
  
     //partner ie Aruba Details 
   this.PartnerName = string.Empty;
   this.PartnerContactEmail = string.Empty;
   this.PartnerContactType = string.Empty;
   this.PartnerContactName = string.Empty;
   this.PartnerContactPhone = string.Empty;

   this.salesEmail = string.Empty;
   this.salesName = string.Empty;

   // initialize other class members
   this.isWebToLeadUrlBuilt = false;
   this.WebToLeadUrl = "https://www.salesforce.com/servlet/servlet.WebToLead?encoding=UTF-8"; // default
   this.WebRequestMethod = "POST"; // default request method
   this.WebContentType = "application/x-www-form-urlencoded"; // default content type
  }
  //public WebToLead() : this(String.Empty) { }
  #endregion
  /// <summary>
  /// Generate the URL and POST Data that will be used to submit the request to salesforce.com
  /// </summary>
  public virtual void BuildWebToLeadUrl()
  {
   const string FORMAT = "&{0}={1}";
   StringBuilder query = new StringBuilder();
   // pull single select values
   foreach (string key in keyValueFields.Keys)
   {
    string val = (string)keyValueFields[key];
    val = HttpUtility.UrlEncode(val);
    string keyName = String.Format(FORMAT, key, val);
    query.Append(keyName);
   }
   // pull multiselect values
   foreach (string key in keyPicklistFields.Keys)
   {
    List<string> vals = (List<string>)keyPicklistFields[key];
    foreach (string val1 in vals)
    {
     if (!String.IsNullOrEmpty(val1))
     {
      string val = val1;
      val = HttpUtility.UrlEncode(val1);
      string keyName = String.Format(FORMAT, key, val);
      query.Append(keyName);
     }
    }
   }
   webToLeadData = query.ToString();
   isWebToLeadUrlBuilt = true;
  }
  /// <summary>
  /// Submit the Web to Lead request to Salesforce.com
  /// </summary>
  public virtual void Submit()
  {
   // make sure submit data has been generated
   if (!isWebToLeadUrlBuilt) this.BuildWebToLeadUrl();
   // verify required properties are set
   //if (String.IsNullOrEmpty(organizationID) || String.IsNullOrEmpty(webToLeadUrl))
   //{
   // throw new SalesforceException("You must specify an organization ID and a web to lead URL before you can submit a lead to Salesforce.");
   //}
   // encode data and convert to byte array to send
   UTF8Encoding encoding = new UTF8Encoding();
   byte[] byteArray = encoding.GetBytes(webToLeadData);
   // Create HttpWebRequest
   HttpWebRequest reqSF = (HttpWebRequest)WebRequest.Create(this.webToLeadUrl);
   reqSF.Method = this.webRequestMethod;
   reqSF.ContentType = this.webContentType;
   reqSF.ContentLength = webToLeadData.Length;
   // Post the data stream to Salesforce
   Stream requestStream = reqSF.GetRequestStream();
   requestStream.Write(byteArray, 0, byteArray.Length);
   requestStream.Close();
   //Get the response data stream from Salesforce
   HttpWebResponse respSF = (HttpWebResponse)reqSF.GetResponse();
   // raise the event
   if (SubmitWebToLeadEvent != null)
   {
    SubmitWebToLeadEvent(this, new SubmitWebToLeadEventArgs(respSF));
   }
   respSF.Close();
   respSF = null;
   reqSF = null;
  }
 }
 /// <summary>
 /// Represents the method that will handle a WebToLeadSubmit event that has WebToLeadSubmitEventArgs data.
 /// </summary>
 /// <param name="sender">Object</param>
 /// <param name="e">WebToLeadSubmitEventArgs</param>
 public delegate void SubmitWebToLeadEventHandler(object sender, SubmitWebToLeadEventArgs e);
 /// <summary>
 /// Provides data for the web to lead submission of the WebToLead class.
 /// </summary>
 public class SubmitWebToLeadEventArgs
 {
  private HttpWebResponse response;
  public HttpWebResponse Response
  {
   set
   {
    response = value;
   }
   get
   {
    return response;
   }
  }
  public SubmitWebToLeadEventArgs(HttpWebResponse input)
  {
   this.response = input;
  }
 }
}

